using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 2023. 02. 14 
/// 
/// ���� Json ������ �ε� �� �����͵��� ������ Ŭ����
/// 
/// ����Ʈ�� �̸���, ���̽� ������ �̸��� ���ƾ� �����Ͱ� �� �� ����
/// </summary>
public class AllOriginalJsonData
{
    private static AllOriginalJsonData instance = null;

    public static AllOriginalJsonData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AllOriginalJsonData();
            }

            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public List<GameJamInfo> OriginalGameJamData = new List<GameJamInfo>();
    public List<InJaeRecommendData> OriginalInJaeRecommendData = new List<InJaeRecommendData>();
    // �̺�Ʈ ���� ����������
    public List<SuddenEventTableData> OriginalSuddenEventDataList = new List<SuddenEventTableData>();                           // ���� �����̺�Ʈ ������
    //public List<SimpleExecutionEventReward> SimpleExecutionEventRewardListData = new List<SimpleExecutionEventReward>();
    //public List<OptionChoiceEventReward> OptionChoiceEventRewardListData = new List<OptionChoiceEventReward>();
    public List<EventScript> EventScriptListData = new List<EventScript>();

    public List<SimpleExecutionEventReward> SimpleEventReward = new List<SimpleExecutionEventReward>();                         //  ��밡���� �����̺�Ʈ ���
    public List<OptionChoiceEventReward> ChoiceEventReward = new List<OptionChoiceEventReward>();
    // �л�, ����, ���� ����������
    //public List<CommonMailBox> OriginalMailData = new List<CommonMailBox>();                                                  // ���� �Ϲݸ��� ������
    //public List<ReportMailBox> ?
    public List<StudentSaveData> OriginalStudentData = new List<StudentSaveData>();                                             // ���� ��ü�л� ������
    public List<ProfessorSaveData> OriginalTeacherhData = new List<ProfessorSaveData>();                                        // ���� ��ü���� ������
    public List<ProfessorSaveData> OriginalInstructorData = new List<ProfessorSaveData>();                                      // ���� ��ü���� ������
    public List<RankScript> OriginalRankScripteData = new List<RankScript>();
    public List<AIAcademyInfo> OriginalAIAcademyInfoData = new List<AIAcademyInfo>();                                           /// 7.14 : ���� �߰��� AI��ī���� ������
    public List<StudentLastName> OriginalStudentLastNameData = new List<StudentLastName>();                                     /// 7.14 : ���� �߰��� �л� �̸� Ǯ
    public List<GameShowData> OriginalRandomGameShowData = new List<GameShowData>();
    //public List<SpecialGameShowData> OriginalSpecialGameShowData = new List<SpecialGameShowData>();
    public List<EmailData> OriginalEmailData = new List<EmailData>();                                                           // ���� ���� ������
    public List<RewardEmailData> OriginalRewardEmailData = new List<RewardEmailData>();                                         // ���� ��ũ ���� ���� ������
    public List<BonusSkillConditions> OriginalBonusSkillConditionData = new List<BonusSkillConditions>();                       // ���� ���ʽ� ��ų ���� ������
    public List<BonusSkillScript> OriginalBonusSkillScriptData = new List<BonusSkillScript>();                                  // ���� ���ʽ� ��ų ��ũ��Ʈ ������
    public List<ClassAlramScript> OriginalClassAlramScriptData = new List<ClassAlramScript>();

    // ����Ʈ
    public List<MissionData> OriginalMissionData = new List<MissionData>();                                                                   // ����Ʈ ������

    // ����Ʈ �߰�
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //Convert �� ���̽� �����͸� ���� �ʿ��� ���� �־��ֱ� -> �� ��ũ��Ʈ����

}
