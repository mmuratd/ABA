using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using Unity.Netcode;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerCombat : NetworkBehaviour,IDamageable
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] private float dodgeDuration = 8.0f / 14.0f;
    [SerializeField] float health = 100;
    BoxCollider attackPointCollider;

    private bool isHit;
    private Rigidbody2D myRigidBody;
    private bool isTouchingGround = false;
    private bool isDodging = false;
    private float dodgeCurrentTime;

    private Sensor_HeroKnight groundSensor;

    Animator animator;
    LayerMask enemyLayers;

    //Attack parametreleri
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private bool canAttack = true;
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float playerHealth = 100f;
    private bool isDead;

    public void Damage(int damageAmount)
    {
        health--;
        animator.SetTrigger("Hurt");
        isHit = true;

        if (health<1)
        {
            GetComponent<Collider2D> ().enabled = false;
            isDead = true;
            animator.SetTrigger("die");
            Destroy(gameObject,30f);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        attackPointCollider = gameObject.GetComponentInChildren<BoxCollider>();

    }
 
    void Update()
    {
       
        if (!IsOwner) return;
        // Increase timer that controls attack combo
        timeSinceAttack += Time.deltaTime;

        //Movement
        HandleMovement();

        //Attack
        if (timeSinceAttack > 0.25f) canAttack = true; 
        if (Input.GetMouseButtonDown(0) && canAttack) Attack();

        // Block
        else if (Input.GetMouseButtonDown(1)) Block();
        else if (Input.GetMouseButtonUp(1)) animator.SetBool("IdleBlock", false);//Release Block

        //Jump
        else if (Input.GetKeyDown("space")) 
        {
            animator.SetTrigger("Jump");
            isTouchingGround = false;
            animator.SetBool("Grounded", isTouchingGround);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, m_jumpForce);
        }
    }

    void HandleMovement()
    {
        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");
        myRigidBody.velocity = new Vector2(inputX * m_speed, myRigidBody.velocity.y);//Movement
        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0,0);
            

        }

        else if (inputX < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);

        }
    }
    void Attack() 
    {
        canAttack = false;
        currentAttack++;
       
        // Loop back to one after third attack
        if (currentAttack > 3)
            currentAttack = 1;
       
        // Reset Attack combo if time since last attack is too large
        if (timeSinceAttack > 1.0f)
            currentAttack = 1;
       
        // Call one of three attack animations "Attack1", "Attack2", "Attack3"
        animator.SetTrigger("Attack" + currentAttack);
       
        // Reset timer
        timeSinceAttack = 0.0f;
       
    }

    void Block()
    {

        animator.SetTrigger("Block");
        animator.SetBool("IdleBlock", true);


    }
}
