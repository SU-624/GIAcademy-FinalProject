using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using StatData.Runtime;

public class Professor : MonoBehaviour
{
    private static Professor instance;

    public static Professor Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    // 각각의 교수 -> 전체 교수를 담을 건지 / 현재 사용하는 교수 학과별 모음
    [SerializeField] private ProfessorController m_SelectProfessor;

    private List<ProfessorStat> m_GameManagerProfessor = new List<ProfessorStat>();
    private List<ProfessorStat> m_ArtProfessor = new List<ProfessorStat>();
    private List<ProfessorStat> m_ProgrammingProfessor = new List<ProfessorStat>();

    // 강의력에 따른 수업 완료시 스탯 배율
    public float[] m_StatMagnification = new float[16]
        { 0, 1.1f, 1.11f, 1.12f, 1.13f, 1.14f, 1.2f, 1.21f, 1.22f, 1.23f, 1.3f, 1.33f, 1.35f, 1.40f, 1.43f, 1.45f };

    // 각 레벨마다 레벨업에 필요한 경험치
    public int[] m_ExperiencePerLevel = new int[16]
        { 0, 100, 200, 270, 400, 450, 700, 750, 800, 900, 1200, 1300, 1400, 1500, 1600, 0 };

    // 클릭당 오를 경험치
    public int[] m_ExperiencePerClick = new int[16]
        { 0, 30, 45, 55, 70, 80, 88, 97, 103, 110, 113, 115, 117, 119, 121, 0 };

    // 클릭당 소모 2차재화
    public int[] m_PointPerClick = new int[16]
        { 0, 50, 70, 90, 100, 120, 200, 230, 260, 300, 400, 430, 480, 550, 680, 0 };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (Json.Instance.UseLoadingData)
        {
            // 저장한 강사 정보를 사용한다.
            InitLoadProfessor();
        }
        else
        {
            // 원본에서 가져온다.
            InitProfessor();
        }

        ObjectManager.Instance.LinkInstructorDataToObject(); // 데이터를 넣는다
    }

    // 게임 시작 초기 사용가능한 강사를 찾아서 리스트에 넣어주는 함수 
    private void InitProfessor()
    {
        for (int i = 0; i < m_SelectProfessor.professorData.Count; i++)
        {
            if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.Art &&
                m_SelectProfessor.professorData.ElementAt(i).Value.m_IsUnLockProfessor == true)
            {
                if (!m_ArtProfessor.Contains(m_SelectProfessor.professorData.ElementAt(i).Value))
                {
                    m_ArtProfessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
                }
            }

            if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.GameDesigner &&
                m_SelectProfessor.professorData.ElementAt(i).Value.m_IsUnLockProfessor == true)
            {
                if (!m_GameManagerProfessor.Contains(m_SelectProfessor.professorData.ElementAt(i).Value))
                {
                    m_GameManagerProfessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
                }
            }

            if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.Programming &&
                m_SelectProfessor.professorData.ElementAt(i).Value.m_IsUnLockProfessor == true)
            {
                if (!m_ProgrammingProfessor.Contains(m_SelectProfessor.professorData.ElementAt(i).Value))
                {
                    m_ProgrammingProfessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
                }
            }
        }
    }

    private void InitLoadProfessor()
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        var setData = typeof(ProfessorStat).GetFields(flags);
        var getData = typeof(ProfessorSaveData).GetProperties(flags);

        foreach (var professor in AllInOneData.Instance.ProfessorData)
        {
            ProfessorStat data = new ProfessorStat();

            foreach (var set in setData)
            {
                foreach (var get in getData)
                {
                    if (set.Name == "m_" + get.Name)
                    {
                        var value = get.GetValue(professor);
                        set.SetValue(data, value);
                        break;
                    }
                }
            }

            // 이미지 넣어주기
            foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
            {
                if (data.m_ProfessorName == temp.ToString())
                {
                    data.m_TeacherProfileImg = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                }
            }

            if (professor.ProfessorType == StudentType.GameDesigner)
            {
                m_GameManagerProfessor.Add(data);
            }
            else if (professor.ProfessorType == StudentType.Art)
            {
                m_ArtProfessor.Add(data);
            }
            else if (professor.ProfessorType == StudentType.Programming)
            {
                m_ProgrammingProfessor.Add(data);
            }
        }


    }

    public int CalculateProfessorSalary()
    {
        int totalSalary = 0;

        for (int i = 0; i < m_GameManagerProfessor.Count; i++)
        {
            totalSalary += m_GameManagerProfessor[i].m_ProfessorPay;
        }

        for (int i = 0; i < m_ArtProfessor.Count; i++)
        {
            totalSalary += m_ArtProfessor[i].m_ProfessorPay;
        }

        for (int i = 0; i < m_ProgrammingProfessor.Count; i++)
        {
            totalSalary += m_ProgrammingProfessor[i].m_ProfessorPay;
        }

        return totalSalary;
    }

    #region _Professor_Property

    public List<ProfessorStat> GameManagerProfessor
    {
        get { return m_GameManagerProfessor; }
        set { m_GameManagerProfessor = value; }
    }

    public List<ProfessorStat> ArtProfessor
    {
        get { return m_ArtProfessor; }
        set { m_ArtProfessor = value; }
    }

    public List<ProfessorStat> ProgrammingProfessor
    {
        get { return m_ProgrammingProfessor; }
        set { m_ProgrammingProfessor = value; }
    }

    public ProfessorController SelectProfessor
    {
        get { return m_SelectProfessor; }
    }

    #endregion
}