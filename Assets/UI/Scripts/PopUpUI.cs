using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Mang 10.18
/// 
/// UI의 PopUp 관련을 담당해줄 스크립트
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

    Vector3 target;

    public void Start()
    {
        // float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        target = new Vector3(m_UI.transform.position.x - ((float)Screen.width / 2f), m_UI.transform.position.y, m_UI.transform.position.z);
    }
    // UI를 켜주는 시간 조정 함수
    public void DelayTurnOnUI(/*int t*/)
    {
        //if(t != 0)
        //{
        //    m_DelayTime = t;
        //}

        Invoke("JustTurnOn", m_DelayTime);
    }

    // 버튼이 눌리면 켜고
    public void TurnOnUI()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            this.gameObject.SetActive(true);

            if (m_UI.gameObject.activeSelf == false)
            {
                m_UI.gameObject.SetActive(true);
            }
        }
        else
        {
            InGameUI.Instance.UIStack.Push(this.gameObject);
            Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);
            if (InGameUI.Instance.UIStack != null)
            {
                this.gameObject.SetActive(true);

                if (m_UI.gameObject.activeSelf == false)
                {
                    m_UI.gameObject.SetActive(true);
                }

                if (SceneManager.GetActiveScene().name == "InGameScene")
                {
                    if (GameTime.Instance != null)
                    {
                        GameTime.Instance.IsGameMode = false;
                    }
                    Time.timeScale = 0;

                    Debug.Log("시간 멈춤");
                }
            }
        }
    }

    public void JustTurnOn()
    {

        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            if (this.gameObject.activeSelf == false)
            {
                this.gameObject.SetActive(true);
            }
        }
        else
        {
            InGameUI.Instance.UIStack.Push(this.gameObject);
            Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);

            if (InGameUI.Instance.UIStack != null)
            {
                if (this.gameObject.activeSelf == false)
                {
                    this.gameObject.SetActive(true);
                }
            }

            if (SceneManager.GetActiveScene().name == "InGameScene")
            {
                if (GameTime.Instance != null)
                {
                    GameTime.Instance.IsGameMode = false;
                }
                Time.timeScale = 0;

                Debug.Log("시간 멈춤");
            }
        }
    }

    public void TurnOnMailIcon()
    {
        if (m_UI.gameObject.activeSelf == false)
        {
            m_UI.SetActive(true);
        }
    }

    // 지정 팝업창을 켜 주면서 그 전의 UI 는 꺼준다.
    public void PopUpMyUI()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            if (m_UI.gameObject.activeSelf == false)
            {
                m_UI.gameObject.SetActive(true);    // 이 스크립트가 붙어있는 오브젝트 본인

                this.gameObject.SetActive(false);   // 누르는 버튼은 꺼주기
            }
        }
        else
        {
            InGameUI.Instance.UIStack.Push(this.gameObject);
            Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);

            if (InGameUI.Instance.UIStack.Count != 0)
            {
                if (m_UI.gameObject.activeSelf == false)
                {
                    InGameUI.Instance.UIStack.Pop();
                    m_UI.gameObject.SetActive(true);    // 이 스크립트가 붙어있는 오브젝트 본인
                    Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);

                    //InGameUI.Instance.UIStack.Push(m_UI.gameObject);
                    this.gameObject.SetActive(false);   // 누르는 버튼은 꺼주기
                                                        //Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);
                }
            }

            if (SceneManager.GetActiveScene().name == "InGameScene")
            {
                if (GameTime.Instance != null)
                {
                    GameTime.Instance.IsGameMode = false;
                }
                Time.timeScale = 0;

                Debug.Log("시간 멈춤");
            }
        }
    }

    // 움직이는 UI의 속도
    [SerializeField]
    float moveSpeed = 0;

    [SerializeField]            // 이동시키고 싶은 좌표
    float movedPositionX = 0;

    Vector3 prevMovedPosition = new Vector3(0, 0, 0);

    // UI 메뉴가 누르면 팝업으로 나오는 함수 (가로 이동)
    public void SlidePopUpUI()
    {
        // 현재 오브젝트 좌표와 이동시킬 좌표가 같지 않을 때
        if (m_UI.transform.position != target)
        {

            prevMovedPosition = m_UI.transform.position;
            m_UI.transform.position = target;
        }
        else
        {
            m_UI.transform.position = prevMovedPosition;
            prevMovedPosition = new Vector3(0, 0, 0);
        }

    }
}
