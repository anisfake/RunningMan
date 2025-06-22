using TMPro;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public SkillType skill;
    [SerializeField] private float magnetRange = 3f;
    [SerializeField] private float magnetSpeed = 5f;

    [SerializeField] private float skillActiveDuration = 10f; // thoi gian hieu luc
    [SerializeField] private float skillCooldownDuration = 10f; // thoi gian hoi chieu

    [SerializeField] private TextMeshProUGUI skillStatusText;

    private bool isSkillActive = false;
    private bool isCooldown = false;
    private float skillTimer = 0f;
    private float cooldownTimer = 0f;
    public bool isShieldOn = false;
    public bool isCoinBoost = false;
    public bool isSlowTime = false;

    void Update()
    {
        // 1. hien thi mac dinh
        if (!isSkillActive && !isCooldown)
        {
            skillStatusText.text = "Tap F to use";
        }

        // 2. kich hoat bang nhan F
        if (Input.GetKeyDown(KeyCode.F) && !isSkillActive && !isCooldown && skill != SkillType.None)
        {
            ActivateSkill();
        }

        // 3. khi ky nang dang hoat dong
        if (isSkillActive)
        {
            skillTimer -= Time.deltaTime;
            skillStatusText.text = skill.ToString() + ": " + Mathf.CeilToInt(skillTimer) + "s";

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
                    ActivateSlowTime(10);
                    break;
            }

            if (skillTimer <= 0f)
            {
                DeactivateSkill();
            }
        }

        // 4. Khi cooldown
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            skillStatusText.text = "Cooldown: " + Mathf.CeilToInt(cooldownTimer) + "s";

            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                skillStatusText.text = "Tap F to use";
                Debug.Log("ky nang da san sang");
            }
        }
    }


    void ActivateSkill()
    {
        isSkillActive = true;
        isCooldown = true;
        skillTimer = skillActiveDuration;
        cooldownTimer = skillCooldownDuration;
        Debug.Log("thoi gian su dung la 10 giay");
    }
    public void DeactivateSkill()
    {
        isSkillActive = false;

        if (skill == SkillType.Shield)
        {
            isShieldOn = false;
        }
        else if (skill == SkillType.SlowTime)
        {
            GameManager.instance.ResetGameSpeed();
        }
        else if (skill == SkillType.CoinBoost)
        {
            isCoinBoost = false;
        }
        Debug.Log("da het hieu luc");
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
    void ActivateShield()
    {
        isShieldOn = true;
    }
    void ActivateCoinBoost()
    {
        isCoinBoost = true;
    }
    void ActivateSlowTime(float duration)
    {
        GameManager.instance.SlowDownGame();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
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
