using UnityEngine;

namespace StoryletCreator
{
    public class StoryletCreatorZoomArea
    {
        // Height of editor window tab so that it is ignored
        private const float EDITOR_WINDOW_TAB_HEIGHT = 21f;
        private static Matrix4x4 previousGUIMatrix;

        public static Rect Begin(float zoomFactor, Rect screenPositionArea)
        {
            // End the group that Unity implicitly begins for every window group
            // DO NOT CALL THIS FUNCTION IN BETWEEN GUI.Begin/EndGroup OR GUILayout.Begin/EndArea
            GUI.EndGroup();

            // Create a clipped area that matches the draw area based on zoomFactor
            Rect clippedArea = screenPositionArea.ScaleSizeBy(1f / zoomFactor, screenPositionArea.TopLeft());
            // Ignore the editor window tab
            clippedArea.y += EDITOR_WINDOW_TAB_HEIGHT;
            
            // Change the GUI.matrix to scale for us
            // 1. Translate clippedArea's top left to the origin
            // 2. Do scaling around the origin
            // 3. Translate the zoomed result back to where clippedArea is supposed to be
            GUI.BeginGroup(clippedArea);
            previousGUIMatrix = GUI.matrix;
            Matrix4x4 translation = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomFactor, zoomFactor, 1f));
            GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

            return clippedArea;
        }

        public static void End()
        {
            // Reset GUI matrix and end the clip area group
            GUI.matrix = previousGUIMatrix;
            GUI.EndGroup();

            // Restart Unity's implicit group
            GUI.BeginGroup(new Rect(0f, EDITOR_WINDOW_TAB_HEIGHT, Screen.width, Screen.height));
        }
    }
}
