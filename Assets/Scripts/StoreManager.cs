using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private string itemId;
    [SerializeField] private int price;

    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text buyButtonText;

    void Start()
    {
        priceText.text = price.ToString();

        buyButton.onClick.AddListener(HandleBuy);
    }

    void HandleBuy()
    {

        if (SpendCoin(price))
        {
            Debug.Log($"mua thanh cong: {itemId}");

            int current = PlayerPrefs.GetInt("Skill_Uses_" + itemId.ToString().ToLower(), 0);

            Debug.Log($"so luong truoc khi mua: {current}");
            Debug.Log($"luu vao playerpref: Skill_Uses_" + itemId.ToString().ToLower());

            PlayerPrefs.SetInt("Skill_Uses_" + itemId.ToString().ToLower(), current + 1);


            int afterBuy = PlayerPrefs.GetInt("Skill_Uses_" + itemId.ToString().ToLower(), 0);
            Debug.Log($"so luong sau khi mua: {afterBuy}");

            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("khong du coin!");
        }
    }
    public bool SpendCoin(int amount)
    {
        int coin = PlayerPrefs.GetInt("Gold", 0);
        if (coin >= amount)
        {
            coin -= amount;
            PlayerPrefs.SetInt("Gold", coin);
            PlayerPrefs.Save();
            MainMenuUI.instance.UpdateCoinUI();
            return true;
        }
        return false;
    }
}
