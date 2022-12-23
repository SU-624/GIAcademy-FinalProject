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
    public string m_DirectorName;


    // 인게임 데이터
    // 보유 교사
    // 보유 학생
    // 보유 금액
    public int m_MyMoney = 10000;
    // 퀘스트
    // 메일함
    // 캘린더
    // 친구
    // 날짜

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
