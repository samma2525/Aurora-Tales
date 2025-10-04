using System.Runtime.InteropServices;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Animator anim;

    private Vector3 originalScale;
    private int facingDirectionHorizontal = 1;
    private Vector2 moveInput;
    private bool isMoving = false;
    public CoinManager cm;


    void Start()
    {
        originalScale = transform.localScale;
        transform.localScale = new Vector3(originalScale.x * facingDirectionHorizontal, originalScale.y, originalScale.z);
    }

    void Update()
    {
        bool moving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        isMoving = moving;

        if (isMoving && !anim.GetCurrentAnimatorStateInfo(0).IsName("walk_plauyer"))
        {
            anim.Play("walk_plauyer");
        }
        else if (!isMoving && !anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.Play("idle");
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;

        Vector2 newPos = rb.position + moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        if (horizontal > 0 && facingDirectionHorizontal < 0)
            FlipHori();
        else if (horizontal < 0 && facingDirectionHorizontal > 0)
            FlipHori();
    }

    private void FlipHori()
    {
        facingDirectionHorizontal *= -1;
        transform.localScale = new Vector3(originalScale.x * facingDirectionHorizontal, originalScale.y, originalScale.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            cm.coinCount++;
            Destroy(other.gameObject);
        }
    }


}