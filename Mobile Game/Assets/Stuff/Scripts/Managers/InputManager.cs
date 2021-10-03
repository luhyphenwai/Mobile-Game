using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Settings")]
    public bool tapped;
    public bool doubleTapped;
    public bool touching;
    public int currentTaps;
    public float tapTime;
    private float touchingTime;

    // Update is called once per frame
    void Update()
    {
        tapped = false;
        doubleTapped = false;
        touching = false;

        if (Input.GetKeyDown(KeyCode.Space)) tapped = true;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began || Input.GetKey(KeyCode.Space))
            {
                // tapped = touch.tapCount == 1 || Input.GetKey(KeyCode.Space);
                doubleTapped = touch.tapCount == 2;
            }
            else if (touchingTime > tapTime && !touching) { touching = true; tapped = touch.tapCount == 1; }
            else if (touchingTime > tapTime) touching = true;
        }
        if (Input.touchCount == 0 || !Input.GetKey(KeyCode.Space)) touchingTime = 0;
        else touchingTime += Input.GetKey(KeyCode.Space) ? Input.touches[0].deltaTime : Time.deltaTime;
    }

}
