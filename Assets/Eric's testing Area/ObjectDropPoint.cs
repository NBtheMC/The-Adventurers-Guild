using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to an object to have it be a dropPoint for something.
/// </summary>
public class ObjectDropPoint : MonoBehaviour
{
	public DropHandler.DropType dropType; // Make the designer figure this out. Probably me later.

	private void Awake()
	{
		
	}

	private void Update()
	{
		Debug.Log(WithinBounds(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
	}

	/// <summary>
	/// Checks if a point is within the bounds of this dropPoint.
	/// </summary>
	/// <returns></returns>
	public bool WithinBounds(Vector2 point)
	{
		bool isItIn = false;

		// Get the current position of the object.
		RectTransform currentRectangle = this.GetComponent<RectTransform>();
		Rect rect = currentRectangle.rect;
		rect.x += currentRectangle.position.x;
		rect.y += currentRectangle.position.y;


		Debug.Log($"X {point.x} between {rect.xMin} and {rect.xMax}");
		Debug.Log($"Y {point.y} between {rect.yMin} and {rect.yMax}");

		// Big if condition
		if(point.x<rect.xMax && point.x>rect.xMin && point.y<rect.yMax && point.y > rect.yMin)
		{
			isItIn = true;
		}

		return isItIn;
	}
}
