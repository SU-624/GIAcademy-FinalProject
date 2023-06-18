using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public RectTransform movingMenuRect;
    float target;
    // menu 의 원 위치를 가질 변수
    Vector3 prevMEnuBarPos;
    public RectTransform pointPos;
    float tempLength;

    public bool isSlideMenuPanelOpend = false;

    public void Start()
    {
        float screenSize = Screen.width;
        if (this.transform.name == "Fold_Button")
        {
            tempLength = pointPos.position.x - screenSize;     // 이동할 만큼의 거리
            target = movingMenuRect.position.x - tempLength;      // 이동할 위치
        }
    }
    // UI를 켜주는 시간 조정 함수
    public void DelayTurnOnUI(/*int t*/)
    {
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
            if (InGameUI.Instance.UIStack.Count != 0)
            {
                if (!InGameUI.Instance.UIStack.Contains(m_UI.gameObject))
                {
                    InGameUI.Instance.UIStack.Push(m_UI.gameObject);
                }
            }
            else
            {
                InGameUI.Instance.UIStack.Push(m_UI.gameObject);
            }

            Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);
            if (InGameUI.Instance.UIStack.Count != 0)
            {
                m_UI.gameObject.SetActive(true);

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
                else if (SceneManager.GetActiveScene().name == "TestScene")
                {
                    if (GameTime.Instance != null)
                    {
                        GameTime.Instance.IsGameMode = false;
                    }
                    Time.timeScale = 0;

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
            if (InGameUI.Instance.UIStack.Count != 0)
            {
                if (!InGameUI.Instance.UIStack.Contains(this.gameObject))
                {
                    InGameUI.Instance.UIStack.Push(this.gameObject);
                }
            }
            else
            {
                InGameUI.Instance.UIStack.Push(this.gameObject);
            }

            Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);

            if (InGameUI.Instance.UIStack.Count != 0)
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
            else if (SceneManager.GetActiveScene().name == "TestScene")
            {
                if (GameTime.Instance != null)
                {
                    GameTime.Instance.IsGameMode = false;
                }
                Time.timeScale = 0;

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
            if (InGameUI.Instance.UIStack.Count != 0)
            {
                if (!InGameUI.Instance.UIStack.Contains(this.gameObject))
                {
                    InGameUI.Instance.UIStack.Push(this.gameObject);
                }
            }
            else
            {
                InGameUI.Instance.UIStack.Push(this.gameObject);
            }

            Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);

            if (InGameUI.Instance.UIStack.Count != 0)
            {
                if (m_UI.gameObject.activeSelf == false)
                {
                    InGameUI.Instance.UIStack.Pop();
                    m_UI.gameObject.SetActive(true);    // 이 스크립트가 붙어있는 오브젝트 본인
                    Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);
                    Debug.Log("stack Name : " + m_UI.name);

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
            else if (SceneManager.GetActiveScene().name == "TestScene")
            {
                if (GameTime.Instance != null)
                {
                    GameTime.Instance.IsGameMode = false;
                }
                Time.timeScale = 0;

            }
        }
    }

    public void PopUpUIOnTitleScene(GameObject obj)
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            obj.SetActive(true);
        }
    }


    // 펼쳐지는 메뉴 만듬!
    public void AutoSlideMenuUI()
    {
        if ((int)movingMenuRect.position.x != (int)target)      // 슬라이드메뉴패널이 들어가있을때
        {
            isSlideMenuPanelOpend = true;
            prevMEnuBarPos = movingMenuRect.position;       // 이전의 위치를 기억해서 돌아갈 자리

            movingMenuRect.position = new Vector3(target, movingMenuRect.position.y, movingMenuRect.position.z);
        }
        else if ((int)movingMenuRect.position.x == (int)target)
        {
            isSlideMenuPanelOpend = false;

            movingMenuRect.position = prevMEnuBarPos;
        }
    }
}