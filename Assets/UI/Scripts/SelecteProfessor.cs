using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using StatData.Runtime;

public class Professor
{
    private List<ProfessorStat> m_ProductManagerProfessor = new List<ProfessorStat>();
    private List<ProfessorStat> m_ArtProfessor = new List<ProfessorStat>();
    private List<ProfessorStat> m_ProgrammingProfessor = new List<ProfessorStat>();

    #region _Professor_Property
    public List<ProfessorStat> ProductManagerProcessor
    {
        get { return m_ProductManagerProfessor; }
        set { m_ProductManagerProfessor = value; }
    }

    public List<ProfessorStat> ArtProcessor
    {
        get { return m_ArtProfessor; }
        set { m_ArtProfessor = value; }
    }

    public List<ProfessorStat> ProgrammingProfessor
    {
        get { return m_ProgrammingProfessor; }
        set { m_ProgrammingProfessor = value; }
    }
    #endregion
}

/// <summary>
/// 교수 선택을 위한 스크립트. 
/// 
/// 
/// </summary>
public class SelecteProfessor : MonoBehaviour
{
    private static SelecteProfessor _instance = null;

    [SerializeField] private GameObject m_SelecteProfessor;
    [SerializeField] private ProfessorController m_LoadProfessorData;
    [SerializeField] private ClassPrefab m_ClassPrefabData;
    [SerializeField] private GameObject m_NextProfessorInfoButton;
    [SerializeField] private GameObject m_PreviousProfessorInfoButton;

    public GameObject m_SelecteClassName1;
    public GameObject m_SelecteClassName2;
    public GameObject m_SelecteClassName3;

    public GameObject m_ChoiceButton1;
    public GameObject m_ChoiceButton2;
    public GameObject m_ChoiceButton3;
    
    public GameObject m_ProfessorName1;
    public GameObject m_ProfessorName2;
    public GameObject m_ProfessorName3;

    public Professor m_NowPlayerProfessor = new Professor();

    public int m_ProfessorDataIndex; // 여러명의 교수가 있을 때 리스트에서 다음 교수의 정보를 가져와 띄워주기 위한 인덱스 변수

    public Stack<int> m_ForClickNextButton = new Stack<int>(); // 다음 버튼을 누를 때 내가 가지고있는 교수의 수에 맞게 버튼이 보일 수 있게 해주기 위한 리스트

    public static SelecteProfessor Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        m_ProfessorDataIndex = 0;

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// 내가 가지고 있는 교수의 수만큼 각 타입의 리스트에 나눠서 넣어준다.
    void Start()
    {
        for (int i = 0; i < m_LoadProfessorData.professorData.Count; i++)
        {
            if (m_LoadProfessorData.professorData.ElementAt(i).Value.m_ProfessorType == Type.Art)
            {
                m_NowPlayerProfessor.ArtProcessor.Add(m_LoadProfessorData.professorData.ElementAt(i).Value);
            }

            if (m_LoadProfessorData.professorData.ElementAt(i).Value.m_ProfessorType == Type.ProductManager)
            {
                m_NowPlayerProfessor.ProductManagerProcessor.Add(m_LoadProfessorData.professorData.ElementAt(i).Value);

            }

            if (m_LoadProfessorData.professorData.ElementAt(i).Value.m_ProfessorType == Type.Programming)
            {
                m_NowPlayerProfessor.ProgrammingProfessor.Add(m_LoadProfessorData.professorData.ElementAt(i).Value);

            }
        }
    }

    /// 교수 선택 창을 띄울 때 옆으로 버튼을 띄우는지 확인하는 함수
    public void SetArrowButtons()
    {
        // 교수가 1명이거나 리스트의 제일 마지막
        if (m_ForClickNextButton.Count == 0)
        {
            m_NextProfessorInfoButton.SetActive(true);
            m_PreviousProfessorInfoButton.SetActive(false);

        }
        else
        {
            m_NextProfessorInfoButton.SetActive(true);
            m_PreviousProfessorInfoButton.SetActive(true);
        }
    }

