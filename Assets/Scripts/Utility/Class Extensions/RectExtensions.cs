using UnityEngine;

public static class RectExtensions
{
    public static Vector2 TopLeft(this Rect rect)
    {
        // Returns the position of the top left corner of the rect
        return new Vector2(rect.xMin, rect.yMin);
    }

    public static Rect ScaleSizeBy(this Rect rect, float scale)
    {
        return rect.ScaleSizeBy(scale, rect.center);
    }

    public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
    {
        Rect result = rect;

        result.x -= pivotPoint.x;
        result.y -= pivotPoint.y;

        result.xMin *= scale;
        result.xMax *= scale;
        result.yMin *= scale;
        result.yMax *= scale;

        result.x += pivotPoint.x;
        result.y += pivotPoint.y;

        return result;
    }

    public static Rect ScaleSizeBy(this Rect rect, Vector2 scale)
    {
        return rect.ScaleSizeBy(scale, rect.center);
    }

    public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint)
    {
        Rect result = rect;

        result.x -= pivotPoint.x;
        result.y -= pivotPoint.y;

        result.xMin *= scale.x;
        result.xMax *= scale.x;
        result.yMin *= scale.y;
        result.yMax *= scale.y;

        result.x += pivotPoint.x;
        result.y += pivotPoint.y;

        return result;
    }
}
