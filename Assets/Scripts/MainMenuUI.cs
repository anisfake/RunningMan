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
        Time.timeScale = 1;
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
}
