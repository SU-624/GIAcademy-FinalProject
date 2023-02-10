using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conditiondata.Runtime;

/// <summary>
///  Mang 23. 02. 08
/// 
/// Json 화 할 데이터 덩어리들
/// 여기에 데이터들을 다 넣어두면 최종본의 데이터저장소가 됨
/// </summary>
public class AllInOneData
{
    private static AllInOneData instance = null;

    public static AllInOneData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AllInOneData();
            }

            return instance;
        }
    }

    public PlayerSaveData player = new PlayerSaveData();
    public List<StudentSaveData> studentData = new List<StudentSaveData>();
    public InGameSaveData InGameData = new InGameSaveData();
    public List<MailSaveData> MailData = new List<MailSaveData>();
    public List<ProffesorSaveData> proffesorData = new List<ProffesorSaveData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    // PlayerInfo.Instance.m_Month   
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //}
}


public class PlayerSaveData
{
    public string m_playerID;
    public string m_AcademyName;
    public string m_DirectorName;
}

public class StudentSaveData
{
    public Student student;
    public StudentStat studentStat;
    public StudentCondition studentCondition;

}

public class InGameSaveData
{
    public int money;

    public int year;
    public int month;
    public int week;
    public int day;

    //Dpublic int[,] studentFriendshipExp = new int[10, 10];       // 이건 아직 예정 아님
}

public class MailSaveData
{
    public bool IsSendedMail;
    public bool IsReadMail;
}

public class ProffesorSaveData
{
    public string proffesorName;

}