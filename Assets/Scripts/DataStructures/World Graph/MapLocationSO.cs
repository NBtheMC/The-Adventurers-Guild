using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Location")]
public class MapLocationSO : ScriptableObject
{
    public string locationName;
    public List<MapEdge> connections;
    [HideInInspector] public double d = double.PositiveInfinity;
    [HideInInspector] public MapLocationSO pred = null;
}