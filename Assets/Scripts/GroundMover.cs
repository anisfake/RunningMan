using UnityEngine;

public class GroundMover : MonoBehaviour
{

    [SerializeField] private float destroyX = -20f;
    [SerializeField] private Transform player;
    private bool gameOverTriggered = false;
    void Update()
    {
        transform.position += Vector3.left * GameManager.instance.GetGameSpeed() * Time.deltaTime;

        float rightEdgeX = transform.position.x + GetComponentInChildren<Renderer>().bounds.size.x;

        if (rightEdgeX < destroyX)
        {
            Destroy(gameObject);
        }

        if (player != null && !gameOverTriggered && player.position.x < destroyX)
        {
            GameManager.instance.GameOver();
        }
    }

}
