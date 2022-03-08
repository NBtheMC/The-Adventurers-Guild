using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNode
{
    public string location;
    public PartySheet party;

    public WorldNode(string location)
    {
        this.location = location;
        this.party = null;
    }
}
