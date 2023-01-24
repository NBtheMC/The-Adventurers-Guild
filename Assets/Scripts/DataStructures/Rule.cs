using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rule
{
    // Enrico: Not too sure if this needs to be a class considering it's just holding values? We'll see
    public string name;
    public List<Trigger> triggers = new List<Trigger>();
    public List<Changer> changers = new List<Changer>();
}
