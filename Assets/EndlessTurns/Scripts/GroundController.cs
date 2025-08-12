using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GroundController : MonoBehaviour
{
    public GameObject groundPrefab; // The ground to create
    public GameObject firstGround; // First ground
    public GameObject theWall; // The wall
    public GameObject gold; // Gold
    public GameObject parentObject; // This object is parent of all ground
    public PlayerController playerController;
    public int groundRandomNumber; // How much ground is created, you can't change it 
    public bool enableTouch;
    public bool finishCreateGround; // Check coroutine RandomGroundAndWall is finish
    [Range(0, 1)]
    public float goldFrequency; //Probability to create gold, if == 1 : create gold everytime we create ground, if == 0 : gold doesn't create
    public float timeToDestroyGround = 0.15f; // How long from the first ground have destroyed to the last ground have destroyed in one list
    public float timeToDestroyGroundAfterGameOver = 0.5f; // How long the DestroyGroundAfterGameOver function is called after game over

    private List<GameObject> listGroundLeft = new List<GameObject>();
    private List<GameObject> listGroundRight = new List<GameObject>();
    private GameObject currentGround;
    private Vector3 firstPosOfGround;
    private Vector3 leftRandomPositionOfTheWall = new Vector3(0, 6f, 0.5f);
    private Vector3 rightRandomtPositionOfTheWall = new Vector3(0.5f, 6f, 0);
    private bool isGroundAndWallHaveRandomOnRight = false;
    private bool isGroundAndWallHaveRandomOnLeft = false;

    private const int maxGroundRandomNumber = 7;
    private const int minGroundRandomNumber = 5;
    private int indexPositionOfGround;
    // Use this for initialization
    void Start()
    {
        groundRandomNumber = 5;
        firstPosOfGround = firstGround.transform.position + Vector3.forward; //Make firstPosOfGround equals to position of firstGround
        listGroundLeft.Add(firstGround); // Add first ground for listGround_2
        StartCoroutine(RandomGroundAndWall(firstPosOfGround, groundRandomNumber, Vector3.forward, leftRandomPositionOfTheWall, 1, listGroundLeft));
              
    }

    // Update is called once per frame
    void Update()
    {

        // If game over , call destroy function after a period of time
        if (playerController.gameOver)
        {
            Invoke("DestroyGroundAfterGameOver", timeToDestroyGroundAfterGameOver);
        }

        if (playerController.dirTurn < 0) // The player is running on left 
        {
            
            if (playerController.isPlayerHitTheWall) // Player hit the wall
            {

                
                if (!isGroundAndWallHaveRandomOnRight) // The ground have not create on right
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.randomGround);
                    isGroundAndWallHaveRandomOnRight = true; // The ground and wall have create on right
                    isGroundAndWallHaveRandomOnLeft = false; // Make the ground and wall haven't create on left

                    // Turn animation of ground on and then destroy it after animation end 20 second, Destroy function is on Ground script
                    if (listGroundRight != null) // If listGroundRight not null
                    {
                        List<GameObject> newList = ListCopyOf(listGroundRight); // Create newList and point it to a list 

                        //Start to scale ground
                        for (int i = 0; i < newList.Count; i++)
                        {
                            GameObject countGround = newList[i];
                            StartCoroutine(ScaleGround(countGround, timeToDestroyGround * (float)i));
                        }
                        //Clear list
                        ListCopyOf(listGroundRight).Clear();
                        listGroundRight.Clear();
                    }
 
                    // Create new ground
                    groundRandomNumber = Random.Range(minGroundRandomNumber, maxGroundRandomNumber); // How much ground is random
                    indexPositionOfGround = Random.Range(2, listGroundLeft.Count - 2); // Position to create ground
                    StartCoroutine(RandomGroundAndWall(listGroundLeft[indexPositionOfGround].transform.position + Vector3.right, groundRandomNumber, Vector3.right, rightRandomtPositionOfTheWall, playerController.dirTurn, listGroundRight)); // Random ground on right

                   

                    // Create new wall
                    int indexOfTheWall = Random.Range(0, indexPositionOfGround - 2); // Position to create wall, it's must be between 0 and indexPositionOfGround -2
                    Instantiate(theWall, listGroundLeft[indexOfTheWall].transform.position + leftRandomPositionOfTheWall, Quaternion.identity); // Create the wall
                    
                }
            }
        }

        else
        {
            if (playerController.isPlayerHitTheWall) //Player hit the wall
            {
                
                if (!isGroundAndWallHaveRandomOnLeft)// The ground and wall haven't create on left
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.randomGround);
                    isGroundAndWallHaveRandomOnLeft = true; //The ground and wall have create on left
                    isGroundAndWallHaveRandomOnRight = false; // Make the ground and wall haven't create on right

                    // Turn animation of ground on and then destroy it after animation end 20 second, Destroy function is on Ground script
                    if (listGroundLeft != null) // If listGroundRight not null
                    {
                        
                        List<GameObject> newList = ListCopyOf(listGroundLeft); // Create newList and point it to a list 

                        //Start to scale ground
                        for (int i = 0; i < newList.Count; i++) 
                        {
                            GameObject countGround = newList[i];
                            StartCoroutine(ScaleGround(countGround, timeToDestroyGround * (float)i));
                        }

                        //Clear list
                        ListCopyOf(listGroundLeft).Clear();
                        listGroundLeft.Clear();
                    }

                    //Create new ground
                    groundRandomNumber = Random.Range(minGroundRandomNumber, maxGroundRandomNumber);//How much ground is random
                    indexPositionOfGround = Random.Range(2, listGroundRight.Count - 2);//Position to create ground
                    StartCoroutine(RandomGroundAndWall(listGroundRight[indexPositionOfGround].transform.position + Vector3.forward, groundRandomNumber, Vector3.forward, leftRandomPositionOfTheWall, playerController.dirTurn, listGroundLeft)); // Create ground on left

                    


                    //Create new wall
                    int indexOfTheWall = Random.Range(0, indexPositionOfGround - 2);// Position to create the wall, it's must be between 0 and indexPositionOfGround -2
                    GameObject currentWall = (GameObject)Instantiate(theWall, listGroundRight[indexOfTheWall].transform.position + rightRandomtPositionOfTheWall, Quaternion.identity); //Create the wall
                    currentWall.transform.rotation = Quaternion.Euler(0, 90, 0); // Rotate the wall
                    
                }

            }
        }
    }
      


    // Create ground and wall at position of last ground
    IEnumerator RandomGroundAndWall(Vector3 pos, int numberOfGround, Vector3 directionOfGround, Vector3 positionOfTheWall, int dirTurn, List<GameObject> list)
    {
        
        enableTouch = false; // Disable touch
        finishCreateGround = false;
        for (int i = 0; i < numberOfGround; i++)
        {
            if (dirTurn < 0)
            {
                pos += new Vector3(0.5f, 0, 0);
            }
            else
            {
                pos += new Vector3(0, 0, 0.5f);
            }
            currentGround = (GameObject)Instantiate(groundPrefab, pos, Quaternion.identity); //Create ground
            currentGround.transform.SetParent(parentObject.transform);
            list.Add(currentGround);
            pos = currentGround.transform.position + directionOfGround;
            yield return new WaitForSeconds(0.05f);
        }


        //Create wall
        GameObject currentWall = (GameObject)Instantiate(theWall, currentGround.transform.position + positionOfTheWall, Quaternion.identity); //Create wall
        finishCreateGround = true;
        if (dirTurn < 0)
        {
            currentWall.transform.rotation = Quaternion.Euler(0, 90, 0);
        }


        //Create gold
        int posGold = Random.Range(0, list.Count - 1); //Position to create gold
        float indexGold = Random.Range(0f, 1f); 
        if (indexGold <= goldFrequency)
        {
            Instantiate(gold, list[posGold].transform.position + new Vector3(0f, 5.5f, 0f), Quaternion.identity);
        }
        enableTouch = true; // Enable touch
    }

    // Create a copy list
    List<GameObject> ListCopyOf(List<GameObject> listToCopy)
    {
        List<GameObject> newList = new List<GameObject>();
        for (int i = 0; i < listToCopy.Count; i++)
        {
            newList.Add(listToCopy[i]);
        }
        return newList;
    }

    // Turn animation of ground on
    IEnumerator ScaleGround(GameObject ground, float time)
    {
        yield return new WaitForSeconds(time);
        ground.GetComponent<Animator>().SetTrigger("Die");
    }


    // Destroy ground after game over
    void DestroyGroundAfterGameOver()
    {
        if (listGroundLeft != null)
        {
            List<GameObject> newList = ListCopyOf(listGroundLeft);
            for (int i = 0; i < newList.Count; i++)
            {
                GameObject countGround = newList[i];
                StartCoroutine(ScaleGround(countGround, timeToDestroyGround * (float)i));
            }

            listGroundLeft.Clear();
        }

        if (listGroundRight != null)
        {
            List<GameObject> newList = ListCopyOf(listGroundRight);
            for (int i = 0; i < newList.Count; i++)
            {
                GameObject countGround = newList[i];
                StartCoroutine(ScaleGround(countGround, timeToDestroyGround * (float)i));
            }
            listGroundRight.Clear();
        }
    }
}
