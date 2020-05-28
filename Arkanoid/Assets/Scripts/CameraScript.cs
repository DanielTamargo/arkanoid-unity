using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public SpriteRenderer rink;

    // Use this for initialization
    void Start()
    {
        //Debug.Log((float)Screen.height);
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = rink.bounds.size.x / rink.bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = rink.bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = rink.bounds.size.y / 2 * differenceInSize;
        }
    }
}
