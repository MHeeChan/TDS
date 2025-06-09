using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 2f;
    Animator anim;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            anim.SetBool("IsAttacking", true);
        }
    }

    void OnAttack()
    {
        Debug.Log("ATTACK");
    }

    void FixedUpdate()
    {
        float heroX = GameManager.Instance.Hero.transform.position.x;
        float myX = rb.position.x;
        rb.velocity = new Vector2(-1 * moveSpeed, rb.velocity.y);
    }
}