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
    // �л��� �������ִ� �⺻ ����. (ü��, ����, ģ�е� ��)
    public string m_StudentName;
    public int m_Health;            // �ִ� 100, �ּ� 0
    public int m_Passion;           // �ִ� 100, �ּ� 0
    //public int[,] m_Familiarity;  // ���� ���� �������ִ� �л��� ����ŭ ������ �������
    public StudentType m_StudentType;      // �� ��Ʈ

    // ��Ʈ���� �л����� ������ �ִ� ���� ����(Ư�� ��ġ�� �����ϸ� ��ų�� ����)
    //public StatType m_StudentStatType;
    public int m_Sense;
    public int m_Concentraction;
    public int m_Wit;
    public int m_Technique;
    public int m_Insight;
    public List<string> m_Skills;

    // �帣
    public int m_Action;
    public int m_Simulation;
    public int m_Adventure;
    public int m_Shooting;
    public int m_RPG;
    public int m_Puzzle;
    public int m_Rythm;
    public int m_Sport;

    // ����
    public string m_Personality;

    //public StudentStat(string _studentName, int _health, int _passion, float[,] _familiarity, StudentType _studentType,
    //            int _sense, int _concentraction, int _wit, int _technique, int _insight, List<StudentSkills> _skills,
    //            int _action, int _simulation, int _adventure, int _shoothing, int _rpg, int _puzzle, int _rythm, int _sport)
    //{
    //    m_StudentName = _studentName;
    //    m_Health = _health;
    //    m_Passion = _passion;
    //    m_Familiarity = _familiarity;
    //    m_StudentType = _studentType;
        
    //    m_Sense = _sense;
    //    m_Concentraction = _concentraction;
    //    m_Wit = _wit;
    //    m_Technique = _technique;
    //    m_Insight = _insight;
    //    m_Skills = _skills;

    //    m_Action = _action;
    //    m_Simulation = _simulation;
    //    m_Adventure = _adventure;
    //    m_Shooting = _shoothing;
    //    m_RPG = _rpg;
    //    m_Puzzle = _puzzle;
    //    m_Rythm = _rythm;
    //    m_Sport = _sport;
    //}

}

public enum StatType
{
    Sense,
    Concentration,
    Wit,
    Technique,
    Insight
}

public enum StudentType
{
    Programming,
    Art,
    GameDesigner,
}

