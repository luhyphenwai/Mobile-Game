using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonObject : MonoBehaviour

{
    private Animator anim;
    private Transform player;
    public float firingSpeed;
    public float animationTime;
    public float playerDistance;
    public Transform firingPosition;
    public GameObject cannonBall;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(firingSpeed);

        if (Vector2.Distance(transform.position, player.position) < playerDistance)
        {
            anim.SetTrigger("Fire");
            yield return new WaitForSeconds(animationTime);
            Instantiate(cannonBall, firingPosition.transform);
        }


        StartCoroutine(Shoot());
    }
}
