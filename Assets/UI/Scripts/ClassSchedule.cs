using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public EachClass m_NowPlayerClass = new EachClass();

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
    }

    // 각 클래스의 월별을 정하기 위한 
    public void SetEachOfClass()
    {
        // 총 3개의 선택을 할 수있는 창이 떠야함. 방금 선택한 버튼의 이름으로 생성하기 그래야 수업이 어느 반의 수업인지 알기 쉬움.
        GameObject gobj = EventSystem.current.currentSelectedGameObject;

        if (gobj.name == "ProductManagerC_Button")
        {
            Debug.Log("기획반");

            //GameObject _button = GameObject.Instantiate(m_SelecteClassArea, m_MonthClassSpace.transform);
            m_SelecteClassArea1.name = gobj.name + "1";
            m_SelecteClassArea2.name = gobj.name + "2";
            m_SelecteClassArea3.name = gobj.name + "3";
        }

        if (gobj.name == "ArtC_Button")
        {
            Debug.Log("아트반");

            //GameObject _button = GameObject.Instantiate(m_SelecteClassArea, m_MonthClassSpace.transform);
            m_SelecteClassArea1.name = gobj.name + "1";
            m_SelecteClassArea2.name = gobj.name + "2";
            m_SelecteClassArea3.name = gobj.name + "3";
        }

        if (gobj.name == "ProgrammingC_Button")
        {
            Debug.Log("플밍반");

            //GameObject.Instantiate(m_SelecteClassArea, m_MonthClassSpace.transform);
            m_SelecteClassArea1.name = gobj.name + "1";
            m_SelecteClassArea2.name = gobj.name + "2";
            m_SelecteClassArea3.name = gobj.name + "3";
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
