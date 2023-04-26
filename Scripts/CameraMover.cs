using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    int BaseSpeed = 2;
    int ShiftSpeed = 4;
    float Change;
    Camera Cam;
    float Size;
    public RenderTexture RenderTexture;
    Vector3 Pos;

    void Start()
    {
        Cam = GetComponent<Camera>();
        Size = Cam.orthographicSize;
        Change = (Size * 2.0f) / RenderTexture.height;

        Pos = new Vector3(0,0,0);
    }

    void Update()
    {
        MoveMomentum();
    }

    float Up = 0;
    float Down = 0;
    float Left = 0;
    float Right = 0;
    int MaxAcc = 100;

    void MoveMomentum()
    {
        float SpeedModifier = 1;
        

        // Check if shifting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SpeedModifier = ShiftSpeed;
        }
        else
        {
            SpeedModifier = BaseSpeed;
        }

        float Intensity = 10;
        Vector3 ChangeNormalizedPos = new Vector3(Pos.x * Change, Pos.y * Change, Pos.z * Change);

        if (Input.GetKey(KeyCode.W) && Up < MaxAcc)
        {
            Up += 10;
        }

        if (Input.GetKey(KeyCode.S) && Down < MaxAcc)
        {
            Down += 10;
        }

        if (Input.GetKey(KeyCode.A) && Left < MaxAcc)
        {
            Left += 10;
        }

        if (Input.GetKey(KeyCode.D) && Right < MaxAcc)
        {
            Right += 10;   
        }

        Pos.y += (int)(SpeedModifier * (1 + Up) / Intensity);
        Pos.y -= (int)(SpeedModifier * (1 + Down) / Intensity);
        Pos.x -= (int)(SpeedModifier * (1 + Left) / Intensity);
        Pos.x += (int)(SpeedModifier * (1 + Right) / Intensity);
        this.transform.localPosition = ChangeNormalizedPos;


        float Intensity2 = 10.0f;

        if (Left > 0)
        {
            Left -= (Left - 0) / Intensity2;
        }
        if (Right > 0)
        {
            Right -= (Right - 0) / Intensity2;
        }
        if (Up > 0)
        {
            Up -= (Up - 0) / Intensity2;
        }
        if (Down > 0)
        {
            Down -= (Down - 0) / Intensity2;
        }
    }
}
