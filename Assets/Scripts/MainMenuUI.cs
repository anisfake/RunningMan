using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI highScoreText;
    void Start()
    {
        int gold = PlayerPrefs.GetInt("Gold", 0);
        float highScore = PlayerPrefs.GetFloat("HighScore", 0f);

        Debug.Log("score: " + gold);

        goldText.text = "Gold: " + gold.ToString();
        highScoreText.text = "Best: " + Mathf.FloorToInt(highScore) + " m";
    }
    public void OnPlayGameButton()
    {
        SceneManager.LoadScene("GameScene");
        GameManager.instance.HandleStartGame();
    }

    public void OnResetDataButton()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Data Reset");


        goldText.text = "Gold: 0";
        highScoreText.text = "Best: 0 m";
    }
}
