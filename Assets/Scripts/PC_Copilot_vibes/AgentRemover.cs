using UnityEngine;

public class AgentRemover : MonoBehaviour
{
    // Assign your Agent GameObject in the Inspector or via script
    public GameObject Agent;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is tagged as "Goal"
        if (other.CompareTag("goal") && Agent != null)
        {
            Destroy(Agent); // Remove Agent from the scene
        }
    }
}
