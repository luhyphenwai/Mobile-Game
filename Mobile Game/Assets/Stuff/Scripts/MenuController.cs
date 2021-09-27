using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("References")]
    private GameManager gm;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }


    public void Play()
    {
        gm.StartCoroutine(gm.Load(1));
    }

    public void Shop()
    {

    }

}

