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

    [Header("Scripts")]
    public int candies;

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

    public void DepositCandies()
    {

    }

    public IEnumerator Load(int scene)
    {
        if (loadingScene) yield break; // Break if already loading scene
        loadingScene = true;
        AsyncOperation load = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        load.allowSceneActivation = false;
        // anim.SetTrigger("Transition");
        yield return new WaitForSeconds(sceneTransitionTime);

        load.allowSceneActivation = true;
    }

    public void PlayerDied()
    {
        GameObject.FindGameObjectWithTag("Level").GetComponent<LevelManager>().velocity = 0;
        anim.SetTrigger("Player Died");
    }
}