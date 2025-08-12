using UnityEngine;
using System.Collections;

public class GoldController : MonoBehaviour {

    public float speedGoldFalling = 20f; // How fast gold falling down
	// Update is called once per frame

    // Make the gold falling down after game over if it didn't destroy
	void Update () {
        Ray rayDown = new Ray(gameObject.transform.position, Vector3.down);
        RaycastHit hit;
        if(!Physics.Raycast(rayDown,out hit, 0.5f))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.down * speedGoldFalling;
        }
	}
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
