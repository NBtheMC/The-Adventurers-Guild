using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace StoryletCreator {
    public class StoryletCreator : EditorWindow
    {
        // Zooming / Panning
        private Vector2 zoomPosition = Vector2.zero;
        private Rect zoomArea;
        private float zoomFactor = 1f;



        /// <summary>
        /// Initializes the editor window
        /// </summary>
        [MenuItem("Window/Storylet Creator")]
        public static void ShowEditor()
        {
            StoryletCreator editor = EditorWindow.GetWindow<StoryletCreator>("Storylet Creator");
        }

        /// <summary>
        /// Draws elements in the visualizer window
        /// </summary>
        public void OnGUI()
        {

            zoomArea.width = position.width;
            zoomArea.height = position.height;

            HandleEvents();

            DrawZoomedArea();
            DrawUnzoomedArea();

            if (GUI.changed)
            {
                Repaint();
            }
        }



        /// <summary>
        /// Handles any relevant mouse/keyboard events for the editor.
        /// </summary>
        private void HandleEvents()
        {
            if (Event.current.Equals( Event.KeyboardEvent( KeyCode.Home.ToString() ) ))
            {
                // Home key; change zoomPosition to root node
                zoomPosition = Vector2.zero;

                // Consume the event so that users can't zoom and pan at the same time
                Event.current.Use();
            }

            if (Event.current.type == EventType.MouseDrag &&
                ((Event.current.button == 2 && Event.current.isMouse) ||
                (Event.current.button == 0 && Event.current.alt)))
            {
                // Middle mouse button / Alt + Left Click Drag;  drag to change the zoomPosition
                // i.e it's indirectly panning the screen

                zoomPosition -= Event.current.delta / zoomFactor;

                // Consume the event so that users can't zoom and pan at the same time
                Event.current.Use();
            }

            if (Event.current.type == EventType.ScrollWheel && Event.current.control)
            {
                // Ctrl + Scroll wheel; zooming in or out

                Vector2 mousePosition = ConvertScreenPositionToZoomPosition(Event.current.mousePosition);
                float oldZoomFactor = zoomFactor;

                // Allow for faster zooming when shift is held down
                float zoomMultiplier = Event.current.shift ? 4f : 1f;
                zoomFactor = Mathf.Clamp(zoomFactor - Event.current.delta.y * 0.01f * zoomMultiplier, 0.3f, 2f);

                // Move the zoomPosition as it is zooming to keep it consistent
                zoomPosition += (mousePosition - zoomPosition) -
                    (oldZoomFactor / zoomFactor) * (mousePosition - zoomPosition);

                // Consume the event so that users can't zoom and pan at the same time
                Event.current.Use();
            }
        }


    #region Zoom / Panning Funcitons

        /// <summary>
        /// Gets the zoom position in the window based on mouse's screen position.
        /// </summary>
        /// <param name="screenPosition">The mouse's screen position in the window</param>
        /// <returns>The position to zoom into in the zoomArea</returns>
        private Vector2 ConvertScreenPositionToZoomPosition(Vector2 screenPosition)
        {
            return (screenPosition - zoomArea.TopLeft()) / zoomFactor + zoomPosition;
        }

        
        private void DrawUnzoomedArea()
        {
            GUILayout.Label("Controls", EditorStyles.boldLabel);
            GUILayout.Label("Ctrl + Scroll Wheel : Zoom in & out (Hold Shift to zoom faster)");
            GUILayout.Label("Middle Mouse Button : Pan around the window");
            GUILayout.Label("Alt + Left Click : Pan around the window");
            GUILayout.Label("Home : Center back on root node");
            GUILayout.Label("");


            zoomFactor = EditorGUILayout.Slider("Zoom Factor", zoomFactor, 0.3f, 2f, GUILayout.Width(300f));
        }

        private void DrawZoomedArea()
        {
            StoryletCreatorZoomArea.Begin(zoomFactor, zoomArea);

            #region Draw zoomed area stuff
                BeginWindows();

                

                EndWindows();
            #endregion

            StoryletCreatorZoomArea.End();
        }

    #endregion
    }
}
