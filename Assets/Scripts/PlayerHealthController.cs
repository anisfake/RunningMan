using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public int maxHearts = 3;
    private int currentHearts;


    public GameObject[] heartObjects;

    private void Start()
    {
        currentHearts = maxHearts;
        UpdateHearts();
    }

    public void TakeDamage(int amount)
    {
        currentHearts -= amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
        UpdateHearts();

        if (currentHearts <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < heartObjects.Length; i++)
        {
            heartObjects[i].SetActive(i < currentHearts);
        }
    }
}
