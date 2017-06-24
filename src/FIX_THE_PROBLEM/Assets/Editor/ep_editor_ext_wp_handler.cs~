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
    private static wp_point_asset wp_blocks;
    public static int SelectedBlock = -1;
    static GameObject new_block_to_create = null;
    static GameObject clicked_go = null;
	static bool was_one_time_remove = false;

	//VARS FOR CONNECTION MODE
	static GameObject con_mode_first_go = null;
	static GameObject con_mode_second_go = null;

    static ep_editor_ext_wp_handler()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        //LOAD ASSET PACK
        wp_blocks = AssetDatabase.LoadAssetAtPath<wp_point_asset>(WP_CON_EDITOR.CON_ASSET_SET_PATH);
        new_block_to_create = null;
        SelectedBlock = -1; 
		was_one_time_remove = false;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {

        if (!WP_CON_EDITOR.WP_SET_MODE && !WP_CON_EDITOR.WP_REMOVE_MODE && !WP_CON_EDITOR.WP_CONNECTION_MODE) { return; }

        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        //DESELECT TOOL 
        if (Event.current.type == EventType.keyDown &&Event.current.keyCode == KeyCode.Escape){
			WP_CON_EDITOR.deselect_tools();
        }
        //SET PLACE BLOCK
        if (WP_CON_EDITOR.WP_SET_MODE && Event.current.type == EventType.mouseDown && Event.current.button == 0 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false)
        {
           if(new_block_to_create != null && cursor_in_valid_area)
            {
                AddWaypoint(CurrentHandlePosition, new_block_to_create);
				if(!WP_CON_EDITOR.WP_SET_MODE_HOLD_LAST_POINT){
                new_block_to_create = null;
				}
            }
        }
        //SET DESELCE BLOCK
        if (WP_CON_EDITOR.WP_SET_MODE && Event.current.type == EventType.mouseDown && Event.current.button == 1 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false){
			if(!WP_CON_EDITOR.WP_SET_MODE_HOLD_LAST_POINT){
				new_block_to_create = null;
			}
				was_one_time_remove = true;
				WP_CON_EDITOR.deselect_tools();
				WP_CON_EDITOR.WP_REMOVE_MODE = true;
        }
        //REMOVE NODE
        if (WP_CON_EDITOR.WP_REMOVE_MODE && Event.current.type == EventType.mouseDown && Event.current.button == 0 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false)
        {
            if (clicked_go != null)
            {
                if (clicked_go.transform.tag == WP_CON_EDITOR.CON_WP_WP_TAG && clicked_go.GetComponent<wp_point_info>() != null)
                {
                    WP_CON_EDITOR.CON_WP_MANAGER_OBJ.GetComponent<wp_manager>().unregister_wp_and_destroy(clicked_go);
                }
				if(was_one_time_remove){
					was_one_time_remove = false;
					WP_CON_EDITOR.deselect_tools();
					WP_CON_EDITOR.WP_SET_MODE = true;
				}
            }
        }
		//SET DESELCE WP IN CON MODE
		if (WP_CON_EDITOR.WP_CONNECTION_MODE && Event.current.type == EventType.mouseDown && Event.current.button == 1 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false){
			con_mode_first_go = null;
			con_mode_second_go = null;
			clicked_go = null;
		}



        //CONNECTION MODE
		if (WP_CON_EDITOR.WP_CONNECTION_MODE && Event.current.type == EventType.mouseDown && Event.current.button == 0 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false)
		{
			if (clicked_go != null)
			{
				//ASSIGN FIRST
				if (con_mode_first_go == null && clicked_go.GetComponent<wp_point_info>() != null)
				{	con_mode_first_go = clicked_go;
					con_mode_second_go = null;
					//ASSIGN SECOND
				}else if(con_mode_first_go != null && con_mode_second_go == null){
					if(con_mode_first_go != clicked_go){
						con_mode_second_go = clicked_go;
					}
					//AND AM
					//TODO IMPROVE IT FOR BIDIR CONNECTIOS
					if(con_mode_first_go != null && con_mode_second_go != null && clicked_go.GetComponent<wp_point_info>() != null){
						Undo.RecordObject(con_mode_first_go, "make con first go");
						Undo.RecordObject(con_mode_second_go, "make con second go");
						//if this connection doesnt exitsts
						if(!con_mode_first_go.GetComponent<wp_point_info>().has_neighbour(con_mode_second_go)){
							//ADD OBJ
							con_mode_first_go.GetComponent<wp_point_info>().reg_neighbour(con_mode_second_go, WP_CON_EDITOR.WP_CONNECTION_MODE_MAKE_BIDIR);
						}else{
							//was_one_time_remove OBJ
							con_mode_first_go.GetComponent<wp_point_info>().unreg_neighbour(con_mode_second_go,WP_CON_EDITOR.WP_CONNECTION_MODE_MAKE_BIDIR);
						}
						//Undo.RegisterCreatedObjectUndo(con_mode_first_go.GetComponent<wp_point_info>(), "Make Connection ");
						//Undo.RegisterCreatedObjectUndo(con_mode_second_go.GetComponent<wp_point_info>(), "Make Connection ");

						Undo.FlushUndoRecordObjects();
						UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
						con_mode_first_go = null;
						con_mode_second_go = null;
					}

				}else{
				con_mode_first_go = null;
				con_mode_second_go = null;
				}



			}
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
            clicked_go = hit.collider.gameObject; //SET HERE THE POINT OBJECT
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
        //IF IN SET MODE DRAW CIRCLE INDICATOR AROUND THE MOUSE POSITION
        if (WP_CON_EDITOR.WP_SET_MODE)
        {
            UnityEditor.Handles.color = Color.red;
            if (cursor_in_valid_area){
                UnityEditor.Handles.color = Color.green;
            }
            UnityEditor.Handles.DrawWireDisc(CurrentHandlePosition + new Vector3(0.0f, 0.1f, 0.0f), Vector3.up, 0.5f);

            if (CurrentHandlePosition != m_OldHandlePosition){
                SceneView.RepaintAll();
                m_OldHandlePosition = CurrentHandlePosition;
            }
        }

		//IF IN CON MODE DRAW MODE MARK SELECTED NODES WITH A CIRCLE
		if(WP_CON_EDITOR.WP_CONNECTION_MODE){
			if(con_mode_first_go != null){
				UnityEditor.Handles.color = Color.blue;
				UnityEditor.Handles.DrawWireDisc(con_mode_first_go.transform.position + new Vector3(0.0f, 0.1f, 0.0f), Vector3.up, 1.0f);
			}
			if(con_mode_second_go != null){
				UnityEditor.Handles.color = Color.cyan;
				UnityEditor.Handles.DrawWireDisc(con_mode_second_go.transform.position + new Vector3(0.0f, 0.1f, 0.0f), Vector3.up, 1.0f);
			}

		}

    }

    public static void AddWaypoint(Vector3 position, GameObject prefab)
    {
        if (prefab == null)
        {
            return;
        }
        GameObject newCube = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        newCube.transform.tag = WP_CON_EDITOR.CON_WP_WP_TAG;
        WP_CON_EDITOR.CON_WP_MANAGER_OBJ.GetComponent<wp_manager>().register_wp(newCube);
        if (WP_CON_EDITOR.CON_WP_MANAGER_OBJ != null)
        {
            newCube.transform.parent = WP_CON_EDITOR.CON_WP_MANAGER_OBJ.transform;
        }
        newCube.transform.position = position;
      //  Undo.RegisterCreatedObjectUndo(newCube, "Add " + prefab.name);
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }








    static void DrawCustomBlockButtons(SceneView sceneView)
    {
        if (wp_blocks != null)
        {
            Handles.BeginGUI();

        GUI.Box(new Rect(0, 0, 110, sceneView.position.height - 35), GUIContent.none, EditorStyles.textArea);     
            for (int i = 0; i < wp_blocks.Blocks.Count; ++i)
            {
                DrawCustomBlockButton(i, sceneView.position);
            }    
        Handles.EndGUI();
    	}
    }







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
