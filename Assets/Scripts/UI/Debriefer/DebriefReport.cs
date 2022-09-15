using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebriefReport : MonoBehaviour
{
	[Header("External Stuff")]
	public DebriefTracker debriefTracker;
	public GameObject displayButton;
	[Header("Report Log")]
	public Text PageNumber;
	public GameObject nextPage;
	public GameObject previousPage;
	public GameObject exitButton;
	public Text mainBriefingText;
	public Text titleText;
	[Header("End of Day Report")]
	public GameObject endReport;
	public GameObject beginGame;
	public GameObject goldDisplay;
	[Header("Payment Page")]
	[SerializeField] private GameObject characterListView;
	[SerializeField] private Text goldAmount;
	[SerializeField] GameObject endRecap;
	
	private TimeSystem timeSystem;
	private CharacterSheetManager adventurerSheetManager;
	
	private GameObject charPaymentPrefab;
	
	public int day;
	private ItemDisplayManager displayManager;
	[HideInInspector]  public bool isDisplayed = false;

	private Dictionary<CharacterSheet, GameObject> hiredCharacters;
	private int gold;

	private void Awake()
	{
		hiredCharacters = new Dictionary<CharacterSheet, GameObject>();

		adventurerSheetManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
		timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();

		if (characterListView == null)
			characterListView = GameObject.Find("DebriefReport/CharPaymentPage/Characters/Viewport/Content");
		if (goldAmount == null)
			goldAmount = GameObject.Find("DebriefReport/CharPaymentPage/Gold/Amount").GetComponent<Text>();
		

		charPaymentPrefab = Resources.Load<GameObject>("PayCharacter");

		GameStartDisplay();
	}

	private void Start()
	{
		day = 0;
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);

		GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>().RosterUpdate += RemoveCharacter;
	}

	private void RemoveCharacter(object source, CharacterSheet charToRemove)
    {
		GameObject go;
		hiredCharacters.TryGetValue(charToRemove, out go);

		if(go != null)
        {
			hiredCharacters.Remove(charToRemove);
			Destroy(go);
        }
	}

	private void GameStartDisplay()
    {
		TriggerEndOfDay();
		SetGoldDisplayState(false);

		endReport.SetActive(false);
		beginGame.SetActive(true);
	}

	public void DisplayPaymentPage()
    {
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(true);

		gold = GameObject.Find("GuildManager").GetComponent<GuildManager>().Gold;
		goldAmount.text = gold.ToString();

		var characters = adventurerSheetManager.GetHiredAdventurers();
		foreach(var character in characters)
        {
            if (!hiredCharacters.ContainsKey(character))
            {
				GameObject go = Instantiate(charPaymentPrefab, characterListView.transform);
				go.GetComponent<PaymentController>().charSheet = character;
				go.transform.GetChild(0).GetComponent<Image>().sprite = character.portrait;
				go.transform.Find("DaySelector/AddDay").GetComponent<Button>().onClick.AddListener(
					delegate { go.GetComponent<PaymentController>().IncrementDay(); });
				go.transform.Find("DaySelector/SubDay").GetComponent<Button>().onClick.AddListener(
					delegate { go.GetComponent<PaymentController>().DecrementDay(); });
				hiredCharacters.Add(character, go);
			}
			GameObject g = hiredCharacters[character];
			PaymentController p = g.GetComponent<PaymentController>();
			p.days = 0;
			p.daysToPayText.text = p.days.ToString();
			int cost = character.salary * (character.daysUnpaid + 1);
			g.transform.GetChild(1).GetComponent<Text>().text = "Amount Owed: " + cost;
		}
    }
	public void UpdateGoldRemaining(int amount)
    {
		gold += amount;
		goldAmount.text = gold.ToString();
	}

	public void PayAdventurers()
    {
		if (gold > 0)
		{
			foreach (Transform characterPayment in characterListView.transform)
			{
				PaymentController payController = characterPayment.GetComponent<PaymentController>();
				if (payController.days > 0)
					adventurerSheetManager.PayAdventurer(payController.charSheet, payController.days);
				else
					adventurerSheetManager.IncrementAdventurerDebt(payController.charSheet);
			}
		}
	}

	public void ToggleDisplay()
    {
		isDisplayed = !isDisplayed;
		if(isDisplayed)
			EnableDisplay();
		else
			DisableDisplay();
	}

	public void EnableDisplay() 
	{
		transform.GetChild(0).gameObject.SetActive(true);
		transform.GetChild(1).gameObject.SetActive(false);
		isDisplayed = true;
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);
		endReport.SetActive(false);
		exitButton.SetActive(true);
		nextPage.SetActive(true);
		previousPage.SetActive(true);
		PageNumber.gameObject.SetActive(true);
		beginGame.SetActive(false);
		titleText.text = "Day " + day + " Report";
	}

	public void DisableDisplay() 
	{
		isDisplayed = false;
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);
		SetGoldDisplayState(false);
		displayButton.SetActive(true);
	}

	public void TriggerEndOfDay()
    {
		transform.GetChild(0).gameObject.SetActive(true);
		transform.GetChild(1).gameObject.SetActive(false);
		isDisplayed = true;
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);
		endReport.SetActive(true);
		exitButton.SetActive(false);
		nextPage.SetActive(false);
		previousPage.SetActive(false);
		SetGoldDisplayState(true);
		PageNumber.gameObject.SetActive(false);
		beginGame.SetActive(false);
		titleText.text = "End of Day " + timeSystem.GameTime.day;

		day += 1;
		PrintReport(day);
	}

	private void OnEnable()
	{
		day = timeSystem.GameTime.day - 1;
		if(day < 0)
			day = 0;
		PrintReport(day);
		displayButton.SetActive(false);
	}

	public void PrintReport(int d)
    {
		PageNumber.text = d + "";
		mainBriefingText.text = debriefTracker.getCompiledDayReport(d);
	}

	public void DisplayNextPage()
    {
		GameTime gameTime = debriefTracker.timeSystem.GameTime;
		if (day < gameTime.day - 1)
			day++;
		titleText.text = "Day " + day + " Report";
		PrintReport(day);
    }

	public void DisplayPrevPage()
    {
		if(day > 0)
			day--;
		titleText.text = "Day " + day + " Report";
		PrintReport(day);
    }

	public void SetGoldDisplayState(bool display)
    {
		goldDisplay.SetActive(display);
    }


}