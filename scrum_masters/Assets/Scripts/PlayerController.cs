using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Animator animator;
    public GameObject world1;
    public GameObject world2;
    GameObject gb;
    Rigidbody2D rb;

    public float Xspeed = 2;
    public float Yspeed = 6;
    private bool isGrounded;
    float directionX;
    float directionY;

    private bool isWearingGlasses = false;

    void Start()
    {
        gb = GetComponent<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        SwitchWorlds();


    }

    // Update is called once per frame
    void Update()
    {
        directionX = Input.GetAxis("Horizontal");

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


}


