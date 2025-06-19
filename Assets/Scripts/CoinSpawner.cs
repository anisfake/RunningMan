using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private Transform highPos;
    [SerializeField] private Transform lowPos;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius = 0.3f;
    [SerializeField] private float offsetY = 0.5f;
    [SerializeField] private float preSpawnOffsetX = 10f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnRate)
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    private void SpawnObstacle()
    {
        int index = Random.Range(0, obstacles.Length);
        Transform basePos = (index == 2) ? highPos : lowPos;

        Vector3 spawnPos = basePos.position;
        spawnPos.x = Camera.main.transform.position.x + preSpawnOffsetX;

        int maxAttempts = 5;
        while (Physics2D.OverlapCircle(spawnPos, checkRadius, groundLayer) && maxAttempts > 0)
        {
            spawnPos.y += offsetY;
            maxAttempts--;
        }


        Instantiate(obstacles[index], spawnPos, Quaternion.identity);
        Debug.Log($"🪙 Spawned: {obstacles[index].name} tại {spawnPos}");
    }
}
