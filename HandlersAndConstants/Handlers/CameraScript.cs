using System;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private void Awake()
    {
        var camera = Camera.main;
        if (!PlayerPrefs.HasKey("main_camera_ortho_scale"))
        {
            float screenRatio = (float)Screen.height/Screen.width;
            float gameRatio = 1920f / 1080f;
            var aspectsRatio = screenRatio / gameRatio;
            var orthographicSize = camera.orthographicSize;
            orthographicSize *= aspectsRatio;
            PlayerPrefs.SetFloat("main_camera_ortho_scale",orthographicSize);
        }
        camera.orthographicSize = PlayerPrefs.GetFloat("main_camera_ortho_scale");
    }
}
