using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyDisplay : MonoBehaviour
{
    public Font font;
    private GameObject PartyDisplayPrefab;
    private GameObject PartyMemberPrefab;
    private Transform PrefabParent;

    private Dictionary<PartySheet, GameObject> partyList;
    private Dictionary<string, GameObject> testpartyList;

    // Start is called before the first frame update
    void Start()
    {
        PartyDisplayPrefab = Resources.Load<GameObject>("Map/PartyDisplay");
        PartyMemberPrefab = Resources.Load<GameObject>("Map/PartyMember");

        PrefabParent = this.transform.GetChild(0);
        PrefabParent.gameObject.SetActive(false);

        partyList = new Dictionary<PartySheet, GameObject>();
        testpartyList = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddParty(PartySheet partyToAdd)
    {
        GameObject go = Instantiate(PartyDisplayPrefab, PrefabParent);
        partyList.Add(partyToAdd, go);

        if(PrefabParent.childCount == 1)
            PrefabParent.gameObject.SetActive(true);

        var party = partyToAdd.Party_Members;
        for(int i = 0; i < party.Count; i++)
        {
            GameObject member = Instantiate(PartyMemberPrefab, go.transform);
            member.GetComponent<Image>().sprite = party[i].portrait;
        }

    }

    public void RemoveParty(PartySheet partyToRemove)
    {
        GameObject go;
        partyList.TryGetValue(partyToRemove, out go);

        if(go != null)
        {
            Destroy(partyList[partyToRemove]);
            partyList.Remove(partyToRemove);

            if (PrefabParent.childCount <= 1)
                PrefabParent.gameObject.SetActive(false);
        }
    }

    public void TestAddParty(string partyToAdd)
    {
        GameObject go = Instantiate(PartyDisplayPrefab, PrefabParent);
        testpartyList.Add(partyToAdd, go);

        if (PrefabParent.childCount == 1)
            PrefabParent.gameObject.SetActive(true);

    }

    public void TestRemoveParty(string partyToRemove)
    {
        Destroy(testpartyList[partyToRemove]);
        testpartyList.Remove(partyToRemove);

        if (PrefabParent.childCount <= 1)
            PrefabParent.gameObject.SetActive(false);
    }
}
