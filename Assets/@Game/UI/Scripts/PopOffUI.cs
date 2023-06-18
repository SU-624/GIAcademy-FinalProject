using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Mang 10.17 
/// 
/// UI의 PopOff 를 담당해 줄 UI 스크립트
/// </summary>
public class PopOffUI : MonoBehaviour
{
    public GameObject m_UI;         // 지정할 UI
    public int m_DelayTime;

    //[SerializeField] private SelectClass m_ClassPrefab;
    //[SerializeField] private SelecteProfessor m_ProfessorPrefab;

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

        if (SceneManager.GetActiveScene().name == "InGameScene")
        {
            if (GameTime.Instance != null)
            {
                GameTime.Instance.IsGameMode = true;
            }

            Time.timeScale = InGameUI.Instance.m_NowGameSpeed;

            Debug.Log("시간 흐름");
        }
    }

    // UI를 꺼주는 시간 조정 함수
    public void DelayTurnOffUI()
    {
        Invoke("TurnOffUI", m_DelayTime);
    }

    public void BackButton()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            m_UI.SetActive(false);
        }
        else
        {
            // 버튼이 활성화 되어있는 지 체크
            if (m_UI.activeSelf == true)
            {
                m_UI.SetActive(false);

                // 수연이의 학생 프리팹이 존재하는지 체크
                //if (m_ClassPrefab != null)
                //{
                //    if (m_ClassPrefab.m_SelecteClassDataList != null)
                //    {
                //        m_ClassPrefab.m_SelecteClassDataList.Clear();
                //    }

                //    if (m_ClassPrefab.m_SelecteClassButtonName != null)
                //    {
                //        m_ClassPrefab.m_SelecteClassButtonName.Clear();
                //    }
                //}

                //if (m_ProfessorPrefab != null)
                //{
                //    m_ProfessorPrefab.m_ProfessorDataIndex = 0;

                //    if (m_ProfessorPrefab.m_ForClickNextButton != null)
                //    {
                //        m_ProfessorPrefab.m_ForClickNextButton.Clear();
                //    }
                //}

                InGameUI.Instance.UIStack.Pop();
                Debug.Log(InGameUI.Instance.UIStack.Count);
            }

            if (InGameUI.Instance.UIStack.Count == 0)
            {
                // 인게임 씬에서만 시간이 체크되도록 체크
                if (SceneManager.GetActiveScene().name == "InGameScene")
                {
                    if (GameTime.Instance != null)
                    {
                        GameTime.Instance.IsGameMode = true;
                    }
                    Time.timeScale = InGameUI.Instance.m_NowGameSpeed;

                    Debug.Log("시간 흐름");
                }
                else if(SceneManager.GetActiveScene().name == "TestScene")
                {
                    if (GameTime.Instance != null)
                    {
                        GameTime.Instance.IsGameMode = true;
                    }
                    Time.timeScale = InGameUI.Instance.m_NowGameSpeed;

                }
            }
        }
    }

    // 뒤로가기 버튼을 눌렀을 때
    public void TurnOffUI()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            // 버튼이 활성화 되어있는 지 체크
            if (m_UI.activeSelf == true)
            {
                m_UI.SetActive(false);
            }
        }
        else
        {
            if (InGameUI.Instance.UIStack.Count != 0)
            {
                // 버튼이 활성화 되어있는 지 체크
                if (m_UI.activeSelf == true)
                {
                    m_UI.SetActive(false);

                    // 수연이의 학생 프리팹이 존재하는지 체크
                    //if (m_ClassPrefab != null)
                    //{
                    //    if (m_ClassPrefab.m_SelecteClassDataList != null)
                    //    {
                    //        m_ClassPrefab.m_SelecteClassDataList.Clear();
                    //    }

                    //    if (m_ClassPrefab.m_SelecteClassButtonName != null)
                    //    {
                    //        m_ClassPrefab.m_SelecteClassButtonName.Clear();
                    //    }
                    //}

                    //if (m_ProfessorPrefab != null)
                    //{
                    //    m_ProfessorPrefab.m_ProfessorDataIndex = 0;

                    //    if (m_ProfessorPrefab.m_ForClickNextButton != null)
                    //    {
                    //        m_ProfessorPrefab.m_ForClickNextButton.Clear();
                    //    }
                    //}

                    InGameUI.Instance.UIStack.Pop();
                    Debug.Log("팝업 창 띄여진 갯수 : " + InGameUI.Instance.UIStack.Count);
                }
            }

            if (InGameUI.Instance.UIStack.Count == 0)
            {
                // 인게임 씬에서만 시간이 체크되도록 체크
                if (SceneManager.GetActiveScene().name == "InGameScene")
                {
                    if (GameTime.Instance != null)
                    {
                        GameTime.Instance.IsGameMode = true;
                    }
                    Time.timeScale = InGameUI.Instance.m_NowGameSpeed;

                    Debug.Log("시간 흐름");
                }
                else if (SceneManager.GetActiveScene().name == "TestScene")
                {
                    if (GameTime.Instance != null)
                    {
                        GameTime.Instance.IsGameMode = true;
                    }
                    Time.timeScale = InGameUI.Instance.m_NowGameSpeed;

                }
            }
        }
    }
        // 선택 완료를 눌렀을 때
        //public void SelecteComplete()
        //{
        //    if (m_UI.activeSelf == true)
        //    {
        //        m_UI.SetActive(false);

        //        if (m_ClassPrefab.m_SelecteClassDataList != null)
        //        {
        //            m_ClassPrefab.m_SelecteClassDataList.Clear();
        //        }
        //    }
        //}
}