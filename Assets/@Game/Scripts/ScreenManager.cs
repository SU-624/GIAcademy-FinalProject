using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    // w 2340 / h 1080 / 0.6배
    private int width = 1404;
    private int height = 648;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        width = Screen.width;
        
        // 변경되었다면 바꾼다.
        //if (width != Screen.width)
        {
            Screen.SetResolution(Screen.width, Screen.width * 1080 / 2340, false);
        }

    }
}
