using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 2023. 06. 12 Mang
/// 
/// 기획적으로 게임의 방향성이 부족하다 는 생각에 퀘스트가 생김.
/// 
/// </summary>
public class Mission : MonoBehaviour
{
    [Header("팝업 퀘스트 패널")]
    [SerializeField] private Image QuestPanel;              // 총 퀘스트 패널
    [Space(5f)]
    [SerializeField] private Image NowMissionStepStarImg1;   // 미션 단계 별 이미지
    [SerializeField] private Image NowMissionStepStarImg2;   // 미션 단계 별 이미지
    [SerializeField] private Image NowMissionStepStarImg3;   // 미션 단계 별 이미지
    [SerializeField] private Image NowMissionStepStarImg4;   // 미션 단계 별 이미지
    [SerializeField] private Image NowMissionStepStarImg5;   // 미션 단계 별 이미지

    [Space(5f)]
    [SerializeField] private MissionPrefab Mission1;
    [SerializeField] private MissionPrefab Mission2;
    [SerializeField] private MissionPrefab Mission3;
    [Space(5f)]


    // 퀘스트의 진행도를 체크하고, 값을 변경 해 줄 변수들
    private int NowMissionStepCount = 0;        // 현재 1~ 5단계 까지의 퀘스트 스텝

    private bool[] NowMissionClearCount = new bool[3];
    private bool isMissionAllClear = false;

    private bool isMissionPanelOpen = false;      // 패널이 열렸는지 체크할 변수

    List<MissionData> MyMissionList = new List<MissionData>();                  // 총 원본 미션 리스트
    List<MissionPrefab> EachMissionPrefabList = new List<MissionPrefab>();      // 각 미션 프리팹을 담아줄 리스트

    public void Start()
    {
        MyMissionList = AllOriginalJsonData.Instance.OriginalMissionData;

        if (Json.Instance.IsSavedDataExists)                     // 플레이 정보가 있다면
        {
            for (int i = 0; i < MyMissionList.Count; i++)
            {
                MyMissionList[i].IsMissionClear = PlayerInfo.Instance.isMissionClear[i];
            }
        }
        else
        {
            NowMissionStepCount = 1;

            NowMissionClearCount[0] = false;
            NowMissionClearCount[1] = false;
            NowMissionClearCount[2] = false;
        }

        EachMissionPrefabList.Add(Mission1);
        EachMissionPrefabList.Add(Mission2);
        EachMissionPrefabList.Add(Mission3);

        Init();

        InGameUI.Instance.GetQuestOpenButton.onClick.AddListener(IfClickMissionPanelButton);
    }

    //public void Update()
    //{
    //    if (isMissionAllClear == false)
    //    {
    //        MissionDispather(MyMissionList);
    //        MissionDispather(NowStepMissionList[1], NowMissionInfo[1]);
    //        MissionDispather(NowStepMissionList[2], NowMissionInfo[2]);

    //        DrawMissionPercentageUI();      // ui 에 글 적어주기
    //    }
    //    else if (isMissionAllClear == true)
    //    {

    //        NextMissionStep();
    //    }
    //}

    // 유아이 프리팹의 기본 설정 켜고 꺼놓기
    public void Init()
    {
        QuestPanel.gameObject.SetActive(false);

        NowMissionStepStarImg1.gameObject.SetActive(true);
        NowMissionStepStarImg2.gameObject.SetActive(false);
        NowMissionStepStarImg3.gameObject.SetActive(false);
        NowMissionStepStarImg4.gameObject.SetActive(false);
        NowMissionStepStarImg5.gameObject.SetActive(false);

        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            EachMissionPrefabList[i].GetCheckBoxCheckedImg.gameObject.SetActive(false);
            EachMissionPrefabList[i].GetClearQuestButton.gameObject.SetActive(false);

            EachMissionPrefabList[i].GetMissionAmountText.gameObject.SetActive(false);

            EachMissionPrefabList[i].GetMissionReward1AmountText.gameObject.SetActive(false);
            EachMissionPrefabList[i].GetMissionReward1AmountText.gameObject.SetActive(false);

            EachMissionPrefabList[i].GetClearQuestButton.gameObject.SetActive(false);
            EachMissionPrefabList[i].GetQuestClearImg.gameObject.SetActive(false);

        }

