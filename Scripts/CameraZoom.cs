using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera Camera;
    public RenderTexture RenderTexture;
    public Light PL;
    Vector2 Dimen;
    public GameObject GO;

    private int ScreenSizeX = 0;
    private int ScreenSizeY = 0;

    private void RescaleCamera()
    {
        if (Screen.width == ScreenSizeX && Screen.height == ScreenSizeY) return;
 
        float targetaspect = 16.0f / 9.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = GetComponent<Camera>();
 
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;
 
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
 
             camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;
 
            Rect rect = camera.rect;
 
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
 
             camera.rect = rect;
        }
 
        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }


    void Start()
    {
        Dimen = new Vector2(960, 540);
        RescaleCamera();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            Camera.orthographicSize /= 2.0f;
            
            RenderTexture.Release();
            Dimen /= 2.0f;
            RenderTexture.width = (int)Dimen.x;
            RenderTexture.height = (int)Dimen.y;
            RenderTexture.Create();
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Camera.orthographicSize *= 2.0f;
            
            RenderTexture.Release();
            Dimen *= 2.0f;
            RenderTexture.width = (int)Dimen.x;
            RenderTexture.height = (int)Dimen.y;
            RenderTexture.Create();
        }
    }

    void OnDisable()
    {
        RenderTexture.Release();
        RenderTexture.width = 960;
        RenderTexture.height = 540;
        RenderTexture.Create();
    }
}