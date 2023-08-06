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
/// ��ȹ������ ������ ���⼺�� �����ϴ� �� ������ ����Ʈ�� ����.
/// 
/// </summary>
public class Mission : MonoBehaviour
{
    [Header("�˾� ����Ʈ �г�")] [SerializeField] private Image QuestPanel; // �� ����Ʈ �г�
    [Space(5f)] [SerializeField] private Image NowMissionStepStarImg1; // �̼� �ܰ� �� �̹���
    [SerializeField] private Image NowMissionStepStarImg2; // �̼� �ܰ� �� �̹���
    [SerializeField] private Image NowMissionStepStarImg3; // �̼� �ܰ� �� �̹���
    [SerializeField] private Image NowMissionStepStarImg4; // �̼� �ܰ� �� �̹���
    [SerializeField] private Image NowMissionStepStarImg5; // �̼� �ܰ� �� �̹���

    [Space(5f)] [SerializeField] private MissionPrefab Mission1;
    [SerializeField] private MissionPrefab Mission2;
    [SerializeField] private MissionPrefab Mission3;

    // ����Ʈ�� ���൵�� üũ�ϰ�, ���� ���� �� �� ������
    private int NowMissionStepCount = 0; // 1~ 5�ܰ� ���� �� ���� �̼� ����

    private int NowStepGoalCount = 0; // ���� ���ǿ��� �̼��� ������ �� ��ǥ ī��Ʈ
    private int NowStepClearedCount = 0; // ���� ���� ������ �̼��� Ŭ���� �� ����
    private bool isNowStepMissionAllClear = false; // ���� ������ �̼��� ��� Ŭ���� �ߴ��� üũ�� �� ����
    private bool isAllMissionClear = false; // ��� ������ �̼��� �������� üũ�� �� ����

    float prevTime = 0;
    float limitTime = 0.5f;

    GameObject tempMissionClearButton; // ���� Ŭ���� ���� ������ �̼� Ŭ���� ��ư �� �˱� ����  �ӽ� Ŭ���� ��ư

    List<MissionData> MyMissionList = new List<MissionData>(); // �� ���� �̼� ����Ʈ

    // List<MissionData> missionChecker = new List<MissionData>();                 // ���� ���� 
    List<MissionChecker> missionChecker = new List<MissionChecker>(); // ���� �ܰ��� �̼Ǹ� ��Ƽ� ���鼭 ���� üũ���� ����Ʈ

    List<MissionPrefab> EachMissionPrefabList = new List<MissionPrefab>(); // �� �̼� �������� ����� ����Ʈ

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

        if (Json.Instance.UseLoadingData) // �÷��� ������ �ִٸ� ������ �־��ֱ�
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

