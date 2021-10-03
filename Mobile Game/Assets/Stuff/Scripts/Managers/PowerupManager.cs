using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    [Header("References")]
    public Projectile projectilePrefab;
    private PlayerController pc;
    private GameManager gm;
    private InputManager input;
    public Button[] buttons;

    [Header("Activated Powerups")]
    public bool canTripleJump;
    public bool slowDownTime;
    public bool extraLife;
    public bool slowDownProjectiles;
    public bool doubleCoins;

    [Header("Settings")]
    public bool tripleJumped;
    public float timeSlowFactor;
    public float slowerProjectileSpeed;
    public int lastCoins;
    public float upVelocity;
    public float velocity;
    private bool canStart;


    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canTripleJump) TripleJumping();
        if (slowDownTime) SlowDowntime();
        if (extraLife) ExtraLife();
        if (slowDownProjectiles) SlowDownProjectiles();

        if (canStart && input.tapped)
        {
            pc.GetComponent<CapsuleCollider2D>().enabled = true;
            pc.GetComponent<Rigidbody2D>().isKinematic = false;
            pc.dead = false;
            GameObject.FindGameObjectWithTag("Background").GetComponent<Animator>().enabled = true;
            GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>().velocity = velocity;
        }
    }

    public void BuyItem(Button button)
    {
        if (button.name == "Triple Jumping ")
        {
            button.interactable = !canTripleJump;
        }
        else if (button.name == "Slow Down Time")
        {
            button.interactable = !slowDownTime;
        }
        else if (button.name == "Extra Life")
        {
            button.interactable = !extraLife;
        }
        else if (button.name == "Slow Down Projectiles")
        {
            button.interactable = !slowDownProjectiles;
        }
        else if (button.name == "Double Coins")
        {
            button.interactable = !doubleCoins;
        }

    }
    public void CheckButtons()
    {
        foreach (Button button in buttons)
        {
            if (button.name == "Triple Jumping ")
            {
                button.interactable = !canTripleJump;
            }
            else if (button.name == "Slow Down Time")
            {
                button.interactable = !slowDownTime;
            }
            else if (button.name == "Extra Life")
            {
                button.interactable = !extraLife;
            }
            else if (button.name == "Slow Down Projectiles")
            {
                button.interactable = !slowDownProjectiles;
            }
            else if (button.name == "Double Coins")
            {
                button.interactable = !doubleCoins;
            }
        }
    }
    void TripleJumping()
    {
        if (pc.jumped && pc.doubleJumped && !tripleJumped)
        {
            pc.doubleJumped = false;
            tripleJumped = true;
        }
        tripleJumped = !(pc.isGrounded && tripleJumped);
    }

    void SlowDowntime()
    {
        if (!gm.paused) Time.timeScale = timeSlowFactor;
    }

    void ExtraLife()
    {
        gm.hasExtraLife = true;
        if (pc.dead)
        {
            StartCoroutine(ResetLife());
            gm.hasExtraLife = false;
            extraLife = false;
        }
    }
    void SlowDownProjectiles()
    {
        if (projectilePrefab.velocity != slowerProjectileSpeed) projectilePrefab.velocity = slowerProjectileSpeed;
    }

    void DoubleCoins()
    {
        if (gm.coins > lastCoins) gm.AddCoin();
        lastCoins = gm.coins;
    }

    IEnumerator ResetLife()
    {
        velocity = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>().velocity;
        GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>().velocity = 0;
        yield return new WaitForSeconds(2);

        GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>().velocity = 0.75f;
        yield return new WaitForSeconds(1);
        GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>().velocity = 0;


        // Reset animation
        Animator anim = pc.GetComponent<Animator>();
        anim.Rebind();
        anim.Update(0f);

        pc.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        pc.GetComponent<Rigidbody2D>().isKinematic = true;

        pc.transform.position = new Vector2(0, -10);
        while (pc.transform.position.y < -0.5)
        {
            pc.transform.position = new Vector2(0, pc.transform.position.y + upVelocity);
            yield return null;
        }

        yield return new WaitForSeconds(0f);


    }
}
