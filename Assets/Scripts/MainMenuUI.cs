using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI instance;
    [SerializeField] private TextMeshProUGUI goldTextInMenu;
    [SerializeField] private TextMeshProUGUI goldTextInShop;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TextMeshProUGUI nameGame;
    [SerializeField] private GameObject coinImg;
    [SerializeField] private GameObject SelectPlayerPanel;
    [SerializeField] private GameObject mapSelectionPanel;
    [SerializeField] private GameObject map;// Panel chọn map
    [SerializeField] private MapOption[] mapOptions; // Danh sách MapOption (custom class)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        int gold = PlayerPrefs.GetInt("Gold", 0);
        float highScore = PlayerPrefs.GetFloat("HighScore", 0f);

        Debug.Log("score: " + gold);

        goldTextInMenu.text = "" + gold.ToString();
        goldTextInShop.text = "" + gold.ToString();
        highScoreText.text = "Best: " + Mathf.FloorToInt(highScore) + " m";
    }
    public void OnPlayGameButton()
    {
        gameObject.SetActive(false);
        mapSelectionPanel.SetActive(true);


    }
    public void SelectMap(int mapIndex)
    {
        if (mapIndex < 0 || mapIndex >= mapOptions.Length)
        {
            Debug.LogError("Map index không hợp lệ!");
            return;
        }

        MapOption selectedMap = mapOptions[mapIndex];

        // Kiểm tra xem randomGroundPrefabs có tồn tại không
        if (selectedMap.randomGroundPrefabs == null || selectedMap.randomGroundPrefabs.Length == 0)
        {
            Debug.LogError($"Map {selectedMap.mapName} không có randomGroundPrefabs!");
            return;
        }

        // Kiểm tra background prefab
        if (selectedMap.backgroundPrefab == null)
        {
            Debug.LogWarning($"Map {selectedMap.mapName} không có backgroundPrefab!");
        }

        // Kiểm tra GameData Instance
        if (GameData.Instance == null)
        {
            Debug.LogError("GameData.Instance là null!");
            return;
        }

        // Sử dụng method mới để set toàn bộ data
        GameData.Instance.SetMapData(selectedMap);

        Debug.Log($"Đã set map data cho GameData: {selectedMap.mapName}");
        Debug.Log($"- Ground Prefabs: {selectedMap.randomGroundPrefabs.Length}");
        Debug.Log($"- Air Obstacles: {(selectedMap.airObstacles?.Length ?? 0)}");
        Debug.Log($"- Background: {(selectedMap.backgroundPrefab != null ? selectedMap.backgroundPrefab.name : "None")}");

        map.SetActive(false);
        SceneManager.LoadScene("GameScene");
    }
    public void OnResetDataButton()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Data Reset");

        goldTextInMenu.text = "0";
        highScoreText.text = "Best: 0 m";
    }
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        nameGame.text = "";
        coinImg.SetActive(false);
    }
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        nameGame.text = "Running Man";
        coinImg.SetActive(true);
    }
    public void UpdateCoinUI()
    {
        int coin = PlayerPrefs.GetInt("Gold", 0);
        goldTextInMenu.text = coin.ToString();
        goldTextInShop.text = coin.ToString();
    }
    public void SelectPlayer()
    {
        SelectPlayerPanel.SetActive(true);
    }
    public void ResetUI()
    {
        // Tắt hết các panel không cần thiết
        mapSelectionPanel.SetActive(false);
        shopPanel.SetActive(false);
        SelectPlayerPanel.SetActive(false);
        // Hiện lại panel chính
        this.gameObject.SetActive(true);


    }

}