        // �̼� Ŭ���� ��ư Ŭ�� �� ������ ����
        EachMissionPrefabList[0].GetClearQuestButton.onClick.AddListener(IfClickMissionClearButton);
        EachMissionPrefabList[1].GetClearQuestButton.onClick.AddListener(IfClickMissionClearButton);
        EachMissionPrefabList[2].GetClearQuestButton.onClick.AddListener(IfClickMissionClearButton);
    }

    public void Update()
    {
        if (isNowStepMissionAllClear == false)
        {
            if (Time.time - prevTime >= limitTime) // limitTime ���� �̺�Ʈ�� ���� �Ǻ��� 
            {
                for (int i = 0; i < MyMissionList.Count; i++)
                {
                    if (NowMissionStepCount == MyMissionList[i].MissionStep)
                    {
                        MissionDispatcher(MyMissionList[i]); // ui �� �� �����ֱ�
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

    // ������ �������� (�ʱ�ȭ)
    public void Init()
    {
        QuestPanel.gameObject.SetActive(false);

        NowMissionStepStarImg1.gameObject.SetActive(true);
        NowMissionStepStarImg2.gameObject.SetActive(false);
        NowMissionStepStarImg3.gameObject.SetActive(false);
        NowMissionStepStarImg4.gameObject.SetActive(false);
        NowMissionStepStarImg5.gameObject.SetActive(false);

        // �⺻ ���� �������� ������ �ϱ� ���� ����
        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            EachMissionPrefabList[i].GetQuestSlideBar.gameObject.SetActive(true);
            EachMissionPrefabList[i].GetMissionRewardText.gameObject.SetActive(false);
            EachMissionPrefabList[i].GetClearQuestButton.gameObject.SetActive(false);

            EachMissionPrefabList[i].GetCheckBoxCheckedImg.gameObject.SetActive(false);
            EachMissionPrefabList[i].GetQuestClearImg.gameObject.SetActive(false);
        }
    }

    //// �ΰ����г��� ����Ʈ ��ư�� Ŭ�� �� ���� ����Ʈ ����� ����Ʈ�� ���� �� �Լ�
    public void IfClickMissionPanelButton()
    {
        if (QuestPanel.gameObject.activeSelf)
        {
            QuestPanel.gameObject.SetActive(false);
        }
        else
        {
            QuestPanel.gameObject.SetActive(true);
            // InGameUI.Instance.GetMissionAlarmImg.gameObject.SetActive(false);        -> �̼� ��ư �����ٰ� ������ ���� Ŭ���� ��ư�� ���� �� ������ �ɷ� ����
            // InGameUI.Instance.GetMissionAlarmEffect.gameObject.SetActive(false);
        }
    }

    //// �̼� ������ �ٲ� ������ ������ �����͸� ��ȯ�����ִ� �Լ�
    public void InitMissionData()
    {
        PlayerInfo.Instance.IsAllMissionClear = isAllMissionClear;
        InGameUI.Instance.GetMissionAlarmImg.gameObject.SetActive(false);
        InGameUI.Instance.GetMissionAlarmEffect.gameObject.SetActive(false);

        missionChecker.Clear();
        NowStepGoalCount = 0;

        int PrefabIndex = 0;

        if (isAllMissionClear == false) // ��ü �̼� ������ üũ
        {
            NowStepClearedCount = 0;

            Init();

            for (int i = 0; i < MyMissionList.Count; i++)
            {
                if (NowMissionStepCount == MyMissionList[i].MissionStep)
                {
                    EachMissionPrefabList[PrefabIndex].GetMissionID =
                        MyMissionList[i].MissionID; // ���� ȹ�� Ȯ���� ���� �̼� ID ����

                    EachMissionPrefabList[PrefabIndex].GetQuestName.fontStyle = FontStyles.Normal;
                    EachMissionPrefabList[PrefabIndex].GetQuestName.text =
                        MyMissionList[i].MissionName; // UI - �̸� ���� ���ֱ�
                    EachMissionPrefabList[PrefabIndex].GetQuestSlideBar.value = 0; // �����̴� �ٵ� ���� ���ֱ�

                    EachMissionPrefabList[PrefabIndex].GetMissionAmountText.text =
                        "0 / " + MyMissionList[i].ConditionAmountN.ToString();

                    EachMissionPrefabList[PrefabIndex].GetMissionReward1AmountText.text =
                        "+ " + MyMissionList[i].Reward1Amount.ToString();
                    EachMissionPrefabList[PrefabIndex].GetMissionReward2AmountText.text =
                        "+ " + MyMissionList[i].Reward2Amount.ToString();

                    if (MyMissionList[i].Reward2Index != 0) // ������ ����� üũ �� �Ⱥ��̰� ���ֱ�
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
                        // �ҷ������ �̹� �Ϸ�� �̼��� �Ϸ�ǥ����
                        IfMissionConditionAccept(MyMissionList[i]);

                        // ������ �̹� �޾Ҵٸ� UI ����
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

            DrawNowStepStar(); // ���� �ܰ� �� �׸���
            DrawNowStepMissionCount(); // ���� �ܰ� �̼� �����ֱ�

            if (NowMissionStepCount <= 5 && (NowStepGoalCount == NowStepClearedCount))
            {
                NowMissionStepCount++;
            }

            // ������ �ܰ谡 �Ǿ�����
            if (NowMissionStepCount == 6)
            {
                isAllMissionClear = true;

                // ��� �̼� ���� -> �̼� ������ ��ü�� ��������.
                InGameUI.Instance.GetQuestOpenButton.gameObject.SetActive(false);
            }
        }
    }

    // ���� ������ �� ������ �׷��ֱ� ���Ѻκ�
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

    // ���� ������ �̼� ������ ī��Ʈ -> �̼� �׷��ֱ�
    public void DrawNowStepMissionCount()
    {
        // ���� ������ �̼� ������ ���� �������� active ���� ����
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

    // TODO : ��� �����̿� �����͸� �׷��� �κ� (�̼��� ���� �� , ���� ���� ��)
    public void DrawMissionPercentageUI(MissionData nowMission, int nowMissionValue)
    {
        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            // ���� �̼��� �����Ϳ� üĿ�� ���̵� ���ؼ� �� ������ �°� �׸��� �׷��ش� 
            // 501, 502 �� �̼��� �޼�ġ�� �޶� �ɾ��ִ� ����. 
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

    // �̼� ���� ���� �� -> �ش� �̼��� �����̴� ����, Ŭ���� �̹��� & ��ư �Ѿ� ��.
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

    // �̼� Ŭ���� ��ư Ŭ�� �� ���� �� �Լ�
    public void IfClickMissionClearButton()
    {
        // ���� ������ �ؽ�Ʈ ���, ���� �ð� �� �������
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;
        tempMissionClearButton = nowClick;
        soundManager.PlayMoneyJackpotSound();       // �� �̼� Ŭ�����ư�� Ŭ�� �� ���� ������

        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            if (nowClick.name == EachMissionPrefabList[i].GetClearQuestButton.name)
            {
                EachMissionPrefabList[i].GetQuestName.fontStyle = FontStyles.Strikethrough;
                EachMissionPrefabList[i].GetMissionRewardText.gameObject.SetActive(true);
                EachMissionPrefabList[i].GetClearQuestButton.gameObject.SetActive(false);

                // ���� �ؽ�Ʈ�� ����ϸ鼭 �� �̼��� ������ �޾����� üũ��
                GetRewardCheckToEventID(EachMissionPrefabList[i].GetMissionID);

                NowStepClearedCount++;
                StartCoroutine(RewardAmountTextPopOff(2, EachMissionPrefabList[i].GetMissionRewardText));
                FindMissionToGetRewardAmount(EachMissionPrefabList[i].GetQuestName.text);
                break;
            }
        }

        // �̼� Ŭ���� ��ǥ �������� ���� �̼�Ŭ���� ������ ������(���� ������ �̼��� �� Ŭ���� ���� ���ϸ�) ����Ʈ�� ���� �ʴ´�.

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

    // �̼��� �� ���� �� �����̸� �� �� �̼ǽ����� �ٲ��ֱ� ���� �Լ�
    IEnumerator DelayInitMissionStep(int delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        InitMissionData();
    }

    // ���� �ܰ��� �̼��� ��� Ŭ���� �ߴ��� üũ. �ϳ��� isMissionClear ���� false ��� ���� �̼� ��.
    public bool NowStepMissionCountChecker()
    {
        for (int i = 0; i < MyMissionList.Count; i++)
        {
            if (NowMissionStepCount == MyMissionList[i].MissionStep)
            {
                if (!MyMissionList[i].IsGetReward)      // isMissionClear �� üũ�ϸ� �̹� �� true�� ��� ������ �ٷ� ������������ �Ѿ����
                {
                    return false;
                }
            }
        }

        return true;
    }

    // Ŭ���� ��ư Ŭ�� �� ���� �ð� �� ���� ���� ���ֱ� ���� �Լ� -> �ڷ�ƾ�� Invoke �� ����
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

    //�̼� ������ �ȹ޾����� ������ ����
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

    // ���� �ܰ��� �̼ǵ� ������ ��� Ȯ�� �� �� �Լ� 
    public bool MissionDispatcher(MissionData NowMission, bool check = false)
    {
        int ConditionAmount = NowMission.ConditionAmountN;

        switch (NowMission.MissionCondition) // ���� üũ
        {
            case 0: // ������ ���ٸ� �����̴�
                {
                    NowMission.IsMissionClear = false;
                }
                break;
            case 101: // 1�� ��ȭ N �̻� ����
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

                            // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 102: // 2�� ��ȭ N �̻� ����
                {
                    if (check == true)
                    {
                        MissionChecker nowTemp = new MissionChecker();
                        nowTemp.MissionName = NowMission.MissionName;
                        nowTemp.NowPercent = PlayerInfo.Instance.SpecialPoint.ToString();

                        missionChecker.Add(nowTemp);

                        DrawMissionPercentageUI(NowMission, PlayerInfo.Instance.SpecialPoint);
                    }

                    // �����̿� ������ �׷��ֱ�
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

                            // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 103: // [��� ������] ������ N ȸ ����
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

                            // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 104: // [��� ���Ӽ�] ���Ӽ� N ȸ ����
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

                            // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 105: // [��� �帣�� ��Ʋ��] �帣�� ���� N ȸ
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
            case 106: // �б���� N �� �̻� �޼�
                {
                    if ((GameTime.Instance.FlowTime.NowMonth == 3 || GameTime.Instance.FlowTime.NowMonth == 6 || GameTime.Instance.FlowTime.NowMonth == 9 ||
                            GameTime.Instance.FlowTime.NowMonth == 12) && GameTime.Instance.FlowTime.NowDay == 5)
                    {
                        //���� �п��� ��ũ�� ����.    AcademyNameToRank -> 1, 2, 3 �� �п��� ������ ��ũ�� �����ϴ� ����Ʈ. ���⼭ �츮 �п��� �̸����� ��ũ�� ã�ƺ���.
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

                                    // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                                    IfMissionConditionAccept(NowMission);
                                }
                            }
                        }
                    }
                }
                break;
            case 107: // ��ũ INT(N) ��� �̻� �޼�
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

                            // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 108: // [��� �л� ������ ��Ʋ��] �л� �� ������ N ȸ Ȯ��
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

                            // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 109: // [��� ���� ������ ��Ʋ��] ���� �� ������ N ȸ Ȯ��
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

                            // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 110: // ���� ID � �ر݉����
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

                            // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;
            case 111: // [��� �帣�� ��Ʋ��] �帣�� ��ġ N ȸ
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

                            // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
                            IfMissionConditionAccept(NowMission);
                        }
                    }
                }
                break;

            case 113: // [��� ���� ��Ʋ�] ���� ���� �� N �޼�
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

        return false; // �̼� ������ �������� ���ϸ�
    }

    public void GetRewardAmount(int rewardIndex, int rewardAmount)
    {
        switch (rewardIndex)
        {
            case 100:       // 1�� ��ȭ
                {
                    PlayerInfo.Instance.MyMoney += rewardAmount;
                }
                break;

            case 101:       // 2�� ��ȭ
                {
                    PlayerInfo.Instance.SpecialPoint += rewardAmount;
                }
                break;

            case 300:       // �����
                {
                    PlayerInfo.Instance.Management += rewardAmount;
                }
                break;

            case 301:       // ��������
                {
                    PlayerInfo.Instance.Famous += rewardAmount;
                }
                break;

            case 302:       // Ȱ������
                {
                    PlayerInfo.Instance.Activity += rewardAmount;
                }
                break;

            case 303:       // ����缺
                {
                    PlayerInfo.Instance.TalentDevelopment += rewardAmount;
                }
                break;

            case 0:
                {
                    Debug.Log("������ �ε����� �̻��� �ִ�");
                }
                break;
        }
    }
}