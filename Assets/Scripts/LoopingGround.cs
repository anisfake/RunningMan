// Improved LoopingGround with Pre-spawn system (No Ground Obstacles)
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class LoopingGround : MonoBehaviour
{
    [Header("Ground Settings")]
    [SerializeField] private GameObject[] randomGroundPrefabs;
    [SerializeField] private float spawnDistance = 15f; // Tăng lên để spawn xa trước
    [SerializeField] private float destroyDistance = 2f;
    [SerializeField] private float groundSpacing = 0.1f;

    [Header("Spawn Settings")]
    [SerializeField] private float coinGroundOffset = 0.8f;
    [SerializeField] private float minAirObstacleHeight = 2.5f; // Tăng chiều cao tối thiểu
    [SerializeField] private float maxAirObstacleHeight = 5f;   // Tăng chiều cao tối đa

    private float groundWidth;
    private bool hasSpawnedNext = false;
    private bool isSpawning = false;
    private bool hasSpawnedObjects = false;
    private Camera mainCamera;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Static variables để tránh overlap
    private static float? globalInitialCameraBottom = null;
    private static bool isInitialized = false;
    private static float lastSpawnTime = 0f;
    private static HashSet<string> spawnedGroundNames = new HashSet<string>();

    void Start()
    {
        mainCamera = Camera.main;
        if (GameData.Instance != null && GameData.Instance.selectedRandomGroundPrefabs != null && GameData.Instance.selectedRandomGroundPrefabs.Length > 0)
        {
            randomGroundPrefabs = GameData.Instance.selectedRandomGroundPrefabs;
        }
        else
        {
            Debug.LogError("randomGroundPrefabs chưa được gán!");
            enabled = false;
            return;
        }

        hasSpawnedNext = false;
        isSpawning = false;
        hasSpawnedObjects = false;

        if (mainCamera == null)
        {
            Debug.LogError("Không tìm thấy Main Camera!");
            return;
        }

        if (!isInitialized)
        {
            globalInitialCameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
            isInitialized = true;
        }

        CalculateGroundWidth();

        // Spawn objects ngay lập tức cho ground hiện tại
        StartCoroutine(DelayedSpawnObjects());
    }

    IEnumerator DelayedSpawnObjects()
    {
        yield return new WaitForSeconds(0.1f);
        if (!hasSpawnedObjects && GameData.Instance != null)
        {
            SpawnObjectsOnCurrentGround();
            hasSpawnedObjects = true;
        }
    }

    void Update()
    {
        if (mainCamera == null) return;

        float cameraRight = mainCamera.transform.position.x + mainCamera.orthographicSize * mainCamera.aspect;
        float cameraLeft = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;
        float rightEdge = GetRightEdge();
        float currentTime = Time.time;

        // Spawn ground mới khi còn cách xa camera (pre-spawn)
        bool canSpawn = !hasSpawnedNext && !isSpawning &&
                       rightEdge <= cameraRight + spawnDistance &&
                       (currentTime - lastSpawnTime) > 0.1f &&
                       !spawnedGroundNames.Contains(gameObject.name + "_spawned");

        if (canSpawn)
        {
            hasSpawnedNext = true;
            isSpawning = true;
            lastSpawnTime = currentTime;
            spawnedGroundNames.Add(gameObject.name + "_spawned");
            SpawnNextGround();
        }

        // Destroy khi đã ra khỏi camera
        if (hasSpawnedNext && rightEdge < cameraLeft - destroyDistance)
        {
            spawnedGroundNames.Remove(gameObject.name + "_spawned");
            DestroySpawnedObjects();
            Destroy(gameObject);
        }
    }

    void CalculateGroundWidth()
    {
        Tilemap tilemap = GetComponentInChildren<Tilemap>();
        if (tilemap != null)
        {
            BoundsInt cellBounds = tilemap.cellBounds;
            groundWidth = cellBounds.size.x * tilemap.cellSize.x;
        }
        else
        {
            Renderer renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                groundWidth = renderer.bounds.size.x;
            }
            else
            {
                groundWidth = 10f;
            }
        }
    }

    float GetRightEdge()
    {
        Tilemap tilemap = GetComponentInChildren<Tilemap>();
        if (tilemap != null)
            return tilemap.transform.TransformPoint(tilemap.localBounds.max).x;

        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
            return renderer.bounds.max.x;

        return transform.position.x + groundWidth / 2f;
    }

    void SpawnNextGround()
    {
        if (!hasSpawnedNext || isSpawning == false) return;

        if (randomGroundPrefabs == null || randomGroundPrefabs.Length == 0)
        {
            isSpawning = false;
            return;
        }

        try
        {
            int randomIndex = Random.Range(0, randomGroundPrefabs.Length);
            GameObject prefab = randomGroundPrefabs[randomIndex];

            if (prefab == null)
            {
                isSpawning = false;
                return;
            }

            GameObject newGround = Instantiate(prefab);
            newGround.name = prefab.name + "_" + Time.time.ToString("F2");

            float currentRightEdge = GetRightEdge();
            StartCoroutine(PositionNewGroundAfterFrame(newGround, currentRightEdge));
        }
        catch
        {
            isSpawning = false;
        }
    }

    System.Collections.IEnumerator PositionNewGroundAfterFrame(GameObject newGround, float currentRightEdge)
    {
        yield return new WaitForEndOfFrame();

        try
        {
            // Position ground
            float newGroundLeftEdge = GetLeftEdge(newGround);
            float targetX = currentRightEdge + groundSpacing - newGroundLeftEdge + newGround.transform.position.x;
            float newGroundBottom = GetBottomEdge(newGround);
            float targetY = globalInitialCameraBottom.Value - newGroundBottom + newGround.transform.position.y;

            newGround.transform.position = new Vector3(targetX, targetY, 0);

            // Setup script cho ground mới
            LoopingGround newScript = newGround.GetComponent<LoopingGround>();
            if (newScript != null)
            {
                newScript.randomGroundPrefabs = randomGroundPrefabs;
                newScript.spawnDistance = spawnDistance;
                newScript.destroyDistance = destroyDistance;
                newScript.groundSpacing = groundSpacing;
                newScript.hasSpawnedNext = false;
                newScript.isSpawning = false;
                newScript.hasSpawnedObjects = false;

                // Spawn objects cho ground mới NGAY LẬP TỨC (vì nó được tạo ngoài tầm nhìn)
                yield return new WaitForSeconds(0.1f);
                newScript.SpawnObjectsOnCurrentGround();
                newScript.hasSpawnedObjects = true;
            }
        }
        finally
        {
            isSpawning = false;
        }
    }

    float GetLeftEdge(GameObject obj)
    {
        if (obj == null) return 0f;

        Tilemap tilemap = obj.GetComponentInChildren<Tilemap>();
        if (tilemap != null)
            return tilemap.transform.TransformPoint(tilemap.localBounds.min).x;

        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer != null)
            return renderer.bounds.min.x;

        Collider2D collider = obj.GetComponentInChildren<Collider2D>();
        if (collider != null)
            return collider.bounds.min.x;

        return obj.transform.position.x - 5f;
    }

    float GetBottomEdge(GameObject obj)
    {
        if (obj == null) return 0f;

        Tilemap tilemap = obj.GetComponentInChildren<Tilemap>();
        if (tilemap != null)
            return tilemap.transform.TransformPoint(tilemap.localBounds.min).y;

        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer != null)
            return renderer.bounds.min.y;

        return obj.transform.position.y;
    }

    // === IMPROVED COIN & AIR OBSTACLE PLACEMENT (NO GROUND OBSTACLES) ===
    void SpawnObjectsOnCurrentGround()
    {
        if (GameData.Instance == null || hasSpawnedObjects) return;

        float leftEdge = GetLeftEdge(gameObject);
        float rightEdge = GetRightEdge();
        float yRayHeight = 5f;
        float yRayOffset = 2f;
        float stepSize = 0.3f;

        List<Vector3> groundPoints = new List<Vector3>();

        // Raycast để tìm surface chính xác
        for (float x = leftEdge + stepSize; x <= rightEdge - stepSize; x += stepSize)
        {
            Vector3 rayOrigin = new Vector3(x, transform.position.y + yRayOffset + yRayHeight, 0);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, yRayHeight * 2f);

            if (hit.collider != null && IsPartOfThisGround(hit.collider.gameObject))
            {
                groundPoints.Add(new Vector3(x, hit.point.y, 0));
            }
            else
            {
                // Fallback: tilemap
                Tilemap tilemap = GetComponentInChildren<Tilemap>();
                if (tilemap != null)
                {
                    Vector3Int cell = tilemap.WorldToCell(new Vector3(x, transform.position.y, 0));
                    if (tilemap.HasTile(cell))
                    {
                        Vector3 world = tilemap.CellToWorld(cell);
                        world.y += tilemap.cellSize.y;
                        groundPoints.Add(new Vector3(x, world.y, 0));
                    }
                }
            }
        }

        if (groundPoints.Count == 0) return;

        // Chỉ spawn coins và air obstacles - không có ground obstacles
        SpawnCoinsOnSurface(groundPoints);
        SpawnAirObstacles(groundPoints);

        hasSpawnedObjects = true;
    }

    void SpawnCoinsOnSurface(List<Vector3> groundPoints)
    {
        if (GameData.Instance.selectedCoinPrefab == null) return;

        int coinCount = Random.Range(GameData.Instance.minCoinsPerGroup, GameData.Instance.maxCoinsPerGroup + 1);
        int startIdx = Random.Range(0, Mathf.Max(1, groundPoints.Count - coinCount + 1));

        for (int i = 0; i < coinCount && startIdx + i < groundPoints.Count; i++)
        {
            Vector3 ground = groundPoints[startIdx + i];
            Vector3 spawn = ground + Vector3.up * coinGroundOffset;
            GameObject coin = Instantiate(GameData.Instance.selectedCoinPrefab, spawn, Quaternion.identity);
            spawnedObjects.Add(coin);
        }
    }

    void SpawnAirObstacles(List<Vector3> groundPoints)
    {
        if (GameData.Instance.selectedAirObstacles == null || GameData.Instance.selectedAirObstacles.Length == 0) return;

        float spawnChance = GameData.Instance.obstacleSpawnChance * 0.5f;
        if (Random.value > spawnChance) return;

        int idx = Random.Range(0, groundPoints.Count);
        Vector3 basePt = groundPoints[idx];

        // Cải thiện vị trí chướng ngại vật trên cao - đảm bảo không quá thấp
        float airHeight = Random.Range(minAirObstacleHeight, maxAirObstacleHeight);
        Vector3 airPt = new Vector3(basePt.x, basePt.y + airHeight, 0);

        GameObject prefab = GameData.Instance.selectedAirObstacles[Random.Range(0, GameData.Instance.selectedAirObstacles.Length)];
        GameObject obs = Instantiate(prefab, airPt, Quaternion.identity);
        spawnedObjects.Add(obs);
    }

    bool IsPartOfThisGround(GameObject obj)
    {
        return obj.transform.IsChildOf(transform) || obj == gameObject;
    }

    void DestroySpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedObjects.Clear();
    }

    public static void ResetInitialCameraBottom()
    {
        globalInitialCameraBottom = null;
        isInitialized = false;
        lastSpawnTime = 0f;
        spawnedGroundNames.Clear();
    }

    void OnDestroy()
    {
        DestroySpawnedObjects();
    }
}