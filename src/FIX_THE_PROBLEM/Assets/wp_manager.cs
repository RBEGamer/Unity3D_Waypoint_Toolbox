using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wp_manager : MonoBehaviour {




    long id_counter = 0;
    public List<GameObject> all_points = new List<GameObject>();


    public void register_wp(GameObject _go) {
    
       GameObject point = _go;
        point.GetComponent<wp_point_info>().get_reg_data(id_counter++);
        all_points.Add(point);
    }


    public void unregister_wp_and_destroy(GameObject _go)
    {
        _go.GetComponent<wp_point_info>().get_unreg_data();
        all_points.Remove(_go);
#if UNITY_EDITOR 
        DestroyImmediate(_go);
#else
        Destroy(_go);
#endif
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}



