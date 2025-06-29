using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Sprite[] previewSprites;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image previewImage;

    [Header("UI")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button selectButton;

    private int currentIndex = 0;

    void Start()
    {
        UpdateUI();

        leftButton.onClick.AddListener(() =>
        {
            currentIndex = (currentIndex - 1 + characterPrefabs.Length) % characterPrefabs.Length;
            UpdateUI();
        });

        rightButton.onClick.AddListener(() =>
        {
            currentIndex = (currentIndex + 1) % characterPrefabs.Length;
            UpdateUI();
        });

        selectButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
            PlayerPrefs.Save();
            Debug.Log("da chon nhan vat: " + currentIndex);

            SceneManager.LoadScene("MainMenu");
        });
    }

    void UpdateUI()
    {
        nameText.text = "nhan vat " + (currentIndex + 1);
        previewImage.sprite = previewSprites[currentIndex];
    }
}
