using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 2023. 02. 14 
/// 
/// 원본 Json 파일을 로드 할 데이터들의 모음인 클래스
/// 
/// 리스트의 이름과, 제이슨 파일의 이름이 같아야 데이터가 들어갈 수 있음
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
    // 이벤트 관련 원본데이터
    public List<SuddenEventTableData> OriginalSuddenEventDataList = new List<SuddenEventTableData>();                           // 원본 돌발이벤트 데이터
    //public List<SimpleExecutionEventReward> SimpleExecutionEventRewardListData = new List<SimpleExecutionEventReward>();
    //public List<OptionChoiceEventReward> OptionChoiceEventRewardListData = new List<OptionChoiceEventReward>();
    public List<EventScript> EventScriptListData = new List<EventScript>();

    public List<SimpleExecutionEventReward> SimpleEventReward = new List<SimpleExecutionEventReward>();                         //  사용가능한 선택이벤트 목록
    public List<OptionChoiceEventReward> ChoiceEventReward = new List<OptionChoiceEventReward>();
    // 학생, 강사, 메일 원본데이터
    //public List<CommonMailBox> OriginalMailData = new List<CommonMailBox>();                                                  // 예전 일반메일 데이터
    //public List<ReportMailBox> ?
    public List<StudentSaveData> OriginalStudentData = new List<StudentSaveData>();                                             // 원본 전체학생 데이터
    public List<ProfessorSaveData> OriginalTeacherhData = new List<ProfessorSaveData>();                                        // 원본 전체교수 데이터
    public List<ProfessorSaveData> OriginalInstructorData = new List<ProfessorSaveData>();                                      // 원본 전체교수 데이터
    public List<RankScript> OriginalRankScripteData = new List<RankScript>();
    public List<AIAcademyInfo> OriginalAIAcademyInfoData = new List<AIAcademyInfo>();                                           /// 7.14 : 새로 추가된 AI아카데미 데이터
    public List<StudentLastName> OriginalStudentLastNameData = new List<StudentLastName>();                                     /// 7.14 : 새로 추가된 학생 이름 풀
    public List<GameShowData> OriginalRandomGameShowData = new List<GameShowData>();
    //public List<SpecialGameShowData> OriginalSpecialGameShowData = new List<SpecialGameShowData>();
    public List<EmailData> OriginalEmailData = new List<EmailData>();                                                           // 원본 메일 데이터
    public List<RewardEmailData> OriginalRewardEmailData = new List<RewardEmailData>();                                         // 원본 랭크 보상 메일 데이터
    public List<BonusSkillConditions> OriginalBonusSkillConditionData = new List<BonusSkillConditions>();                       // 원본 보너스 스킬 조건 데이터
    public List<BonusSkillScript> OriginalBonusSkillScriptData = new List<BonusSkillScript>();                                  // 원본 보너스 스킬 스크립트 데이터
    public List<ClassAlramScript> OriginalClassAlramScriptData = new List<ClassAlramScript>();

    // 퀘스트
    public List<MissionData> OriginalMissionData = new List<MissionData>();                                                                   // 퀘스트 데이터

    // 리스트 추가
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //Convert 한 제이슨 데이터를 이제 필요한 곳에 넣어주기 -> 각 스크립트에서

}
