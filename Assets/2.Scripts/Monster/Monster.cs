using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 2f;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    void FixedUpdate()
    {
        float heroX = GameManager.Instance.Hero.transform.position.x;
        float myX = rb.position.x;
        
        rb.velocity = new Vector2(-1 * moveSpeed, rb.velocity.y);
    }
}