using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer = 10000;
    //public float attackRange
    public LayerMask enemyLayer;
    public Transform attackCornerOne;
    public Transform attackCornerTwo;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && cooldownTimer > attackCooldown && playerMovement.canAttack())
        {
            attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void attack()
    {
        //Cooldown
        cooldownTimer = 0;

        //animation abspielen

        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(attackCornerOne.position, attackCornerTwo.position, enemyLayer);

        //Damage the Enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            print("hit");       
        }
    }
}
