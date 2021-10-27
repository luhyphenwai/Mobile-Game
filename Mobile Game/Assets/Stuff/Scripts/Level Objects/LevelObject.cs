using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocity;
    public bool spawned;
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.right * velocity;
        if (parent != null) transform.position = parent.position;
    }
}
