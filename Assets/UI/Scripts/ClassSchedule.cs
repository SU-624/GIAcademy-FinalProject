using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using StatData.Runtime;

/// <summary>
/// Mang 2022. 11. 21
/// Ocean 2022. 12. 22
/// 
/// Mang_플레이어가 선택할 기획, 아트, 플밍 각 월 수업 정할 것
/// Ocean_선택한 타입에 따라 수업이 뜰 수 있게 해주기
/// </summary>

public class EachClass
{
    // 각 클래스의 월별 스케줄을 저장할 데이터
    private List<Class> m_ProductManagerClass = new List<Class>();         // 기획
    private List<Class> m_ArtClass = new List<Class>();           // 아트
    private List<Class> m_ProgrammingClass = new List<Class>();   // 플머

    // private List<Class> m_Design = new List<Class>();        // 수연이의 데이터와 연결 하면 쓸 변수

    #region EachClass_Property
    public List<Class> ProductManagerClass
    {
        get { return m_ProductManagerClass; }
        set { m_ProductManagerClass = value; }
    }

    public List<Class> ArtClass
    {
        get { return m_ArtClass; }
        set { m_ArtClass = value; }
    }
    public List<Class> ProgrammingClass
    {
        get { return m_ProgrammingClass; }
        set { m_ProgrammingClass = value; }
    }
    #endregion
}


public class ClassSchedule : MonoBehaviour
{
    private static ClassSchedule _instance = null;

    [SerializeField] private GameObject m_SelecteClassArea1;
    [SerializeField] private GameObject m_SelecteClassArea2;
    [SerializeField] private GameObject m_SelecteClassArea3;
    [SerializeField] private GameObject m_MonthClassSpace;

    [SerializeField] private ClassController m_LoadClassData;
    [SerializeField] private ClassPrefab m_ClassPrefab;
    [SerializeField] private TextMeshProUGUI m_ClickClass;
    [SerializeField] private SelecteProfessor m_ChangeData;

    private List<GameObject> m_ManagementSelecteClassName = new List<GameObject>(); // 수업 이름 관리하는 리스트
    private List<GameObject> m_ManagementChoiceButton = new List<GameObject>(); // 지정된 수업의 지정버튼 없애는 리스트
    private List<GameObject> m_ManagementChoiceProfessorName = new List<GameObject>(); // 지정된 수업의 지정버튼 없애는 리스트

    public EachClass m_NowPlayerClass = new EachClass();
    //public List<string> m_

    public static ClassSchedule Instance
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

    void Start()
    {
        for (int i = 0; i < m_LoadClassData.classData.Count; i++)
        {
            if (m_LoadClassData.classData.ElementAt(i).Value.m_ClassType == Type.Art)
            {
                m_NowPlayerClass.ArtClass.Add(m_LoadClassData.classData.ElementAt(i).Value);
                Debug.Log("아트수업");
                Debug.Log(m_LoadClassData.classData.ElementAt(i).Value.m_ClassName);
            }

            if (m_LoadClassData.classData.ElementAt(i).Value.m_ClassType == Type.ProductManager)
            {
                m_NowPlayerClass.ProductManagerClass.Add(m_LoadClassData.classData.ElementAt(i).Value);
                Debug.Log("기획수업");
                Debug.Log(m_LoadClassData.classData.ElementAt(i).Value.m_ClassName);
            }

            if (m_LoadClassData.classData.ElementAt(i).Value.m_ClassType == Type.Programming)
            {
                m_NowPlayerClass.ProgrammingClass.Add(m_LoadClassData.classData.ElementAt(i).Value);
                Debug.Log("플밍수업");
                Debug.Log(m_LoadClassData.classData.ElementAt(i).Value.m_ClassName);
            }
        }

        m_ManagementSelecteClassName.Add(m_ChangeData.m_SelecteClassName1);
        m_ManagementSelecteClassName.Add(m_ChangeData.m_SelecteClassName2);
        m_ManagementSelecteClassName.Add(m_ChangeData.m_SelecteClassName3);

        m_ManagementChoiceButton.Add(m_ChangeData.m_ChoiceButton1);
        m_ManagementChoiceButton.Add(m_ChangeData.m_ChoiceButton2);
        m_ManagementChoiceButton.Add(m_ChangeData.m_ChoiceButton3);

        m_ManagementChoiceProfessorName.Add(m_ChangeData.m_ProfessorName1);
        m_ManagementChoiceProfessorName.Add(m_ChangeData.m_ProfessorName2);
        m_ManagementChoiceProfessorName.Add(m_ChangeData.m_ProfessorName3);
    }

