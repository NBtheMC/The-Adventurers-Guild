using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace StoryletCreator {
    public class StoryletCreator : EditorWindow
    {
        // Zooming / Panning
        private Vector2 zoomPosition = Vector2.zero;
        private Rect zoomArea;
        private float zoomFactor = 1f;


        // Storylet
        private Storylet currentStorylet;
        private SerializedObject serializedStorylet;
        private string newStoryletAssetPath;
        private bool shouldGenerateQuest = true;

        private SerializedProperty triggerInt;
        private SerializedProperty triggerValue;
        private SerializedProperty triggerState;

        private SerializedProperty changeInt;
        private SerializedProperty changeValue;
        private SerializedProperty changeState;


        // Misc. Variables/Objects
        private Vector2 triggerChangesScrollPosition = Vector2.zero;
        private const float EMPTY_WINDOW_HEIGHT = 50f;
        private GUIStyle wordwrapStyle;


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
            #if UNITY_EDITOR

                if (wordwrapStyle == null)
                {
                    wordwrapStyle = new GUIStyle(EditorStyles.textArea);
                    wordwrapStyle.wordWrap = true;
                }

                zoomArea.width = position.width;
                zoomArea.height = position.height;

                GetStoryletLists();

                HandleEvents();

                BeginWindows();
                    DrawZoomedArea();
                    DrawUnzoomedArea();

                    // Bring settings window to top
                    GUI.BringWindowToFront(2);
                EndWindows();

                if (GUI.changed)
                {
                    Repaint();
                }

            #endif
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
                zoomFactor = Mathf.Clamp(zoomFactor - Event.current.delta.y * 0.01f * zoomMultiplier, 1f, 2f);

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
            return ((screenPosition - zoomArea.TopLeft()) / zoomFactor) + zoomPosition;
        }

    #endregion

        
    #region Draw Functions

        private void DrawUnzoomedArea()
        {
            // Draw window background
            Rect controlWindows = new Rect(0, 0, 320, position.height);
            controlWindows = GUI.Window(2, controlWindows, DrawSettingsWindow, "Storylet Creator");
        }


        private void DrawZoomedArea()
        {
            StoryletCreatorZoomArea.Begin(zoomFactor, zoomArea);

            #region Draw zoomed area stuff

                string storyletName = "";
                float dataWindowHeight = 0f, triggerWindowHeight = 0f;
                if (currentStorylet)
                {
                    storyletName = currentStorylet.questName == "" ?
                        "New Storylet" : currentStorylet.questName;

                    if (shouldGenerateQuest)
                    {
                        dataWindowHeight = 620f;
                    }
                    else
                    {
                        if (currentStorylet.endGame)
                        {
                            dataWindowHeight = 440f;
                        }
                        else
                        {
                            dataWindowHeight = 380f;
                        }
                    }

                    triggerWindowHeight = 400f;
                }
                else
                {
                    storyletName = "No storylet loaded";
                    dataWindowHeight = EMPTY_WINDOW_HEIGHT;
                    triggerWindowHeight = EMPTY_WINDOW_HEIGHT;
                }


                Rect dataWindow = new Rect(position.width / 2f - 300f - zoomPosition.x, position.height / 2f - 200f - zoomPosition.y, 400f, dataWindowHeight);
                dataWindow = GUI.Window(0, dataWindow, DataWindow, currentStorylet ? $"{storyletName} Data" : storyletName);

                Rect triggerChangesWindow = new Rect(dataWindow.x, dataWindow.y + dataWindow.height + 50f, 400f, triggerWindowHeight);
                triggerChangesWindow = GUI.Window(1, triggerChangesWindow, TriggerChangesWindow, "Triggers & Changes");

            #endregion

            StoryletCreatorZoomArea.End();
        }

    #endregion


    #region Window Functions

        private void DataWindow(int id)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (currentStorylet)
            {
                // Image
                currentStorylet.cgImage = EditorGUILayout.ObjectField("CG Image", currentStorylet.cgImage, typeof(UnityEngine.UI.Image), false) as UnityEngine.UI.Image;

                // Basic Information
                GUILayout.Label("");

                currentStorylet.questName = EditorGUILayout.TextField("Quest Name", currentStorylet.questName);
                currentStorylet.questDescription = EditorGUILayout.TextField("Quest Descriptions", currentStorylet.questDescription, wordwrapStyle, GUILayout.Height(80));
                currentStorylet.endGame = EditorGUILayout.Toggle(new GUIContent("Ends Game", "Should this quest end the game?"), currentStorylet.endGame);
                currentStorylet.canBeDuplicated = EditorGUILayout.Toggle(new GUIContent("Duplicable", "Can this storylet happen more than once?"), currentStorylet.canBeDuplicated);

                GUILayout.Label("");
                currentStorylet.comments = EditorGUILayout.TextField("Comments", currentStorylet.comments, wordwrapStyle, GUILayout.Height(80));


                // Quest generation data
                GUILayout.Label("");
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                if (currentStorylet.endGame)
                {
                    shouldGenerateQuest = false;
                    GUILayout.Label("Storylets that ends the game cannot generate a quest!\n\nQuest Description will be used as the final message.", wordwrapStyle);

                    GUILayout.Label("");
                    currentStorylet.finalImage = EditorGUILayout.ObjectField("Final Image", currentStorylet.finalImage, typeof(UnityEngine.UI.Image), false) as UnityEngine.UI.Image;
                }
                else
                {
                    shouldGenerateQuest = EditorGUILayout.Toggle(new GUIContent("Should Generate Quest", "Shows information for data pertaining to quests (this value is local to this tool and is not saved to the storylet)"), shouldGenerateQuest);
                }
                

                if (shouldGenerateQuest)
                {
                    GUILayout.Label("");
                    currentStorylet.eventHead = EditorGUILayout.ObjectField("Event Head", currentStorylet.eventHead, typeof(EventNode), false) as EventNode;
                    currentStorylet.canBeInstanced = EditorGUILayout.Toggle(new GUIContent("Can Be Instanced", "Can this quest have multiple instances at the same time?"), currentStorylet.canBeInstanced);


                    GUILayout.Label("");
                    currentStorylet.issuerName = EditorGUILayout.TextField("Issuer Name", currentStorylet.issuerName);
                    currentStorylet.factionName = EditorGUILayout.TextField("Faction Name", currentStorylet.factionName);
                    currentStorylet.debriefMessage = EditorGUILayout.TextField("Debrief Message", currentStorylet.questDescription, wordwrapStyle, GUILayout.Height(80));


                    GUILayout.Label("");
                    currentStorylet.waitTime = EditorGUILayout.FloatField(new GUIContent("Wait Time", "How long to wait before the quest is expected to run out (in ingame hours). If this quest has no timer, input -1"), currentStorylet.waitTime);

                }

            }
                
            GUI.DragWindow();
        }

        
        private void TriggerChangesWindow(int id)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (currentStorylet)
            {
                triggerChangesScrollPosition = EditorGUILayout.BeginScrollView(triggerChangesScrollPosition, false, true);

                    GUILayout.Label("\nTriggers", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(triggerInt, true);
                    EditorGUILayout.PropertyField(triggerValue, true);
                    EditorGUILayout.PropertyField(triggerState, true);

                    GUILayout.Label("\nChanges", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(changeInt, true);
                    EditorGUILayout.PropertyField(changeValue, true);
                    EditorGUILayout.PropertyField(changeState, true);

                    serializedStorylet.ApplyModifiedProperties();
                    GUILayout.Label("");

                EditorGUILayout.EndScrollView();

                GUILayout.Label("");
            }
        }


        private void DrawSettingsWindow(int id)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("If you are confused on what fields are for, hover over them for tooltips!", wordwrapStyle);

            // Draw controls and settings
            GUILayout.Label("\nControls", EditorStyles.boldLabel);
            GUILayout.Label("Ctrl + Scroll Wheel : Zoom in & out (Hold Shift to zoom faster)");
            GUILayout.Label("Middle Mouse Button : Pan around the window");
            GUILayout.Label("Alt + Left Click : Pan around the window");
            GUILayout.Label("Home : Reset zoom position");
            GUILayout.Label("");
            zoomFactor = EditorGUILayout.Slider("Zoom Factor", zoomFactor, 1f, 2f);
            
            if ( GUILayout.Button("Reset zoom view") )
            {
                zoomPosition = Vector2.zero;
                zoomFactor = 1f;
            }


            // Storylet management
            GUILayout.Label("");
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("\nStorylet Management", EditorStyles.boldLabel);

            currentStorylet = EditorGUILayout.ObjectField("Current Storylet", currentStorylet, typeof(Storylet), false) as Storylet;

            GUILayout.Label("");

            //Storylet image

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (currentStorylet)
            {
                // Rename Storylet filename to name on file
                if ( GUILayout.Button( new GUIContent("Rename Storylet filename", "Renames the file name of the current Storylet to the name on file (this will also save any changes to the Storylet)") ) )
                {
                    if (currentStorylet.questName == "")
                    {
                        Debug.LogWarning("The current storylet doesn't have a name! Set a name before renaming the file");
                    }
                    else
                    {
                        string storyletPath = AssetDatabase.GetAssetPath(currentStorylet);
                        AssetDatabase.RenameAsset(storyletPath, currentStorylet.questName);
                        SaveCurrentStorylet();
                    }
                }

                if ( GUILayout.Button( new GUIContent("Save Storylet", "Saves any changes to the Storylet asset to disk") ) )
                {
                    SaveCurrentStorylet();
                }
            }


            // New Storylet Creator
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("\nNew Storylet Creator", EditorStyles.boldLabel);

            // Create a new Storylet asset
            newStoryletAssetPath = EditorGUILayout.TextField( new GUIContent("Asset Path", "Asset path to create new storylet at. INCLUDE THE FILE NAME OF THE ASSET TO BE CREATED! (includes the .asset extension)"), newStoryletAssetPath);

            if ( GUILayout.Button( new GUIContent("Create New Storylet", "Creates a new Storylet at the specified path. MAKE SURE THAT THE PATH ENDS WITH .asset!") ) )
            {
                if (!newStoryletAssetPath.EndsWith(".asset"))
                {
                    Debug.LogWarning("Asset Path does not end with '.asset'!");
                }
                else
                {
                    Storylet newStorylet = ScriptableObject.CreateInstance<Storylet>();
                    newStorylet.questName = "New Storylet";

                    AssetDatabase.CreateAsset(newStorylet, newStoryletAssetPath);
                    currentStorylet = newStorylet;

                    SaveCurrentStorylet();
                }
            }
        }

    #endregion


        private void SaveCurrentStorylet()
        {
            EditorUtility.SetDirty(currentStorylet);
            AssetDatabase.SaveAssets();
        }

        private void GetStoryletLists()
        {
            if (currentStorylet)
            {
                serializedStorylet = new SerializedObject(currentStorylet);

                triggerInt = serializedStorylet.FindProperty("triggerInts");
                triggerValue = serializedStorylet.FindProperty("triggerValues");
                triggerState = serializedStorylet.FindProperty("triggerStates");

                changeInt = serializedStorylet.FindProperty("triggerIntChanges");
                changeValue = serializedStorylet.FindProperty("triggerValueChanges");
                changeState = serializedStorylet.FindProperty("triggerStateChanges");
            }
        }

    }
}
