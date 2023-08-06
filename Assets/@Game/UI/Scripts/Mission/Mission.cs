using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Windows;

public struct MissionChecker
{
    public string MissionName;
    public string NowPercent;
}

/// <summary>
/// 2023. 06. 12 Mang
/// 
/// 기획적으로 게임의 방향성이 부족하다 는 생각에 퀘스트가 생김.
/// 
/// </summary>
public class Mission : MonoBehaviour
{
    [Header("팝업 퀘스트 패널")] [SerializeField] private Image QuestPanel; // 총 퀘스트 패널
    [Space(5f)] [SerializeField] private Image NowMissionStepStarImg1; // 미션 단계 별 이미지
    [SerializeField] private Image NowMissionStepStarImg2; // 미션 단계 별 이미지
    [SerializeField] private Image NowMissionStepStarImg3; // 미션 단계 별 이미지
    [SerializeField] private Image NowMissionStepStarImg4; // 미션 단계 별 이미지
    [SerializeField] private Image NowMissionStepStarImg5; // 미션 단계 별 이미지

    [Space(5f)] [SerializeField] private MissionPrefab Mission1;
    [SerializeField] private MissionPrefab Mission2;
    [SerializeField] private MissionPrefab Mission3;

    // 퀘스트의 진행도를 체크하고, 값을 변경 해 줄 변수들
    private int NowMissionStepCount = 0; // 1~ 5단계 까지 중 현재 미션 스텝

    private int NowStepGoalCount = 0; // 현재 스탭에서 미션을 끝내야 할 목표 카운트
    private int NowStepClearedCount = 0; // 나의 현재 스탭의 미션을 클리어 한 갯수
    private bool isNowStepMissionAllClear = false; // 현재 스텝의 미션을 모두 클리어 했는지 체크할 불 변수
    private bool isAllMissionClear = false; // 모든 스텝의 미션이 끝났는지 체크할 불 변수

    float prevTime = 0;
    float limitTime = 0.5f;

    GameObject tempMissionClearButton; // 내가 클릭한 현재 스텝의 미션 클리어 버튼 을 알기 위한  임시 클리어 버튼

    List<MissionData> MyMissionList = new List<MissionData>(); // 총 원본 미션 리스트

    // List<MissionData> missionChecker = new List<MissionData>();                 // 현재 스텝 
    List<MissionChecker> missionChecker = new List<MissionChecker>(); // 현재 단계의 미션만 담아서 돌면서 조건 체크해줄 리스트

    List<MissionPrefab> EachMissionPrefabList = new List<MissionPrefab>(); // 각 미션 프리팹을 담아줄 리스트

    [SerializeField] private SoundManager soundManager;

    public void Start()
    {
        MyMissionList.Clear();
        MyMissionList = AllOriginalJsonData.Instance.OriginalMissionData;

        missionChecker.Clear();
        EachMissionPrefabList.Clear();

        EachMissionPrefabList.Add(Mission1);
        EachMissionPrefabList.Add(Mission2);
        EachMissionPrefabList.Add(Mission3);

        if (Json.Instance.UseLoadingData) // 플레이 정보가 있다면 데이터 넣어주기
        {
            NowMissionStepCount = PlayerInfo.Instance.NowMissionStepCount;
            isAllMissionClear = PlayerInfo.Instance.IsAllMissionClear;

            for (int i = 0; i < MyMissionList.Count; i++)
            {
                MyMissionList[i].IsMissionClear = PlayerInfo.Instance.IsMissionClear[i];
                MyMissionList[i].IsGetReward = PlayerInfo.Instance.IsGetReward[i];
            }
        }
        else
        {
            NowMissionStepCount = 1;
        }

        Init();
        InitMissionData();

        InGameUI.Instance.GetQuestOpenButton.onClick.AddListener(IfClickMissionPanelButton);
        InGameUI.Instance.GetMissionAlarmImg.gameObject.SetActive(false);
        InGameUI.Instance.GetMissionAlarmEffect.gameObject.SetActive(false);

        // 미션 클리어 버튼 클릭 시 유아이 관리
        EachMissionPrefabList[0].GetClearQuestButton.onClick.AddListener(IfClickMissionClearButton);
        EachMissionPrefabList[1].GetClearQuestButton.onClick.AddListener(IfClickMissionClearButton);
        EachMissionPrefabList[2].GetClearQuestButton.onClick.AddListener(IfClickMissionClearButton);
    }

