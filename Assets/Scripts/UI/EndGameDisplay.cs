using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameDisplay : MonoBehaviour
{
	public Button exitButton; // Reference to the UI button.
	public Text titleText; // Reference to the title text.
	public Text textDescription; // Reference to the text description.

	/// <summary>
	/// Tells the button on this prefab what to do.
	/// </summary>
	/// <param name="gameManager">The Game manager with the scene switching.</param>
	public void SetExitAccess(GameManager gameManager)
	{
		exitButton.onClick.AddListener(() => gameManager.ChangeScenes("Menu")); // Tells that button to go to the menu.
	}

	public void ShowInfo(Storylet info)
	{
		titleText.text = info.name;
		textDescription.text = info.questDescription;
	}
}
