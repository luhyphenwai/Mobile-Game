using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem collectedParticle;
    private GameManager gm;
    private SpriteRenderer sr;

    [Header("Floating variables")]
    public float highest;
    public float lowest;
    private float currentTarget;
    public float speed;

    [Header("State")]
    public bool collected;
    public bool isGem;

    [Header("Animation Parameters")]
    public int animationTime;
    public float heightIncrease;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.GetChild(0).transform.position.y - currentTarget) > 0.2f && !collected)
        {
            transform.GetChild(0).transform.position = new Vector2(transform.GetChild(0).transform.position.x, Mathf.Lerp(transform.GetChild(0).transform.position.y, currentTarget, speed));
        }
        else if (!collected) currentTarget = currentTarget == lowest ? highest : lowest;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (!collected)
            {
                if (isGem) gm.AddGems();
                else gm.candies += 1;
                collectedParticle.Play();
                StartCoroutine(CollectAnimation());
            }
            collected = true;

        }
    }

    IEnumerator CollectAnimation()
    {
        for (int i = 0; i < animationTime; i++)
        {
            Color color = sr.color;
            float value = 1 / (float)animationTime;
            color.a -= value;
            sr.color = color;

            Transform child = transform.GetChild(0).transform;
            child.position = new Vector2(child.position.x, child.position.y + heightIncrease / animationTime);
            yield return new WaitForSeconds(0.01f);

        }
        yield return new WaitForSeconds(5);
    }
}
