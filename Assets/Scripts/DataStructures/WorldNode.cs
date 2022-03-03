using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNode
{
    private static int counter = 0;
    public string location;
    public int id { get; private set; }
    public PartySheet party;

    public WorldNode(string location)
    {
        this.location = location;
        this.party = null;
        this.id = System.Threading.Interlocked.Increment(ref counter);
    }

    /*~WorldNode()
    {
        System.Threading.Interlocked.Decrement(ref counter);
    }*/
}
