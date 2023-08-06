using System.Collections.Generic;

//namespace StatData.Runtime
//{
//    /// <summary>
//    /// ���¿� �ִ� �л��� ������ �ҷ����� ���� Ŭ����
//    /// 
//    /// </summary>

//}

public class StudentStat
{
    public string m_StudentID;      // �𵨸� ã�¿�
    public string m_StudentName;
    public string m_UserSettingName;
    public int m_Health;            // �ִ� 100, �ּ� 0
    public int m_Passion;           // �ִ� 100, �ּ� 0
    public int m_Gender;            // ����
    //public int[,] m_Familiarity;  // ���� ���� �������ִ� �л��� ����ŭ ������ �������
    public StudentType m_StudentType;      // �� ��Ʈ

    //public StatType m_StudentStatType;
    // ��Ʈ���� �л����� ������ �ִ� ���� ����(Ư�� ��ġ�� �����ϸ� ��ų�� ����)
    public int[] m_AbilityAmountArr = new int[5];
    public int[] m_AbilitySkills = new int[5];      // Ư�� ��ġ�� �����ϸ� ��Ʈ�� �л����� ��� �� �⺻ ���� ��ų�� ����
    public List<string> m_Skills;                   // Ư�� ������ �������� �� ���� ���ʽ� ��ų

    // �帣
    public int[] m_GenreAmountArr = new int[8];

    // ����
    public string m_Personality;

    public int m_NumberOfEntries;
    public bool m_IsActiving;       // ���� �������̳� �̺�Ʈ���� ���õǾ� �İ�������
    public bool m_IsRecommend;      // ��õ�� �޾Ҵ��� �ƴ��� �Ǻ�
}

public enum DetaildeAbilitySkillGameDesigner
{
    // ��ȹ
    Businesspower,                  // �����
    Understanding,                  // ���ط�
    Creativity,                     // â�Ƿ�
    Communicationskills,            // �����
    AnalyticalPower,                // �м���
}

public enum DetaildeAbilitySkillArt
{
    // ��Ʈ
    SenseOfSpace,                   // ������
    Imagination,                    // ����
    Expressiveness,                 // ǥ����
    Technique,                      // ��ũ��
    ColorSense,                     // ������
}


public enum DetaildeAbilitySkillProgramming
{
    // �ù�
    Madness,                        // ����
    PowerOfImplementation,          // ������
    Utilization,                    // Ȱ���
    PowerOfInquiry,                 // Ž����
    Logic                           // ����
}

public enum AbilityType
{
    Insight,
    Concentration,
    Sense,
    Technique,
    Wit,
    Count
}

public enum StudentType
{
    GameDesigner,
    Art,
    Programming,
    None,
    Count
}

public enum GenreStat
{
    Puzzle = 0,
    Simulation,
    Rhythm,
    Adventure,
    RPG,
    Sports,
    Action,
    Shooting,
    Count,
}