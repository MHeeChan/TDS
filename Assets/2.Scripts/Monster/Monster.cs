using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 2f;
	private double maxHP;
    private double currentHP;
    private bool isMove = true;    
    float maxAngle = 15f;
	float jumpCooldown = 1f;
    float lastJumpTime = -10f;
	[SerializeField] public Slider hpSlider;
    
	Animator anim;
    Rigidbody2D rb;
	GameObject targetBox;
	private bool isJumping = false;
	void ResetJump() => isJumping = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
		maxHP = 50;
		currentHP = maxHP;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            targetBox = collision.gameObject;
            //Debug.Log("Collided with Box");
            anim.SetBool("IsAttacking", true);
        }
	
		if (collision.gameObject.layer == LayerMask.NameToLayer("Hero"))
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
			isMove = true;
        }

		if (collision.gameObject.layer == LayerMask.NameToLayer("Hero"))
        {
            if (targetBox == collision.gameObject)
                targetBox = null; // 현재 타겟 해제
            anim.SetBool("IsAttacking", false);
			isMove = true;
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
                health.TakeDamage(1); // 10만큼 데미지
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

        // if (angle > maxAngle)
        //     rb.AddTorque(-200f); // 왼쪽으로 살짝 돌려주기 (값은 튜닝)
        // else if (angle < -maxAngle)
        //     rb.AddTorque(200f); 

		Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.0f, 1.0f), 0, LayerMask.GetMask("Monster"));

        // 자기 자신 제외한 몬스터 수
        int count = 0;
        foreach (var col in hits)
            if (col.gameObject != gameObject) count++;

        //3마리 이상 겹치면, 쿨타임 체크 후 한 번만 점프
        // if (count >= 1 && Time.time - lastJumpTime > jumpCooldown && isMove)
        // {
        //     Debug.Log("점프준비");
        //     JumpRandom();
        //     lastJumpTime = Time.time;
        // }
    }

    // IEnumerator TemporaryIgnoreCollisionDuringJump(float duration = 1f)
    // {
    //     var collider = GetComponent<Collider2D>();
    //
    //     ForceSend/Receive에서 Monster 제거 (충돌 안 하게)
    //     collider.forceSendLayers &= ~LayerMask.GetMask("Monster");
    //     collider.forceReceiveLayers &= ~LayerMask.GetMask("Monster");
    //
    //     yield return new WaitForSeconds(duration);
    //
    //     다시 Monster 충돌 허용
    //     collider.forceSendLayers |= LayerMask.GetMask("Monster");
    //     collider.forceReceiveLayers |= LayerMask.GetMask("Monster");
    // }
    
    private IEnumerator DelayedJump(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb.AddForce(0.1f * Vector2.up, ForceMode2D.Impulse);

        //StartCoroutine(TemporaryIgnoreCollisionDuringJump()); // 점프 직후 충돌 해제
    }
    
    public void JumpRandom()
    {
        float delay = Random.Range(0f, 5f); // 0~5초 사이 랜덤 시간
        Debug.Log(delay + "초 후 점프");
        StartCoroutine(DelayedJump(delay));
    }

	public void TakeDamage(double damage)
    {
        currentHP -= damage;
        hpSlider.value = (float)(currentHP / maxHP);
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}