using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public GameObject player;
    public ParticleSystem particle;
    public GroundController groundController;
    public int dirTurn; // This variable make the player turn left or turn right
    public bool touchDisable;
    public bool isPlayerHitTheWall; // Check player hit the wall
    public bool gameOver;
    public float movingSpeedOfPlayer = 10f; //How fast player moving
    public float movingSpeedIncrement = 0.2f;  // How much to increase player speed after each score
    public float timeToDestroyParticle = 0.5f; // How long particle survive



    private ParticleSystem particleTemp;
    private Vector3 dir; // This variable make the player run back when it hit the wall
    private bool hittedWallLeft; // Check the wall left is hitted
    private bool hittedWallright; // Check the wall right is hitted
    // Use this for initialization
    void Start()
    {
        StartCoroutine(MovePlayer());
        touchDisable = false;
        gameOver = false;
        hittedWallLeft = false;
        hittedWallright = false;
        dirTurn = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Make the player redirected every time we touch the screen
        if (Input.GetMouseButtonDown(0) && groundController.enableTouch && !touchDisable)
        {
            movingSpeedOfPlayer += movingSpeedIncrement;
            touchDisable = true;
            dirTurn = dirTurn * (-1);
            if (dirTurn < 0)
            {
                dir = Vector3.forward;
            }
            else
            {
                dir = Vector3.right;
            }

        }

        Ray rayDown = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(rayDown,out hit, 0.6f))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                isPlayerHitTheWall = false;
                Ray rayForward = new Ray(player.transform.position, Vector3.forward);
                Ray rayBack = new Ray(player.transform.position, Vector3.back);
                Ray rayRight = new Ray(player.transform.position, Vector3.right);
                Ray rayLeft = new Ray(player.transform.position, Vector3.left);

                // If player hit the wall, player will run back
                // If player hit gold, count gold and destroy it
                if (Physics.Raycast(rayForward, out hit, 0.6f))
                {
                    if (hit.collider.tag == "Gold")
                    {
                        SoundManager.Instance.PlaySound(SoundManager.Instance.hitCoin);
                        CoinManager.Instance.AddCoins(1);
                        particleTemp = (ParticleSystem)Instantiate(particle, hit.collider.gameObject.transform.position, Quaternion.identity);
                        particleTemp.transform.rotation = Quaternion.Euler(90, 0, 0);
                        particleTemp.Simulate(0.5f, true, false);
                        particleTemp.Play();
                        Destroy(particleTemp, timeToDestroyParticle);
                        Destroy(hit.collider.gameObject);
                    }

                    if (hit.collider.tag == "TheWall")
                    {

                        if (!hittedWallLeft)
                        {
                            hittedWallLeft = true;
                            hittedWallright = false;
                            ScoreManager.Instance.AddScore(1);
                        }
                        isPlayerHitTheWall = true;
                        touchDisable = false;
                        dir = Vector3.back;
                    }
                }

                // If player hit the wall, player will run forward
                else if (Physics.Raycast(rayBack, out hit, 0.6f))
                {
                    if (hit.collider.tag == "TheWall")
                    {

                        dir = Vector3.forward;
                    }
                }

                // If player hit the wall, player will run left
                // If player hit gold, count gold and destroy it
                else if (Physics.Raycast(rayRight, out hit, 0.6f))
                {
                    if (hit.collider.tag == "Gold")
                    {
                        SoundManager.Instance.PlaySound(SoundManager.Instance.hitCoin);
                        CoinManager.Instance.AddCoins(1);
                        particleTemp = (ParticleSystem)Instantiate(particle, hit.collider.gameObject.transform.position, Quaternion.identity);
                        particleTemp.transform.rotation = Quaternion.Euler(90, 0, 0);
                        particleTemp.Simulate(0.5f, true, false);
                        particleTemp.Play();
                        Destroy(particleTemp, timeToDestroyParticle);
                        Destroy(hit.collider.gameObject);
                    }
                    if (hit.collider.tag == "TheWall")
                    {

                        if (!hittedWallright)
                        {
                            hittedWallright = true;
                            hittedWallLeft = false;
                            ScoreManager.Instance.AddScore(1);
                        }
                        isPlayerHitTheWall = true;
                        touchDisable = false;
                        dir = Vector3.left;
                    }
                }

                // If player hit the wall, player will run right
                else if (Physics.Raycast(rayLeft, out hit, 0.6f))
                {
                    if (hit.collider.tag == "TheWall")
                    {

                        dir = Vector3.right;
                    }
                }
            }

        }
        else
        {
            if (!gameOver)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
                touchDisable = true;
                gameOver = true;
                dir = Vector3.down + new Vector3(0, -1, 0);
            }
        }
    }


    // If player hit gold(trigger), destroy gold
    void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.hitCoin);
        CoinManager.Instance.AddCoins(1);
        particleTemp = (ParticleSystem)Instantiate(particle, other.gameObject.transform.position, Quaternion.identity);
        particleTemp.transform.rotation = Quaternion.Euler(90, 0, 0);
        particleTemp.Simulate(0.5f, true, false);
        particleTemp.Play();
        Destroy(particleTemp, timeToDestroyParticle);
        Destroy(other.gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(player);
    }

    // This function make player move with direction(dir), speed(movingSpeedOfPlayer) and real time
    IEnumerator MovePlayer()
    {
        while (true)
        {
            player.transform.position = player.transform.position + dir * movingSpeedOfPlayer * Time.deltaTime;
            yield return null;
        }
    }

}
