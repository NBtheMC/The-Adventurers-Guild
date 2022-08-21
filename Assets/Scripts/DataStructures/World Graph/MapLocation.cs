using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Location")]
public class MapLocation : ScriptableObject
{
    public string locationName;
    public List<MapEdge> connections;
    [HideInInspector] public double d = double.PositiveInfinity;
    [HideInInspector] public MapLocation pred = null;
}