using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles all processing of social models
public class CiF : MonoBehaviour
{
    // REQUIREMENTS--------------------------------
    // Predicates: Sets of variables that will bind to characters during evaluation. eg (Friend ?x ?y)
    // Rules: Made up of predicates to make adjustments. eg (Friend ?x ?y) !(Friend ?y ?z) => (Friend ?y ?x)

    public GameObject adventurers; //parent of all adventurers in the scene. Named the same

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //loops all rules through all people 's volitions, relationships, etc, in order to
    void RecalculateRelationships(){
        //loop through all Adventurers in entire scene
        foreach (Transform adventurer in adventurers.transform){
            Adventurer a = adventurer.GetComponent<Adventurer>();
            //Determine if each rule applies by calculating predicate
            //NEW FRIENDSHIPS

            //NEW ENEMYSHIPS


        }
    }
}
