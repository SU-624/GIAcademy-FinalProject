using System.Collections.Generic;

//namespace StatData.Runtime
//{
//    /// <summary>
//    /// 에셋에 있는 학생의 스탯을 불러오기 위한 클래스
//    /// 
//    /// </summary>

//}

public class StudentStat
{
    public string m_StudentID;      // 모델링 찾는용
    public string m_StudentName;
    public string m_UserSettingName;
    public int m_Health;            // 최대 100, 최소 0
    public int m_Passion;           // 최대 100, 최소 0
    public int m_Gender;            // 성별
    //public int[,] m_Familiarity;  // 내가 현재 데리고있는 학생의 수만큼 설정을 해줘야함
    public StudentType m_StudentType;      // 각 파트

    //public StatType m_StudentStatType;
    // 파트별로 학생들이 가지고 있는 공통 스탯(특정 수치에 도달하면 스킬을 얻음)
    public int[] m_AbilityAmountArr = new int[5];
    public int[] m_AbilitySkills = new int[5];      // 특정 수치에 도달하면 파트별 학생들이 얻게 될 기본 스탯 스킬의 레벨
    public List<string> m_Skills;                   // 특정 조건을 만족했을 때 얻을 보너스 스킬

    // 장르
    public int[] m_GenreAmountArr = new int[8];

    // 성격
    public string m_Personality;

    public int m_NumberOfEntries;
    public bool m_IsActiving;       // 현재 게임잼이나 이벤트에서 선택되어 파견중인지
    public bool m_IsRecommend;      // 추천을 받았는지 아닌지 판별
}

public enum DetaildeAbilitySkillGameDesigner
{
    // 기획
    Businesspower,                  // 사업력
    Understanding,                  // 이해력
    Creativity,                     // 창의력
    Communicationskills,            // 소통력
    AnalyticalPower,                // 분석력
}

public enum DetaildeAbilitySkillArt
{
    // 아트
    SenseOfSpace,                   // 공간감
    Imagination,                    // 상상력
    Expressiveness,                 // 표현력
    Technique,                      // 테크닉
    ColorSense,                     // 색감각
}


public enum DetaildeAbilitySkillProgramming
{
    // 플밍
    Madness,                        // 광기
    PowerOfImplementation,          // 구현력
    Utilization,                    // 활용력
    PowerOfInquiry,                 // 탐구력
    Logic                           // 논리력
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