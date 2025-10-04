using UnityEngine;

public class PLayer : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(horizontal, vertical).normalized;
        rb.linearVelocity = move * speed;
    }
}
