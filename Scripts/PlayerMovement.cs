using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float walkSpeed = 2f;    
    public float runSpeed = 4f;
    public float jumpForce = 4f;

    // Yukarıdaki float değerleri character inspector'daki character movement değerleri tarafından override'a
    // uğruyor o yüzden buranın bir mantığı yok. Sebebini anlamadım ama inspector'dan değiştirmek en sağlıklısı.

    private bool isGrounded = true;
    private bool allowRun = false;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
{
    float moveX = Input.GetAxisRaw("Horizontal");
    bool isRunKeyPressed = Input.GetKey(KeyCode.X);

    if (moveX != 0 && !allowRun && !isRunKeyPressed)
    {
        allowRun = true;
    }

    bool isAllowedToRun = allowRun && isRunKeyPressed;
    float currentSpeed = isAllowedToRun ? runSpeed : walkSpeed;

    rb.linearVelocity = new Vector2(moveX * currentSpeed, rb.linearVelocity.y);
    animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

    if (moveX < 0)
        spriteRenderer.flipX = true;
    else if (moveX > 0)
        spriteRenderer.flipX = false;

    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetTrigger("Jump");
        isGrounded = false;
        animator.SetBool("isGrounded", false);
    }

    // Light Attack (E)
    if (Input.GetKeyDown(KeyCode.E))
    {
        animator.speed = 1.5f;
        animator.SetTrigger("Attack1");
    }

    // Heavy Attack (R)
    if (Input.GetKeyDown(KeyCode.R))
    {
        allowRun = false;
        animator.speed = 0.5f; // daha yavaş oynat
        animator.SetTrigger("Attack3");

        // Belirli bir süre sonra normale dön
        Invoke(nameof(ResetAnimatorSpeed), 1.5f); // süreyi animasyon süresine göre ayarla
    }

    if (Input.GetKeyDown(KeyCode.Q))
    {
        allowRun = false;
        animator.SetTrigger("Shield");
    }

    if (moveX == 0 && !isRunKeyPressed && rb.linearVelocity.x == 0)
    {
        allowRun = false;
    }
}

void ResetAnimatorSpeed()
{
    animator.speed = 1f;
}


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
    }
}
