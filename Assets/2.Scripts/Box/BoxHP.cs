using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoxHP : MonoBehaviour
{
    private double maxHP;
    private double currentHP;
    [SerializeField] public Slider hpSlider;
    // Start is called before the first frame update
    void Start()
    {
        maxHP = 100;
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
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
