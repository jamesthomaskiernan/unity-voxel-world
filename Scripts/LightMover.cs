using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMover : MonoBehaviour
{


    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            transform.position = transform.position + new Vector3(.1f, 0, .1f);
        }

        if (Input.GetKey(KeyCode.K))
        {
            transform.position = transform.position + new Vector3(-.1f, 0, -.1f);
        }

        if (Input.GetKey(KeyCode.J))
        {
            transform.position = transform.position + new Vector3(-.1f, 0, .1f);
        }

        if (Input.GetKey(KeyCode.L))
        {
            transform.position = transform.position + new Vector3(.1f, 0, -.1f);
        }
    }
}
