using UnityEngine;

public class PlayerRecover : MonoBehaviour
{
    [SerializeField] private float targetX = -2f;
    [SerializeField] private float recoverSpeed = 0.15f;
    private bool isBlocked = false;

    private void Update()
    {

        if (!isBlocked && transform.position.x < targetX - 0.01f)
        {
            float newX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * recoverSpeed);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            isBlocked = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            isBlocked = false;
        }
    }
}
