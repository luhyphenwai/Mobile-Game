using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelObject> levelObjects;
    public GameObject[] levelPrefabs;
    public float velocity;
    public float levelSpawnPosition;
    public float levelDeletePosition;
    public GameObject SideDeathZone;
    // Start is called before the first frame update
    void Start()
    {
        SideDeathZone.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(-Screen.width / 3.5f, 0, 10));
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
            else if (levelObjects[i].transform.position.x < levelDeletePosition)
            {
                // Remove object from list and destroy object
                LevelObject levelObject = levelObjects[i];
                levelObjects.Remove(levelObjects[i]);
                Destroy(levelObject.gameObject);
            }
        }
    }

    void SpawnLevelObject(GameObject level)
    {
        GameObject levelObject = Instantiate(levelPrefabs[Random.Range(0, levelPrefabs.Length)], level.transform.GetChild(0).position, Quaternion.identity);
        LevelObject levelScript = levelObject.GetComponent<LevelObject>();
        levelScript.spawned = false;
        levelObjects.Add(levelScript);
    }
}
