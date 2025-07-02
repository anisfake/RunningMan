using UnityEngine;

[System.Serializable]
public class MapOption
{
    [Header("Map Info")]
    public string mapName;
    public GameObject[] randomGroundPrefabs;
    public GameObject backgroundPrefab; // Thêm background cho mỗi map

    [Header("Obstacles")]
    public GameObject[] airObstacles;    // Chỉ giữ lại chướng ngại vật trên không
    [Range(0f, 1f)] public float obstacleSpawnChance = 0.3f; // Tỷ lệ sinh chướng ngại vật

    [Header("Coins")]
    public GameObject coinPrefab; // Prefab đồng xu
    [Range(0f, 1f)] public float coinSpawnChance = 0.4f; // Tỷ lệ sinh đồng xu
    public int minCoinsPerGroup = 1; // Số coin tối thiểu trong 1 nhóm
    public int maxCoinsPerGroup = 5; // Số coin tối đa trong 1 nhóm
    public float coinSpacing = 1f; // Khoảng cách giữa các coin
    public float coinGroundOffset = 0.5f;
}