        InitMissionData();
    }

    //// 인게임패널의 퀘스트 버튼을 클릭 시 현재 퀘스트 목록의 리스트를 보여 줄 함수
    public void IfClickMissionPanelButton()
    {
        if (QuestPanel.gameObject.activeSelf)
        {
            QuestPanel.gameObject.SetActive(false);
        }
        else
        {
            QuestPanel.gameObject.SetActive(true);
        }
    }

    //// 미션 스텝이 바뀔 때마다 유아이 데이터를 교환시켜주는 함수
    public void InitMissionData()
    {
        int nowStepMissionCount = 0;

        for (int i = 0; i < MyMissionList.Count; i++)
        {
            if (NowMissionStepCount == MyMissionList[i].MissionStep)
            {
                EachMissionPrefabList[i].GetQuestName.text = MyMissionList[i].MissionName;          // UI - 이름 리셋 해주기
                EachMissionPrefabList[i].GetQuestSlideBar.value = 0;                                // 슬라이더 바도 리셋 해주기
                nowStepMissionCount++;
            }
        }

        if (NowMissionStepCount == 2)
        {
            NowMissionStepStarImg1.gameObject.SetActive(true);
            NowMissionStepStarImg2.gameObject.SetActive(true);
            NowMissionStepStarImg3.gameObject.SetActive(false);
            NowMissionStepStarImg4.gameObject.SetActive(false);
            NowMissionStepStarImg5.gameObject.SetActive(false);
        }
        else if (NowMissionStepCount == 3)
        {
            NowMissionStepStarImg1.gameObject.SetActive(true);
            NowMissionStepStarImg2.gameObject.SetActive(true);
            NowMissionStepStarImg3.gameObject.SetActive(true);
            NowMissionStepStarImg4.gameObject.SetActive(false);
            NowMissionStepStarImg5.gameObject.SetActive(false);
        }
        else if (NowMissionStepCount == 4)
        {
            NowMissionStepStarImg1.gameObject.SetActive(true);
            NowMissionStepStarImg2.gameObject.SetActive(true);
            NowMissionStepStarImg3.gameObject.SetActive(true);
            NowMissionStepStarImg4.gameObject.SetActive(true);
            NowMissionStepStarImg5.gameObject.SetActive(false);
        }
        else if (NowMissionStepCount == 5)
        {
            NowMissionStepStarImg1.gameObject.SetActive(true);
            NowMissionStepStarImg2.gameObject.SetActive(true);
            NowMissionStepStarImg3.gameObject.SetActive(true);
            NowMissionStepStarImg4.gameObject.SetActive(true);
            NowMissionStepStarImg5.gameObject.SetActive(true);
        }

        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {

            // 미션의 갯수에 따라 유아이의 active 상태 결정
            if (nowStepMissionCount == 1)
            {
                EachMissionPrefabList[i].GetNowMissionPrefab.gameObject.SetActive(true);
                EachMissionPrefabList[i].GetNowMissionPrefab.gameObject.SetActive(false);
                EachMissionPrefabList[i].GetNowMissionPrefab.gameObject.SetActive(false);
            }
            else if (nowStepMissionCount == 2)
            {
                EachMissionPrefabList[i].GetNowMissionPrefab.gameObject.SetActive(true);
                EachMissionPrefabList[i].GetNowMissionPrefab.gameObject.SetActive(true);
                EachMissionPrefabList[i].GetNowMissionPrefab.gameObject.SetActive(false);
            }
            else if (nowStepMissionCount == 3)
            {
                EachMissionPrefabList[i].GetNowMissionPrefab.gameObject.SetActive(true);
                EachMissionPrefabList[i].GetNowMissionPrefab.gameObject.SetActive(true);
                EachMissionPrefabList[i].GetNowMissionPrefab.gameObject.SetActive(true);
            }
        }

        if (NowMissionStepCount <= 5)
        {
            NowMissionStepCount++;
        }
        else if (6 <= NowMissionStepCount)
        {
            // 모든 미션 끝남 -> 미션 아이콘 자체를 꺼버린다.
            InGameUI.Instance.GetQuestOpenButton.gameObject.SetActive(false);
        }
    }

    // TODO : 계속 유아이에 데이터를 그려줄 부분
    public void DrawMissionPercentageUI()
    {
        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            // EachMissionPrefabList.
        }

        // // 미션 1
        // QuestSlideBar1.value = (NowMissionInfo[0].NowMissionTargetValue / NowMissionInfo[0].NowMissionValue);
        // Mission1AmountText.text = NowMissionInfo[0].NowMissionValue.ToString() + " / " + NowMissionInfo[0].NowMissionTargetValue.ToString();
        // // 미션 2
        // QuestSlideBar1.value = (NowMissionInfo[1].NowMissionTargetValue / NowMissionInfo[1].NowMissionValue);
        // Mission1AmountText.text = NowMissionInfo[1].NowMissionValue.ToString() + " / " + NowMissionInfo[1].NowMissionTargetValue.ToString();
        // // 미션 3
        // QuestSlideBar1.value = (NowMissionInfo[2].NowMissionTargetValue / NowMissionInfo[2].NowMissionValue);
        // Mission1AmountText.text = NowMissionInfo[2].NowMissionValue.ToString() + " / " + NowMissionInfo[2].NowMissionTargetValue.ToString();
    }

    //// 미션 조건 충족 시 -> 
    //public void IfMissionConditionAccept(MissionData NowMission)
    //{
    //    for (int i = 0; i < MyMissionList.Count; i++)
    //    {
    //        if (NowMission.MissionID == MyMissionList[i].MissionID)
    //        {

    //        }
    //    }
    //}

    //public int CountGenreRoomRepairCount(List<InteractionManager.GenreRoomInfo> genreRooms)
    //{
    //    int result = 0;

    //    result += genreRooms[(int)InteractionManager.SpotName.ActionRoom].RepairCount;
    //    result += genreRooms[(int)InteractionManager.SpotName.AdventureRoom].RepairCount;
    //    result += genreRooms[(int)InteractionManager.SpotName.PuzzleRoom].RepairCount;
    //    result += genreRooms[(int)InteractionManager.SpotName.RhythmRoom].RepairCount;

    //    result += genreRooms[(int)InteractionManager.SpotName.RPGRoom].RepairCount;
    //    result += genreRooms[(int)InteractionManager.SpotName.ShootingRoom].RepairCount;
    //    result += genreRooms[(int)InteractionManager.SpotName.SimulationRoom].RepairCount;
    //    result += genreRooms[(int)InteractionManager.SpotName.SportsRoom].RepairCount;

    //    return result;
    //}

    //public int CountTeacherAllLevel()
    //{
    //    int result = 0;

    //    for (int i = 0; i < ObjectManager.Instance.nowProfessor.GameManagerProfessor.Count; i++)
    //    {
    //        result += ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPower;
    //    }

    //    for (int i = 0; i < ObjectManager.Instance.nowProfessor.ArtProfessor.Count; i++)
    //    {
    //        result += ObjectManager.Instance.nowProfessor.ArtProfessor[i].m_ProfessorPower;
    //    }

    //    for (int i = 0; i < ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Count; i++)
    //    {
    //        result += ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorPower;
    //    }

    //    return result;
    //}

    //// 현재 단계의 미션들 조건을 계속 확인 해 줄 함수
    //public bool MissionDispather(List<MissionData> NowMission)
    //{
    //    NowMission

    //    int ConditionAmount = NowMission.ConditionAmountN;

    //    switch (NowMission.MissionCondition)          // 조건 체크
    //    {
    //        case 0:         // 조건이 없다면 오류이다
    //            {
    //                isMissionConditionAccept = false;
    //            }
    //            break;
    //        case 101:       // 1차 재화 N 이상 보유
    //            {
    //                // 유아이에 데이터 그려주기

    //                if (PlayerInfo.Instance.m_MyMoney >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;

    //                    // 함수로 이동, 퍼센테이지 조절 하는 함수
    //                    IfMissionConditionAccept(NowMission);
    //                }

    //                nowMissionStep.NowMissionValue = PlayerInfo.Instance.m_MyMoney;
    //            }
    //            break;
    //        case 102:       // 2차 재화 N 이상 보유
    //            {
    //                if (PlayerInfo.Instance.m_SpecialPoint >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 103:       // [모든 게임잼] 게임잼 N 회 참여
    //            {
    //                if (PlayerInfo.Instance.m_SpecialPoint >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 104:       // [모든 게임쇼] 게임쇼 N 회 참여
    //            {
    //                if (PlayerInfo.Instance.m_SpecialPoint >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 105:       // [모든 장르방 통틀어] 장르방 수리 N 회
    //            {
    //                List<InteractionManager.GenreRoomInfo> genreRooms = InteractionManager.Instance.GenreRoomList;
    //                int TotalGenreRoomReapairCount = 0;

    //                TotalGenreRoomReapairCount = CountGenreRoomRepairCount(genreRooms);


    //                if (TotalGenreRoomReapairCount >= ConditionAmount)
    //                {


    //                    isMissionConditionAccept = true;

    //                    IfMissionConditionAccept();
    //                }
    //            }
    //            break;
    //        case 106:       // 분기순위 N 위 이상 달성
    //            {
    //                if ((GameTime.Instance.FlowTime.NowMonth == 3 || GameTime.Instance.FlowTime.NowMonth == 6 || GameTime.Instance.FlowTime.NowMonth == 9 ||
    //                        GameTime.Instance.FlowTime.NowMonth == 12) && GameTime.Instance.FlowTime.NowDay == 5)
    //                {
    //                    int temp = QuarterlyReport.Instance.AcademyNameToRank.FindIndex(x => x.Key.Equals(PlayerInfo.Instance.m_AcademyName));

    //                    if (temp >= ConditionAmount)
    //                    {
    //                        isMissionConditionAccept = true;
    //                    }
    //                }
    //            }
    //            break;
    //        case 107:       // 랭크 INT(N) 등급 이상 달성
    //            {
    //                if ((int)PlayerInfo.Instance.m_CurrentRank - 1 >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;
    //                }
    //            }
    //            break;
    //        case 108:       // [모든 학생 프로필 통틀어] 학생 상세 프로필 N 회 확인
    //            {
    //                if (PlayerInfo.Instance.StudentProfileClickCount >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 109:       // [모든 강사 프로필 통틀어] 강사 상세 프로필 N 회 확인
    //            {
    //                if (PlayerInfo.Instance.TeacherProfileClickCount >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 110:       // 강사 ID 몇개 해금됬는지
    //            {
    //                if (PlayerInfo.Instance.UnLockTeacherCount >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 111:       // [모든 장르방 통틀어] 장르방 터치 N 회
    //            {
    //                if (PlayerInfo.Instance.GenreRoomClickCount >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        //case 112:       // 수업 ID N 실행
    //        //    {
    //        //        int tempClassTime = 0;
    //        //        // ClassInfo.FindLessonClassIDCount(_classCondition.ClassID, _classCondition.ClassType, _classCondition.Numberoflessons, SelectClass.m_GameDesignerData);

    //        //        if (tempClassTime >= ConditionAmount)
    //        //        {
    //        //            isMissionConditionAccept = true;


    //        //        }
    //        //    }
    //        //    break;
    //        case 113:       // [모든 강사 통틀어서] 강사 레벨 합 N 달성
    //            {
    //                int AllTeacherLevel = 0;

    //                AllTeacherLevel = CountTeacherAllLevel();

    //                if (AllTeacherLevel >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;

    //                    IfMissionConditionAccept();
    //                }
    //            }
    //            break;


    //    }

    //    return false; // 모든 조건을 만족한다면
    //}
}
