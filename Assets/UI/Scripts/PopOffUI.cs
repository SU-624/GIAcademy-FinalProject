using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mang 10.17 
/// 
/// UI의 PopOff 를 담당해 줄 UI 스크립트
/// </summary>
public class PopOffUI : MonoBehaviour
{
    public GameObject m_UI;         // 지정할 UI
    public int m_DelayTime;

    [SerializeField]
    private GameObject MyUI
    {
        set { m_UI = value; }
    }

    [SerializeField]
    private int DelayTime
    {
        set { m_DelayTime = value; }
    }

    // 실행 될 때 꺼둘 UI는 다 꺼두기
    void Start()
    {
        //SetTurnOffUI(m_gameObject);
    }

    // 팝업으로 띄울 UI는 게임 시작 전에 꺼두기
    public void SetTurnOffUI(GameObject gObject)
    {
        gObject.SetActive(false);
    }

    // UI를 꺼주는 시간 조정 함수
    public void DelayTurnOffUI()
    {
        Invoke("TurnOffUI", m_DelayTime);
    }

    // 팝업 되어서 켜진 UI를 꺼주는 함수
    public void TurnOffUI()
    {
        if(m_UI.activeSelf == true)
        {
            m_UI.SetActive(false);
        }

        Time.timeScale = 1;
        Debug.Log("시간 흐름");
    }

}