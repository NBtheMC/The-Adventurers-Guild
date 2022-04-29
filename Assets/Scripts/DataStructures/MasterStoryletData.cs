using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryletDataContainer", menuName = "Storylet Data Container", order = 2)]
public class MasterStoryletData : ScriptableObject
{
    public List<Storylet> heldStorylets;
    public List<EventNode> heldEvents;
}
