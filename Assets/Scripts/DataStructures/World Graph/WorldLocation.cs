using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Location")]
public class WorldLocation : ScriptableObject
{
    public string locationName;
    public List<GraphEdge> connections;
    [HideInInspector] public double d = double.PositiveInfinity;
    [HideInInspector] public WorldLocation pred = null;
}