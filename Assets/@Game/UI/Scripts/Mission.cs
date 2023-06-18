using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 2023. 06. 12 Mang
/// 
/// ��ȹ������ ������ ���⼺�� �����ϴ� �� ������ ����Ʈ�� ����.
/// 
/// </summary>
public class Mission : MonoBehaviour
{
    [Header("�˾� ����Ʈ �г�")]
    [SerializeField] private Image QuestPanel;              // �� ����Ʈ �г�
    [Space(5f)]
    [SerializeField] private Image NowMissionStepStarImg1;   // �̼� �ܰ� �� �̹���
    [SerializeField] private Image NowMissionStepStarImg2;   // �̼� �ܰ� �� �̹���
    [SerializeField] private Image NowMissionStepStarImg3;   // �̼� �ܰ� �� �̹���
    [SerializeField] private Image NowMissionStepStarImg4;   // �̼� �ܰ� �� �̹���
    [SerializeField] private Image NowMissionStepStarImg5;   // �̼� �ܰ� �� �̹���

    [Space(5f)]
    [SerializeField] private MissionPrefab Mission1;
    [SerializeField] private MissionPrefab Mission2;
    [SerializeField] private MissionPrefab Mission3;
    [Space(5f)]


    // ����Ʈ�� ���൵�� üũ�ϰ�, ���� ���� �� �� ������
    private int NowMissionStepCount = 0;        // ���� 1~ 5�ܰ� ������ ����Ʈ ����

    private bool[] NowMissionClearCount = new bool[3];
    private bool isMissionAllClear = false;

    private bool isMissionPanelOpen = false;      // �г��� ���ȴ��� üũ�� ����

    List<MissionData> MyMissionList = new List<MissionData>();                  // �� ���� �̼� ����Ʈ
    List<MissionPrefab> EachMissionPrefabList = new List<MissionPrefab>();      // �� �̼� �������� ����� ����Ʈ

