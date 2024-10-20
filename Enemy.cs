using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    //Quelle-https://www.youtube.com/watch?v=sPiVz1k-fEs
    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        //animation

        if(currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //animation

        print("died");
    }
}

