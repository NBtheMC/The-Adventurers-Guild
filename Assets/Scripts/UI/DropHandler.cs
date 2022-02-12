using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tells gameobjects where they're allowed to drop.
/// </summary>
public class DropHandler : MonoBehaviour
{
	public List<ObjectDropPoint> dropPoints;
	public enum DropType {character, item}; // An identifier to tell this script what to match with what.

	public void Awake()
	{
		Debug.Assert(dropPoints.Contains(null) == false);
	}

	public void AddDropPoint(ObjectDropPoint newDragPoint)
	{
		dropPoints.Add(newDragPoint);
	}

	/// <summary>
	/// Checks if there is a valid drop point at the location. If there is, it locks the object in, otherwise, it rejects it.
	/// Usually called by the item being dragged.
	/// </summary>
	/// <param name="itemtoHold">The game object to reside in that location.</param>
	/// <param name="currentLocation"></param>
	public bool inDropPoint(DraggerController itemtoHold)
	{

		foreach (ObjectDropPoint dropPoint in dropPoints)
		{
			// Continue if the dropPoint already contains an object. May change as we change how this works.
			if (dropPoint.heldObject != null)
			{
				continue;
			}
			if (dropPoint.WithinBounds(Input.mousePosition))
			{
				dropPoint.heldObject = itemtoHold;
				itemtoHold.objectDropPoint.heldObject = null;
				itemtoHold.objectDropPoint = dropPoint;

				itemtoHold.gameObject.transform.parent = dropPoint.gameObject.transform.parent;

				CharacterPoolController controller = itemtoHold.transform.parent.gameObject.GetComponent<CharacterPoolController>();
				Debug.Log("CharacterPoolController" + controller);
				//controller.RefreshCharacterPool();
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Empties list of drop points
	/// </summary>
	public void ClearDropPoints()
    {
		dropPoints.Clear();
    }
}
