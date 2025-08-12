using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray rayDown = new Ray(gameObject.transform.position, Vector3.down);
        RaycastHit hit;

        if(!Physics.Raycast(rayDown,out hit, 1.6f))
        {
            Destroy(gameObject);
        }
	}
}
