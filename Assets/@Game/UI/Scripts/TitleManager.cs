using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Button TouchScreenButton;
    public SoundManager Sound;

    // w 2340 / h 1080 / 0.6��
    private int width = 1404;
    private int height = 648;
    
    private void Awake()
    {
        // *0.6
        Screen.SetResolution(width, height, false);
    }

    void Start()
    {
        // EventScriptManager�� ��ư�� �Լ� �ޱ� �Ƿ���
        Sound.PlayTitleBgm();
    }

    // Update is called once per frame
    void Update()
    {
        // ����Ǿ��ٸ� �ٲ۴�.
        //if (width != Screen.width)
        {
            Screen.SetResolution(Screen.width, Screen.width * 1080 / 2340, false);
        }
    }
}