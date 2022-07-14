using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoryletCreator : EditorWindow
{
    

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

    }



    /// <summary>
    /// Handles any relevant mouse/keyboard events for the editor.
    /// </summary>
    private void HandleEvents()
    {

    }
}
