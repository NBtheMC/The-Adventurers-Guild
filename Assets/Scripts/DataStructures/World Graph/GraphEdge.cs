using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GraphEdge
{
    [HideInInspector] public WorldLocation source;
    public WorldLocation dest;
    public float timeToTravel;
    //public float difficulty;
}
