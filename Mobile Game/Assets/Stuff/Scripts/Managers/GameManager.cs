using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    private Animator anim;

    [Header("State")]
    public bool loadingScene;
    public bool paused;
    public RuntimeAnimatorController playerSkin;
    public bool hasExtraLife;

    [Header("Scripts")]
    public int candies;

    [Header("Variables")]
    public int coins;
    public int gems;
    public int candyToCoinRate;
    public int coinCollectRate;

    [Header("Scene Loading")]
    public float sceneTransitionTime;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Pausing();
    }

    public void TriggerPause()
    {
        paused = !paused;
    }
    void Pausing()
    {
        anim.SetBool("Paused", paused);
        Time.timeScale = paused ? 0 : 1;
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(Load(0));
    }
    public void PlayAgain()
    {
        StartCoroutine(Load(1));
    }

    public void DepositCandies()
    {
        coins += candies * candyToCoinRate;
    }

    public IEnumerator Load(int scene)
    {
        print("bruh");
        if (loadingScene) yield break; // Break if already loading scene
        loadingScene = true;
        AsyncOperation load = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        load.allowSceneActivation = false;
        anim.SetTrigger("Transition");
        yield return new WaitForSeconds(sceneTransitionTime);

        anim.SetTrigger("Reset");

        load.allowSceneActivation = true;

        // anim.SetTrigger("Transition");
        yield return new WaitForSeconds(sceneTransitionTime);
        loadingScene = false;

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    public void PlayerDied()
    {
        if (hasExtraLife)
        {

        }
        else
        {
            GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>().velocity = 0;
            anim.SetTrigger("Player Died");
        }

    }

    public void AddCoin()
    {
        coins += coinCollectRate;
    }
}