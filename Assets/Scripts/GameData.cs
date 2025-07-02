using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    [Header("Selected Map Data")]
    public GameObject[] selectedRandomGroundPrefabs;
    public GameObject[] selectedAirObstacles;
    public GameObject selectedCoinPrefab;
    public GameObject selectedBackgroundPrefab; // Thêm background

    [Header("Spawn Settings")]
    [Range(0f, 1f)] public float obstacleSpawnChance = 0.3f;
    [Range(0f, 1f)] public float coinSpawnChance = 0.4f;
    public int minCoinsPerGroup = 1;
    public int maxCoinsPerGroup = 5;
    public float coinSpacing = 1f;

    [Header("Ground Object Settings")]
    [Tooltip("Khoảng cách giữa coin mặt đất và surface")]
    public float coinGroundOffset = 0.8f;

    private void Awake()
    {
        // Đảm bảo chỉ có một instance tồn tại
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại giữa các scene
    }

    // Method để set data từ MapOption
    public void SetMapData(MapOption mapOption)
    {
        selectedRandomGroundPrefabs = mapOption.randomGroundPrefabs;
        selectedAirObstacles = mapOption.airObstacles;
        selectedCoinPrefab = mapOption.coinPrefab;
        selectedBackgroundPrefab = mapOption.backgroundPrefab;

        obstacleSpawnChance = mapOption.obstacleSpawnChance;
        coinSpawnChance = mapOption.coinSpawnChance;
        minCoinsPerGroup = mapOption.minCoinsPerGroup;
        maxCoinsPerGroup = mapOption.maxCoinsPerGroup;
        coinSpacing = mapOption.coinSpacing;
        coinGroundOffset = mapOption.coinGroundOffset;
    }
}