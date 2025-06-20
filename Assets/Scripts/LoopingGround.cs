using UnityEngine;
using UnityEngine.Tilemaps;

public class LoopingGround : MonoBehaviour
{
    [SerializeField] private GameObject[] groundPrefabs;

    private float groundWidth;
    private bool hasSpawnedNext = false;

    void Start()
    {
        CalculateGroundWidth();
    }

    void Update()
    {

        float cameraRight = Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect;
        float cameraLeft = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;

        float rightEdge = GetRightEdge();

        if (!hasSpawnedNext && rightEdge < cameraRight + 1f)
        {
            hasSpawnedNext = true;
            SpawnNextGround();
        }

        if (hasSpawnedNext && rightEdge < cameraLeft - 2f)
        {
            Destroy(gameObject);
        }
    }

    void CalculateGroundWidth()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            groundWidth = renderer.bounds.size.x;
        }
        else
        {
            Tilemap tilemap = GetComponentInChildren<Tilemap>();
            if (tilemap != null)
            {
                groundWidth = tilemap.cellBounds.size.x * tilemap.cellSize.x;
            }
            else
            {
                groundWidth = 10f;
                Debug.LogWarning($"{gameObject.name} fallback width = 10");
            }
        }
    }

    float GetRightEdge()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null) return renderer.bounds.max.x;

        Tilemap tilemap = GetComponentInChildren<Tilemap>();
        if (tilemap != null) return transform.position.x + tilemap.localBounds.max.x;

        return transform.position.x + groundWidth;
    }

    void SpawnNextGround()
    {
        if (groundPrefabs == null || groundPrefabs.Length == 0)
        {
            Debug.LogWarning("Chưa gán groundPrefabs!");
            return;
        }

        int rand = Random.Range(0, groundPrefabs.Length);
        GameObject newGround = Instantiate(groundPrefabs[2]);

        float currentRightEdge = GetRightEdge(new GameObject[] { gameObject });
        float newGroundLeftEdge = GetLeftEdge(newGround);

        float offset = currentRightEdge - newGroundLeftEdge;
        newGround.transform.position += new Vector3(offset, 0, 0);

        Debug.Log($"Spawned: {newGround.name} | Dịch offset: {offset}");

        LoopingGround script = newGround.GetComponent<LoopingGround>();
        if (script != null)
        {
            script.groundPrefabs = groundPrefabs;
        }
    }

    float GetRightEdge(GameObject[] objs)
    {
        float max = float.MinValue;
        foreach (GameObject obj in objs)
        {
            Renderer renderer = obj.GetComponentInChildren<Renderer>();
            if (renderer != null)
                max = Mathf.Max(max, renderer.bounds.max.x);

            Tilemap tilemap = obj.GetComponentInChildren<Tilemap>();
            if (tilemap != null)
                max = Mathf.Max(max, tilemap.localBounds.max.x + obj.transform.position.x);
        }
        return max;
    }

    float GetLeftEdge(GameObject obj)
    {
        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer != null)
            return renderer.bounds.min.x;

        Tilemap tilemap = obj.GetComponentInChildren<Tilemap>();
        if (tilemap != null)
            return tilemap.localBounds.min.x + obj.transform.position.x;

        return obj.transform.position.x;
    }

}
