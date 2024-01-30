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
    public AudioManager audioManager;
    public float groundCheckRadius = 0.2f;


    [Header("Movement")]
    public float Xspeed = 2;
    public float Yspeed = 6;
    private bool isGrounded;
    float directionX;
    float directionY;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    private bool isWearingGlasses = false;
    public bool spring;
    public float springJumpForce = 2f;

    [Header("Combat")]
    public Transform meeleAttackOrigin = null;
    public float meeleAttackRadius = 0.6f;
    public float meleeDamage = 2f;
    public float meeleAttackDelay = 1.1f;
    public LayerMask enemyLayer = 8;
    public LayerMask springLayer = 6;
    public KeyCode meleeAtackKey = KeyCode.J;
    private float timeUntilMeleeReadied = 0f;
    private bool attemptMeleeAttack = false;

    void Start()
    {
        gb = GetComponent<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        SwitchWorlds();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
       
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
        isGrounded = IsGrounded();
        float horizontalInput = Input.GetAxis("Horizontal");
        Flip(horizontalInput);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        Vector3 movement = new Vector3(directionX, 0f, 0f);
        transform.position += movement * Xspeed * Time.deltaTime;


        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isWearingGlasses = !isWearingGlasses;
            SwitchWorlds();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.jumpsound);
                
            }
        }
        if(Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Jumping", true);

        }
        else
        {
            animator.SetBool("Jumping", false);

        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        IsSpring();
        if (IsSpring())
        {
            rb.velocity = new Vector2(directionX, springJumpForce);
        }
    }

    bool IsSpring()
    {
        return Physics2D.OverlapBox(transform.position - new Vector3(0, 0f, 0), new Vector2(1f, 3f), 0, LayerMask.GetMask("Spring"));
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, Yspeed);

    }
    void SwitchWorlds()
    {
        world1.SetActive(!isWearingGlasses);
        world2.SetActive(isWearingGlasses);
        EventManager.Instance.WorldSwitch();

    }

    bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, groundCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Ground") || collider.CompareTag("Box"))
            {
                return true;
            }
        }

        return false;
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
                    enemyAttributes.ApplyDamage(0);
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


