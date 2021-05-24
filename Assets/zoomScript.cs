using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomScript : MonoBehaviour
{
    private float zoomChange = 12;
    private float smoothChange = 0.7f;
    private float minSize = 2, maxSize = 1000;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            cam.orthographicSize -= zoomChange * Time.deltaTime * smoothChange;
        }
        if (Input.mouseScrollDelta.y < 0)
            cam.orthographicSize += zoomChange * Time.deltaTime * smoothChange;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minSize, maxSize);
    }
}
