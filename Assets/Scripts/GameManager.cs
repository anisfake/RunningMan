using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float gameSpeed = 3f;
    [SerializeField]
    private float speedIncrease = 0.15f;
    [SerializeField] private TextMeshProUGUI scoreText;
    private float score = 0;
    [SerializeField] private GameObject scoreTextObject;
    [SerializeField] private GameObject gameOverMess;
    [SerializeField] private GameObject backToMenuButton;

    private int currentCoin = 0;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject coinTextObject;
    private bool isGameOver = false;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject skillSelectionPanel;
    private bool hasShownSkillChoice = false;
    private float originalGameSpeed;

    private void Awake()
    {
        Time.timeScale = 1;

        if (instance == null)
        {
            instance = this;
        }
        originalGameSpeed = gameSpeed;
    }
    public float GetGameSpeed()
    {
        float maxSpeed = 12f;
        if (gameSpeed > maxSpeed)
        {
            gameSpeed = maxSpeed;
        }
        return gameSpeed;
    }
    public void SlowDownGame()
    {
        gameSpeed = originalGameSpeed * 0.3f;
        Debug.Log("game speed da giam: " + gameSpeed);
    }

    public void ResetGameSpeed()
    {
        gameSpeed = originalGameSpeed;
        Debug.Log("tro lai toc do game binh thuong: " + gameSpeed);
    }
    void Start()
    {
        currentCoin = 0;
        UpdateCoin();
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

        if (!hasShownSkillChoice && score >= 50f)
        {
            hasShownSkillChoice = true;
            ShowSkillSelection();

        }
    }
    public void HandleStartGame()
    {
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
        backToMenuButton.SetActive(true);
        Time.timeScale = 0;

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
    public void SelectSkill(PlayerSkillController.SkillType newSkill)
    {
        PlayerSkillController player = FindFirstObjectByType<PlayerSkillController>();

        if (player != null)
        {
            player.skill = newSkill;
            int current = PlayerPrefs.GetInt("Skill_Uses_" + newSkill.ToString(), 1);
            PlayerPrefs.SetInt("Skill_Uses_" + newSkill.ToString(), current + 1);
            Debug.Log("new skill:" + newSkill);
        }

        skillSelectionPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void SelectMagnetSkill() => SelectSkill(PlayerSkillController.SkillType.Magnet);
    public void SelectShieldSkill() => SelectSkill(PlayerSkillController.SkillType.Shield);
    public void SelectCoinBoostSkill() => SelectSkill(PlayerSkillController.SkillType.CoinBoost);
    public void SelectSlowTimeSkill() => SelectSkill(PlayerSkillController.SkillType.SlowTime);
    private void ShowSkillSelection()
    {
        skillSelectionPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void LoadMenuUIScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}