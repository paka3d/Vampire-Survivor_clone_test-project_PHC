using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 0.05f;
    public int damageOnHit = 10;

    private Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (target == null) return;
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            DestroyEnemy();
        }
        if (other.CompareTag("Player"))
        {
            PlayerAgentController pc = other.GetComponent<PlayerAgentController>();
            if (pc != null)
            {
                pc.TakeDamage(damageOnHit);
                pc.OnEnemyDefeated(); // <-- Make sure this is here!
            }
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        GameManager.instance.AddScore(1);
        if (GameManager.instance.ammoDropPrefab != null)
        {
            Instantiate(GameManager.instance.ammoDropPrefab, transform.position, Quaternion.identity);
        }
        EnemyPool.instance.ReturnEnemy(gameObject);
    }
}
