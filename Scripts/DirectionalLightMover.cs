using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightMover : MonoBehaviour
{
    private float sliderLastX = 0f;
    private float sliderX = .1f;
    private float sliderLastY = 0f;
    private float sliderY = .1f;
 
    void Update() {

    

        // DAY TIME
        if (Input.GetKey(KeyCode.T))
        {
            sliderY += .3f;
            Quaternion localRotation = Quaternion.Euler(0f, sliderY - sliderLastY, 0f);
            transform.rotation = transform.rotation * localRotation;
            sliderLastY = sliderY;
        }
            
        // DIRECTION
        if (Input.GetKey(KeyCode.Y))
        {
            sliderX += .3f;
            Quaternion localRotation = Quaternion.Euler(sliderX - sliderLastX, 0f, 0f);
            transform.rotation = transform.rotation * localRotation;
            sliderLastX = sliderX;
        }
    }
}
