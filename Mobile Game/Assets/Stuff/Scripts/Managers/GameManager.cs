using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using TMPro;

public class GameManager : MonoBehaviour, IUnityAdsListener
{
    [Header("References")]
    public Button PauseButton;
    public TMP_Text distanceText;
    public TMP_Text candiesCollectedText;
    public TMP_Text gemsCollectedText;
    private Animator anim;

#if UNITY_IOS
    private string gameId = "4375458";
#elif UNITY_ANDROID
    private string gameId = "4375459";
#elif UNITY_EDITOR
    private string gameId = "4375459";
#endif
    private string addGemId = "Add_Gems";
    private string gameStartId = "Game_Start";
    public int gemReward;

    [Header("State")]
    public bool loadingScene;
    public bool paused;
    public RuntimeAnimatorController playerSkin;
    public bool hasExtraLife;
    public bool powerupOpen;
    public int timeToPlayAd;
    private int timesPlayed;


    [Header("Player Items")]
    public int candies;
    public int gems;
    public int candiesCollected;
    public int gemsCollected;

    [Header("Conversions")]
    public int candyToGemRate;
    public int gemCollectRate;

    [Header("Scene Loading")]
    public float sceneTransitionTime;

    public void OnUnityAdsDidError(string message)
    {

    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult result)
    {
        if (result == ShowResult.Finished && placementId == addGemId)
        {
            gems += gemReward;
        }
        paused = false;
    }
    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void PlayGemAd() { Advertisement.Show(addGemId); }
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
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
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
        distanceText.text = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>().distance.text;
        candiesCollectedText.text = candiesCollected.ToString();
        gemsCollectedText.text = gemsCollected.ToString();
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
        gemsCollected = 0;
        candiesCollected = 0;
        loadingScene = false;

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        PauseButton.gameObject.SetActive(SceneManager.GetActiveScene().buildIndex == 1);
        anim.SetTrigger("Reset");

        timesPlayed += 1;
        if (timesPlayed >= timeToPlayAd)
        {
            paused = true;
            Advertisement.Show(gameStartId);
            timesPlayed = 0;
        }
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
            distanceText.text = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>().distance.text;
            candiesCollectedText.text = candiesCollected.ToString();
            gemsCollectedText.text = gemsCollected.ToString();
        }

    }

    public void AddGems()
    {
        gems += gemCollectRate;
        gemsCollected += gemCollectRate;
    }
    public void AddCandies()
    {
        candies += 1;
        candiesCollected += 1;
    }
}