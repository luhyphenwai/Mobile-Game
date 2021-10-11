using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public Button PauseButton;
    private Animator anim;

    [Header("State")]
    public bool loadingScene;
    public bool paused;
    public RuntimeAnimatorController playerSkin;
    public bool hasExtraLife;
    public bool powerupOpen;


    [Header("Player Items")]
    public int candies;
    public int gems;

    [Header("Conversions")]
    public int candyToGemRate;
    public int gemCollectRate;

    [Header("Scene Loading")]
    public float sceneTransitionTime;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        anim = GetComponent<Animator>();
        PauseButton.gameObject.SetActive(SceneManager.GetActiveScene().buildIndex == 1);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Pausing();
        // print(SceneManager.GetActiveScene().buildIndex);
    }

    public void TriggerPause()
    {
        paused = !paused;
        PauseButton.interactable = !paused;
    }
    public void TriggerPowerupMenu()
    {
        paused = !paused;
        powerupOpen = !powerupOpen;
    }
    void Pausing()
    {
        anim.SetBool("Paused", paused && !powerupOpen);
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
        gems += candies * candyToGemRate;
    }

    public IEnumerator Load(int scene)
    {
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

        PauseButton.gameObject.SetActive(SceneManager.GetActiveScene().buildIndex == 1);
        anim.SetTrigger("Reset");
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

    public void AddGems()
    {
        gems += gemCollectRate;
    }
}