    /// 교수의 정보를 띄워주는 함수
    public void ProfessorInfo()
    {
        m_SelecteProfessor.SetActive(true);

        int _tempIndex = 0; // 리스트에 들어있는 수업 중 내가 선택한 수업의 인덱스를 찾아주기 위해 잠시 만든 temp 변수.

        if (m_ClassPrefabData.m_SelecteClassDataList.Count > 0)
        {
            _tempIndex = m_ClassPrefabData.m_SelecteClassDataList.Count - 1;
        }
        else
        {
            _tempIndex = m_ClassPrefabData.m_SelecteClassDataList.Count;
        }

        GameObject _classInfo = GameObject.Find("ClassDataPanel");
        GameObject _professorInfo = GameObject.Find("ProfessorInfoPanel");

        // 현재는 수업의 스탯이름을 고정으로 해놨지만 추후 수업이 가지고 있는 정보를 토대로 만들어줘야 함.
        _classInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassName;
        _classInfo.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "System";
        _classInfo.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Content";
        _classInfo.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Balance";

        // 내가 선택한 수업에 맞는 타입의 교수 정보를 띄워야 한다.
        if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Art)
        {
            // 매 번마다 옆으로 넘기는 버튼을 만들지 확인을 해줘야 한다.
            SetArrowButtons();

            if (m_ForClickNextButton.Count == m_NowPlayerProfessor.ArtProcessor.Count - 1)
            {
                m_NextProfessorInfoButton.SetActive(false);
            }

            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;
        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.ProductManager)
        {
            SetArrowButtons();                

            if (m_ForClickNextButton.Count == m_NowPlayerProfessor.ProductManagerProcessor.Count - 1)
            {
                m_NextProfessorInfoButton.SetActive(false);
            }

            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProductManagerProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;
        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Programming)
        {
            SetArrowButtons();
            
            if (m_ForClickNextButton.Count == m_NowPlayerProfessor.ProgrammingProfessor.Count - 1)
            {
                m_NextProfessorInfoButton.SetActive(false);
            }

            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[m_ProfessorDataIndex].m_ProfessorNameValue;
        }
    }

    /// 교수가 여러명일 때 옆으로 버튼을 누르면 인덱스를 증가시켜줘서 다음 교수의 정보를 띄워줄 수 있게 해준다.
    public void ClickRightArrowButton()
    {
        m_ForClickNextButton.Push(m_ProfessorDataIndex);

        m_ProfessorDataIndex++;

        GameObject _professorInfo = GameObject.Find("ProfessorInfoPanel");

        int _tempIndex = m_ClassPrefabData.m_SelecteClassDataList.Count - 1;

        if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Art)
        {
            SetArrowButtons();
            
            if (m_ForClickNextButton.Count == m_NowPlayerProfessor.ArtProcessor.Count - 1)
            {
                m_NextProfessorInfoButton.SetActive(false);
            }
            
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;
        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.ProductManager)
        {
            SetArrowButtons();
            
            if (m_ForClickNextButton.Count == m_NowPlayerProfessor.ProductManagerProcessor.Count - 1)
            {
                m_NextProfessorInfoButton.SetActive(false);
            }
            
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProductManagerProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;
        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Programming)
        {
            SetArrowButtons();

            if (m_ForClickNextButton.Count == m_NowPlayerProfessor.ProgrammingProfessor.Count - 1)
            {
                m_NextProfessorInfoButton.SetActive(false);
            }
            
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[m_ProfessorDataIndex].m_ProfessorNameValue;
        }
    }

    /// 이전 교수의 스탯을 보기위해 버튼을 눌렀을 때 실행해줄 함수
    public void ClickLeftArrowButton()
    {
        m_ForClickNextButton.Pop();

        m_ProfessorDataIndex--;

        GameObject _professorInfo = GameObject.Find("ProfessorInfoPanel");

        int _tempIndex = m_ClassPrefabData.m_SelecteClassDataList.Count -1;

        if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Art)
        {
            SetArrowButtons();

            if (m_ForClickNextButton.Count == m_NowPlayerProfessor.ArtProcessor.Count - 1)
            {
                m_NextProfessorInfoButton.SetActive(false);
            }
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;
        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.ProductManager)
        {
            SetArrowButtons();

            if (m_ForClickNextButton.Count == m_NowPlayerProfessor.ProductManagerProcessor.Count - 1)
            {
                m_NextProfessorInfoButton.SetActive(false);
            }
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProductManagerProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;

        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Programming)
        {
            SetArrowButtons();

            if (m_ForClickNextButton.Count == m_NowPlayerProfessor.ProgrammingProfessor.Count - 1)
            {
                m_NextProfessorInfoButton.SetActive(false);
            }
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[m_ProfessorDataIndex].m_ProfessorNameValue;

        }
    }

    /// 모든 선택이 끝났을 때 (수업과 교수) 내가 여태 선택했던 것들의 데이터를 저장해뒀다가 화면에 띄워줘야 한다.
    public void SelecteComplete()
    {
        int _tempIndex = 0;

        if (m_ClassPrefabData.m_SelecteClassDataList.Count > 0)
        {
            _tempIndex = m_ClassPrefabData.m_SelecteClassDataList.Count - 1;
        }
        else
        {
            _tempIndex = m_ClassPrefabData.m_SelecteClassDataList.Count;
        }

        // 내가 선택한 버튼의 위치를 기억했다가 선택한 위치에 해당 수업의 정보를 넣어줘야한다.
        if (m_ClassPrefabData.m_SelecteClassButtonName != null)
        {
            if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Art)
            {
                m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave = m_NowPlayerProfessor.ArtProcessor[m_ProfessorDataIndex];
                m_ClassPrefabData.m_ArtData.Add(m_ClassPrefabData.m_SaveData);

                // 클릭한 버튼의 순서에 따라 리스트의 인덱스를 바꿔준다(Button1이면 인덱스 0번에 넣어주기)
                if (m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button1")
                {
                    m_ClassPrefabData.ChangeListIndex(m_ClassPrefabData.m_ArtData, 0, m_ClassPrefabData.m_SaveData);
                    m_ClassPrefabData.m_ArtData.RemoveAt(3);
                    m_ChoiceButton1.SetActive(false);
                    m_SelecteClassName1.SetActive(true);
                    m_ProfessorName1.SetActive(true);
                    m_SelecteClassName1.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteClassDataSave.m_ClassName;
                    m_ProfessorName1.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave.m_ProfessorNameValue;
                }
                else if (m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button2")
                {
                    m_ClassPrefabData.ChangeListIndex(m_ClassPrefabData.m_ArtData, 1, m_ClassPrefabData.m_SaveData);
                    m_ClassPrefabData.m_ArtData.RemoveAt(3);
                    m_ChoiceButton2.SetActive(false);
                    m_SelecteClassName2.SetActive(true);
                    m_ProfessorName2.SetActive(true);
                    m_SelecteClassName2.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteClassDataSave.m_ClassName;
                    m_ProfessorName2.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave.m_ProfessorNameValue;

                }
                else if (m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button3")
                {
                    m_ClassPrefabData.ChangeListIndex(m_ClassPrefabData.m_ArtData, 2, m_ClassPrefabData.m_SaveData);
                    m_ClassPrefabData.m_ArtData.RemoveAt(3);
                    m_ChoiceButton3.SetActive(false);
                    m_SelecteClassName3.SetActive(true);
                    m_ProfessorName3.SetActive(true);
                    m_SelecteClassName3.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteClassDataSave.m_ClassName;
                    m_ProfessorName3.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave.m_ProfessorNameValue;
                }
            }
            else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.ProductManager)
            {
                m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave = m_NowPlayerProfessor.ProductManagerProcessor[m_ProfessorDataIndex];
                m_ClassPrefabData.m_ProductManagerData.Add(m_ClassPrefabData.m_SaveData);

                if (m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button1")
                {
                    m_ClassPrefabData.ChangeListIndex(m_ClassPrefabData.m_ProductManagerData, 0, m_ClassPrefabData.m_SaveData);
                    m_ClassPrefabData.m_ProductManagerData.RemoveAt(3);
                    m_ChoiceButton1.SetActive(false);
                    m_SelecteClassName1.SetActive(true);
                    m_ProfessorName1.SetActive(true);
                    m_SelecteClassName1.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteClassDataSave.m_ClassName;
                    m_ProfessorName1.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave.m_ProfessorNameValue;
                }
                else if (m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button2")
                {
                    m_ClassPrefabData.ChangeListIndex(m_ClassPrefabData.m_ProductManagerData, 1, m_ClassPrefabData.m_SaveData);
                    m_ClassPrefabData.m_ProductManagerData.RemoveAt(3);
                    m_ChoiceButton2.SetActive(false);
                    m_SelecteClassName2.SetActive(true);
                    m_ProfessorName2.SetActive(true);
                    m_SelecteClassName2.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteClassDataSave.m_ClassName;
                    m_ProfessorName2.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave.m_ProfessorNameValue;

                }
                else if (m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button3")
                {
                    m_ClassPrefabData.ChangeListIndex(m_ClassPrefabData.m_ProductManagerData, 2, m_ClassPrefabData.m_SaveData);
                    m_ClassPrefabData.m_ProductManagerData.RemoveAt(3);
                    m_ChoiceButton3.SetActive(false);
                    m_SelecteClassName3.SetActive(true);
                    m_ProfessorName3.SetActive(true);
                    m_SelecteClassName3.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteClassDataSave.m_ClassName;
                    m_ProfessorName3.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave.m_ProfessorNameValue;
                }
            }
            else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Programming)
            {
                m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave = m_NowPlayerProfessor.ProgrammingProfessor[m_ProfessorDataIndex];
                m_ClassPrefabData.m_ProgrammingData.Add(m_ClassPrefabData.m_SaveData);

                if (m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button1")
                {
                    m_ClassPrefabData.ChangeListIndex(m_ClassPrefabData.m_ProgrammingData, 0, m_ClassPrefabData.m_SaveData);
                    m_ClassPrefabData.m_ProgrammingData.RemoveAt(3);
                    m_ChoiceButton1.SetActive(false);
                    m_SelecteClassName1.SetActive(true);
                    m_ProfessorName1.SetActive(true);
                    m_SelecteClassName1.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteClassDataSave.m_ClassName;
                    m_ProfessorName1.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave.m_ProfessorNameValue;
                }
                else if (m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button2")
                {
                    m_ClassPrefabData.ChangeListIndex(m_ClassPrefabData.m_ProgrammingData, 1, m_ClassPrefabData.m_SaveData);
                    m_ClassPrefabData.m_ProgrammingData.RemoveAt(3);
                    m_ChoiceButton2.SetActive(false);
                    m_SelecteClassName2.SetActive(true);
                    m_ProfessorName2.SetActive(true);
                    m_SelecteClassName2.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteClassDataSave.m_ClassName;
                    m_ProfessorName2.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave.m_ProfessorNameValue;

                }
                else if (m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button3")
                {
                    m_ClassPrefabData.ChangeListIndex(m_ClassPrefabData.m_ProgrammingData, 2, m_ClassPrefabData.m_SaveData);
                    m_ClassPrefabData.m_ProgrammingData.RemoveAt(3);
                    m_ChoiceButton3.SetActive(false);
                    m_SelecteClassName3.SetActive(true);
                    m_ProfessorName3.SetActive(true);
                    m_SelecteClassName3.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteClassDataSave.m_ClassName;
                    m_ProfessorName3.GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave.m_ProfessorNameValue;
                }
            }
        }
    }
}
