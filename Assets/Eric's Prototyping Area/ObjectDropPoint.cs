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
		heldObject = null;
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
		Rect rect = currentRectangle.rect;
		rect.x += currentRectangle.position.x;
		rect.y += currentRectangle.position.y;

		// Big if condition 
		if(point.x<rect.xMax && point.x>rect.xMin && point.y<rect.yMax && point.y > rect.yMin)
		{
			isItIn = true;
		}

		return isItIn;
	}
}
