using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public LayerMask groundLayer;
    private InputManager input;
    private GameManager gm;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Animator anim;

    [Header("Player State")]
    private bool isGrounded;
    private bool dead;
    private bool atHouse;

    [Header("Movement Variables")]
    public float jumpHeight;
    public bool doubleJumped;
    public float velocity;
    public float deathJumpHeight;

    [Header("Level Interactions")]
    public float jumpPadHeight;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

    }
    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputManager>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }


    // Update is called once per frame
    void Update()
    {
        // Check if player is on ground
        RaycastHit2D groundCast = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        isGrounded = groundCast == true;

        if (!dead)
        {
            // If just touched screen && can jump
            if ((isGrounded || !doubleJumped) && input.tapped && !atHouse)
            {
                doubleJumped = !isGrounded;
                rb.velocity = new Vector2(0, jumpHeight);
                anim.SetTrigger("Jump");
            }
            else if (atHouse && input.tapped)
            {
                gm.DepositCandies();
            }
            anim.SetBool("IsGrounded", isGrounded);

            // Keep position in center
            rb.velocity = new Vector2(Mathf.Lerp(transform.position.x, 0, velocity), rb.velocity.y);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hurt" && !dead)
        {
            anim.SetTrigger("Died");
            dead = true;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GameObject.FindGameObjectWithTag("Background").GetComponent<Animator>().enabled = false;
            rb.velocity = new Vector2(0, jumpHeight);
            gm.PlayerDied();
        }
        else if (other.tag == "Jump")
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("Launch");
            rb.velocity = new Vector2(0, jumpPadHeight);
        }
        else if (other.tag == "House")
        {
            atHouse = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "House")
        {
            atHouse = false;
        }
    }
}
