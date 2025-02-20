using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public Volume ppVol;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        if (Input.GetKeyDown(KeyCode.V) && ppVol.weight > 0f)
        {
            ppVol.weight = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.V) && ppVol.weight < 1f)
        {
            ppVol.weight = 1f;
        }
    }
}
