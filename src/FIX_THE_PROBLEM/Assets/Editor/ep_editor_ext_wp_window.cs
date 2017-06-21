using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ep_editor_ext_wp_window : EditorWindow {

    [MenuItem("WAYPOINT/CONNECTION_EDITOR")]
    public static void ShowWindow()
    {
        GetWindow<ep_editor_ext_wp_window>(false, "WP CONECTOR", true);
      //  SceneView.onSceneGUIDelegate += OnSceneGUI;
    }




void OnGUI()
{





    if (!WP_CON_EDITOR.WP_SET_MODE && GUILayout.Button("ENABLE SET MODE"))
    {
        Debug.Log("enable_con_mode");
            WP_CON_EDITOR.deselect_tools();
        WP_CON_EDITOR.WP_SET_MODE = true;


    }
    if (WP_CON_EDITOR.WP_SET_MODE && GUILayout.Button("DISABLE SET MODE"))
    {
        Debug.Log("disable_con_mode");
            WP_CON_EDITOR.deselect_tools();
    }







    if (!WP_CON_EDITOR.WP_REMOVE_MODE && GUILayout.Button("ENABLE CONNECTION REMOVE MODE"))
    {
        Debug.Log("enable_con_remove_mode");
            WP_CON_EDITOR.deselect_tools();
        WP_CON_EDITOR.WP_REMOVE_MODE = true;
    

    }
    if (WP_CON_EDITOR.WP_REMOVE_MODE && GUILayout.Button("DISABLE CONNECTION REMOVE MODE"))
    {
        Debug.Log("disable_con_remove_mode");
            WP_CON_EDITOR.deselect_tools();

    }


        GUILayout.Label("BASE SETTINGS", EditorStyles.boldLabel);
        WP_CON_EDITOR.set_placement_tag(EditorGUILayout.TextField("VALID_PLACE_TAG", WP_CON_EDITOR.CON_PLACEMENT_TAG));

        WP_CON_EDITOR.set_wp_types_asset_path(EditorGUILayout.TextField("WP_ASSET_PATH", WP_CON_EDITOR.CON_ASSET_SET_PATH));

     

    }



}



// return EditorPrefs.GetBool("MuteAllSounds", false);

public class WP_CON_EDITOR
{
    public static bool WP_SET_MODE = false;
    public static bool WP_REMOVE_MODE = false;
    public static string CON_ASSET_SET_PATH = "Assets\\WP_ASSETS\\wpt.asset";
    public static string CON_PLACEMENT_TAG = "WP_PLACEABLE_GROUND";


    public static void set_placement_tag(string _s)
    {
        bool found = false;
        foreach (string n in UnityEditorInternal.InternalEditorUtility.tags)
        {
            if(n.Contains(_s)) { found = true; }
        }
        if (!found){
            Debug.LogError("ERROR - PLEASE SET THE TAG IN THE TAG MANAGER");
            return;
        }
        Debug.Log("WP_TAG - tag exitsts");
        CON_PLACEMENT_TAG = _s;
    }

    public static void set_wp_types_asset_path(string _s)
    {
        wp_point_types wp_blocks = null;
        wp_blocks = AssetDatabase.LoadAssetAtPath<wp_point_types>(_s);
        if(wp_blocks == null)
        {
            Debug.LogError("ERROR - PLEASE SET A VALID ASSET PATH FOR THE WP_TYPES.asset");
            CON_ASSET_SET_PATH = "";
        }
        CON_ASSET_SET_PATH = _s;
        Debug.Log("WP_ASSET - Path succ set");
       
    }
    public static void deselect_tools()
    {
        WP_REMOVE_MODE = false;
        WP_SET_MODE = false;
    }


}