using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conditiondata.Runtime;

/// <summary>
///  Mang 23. 02. 08
/// 
/// Json ȭ �� ������ �����
/// ���⿡ �����͵��� �� �־�θ� �������� ����������Ұ� ��
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

        set
        {
            instance = value;
        }
    }

    public PlayerSaveData player = new PlayerSaveData();                            // �÷��̾��� �ΰ��� ����
    public List<StudentSaveData> studentData = new List<StudentSaveData>();         // ���� ����ϴ� �л� ����
    public InGameSaveData InGameData = new InGameSaveData();                        // �ΰ��ӿ� �ʿ��� �����͵�
    public List<MailSaveData> MailData = new List<MailSaveData>();                  // ���� �� ������ ����
    public List<TeacherSaveData> proffesorData = new List<TeacherSaveData>();   // ���� ����ϴ� ���� ����
    public List<EventSaveData> EventData = new List<EventSaveData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SaveAllJsonData()
    {
        SavePlayerData();

        SaveInGameData();

        SaveEventData();
    }

    // �� Json���� ���� �� �����͵��� ��ȭ�Ǵ� ���� ���� �Լ����� �־��ֱ�
    private void SavePlayerData()
    {
        player.m_Money = PlayerInfo.Instance.m_MyMoney;
        player.m_SpecialPoint = PlayerInfo.Instance.m_SpecialPoint;         // 2�� ��ȭ�Դϴ�
        player.m_AcademyName = PlayerInfo.Instance.m_AcademyName;
        player.m_PrincipalName = PlayerInfo.Instance.m_PrincipalName;
    }

    private void SaveInGameData()
    {
        InGameData.year = GameTime.Instance.FlowTime.NowYear;
        InGameData.month = GameTime.Instance.FlowTime.NowMonth;
        InGameData.week = GameTime.Instance.FlowTime.NowWeek;
        InGameData.day = GameTime.Instance.FlowTime.NowDay;
    }

    private void SaveEventData()
    {
        for (int i = 0; i < EventData.Count; i++)
        {
            EventData.Remove(EventData[i]);
        }

        // ����Ʈ �̹Ƿ� �ݺ����� ���鼭 �����͸� �־�����
        for (int i = 0; i < SwitchEventList.Instance.MyEventList.Count; i++)
        {
            EventSaveData tempData = new EventSaveData();

            tempData.EventNumber = SwitchEventList.Instance.MyEventList[i].EventNumber;

            tempData.EventDate[0] = SwitchEventList.Instance.MyEventList[i].EventDate[0];
            tempData.EventDate[1] = SwitchEventList.Instance.MyEventList[i].EventDate[1];
            tempData.EventDate[2] = SwitchEventList.Instance.MyEventList[i].EventDate[2];
            tempData.EventDate[3] = SwitchEventList.Instance.MyEventList[i].EventDate[3];

            tempData.IsPossibleUseEvent = SwitchEventList.Instance.MyEventList[i].IsPossibleUseEvent;
            tempData.EventName = SwitchEventList.Instance.MyEventList[i].EventClassName;
            tempData.EventInformation = SwitchEventList.Instance.MyEventList[i].EventInformation;
            // tempData.IsPopUp = SwitchEventList.Instance.MyEventList[i].IsPopUp;     // �ҷ����� �� �˾��� �� ������ �ƴ��� üũ? ������ ���ڷ� �ϸ� �ʿ䰡 ����

            tempData.EventRewardMoney = SwitchEventList.Instance.MyEventList[i].EventRewardMoney;
            tempData.EventRewardSpecialPoint = SwitchEventList.Instance.MyEventList[i].EventRewardSpecialPoint;

            tempData.EventRewardStat[0] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[0];       // �л�����
            tempData.EventRewardStat[1] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[1];       // ȫ������
            tempData.EventRewardStat[2] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[2];       // ����
            tempData.EventRewardStat[3] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[3];       // ������

            EventData.Add(tempData);
        }
    }

    // ���� �ε�� ���⼭ ���� ���� ������ �����Ͱ� �ʿ��� ������ ���� �־��ְ� ���� �ѱ����� �ϴ°� �� ���ϴٰ� �����ߴµ� �̷��� �ȵɰ� ����?
    // �ֳ� ������ ��ũ��Ʈ�� ��������� �κ��� �ٸ��� �����̴�
    // AllInOneData.cs -> MonoBehaviors�� ��ӹ��� �ʱ� ������ Json.cs �� Awake���� json�� ������ �� �־��ֵ��� ����
    public void LoadAllJsonData()
    {
        // LoadInGameData();
        // LoadNowMonthEventData();
    }

    public void LoadInGameData()
    {
        // �ΰ��� ��¥
        GameTime.Instance.FlowTime.NowYear = InGameData.year;
        GameTime.Instance.FlowTime.NowMonth = InGameData.month;
        GameTime.Instance.FlowTime.NowWeek = InGameData.week;
        GameTime.Instance.FlowTime.NowDay = InGameData.day;
    }

    public void LoadNowMonthEventData()
    {
        for (int i = 0; i < EventData.Count; i++)
        {
            SaveEventData tempData = new SaveEventData();

            tempData.EventNumber = EventData[i].EventNumber;

            tempData.EventDate[0] = EventData[i].EventDate[0];
            tempData.EventDate[1] = EventData[i].EventDate[1];
            tempData.EventDate[2] = EventData[i].EventDate[2];
            tempData.EventDate[3] = EventData[i].EventDate[3];

            tempData.IsPossibleUseEvent = EventData[i].IsPossibleUseEvent;
            tempData.EventClassName = EventData[i].EventName;
            tempData.EventInformation = EventData[i].EventInformation;
            // tempData.IsPopUp = SwitchEventList.Instance.MyEventList[i].IsPopUp;     // �ҷ����� �� �˾��� �� ������ �ƴ��� üũ? ������ ���ڷ� �ϸ� �ʿ䰡 ����

            tempData.EventRewardMoney = EventData[i].EventRewardMoney;
            tempData.EventRewardSpecialPoint = EventData[i].EventRewardSpecialPoint;

            tempData.EventRewardStat[0] = EventData[i].EventRewardStat[0];       // �л�����
            tempData.EventRewardStat[1] = EventData[i].EventRewardStat[1];       // ȫ������
            tempData.EventRewardStat[2] = EventData[i].EventRewardStat[2];       // ����
            tempData.EventRewardStat[3] = EventData[i].EventRewardStat[3];       // ������

            SwitchEventList.Instance.MyEventList.Add(tempData);
        }
    }
    // �л� -> objectManager ��  Awake���� ������ �ε� �ϰ�
    // ���� -> ChangeMailContent ���� ���� �ε�, ���� ���� ���� �ε� 
}


public class PlayerSaveData
{
    public string m_playerID;
    public string m_AcademyName;
    public string m_PrincipalName;

    public int m_Money;
    public int m_SpecialPoint;
}


// �ʿ��� ������ �����̳ʸ� ������
public class StudentSaveData
{
    public Student student;
    public StudentStat studentStat;
    public StudentCondition studentCondition;

}

public class InGameSaveData
{
    public int year;
    public int month;
    public int week;
    public int day;

    //Dpublic int[,] studentFriendshipExp = new int[10, 10];       // �̰� ���� ���� �ƴ�
}

public class MailSaveData
{
    public bool IsSendedMail;
    public bool IsReadMail;
}

public class TeacherSaveData
{
    public string TeacherName;

}

public class EventSaveData
{
    public int EventNumber;
    public int[] EventDate = new int[4];
    public bool IsPossibleUseEvent;
    public bool IsFixedEvent;
    public string EventName;
    public string EventInformation;
    // public bool IsPopUp;

    public float[] EventRewardStat = new float[4];
    public int EventRewardMoney;                    // ���� - �Ӵ�
    public int EventRewardSpecialPoint;             // 2�� ��ȭ �Դϵ�

}