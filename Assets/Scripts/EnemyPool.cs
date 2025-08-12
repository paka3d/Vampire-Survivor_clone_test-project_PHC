using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;
    public GameObject enemyPrefab;
    public int poolSize = 100;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy(Vector3 position)
    {
        if (pool.Count > 0)
        {
            GameObject enemy = pool.Dequeue();
            enemy.transform.position = position;
            enemy.transform.rotation = Quaternion.identity;
            enemy.SetActive(true);
            return enemy;
        }
        return null;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}
