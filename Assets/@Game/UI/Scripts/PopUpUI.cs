using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Mang 10.18
/// 
/// UI�� PopUp ������ ������� ��ũ��Ʈ
/// </summary>
public class PopUpUI : MonoBehaviour
{
    public GameObject m_UI;         // ������ UI!
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
    // menu �� �� ��ġ�� ���� ����
    Vector3 prevMEnuBarPos;
    public RectTransform pointPos;
    float tempLength;

    public void Start()
    {
        float screenSize = Screen.width;
        if (this.transform.name == "Fold_Button")
        {
            tempLength = pointPos.position.x - screenSize;     // �̵��� ��ŭ�� �Ÿ�
            target = movingMenuRect.position.x - tempLength;      // �̵��� ��ġ
        }
    }
    // UI�� ���ִ� �ð� ���� �Լ�
    public void DelayTurnOnUI(/*int t*/)
    {
        //if(t != 0)
        //{
        //    m_DelayTime = t;
        //}

        Invoke("JustTurnOn", m_DelayTime);
    }

    // ��ư�� ������ �Ѱ�
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

                    Debug.Log("�ð� ����");
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

                Debug.Log("�ð� ����");
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

    // ���� �˾�â�� �� �ָ鼭 �� ���� UI �� ���ش�.
    public void PopUpMyUI()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            if (m_UI.gameObject.activeSelf == false)
            {
                m_UI.gameObject.SetActive(true);    // �� ��ũ��Ʈ�� �پ��ִ� ������Ʈ ����

                this.gameObject.SetActive(false);   // ������ ��ư�� ���ֱ�
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
                    m_UI.gameObject.SetActive(true);    // �� ��ũ��Ʈ�� �پ��ִ� ������Ʈ ����
                    Debug.Log("stack count : " + InGameUI.Instance.UIStack.Count);

                    //InGameUI.Instance.UIStack.Push(m_UI.gameObject);
                    this.gameObject.SetActive(false);   // ������ ��ư�� ���ֱ�
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

                Debug.Log("�ð� ����");
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

    // �������� �޴� ����� �;��µ� ���� �ȵ�
    public void AutoSlideMenuUI()
    {
        if ((int)movingMenuRect.position.x != (int)target)
        {
            prevMEnuBarPos = movingMenuRect.position;       // ������ ��ġ�� ����ؼ� ���ư� �ڸ�

            movingMenuRect.position = new Vector3(target, movingMenuRect.position.y, movingMenuRect.position.z);
        }
        else if ((int)movingMenuRect.position.x == (int)target)
        {
            movingMenuRect.position = prevMEnuBarPos;
        }

    }
}