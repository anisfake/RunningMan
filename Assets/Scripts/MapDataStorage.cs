//using UnityEngine;

//public class MapDataStorage : MonoBehaviour
//{
//    public static MapDataStorage Instance { get; private set; }

//    [Header("All Available Maps")]
//    [SerializeField] private MapOption[] allMaps; // Gán tất cả maps trong Inspector

//    [Header("Currently Selected Map Data")]
//    private MapOption currentSelectedMap;
//    private int selectedMapIndex = 0;

//    private void Awake()
//    {
//        // Singleton pattern với DontDestroyOnLoad
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);

//            // Load map index đã lưu (nếu có)
//            selectedMapIndex = PlayerPrefs.GetInt("SelectedMapIndex", 0);

//            // Khôi phục dữ liệu map
//            RestoreMapData();

//            Debug.Log("MapDataStorage created and map data restored");
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    /// <summary>
//    /// Lưu dữ liệu map được chọn
//    /// </summary>
//    public void SaveSelectedMap(int mapIndex)
//    {
//        if (mapIndex < 0 || mapIndex >= allMaps.Length)
//        {
//            Debug.LogError($"Map index {mapIndex} không hợp lệ!");
//            return;
//        }

//        selectedMapIndex = mapIndex;
//        currentSelectedMap = allMaps[mapIndex];

//        // Lưu vào PlayerPrefs
//        PlayerPrefs.SetInt("SelectedMapIndex", selectedMapIndex);
//        PlayerPrefs.Save();

//        // Cập nhật GameData
//        UpdateGameData();

//        Debug.Log($"Đã lưu map: {currentSelectedMap.mapName} (Index: {mapIndex})");
//    }

//    /// <summary>
//    /// Khôi phục dữ liệu map từ PlayerPrefs
//    /// </summary>
//    public void RestoreMapData()
//    {
//        if (allMaps == null || allMaps.Length == 0)
//        {
//            Debug.LogError("Chưa gán allMaps trong MapDataStorage!");
//            return;
//        }

//        // Đảm bảo index hợp lệ
//        if (selectedMapIndex >= allMaps.Length)
//        {
//            selectedMapIndex = 0;
//        }

//        currentSelectedMap = allMaps[selectedMapIndex];

//        // Cập nhật GameData
//        UpdateGameData();

//        Debug.Log($"Đã khôi phục map: {currentSelectedMap.mapName} (Index: {selectedMapIndex})");
//    }

//    /// <summary>
//    /// Cập nhật GameData với dữ liệu map hiện tại
//    /// </summary>
//    private void UpdateGameData()
//    {
//        if (GameData.Instance == null)
//        {
//            Debug.LogWarning("GameData.Instance is null! Tạo GameData object mới.");
//            // Tạo GameData nếu chưa có
//            GameObject gameDataObj = new GameObject("GameData");
//            gameDataObj.AddComponent<GameData>();
//        }

//        if (currentSelectedMap != null)
//        {
//            GameData.Instance.SetMapData(currentSelectedMap);
//            Debug.Log($"Đã cập nhật GameData với map: {currentSelectedMap.mapName}");
//        }
//    }

//    /// <summary>
//    /// Lấy thông tin map hiện tại
//    /// </summary>
//    public MapOption GetCurrentMap()
//    {
//        return currentSelectedMap;
//    }

//    /// <summary>
//    /// Lấy index map hiện tại
//    /// </summary>
//    public int GetCurrentMapIndex()
//    {
//        return selectedMapIndex;
//    }

//    /// <summary>
//    /// Lấy tất cả maps có sẵn
//    /// </summary>
//    public MapOption[] GetAllMaps()
//    {
//        return allMaps;
//    }

//    /// <summary>
//    /// Lấy background cố định của map hiện tại
//    /// </summary>
//    public GameObject GetCurrentBackground()
//    {
//        return currentSelectedMap?.backgroundPrefab;
//    }

//    /// <summary>
//    /// Lấy array ground prefabs để random spawn
//    /// </summary>
//    public GameObject[] GetCurrentRandomGroundPrefabs()
//    {
//        return currentSelectedMap?.randomGroundPrefabs;
//    }

//    /// <summary>
//    /// Lấy array obstacles để random spawn
//    /// </summary>
//    public GameObject[] GetCurrentAirObstacles()
//    {
//        return currentSelectedMap?.airObstacles;
//    }

//    /// <summary>
//    /// Lấy coin prefab
//    /// </summary>
//    public GameObject GetCurrentCoinPrefab()
//    {
//        return currentSelectedMap?.coinPrefab;
//    }

//    /// <summary>
//    /// Lấy random ground prefab từ array
//    /// </summary>
//    public GameObject GetRandomGroundPrefab()
//    {
//        if (currentSelectedMap?.randomGroundPrefabs == null || currentSelectedMap.randomGroundPrefabs.Length == 0)
//            return null;

//        int randomIndex = Random.Range(0, currentSelectedMap.randomGroundPrefabs.Length);
//        return currentSelectedMap.randomGroundPrefabs[randomIndex];
//    }

//    /// <summary>
//    /// Lấy random obstacle prefab từ array
//    /// </summary>
//    public GameObject GetRandomObstaclePrefab()
//    {
//        if (currentSelectedMap?.airObstacles == null || currentSelectedMap.airObstacles.Length == 0)
//            return null;

//        int randomIndex = Random.Range(0, currentSelectedMap.airObstacles.Length);
//        return currentSelectedMap.airObstacles[randomIndex];
//    }

//    /// <summary>
//    /// Kiểm tra dữ liệu có hợp lệ không
//    /// </summary>
//    public bool IsDataValid()
//    {
//        return currentSelectedMap != null &&
//               currentSelectedMap.randomGroundPrefabs != null &&
//               currentSelectedMap.randomGroundPrefabs.Length > 0;
//    }

//    /// <summary>
//    /// Debug thông tin map hiện tại
//    /// </summary>
//    [ContextMenu("Debug Current Map")]
//    public void DebugCurrentMap()
//    {
//        if (currentSelectedMap != null)
//        {
//            Debug.Log($"=== Current Map Info ===");
//            Debug.Log($"Name: {currentSelectedMap.mapName}");
//            Debug.Log($"Random Ground Prefabs for spawning: {currentSelectedMap.randomGroundPrefabs?.Length ?? 0}");

//            // Debug từng ground prefab
//            if (currentSelectedMap.randomGroundPrefabs != null)
//            {
//                for (int i = 0; i < currentSelectedMap.randomGroundPrefabs.Length; i++)
//                {
//                    Debug.Log($"  - Ground {i}: {currentSelectedMap.randomGroundPrefabs[i]?.name ?? "null"}");
//                }
//            }

//            Debug.Log($"Fixed Background: {currentSelectedMap.backgroundPrefab?.name ?? "null"}");
//            Debug.Log($"Air Obstacles for random spawn: {currentSelectedMap.airObstacles?.Length ?? 0}");
//            Debug.Log($"Coin Prefab: {currentSelectedMap.coinPrefab?.name ?? "null"}");
//            Debug.Log($"Obstacle Spawn Chance: {currentSelectedMap.obstacleSpawnChance:F2}");
//            Debug.Log($"Coin Spawn Chance: {currentSelectedMap.coinSpawnChance:F2}");
//        }
//        else
//        {
//            Debug.Log("No map selected!");
//        }
//    }
//}