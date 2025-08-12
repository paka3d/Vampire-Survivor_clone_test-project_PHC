using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // The Rigidbody will handle movement via velocity in PlayerController.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Damage handled in Enemy.cs
            Destroy(gameObject);
        }
    }
}