    public void Update()
    {
        if (isNowStepMissionAllClear == false)
        {
            if (Time.time - prevTime >= limitTime) // limitTime 마다 이벤트의 조건 판별함 
            {
                for (int i = 0; i < MyMissionList.Count; i++)
                {
                    if (NowMissionStepCount == MyMissionList[i].MissionStep)
                    {
                        MissionDispatcher(MyMissionList[i]); // ui 에 글 적어주기
                    }
                }

                prevTime = Time.time;

                // UpdatePlayerInfo
                for (var index = 0; index < MyMissionList.Count; index++)
                {
                    PlayerInfo.Instance.IsMissionClear[index] = MyMissionList[index].IsMissionClear;
                    PlayerInfo.Instance.IsGetReward[index] = MyMissionList[index].IsGetReward;
                }

                PlayerInfo.Instance.NowMissionStepCount = NowMissionStepCount;
                PlayerInfo.Instance.IsAllMissionClear = isAllMissionClear;
            }
        }
        else if (isNowStepMissionAllClear == true)
        {
            isNowStepMissionAllClear = false;

            StartCoroutine(DelayInitMissionStep(2));
            // InitMissionData();
        }
    }

    // 유아이 프리팹의 (초기화)
    public void Init()
    {
        QuestPanel.gameObject.SetActive(false);

        NowMissionStepStarImg1.gameObject.SetActive(true);
        NowMissionStepStarImg2.gameObject.SetActive(false);
        NowMissionStepStarImg3.gameObject.SetActive(false);
        NowMissionStepStarImg4.gameObject.SetActive(false);
        NowMissionStepStarImg5.gameObject.SetActive(false);

        // 기본 적인 유아이의 설정을 하기 위한 내용
        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            EachMissionPrefabList[i].GetQuestSlideBar.gameObject.SetActive(true);
            EachMissionPrefabList[i].GetMissionRewardText.gameObject.SetActive(false);
            EachMissionPrefabList[i].GetClearQuestButton.gameObject.SetActive(false);

            EachMissionPrefabList[i].GetCheckBoxCheckedImg.gameObject.SetActive(false);
            EachMissionPrefabList[i].GetQuestClearImg.gameObject.SetActive(false);
        }
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
            // InGameUI.Instance.GetMissionAlarmImg.gameObject.SetActive(false);        -> 미션 버튼 누른다고 꺼지지 말고 클리어 버튼이 없을 때 꺼지는 걸로 하자
            // InGameUI.Instance.GetMissionAlarmEffect.gameObject.SetActive(false);
        }
    }

    //// 미션 스텝이 바뀔 때마다 유아이 데이터를 교환시켜주는 함수
    public void InitMissionData()
    {
        PlayerInfo.Instance.IsAllMissionClear = isAllMissionClear;
        InGameUI.Instance.GetMissionAlarmImg.gameObject.SetActive(false);
        InGameUI.Instance.GetMissionAlarmEffect.gameObject.SetActive(false);

        missionChecker.Clear();
        NowStepGoalCount = 0;

        int PrefabIndex = 0;

        if (isAllMissionClear == false) // 전체 미션 깬건지 체크
        {
            NowStepClearedCount = 0;

            Init();

            for (int i = 0; i < MyMissionList.Count; i++)
            {
                if (NowMissionStepCount == MyMissionList[i].MissionStep)
                {
                    EachMissionPrefabList[PrefabIndex].GetMissionID =
                        MyMissionList[i].MissionID; // 보상 획득 확인을 위한 미션 ID 정보

                    EachMissionPrefabList[PrefabIndex].GetQuestName.fontStyle = FontStyles.Normal;
                    EachMissionPrefabList[PrefabIndex].GetQuestName.text =
                        MyMissionList[i].MissionName; // UI - 이름 리셋 해주기
                    EachMissionPrefabList[PrefabIndex].GetQuestSlideBar.value = 0; // 슬라이더 바도 리셋 해주기

                    EachMissionPrefabList[PrefabIndex].GetMissionAmountText.text =
                        "0 / " + MyMissionList[i].ConditionAmountN.ToString();

                    EachMissionPrefabList[PrefabIndex].GetMissionReward1AmountText.text =
                        "+ " + MyMissionList[i].Reward1Amount.ToString();
                    EachMissionPrefabList[PrefabIndex].GetMissionReward2AmountText.text =
                        "+ " + MyMissionList[i].Reward2Amount.ToString();

                    if (MyMissionList[i].Reward2Index != 0) // 보상이 몇개인지 체크 후 안보이게 꺼주기
                    {
                        EachMissionPrefabList[PrefabIndex].GetMissionReward1.gameObject.SetActive(true);
                        EachMissionPrefabList[PrefabIndex].GetMissionReward2.gameObject.SetActive(true);
                    }
                    else if (MyMissionList[i].Reward1Index != 0)
                    {
                        EachMissionPrefabList[PrefabIndex].GetMissionReward1.gameObject.SetActive(true);
                        EachMissionPrefabList[PrefabIndex].GetMissionReward2.gameObject.SetActive(false);
                    }
                    else
                    {
                        EachMissionPrefabList[PrefabIndex].GetMissionReward1.gameObject.SetActive(false);
                        EachMissionPrefabList[PrefabIndex].GetMissionReward2.gameObject.SetActive(false);
                    }

                    NowStepGoalCount++;
                    PrefabIndex++;

                    MissionDispatcher(MyMissionList[i], true);


                    if (MyMissionList[i].IsMissionClear)
                    {
                        // 불러오기시 이미 완료된 미션은 완료표시함
                        IfMissionConditionAccept(MyMissionList[i]);

                        // 보상을 이미 받았다면 UI 변경
                        if (MyMissionList[i].IsGetReward)
                        {
                            for (int index = 0; index < EachMissionPrefabList.Count; index++)
                            {
                                if (MyMissionList[i].MissionID == EachMissionPrefabList[index].GetMissionID)
                                {
                                    EachMissionPrefabList[i].GetClearQuestButton.gameObject.SetActive(false);

                                    NowStepClearedCount++;
                                }
                            }
                        }
                    }
                }
            }

            DrawNowStepStar(); // 현재 단계 별 그리기
            DrawNowStepMissionCount(); // 현재 단계 미션 보여주기

            if (NowMissionStepCount <= 5 && (NowStepGoalCount == NowStepClearedCount))
            {
                NowMissionStepCount++;
            }

            // 마지막 단계가 되었을때
            if (NowMissionStepCount == 6)
            {
                isAllMissionClear = true;

                // 모든 미션 끝남 -> 미션 아이콘 자체를 꺼버린다.
                InGameUI.Instance.GetQuestOpenButton.gameObject.SetActive(false);
            }
        }
    }

    // 현재 스텝의 별 갯수를 그려주기 위한부분
    public void DrawNowStepStar()
    {
        NowMissionStepStarImg1.gameObject.SetActive(false);
        NowMissionStepStarImg2.gameObject.SetActive(false);
        NowMissionStepStarImg3.gameObject.SetActive(false);
        NowMissionStepStarImg4.gameObject.SetActive(false);
        NowMissionStepStarImg5.gameObject.SetActive(false);

        if (NowMissionStepCount == 1)
        {
            NowMissionStepStarImg1.gameObject.SetActive(true);
        }
        else if (NowMissionStepCount == 2)
        {
            NowMissionStepStarImg1.gameObject.SetActive(true);
            NowMissionStepStarImg2.gameObject.SetActive(true);
        }
        else if (NowMissionStepCount == 3)
        {
            NowMissionStepStarImg1.gameObject.SetActive(true);
            NowMissionStepStarImg2.gameObject.SetActive(true);
            NowMissionStepStarImg3.gameObject.SetActive(true);
        }
        else if (NowMissionStepCount == 4)
        {
            NowMissionStepStarImg1.gameObject.SetActive(true);
            NowMissionStepStarImg2.gameObject.SetActive(true);
            NowMissionStepStarImg3.gameObject.SetActive(true);
            NowMissionStepStarImg4.gameObject.SetActive(true);
        }
        else if (NowMissionStepCount == 5)
        {
            NowMissionStepStarImg1.gameObject.SetActive(true);
            NowMissionStepStarImg2.gameObject.SetActive(true);
            NowMissionStepStarImg3.gameObject.SetActive(true);
            NowMissionStepStarImg4.gameObject.SetActive(true);
            NowMissionStepStarImg5.gameObject.SetActive(true);
        }
    }

    // 현재 스텝의 미션 갯수를 카운트 -> 미션 그려주기
    public void DrawNowStepMissionCount()
    {
        // 현재 스텝의 미션 갯수에 따라 유아이의 active 상태 결정
        if (NowStepGoalCount == 1)
        {
            EachMissionPrefabList[0].GetNowMissionPrefab.gameObject.SetActive(true);
            EachMissionPrefabList[1].GetNowMissionPrefab.gameObject.SetActive(false);
            EachMissionPrefabList[2].GetNowMissionPrefab.gameObject.SetActive(false);
        }
        else if (NowStepGoalCount == 2)
        {
            EachMissionPrefabList[0].GetNowMissionPrefab.gameObject.SetActive(true);
            EachMissionPrefabList[1].GetNowMissionPrefab.gameObject.SetActive(true);
            EachMissionPrefabList[2].GetNowMissionPrefab.gameObject.SetActive(false);
        }
        else if (NowStepGoalCount == 3)
        {
            EachMissionPrefabList[0].GetNowMissionPrefab.gameObject.SetActive(true);
            EachMissionPrefabList[1].GetNowMissionPrefab.gameObject.SetActive(true);
            EachMissionPrefabList[2].GetNowMissionPrefab.gameObject.SetActive(true);
        }
    }

    // TODO : 계속 유아이에 데이터를 그려줄 부분 (미션의 조건 양 , 현재 나의 양)
    public void DrawMissionPercentageUI(MissionData nowMission, int nowMissionValue)
    {
        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            // 현재 미션의 데이터와 체커의 아이디를 비교해서 그 순서에 맞게 그림을 그려준다 
            // 501, 502 번 미션의 달성치가 달라서 걸어주는 조건. 
            if (nowMission.MissionName == EachMissionPrefabList[i].GetQuestName.text &&
                nowMission.ConditionAmountN > nowMissionValue)
            {
                if (nowMission.MissionID == 501 || nowMission.MissionID == 502)
                {
                    EachMissionPrefabList[i].GetQuestSlideBar.value = 0;
                    EachMissionPrefabList[i].GetMissionAmountText.text = "0 / 1";
                }
                else
                {
                    float tempPercent = (float)nowMissionValue / (float)nowMission.ConditionAmountN;

                    EachMissionPrefabList[i].GetQuestSlideBar.value = tempPercent;
                    EachMissionPrefabList[i].GetMissionAmountText.text = nowMissionValue.ToString() + " / " + nowMission.ConditionAmountN.ToString();
                }
            }
            else if (nowMission.MissionName == EachMissionPrefabList[i].GetQuestName.text &&
                     nowMission.ConditionAmountN <= nowMissionValue)
            {
                if (nowMission.MissionID == 501 || nowMission.MissionID == 502)
                {
                    if (nowMission.ConditionAmountN < nowMissionValue && nowMission.MissionID == 502)
                    {
                        EachMissionPrefabList[i].GetQuestSlideBar.value = 0;
                        EachMissionPrefabList[i].GetMissionAmountText.text = "0 / 1";
                    }
                    else
                    {
                        EachMissionPrefabList[i].GetQuestSlideBar.value = 1;
                        EachMissionPrefabList[i].GetMissionAmountText.text = "1 / 1";
                    }
                }
                else
                {
                    EachMissionPrefabList[i].GetQuestSlideBar.value = 1;
                    EachMissionPrefabList[i].GetMissionAmountText.text = nowMissionValue.ToString() + " / " + nowMission.ConditionAmountN.ToString();
                }
            }
        }
    }

    // 미션 조건 충족 시 -> 해당 미션의 슬라이더 끄고, 클리어 이미지 & 버튼 켜야 함.
    public void IfMissionConditionAccept(MissionData NowMission)
    {
        for (int i = 0; i < MyMissionList.Count; i++)
        {
            if (NowMission.MissionID == MyMissionList[i].MissionID)
            {
                MyMissionList[i].IsMissionClear = true;
                QuestPanel.gameObject.SetActive(false);
                InGameUI.Instance.GetMissionAlarmImg.gameObject.SetActive(true);
                InGameUI.Instance.GetMissionAlarmEffect.gameObject.SetActive(true);

                if (EachMissionPrefabList[0].GetQuestName.text == MyMissionList[i].MissionName)
                {
                    EachMissionPrefabList[0].GetCheckBoxCheckedImg.gameObject.SetActive(true);
                    EachMissionPrefabList[0].GetClearQuestButton.gameObject.SetActive(true);
                    EachMissionPrefabList[0].GetQuestClearImg.gameObject.SetActive(true);
                    EachMissionPrefabList[0].GetQuestSlideBar.gameObject.SetActive(false);
                }
                else if (EachMissionPrefabList[1].GetQuestName.text == MyMissionList[i].MissionName)
                {
                    EachMissionPrefabList[1].GetCheckBoxCheckedImg.gameObject.SetActive(true);
                    EachMissionPrefabList[1].GetClearQuestButton.gameObject.SetActive(true);
                    EachMissionPrefabList[1].GetQuestClearImg.gameObject.SetActive(true);
                    EachMissionPrefabList[1].GetQuestSlideBar.gameObject.SetActive(false);
                }
                else if (EachMissionPrefabList[2].GetQuestName.text == MyMissionList[i].MissionName)
                {
                    EachMissionPrefabList[2].GetCheckBoxCheckedImg.gameObject.SetActive(true);
                    EachMissionPrefabList[2].GetClearQuestButton.gameObject.SetActive(true);
                    EachMissionPrefabList[2].GetQuestClearImg.gameObject.SetActive(true);
                    EachMissionPrefabList[2].GetQuestSlideBar.gameObject.SetActive(false);
                }
            }
        }
    }

    // 미션 클리어 버튼 클릭 시 진행 될 함수
    public void IfClickMissionClearButton()
    {
        // 보상 데이터 텍스트 출력, 일정 시간 후 사라지기
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;
        tempMissionClearButton = nowClick;
        soundManager.PlayMoneyJackpotSound();       // 각 미션 클리어버튼을 클릭 시 사운드 나도록

        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            if (nowClick.name == EachMissionPrefabList[i].GetClearQuestButton.name)
            {
                EachMissionPrefabList[i].GetQuestName.fontStyle = FontStyles.Strikethrough;
                EachMissionPrefabList[i].GetMissionRewardText.gameObject.SetActive(true);
                EachMissionPrefabList[i].GetClearQuestButton.gameObject.SetActive(false);

                // 보상 텍스트를 출력하면서 이 미션의 보상을 받았음을 체크함
                GetRewardCheckToEventID(EachMissionPrefabList[i].GetMissionID);

                NowStepClearedCount++;
                StartCoroutine(RewardAmountTextPopOff(2, EachMissionPrefabList[i].GetMissionRewardText));
                FindMissionToGetRewardAmount(EachMissionPrefabList[i].GetQuestName.text);
                break;
            }
        }

        // 미션 클리어 목표 갯수보다 현재 미션클리어 갯수가 적을때(현재 스텝의 미션을 다 클리어 하지 못하면) 이펙트를 끄지 않는다.

        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            if (EachMissionPrefabList[i].GetClearQuestButton.gameObject.activeSelf == true)
            {
                InGameUI.Instance.GetMissionAlarmEffect.gameObject.SetActive(true);
                InGameUI.Instance.GetMissionAlarmImg.gameObject.SetActive(true);
                break;
            }
            else
            {
                InGameUI.Instance.GetMissionAlarmEffect.gameObject.SetActive(false);
                InGameUI.Instance.GetMissionAlarmImg.gameObject.SetActive(false);
            }
        }

        if (NowStepMissionCountChecker())
        {
            NowMissionStepCount++;
            PlayerInfo.Instance.NowMissionStepCount++;
            isNowStepMissionAllClear = true;
            // DelayInitMissionStep(1);
        }
    }

    // 미션이 다 끝난 후 딜레이를 준 후 미션스텝을 바꿔주기 위한 함수
    IEnumerator DelayInitMissionStep(int delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        InitMissionData();
    }

    // 현재 단계의 미션을 모두 클리어 했는지 체크. 하나라도 isMissionClear 값이 false 라면 아직 미션 중.
    public bool NowStepMissionCountChecker()
    {
        for (int i = 0; i < MyMissionList.Count; i++)
        {
            if (NowMissionStepCount == MyMissionList[i].MissionStep)
            {
                if (!MyMissionList[i].IsGetReward)      // isMissionClear 로 체크하면 이미 다 true가 됬기 때문에 바로 다음스텝으로 넘어가버림
                {
                    return false;
                }
            }
        }

        return true;
    }

    // 클리어 버튼 클릭 후 일정 시간 뒤 보상 글을 꺼주기 위한 함수 -> 코루틴을 Invoke 로 수정
    IEnumerator RewardAmountTextPopOff(int delayTime, RectTransform nowRewardText)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            if (tempMissionClearButton.name == EachMissionPrefabList[i].GetClearQuestButton.name)
            {
                nowRewardText.gameObject.SetActive(false);
            }
        }
    }

    public int CountGenreRoomRepairCount(List<InteractionManager.GenreRoomInfo> genreRooms)
    {
        int result = 0;

        result += genreRooms[(int)InteractionManager.SpotName.ActionRoom].RepairCount;
        result += genreRooms[(int)InteractionManager.SpotName.AdventureRoom].RepairCount;
        result += genreRooms[(int)InteractionManager.SpotName.PuzzleRoom].RepairCount;
        result += genreRooms[(int)InteractionManager.SpotName.RhythmRoom].RepairCount;

        result += genreRooms[(int)InteractionManager.SpotName.RPGRoom].RepairCount;
        result += genreRooms[(int)InteractionManager.SpotName.ShootingRoom].RepairCount;
        result += genreRooms[(int)InteractionManager.SpotName.SimulationRoom].RepairCount;
        result += genreRooms[(int)InteractionManager.SpotName.SportsRoom].RepairCount;

        return result;
    }

    public int CountTeacherAllLevel()
    {
        int result = 0;

        for (int i = 0; i < Professor.Instance.GameManagerProfessor.Count; i++)
        {
            result += Professor.Instance.GameManagerProfessor[i].m_ProfessorPower;
        }

        for (int i = 0; i < Professor.Instance.ArtProfessor.Count; i++)
        {
            result += Professor.Instance.ArtProfessor[i].m_ProfessorPower;
        }

        for (int i = 0; i < Professor.Instance.ProgrammingProfessor.Count; i++)
        {
            result += Professor.Instance.ProgrammingProfessor[i].m_ProfessorPower;
        }

        return result;
    }

    private void GetRewardCheckToEventID(int id)
    {
        foreach (var mission in MyMissionList)
        {
            if (mission.MissionID == id)
            {
                mission.IsGetReward = true;
            }
        }
    }

    //미션 보상을 안받았으면 보상을 받자
    public void FindMissionToGetRewardAmount(string nowEventName)
    {
        for (int i = 0; i < MyMissionList.Count; i++)
        {
            if (MyMissionList[i].MissionName == nowEventName)
            {
                GetRewardAmount(MyMissionList[i].Reward1Index, MyMissionList[i].Reward1Amount);
                GetRewardAmount(MyMissionList[i].Reward2Index, MyMissionList[i].Reward2Amount);
            }
        }
    }

    // 현재 단계의 미션들 조건을 계속 확인 해 줄 함수 
    public bool MissionDispatcher(MissionData NowMission, bool check = false)
    {
        int ConditionAmount = NowMission.ConditionAmountN;

        switch (NowMission.MissionCondition) // 조건 체크
        {
            case 0: // 조건이 없다면 오류이다
                {
                    NowMission.IsMissionClear = false;
                }
                break;
            case 101: // 1차 재화 N 이상 보유
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.MyMoney.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.MyMoney);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != PlayerInfo.Instance.MyMoney.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.MyMoney);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = PlayerInfo.Instance.MyMoney.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (PlayerInfo.Instance.MyMoney >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            // 함수로 이동, 퍼센테이지 조절 하는 함수
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 102: // 2차 재화 N 이상 보유
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.SpecialPoint.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.SpecialPoint);
                    }

                    // 유아이에 데이터 그려주기
                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != PlayerInfo.Instance.SpecialPoint.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.SpecialPoint);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = PlayerInfo.Instance.SpecialPoint.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (PlayerInfo.Instance.SpecialPoint >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            // 함수로 이동, 퍼센테이지 조절 하는 함수
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 103: // [모든 게임잼] 게임잼 N 회 참여
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.ParticipatedGameJamCount.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.ParticipatedGameJamCount);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != PlayerInfo.Instance.ParticipatedGameJamCount.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.ParticipatedGameJamCount);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = PlayerInfo.Instance.ParticipatedGameJamCount.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (PlayerInfo.Instance.ParticipatedGameJamCount >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            // 함수로 이동, 퍼센테이지 조절 하는 함수
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 104: // [모든 게임쇼] 게임쇼 N 회 참여
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.ParticipatedGameShowCount.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.ParticipatedGameShowCount);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != PlayerInfo.Instance.ParticipatedGameShowCount.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.ParticipatedGameShowCount);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = PlayerInfo.Instance.ParticipatedGameShowCount.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (PlayerInfo.Instance.ParticipatedGameShowCount >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            // 함수로 이동, 퍼센테이지 조절 하는 함수
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 105: // [모든 장르방 통틀어] 장르방 수리 N 회
                {
                    List<InteractionManager.GenreRoomInfo> genreRooms = InteractionManager.Instance.GenreRoomList;
                    int TotalGenreRoomReapairCount = 0;

                    TotalGenreRoomReapairCount = CountGenreRoomRepairCount(genreRooms);

                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = TotalGenreRoomReapairCount.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, TotalGenreRoomReapairCount);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != TotalGenreRoomReapairCount.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, TotalGenreRoomReapairCount);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = TotalGenreRoomReapairCount.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (TotalGenreRoomReapairCount >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 106: // 분기순위 N 위 이상 달성
                {
                    if ((GameTime.Instance.FlowTime.NowMonth == 3 || GameTime.Instance.FlowTime.NowMonth == 6 || GameTime.Instance.FlowTime.NowMonth == 9 ||
                            GameTime.Instance.FlowTime.NowMonth == 12) && GameTime.Instance.FlowTime.NowDay == 5)
                    {
                        //현재 학원의 랭크를 저장.    AcademyNameToRank -> 1, 2, 3 등 학원의 정보와 랭크를 저장하는 리스트. 여기서 우리 학원의 이름으로 랭크를 찾아본다.
                        if (QuarterlyReport.Instance.AcademyNameToRank.Count != 0)
                        {
                            int temp = QuarterlyReport.Instance.AcademyNameToRank.FindIndex(x =>
                                x.Key.Equals(PlayerInfo.Instance.AcademyName));

                            if (check == true)
                            {
                                MissionChecker nowTemp = new MissionChecker();
                                nowTemp.MissionName = NowMission.MissionName;
                                nowTemp.NowPercent = temp.ToString();

                                missionChecker.Add(nowTemp);

                                DrawMissionPercentageUI(NowMission, temp);
                            }

                            for (int i = 0; i < missionChecker.Count; i++)
                            {
                                if (missionChecker[i].MissionName == NowMission.MissionName)
                                {
                                    if (missionChecker[i].NowPercent != temp.ToString())
                                    {
                                        DrawMissionPercentageUI(NowMission, temp);

                                        MissionChecker tempChecker = missionChecker[i];

                                        tempChecker.NowPercent = temp.ToString();

                                        missionChecker[i] = tempChecker;
                                    }
                                }
                            }

                            if (!NowMission.IsMissionClear)
                            {
                                if (temp <= ConditionAmount)
                                {
                                    NowMission.IsMissionClear = true;

                                    // 함수로 이동, 퍼센테이지 조절 하는 함수
                                    IfMissionConditionAccept(NowMission);
                                }
                            }
                        }
                    }
                }
                break;
            case 107: // 랭크 INT(N) 등급 이상 달성
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.CurrentRank.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, (int)PlayerInfo.Instance.CurrentRank + 1);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != PlayerInfo.Instance.CurrentRank.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, (int)PlayerInfo.Instance.CurrentRank + 1);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = PlayerInfo.Instance.CurrentRank.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if ((int)PlayerInfo.Instance.CurrentRank + 1 <= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            // 함수로 이동, 퍼센테이지 조절 하는 함수
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 108: // [모든 학생 프로필 통틀어] 학생 상세 프로필 N 회 확인
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.StudentProfileClickCount.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.StudentProfileClickCount);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != PlayerInfo.Instance.StudentProfileClickCount.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.StudentProfileClickCount);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = PlayerInfo.Instance.StudentProfileClickCount.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (PlayerInfo.Instance.StudentProfileClickCount >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            // 함수로 이동, 퍼센테이지 조절 하는 함수
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 109: // [모든 강사 프로필 통틀어] 강사 상세 프로필 N 회 확인
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.TeacherProfileClickCount.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.TeacherProfileClickCount);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != PlayerInfo.Instance.TeacherProfileClickCount.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.TeacherProfileClickCount);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = PlayerInfo.Instance.TeacherProfileClickCount.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (PlayerInfo.Instance.TeacherProfileClickCount >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            // 함수로 이동, 퍼센테이지 조절 하는 함수
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 110: // 강사 ID 몇개 해금됬는지
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.UnLockTeacherCount.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.UnLockTeacherCount);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != PlayerInfo.Instance.UnLockTeacherCount.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.UnLockTeacherCount);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = PlayerInfo.Instance.UnLockTeacherCount.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (PlayerInfo.Instance.UnLockTeacherCount >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            // 함수로 이동, 퍼센테이지 조절 하는 함수
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 111: // [모든 장르방 통틀어] 장르방 터치 N 회
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.GenreRoomClickCount.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.GenreRoomClickCount);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != PlayerInfo.Instance.GenreRoomClickCount.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.GenreRoomClickCount);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = PlayerInfo.Instance.GenreRoomClickCount.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (PlayerInfo.Instance.GenreRoomClickCount >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            // 함수로 이동, 퍼센테이지 조절 하는 함수
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;

            case 113: // [모든 강사 통틀어서] 강사 레벨 합 N 달성
                {
                    int AllTeacherLevel = 0;

                    AllTeacherLevel = CountTeacherAllLevel();

                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = AllTeacherLevel.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, AllTeacherLevel);
                    }

                    for (int i = 0; i < missionChecker.Count; i++)
                    {
                        if (missionChecker[i].MissionName == NowMission.MissionName)
                        {
                            if (missionChecker[i].NowPercent != AllTeacherLevel.ToString())
                            {
                                DrawMissionPercentageUI(NowMission, AllTeacherLevel);

                                MissionChecker tempChecker = missionChecker[i];

                                tempChecker.NowPercent = AllTeacherLevel.ToString();

                                missionChecker[i] = tempChecker;
                            }
                        }
                    }

                    if (!NowMission.IsMissionClear)
                    {
                        if (AllTeacherLevel >= ConditionAmount)
                        {
                            NowMission.IsMissionClear = true;

                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
        }

        return false; // 미션 조건을 만족하지 못하면
    }

    public void GetRewardAmount(int rewardIndex, int rewardAmount)
    {
        switch (rewardIndex)
        {
            case 100:       // 1차 재화
                {
                    PlayerInfo.Instance.MyMoney += rewardAmount;
                }
                break;

            case 101:       // 2차 재화
                {
                    PlayerInfo.Instance.SpecialPoint += rewardAmount;
                }
                break;

            case 300:       // 운영점수
                {
                    PlayerInfo.Instance.Management += rewardAmount;
                }
                break;

            case 301:       // 유명점수
                {
                    PlayerInfo.Instance.Famous += rewardAmount;
                }
                break;

            case 302:       // 활동점수
                {
                    PlayerInfo.Instance.Activity += rewardAmount;
                }
                break;

            case 303:       // 인재양성
                {
                    PlayerInfo.Instance.TalentDevelopment += rewardAmount;
                }
                break;

            case 0:
                {
                    Debug.Log("리워드 인덱스에 이상이 있다");
                }
                break;
        }
    }
}