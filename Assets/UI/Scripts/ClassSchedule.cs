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
/// Mang_�÷��̾ ������ ��ȹ, ��Ʈ, �ù� �� �� ���� ���� ��
/// Ocean_������ Ÿ�Կ� ���� ������ �� �� �ְ� ���ֱ�
/// </summary>

public class EachClass
{
    // �� Ŭ������ ���� �������� ������ ������
    private List<string> m_DesignClass = new List<string>();         // ��ȹ
    private List<string> m_ArtClass = new List<string>();           // ��Ʈ
    private List<string> m_ProgrammingClass = new List<string>();   // �ø�

    // private List<Class> m_Design = new List<Class>();        // �������� �����Ϳ� ���� �ϸ� �� ����

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

    // �� Ŭ������ ������ ���ϱ� ���� 
    public void SetEachOfClass()
    {
        if (this.gameObject.name == "ProductManagerC_Button")
        {
            Debug.Log("��ȹ��");

            //Debug.Log(m_NowPlayerClass);

            // m_NowClassButton.gameObject

            // this.gameObject.

            GameObject gobj = EventSystem.current.currentSelectedGameObject;

        }
        if (this.gameObject.name == "ArtC_Button")
        {
            Debug.Log("��Ʈ��");
        }
        if (this.gameObject.name == "ProgrammingC_Button")
        {
            Debug.Log("�ùֹ�");
        }
    }

    public void ChangeColor()
    {
        if (this.gameObject.name == "ProductManagerC_Button")
        {
            Debug.Log("��ȹ��");

            m_img1.color = new Color(1.0f, 0.3726415f, 0.3726415f, 1.0f);
            m_img2.color = new Color(1.0f, 0.3726415f, 0.3726415f, 1.0f);
            m_img3.color = new Color(1.0f, 0.3726415f, 0.3726415f, 1.0f);
        }
        if (this.gameObject.name == "ArtC_Button")
        {
            Debug.Log("��Ʈ��");

            m_img1.color = new Color(1.0f, 0.7988446f, 0.0f, 1.0f);
            m_img2.color = new Color(1.0f, 0.7988446f, 0.0f, 1.0f);
            m_img3.color = new Color(1.0f, 0.7988446f, 0.0f, 1.0f);
        }
        if (this.gameObject.name == "ProgrammingC_Button")
        {
            Debug.Log("�ùֹ�");

            m_img1.color = new Color(0.0f, 0.9893317f, 1.0f, 1.0f);
            m_img2.color = new Color(0.0f, 0.9893317f, 1.0f, 1.0f);
            m_img3.color = new Color(0.0f, 0.9893317f, 1.0f, 1.0f);

        }
    }
}
