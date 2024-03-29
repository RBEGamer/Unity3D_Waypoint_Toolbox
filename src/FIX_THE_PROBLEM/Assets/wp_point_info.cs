﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wp_point_info : MonoBehaviour {


    public enum WP_TYPE
    {
        WP_START,
        WP_END,
        WP_POINT,
        WP_INTERACTION
    }


    public WP_TYPE type;
    public long id = -1;
    public bool registered = false;



    public List<GameObject> neighbour_wp = new List<GameObject>();

	public bool unreg_neighbour(GameObject _go, bool _bidir = false){
		if(_go == null || _go.GetComponent<wp_point_info>() == null){
			Debug.LogError("NEIGHBOUR REGISTER FAILED - no wp info script");
			return false;
		}
		if(_bidir){
			_go.GetComponent<wp_point_info>().unreg_neighbour(this.gameObject,false);
		}
		neighbour_wp.Remove(_go);
		_go.GetComponent<wp_point_info>().update_wp_renderer();
		remove_unness_neigh();
		update_wp_renderer();
		return true;
	}

    public bool reg_neighbour(GameObject _go, bool _bidir = false)
    {
        if(_go.GetComponent<wp_point_info>() == null)
        {
            Debug.LogError("NEIGHBOUR REGISTER FAILED - no wp info script");
            return false;
        }
		if (_bidir)
		{
			_go.GetComponent<wp_point_info>().reg_neighbour(this.gameObject, false);
			Debug.Log("try to create bidec connection");
		}
		for (int i = 0; i < neighbour_wp.Count; i++) {
				if(neighbour_wp[i] == _go)
            	{
				Debug.Log("NEIGHBOUR REGISTER FAILED - neighbour already exists no connection made");
                return false;
            	}
        }   
			//finally register in list
        neighbour_wp.Add(_go);
		_go.GetComponent<wp_point_info>().update_wp_renderer();
		remove_unness_neigh();
		update_wp_renderer();
        return true;
    }

	//removes null objects from list (if the user deletes points without the editor extention)
	public void remove_unness_neigh(){
		List<GameObject> tmpl = new List<GameObject>();
		for (int i = 0; i < neighbour_wp.Count; i++) {
			if(neighbour_wp[i] != null){
				tmpl.Add(neighbour_wp[i]);
			}
		}
		neighbour_wp = tmpl;
	}

	public bool has_neighbour(GameObject _go){
		if(_go == null || _go.GetComponent<wp_point_info>() == null){
			Debug.LogError("NEIGHBOUR REGISTER FAILED - no wp info script");
			return false;
		}
		for (int i = 0; i < neighbour_wp.Count; i++) {
			if(neighbour_wp[i] == _go){
				return true;
			}	
		}
		return false;
	}
		
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.black;
        for (int i = 0; i < neighbour_wp.Count; i++)
        {
            if(neighbour_wp[i] == null) { continue; }
            UnityEditor.Handles.DrawLine(this.transform.position + new Vector3(0.0f, 0.1f, 0.0f), neighbour_wp[i].transform.position + new Vector3(0.0f, 0.1f, 0.0f));
			UnityEditor.Handles.DrawWireCube(Vector3.Lerp(this.transform.position,neighbour_wp[i].transform.position,0.8f),new Vector3(0.1f,0.1f,0.1f));
		}
    }
		
    public void get_reg_data(long _id)
    {
        id = _id;
        this.name = "wp_" + type.ToString().ToLower() + "_" + id.ToString();
        registered = true;
    }

	[ContextMenu("UNREGISTER WAYPOINT")]
    public void get_unreg_data()
    {
		//DELETE this go OWN ON NEIGHBOURS
		for (int i = 0; i < neighbour_wp.Count ; i++) {
			if(neighbour_wp[i] != null && neighbour_wp[i].GetComponent<wp_point_info>() != null){
					neighbour_wp[i].GetComponent<wp_point_info>().unreg_neighbour(this.gameObject, false);
			}
		}
		neighbour_wp.Clear();
        id = -1;
        registered = false;
        this.name = "wp_" + type.ToString().ToLower() + "_" + id.ToString() + "invalid";
		update_wp_renderer();

    }


	public void update_wp_renderer(){
		//IF A wp_point_way_line_renderer componend is attached
		if(this.gameObject.GetComponent<wp_point_way_line_renderer>() != null){
			this.gameObject.GetComponent<wp_point_way_line_renderer>().update_renderer();
		}
		//TODO
		//IF A wp_point_way_mesh_renderer componen is attached
	}


	// Use this for initialization
	void Start () {
		if(neighbour_wp.Count <= 0 && type == WP_TYPE.WP_START){
			Debug.LogWarning("This START_WAYPOINT has no neighbours");
		}


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
