using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// 여기다가 이벤트 관련 제이슨 파일들을 받을 클래스를 만든다
/// </summary>
/// 
// 이벤트 리스트
public class SuddenEventTableData
{
    public bool IsSuddenEvent;                                              // 돌발/지정 이벤트 여부
    public int SuddenEventID;                                            // 이벤트 ID
    public string EventName;                                                // 이벤트 이름
    public int[] SuddenEventDate = new int[4];                              // 이벤트가 실행될 날짜 배열( 년, 월, 주, 요일)

    public int EventDelayTime;                                              // 이벤트의 딜레이시간(초)
    public int SuddenEventType;                                             // 이벤트의 종류를 나눈 인덱스
    public string Where;                                                    // 대상지정이벤트 - 파견장소 어디?
    public int ConditionType;                                               // 이벤트의 조건 유형

    public bool SuddenEventRepeat;                                          // 이벤트의 반복 여부
    public Dictionary<string, int> SuddenEventRepeatGap;                    // 이벤트의 반복 주기(년)
    public bool SuddenEventOpen;                                            // 이벤트의 해금 여부
    public int SuddenEventDuration;                                         // 이벤트 시행기간     --> 이벤트가 실행되고 얼마나 걸릴것인지
    public int SuddenEventTime;                                             // 이벤트 상호작용시간   --> 단순이벤트시 시간이 얼마나 걸리는 것인지
    public int SuddenEventScript;                                           // 이벤트 내용 텍스트 구별 인덱스

    public int Pay;                                                         // 이벤트 소모 재화 인덱스
    public int Amount;                                                      // 소모 재화량

    public int SuddenEventCondition1;                                       // 조건 1 index
    public int Amount1N;                                                    // 조건 1 N 값
    public int Amount1M;                                                    // 조건 1 M 값
    public int SeddenEventCondition2;                                       // 조건 2 index
    public int Amount2N;                                                    // 조건 2 N 값
    public int Amount2M;                                                    // 조건 2 M 값
    public int SeddenEventCondition3;                                       // 조건 3 index
    public int Amount3N;                                                    // 조건 3 N 값
    public int Amount3M;                                                    // 조건 3 M 값

    public int SelectRewardIndex1;                                          // 선택지 보상 1
    public int SelectRewardIndex2;                                          // 선택지 보상 2
    public int SelectRewardIndex3;                                          // 선택지 보상 3
    public int RewardIndex1;                                                // 단순 실행 보상 1
    public int RewardIndex2;                                                // 단순 실행 보상 2
    public int RewardIndex3;                                                // 단순 실행 보상 3

    public int Teacher;     // UnLockTeacherID;                                          // 해금되는 강사 ID
    public int Company;        // UnLockCompanyID;                                             // 해금되는 회사 ID
}

// 단순 실행 보상
public class SimpleExecutionEventReward
{
    public int EventID;                        // 단순실행보상 ID
    public string RewardTaker;                 // 보상을 받을 객체
    public int Reward;                         // 보상 인덱스
    public int Amount;                         // 보상 양
}

// 선택지 보상
public class OptionChoiceEventReward
{
    public int SelectEventID;                   // 선택지 보상 ID
    public string SelectScript;                 // 선택지 내용
    public int SelectPay;                       // 소모재화 종류 인덱스
    public int PayAmount;                       // 소모 재화량

    public string RewardTaker;                  // 보상을 받는 객체

    public int Reward1;                         // 보상1 인덱스
    public int Amount1;                         // 보상1 양
    public int Reward2;                         // 보상2 인덱스
    public int Amount2;                         // 보상2 양
}

// 이벤트 시나리오 스크립트
public class EventScript
{
    public int EventScriptID;                    // 이벤트 스크립트 ID
    public string EventScriptTextStart;             // 이벤트 시작 스크립트
    public string EventScriptTextMiddle;            // 이벤트 중간 스크립트
    public string EventScriptTextSelect1;           // 이벤트 선택지 1
    public string EventScriptTextSelect2;           // 선택지 2
    public string EventScriptTextSelect3;           // 선택지 3
    public string EventScriptTextFin;               // 이벤트 스크립트 마지막
}

// 사용한 이벤트의 정보를 모으는 클래스
public class UsedEventRepeatData
{
    public int SuddenEventID;        // 이벤트의 ID
    public int YearData;                // 사용 가능한 년도
    public int MonthData;               // 사용 가능한 월
    public int WeekData;                // 사용 가능한 주
    public int DayData;                 // 사용 가능한 날
}

//public class EventJsonCollection : MonoBehaviour
//{
//    // Json 파일을 파싱해서 그 데이터들을 다 담아 줄 리스트 변수
//    // 이 변수들도 EventSchedule 의 Instance.변수 들에 넣어주고 쓰도록 하자
//    // 돌발이벤트
//    public List<SuddenEventTableData> AllSuddenEventList = new List<SuddenEventTableData>();                            // 전체 돌발(고정) 이벤트
//    //
//    public List<SimpleExecutionEventReward> SimpleEventReward = new List<SimpleExecutionEventReward>();                 //  사용가능한 선택이벤트 목록
//    public List<OptionChoiceEventReward> CHoiceEventReward = new List<OptionChoiceEventReward>();                       // 현재 나의 이벤트 목록
//    // 선택이벤트
//    // public List<> AllSelectEventList = new List<>();                             // 전체 선택 이벤트
//    public List<EventScript> PrevIChoosedEvent = new List<EventScript>();                                               // 현재 내가 선택한 이벤트(최대 2개) 담아 줄 임시 변수
//    public SuddenEventTableData TempIChoosed;                                                                           // 임시로 내가 방금 누른 것
    
//}