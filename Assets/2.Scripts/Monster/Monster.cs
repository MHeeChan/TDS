using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 2f;
    Animator anim;
    Rigidbody2D rb;

    private bool isMove = true;
    GameObject targetBox;
    float maxAngle = 30f;
    
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
            targetBox = collision.gameObject;
            //Debug.Log("Collided with Box");
            anim.SetBool("IsAttacking", true);
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            if (targetBox == collision.gameObject)
                targetBox = null; // 현재 타겟 해제
            anim.SetBool("IsAttacking", false);
        }
    }

    void OnAttack()
    {
        if (targetBox != null)
        {
            var health = targetBox.GetComponent<BoxHP>();
            Debug.Log(health);
            if (health != null)
            {
                Debug.Log("damage Box");
                health.TakeDamage(20); // 10만큼 데미지
                isMove = false;
            }
        }
    }

    void FixedUpdate()
    {
        if(isMove){
            float heroX = GameManager.Instance.Hero.transform.position.x;
            float myX = rb.position.x;
            rb.velocity = new Vector2(-1 * moveSpeed, rb.velocity.y);
        }
        
        float angle = transform.eulerAngles.z;
        if (angle > 180f) angle -= 360f;

        if (angle > maxAngle)
            rb.AddTorque(-50f); // 왼쪽으로 살짝 돌려주기 (값은 튜닝)
        else if (angle < -maxAngle)
            rb.AddTorque(50f); 
    }
}