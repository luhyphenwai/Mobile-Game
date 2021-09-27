using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonObject : MonoBehaviour

{
    private Animator anim;
    public float firingSpeed;
    public float animationTime;
    public Transform firingPosition;
    public GameObject cannonBall;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(firingSpeed);
        anim.SetTrigger("Fire");
        yield return new WaitForSeconds(animationTime);
        Instantiate(cannonBall, firingPosition.transform);

        StartCoroutine(Shoot());
    }
}
