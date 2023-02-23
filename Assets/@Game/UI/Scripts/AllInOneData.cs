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

        set
        {
            instance = value;
        }
    }

    public PlayerSaveData player = new PlayerSaveData();                            // 플레이어의 인게임 정보
    public List<StudentSaveData> studentData = new List<StudentSaveData>();         // 현재 사용하는 학생 정보
    public InGameSaveData InGameData = new InGameSaveData();                        // 인게임에 필요한 데이터들
    public List<MailSaveData> MailData = new List<MailSaveData>();                  // 게임 내 메일의 정보
    public List<TeacherSaveData> proffesorData = new List<TeacherSaveData>();   // 현재 사용하는 교수 정보
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

    // 각 Json으로 저장 할 데이터들의 변화되는 값을 여기 함수에서 넣어주기
    private void SavePlayerData()
    {
        player.m_Money = PlayerInfo.Instance.m_MyMoney;
        player.m_SpecialPoint = PlayerInfo.Instance.m_SpecialPoint;         // 2차 재화입니다
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

        // 리스트 이므로 반복문을 돌면서 데이터를 넣어주자
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
            // tempData.IsPopUp = SwitchEventList.Instance.MyEventList[i].IsPopUp;     // 불러오기 시 팝업이 된 것인지 아닌지 체크? 어차피 날자로 하면 필요가 없나

            tempData.EventRewardMoney = SwitchEventList.Instance.MyEventList[i].EventRewardMoney;
            tempData.EventRewardSpecialPoint = SwitchEventList.Instance.MyEventList[i].EventRewardSpecialPoint;

            tempData.EventRewardStat[0] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[0];       // 학생복지
            tempData.EventRewardStat[1] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[1];       // 홍보점수
            tempData.EventRewardStat[2] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[2];       // 버프
            tempData.EventRewardStat[3] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[3];       // 아이템

            EventData.Add(tempData);
        }
    }

    // 현재 로드는 여기서 쓰지 않음 각각의 데이터가 필요한 곳에서 직접 넣어주고 있음 한군데서 하는게 더 편하다고 생각했는데 이러면 안될것 같다?
    // 왜냐 각각의 스크립트가 만들어지는 부분이 다르기 때문이다
    // AllInOneData.cs -> MonoBehaviors를 상속받지 않기 때문에 Json.cs 의 Awake에서 json의 정보를 다 넣어주도록 하자
    public void LoadAllJsonData()
    {
        // LoadInGameData();
        // LoadNowMonthEventData();
    }

    public void LoadInGameData()
    {
        // 인게임 날짜
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
            // tempData.IsPopUp = SwitchEventList.Instance.MyEventList[i].IsPopUp;     // 불러오기 시 팝업이 된 것인지 아닌지 체크? 어차피 날자로 하면 필요가 없나

            tempData.EventRewardMoney = EventData[i].EventRewardMoney;
            tempData.EventRewardSpecialPoint = EventData[i].EventRewardSpecialPoint;

            tempData.EventRewardStat[0] = EventData[i].EventRewardStat[0];       // 학생복지
            tempData.EventRewardStat[1] = EventData[i].EventRewardStat[1];       // 홍보점수
            tempData.EventRewardStat[2] = EventData[i].EventRewardStat[2];       // 버프
            tempData.EventRewardStat[3] = EventData[i].EventRewardStat[3];       // 아이템

            SwitchEventList.Instance.MyEventList.Add(tempData);
        }
    }
    // 학생 -> objectManager 의  Awake에서 데이터 로드 하고
    // 메일 -> ChangeMailContent 에서 원본 로드, 현재 메일 상태 로드 
}


public class PlayerSaveData
{
    public string m_playerID;
    public string m_AcademyName;
    public string m_PrincipalName;

    public int m_Money;
    public int m_SpecialPoint;
}


// 필요한 데이터 컨테이너만 남기자
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

    //Dpublic int[,] studentFriendshipExp = new int[10, 10];       // 이건 아직 예정 아님
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
    public int EventRewardMoney;                    // 보상 - 머니
    public int EventRewardSpecialPoint;             // 2차 재화 입니덩

}