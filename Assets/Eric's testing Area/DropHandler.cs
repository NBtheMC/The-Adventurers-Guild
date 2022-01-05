using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tells gameobjects where they're allowed to drop.
/// </summary>
public class DropHandler : MonoBehaviour
{
	public List<GameObject> dropPoints;
	public enum DropType {character, item}; // An identifier to tell this script what to match with what.

	/// <summary>
	/// Checks if there is a valid drop point at the location. If there is, it locks the object in, otherwise, it rejects it.
	/// </summary>
	/// <param name="itemtoHold">The game object to reside in that location.</param>
	/// <param name="currentLocation"></param>
	public bool inDropPoint(GameObject itemtoHold)
	{
		bool secured = false;

		// A few calculations to change a mousePosition from Camera to Viewpoint to Vector 2.
		Vector3 calculatedViewPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		Vector2 mousePosition = new Vector2(calculatedViewPoint.x,calculatedViewPoint.y);

		foreach (GameObject dropPoint in dropPoints)
		{
			//First, a very percise check to see if something is residing in the correct screenspace.
			
		}



		return secured;
	}
}
