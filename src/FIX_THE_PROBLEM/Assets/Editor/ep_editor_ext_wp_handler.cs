using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ep_editor_ext_wp_handler : Editor
{
    public static Vector3 CurrentHandlePosition = Vector3.zero;
    static Vector3 m_OldHandlePosition = Vector3.zero;
    static bool cursor_in_valid_area = false;
    private static wp_point_types wp_blocks;

    static Transform wp_add_parent_transform = null;


    //public static int SelectedBlock
    //{
    //    get
    //    {
    //        return EditorPrefs.GetInt("SelectedEditorBlock", 0);
    //    }
    //    set
    //    {
    //        EditorPrefs.SetInt("SelectedEditorBlock", value);
    //    }
    //}



    //AREA WP BLOCK TO SET AT CON_SETMODE
    static GameObject new_block_to_create = null;


    static ep_editor_ext_wp_handler()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        //LOAD ASSETS
        wp_blocks = AssetDatabase.LoadAssetAtPath<wp_point_types>(WP_CON_EDITOR.CON_ASSET_SET_PATH);
        new_block_to_create = null;
        SelectedBlock = -1;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {

        if (!WP_CON_EDITOR.WP_SET_MODE) { return; }

        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        //DESELECT TOOL 
        if (Event.current.type == EventType.keyDown &&Event.current.keyCode == KeyCode.Escape){
            WP_CON_EDITOR.deselect_tools();
        }

        if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false)
        {
           if(new_block_to_create != null && cursor_in_valid_area)
            {
                AddWaypoint(CurrentHandlePosition, new_block_to_create);

                new_block_to_create = null;
            }
        }
        else if (Event.current.type == EventType.mouseDown && Event.current.button == 1 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false)
        {
            new_block_to_create = null;
        }

            HandleUtility.AddDefaultControl(controlId);
        UpdateHandlePosition();
        UpdateRepaint();
        DrawCustomBlockButtons(sceneView);

    }


    static void UpdateHandlePosition()
    {
        if (Event.current == null)
        {
            return;
        }

        Vector2 mousePosition = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);
       
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 offset = Vector3.zero;
             offset = hit.normal;

            CurrentHandlePosition.x = Mathf.Floor(hit.point.x - hit.normal.x * 0.001f + offset.x);
            CurrentHandlePosition.y = Mathf.Floor(hit.point.y - hit.normal.y * 0.001f + offset.y);
            CurrentHandlePosition.z = Mathf.Floor(hit.point.z - hit.normal.z * 0.001f + offset.z);
           // CurrentHandlePosition += new Vector3(0.5f, 0.5f, 0.5f);

            if (hit.collider.tag == WP_CON_EDITOR.CON_PLACEMENT_TAG)
            {
                cursor_in_valid_area = true;
            }else
            {
                cursor_in_valid_area = false;
            }
        }
    }

    static void UpdateRepaint()
    {
        //DRAW HANDLE INDICATOR
        UnityEditor.Handles.color = Color.red;
        if (cursor_in_valid_area)
        {
            UnityEditor.Handles.color = Color.green;
        }
        UnityEditor.Handles.DrawWireDisc(CurrentHandlePosition+ new Vector3(0.0f,0.1f,0.0f), Vector3.up, 1.0f);


        //If the cube handle position has changed, repaint the scene
        if (CurrentHandlePosition != m_OldHandlePosition)
        {
                        SceneView.RepaintAll();
            m_OldHandlePosition = CurrentHandlePosition;
        }
    }

    public static void AddWaypoint(Vector3 position, GameObject prefab)
    {
        if (prefab == null)
        {
            return;
        }
        GameObject newCube = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        if (wp_add_parent_transform != null)
        {
            newCube.transform.parent = wp_add_parent_transform;
        }
        newCube.transform.position = position;
        Undo.RegisterCreatedObjectUndo(newCube, "Add " + prefab.name);
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }








    static void DrawCustomBlockButtons(SceneView sceneView)
    {
        Handles.BeginGUI();

        GUI.Box(new Rect(0, 0, 110, sceneView.position.height - 35), GUIContent.none, EditorStyles.textArea);

        if (wp_blocks != null)
        {
            for (int i = 0; i < wp_blocks.Blocks.Count; ++i)
            {
                DrawCustomBlockButton(i, sceneView.position);
            }
        }
        Handles.EndGUI();
    }




    public static int SelectedBlock = -1;
   

    static void DrawCustomBlockButton(int index, Rect sceneViewRect)
    {


        if (WP_CON_EDITOR.WP_SET_MODE)
        {
            bool isActive = false;


            Texture2D previewImage = AssetPreview.GetAssetPreview(wp_blocks.Blocks[index].preview_image_128_128);
            GUIContent buttonContent = new GUIContent(previewImage);
            GUI.Label(new Rect(5, index * 128 + 5, 100, 20), wp_blocks.Blocks[index].Name);
            bool isToggleDown = GUI.Toggle(new Rect(5, index * 128 + 25, 100, 100), isActive, buttonContent, GUI.skin.button); //WATCH OUT
            if (isToggleDown == true && !isActive)
            {
                SelectedBlock = index;
                new_block_to_create = wp_blocks.Blocks[index].Prefab;
                Debug.Log("EP_EDITOR - BLOCK TO SET :" + wp_blocks.Blocks[index].Name);
            }
        }else
        {
            new_block_to_create = null;
        }
    }








}
