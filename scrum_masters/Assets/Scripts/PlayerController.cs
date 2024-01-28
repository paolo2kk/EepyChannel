using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Animator animator;
    public GameObject world1;
    public GameObject world2;
    GameObject gb;
    Rigidbody2D rb;

    [Header("Movement")]
    public float Xspeed = 2;
    public float Yspeed = 6;
    private bool isGrounded;
    float directionX;
    float directionY;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    private bool isWearingGlasses = false;

    [Header("Combat")]
    public Transform meeleAttackOrigin = null;
    public float meeleAttackRadius = 0.6f;
    public float meleeDamage = 2f;
    public float meeleAttackDelay = 1.1f;
    public LayerMask enemyLayer = 8;
    public KeyCode meleeAtackKey = KeyCode.J;
    private float timeUntilMeleeReadied = 0f;
    private bool attemptMeleeAttack = false;

    void Start()
    {
        gb = GetComponent<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        SwitchWorlds();
        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    private void GetInput()
    {
        attemptMeleeAttack = Input.GetKeyDown(meleeAtackKey);
        directionX = Input.GetAxis("Horizontal");
    }

    void Update()
    {
        

        GetInput();
        HandleMeeleAttack();
        float horizontalInput = Input.GetAxis("Horizontal");
        Flip(horizontalInput);
        //animator.SetBool("isWalking", true);
        Vector3 movement = new Vector3(directionX, 0f, 0f);
        transform.position += movement * Xspeed * Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            //audioManager.PlaySFX(audioManager.Jump);
            //animator.SetBool("isJumping", true);
        }
        else
        {
            //animator.SetBool("isJumping", false);

        }
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isWearingGlasses = !isWearingGlasses;
            SwitchWorlds();
        }
        


    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, Yspeed);
        isGrounded = false;

    }
    void SwitchWorlds()
    {
        world1.SetActive(!isWearingGlasses);
        world2.SetActive(isWearingGlasses);
        EventManager.Instance.WorldSwitch();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

    }

    private void HandleMeeleAttack()
    {
        if(attemptMeleeAttack && timeUntilMeleeReadied <= 0)
        {
            Debug.Log("aaaaa");
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(meeleAttackOrigin.position, meeleAttackRadius, enemyLayer);
            for(int i = 0; i < overlappedColliders.Length; i++)
            {
                IDamageable enemyAttributes = overlappedColliders[i].GetComponent<IDamageable>();
                if (enemyAttributes != null)
                {
                    enemyAttributes.ApplyDamage(meleeDamage);
                }
            }
            timeUntilMeleeReadied = meeleAttackDelay;

        }
        else
        {
            timeUntilMeleeReadied -= Time.deltaTime;
        }
    }
    void Flip(float horizontalInput)
    {
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

}


