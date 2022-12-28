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

public class SelecteProfessor : MonoBehaviour
{
    private static SelecteProfessor _instance = null;

    [SerializeField] private GameObject m_SelecteProfessor;
    [SerializeField] private ProfessorController m_LoadProfessorData;
    [SerializeField] private ClassPrefab m_ClassPrefabData;

    public Professor m_NowPlayerProfessor = new Professor();

    public int m_ProfessorDataIndex;

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

    // Start is called before the first frame update
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

    // ������ ������ ����ִ� �Լ�
    public void ProfessorInfo()
    {
        m_SelecteProfessor.SetActive(true);

        int _tempIndex = 0;
        
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

        // ����� ������ �����̸��� �������� �س����� ���� ������ ������ �ִ� ������ ���� �������� ��.
        _classInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassName;
        //int i = _classInfo.transform.childCount;
        //Debug.Log(i);
        _classInfo.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "System";
        _classInfo.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Content";
        _classInfo.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Balance";

        if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Art)
        {
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;
        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.ProductManager)
        {
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProductManagerProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;

        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Programming)
        {
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[m_ProfessorDataIndex].m_ProfessorNameValue;

        }
    }

    public void ClickNextProfessorInfo()
    {
        m_ProfessorDataIndex++;

        GameObject _professorInfo = GameObject.Find("ProfessorInfo");

        int _tempIndex = m_ClassPrefabData.m_SelecteClassDataList.Count;

        if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Art)
        {
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;
        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.ProductManager)
        {
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProductManagerProcessor[m_ProfessorDataIndex].m_ProfessorNameValue;

        }
        else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Programming)
        {
            _professorInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[m_ProfessorDataIndex].m_ProfessorNameValue;

        }
    }

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

        if (m_ClassPrefabData.m_SelecteClassButtonName != null)
        {
            if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Art)
            {
                m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave = m_NowPlayerProfessor.ArtProcessor[m_ProfessorDataIndex];
                m_ClassPrefabData.m_ArtData.Add(m_ClassPrefabData.m_SaveData);

                // Ŭ���� ��ư�� ������ ���� ����Ʈ�� �ε����� �ٲ��ش�(Button1�̸� �ε��� 0���� �־��ֱ�)
                if(m_ClassPrefabData.m_SaveData.m_ClickPointDataSave == "1Week_Button1")
                {
                    /// ToDo
                }
            }
            else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.ProductManager)
            {
                m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave = m_NowPlayerProfessor.ProductManagerProcessor[m_ProfessorDataIndex];
                m_ClassPrefabData.m_ProductManagerData.Add(m_ClassPrefabData.m_SaveData);
            }
            else if (m_ClassPrefabData.m_SelecteClassDataList[_tempIndex].m_ClassType == Type.Programming)
            {
                m_ClassPrefabData.m_SaveData.m_SelecteProfessorDataSave = m_NowPlayerProfessor.ProgrammingProfessor[m_ProfessorDataIndex];
                m_ClassPrefabData.m_ProgrammingData.Add(m_ClassPrefabData.m_SaveData);
            }
        }
    }
}
