using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Camera cam;
    public PlayerController playerController;
    public GroundController groundController;
    public Color[] colors;

    private bool isMoved;
    private bool cameraFixedLeft;
    private bool cameraFixedRight;
    private float timeToMoveCamera = 0.5f;
    private float timeToFixCamera = 0.3f;
    private int temp;
	// Use this for initialization
	void Start () {

        int indexColor = Random.Range(0, colors.Length);
        cam.backgroundColor = colors[indexColor];

        isMoved = false;
        cameraFixedLeft = false;
        cameraFixedRight = false;
        temp = groundController.groundRandomNumber;
	}

    // Update is called once per frame
    void Update()
    {
        if (groundController.finishCreateGround && !isMoved)
        {
            isMoved = true;
            if (temp < groundController.groundRandomNumber)
            {
                StartCoroutine(FixCamera());             
            }
            temp = groundController.groundRandomNumber;
        }
        
        if (!groundController.finishCreateGround)
        {
            isMoved = false;
        }

        if (Input.GetMouseButtonDown(0) && !playerController.touchDisable)
        {
            if (playerController.dirTurn < 0)
            {
                StartCoroutine(MoveCamera());
            }
        }     
    }

    //Move camera by y position with Lerp function
    IEnumerator MoveCamera()
    {
        Vector3 startPos = cam.transform.position;
        Vector3 endPos = cam.transform.position += new Vector3(2.7f, 2.7f, 2.7f);
        float t = 0;
        while (t < timeToMoveCamera)
        {
            float fraction = t / timeToMoveCamera;
            cam.transform.position = Vector3.Lerp(startPos, endPos, fraction);
            t += Time.deltaTime;
            yield return null;
        }
        cam.transform.position = endPos;
    }


    //This funtion make camera always located between the screen and make sure it not fall off the screen
    IEnumerator FixCamera()
    {
        Vector3 startPos = cam.transform.position;
        float t = 0;
        if (playerController.dirTurn > 0 && !cameraFixedLeft)
        {
            cameraFixedLeft = true;
            cameraFixedRight = false;
            Vector3 endPos = cam.transform.position + new Vector3(-1.5f, 0, 1.5f);
            while (t < timeToFixCamera)
            {
                float fraction = t / timeToFixCamera;
                cam.transform.position = Vector3.Lerp(startPos, endPos, fraction);
                t += Time.deltaTime;
                yield return null;
            }
            cam.transform.position = endPos;
        }
        if(playerController.dirTurn < 0 && !cameraFixedRight)
        {
            cameraFixedRight = true;
            cameraFixedLeft = false;
            Vector3 endPos = cam.transform.position + new Vector3(1.5f, 0, -1.5f);
            while (t < timeToFixCamera)
            {
                float fraction = t / timeToFixCamera;
                cam.transform.position = Vector3.Lerp(startPos, endPos, fraction);
                t += Time.deltaTime;
                yield return null;
            }
            cam.transform.position = endPos;
        }       
    }
}
