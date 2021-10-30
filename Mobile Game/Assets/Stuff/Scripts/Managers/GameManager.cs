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
    public AudioListener al;
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
    private string gameBannerID = "Banner";
    public int gemReward;

    [Header("State")]
    public bool testMode = true;
    public bool loadingScene;
    public bool paused;
    public RuntimeAnimatorController playerSkin;
    public bool hasExtraLife;
    public bool powerupOpen;
    public int timeToPlayAd;
    private int timesPlayed;
    public int distanceTraveled;
    public int highScore;


    [Header("Player Items")]
    public int candies;
    public int gems;
    public int candiesCollected;
    public int gemsCollected;
    public Skin[] skins;

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
    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(gameBannerID))
        {
            yield return null;
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(gameBannerID);
    }
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
        Advertisement.Initialize(gameId, testMode);
        StartCoroutine(ShowBannerWhenReady());

        Data data = SaveSystem.LoadData();
        gems = data.gems;
        highScore = data.highScore;
    }

    public void setAudio(bool enabled)
    {
        al.enabled = enabled;
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


        anim.SetTrigger("Transition");
        anim.SetTrigger("Reset");
        yield return new WaitForSeconds(sceneTransitionTime);

        AsyncOperation load = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        if (!load.isDone) yield return null;

        yield return new WaitForSeconds(0.5f);

        // Set animations and reshow settings if needed
        anim.SetTrigger("Transition");
        anim.ResetTrigger("Reset");
        PauseButton.gameObject.SetActive(SceneManager.GetActiveScene().name != "Main Menu");

        // Show and hide ads
        if (SceneManager.GetActiveScene().name == "Main Menu") Advertisement.Banner.Show();
        else Advertisement.Banner.Hide();

        timesPlayed += 1;
        if (timesPlayed >= timeToPlayAd)
        {
            paused = true;
            Advertisement.Show(gameStartId);
            timesPlayed = 0;
        }
        yield return new WaitForSeconds(sceneTransitionTime);

        // Reset stats
        gemsCollected = 0;
        candiesCollected = 0;

        // Confirm that scene is loaded
        loadingScene = false;

        // Deselect buttons
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);




        highScore = distanceTraveled > highScore ? distanceTraveled : highScore;
        SaveSystem.SaveData(this);
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
            gemsCollectedText.text = (gemsCollected + candiesCollected * candyToGemRate).ToString();
            gems += candiesCollected * candyToGemRate;
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