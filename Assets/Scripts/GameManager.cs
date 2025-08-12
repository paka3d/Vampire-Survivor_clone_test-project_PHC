using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject enemyPrefab;
    public GameObject ammoDropPrefab;
    public int enemiesPerWave = 5;
    public float spawnInterval = 2f;
    public float spawnRadius = 20f;

    public int score = 0;

    private PlayerAgentController player;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAgentController>();
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0.8f,
            Random.Range(-spawnRadius, spawnRadius));

        if (Vector3.Distance(spawnPos, player.transform.position) < 5f)
        {
            spawnPos += (player.transform.position - spawnPos).normalized * 5f;
        }

        EnemyPool.instance.GetEnemy(spawnPos);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Score: {score}");
        if (score % 10 == 0 && player != null && player.levelUpUI != null)
        {
            Debug.Log("Level Up UI Triggered by Score!");
            player.levelUpUI.ShowLevelUpOptions(player);
        }
    }

    public void PlayerDied()
    {
        Debug.Log("Player has died!");
        // Add game over logic here, e.g. reload scene, show UI, etc.
    }
}
