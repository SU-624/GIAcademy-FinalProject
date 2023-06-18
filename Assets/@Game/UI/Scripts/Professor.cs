using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using StatData.Runtime;

public class Professor
{
    // ������ ���� -> ��ü ������ ���� ���� / ���� ����ϴ� ���� �а��� ����
    private List<ProfessorStat> m_GameManagerProfessor = new List<ProfessorStat>();
    private List<ProfessorStat> m_ArtProfessor = new List<ProfessorStat>();
    private List<ProfessorStat> m_ProgrammingProfessor = new List<ProfessorStat>();

    // ���Ƿ¿� ���� ���� �Ϸ�� ���� ����
    public float[] m_StatMagnification = new float[16]
        { 0, 1.1f, 1.11f, 1.12f, 1.13f, 1.14f, 1.2f, 1.21f, 1.22f, 1.23f, 1.3f, 1.33f, 1.35f, 1.40f, 1.43f, 1.45f };

    // �� �������� �������� �ʿ��� ����ġ
    public int[] m_ExperiencePerLevel = new int[16]
        { 0, 100, 200, 270, 400, 450, 700, 750, 800, 900, 1200, 1300, 1400, 1500, 1600, 0 };

    // Ŭ���� ���� ����ġ
    public int[] m_ExperiencePerClick = new int[16]
        { 0, 30, 45, 55, 70, 80, 88, 97, 103, 110, 113, 115, 117, 119, 121, 0 };

    // Ŭ���� �Ҹ� 2����ȭ
    public int[] m_PointPerClick = new int[16]
        { 0, 50, 70, 90, 100, 120, 200, 230, 260, 300, 400, 430, 480, 550, 680, 0 };

    // ���� ����ϴ� ��� ����
    // private List<ProfessorStat> m_AllProfessor = new List<ProfessorStat>();

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

    // public List<ProfessorStat> AllProfessor
    // {
    //     get { return m_AllProfessor; }
    //     set { m_AllProfessor = value; }
    // }

    #endregion
}