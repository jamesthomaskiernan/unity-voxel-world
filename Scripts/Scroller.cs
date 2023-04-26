using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scroller : MonoBehaviour
{
    int CurrentLayer = 20;
    public GameObject World;
    public ComputeShader WorldGeneration;
    public ComputeShader CollisionGeneration;
    public ComputeShader Grass;
    double y_off = Math.Pow((2.0/3.0), .5);
    
    // Called every frame
    void Update()
    {
        CheckScroller();
    }

    // Checks for changes to CurrentLayer, regenerates world if there were changes
    public void CheckScroller()
    {
        bool Reload = false;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0)
        {
            CurrentLayer += 1;
            Reload = true;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0)
        {
            CurrentLayer -= 1;
            Reload = true;
        }
        
        // If CurrentLayer was changed
        if (Reload)
        {
            // Update CurrentLayer
            WorldGeneration.SetInt("CurrentLayer", CurrentLayer);
            CollisionGeneration.SetInt("CurrentLayer", CurrentLayer);
            Grass.SetInt("CurrentLayer", CurrentLayer);
            
            // Regenerate world
            World.GetComponent<WorldGenerator>().RegenerateWorld();
        }
    }
}
