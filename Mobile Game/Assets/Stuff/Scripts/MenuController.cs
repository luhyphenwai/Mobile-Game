using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("References")]
    private GameManager gm;
    private Animator anim;

    private bool inShop = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
    }


    public void Play()
    {
        gm.StartCoroutine(gm.Load(1));
    }

    public void PlayGemAd()
    {
        gm.PlayGemAd();
    }

    public void OpenShop()
    {
        if (!inShop)
        {
            anim.SetTrigger("ToggleShop");
            inShop = true;
        }
    }
    public void CloseShop()
    {
        if (inShop)
        {
            anim.SetTrigger("ToggleShop");
            inShop = false;
        }
    }

}

