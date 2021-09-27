using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    private Animator anim;
    private GameManager gm;
    public int collectableType;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            anim.SetTrigger("Collected");
            if (collectableType == 0) gm.candies += 1;

            Destroy(gameObject);
        }
    }
}
