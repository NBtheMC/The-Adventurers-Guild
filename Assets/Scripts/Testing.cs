using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform adventurers;
    [SerializeField] private RelationshipManager cif;

    public void RunTest(){
        //Set Guys
        GameObject guy1 = new GameObject("guy1");
        guy1.transform.parent = adventurers;
        Adventurer spiderman = guy1.AddComponent<Adventurer>();
        GameObject guy2 = new GameObject("guy2");
        guy2.transform.parent = adventurers;
        Adventurer greenGoblin = guy2.AddComponent<Adventurer>();
        GameObject guy3 = new GameObject("guy3");
        guy3.transform.parent = adventurers;
        Adventurer docOck = guy3.AddComponent<Adventurer>();
        //Change friendship levels
        spiderman.ChangeFriendship(greenGoblin, -5);
        greenGoblin.ChangeFriendship(spiderman, -5);
        spiderman.ChangeFriendship(docOck, 5);
        docOck.ChangeFriendship(spiderman, 5);
        greenGoblin.ChangeFriendship(docOck, 5);
        docOck.ChangeFriendship(greenGoblin, 5);
        //See if levels are set correctly
        Debug.Log("Spiderman x Green Goblin friendship = " + spiderman.GetFriendship(greenGoblin));
        Debug.Log("Green Goblin x Spiderman friendship = " + greenGoblin.GetFriendship(spiderman));
        Debug.Log("Spiderman x Doc Ock friendship = " + spiderman.GetFriendship(greenGoblin));
        Debug.Log("Doc Ock x Spiderman friendship = " + docOck.GetFriendship(spiderman));
        Debug.Log("Green Goblin x Doc Ock friendship = " + greenGoblin.GetFriendship(docOck));
        Debug.Log("Doc Ock x Green Goblin friendship = " + docOck.GetFriendship(spiderman));
        //Run the relationship manager and check values again
        cif.RecalculateAllRelationships();
        Debug.Log("Recalculated relationships");
        Debug.Log("Spiderman x Green Goblin friendship = " + spiderman.GetFriendship(greenGoblin));
        Debug.Log("Green Goblin x Spiderman friendship = " + greenGoblin.GetFriendship(spiderman));
        Debug.Log("Spiderman x Doc Ock friendship = " + spiderman.GetFriendship(greenGoblin));
        Debug.Log("Doc Ock x Spiderman friendship = " + docOck.GetFriendship(spiderman));
        Debug.Log("Green Goblin x Doc Ock friendship = " + greenGoblin.GetFriendship(docOck));
        Debug.Log("Doc Ock x Green Goblin friendship = " + docOck.GetFriendship(spiderman));
    }
}
