using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{

    public TMP_Text distance;
    public TMP_Text candy;
    private GameManager gm;
    public float distanceFactor;
    public List<LevelObject> levelObjects;
    public GameObject[] levelPrefabs;
    public float velocity;
    public float levelSpawnPosition;
    public float levelDeletePosition;
    public GameObject SideDeathZone;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        SideDeathZone.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)) - Vector3.right * 3;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < levelObjects.Count; i++)
        {
            levelObjects[i].velocity = velocity; // Set speed

            // Check if the position is past the level spawn position
            if (!levelObjects[i].spawned && levelObjects[i].transform.position.x < levelSpawnPosition)
            {
                // Record that object has already be spawned and spawn object
                levelObjects[i].spawned = true;
                SpawnLevelObject(levelObjects[i].gameObject);
            }
            else if (i != 0 && levelObjects[i].transform.position.x < levelDeletePosition)
            {
                // Remove object from list and destroy object
                LevelObject levelObject = levelObjects[i];
                levelObjects.Remove(levelObjects[i]);
                Destroy(levelObject.gameObject);
            }
        }
        distance.text = (-Mathf.Round(levelObjects[0].transform.position.x - 12) * distanceFactor).ToString() + "m";
        gm.distanceTraveled = (int)-Mathf.Round((levelObjects[0].transform.position.x - 12) * distanceFactor);
        candy.text = gm.candiesCollected.ToString();
    }

    void SpawnLevelObject(GameObject level)
    {
        GameObject levelObject = Instantiate(levelPrefabs[Random.Range(0, levelPrefabs.Length)], level.transform.GetChild(0).position, Quaternion.identity);
        LevelObject levelScript = levelObject.GetComponent<LevelObject>();
        levelScript.spawned = false;
        levelScript.parent = level.transform.GetChild(0).transform;

        levelObjects.Add(levelScript);
    }
}
