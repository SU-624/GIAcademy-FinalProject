using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mang 10.18
/// 
/// UI의 PopUp 관련을 담당해줄 스크립트S
/// </summary>
public class PopUpUI : MonoBehaviour
{
    public GameObject m_UI;         // 지정할 UI!
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

    // UI를 켜주는 시간 조정 함수
    public void DelayTurnOnUI()
    {
        Invoke("JustTurnOn", m_DelayTime);
    }

    // 버튼이 눌리면 그냥 켜주기
    public void TurnOnUI()
    {
        this.gameObject.SetActive(true);

        if (m_UI.gameObject.activeSelf == false)
        {
            m_UI.gameObject.SetActive(true);
        }
    }

    public void JustTurnOn()
    {

        if (this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }
    }

    // 지정 팝업창을 켜 주면서 그 전의 UI 는 꺼준다.
    public void PopUpMyUI()
    {
        if (m_UI.gameObject.activeSelf == false)
        {
            m_UI.gameObject.SetActive(true);    // 이 스크립트가 붙어있는 오브젝트 본인
            this.gameObject.SetActive(false);   // 누르는 버튼은 꺼주기
        }
    }


}
