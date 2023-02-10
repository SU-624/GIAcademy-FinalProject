using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mang 2022. 10. 14
/// 
/// 맨 처음 시작 화면 -> 클릭 시 로그인 팝업 창을 띄워 줄 클래스
/// </summary>
public class StartTitle : MonoBehaviour
{
    public GameObject PressScreenButton;        // 화면을 눌러주세요 버튼

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 화면을 누르면 로그인 팝업창을 띄워 줄 함수
    public void ClickOkButton()
    {
        if (PressScreenButton.activeSelf == false)
        {
            PressScreenButton.SetActive(true);                                                                                                                                                                                                     
        }
    }
}
