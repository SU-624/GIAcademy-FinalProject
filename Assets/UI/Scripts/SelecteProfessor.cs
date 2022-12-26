using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private GameObject m_SelecteProfessor;
    [SerializeField] private ProfessorController m_LoadProfessorData;
    [SerializeField] private ClassPrefab m_ClassPrefabData;

    public Professor m_NowPlayerProfessor = new Professor();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_LoadProfessorData.professorData.Count; i++)
        {
            if(m_LoadProfessorData.professorData.ElementAt(i).Value.m_ProfessorType == Type.Art)
            {
                m_NowPlayerProfessor.ArtProcessor.Add(m_LoadProfessorData.professorData.ElementAt(i).Value);
            }
            
            if (m_LoadProfessorData.professorData.ElementAt(i).Value.m_ProfessorType == Type.ProductManager)
            {
                m_NowPlayerProfessor.ArtProcessor.Add(m_LoadProfessorData.professorData.ElementAt(i).Value);

            }

            if (m_LoadProfessorData.professorData.ElementAt(i).Value.m_ProfessorType == Type.Programming)
            {
                m_NowPlayerProfessor.ArtProcessor.Add(m_LoadProfessorData.professorData.ElementAt(i).Value);

            }
        }
    }

    public void ProfessorInfo()
    {
        m_SelecteProfessor.SetActive(true);

        GameObject _classInfo = GameObject.Find("ClassInfo");


    }
}
