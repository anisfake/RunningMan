using TMPro;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public SkillType skill = SkillType.None;
    [SerializeField] private float magnetRange = 3f;
    [SerializeField] private float magnetSpeed = 5f;

    [SerializeField] private float skillActiveDuration = 10f; // thoi gian hieu luc
    [SerializeField] private float skillCooldownDuration = 60f; // thoi gian hoi chieu

    [SerializeField] private TextMeshProUGUI skillStatusText;

    private bool isSkillActive = false;
    private bool isCooldown = false;
    private float skillTimer = 0f;
    private float cooldownTimer = 0f;

    void Update()
    {
        skillStatusText.text = "Tap F to use";
        if (Input.GetKeyDown(KeyCode.F) && skill == SkillType.Magnet && !isSkillActive && !isCooldown)
        {
            ActivateSkill();
        }

        if (isSkillActive)
        {
            skillStatusText.text = "Magnet: " + Mathf.CeilToInt(skillTimer) + "s";

            skillTimer -= Time.deltaTime;
            AttractCoins();

            if (skillTimer <= 0f)
            {
                DeactivateSkill();
            }
        }

        if (isCooldown)
        {
            skillStatusText.text = "Cooldown: " + Mathf.CeilToInt(cooldownTimer) + "s";

            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                skillStatusText.text = "Tap F to use";
                isCooldown = false;
                Debug.Log("san sang dung lai.");
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
    void DeactivateSkill()
    {
        isSkillActive = false;
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
        Dash,
    }

}
