using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private List<string> m_DesignClass = new List<string>();         // 기획
    private List<string> m_ArtClass = new List<string>();           // 아트
    private List<string> m_ProgrammingClass = new List<string>();   // 플머

    // private List<Class> m_Design = new List<Class>();        // 수연이의 데이터와 연결 하면 쓸 변수

    #region EachClass_Property
    public List<string> DesignClass
    {
        get { return m_DesignClass; }
        set { m_DesignClass = value; }
    }

    public List<string> ArtClass
    {
        get { return m_ArtClass; }
        set { m_ArtClass = value; }
    }
    public List<string> ProgrammingClass
    {
        get { return m_ProgrammingClass; }
        set { m_ProgrammingClass = value; }
    }
    #endregion
}


public class ClassSchedule : MonoBehaviour
{
    EachClass m_NowPlayerClass;

    GameObject m_nowObj;

    public Image m_img1;
    public Image m_img2;
    public Image m_img3;
    Type m_ClassType;

    // public 

    //Color m_newColor;

    // Start is called before the first frame update
    void Start()
    {
        if(m_ClassType == Type.Art)
        {
            // for(int i = 0; i< )
        }

        m_NowPlayerClass = new EachClass();

        for (int i = 1; i < 13; i++)
        {
            m_NowPlayerClass.DesignClass.Add(i.ToString());
            m_NowPlayerClass.ArtClass.Add(i.ToString());
            m_NowPlayerClass.ProgrammingClass.Add(i.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 각 클래스의 월별을 정하기 위한 
    public void SetEachOfClass()
    {
        if (this.gameObject.name == "ProductManagerC_Button")
        {
            Debug.Log("기획반");

            //Debug.Log(m_NowPlayerClass);

            // m_NowClassButton.gameObject

            // this.gameObject.

            GameObject gobj = EventSystem.current.currentSelectedGameObject;

        }
        if (this.gameObject.name == "ArtC_Button")
        {
            Debug.Log("아트반");
        }
        if (this.gameObject.name == "ProgrammingC_Button")
        {
            Debug.Log("플밍반");
        }
    }

    public void ChangeColor()
    {
        if (this.gameObject.name == "ProductManagerC_Button")
        {
            Debug.Log("기획반");

            m_img1.color = new Color(1.0f, 0.3726415f, 0.3726415f, 1.0f);
            m_img2.color = new Color(1.0f, 0.3726415f, 0.3726415f, 1.0f);
            m_img3.color = new Color(1.0f, 0.3726415f, 0.3726415f, 1.0f);
        }
        if (this.gameObject.name == "ArtC_Button")
        {
            Debug.Log("아트반");

            m_img1.color = new Color(1.0f, 0.7988446f, 0.0f, 1.0f);
            m_img2.color = new Color(1.0f, 0.7988446f, 0.0f, 1.0f);
            m_img3.color = new Color(1.0f, 0.7988446f, 0.0f, 1.0f);
        }
        if (this.gameObject.name == "ProgrammingC_Button")
        {
            Debug.Log("플밍반");

            m_img1.color = new Color(0.0f, 0.9893317f, 1.0f, 1.0f);
            m_img2.color = new Color(0.0f, 0.9893317f, 1.0f, 1.0f);
            m_img3.color = new Color(0.0f, 0.9893317f, 1.0f, 1.0f);

        }
    }
}