    // 각 클래스의 월별을 정하기 위한 
    public void SetEachOfClass()
    {
        // 총 3개의 선택을 할 수있는 창이 떠야함. 방금 선택한 버튼의 이름으로 생성하기 그래야 수업이 어느 반의 수업인지 알기 쉬움.
        GameObject gobj = EventSystem.current.currentSelectedGameObject;

        // 일단은 먼저 다 비워준다.
        for (int i = 0; i < 3; i++)
        {
            m_ManagementSelecteClassName[i].SetActive(false);
            m_ManagementChoiceButton[i].SetActive(true);
            m_ManagementChoiceProfessorName[i].SetActive(false);
        }

        if (gobj.name == "ProductManagerC_Button")
        {
            Debug.Log("기획반");
            m_ClickClass.text = "[기획]반 스케쥴을 정해주세요.";
            m_SelecteClassArea1.name = gobj.name + "1";
            m_SelecteClassArea2.name = gobj.name + "2";
            m_SelecteClassArea3.name = gobj.name + "3";

            // 맨 마지막에 선택을 완료했을 때 저장했던 정보들을 저장한 m_ProductManagerData리스트가 비어있는지 확인해서 비어있지 않다면 해당 정보를 꺼내와서 넣어준다.
            for (int i = 0; i < 3; i++)
            {
                if (m_ClassPrefab.m_ProductManagerData[i].m_ClassName != null)
                {
                    m_ManagementSelecteClassName[i].SetActive(true);
                    m_ManagementChoiceButton[i].SetActive(false);
                    m_ManagementChoiceProfessorName[i].SetActive(true);
                    m_ManagementSelecteClassName[i].GetComponent<TextMeshProUGUI>().text = m_ClassPrefab.m_ProductManagerData[i].m_SelecteClassDataSave.m_ClassName;
                    m_ManagementChoiceProfessorName[i].GetComponent<TextMeshProUGUI>().text = m_ClassPrefab.m_ProductManagerData[i].m_SelecteProfessorDataSave.m_ProfessorNameValue;
                }
            }
            m_ClassPrefab.m_SaveData.m_ClassName = "ProductManager";
        }

        if (gobj.name == "ArtC_Button")
        {
            Debug.Log("아트반");
            m_ClickClass.text = "[아트]반 스케쥴을 정해주세요.";
            //GameObject _button = GameObject.Instantiate(m_SelecteClassArea, m_MonthClassSpace.transform);
            m_SelecteClassArea1.name = gobj.name + "1";
            m_SelecteClassArea2.name = gobj.name + "2";
            m_SelecteClassArea3.name = gobj.name + "3";

            for (int i = 0; i < 3; i++)
            {
                if (m_ClassPrefab.m_ArtData[i].m_ClassName != null)
                {
                    m_ManagementSelecteClassName[i].SetActive(true);
                    m_ManagementChoiceButton[i].SetActive(false);
                    m_ManagementChoiceProfessorName[i].SetActive(true);
                    m_ManagementSelecteClassName[i].GetComponent<TextMeshProUGUI>().text = m_ClassPrefab.m_ArtData[i].m_SelecteClassDataSave.m_ClassName;
                    m_ManagementChoiceProfessorName[i].GetComponent<TextMeshProUGUI>().text = m_ClassPrefab.m_ArtData[i].m_SelecteProfessorDataSave.m_ProfessorNameValue;
                }
            }

            m_ClassPrefab.m_SaveData.m_ClassName = "Art";
        }

        if (gobj.name == "ProgrammingC_Button")
        {
            Debug.Log("플밍반");
            m_ClickClass.text = "[프로그래밍]반 스케쥴을 정해주세요.";
            //GameObject.Instantiate(m_SelecteClassArea, m_MonthClassSpace.transform);
            m_SelecteClassArea1.name = gobj.name + "1";
            m_SelecteClassArea2.name = gobj.name + "2";
            m_SelecteClassArea3.name = gobj.name + "3";
            
            for (int i = 0; i < 3; i++)
            {
                if (m_ClassPrefab.m_ProgrammingData[i].m_ClassName != null)
                {
                    m_ManagementSelecteClassName[i].SetActive(true);
                    m_ManagementChoiceButton[i].SetActive(false);
                    m_ManagementChoiceProfessorName[i].SetActive(true);
                    m_ManagementSelecteClassName[i].GetComponent<TextMeshProUGUI>().text = m_ClassPrefab.m_ProgrammingData[i].m_SelecteClassDataSave.m_ClassName;
                    m_ManagementChoiceProfessorName[i].GetComponent<TextMeshProUGUI>().text = m_ClassPrefab.m_ProgrammingData[i].m_SelecteProfessorDataSave.m_ProfessorNameValue;
                }
            }

            m_ClassPrefab.m_SaveData.m_ClassName = "Programming";
        }

    }

    //public void ChangeColor()
    //{
    //    if (this.gameObject.name == "ProductManagerC_Button")
    //    {
    //        Debug.Log("기획반");

    //    }
    //    if (this.gameObject.name == "ArtC_Button")
    //    {
    //        Debug.Log("아트반");

    //    }
    //    if (this.gameObject.name == "ProgrammingC_Button")
    //    {
    //        Debug.Log("플밍반");


    //    }
    //}
}
