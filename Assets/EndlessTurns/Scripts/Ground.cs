using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {


    //Destroy ground after end animation
    public void OnAnimationEnd()
    {
       gameObject.GetComponent<MeshRenderer>().enabled = false;
       Destroy(gameObject);
    }
}
