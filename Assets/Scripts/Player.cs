using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 15f;
    private Rigidbody2D rb;
    private bool isGrounded;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.2f;
    [SerializeField]
    private LayerMask groundLayer;
    private Animator animator;
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private CapsuleCollider2D capsuleCollider;


    private int jumpCount = 0;
    private int maxJumpCount = 2;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider.enabled = true;
        capsuleCollider.enabled = false;
    }

    void Update()
    {
        isGrounded = CheckIfGrounded();
        checkPlayerInBound();
        HandleJump();
        HandleBow();
        HandleSoundEffect();
    }
    private bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;

        }

        if (isGrounded)
        {
            jumpCount = 0;
        }
    }

    private void HandleBow()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            boxCollider.enabled = false;
            capsuleCollider.enabled = true;
            animator.SetBool("isBow", true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            boxCollider.enabled = true;
            capsuleCollider.enabled = false;
            animator.SetBool("isBow", false);
        }
    }
    private void HandleSoundEffect()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            AudioManager.instance.PlayJumpClip();
        }
        if (isGrounded && !AudioManager.instance.HasPlayEffectSound())
        {
            AudioManager.instance.PlayTapClip();
            AudioManager.instance.SetHasPlayEffectSound(true);
        }
        else if (!isGrounded)
        {
            AudioManager.instance.SetHasPlayEffectSound(false);
        }
    }

    private void checkPlayerInBound()
    {
        float cameraLeft = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;

        if (transform.position.x < cameraLeft)
        {
            GameManager.instance.GameOver();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            AudioManager.instance.PlayHurtClip();

        }


        if (collision.CompareTag("Water"))
        {
            Debug.Log("nước → chết");
            GameManager.instance.GameOver();
        }

        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Bú bú ");
            GameManager.instance.AddCoin(1);
        }


    }
}
