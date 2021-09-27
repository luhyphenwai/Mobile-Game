using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PassablePlatforms : MonoBehaviour
{
    [Header("References")]
    private BoxCollider2D bc;
    private PlayerController player;
    private InputManager input;

    [Header("Variables")]
    public float centerOffset;
    public float offTime = 0.8f;
    public bool off;

    private void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputManager>();
        bc = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    private void Update()
    {
        if (input.touching || off)
        {
            bc.isTrigger = true;
            if (!off) StartCoroutine(Timer());
        }
        else if (player.transform.position.y - (transform.position.y + centerOffset) > 0) bc.isTrigger = false; // Set block solid if player is above the object
        else bc.isTrigger = true;
    }

    IEnumerator Timer()
    {
        off = true;
        yield return new WaitForSeconds(offTime);
        off = false;
    }
}
