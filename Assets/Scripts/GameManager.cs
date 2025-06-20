using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerSkillController;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float gameSpeed = 5f;
    [SerializeField]
    private float speedIncrease = 0.15f;
    [SerializeField] private TextMeshProUGUI scoreText;
    private float score = 0;
    [SerializeField] private GameObject scoreTextObject;
    [SerializeField] private GameObject gameOverMess;

    private int currentCoin = 0;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject coinTextObject;
    private bool isGameOver = false;
    [SerializeField] private GameObject player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public float GetGameSpeed()
    {
        return gameSpeed;
    }
    void Start()
    {
        currentCoin = 0;
        UpdateCoin();
        PlayerSkillController player = FindFirstObjectByType<PlayerSkillController>();
        if (player != null)
        {
            player.skill = SkillType.Magnet;
        }
        else
        {
            Debug.LogWarning("not find!");
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            UpdateGameSpeed();
            UpdateScore();
            UpdateCoin();
            HandleStartGame();
        }
    }
    private void UpdateGameSpeed()
    {
        gameSpeed += Time.deltaTime * speedIncrease;
    }
    private void UpdateScore()
    {
        score += Time.deltaTime * 10;
        scoreText.text = "Score:" + Mathf.FloorToInt(score);
    }
    public void HandleStartGame()
    {
        Time.timeScale = 1;
        scoreTextObject.SetActive(true);
        coinTextObject.SetActive(true);
        gameOverMess.SetActive(false);

    }

    public void AddCoin(int coins)
    {
        currentCoin += coins;
        UpdateCoin();
    }
    private void UpdateCoin()
    {
        coinText.text = "Coin:" + currentCoin.ToString();
    }
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GameOver - currentCoin: " + currentCoin);

        gameOverMess.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(ReLoadScene());

        int totalCoin = PlayerPrefs.GetInt("Gold", 0);
        totalCoin += currentCoin;
        PlayerPrefs.SetInt("Gold", totalCoin);

        float oldHighScore = PlayerPrefs.GetFloat("HighScore", 0);
        if (score > oldHighScore)
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }

        PlayerPrefs.Save();
    }
    private IEnumerator ReLoadScene()
    {
        yield return new WaitForSecondsRealtime(10f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
