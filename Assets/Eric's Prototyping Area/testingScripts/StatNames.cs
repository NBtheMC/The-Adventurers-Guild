using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Statname",menuName = "Statline Object")]
public class StatNames : ScriptableObject
{
    public List<string> stats = new List<string>();
}