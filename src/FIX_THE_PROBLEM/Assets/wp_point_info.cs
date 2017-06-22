using System.Collections;
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

    public void get_reg_data(long _id)
    {
        id = _id;
        this.name = "wp_" + type.ToString().ToLower() + "_" + id.ToString();
        registered = true;
    }

    public void get_unreg_data()
    {
        id = -1;
        registered = false;
        this.name = "wp_" + type.ToString().ToLower() + "_" + id.ToString() + "invalid";
    }
	// Use this for initialization
	void Start () {
        this.name = "wp_" + type.ToString().ToLower() + "_" + id.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
