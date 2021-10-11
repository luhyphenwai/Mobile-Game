using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ParticleController : MonoBehaviour
{
    private Light2D lightObject;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        lightObject = GetComponent<Light2D>();
        StartCoroutine(Timer());
    }


    IEnumerator Timer()
    {
        while (lightObject.intensity > 0)
        {
            lightObject.intensity -= 1 / speed * 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        lightObject.intensity = 0;
    }
}
