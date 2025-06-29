using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Vector2 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        ParralaxScroll();
    }
    private void ParralaxScroll()
    {
        float newX = Mathf.Sin(Time.time * 0.5f) * 0.5f;
        transform.position = startPos + new Vector2(newX, 0);
    }
}
