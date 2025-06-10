using System;
using UnityEngine;

public class Bullet : PoolAble
{
    public string weaponName { get; set; } = "Bullet";
    private bool _returned = false;
    public bool BackUp { get; set; } = false;
    public float lifetime { get; set; } = 2.0f;
    public float spawnTime { get; set; }
    public float Damage { get; set; } = 10f;     
    public float Speed { get; set; } = 10.0f;
    
    /// <summary>
    /// 아직 할당 못한 부분
    /// </summary>
    public float Size { get; set; } = 1;
    public int Count { get; set; } = 1;
    
    public int Level { get; set; } = 0;
    
    private Vector3 direction = Vector3.up;
    public virtual void OnEnable() {
        // 생성된 시간을 현재 시간으로 설정
        spawnTime = Time.time;
        
        RectTransform rt = GetComponent<RectTransform>();
        _returned = false;
        if (rt != null)
            rt.transform.localScale = new Vector2(Size, Size);
    }

    public virtual void Update()
    {
        if (_returned || BackUp)
            return;

        // 수명 종료 시 반환
        if (Time.time - spawnTime > lifetime)
        {   
            _returned = true;
            ReleaseObject();
            return;
        }
        
        // 위쪽 방향으로 이동
        this.transform.Translate(direction * Speed * Time.deltaTime);
    }
    
    public override void SetDirection(Vector3 newDirection) {
        direction = newDirection.normalized; // 방향 벡터를 정규화
    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Monster")
        {
            other.gameObject.GetComponent<Monster>().TakeDamage(Damage);
            if(!_returned)
                ReleaseObject();
            _returned = true;
        }
    }
}