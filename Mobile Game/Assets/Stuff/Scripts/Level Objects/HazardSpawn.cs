using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpawn : MonoBehaviour
{

    public GameObject spikes;
    public GameObject cannons;
    public int spikeRate;
    public int cannonRate;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, spikeRate + cannonRate) > spikeRate)
        {
            Instantiate(spikes, transform);
        }
        else
        {
            Instantiate(cannons, transform);
        }
    }

}
