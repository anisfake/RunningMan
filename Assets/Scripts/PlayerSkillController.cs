using TMPro;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public SkillType skill;
    [SerializeField] private float magnetRange = 3f;
    [SerializeField] private float magnetSpeed = 5f;

    [SerializeField] private TextMeshProUGUI skillStatusText;

    private int remainingUses;

    public bool isShieldOn = false;
    public bool isCoinBoost = false;
    public bool isSlowTime = false;
    private string itemId;

    void Start()
    {
        PlayerSkillController player = FindFirstObjectByType<PlayerSkillController>();
        if (player != null)
        {
            player.skill = SkillType.None;
        }
        else
        {
            Debug.LogWarning("not find!");
        }

        if (skill != SkillType.None)
        {
            itemId = skill.ToString();
            remainingUses = GetSkillUses(itemId.ToLower());
        }
        else
        {
            itemId = "None";
            remainingUses = 0;
        }
        UpdateUI();

    }

    void Update()
    {

        itemId = skill.ToString();

        if (skill == SkillType.None)
        {
            remainingUses = 0;
        }
        else
        {
            remainingUses = GetSkillUses(itemId.ToLower());
        }


        if (remainingUses > 0 && Input.GetKeyDown(KeyCode.F))
        {
            UseSkill();
        }

        UpdateUI();
    }


    void UseSkill()
    {
        remainingUses--;
        SaveSkillUses(itemId, remainingUses);

        Debug.Log("da dung ky nang: " + skill + " | con lai: " + remainingUses);

        switch (skill)
        {
            case SkillType.Magnet:
                AttractCoins();
                break;
            case SkillType.Shield:
                ActivateShield();
                break;
            case SkillType.CoinBoost:
                ActivateCoinBoost();
                break;
            case SkillType.SlowTime:
                ActivateSlowTime();
                break;
        }

        if (remainingUses <= 0)
        {
            skillStatusText.text = "het ky nang";
        }
    }

    void UpdateUI()
    {
        if (skill == SkillType.None)
        {
            skillStatusText.text = "Chua co ky nang";
        }
        else if (remainingUses > 0)
        {
            skillStatusText.text = skill + " - Uses: " + remainingUses;
        }
        else
        {
            skillStatusText.text = "het ky nang";
        }
    }

    void AttractCoins()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Coin"))
            {
                Transform coin = hit.transform;
                coin.position = Vector3.MoveTowards(coin.position, transform.position, magnetSpeed * Time.deltaTime);
            }
        }
    }

    void ActivateShield() => isShieldOn = true;
    void ActivateCoinBoost() => isCoinBoost = true;
    void ActivateSlowTime() => GameManager.instance.SlowDownGame();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }

    // ---- Skill Use Save/Load Logic ----
    int GetSkillUses(string itemId)
    {
        if (itemId.Equals("None")) return 0;
        return PlayerPrefs.GetInt("Skill_Uses_" + itemId.ToString().ToLower(), 1); // default 1
    }

    void SaveSkillUses(string itemId, int amount)
    {
        PlayerPrefs.SetInt("Skill_Uses_" + itemId.ToString().ToLower(), amount);
        PlayerPrefs.Save();
    }

    public enum SkillType
    {
        None,
        Magnet,
        Shield,
        CoinBoost,
        SlowTime
    }
}
