using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    public int ammoAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAgentController pc = other.GetComponent<PlayerAgentController>();
            if (pc != null)
            {
                pc.AddAmmo(ammoAmount);
            }
            Destroy(gameObject);
        }
    }
}
