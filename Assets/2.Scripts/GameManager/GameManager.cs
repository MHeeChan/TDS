using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float moveSpeed = 100f;
    // Start is called before the first frame update
    [SerializeField] public GameObject Hero;
	[SerializeField] public GameObject Truck;
    [SerializeField] public GameObject Lose;
    public bool isMove = true;
	[SerializeField] Rigidbody2D[] Trb;
    private LinkedList<Rigidbody2D> myList;
	Rigidbody2D Hrb;	
    void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 살아남게
        }
        else
        {
            Destroy(gameObject); // 중복 GameManager 제거
        }
    }
    
    void Start()
    {
        Hrb = Hero.GetComponent<Rigidbody2D>();
        myList = new LinkedList<Rigidbody2D>(Trb);
    }

    void DestroyLastBox()
    {
        myList.RemoveLast();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMove)
    {
        Hrb.velocity = new Vector2(moveSpeed, Hrb.velocity.y);
		//Hrb.MovePosition(Hrb.position + Vector2.right * moveSpeed * Time.fixedDeltaTime);
        foreach (var i in myList)
        {
            if(i!=null)
                i.velocity = new Vector2(moveSpeed, i.velocity.y); // i는 현재 반복중인 Rigidbody2D
			//i.MovePosition(i.position + Vector2.right * moveSpeed * Time.fixedDeltaTime);
        }
    }
    }

    void Update()
    {
        if (!Hero.gameObject.activeSelf && !Lose.activeSelf)
        {
            Lose.gameObject.SetActive(true);
        }
    }
}