    public void Start()
    {
        MyMissionList = AllOriginalJsonData.Instance.OriginalMissionData;

        if (Json.Instance.IsSavedDataExists)                     // �÷��� ������ �ִٸ�
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

    //        DrawMissionPercentageUI();      // ui �� �� �����ֱ�
    //    }
    //    else if (isMissionAllClear == true)
    //    {

    //        NextMissionStep();
    //    }
    //}

    // ������ �������� �⺻ ���� �Ѱ� ������
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
        }
    }

    //// �̼� ������ �ٲ� ������ ������ �����͸� ��ȯ�����ִ� �Լ�
    public void InitMissionData()
    {
        int nowStepMissionCount = 0;

        for (int i = 0; i < MyMissionList.Count; i++)
        {
            if (NowMissionStepCount == MyMissionList[i].MissionStep)
            {
                EachMissionPrefabList[i].GetQuestName.text = MyMissionList[i].MissionName;          // UI - �̸� ���� ���ֱ�
                EachMissionPrefabList[i].GetQuestSlideBar.value = 0;                                // �����̴� �ٵ� ���� ���ֱ�
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

            // �̼��� ������ ���� �������� active ���� ����
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
            // ��� �̼� ���� -> �̼� ������ ��ü�� ��������.
            InGameUI.Instance.GetQuestOpenButton.gameObject.SetActive(false);
        }
    }

    // TODO : ��� �����̿� �����͸� �׷��� �κ�
    public void DrawMissionPercentageUI()
    {
        for (int i = 0; i < EachMissionPrefabList.Count; i++)
        {
            // EachMissionPrefabList.
        }

        // // �̼� 1
        // QuestSlideBar1.value = (NowMissionInfo[0].NowMissionTargetValue / NowMissionInfo[0].NowMissionValue);
        // Mission1AmountText.text = NowMissionInfo[0].NowMissionValue.ToString() + " / " + NowMissionInfo[0].NowMissionTargetValue.ToString();
        // // �̼� 2
        // QuestSlideBar1.value = (NowMissionInfo[1].NowMissionTargetValue / NowMissionInfo[1].NowMissionValue);
        // Mission1AmountText.text = NowMissionInfo[1].NowMissionValue.ToString() + " / " + NowMissionInfo[1].NowMissionTargetValue.ToString();
        // // �̼� 3
        // QuestSlideBar1.value = (NowMissionInfo[2].NowMissionTargetValue / NowMissionInfo[2].NowMissionValue);
        // Mission1AmountText.text = NowMissionInfo[2].NowMissionValue.ToString() + " / " + NowMissionInfo[2].NowMissionTargetValue.ToString();
    }

    //// �̼� ���� ���� �� -> 
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

    //// ���� �ܰ��� �̼ǵ� ������ ��� Ȯ�� �� �� �Լ�
    //public bool MissionDispather(List<MissionData> NowMission)
    //{
    //    NowMission

    //    int ConditionAmount = NowMission.ConditionAmountN;

    //    switch (NowMission.MissionCondition)          // ���� üũ
    //    {
    //        case 0:         // ������ ���ٸ� �����̴�
    //            {
    //                isMissionConditionAccept = false;
    //            }
    //            break;
    //        case 101:       // 1�� ��ȭ N �̻� ����
    //            {
    //                // �����̿� ������ �׷��ֱ�

    //                if (PlayerInfo.Instance.m_MyMoney >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;

    //                    // �Լ��� �̵�, �ۼ������� ���� �ϴ� �Լ�
    //                    IfMissionConditionAccept(NowMission);
    //                }

    //                nowMissionStep.NowMissionValue = PlayerInfo.Instance.m_MyMoney;
    //            }
    //            break;
    //        case 102:       // 2�� ��ȭ N �̻� ����
    //            {
    //                if (PlayerInfo.Instance.m_SpecialPoint >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 103:       // [��� ������] ������ N ȸ ����
    //            {
    //                if (PlayerInfo.Instance.m_SpecialPoint >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 104:       // [��� ���Ӽ�] ���Ӽ� N ȸ ����
    //            {
    //                if (PlayerInfo.Instance.m_SpecialPoint >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 105:       // [��� �帣�� ��Ʋ��] �帣�� ���� N ȸ
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
    //        case 106:       // �б���� N �� �̻� �޼�
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
    //        case 107:       // ��ũ INT(N) ��� �̻� �޼�
    //            {
    //                if ((int)PlayerInfo.Instance.m_CurrentRank - 1 >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;
    //                }
    //            }
    //            break;
    //        case 108:       // [��� �л� ������ ��Ʋ��] �л� �� ������ N ȸ Ȯ��
    //            {
    //                if (PlayerInfo.Instance.StudentProfileClickCount >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 109:       // [��� ���� ������ ��Ʋ��] ���� �� ������ N ȸ Ȯ��
    //            {
    //                if (PlayerInfo.Instance.TeacherProfileClickCount >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 110:       // ���� ID � �ر݉����
    //            {
    //                if (PlayerInfo.Instance.UnLockTeacherCount >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        case 111:       // [��� �帣�� ��Ʋ��] �帣�� ��ġ N ȸ
    //            {
    //                if (PlayerInfo.Instance.GenreRoomClickCount >= ConditionAmount)
    //                {
    //                    isMissionConditionAccept = true;


    //                }
    //            }
    //            break;
    //        //case 112:       // ���� ID N ����
    //        //    {
    //        //        int tempClassTime = 0;
    //        //        // ClassInfo.FindLessonClassIDCount(_classCondition.ClassID, _classCondition.ClassType, _classCondition.Numberoflessons, SelectClass.m_GameDesignerData);

    //        //        if (tempClassTime >= ConditionAmount)
    //        //        {
    //        //            isMissionConditionAccept = true;


    //        //        }
    //        //    }
    //        //    break;
    //        case 113:       // [��� ���� ��Ʋ�] ���� ���� �� N �޼�
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

    //    return false; // ��� ������ �����Ѵٸ�
    //}
}
