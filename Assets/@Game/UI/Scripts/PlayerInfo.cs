using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Mang 11. 08
/// 
/// 플레이어 자체의 정보
/// 여기에 플레이어가 들고있을 데이터들을 다 넣어두면 최종본의 데이터저장소가 됨
/// </summary>
public class PlayerInfo
{
    public List<StudentSaveData> studentData = new List<StudentSaveData>();

    private static PlayerInfo instance = null;       // Manager 변수는 싱글톤으로 사용

    public static PlayerInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerInfo();
            }
            return instance;
        }
    }

    // 로그인 데이터
    public string m_PlayerID;
    public string m_AcademyName;
    public string m_PrincipalName;

    public int m_MyMoney;
    public int m_SpecialPoint;     // 2차 재화입니다

    public string m_PlayerServer;



    //public int[,] testArr = new int[3, 4];



    // 퀘스트
    // 캘린더

    // 보유 교사
    // 보유 학생
    // 메일함

    //public void SaveNowTime()
    //{
    //    m_Year = GameTime.Instance.FlowTime.NowYear;
    //    m_Month = GameTime.Instance.FlowTime.NowMonth;
    //    m_Week = GameTime.Instance.FlowTime.NowWeek;
    //    m_Day = GameTime.Instance.FlowTime.NowDay;
    //}

}
