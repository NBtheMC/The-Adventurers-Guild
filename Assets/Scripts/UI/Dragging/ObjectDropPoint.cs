using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to an object to have it be a dropPoint for something.
/// </summary>
public class ObjectDropPoint : MonoBehaviour
{
	public DropHandler.DropType dropType; // Used by inspector or instantiator to set the type of drop point.
	public DraggerController heldObject;

	private void Awake()
	{
		
	}

	private void Update()
	{

	}

	/// <summary>
	/// Checks if a point is within the bounds of this dropPoint.
	/// </summary>
	/// <returns></returns>
	public bool WithinBounds(Vector2 point)
	{
		bool isItIn = false;

		// Defines the space of the area we need to check.
		RectTransform currentRectangle = this.GetComponent<RectTransform>();

		Vector3[] v = new Vector3[4];
		currentRectangle.GetWorldCorners(v);
		float x_min = v[0].x;
		float x_max = v[2].x;
		float y_min = v[0].y;
		float y_max = v[1].y;

		// Big if condition 
		if (point.x < x_max && point.x > x_min && point.y < y_max && point.y > y_min)
		{
			isItIn = true;
		}

		return isItIn;
	}
}
