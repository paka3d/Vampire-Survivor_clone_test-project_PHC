using UnityEngine;

public class Item_Spawn : MonoBehaviour
{
    [Header("Spawnable Prefabs")]
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    [Header("Spawn Settings")]
    [Range(1, 100)]
    public int spawnAmount = 10;

    [Header("Terrain Reference")]
    public Terrain targetTerrain;

    void Start()
    {
        if (targetTerrain == null)
        {
            Debug.LogError("Terrain not assigned!");
            return;
        }

        SpawnItems();
    }

    void SpawnItems()
    {
        GameObject[] items = new GameObject[] { item1, item2, item3 };
        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainPos = targetTerrain.transform.position;

        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject itemToSpawn = items[Random.Range(0, items.Length)];
            if (itemToSpawn == null) continue;

            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);
            float y = terrainData.GetInterpolatedHeight(x / terrainData.size.x, z / terrainData.size.z);

            Vector3 worldPos = new Vector3(x + terrainPos.x, y + terrainPos.y + 1f, z + terrainPos.z); // +1f to raycast from above

            RaycastHit hit;
            if (Physics.Raycast(worldPos, Vector3.down, out hit, 5f))
            {
                GameObject spawnedItem = Instantiate(itemToSpawn, hit.point, Quaternion.identity);
                spawnedItem.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }
    }
}
