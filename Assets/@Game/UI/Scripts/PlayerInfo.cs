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
    // 5/15 woodpie9 
    // public List<StudentSaveData> studentData = new List<StudentSaveData>();

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
    // public string m_PlayerID;  // 미사용...
    public string m_AcademyName;
    public string m_TeacherName;

    public int m_MyMoney;
    public int m_SpecialPoint;              // 2차 재화입니다
    public int m_AcademyScore;
    public string m_PlayerServer;           // 서버 어디에다 저장했는지?

    public int m_Awareness;                 // 인지도 - 유명
    public int m_TalentDevelopment;         // 인재 양성
    public int m_Management;                // 운영
    public int m_Activity;                  // 활동점수
    public int m_Goods;                     // 재화점수
    public Rank m_CurrentRank;

    // 튜토리얼 여부
    public bool IsFirstAcademySetting;
    public bool IsFirstClassSetting;
    public bool IsFirstClassSettingPDEnd;
    public bool IsFirstGameJam;
    public bool IsFirstClassEnd;

    // 퀘스트
    public bool[] isMissionClear;               // 퀘스트 클리어 유무만 저장

    // 캘린더
    public int StudentProfileClickCount;        // 학생 상세 프로필 확인 카운트
    public int TeacherProfileClickCount;        // 강사 상세 프로필 확인 카운트
    public int GenreRoomClickCount;             // 모든 장르방 터치 카운트

    public int UnLockTeacherCount;



    // 보유 교사
    // 보유 학생
    // 메일함
}
