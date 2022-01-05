using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public void RunTest(){
        GameObject guy1 = new GameObject("guy1");
        Adventurer spiderman = guy1.AddComponent<Adventurer>();
        GameObject guy2 = new GameObject("guy2");
        Adventurer greenGoblin = guy2.AddComponent<Adventurer>();
        GameObject guy3 = new GameObject("guy3");
        Adventurer docOck = guy3.AddComponent<Adventurer>();
        spiderman.ChangeFriendship(greenGoblin, 5);
        greenGoblin.ChangeFriendship(spiderman, 5);
        Debug.Log("Spiderman x Green Goblin friendship = " + spiderman.GetFriendship(greenGoblin));
        Debug.Log("Green Goblin x Spiderman friendship = " + greenGoblin.GetFriendship(spiderman));
    }
}
