using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using StatData.Runtime;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum RewardTaker
{
    None, // 초기화값 에러방지
    academy, // 아카데미 자체
    allstu, // 보유한 모든 학생
    allprof, // 보유한 모든 강사
    design, // 보유한 기획파트 학생 모두
    art, // 보유한 아트파트 학생 모두
    pro, // 보유한 플밍파트 학생 모두
    alone,   // 학생 이벤트 실행자 단독
    teacherAlone    // 강사 이벤트 실행자 단독
}

public struct EachScript
{
    public List<ESpeakerImgIndex> speaker;
    public List<string> splitText;
}

// 화자가 각 텍스트 당 한명 일때 
public struct SplitScript
{
    public int PossibleSelectCount;
    public EachScript SplitEventScriptTextStart;
    public EachScript SplitEventScriptTextMiddle;
    public EachScript SplitEventScriptTextSelect1;
    public EachScript SplitEventScriptTextSelect2;
    public EachScript SplitEventScriptTextSelect3;
    public EachScript SplitEventScriptTextFin;
}

// 이벤트 보상에 대한 정보를 담는 구조체
public struct RewardInfo
{
    public int RewardID;                // 보상 아이디인덱스
    public int RewardAmount;            // 보상 량
    public string RewardName;           // 보상 이름 출력
    public string RewardTakerName;      // 보상받는 사람
    // TODO : 강사 회사 정보 추가

    public int TeacherID;
    public int CompanyID;
    public string RewardSpriteName;
}

public struct OptionEventPay
{
    public int SelectPay1;
    public int PayAmount1;

    public int SelectPay2;
    public int PayAmount2;

    public int SelectPay3;
    public int PayAmount3;
}

public class NowDepartmentStudentList
{
    public Sprite studentImg;
    public string studentName;
    //public Button NowStudentButton;
}

/// <summary>
/// 2023. 04. 27 Mang
/// 
/// 이벤트 들의 동작을 담당하는 클래스.
/// 
/// </summary>
public class EventScriptManager : MonoBehaviour
{
    [SerializeField] private SuddenEventPanel suddenEventPanel;
    [SerializeField] private GameObject studentInfo;        // 타겟 보상을 주기 위한..! 이거 저장해야하나

    [SerializeField] private InJaeRecommend NowCompanyList;

    IEnumerator SuddenEventCheckCoroutine;

    // ID, 나눠진 텍스트 담을 완전체딕셔너리
    private Dictionary<int, SplitScript> SplitEventTextDic;
    [SerializeField] private EventScheduleSystem eventScheduleSystem;

    [SerializeField] private ProfessorController m_SelectProfessor;     // 모든 강사를 돌면서 해금할 강사를 찾기 위한 리스트

    private int nowEventUIClickCount = 0;
    private bool isReadyNowEventPopUp = false;
    // 임시 디버깅용
    private bool TempIsReadyNowEventPopUp = false;

    public delegate void DelegateFunction(bool value);
    DelegateFunction delegateF;

    private bool StartScriptReady = false;
    private bool MiddelScriptReady = false;
    private bool FinScriptReady = false;

    private int NowEventType = 0;
    private SplitScript nowEventScript;
    private string tempNowSpeakerText;

    int SelectedOptionNumber = 0; // 선택지 보상에서 몇번째를 선택했는지를 저장하는 변수와 연결해야함.

    RewardInfo[] NowEventRewardInfo = new RewardInfo[6]; // 이벤트 선택지의 보상에 대한 정보 모음
    OptionEventPay OptionEventNeedPay;      // 현재 이벤트가 선택이벤트/날짜지정이벤트 일때? 소호재화를 저장하기 위한 변수
    int NowEvnetID = 0;             // 현재 선택지이벤트의 아이디를 저장하기 위한 변수
    int UnLockTeacherIndex = 0;
    int UnLockCompanyIndex = 0;

    float TimeBarStartTime;
    float TimeBarCurrentTime;
    float TimeBarTime;
    string[] timebartext = new string[3];
    Image timebarimg;
    bool isLoadingBarEnd = false;
    int TimeBarDuration;

    bool isRewardNothing = true;

    Student nowTargetStudent = new Student();           // 대상선택 이벤트 시 선택한 학생을 저장할 변수
    ProfessorStat nowTargetTeacher = new ProfessorStat();

    List<NowDepartmentStudentList> EachDepartmentStduentList = new List<NowDepartmentStudentList>();

    public Dictionary<int, SplitScript> MySplitEventTextDic
    {
        get { return SplitEventTextDic; }
    }

    // Start is called before the first frame update
    void Start()
    {
        SplitEventTextDic = new Dictionary<int, SplitScript>();
        // EventTextScriptSplit();
        // ScreenTouchButton.onClick.AddListener(EventTextScriptSplit);
        suddenEventPanel.GetOption1Button.onClick.AddListener(IfClickOptionButton);
        suddenEventPanel.GetOption2Button.onClick.AddListener(IfClickOptionButton);
        suddenEventPanel.GetOption3Button.onClick.AddListener(IfClickOptionButton);
        // ---------------------------------------
        suddenEventPanel.GetGameDesignerIndex.onClick.AddListener(IfClickEachStduentDepartmentIndexButton);
        suddenEventPanel.GetArtIndex.onClick.AddListener(IfClickEachStduentDepartmentIndexButton);
        suddenEventPanel.GetProgrammerIndex.onClick.AddListener(IfClickEachStduentDepartmentIndexButton);

        suddenEventPanel.GetStudentListButton1.onClick.AddListener(IfClickEachStudent);
        suddenEventPanel.GetStudentListButton2.onClick.AddListener(IfClickEachStudent);
        suddenEventPanel.GetStudentListButton3.onClick.AddListener(IfClickEachStudent);
        suddenEventPanel.GetStudentListButton4.onClick.AddListener(IfClickEachStudent);
        suddenEventPanel.GetStudentListButton5.onClick.AddListener(IfClickEachStudent);
        suddenEventPanel.GetStudentListButton6.onClick.AddListener(IfClickEachStudent);
        // ---------------------------------------
        suddenEventPanel.GetTargetSetOKButton.onClick.AddListener(IfClickTargetEventSetOKButton);
        // ---------------------------------------
        suddenEventPanel.GetGiveUpSelectEventButton.onClick.AddListener(eventScheduleSystem.IfGiveUpButtonClick);
        suddenEventPanel.GetGiveUpTargetEventButton.onClick.AddListener(eventScheduleSystem.IfGiveUpButtonClick);
        // ---------------------------------------
        suddenEventPanel.GetScenarioPanelNextButton.onClick.AddListener(ClickScenarioPanelNextButton);
        // ---------------------------------------
        suddenEventPanel.GetSaveRewardButton.onClick.AddListener(IfIClickGetRewardButton);

        EventTextScriptSplit();

        delegateF += ChangeReadyNowEventPopUp;
        delegateF(TempIsReadyNowEventPopUp);
    }

    public void ChangeReadyNowEventPopUp(bool value)
    {
        if (isReadyNowEventPopUp != value)
        {
            Debug.Log("isReadyNowEventPopUp : " + isReadyNowEventPopUp);

            TempIsReadyNowEventPopUp = isReadyNowEventPopUp;
        }

    }

    // Update is called once per frame
    void Update()
    {


        FirstCheckReadySuddenEventList();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("현재 Time.unscaledTime : " + Time.unscaledTime);
            Debug.Log("현재 Time.time : " + Time.time);
            Debug.Log("현재 Time.realtimeSinceStartup : " + Time.realtimeSinceStartup);
        }
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("현재 Time.unscaledTime : " + Time.unscaledTime);
        //     Debug.Log("현재 Time.time : " + Time.time);
        //     Debug.Log("현재 Time.realtimeSinceStartup : " + Time.realtimeSinceStartup);
        // }

        // if (suddenEventPanel.GetSimpleEventTimeBarPanel.activeSelf == true)     // 이벤트팝업 창이 켜지면 현재 시간 저장.
        // {
        //     TimeBarStartTime = Time.unscaledTime;
        // 
        //     UpdateLoadingBar(TimeBarDuration);
        // }

        if (suddenEventPanel.GetSimpleEventTimeBarPanel.activeSelf == true)
        {
            // prograss 는 진행 수치. 이것을 가지고 하면 되려나
            float prograss = (Time.unscaledTime - TimeBarStartTime) / TimeBarDuration;  //Mathf.Clamp01((Time.unscaledTime - TimeBarStartTime) / TimeBarDuration);
            int TimeBarCount = 3;

            if (nowEventScript.SplitEventScriptTextSelect2.splitText.Count == 0)
            {
                TimeBarCount = 1;
            }
            else if (nowEventScript.SplitEventScriptTextSelect3.splitText.Count == 0)
            {
                TimeBarCount = 2;
            }

            float[] thresholds = new float[TimeBarCount];
            for (int i = 0; i < timebartext.Length; i++)
            {
                thresholds[i] = (i + 1) / (float)timebartext.Length;
            }

            UpdateLoadingBar(prograss, thresholds, timebartext);

            if (prograss >= 1f)          // 로딩바가 100퍼센트 다 찼는지 체크하기 위한 것. 다 찼으면 이제 이 문을 안들어오면 되니까 상관 없네 여기선
            {
                Debug.Log("로딩바 진행 완");
                FinScriptReady = true;
                suddenEventPanel.PopOffTimeBarPanel();
                ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
            }
        }

    }

    // // 전체적으로 데이터 나누고 넣어주는 곳
    public void EventTextScriptSplit()
    {
        if (AllOriginalJsonData.Instance.EventScriptListData != null)
        {
            // 원본 데이터 전체 돌면서 텍스트를 나눠서 저장 해 줄 것임
            int allScriptCount = AllOriginalJsonData.Instance.EventScriptListData.Count;
            for (int i = 0; i < allScriptCount; i++)
            {
                SplitScript tempSplitScript = new SplitScript();
                tempSplitScript.SplitEventScriptTextStart = new EachScript();
                tempSplitScript.SplitEventScriptTextStart.splitText = new List<string>();
                tempSplitScript.SplitEventScriptTextMiddle = new EachScript();
                tempSplitScript.SplitEventScriptTextMiddle.splitText = new List<string>();
                tempSplitScript.SplitEventScriptTextFin = new EachScript();
                tempSplitScript.SplitEventScriptTextFin.splitText = new List<string>();

                tempSplitScript.SplitEventScriptTextSelect1 = new EachScript();
                tempSplitScript.SplitEventScriptTextSelect1.splitText = new List<string>();
                tempSplitScript.SplitEventScriptTextSelect2 = new EachScript();
                tempSplitScript.SplitEventScriptTextSelect2.splitText = new List<string>();
                tempSplitScript.SplitEventScriptTextSelect3 = new EachScript();
                tempSplitScript.SplitEventScriptTextSelect3.splitText = new List<string>();

                tempSplitScript.SplitEventScriptTextStart =
                    SplitSpeaker(AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptTextStart);
                tempSplitScript.SplitEventScriptTextMiddle =
                    SplitSpeaker(AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptTextMiddle);

                if (AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptTextSelect1 == "")
                {
                    tempSplitScript.PossibleSelectCount = 0;
                }
                else if (AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptTextSelect2 == "")
                {
                    tempSplitScript.PossibleSelectCount = 1;
                }
                else if (AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptTextSelect3 == "")
                {
                    tempSplitScript.PossibleSelectCount = 2;
                }
                else
                {
                    tempSplitScript.PossibleSelectCount = 3;
                }

                tempSplitScript.SplitEventScriptTextSelect1 =
                    SplitSpeaker(AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptTextSelect1);
                tempSplitScript.SplitEventScriptTextSelect2 =
                    SplitSpeaker(AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptTextSelect2);
                tempSplitScript.SplitEventScriptTextSelect3 =
                    SplitSpeaker(AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptTextSelect3);

                tempSplitScript.SplitEventScriptTextFin =
                    SplitSpeaker(AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptTextFin);

                SplitEventTextDic.Add(AllOriginalJsonData.Instance.EventScriptListData[i].EventScriptID,
                    tempSplitScript);

                //cfDebug.Log("제이슨 데이터 로드가 안됬다");
            }
        }
        else
        {
            Debug.Log("제이슨 데이터가 로드되지 않았따");
        }
    }

    // 각 텍스트 목록을 "/" 와 "#" 로 나눠서 담기
    public EachScript SplitSpeaker(string tempText)
    {
        EachScript tempScript = new EachScript();
        tempScript.splitText = new List<string>();
        tempScript.speaker = new List<ESpeakerImgIndex>();

        if (tempText != "")
        {
            string[] temptext = tempText.Split("/");        // 원본을 텍스트 갯수로 먼저 나눔

            for (int i = 0; i < temptext.Length; i++)
            {
                tempScript.speaker.Add(ESpeakerImgIndex.None);

                string temp = temptext[i];
                int tempIndex = temp.IndexOf("#");

                if (tempIndex >= 0)         // speaker 가 있을 때
                {
                    tempScript.speaker[i] = (ESpeakerImgIndex)ESpeakerImgIndex.Parse(typeof(ESpeakerImgIndex), temp.Substring(0, tempIndex));
                    // tempScript.speaker.Add((Speaker)Speaker.Parse(typeof(Speaker), tempText.Substring(0, tempIndex)));  // string 화자를 enum으로 바꿔주기
                    tempScript.splitText.Add(temp.Substring(tempIndex + 1));
                }
                else
                {
                    if ((i - 1) >= 0)
                    {
                        if (tempScript.speaker[i - 1] != ESpeakerImgIndex.None)              // 현재 문장 speaker 없음. 이전 speaker를 사용할것
                        {
                            tempScript.speaker[i] = tempScript.speaker[i - 1];
                        }
                    }

                    tempScript.splitText.Add(temp);

                }
            }
        }
        return tempScript;
    }

    // 유아이 띄우는 함수는 따로 만들자
    // 기존 popUP 함수 사용 해서...

    // update문에서 사용할 이벤트 리스트가 비어있는지 계속 체크 -> 맨 처음에 뜰 시나리오 유아이ㄴ
    public void FirstCheckReadySuddenEventList()
    {
        // 스크립트 아이디, 이벤트 아이디, 보상 아이디
        // 이벤트 리스트를 계속 체크 -> 이벤트가 생긴다?
        if (eventScheduleSystem.TodaySuddenEventList.Count != 0)
        {
            if (GameTime.Instance.IsGameMode == true && isReadyNowEventPopUp == false)
            {

                isReadyNowEventPopUp = true;

                // OptionEventNeedPay.SelectPay = AllOriginalJsonData.Instance.OptionChoiceEventRewardListData[0].SelectPay;
                TimeBarDuration = eventScheduleSystem.TodaySuddenEventList[0].SuddenEventTime;
                NowEventType = eventScheduleSystem.TodaySuddenEventList[0].SuddenEventType;
                nowEventScript = SplitEventTextDic[eventScheduleSystem.TodaySuddenEventList[0].SuddenEventScript];

                UnLockTeacherIndex = eventScheduleSystem.TodaySuddenEventList[0].Teacher;
                UnLockCompanyIndex = eventScheduleSystem.TodaySuddenEventList[0].Company;

                // 우선은 세개로 고정 -> 각 선택갯수대로 
                if (nowEventScript.SplitEventScriptTextSelect1.splitText.Count != 0)
                {
                    timebartext[0] = nowEventScript.SplitEventScriptTextSelect1.splitText[0].ToString();
                }

                if (nowEventScript.SplitEventScriptTextSelect2.splitText.Count != 0)
                {
                    timebartext[1] = nowEventScript.SplitEventScriptTextSelect2.splitText[0].ToString();
                }

                if (nowEventScript.SplitEventScriptTextSelect3.splitText.Count != 0)
                {
                    timebartext[2] = nowEventScript.SplitEventScriptTextSelect3.splitText[0].ToString();
                }

                PeekOrSetEventReward(true); // 인자 값이 true 일 때 -> UI 에 쓸 텍스트, 이미지, 변경수치 리턴

                StartScriptReady = true;
                // NowEventReward = SplitEventTextDic[eventScheduleSystem.ReadySuddenEventList[0].RewardIndex1];
                ReadyScenarioPanel(nowEventScript.SplitEventScriptTextStart);

                // 삭제 -> 마지막 보상 버튼을 누를 시 isReadyNowEventPopUp = false 해주고(이건 팝오프 될때마다 해주기) 리스트도 비워주기
                // eventScheduleSystem.ReadySuddenEventList.RemoveAt(0);
            }
        }
    }

    public void ReadyScenarioPanel(EachScript nowScript)
    {
        nowEventUIClickCount = 0;

        // 현재 이벤트의 스크립트      

        // 단순 or 대상 선택 이벤트의 start대사
        string nowtempScript = nowScript.splitText[nowEventUIClickCount];
        string nowTempScriptSpeaker = nowScript.speaker[nowEventUIClickCount].ToString();
        suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);

        // UI 보여주는 처리
        suddenEventPanel.PopUpStartEventScriptPanel();
    }

    public void PutOnScreenRewardPanel()
    {
        PeekOrSetEventReward(true); // 이벤트 보상 내용을 가져온다.
                                    // 강사해금 - 단순실행 보상으로만 풀리나?
        if (UnLockTeacherIndex != 0)
        {
            suddenEventPanel.ReadyTeacherUnLockPanel(NowEventRewardInfo);
        }
        else if (UnLockCompanyIndex != 0)
        {
            suddenEventPanel.ReadyCompanyUnLockPanel(NowEventRewardInfo, NowCompanyList);
        }
        else
        {
            suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
        }
        suddenEventPanel.PopOffOptionChoiceEvent();
        suddenEventPanel.PopOffStartEventScriptPanel();

        int removeEventCount = 0;

        for (int i = 0; i < NowEventRewardInfo.Length; i++)
        {
            if (NowEventRewardInfo[i].RewardAmount != 0 || eventScheduleSystem.TodaySuddenEventList[0].Teacher != 0)
            {
                suddenEventPanel.PopUpRewardPanel();

                break;
            }
            else
            {
                if (removeEventCount == 0)
                {
                    isReadyNowEventPopUp = false;

                    removeEventCount++;
                }
            }
        }

        if (removeEventCount != 0)
        {
            eventScheduleSystem.TodaySuddenEventList.RemoveAt(0);
        }
        FinScriptReady = false;
    }

    // 스타트스크립트의 마지막 대사 후 버튼 클릭 시 세가지의 다음순서 패널로 알맞게 찾아가기 위한 함수
    public void ClickScenarioPanelNextButton()
    {
        nowEventUIClickCount++; // "/" 로 나눠진 대화를 체크 하기 위한 변수 

        // start는 이벤트를 시작시키는 곳에서 true로.
        if (StartScriptReady)
        {
            // 잘린 대화 있는지 체크, 있으면 대화 넣고 띄우기, 없으면 다음 UI로 진행
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextStart.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextStart.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextStart.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
            }
            else
            {
                if (NowEventType == 2) // 선택지
                {
                    StartScriptReady = false;


                    ReadyChoiceOptionScriptText();
                }
                else if (NowEventType == 1) // 단순실행
                {
                    StartScriptReady = false;
                    nowEventUIClickCount = 0;
                    suddenEventPanel.PopOffStartEventScriptPanel();

                    ReadyTimeBarPanel();
                }
                else if (NowEventType == 3) // 대상선택
                {
                    StartScriptReady = false;

                    ReadyTargetSelectScriptText();
                }
            }
        }
        else if (MiddelScriptReady)
        {
            // 미들 있는 지 체크
            // 잘린 대화 있는지 체크, 있으면 대화 넣고 띄우기, 없으면 다음 UI로 진행
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextMiddle.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextMiddle.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextMiddle.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
            }
            else
            {
                // 
                MiddelScriptReady = false;
                suddenEventPanel.PopOffStartEventScriptPanel();
                //FinScriptReady = true;
            }
        }
        // 미들 종료 후 코루틴 호출해야 하는데?


        else if (FinScriptReady)
        {
            // 잘린 대화 있는지 체크, 있으면 대화 넣고 띄우기, 없으면 다음 UI로 진행
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextFin.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextFin.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextFin.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
            }
            else // 보상 창 띄우기
            {
                PutOnScreenRewardPanel();
            }
        }
        else
        {
            switch (SelectedOptionNumber)
            {
                case 1:
                    {
                        // 잘린 대화 있는지 체크, 있으면 대화 넣고 띄우기, 없으면 다음 UI로 진행
                        if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextSelect1.splitText.Count)
                        {
                            string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextSelect1.speaker[nowEventUIClickCount].ToString();
                            string nowtempScript = nowEventScript.SplitEventScriptTextSelect1.splitText[nowEventUIClickCount];

                            suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
                        }
                        else if (nowEventScript.SplitEventScriptTextFin.splitText.Count != 0)
                        {
                            FinScriptReady = true;

                            ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
                            suddenEventPanel.PopOffOptionChoiceEvent();

                        }
                        else if (nowEventScript.SplitEventScriptTextFin.splitText.Count == 0)
                        {
                            PutOnScreenRewardPanel();
                        }
                    }
                    break;
                case 2:
                    {
                        // 잘린 대화 있는지 체크, 있으면 대화 넣고 띄우기, 없으면 다음 UI로 진행
                        if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextSelect2.splitText.Count)
                        {
                            string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextSelect2.speaker[nowEventUIClickCount].ToString();
                            string nowtempScript = nowEventScript.SplitEventScriptTextSelect2.splitText[nowEventUIClickCount];

                            suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
                        }
                        else if (nowEventScript.SplitEventScriptTextFin.splitText.Count != 0)
                        {
                            FinScriptReady = true;

                            ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
                            suddenEventPanel.PopOffOptionChoiceEvent();

                        }
                        else if (nowEventScript.SplitEventScriptTextFin.splitText.Count == 0)
                        {
                            PutOnScreenRewardPanel();
                        }
                    }
                    break;
                case 3:
                    {
                        // 잘린 대화 있는지 체크, 있으면 대화 넣고 띄우기, 없으면 다음 UI로 진행
                        if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextSelect3.splitText.Count)
                        {
                            string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextSelect3.speaker[nowEventUIClickCount].ToString();
                            string nowtempScript = nowEventScript.SplitEventScriptTextSelect3.splitText[nowEventUIClickCount];

                            suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
                        }
                        else if (nowEventScript.SplitEventScriptTextFin.splitText.Count != 0)
                        {
                            FinScriptReady = true;

                            ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
                            suddenEventPanel.PopOffOptionChoiceEvent();

                        }
                        else if (nowEventScript.SplitEventScriptTextFin.splitText.Count == 0)
                        {
                            PutOnScreenRewardPanel();
                        }
                    }
                    break;
            }

        }

        #region 이전의 돌발이벤트 루프 도는 코드
        /*
                 if (FinScriptReady) // 현재 대화가 start 인지 fin 인지 middle인지 체크
        {
            if (MiddelScriptReady)
            {
                // 잘린 대화 있는지 체크, 있으면 대화 넣고 띄우기, 없으면 다음 UI로 진행
                if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextFin.splitText.Count)
                {
                    string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextFin.speaker[nowEventUIClickCount].ToString();
                    string nowtempScript = nowEventScript.SplitEventScriptTextFin.splitText[nowEventUIClickCount];

                    suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript);
                }
                else // 보상 창 띄우기
                {
                    // 대상선택 이벤트의 실행시간이 끝났는지를 판별 -> 스크립트가 아니라 시간으로 판별해야하나? 아니면 이번 이벤트의 타입이 대상선택 이벤트라는것을 체크한 후 해야하나?
                    if (eventScheduleSystem.ReadySuddenEventList[0].SuddenEventDuration != 0)
                    {
                        suddenEventPanel.PopOffStartEventScriptPanel(); // 실행시간 존재 시 그냥 창 꺼주기

                        // if ((((GameTime.Instance.FlowTime.NowYear - 1) * 12 * 4 * 5) +
                        // ((GameTime.Instance.FlowTime.NowMonth - 1) * 4 * 5) + ((GameTime.Instance.FlowTime.NowWeek - 1) * 5) + GameTime.Instance.FlowTime.NowDay) =>
                        // eventEndDay)

                        if (nowEventUIClickCount <= nowEventScript.SplitEventScriptTextFin.splitText.Count)       // 대화 끝남 감지
                        {
                            PeekOrSetEventReward(true); // 이벤트 보상 내용을 가져온다.
                            // 강사해금 - 단순실행 보상으로만 풀리나?
                            if (UnLockTeacherIndex != 0)
                            {
                                suddenEventPanel.ReadyTeacherUnLockPanel();
                            }
                            else
                            {
                                suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
                            }
                            suddenEventPanel.PopOffOptionChoiceEvent();
                            suddenEventPanel.PopOffStartEventScriptPanel();
                            suddenEventPanel.PopUpRewardPanel();

                            FinScriptReady = false;
                            MiddelScriptReady = false;
                        }
                    }
                    else
                    {
                        if (nowEventScript.SplitEventScriptTextFin.splitText.Count != 0)
                        {
                            MiddelScriptReady = false;

                            if (nowEventUIClickCount <= nowEventScript.SplitEventScriptTextMiddle.splitText.Count)
                            {
                                suddenEventPanel.PopOffStartEventScriptPanel(); // 실행시간 존재 시 그냥 창 꺼주기

                                // 보상
                                // suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
                                // suddenEventPanel.PopOffStartEventScriptPanel();
                                // suddenEventPanel.PopUpRewardPanel();
                            }
                            // else
                            // {
                            // 추가로 남은 fin의 텍스트를 출력함


                            // ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
                            // suddenEventPanel.PopOffStartEventScriptPanel();
                            // }
                        }
                        else
                        {
                            PeekOrSetEventReward(true); // 이벤트 보상 내용을 가져온다.
                                                        // 강사해금 - 단순실행 보상으로만 풀리나?
                            if (UnLockTeacherIndex != 0)
                            {
                                suddenEventPanel.ReadyTeacherUnLockPanel();
                            }
                            else
                            {
                                suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
                            }
                            suddenEventPanel.PopOffOptionChoiceEvent();
                            suddenEventPanel.PopOffStartEventScriptPanel();
                            suddenEventPanel.PopUpRewardPanel();

                            FinScriptReady = false;
                            MiddelScriptReady = false;
                        }
                    }
                }
            }
            else
            {
                // 잘린 대화 있는지 체크, 있으면 대화 넣고 띄우기, 없으면 다음 UI로 진행
                if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextFin.splitText.Count)
                {
                    string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextFin.speaker[nowEventUIClickCount].ToString();
                    string nowtempScript = nowEventScript.SplitEventScriptTextFin.splitText[nowEventUIClickCount];

                    suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript);
                }
                else // 보상 창 띄우기
                {
                    PeekOrSetEventReward(true);
                    // 강사해금 - 단순실행 보상으로만 풀리나?
                    if (UnLockTeacherIndex != 0)
                    {
                        suddenEventPanel.ReadyTeacherUnLockPanel();
                    }
                    else
                    {
                        suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
                    }
                    suddenEventPanel.PopOffStartEventScriptPanel();
                    suddenEventPanel.PopUpRewardPanel();

                    FinScriptReady = false;
                    MiddelScriptReady = false;
                }
            }
        }
        else
        {
            // 잘린 대화 있는지 체크, 있으면 대화 넣고 띄우기, 없으면 다음 UI로 진행
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextStart.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextStart.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextStart.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript);
            }
            else
            {
                if (NowEventType == 2) // 선택지
                {
                    FinScriptReady = true;
                    MiddelScriptReady = true;

                    ReadyChoiceOptionScriptText();
                }
                else if (NowEventType == 1) // 단순실행
                {
                    nowEventUIClickCount = 0;
                    FinScriptReady = true;
                    MiddelScriptReady = true;
                    suddenEventPanel.PopOffStartEventScriptPanel();

                    ReadyTimeBarPanel();
                }
                else if (NowEventType == 3) // 대상선택
                {
                    FinScriptReady = true;
                    MiddelScriptReady = true;

                    ReadyTargetSelectScriptText();
                }
            }
        }
         */
        #endregion
    }

    // 각 종류의 이벤트에 눌리는 버튼들에 달아놓고, 미들 체크 후 없으면 그냥 쓰루, 있으면 시나리오패널 팝업 하기
    public void CheckMiddelScenarioPanelPop()
    {
        // 현재 진행중인 이벤트의 스크립트에 middle이 있는지 체크한다. "" 일 경우, 아무 것도 하지 않고 보상 창으로 넘어가도록 한다
        if (nowEventScript.SplitEventScriptTextMiddle.splitText.Count != 0)
        {
            ReadyScenarioPanel(nowEventScript.SplitEventScriptTextMiddle);

            if (NowEventType == 1)
            {
                suddenEventPanel.PopOffTimeBarPanel();
            }
            else if (NowEventType == 2)
            {
                suddenEventPanel.PopOffOptionChoiceEvent();
            }
            else if (NowEventType == 3)
            {
                suddenEventPanel.PopOffTargetSelectEvent();
            }

            suddenEventPanel.PopUpStartEventScriptPanel();
        }
    }

    // 선택지이벤트 1, 2, 3을 판별 하기 위한 함수 -> 단순실행에서는 타임바패널의 대사를 띄우기 위한것, 선택지에서는 각각의 선택지를 띄우기 위한 스크립트
    public void ReadyTimeBarPanel()
    {
        if (eventScheduleSystem.TodaySuddenEventList[0].SuddenEventTime != 0) // 이벤트 실행시간이 있을때
        {
            int TimeBarScriptTime = eventScheduleSystem.TodaySuddenEventList[0].SuddenEventTime;
            suddenEventPanel.GetSimpleExecutionFillBar.fillAmount = 0;

            suddenEventPanel.PopUpTimeBarPanel();
            TimeBarStartTime = Time.unscaledTime;       // 여기서 타임바의 시작 시간을 받기

            if (nowEventScript.SplitEventScriptTextSelect1.splitText.Count != 0) // 대사가 있을 때
            {
                suddenEventPanel.CheckTimeBarScriptText(nowEventScript, TimeBarScriptTime);
            }
            // while문을 돌면서 시간체크를 한후에 나가야 하나
            // ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
        }
        else // 실행시간 없고, end 시나리오가 있으면? 시나리오 보여 준 후 팝업 보상 창으로 넘어가기
        {
            if (nowEventScript.SplitEventScriptTextFin.splitText.Count != 0)
            {
                ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);

                FinScriptReady = true;
            }
            else                        // 보상 창 띄우기
            {
                PeekOrSetEventReward(true); // 이벤트 보상 내용을 가져온다.

                // 강사해금 - 단순실행 보상으로만 풀리나?
                if (UnLockTeacherIndex != 0)
                {
                    suddenEventPanel.ReadyTeacherUnLockPanel(NowEventRewardInfo);
                }
                else if (UnLockCompanyIndex != 0)
                {
                    suddenEventPanel.ReadyCompanyUnLockPanel(NowEventRewardInfo, NowCompanyList);
                }
                else
                {
                    suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
                }

                suddenEventPanel.PopOffStartEventScriptPanel();

                int removeEventCount = 0;

                for (int i = 0; i < NowEventRewardInfo.Length; i++)
                {
                    if (NowEventRewardInfo[i].RewardAmount != 0 || eventScheduleSystem.TodaySuddenEventList[0].Teacher != 0)
                    {
                        suddenEventPanel.PopUpRewardPanel();

                        break;
                    }
                    else
                    {
                        if (removeEventCount == 0)
                        {
                            isReadyNowEventPopUp = false;

                            removeEventCount++;
                        }
                    }
                }

                if (removeEventCount != 0)
                {
                    eventScheduleSystem.TodaySuddenEventList.RemoveAt(0);
                }

                FinScriptReady = false;
                MiddelScriptReady = false;
            }
        }
    }

    // Time.time이 0 일때, 타임바를 흐르게 하기 위한 함수
    public void UpdateLoadingBar(float prograss, float[] thresholds, string[] texts)
    {
        // 인자로 받은 durationTime 의 시간만큼 로딩바를 흐르게 한다.
        // suddenEventPanel.GetSimpleExecutionFillBar.fillAmount = durationTime;

        suddenEventPanel.GetSimpleExecutionSlideBar.value = prograss;

        if (thresholds.Length == 1)
        {
            if (prograss <= thresholds[0])
            {
                suddenEventPanel.GetSimpleExecutionEventText.text = texts[0];
            }
        }
        else if (thresholds.Length == 2)
        {
            if (prograss <= thresholds[0])
            {
                suddenEventPanel.GetSimpleExecutionEventText.text = texts[0];
            }
            else if (prograss <= thresholds[1])
            {
                suddenEventPanel.GetSimpleExecutionEventText.text = texts[1];
            }
        }
        else if (thresholds.Length == 3)
        {
            if (prograss <= thresholds[0])
            {
                suddenEventPanel.GetSimpleExecutionEventText.text = texts[0];
            }
            else if (prograss <= thresholds[1])
            {
                suddenEventPanel.GetSimpleExecutionEventText.text = texts[1];
            }
            else
            {
                suddenEventPanel.GetSimpleExecutionEventText.text = texts[2];
            }
        }
    }

    // 선택지 이벤트의 대사를 띄우기 위한 준비
    public void ReadyChoiceOptionScriptText()
    {
        int SpeakerIndex = nowEventScript.SplitEventScriptTextStart.speaker.Count;
        string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextStart.speaker[SpeakerIndex - 1].ToString();        // 마지막 대사의 화자
        string nowtempScript = ""; // 스크립트

        // 클릭한 수로 배열 체크 하니까 배열 크기 넘어가서 터짐
        // split 해둔 모든 대사를 하나로 합친 후 UI에 출력

        // for (int i = 0; i < nowEventScript.SplitEventScriptTextStart.splitText.Count; i++)
        // {
        //     nowtempScript += nowEventScript.SplitEventScriptTextStart.splitText[i] + "\n";
        // }

        nowtempScript = nowEventScript.SplitEventScriptTextStart.splitText[SpeakerIndex - 1];

        string[] OptionScript = new string[3];      // 선택지 갯수를 받은 것
        OptionScript[0] = "";
        OptionScript[1] = "";
        OptionScript[2] = "";

        // 포문을 돌면서 알맞은 스크립트를 찾는 함수 필요
        ReadyOptionsChoiceEvent(OptionScript);

        if (OptionEventNeedPay.SelectPay1 == 100)           // 소모재화가 돈 일때
        {
            suddenEventPanel.IfHaveLowMoneyNow(OptionEventNeedPay, 1);
        }

        if (OptionEventNeedPay.SelectPay2 == 100)           // 소모재화가 돈 일때
        {
            suddenEventPanel.IfHaveLowMoneyNow(OptionEventNeedPay, 2);
        }

        if (OptionEventNeedPay.SelectPay3 == 100)           // 소모재화가 돈 일때
        {
            suddenEventPanel.IfHaveLowMoneyNow(OptionEventNeedPay, 3);
        }

        // nowEventUIClickCount = 0;
        suddenEventPanel.ReadyEventRewardText(OptionEventNeedPay);
        suddenEventPanel.ReadyOptionChoicePanelText(nowTempScriptSpeaker, nowtempScript, OptionScript, NowEventRewardInfo, nowTargetStudent);
        suddenEventPanel.PopOffStartEventScriptPanel();
        suddenEventPanel.PopUpOptionChoiceEvent();
    }

    // 선택지 이벤트의 패널에 데이터를 채우기 위한 함수
    public void ReadyOptionsChoiceEvent(string[] OptionScript)
    {
        for (int i = 0; i < AllOriginalJsonData.Instance.ChoiceEventReward.Count; i++)
        {
            // 해당 이벤트의 선택지ID 3개를 리워드DT의 ID 와 비교해서 데이터를 넣어주자
            if (eventScheduleSystem.TodaySuddenEventList[0].SelectRewardIndex1 ==
                AllOriginalJsonData.Instance.ChoiceEventReward[i].SelectEventID)
            {
                OptionScript[0] = AllOriginalJsonData.Instance.ChoiceEventReward[i].SelectScript;

                OptionEventNeedPay.SelectPay1 = AllOriginalJsonData.Instance.ChoiceEventReward[i].SelectPay;
                OptionEventNeedPay.PayAmount1 = AllOriginalJsonData.Instance.ChoiceEventReward[i].PayAmount;
            }

            if (eventScheduleSystem.TodaySuddenEventList[0].SelectRewardIndex2 ==
                AllOriginalJsonData.Instance.ChoiceEventReward[i].SelectEventID)
            {
                OptionScript[1] = AllOriginalJsonData.Instance.ChoiceEventReward[i].SelectScript;

                OptionEventNeedPay.SelectPay2 = AllOriginalJsonData.Instance.ChoiceEventReward[i].SelectPay;
                OptionEventNeedPay.PayAmount2 = AllOriginalJsonData.Instance.ChoiceEventReward[i].PayAmount;
            }

            if (eventScheduleSystem.TodaySuddenEventList[0].SelectRewardIndex3 ==
                AllOriginalJsonData.Instance.ChoiceEventReward[i].SelectEventID)
            {
                OptionScript[2] = AllOriginalJsonData.Instance.ChoiceEventReward[i].SelectScript;

                OptionEventNeedPay.SelectPay3 = AllOriginalJsonData.Instance.ChoiceEventReward[i].SelectPay;
                OptionEventNeedPay.PayAmount3 = AllOriginalJsonData.Instance.ChoiceEventReward[i].PayAmount;
            }
        }
    }

    // 내가 누른 버튼을 구별하기 위한 변수 값넣기, 버튼 클릭 시 화면을 전환
    public void IfClickOptionButton()
    {
        // 소모재화량과 현재 나의 재화를 비교 해야 한다.
        // 소모 재화는 2가지. 
        GameObject nowClickOption = EventSystem.current.currentSelectedGameObject;

        if (nowClickOption.name == suddenEventPanel.GetOption1Button.name) // 선택지 구별             1번 선택지
        {
            SelectedOptionNumber = 1;

            PlayerInfo.Instance.m_MyMoney -= OptionEventNeedPay.PayAmount1;
            MonthlyReporter.Instance.m_NowMonth.ExpensesEventCost -= OptionEventNeedPay.PayAmount1;

            if (nowEventScript.SplitEventScriptTextSelect1.splitText.Count != 0)
            {
                // FinScriptReady = true;
                ReadyScenarioPanel(nowEventScript.SplitEventScriptTextSelect1);
                suddenEventPanel.PopOffOptionChoiceEvent();
            }
            else
            {
                if (nowEventScript.SplitEventScriptTextFin.splitText.Count != 0)
                {
                    FinScriptReady = true;
                    ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
                    suddenEventPanel.PopOffOptionChoiceEvent();
                }
                else
                {
                    PeekOrSetEventReward(true);

                    suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
                    suddenEventPanel.PopOffOptionChoiceEvent();

                    int removeEventCount = 0;

                    for (int i = 0; i < NowEventRewardInfo.Length; i++)
                    {
                        if (NowEventRewardInfo[i].RewardAmount != 0 || eventScheduleSystem.TodaySuddenEventList[0].Teacher != 0)
                        {
                            suddenEventPanel.PopUpRewardPanel();

                            break;
                        }
                        else
                        {
                            if (removeEventCount == 0)
                            {
                                isReadyNowEventPopUp = false;

                                removeEventCount++;
                            }
                        }
                    }

                    if (removeEventCount != 0)
                    {
                        eventScheduleSystem.TodaySuddenEventList.RemoveAt(0);
                    }

                    FinScriptReady = false;
                    MiddelScriptReady = false;
                }
            }
        }
        else if (nowClickOption.name == suddenEventPanel.GetOption2Button.name)                 //     2번 선택지
        {
            SelectedOptionNumber = 2;

            PlayerInfo.Instance.m_MyMoney -= OptionEventNeedPay.PayAmount2;
            MonthlyReporter.Instance.m_NowMonth.ExpensesEventCost -= OptionEventNeedPay.PayAmount2;

            if (nowEventScript.SplitEventScriptTextSelect2.splitText.Count != 0)
            {
                // FinScriptReady = true;
                ReadyScenarioPanel(nowEventScript.SplitEventScriptTextSelect2);
                suddenEventPanel.PopOffOptionChoiceEvent();
            }
            else
            {
                if (nowEventScript.SplitEventScriptTextFin.splitText.Count != 0)
                {
                    FinScriptReady = true;
                    ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
                    suddenEventPanel.PopOffOptionChoiceEvent();
                }
                else
                {
                    PeekOrSetEventReward(true);

                    suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
                    suddenEventPanel.PopOffOptionChoiceEvent();

                    int removeEventCount = 0;

                    for (int i = 0; i < NowEventRewardInfo.Length; i++)
                    {
                        if (NowEventRewardInfo[i].RewardAmount != 0 || eventScheduleSystem.TodaySuddenEventList[0].Teacher != 0)
                        {
                            suddenEventPanel.PopUpRewardPanel();

                            break;
                        }
                        else
                        {
                            if (removeEventCount == 0)
                            {
                                // eventScheduleSystem.TodaySuddenEventList.RemoveAt(0);
                                isReadyNowEventPopUp = false;

                                removeEventCount++;
                            }
                        }
                    }

                    if (removeEventCount != 0)
                    {
                        eventScheduleSystem.TodaySuddenEventList.RemoveAt(0);
                    }

                    FinScriptReady = false;
                    MiddelScriptReady = false;
                }
            }
        }
        else if (nowClickOption.name == suddenEventPanel.GetOption3Button.name)                 //     3번 선택지
        {
            SelectedOptionNumber = 3;

            PlayerInfo.Instance.m_MyMoney -= OptionEventNeedPay.PayAmount3;
            MonthlyReporter.Instance.m_NowMonth.ExpensesEventCost -= OptionEventNeedPay.PayAmount3;


            if (nowEventScript.SplitEventScriptTextSelect3.splitText.Count != 0)
            {
                // FinScriptReady = true;

                ReadyScenarioPanel(nowEventScript.SplitEventScriptTextSelect3);
                suddenEventPanel.PopOffOptionChoiceEvent();
            }
            else
            {
                if (nowEventScript.SplitEventScriptTextFin.splitText.Count != 0)
                {
                    FinScriptReady = true;

                    ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
                    suddenEventPanel.PopOffOptionChoiceEvent();
                }
                else
                {
                    PeekOrSetEventReward(true);

                    suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
                    suddenEventPanel.PopOffOptionChoiceEvent();

                    int removeEventCount = 0;

                    for (int i = 0; i < NowEventRewardInfo.Length; i++)
                    {
                        if (NowEventRewardInfo[i].RewardAmount != 0 || eventScheduleSystem.TodaySuddenEventList[0].Teacher != 0)
                        {
                            suddenEventPanel.PopUpRewardPanel();

                            break;
                        }
                        else
                        {
                            if (removeEventCount == 0)
                            {
                                isReadyNowEventPopUp = false;

                                removeEventCount++;
                            }
                        }
                    }

                    if (removeEventCount != 0)
                    {
                        eventScheduleSystem.TodaySuddenEventList.RemoveAt(0);
                    }

                    FinScriptReady = false;
                    MiddelScriptReady = false;
                }
            }
        }
    }

    // 대상 선택 이벤트의 대사를 띄우기 위한 준비
    public void ReadyTargetSelectScriptText()
    {
        // 패널 띄우기, 해당 이벤트 정보는 보임, 학생정보 안들어가있음
        // 학생 로드 해야함 -> 프리팹 생성 안하고 6칸에 정보만 다르게 뜨도록
        // 학생 클릭 시 해당 학생 정보 뜸

        // 대상선택 이벤트 팝업 시 처음에 정보를 보여주기 위한 학생 목록 세팅
        SetTargetEventStudentPanel();

        // 대상선택 이벤트 패널 띄우기
        suddenEventPanel.PopUpTargetSelectEvent();
        suddenEventPanel.PopOffStartEventScriptPanel();

    }

    // 타겟이벤트가 뜰 때, 데이터 정리해서 띄워줄 함수
    public void SetTargetEventStudentPanel()
    {
        List<Student> tempData = ObjectManager.Instance.m_StudentList;
        int studentCount = ObjectManager.Instance.m_StudentList.Count;

        string tempTerm = eventScheduleSystem.TodaySuddenEventList[0].SuddenEventDuration + "일";

        Image[] tempImg = new Image[6];
        string[] tempName = { };

        for (int i = 0; i < studentCount; i++)
        {
            if (tempData[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
            {
                NowDepartmentStudentList tempStudent = new NowDepartmentStudentList();

                tempStudent.studentName = tempData[i].m_StudentStat.m_StudentName;
                tempStudent.studentImg = tempData[i].StudentProfileImg;

                EachDepartmentStduentList.Add(tempStudent);
            }
        }

        suddenEventPanel.TargetEventStudentListSetData(EachDepartmentStduentList);
        suddenEventPanel.SetTargetEventInfo(NowEventRewardInfo, tempTerm);
        suddenEventPanel.PopUpTargetEventPanelSetData();
    }

    // 각 학과버튼 클릭시 학과에 맞는 학생 정보가 뜨도록 해야함
    public void IfClickEachStduentDepartmentIndexButton()
    {
        // 내가 클릭한 학과의 이름을 알고, 그 학과의 학생을 찾아서 데이터를 보여주도록 하자
        GameObject nowClickButton = EventSystem.current.currentSelectedGameObject;
        List<Student> tempData = ObjectManager.Instance.m_StudentList;
        int studentCount = ObjectManager.Instance.m_StudentList.Count;

        for (int i = 0; i < suddenEventPanel.GetCheckedStudentImgList.Count; i++)
        {
            suddenEventPanel.GetCheckedStudentImgList[i].gameObject.SetActive(false);
        }

        switch (nowClickButton.name)
        {
            case "GameDesignerIndex":
                {
                    EachDepartmentStduentList.Clear();

                    for (int i = 0; i < studentCount; i++)
                    {
                        if (tempData[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                        {
                            NowDepartmentStudentList tempStudent = new NowDepartmentStudentList();
                            tempStudent.studentName = tempData[i].m_StudentStat.m_StudentName;
                            tempStudent.studentImg = tempData[i].StudentProfileImg;

                            EachDepartmentStduentList.Add(tempStudent);
                        }
                    }
                    nowTargetStudent = null;

                    suddenEventPanel.TargetEventStudentListSetData(EachDepartmentStduentList);
                    suddenEventPanel.PopUpTargetEventPanelSetData(nowTargetStudent);
                }
                break;
            case "ArtIndex":
                {
                    EachDepartmentStduentList.Clear();

                    int EachDepartementStudentCount = 0;

                    Image[] tempImg = null;
                    string[] tempName = { };

                    for (int i = 0; i < studentCount; i++)
                    {
                        if (tempData[i].m_StudentStat.m_StudentType == StudentType.Art)
                        {
                            NowDepartmentStudentList tempStudent = new NowDepartmentStudentList();
                            tempStudent.studentName = tempData[i].m_StudentStat.m_StudentName;
                            tempStudent.studentImg = tempData[i].StudentProfileImg;

                            EachDepartmentStduentList.Add(tempStudent);
                        }
                    }
                    nowTargetStudent = null;

                    suddenEventPanel.TargetEventStudentListSetData(EachDepartmentStduentList);
                    suddenEventPanel.PopUpTargetEventPanelSetData(nowTargetStudent);
                }
                break;
            case "ProgrammingIndex":
                {
                    EachDepartmentStduentList.Clear();

                    Image[] tempImg = null;
                    string[] tempName = { };

                    for (int i = 0; i < studentCount; i++)
                    {
                        if (tempData[i].m_StudentStat.m_StudentType == StudentType.Programming)
                        {
                            NowDepartmentStudentList tempStudent = new NowDepartmentStudentList();
                            tempStudent.studentName = tempData[i].m_StudentStat.m_StudentName;
                            tempStudent.studentImg = tempData[i].StudentProfileImg;

                            EachDepartmentStduentList.Add(tempStudent);
                        }
                    }
                    nowTargetStudent = null;

                    suddenEventPanel.TargetEventStudentListSetData(EachDepartmentStduentList);
                    suddenEventPanel.PopUpTargetEventPanelSetData(nowTargetStudent);
                }
                break;
        }
    }

    // 각 학생의 이미지를 클릭 시 -> 학생 데이터 띄워주기
    public void IfClickEachStudent()
    {
        GameObject nowClickButton = EventSystem.current.currentSelectedGameObject;
        List<Student> tempData = ObjectManager.Instance.m_StudentList;
        int studentCount = ObjectManager.Instance.m_StudentList.Count;

        for (int i = 0; i < tempData.Count; i++)
        {
            if (nowClickButton.name == tempData[i].m_StudentStat.m_StudentName)
            {
                nowTargetStudent = tempData[i];

                suddenEventPanel.PopUpTargetEventPanelSetData(nowTargetStudent);
            }
        }
    }

    // 대상선택 이벤트 선택완료 버튼 누르면? -> 시나리오패널 대사 확인, 있으면 시나리오 패널 띄우기 / 없으면 패널 끄고 시간 지나기
    public void IfClickTargetEventSetOKButton()
    {
        if (nowTargetStudent != null)
        {
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (nowTargetStudent.m_StudentStat.m_StudentName == ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentName)
                {
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_IsActiving = false;
                    // 이벤트에 선택된 학생의 상태를 바꿔준다
                    NowTime eventEndTime = new NowTime();
                    eventEndTime.NowYear = GameTime.Instance.FlowTime.NowYear;
                    eventEndTime.NowMonth = GameTime.Instance.FlowTime.NowMonth;
                    eventEndTime.NowWeek = GameTime.Instance.FlowTime.NowWeek;
                    eventEndTime.NowDay = GameTime.Instance.FlowTime.NowDay;

                    int eventDay = 5;       // 이벤트 (일) 더해주기
                    eventEndTime.NowDay += eventDay;
                    if (eventEndTime.NowDay / 5 >= 1)
                    {
                        eventEndTime.NowWeek += eventEndTime.NowDay / 5;
                        eventEndTime.NowDay %= 5;
                    }


                    int EndTime = ((eventEndTime.NowYear - 1) * 12 * 4 * 5) +
                        ((eventEndTime.NowMonth - 1) * 4 * 5) + ((eventEndTime.NowWeek - 1) * 5) + eventEndTime.NowDay;

                    StartCoroutine(CheckTargetEventDispatchedStudent(EndTime));     // startCoroutine을 하면 호출 될 때 한번 불린다.
                }
            }

            suddenEventPanel.PopOffTargetSelectEvent();

            // 미들 텍스트 출력
            MiddelScriptReady = true;
            ReadyScenarioPanel(nowEventScript.SplitEventScriptTextMiddle);
        }
    }

    // 코루틴? 업데이트? 코루틴 돌면서
    // 이벤트 진행시간을 체크, 시간 넘어서면 함수 발동
    IEnumerator CheckTargetEventDispatchedStudent(int eventEndDay)
    {
        // 함수의 조건을 비교해서, 조건이 맞아지면  yeild return 아래의 줄이 실행된다
        yield return new WaitUntil(/*조건*/() =>
        (((GameTime.Instance.FlowTime.NowYear - 1) * 12 * 4 * 5) +
                    ((GameTime.Instance.FlowTime.NowMonth - 1) * 4 * 5) + ((GameTime.Instance.FlowTime.NowWeek - 1) * 5) + GameTime.Instance.FlowTime.NowDay) ==
        eventEndDay);

        // 팝업창 뜰 수 있을 때 시나리오 패널 준비
        if (GameTime.Instance.IsGameMode == true && isReadyNowEventPopUp == true)
        {
            FinScriptReady = true;
            ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
        }
        // fin 스크립트가 있는 지 체크, 있으면 띄워주기
        // 학생 상태 변화


    }


    public void IfIClickGetRewardButton()
    {
        isReadyNowEventPopUp = false;
        PeekOrSetEventReward(false); // 인자 값이 false 일 때 -> 보상 받고 실제 값 변경
        suddenEventPanel.PopOffRewardPanel();
        SelectedOptionNumber = 0;

        eventScheduleSystem.TodaySuddenEventList.RemoveAt(0);
        nowTargetStudent = null;
        EachDepartmentStduentList.Clear();
    }

    // 보상 정보 구조체에 저장하는 부분을 모은 함수
    private void RewardInfoSetup(int index, int rewardID, int rewardAmount, string rewardName/*, string photoID = null, string companyName = null*/)
    {
        NowEventRewardInfo[index].RewardID = rewardID;
        NowEventRewardInfo[index].RewardAmount = rewardAmount;
        NowEventRewardInfo[index].RewardName = rewardName;
    }

    // true 일 때 -> UI 에 쓸 텍스트, 이미지, 변경수치 리턴 (실제 값이 변경되지는 않음)
    // false 일때 -> 이벤트에 의한 값을 변경한다.
    public void PeekOrSetEventReward(bool peek)
    {
        // TODO :: rewordText를 사용하기 전에 초기화를 해줘야 한다.
        // 보상 종류 결과 텍스트 출력을 위한 리턴
        string[] rewardText = new string[2];
        int[] rewardID = new int[2]; // 어떤 보상인지
        int[] amount = new int[2]; // 얼마나 증가할지

        // 결과를 저장할 구조체 배열 초기화
        for (int i = 0; i < NowEventRewardInfo.Length; i++)
        {
            NowEventRewardInfo[i].RewardID = 0;
            NowEventRewardInfo[i].RewardAmount = 0;
            NowEventRewardInfo[i].RewardName = "안녕";
        }

        var nowEvent = eventScheduleSystem.TodaySuddenEventList[0];

        // 단순 실행 보상 관련 (100 ~ 199) 함수당 하나의 보상씩만 담당
        if (nowEvent.RewardIndex3 != 0) // 보상이 3개일 때
        {
            ApplyEventReword(nowEvent.RewardIndex3, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
                RewardInfoSetup(2, rewardID[0], amount[0], rewardText[0]);

            ApplyEventReword(nowEvent.RewardIndex2, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
                RewardInfoSetup(1, rewardID[0], amount[0], rewardText[0]);

            ApplyEventReword(nowEvent.RewardIndex1, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
                RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
        }
        else if (nowEvent.RewardIndex2 != 0) // 보상이 2개일 때
        {
            ApplyEventReword(nowEvent.RewardIndex2, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
                RewardInfoSetup(1, rewardID[0], amount[0], rewardText[0]);

            ApplyEventReword(nowEvent.RewardIndex1, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
                RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
        }
        else if (nowEvent.RewardIndex1 != 0) // 보상이 1개일 때
        {
            ApplyEventReword(nowEvent.RewardIndex1, ref rewardText, ref rewardID, ref amount, peek);
            RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
        }

        // 돌발 선택지에 대한 보상 관련 (200 ~ 300)
        else if (nowEvent.RewardIndex1 == 0 && SelectedOptionNumber == 0) // 돌발 단순 이벤트가 아니고, 보상 정보를 받아오고 싶을때
        {
            ApplyEventReword(nowEvent.SelectRewardIndex1, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
            {
                RewardInfoSetup(1, rewardID[1], amount[1], rewardText[1]);
                RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
            }

            ApplyEventReword(nowEvent.SelectRewardIndex2, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
            {
                RewardInfoSetup(3, rewardID[1], amount[1], rewardText[1]);
                RewardInfoSetup(2, rewardID[0], amount[0], rewardText[0]);
            }

            ApplyEventReword(nowEvent.SelectRewardIndex3, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
            {
                RewardInfoSetup(5, rewardID[1], amount[1], rewardText[1]);
                RewardInfoSetup(4, rewardID[0], amount[0], rewardText[0]);
            }
        }
        else if (nowEvent.RewardIndex1 == 0 && SelectedOptionNumber == 1) // 돌발 단순 이벤트가 아니고, 첫번째 선택지를 누르면
        {
            ApplyEventReword(nowEvent.SelectRewardIndex1, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
            {
                RewardInfoSetup(1, rewardID[1], amount[1], rewardText[1]);
                RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
            }
        }
        else if (nowEvent.RewardIndex1 == 0 && SelectedOptionNumber == 2) // 두번째 선택지를 누르면
        {
            ApplyEventReword(nowEvent.SelectRewardIndex2, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
            {
                RewardInfoSetup(1, rewardID[1], amount[1], rewardText[1]);
                RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
            }
        }
        else if (nowEvent.RewardIndex1 == 0 && SelectedOptionNumber == 3) // 세번째 선택지를 누르면
        {
            ApplyEventReword(nowEvent.SelectRewardIndex3, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
            {
                RewardInfoSetup(1, rewardID[1], amount[1], rewardText[1]);
                RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
            }
        }
    }

    private void ApplyEventReword(int rewardID, ref string[] rewardTextArr, ref int[] rewardIndex, ref int[] amount,
        bool check)
    {
        RewardTaker[] rewardTaker = { RewardTaker.None, RewardTaker.None }; // 누가 받는지

        // 돌발_단순실행,대상선택보상 (100 ~ 199번)
        if (rewardID < 199)
        {
            foreach (var eventReward in AllOriginalJsonData.Instance.SimpleEventReward)
            {
                if (eventReward.EventID == rewardID)
                {
                    rewardTaker[0] = (RewardTaker)Enum.Parse(typeof(RewardTaker), eventReward.RewardTaker); // 보상 수령자
                    rewardIndex[0] = eventReward.Reward; // 보상 인덱스
                    amount[0] = eventReward.Amount; // 보상 양
                }
            }
        }

        // 돌발_선택지 보상 (200 ~ 299번)
        else if (rewardID < 299)
        {
            foreach (var eventReward in AllOriginalJsonData.Instance.ChoiceEventReward)
            {
                if (eventReward.SelectEventID == rewardID)
                {
                    rewardTaker[0] = (RewardTaker)Enum.Parse(typeof(RewardTaker), eventReward.RewardTaker); // 보상1 수령자
                    rewardIndex[0] = eventReward.Reward1; // 보상1 인덱스
                    amount[0] = eventReward.Amount1; // 보상1 양
                    rewardTaker[1] = (RewardTaker)Enum.Parse(typeof(RewardTaker), eventReward.RewardTaker); // 보상2 수령자
                    rewardIndex[1] = eventReward.Reward2; // 보상2 인덱스
                    amount[1] = eventReward.Amount2; // 보상2 양

                    break;
                }
            }
        }

        for (int i = 0; i <= 1; i++)
        {
            switch (rewardTaker[i])
            {
                case RewardTaker.None: // 초기화값 에러방지
                    rewardTextArr[i] = "이벤트가 없습니다...";
                    if (rewardID == 600) // 교수 해금을 합니다
                    {
                        rewardTextArr[i] = ApplyUnLockTeacherReward(check);
                    }
                    else if (rewardID == 601)
                    {
                        rewardTextArr[i] = ApplyUnLockCompanyReward(check);
                    }
                    break;

                case RewardTaker.academy: // 아카데미 자체

                    //else
                    {
                        rewardTextArr[i] = ApplyAcademyEventReword(rewardIndex[i], amount[i], check);
                    }
                    break;

                case RewardTaker.allprof: // 보유한 모든 강사
                    foreach (var professor in ObjectManager.Instance.nowProfessor.GameManagerProfessor)
                    {
                        rewardTextArr[i] = ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                    }
                    foreach (var professor in ObjectManager.Instance.nowProfessor.ArtProfessor)
                    {
                        rewardTextArr[i] = ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                    }
                    foreach (var professor in ObjectManager.Instance.nowProfessor.ProgrammingProfessor)
                    {
                        rewardTextArr[i] = ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                    }

                    break;

                case RewardTaker.allstu: // 보유한 모든 학생
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        // 모든 학생 능력치 증가시키기...
                        rewardTextArr[i] =
                            ApplyStudentEventReword(student.m_StudentStat, rewardIndex[i], amount[i], check);
                    }

                    break;


                case RewardTaker.design: // 보유한 기획파트 학생 모두
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        if (student.m_StudentStat.m_StudentType == StudentType.GameDesigner)
                        {
                            rewardTextArr[i] =
                                ApplyStudentEventReword(student.m_StudentStat, rewardIndex[i], amount[i], check);
                        }
                    }

                    break;

                case RewardTaker.art: // 보유한 아트파트 학생 모두
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        if (student.m_StudentStat.m_StudentType == StudentType.Art)
                        {
                            rewardTextArr[i] =
                                ApplyStudentEventReword(student.m_StudentStat, rewardIndex[i], amount[i], check);
                        }
                    }

                    break;

                case RewardTaker.pro: // 보유한 플밍파트 학생 모두
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        if (student.m_StudentStat.m_StudentType == StudentType.Programming)
                        {
                            rewardTextArr[i] =
                                ApplyStudentEventReword(student.m_StudentStat, rewardIndex[i], amount[i], check);
                        }
                    }

                    break;

                // TODO : 
                case RewardTaker.alone: // 이벤트 실행자 단독
                    if (check == true)
                    {
                        rewardTextArr[i] =
                       ApplyStudentEventReword(null, rewardIndex[i], amount[i], check);
                    }
                    else
                    {
                        foreach (Student student in ObjectManager.Instance.m_StudentList)
                        {
                            if (nowTargetStudent.m_StudentStat.m_StudentName == student.m_StudentStat.m_StudentName)
                            {
                                rewardTextArr[i] =
                                ApplyStudentEventReword(student.m_StudentStat, rewardIndex[i], amount[i], check);
                            }
                        }
                    }
                    break;

                case RewardTaker.teacherAlone: // 이벤트 실행자 단독
                    foreach (var professor in ObjectManager.Instance.nowProfessor.GameManagerProfessor)
                    {
                        if (nowTargetTeacher.m_ProfessorNameValue == professor.m_ProfessorNameValue)
                        {
                            rewardTextArr[i] =
                            ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                        }
                    }
                    foreach (var professor in ObjectManager.Instance.nowProfessor.ArtProfessor)
                    {
                        if (nowTargetTeacher.m_ProfessorNameValue == professor.m_ProfessorNameValue)
                        {
                            rewardTextArr[i] =
                            ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                        }
                    }
                    foreach (var professor in ObjectManager.Instance.nowProfessor.ProgrammingProfessor)
                    {
                        if (nowTargetTeacher.m_ProfessorNameValue == professor.m_ProfessorNameValue)
                        {
                            rewardTextArr[i] =
                            ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                        }
                    }
                    break;
            }
        }
    }

    // 이벤트로 아카데미의 정보를 병경합니다.
    private string ApplyAcademyEventReword(int rewardIndex, int amount, bool check)
    {
        switch (rewardIndex)
        {
            case 0:
                return "보상 없음";

            case 100: //	1차재화
                if (!check)
                {
                    PlayerInfo.Instance.m_MyMoney += amount;
                    if (amount < 0)
                    {
                        MonthlyReporter.Instance.m_NowMonth.ExpensesEventResult -= amount;      // 지출
                    }
                    else
                    {
                        MonthlyReporter.Instance.m_NowMonth.IncomeEventResult += amount;        // 수입
                    }
                }
                return "골드";

            case 101: //	2차재화
                if (!check)
                {
                    PlayerInfo.Instance.m_SpecialPoint += amount;
                }
                return "루비";

            case 300: //	운영점수		랭크에 반영
                if (!check)
                {
                    PlayerInfo.Instance.m_Management += amount;
                    MonthlyReporter.Instance.m_NowMonth.GoodsScore += amount;
                }
                return "운영점수";

            case 301: //	유명도 점수		랭크에 반영
                if (!check)
                {
                    PlayerInfo.Instance.m_Awareness += amount;
                    MonthlyReporter.Instance.m_NowMonth.FamousScore += amount;
                }
                return "유명점수";

            case 302: //	활동 점수		랭크에 반영
                if (!check)
                {
                    PlayerInfo.Instance.m_Activity += amount;
                    MonthlyReporter.Instance.m_NowMonth.ActivityScore += amount;
                }
                return "활동점수";

            // 보상 추가
            case 303: //	인재양성
                if (!check)
                {
                    PlayerInfo.Instance.m_TalentDevelopment += amount;
                    MonthlyReporter.Instance.m_NowMonth.TalentDevelopmentScore += amount;
                }

                return "인재양성";

            default:
                return "아카데미 잘못된 보상 인덱스";
        }
    }

    // 이벤트로 특정 교사의 능력치를 변경합니다.
    private string ApplyProfEventReword(ProfessorStat professorStat, int rewardIndex, int amount, bool check)
    {
        switch (rewardIndex)
        {
            case 0:
                return "보상 없음";

            case 200: //	체력
                if (!check)
                    professorStat.professorHealth += amount;
                return "교사-체력";

            case 201: //	열정
                if (!check)
                    professorStat.professorPassion += amount;
                return "교사-열정";

            case 400: //	경험치
                if (!check)
                    professorStat.m_ProfessorExperience += amount;
                return "교사-경험치";

            case 401: //	월급상승
                if (!check)
                    professorStat.m_ProfessorPay += amount;
                return "교사-월급상승";

            default:
                return "교사-잘못된 보상 인덱스";
        }
    }


    // 이벤트로 특정 학생의 능력치를 변경합니다.
    private string ApplyStudentEventReword(StudentStat studentStat, int rewardIndex, int amount, bool check)
    {
        switch (rewardIndex)
        {
            case 0:
                return "보상 없음";

            case 200: //	체력
                if (!check)
                    studentStat.m_Health += amount;
                return "학생-체력";

            case 201: //	열정
                if (!check)
                    studentStat.m_Passion += amount;
                return "학생-열정";

            case 500: //	감각
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Sense] += amount;
                return "학생-감각";

            case 501: //	집중
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += amount;
                return "학생-집중";

            case 502: //	재치
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Wit] += amount;
                return "학생-재치";

            case 503: //	기술
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Technique] += amount;
                return "학생-기술";

            case 504: //	통찰
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Insight] += amount;
                return "학생-통찰";

            case 505: //	액션
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Action] += amount;
                return "학생-액션";

            case 506: //	시뮬레이션
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Simulation] += amount;
                return "학생-시뮬레이션";

            case 507: //	어드벤쳐
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Adventure] += amount;
                return "학생-어드벤처";

            case 508: //	슈팅
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Shooting] += amount;
                return "학생-슈팅";

            case 509: //	RPG
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.RPG] += amount;
                return "학생-RPG";

            case 510: //	퍼즐
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Puzzle] += amount;
                return "학생-퍼즐";

            case 511: //	리듬
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Rhythm] += amount;
                return "학생-리듬";

            case 512: //	스포츠
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Sports] += amount;
                return "학생-스포츠";

            default:
                return "학생-잘못된 보상 인덱스";
        }
    }

    // 강사, 회사 해금 보상얻기 (index 600) 
    private string ApplyUnLockTeacherReward(bool check)
    {

        ProfessorStat tempTeacherInfo = new ProfessorStat(); // 해금할 강사의 정보

        foreach (var eventReward in AllOriginalJsonData.Instance.OriginalSuddenEventDataList) // 전체 이벤트 탐색
        {
            if (eventReward.SuddenEventID == eventScheduleSystem.TodaySuddenEventList[0].SuddenEventID) // 지금 이벤트와 전체 이벤트 비교
            {
                int teacherId = eventReward.Teacher;

                // TODO : 강사를 어떻게 해야 할 지 더 생각해보기
                //List<ProfessorStat> tempData = new List<ProfessorStat>();// 모든 강사 정보 모음
                //tempData.Clear();

                for (int j = 0; j < m_SelectProfessor.professorData.Count; j++)
                {
                    if (m_SelectProfessor.professorData.ElementAt(j).Value.professorID == teacherId) // 모든 강사 정보에서 해금할 강사를 찾는다.
                    {
                        tempTeacherInfo = m_SelectProfessor.professorData.ElementAt(j).Value;
                    }
                }
            }
        }

        if (!check)     // 실제로 해금 해 줄 코드
        {
            // 강사 이름
            tempTeacherInfo.m_IsUnLockProfessor = true;

            EventRewardUnLockTeacher(tempTeacherInfo);

            PlayerInfo.Instance.UnLockTeacherCount++;       // 강사 해금 몇개 됬는지 카운트
        }

        return tempTeacherInfo.m_ProfessorNameValue;        // 강사 이름
    }

    private string ApplyUnLockCompanyReward(bool check) //    (index 601) 
    {
        int count = 0;

        foreach (var eventReward in AllOriginalJsonData.Instance.OriginalSuddenEventDataList) // 전체 이벤트 탐색
        {
            if (eventReward.SuddenEventID == eventScheduleSystem.TodaySuddenEventList[0].SuddenEventID) // 지금 이벤트와 전체 이벤트 비교
            {
                int CompanyId = eventReward.Company;

                for (int i = 0; i < NowCompanyList.m_CompanyList.Count; i++)
                {
                    Company temp = NowCompanyList.m_CompanyList[i];
                    if (temp.CompanyID == CompanyId)
                    {
                        count = i;
                    }

                }


            }
        }

        if (!check)
        {

            NowCompanyList.m_CompanyList[count].IsUnLockCompany = true;
        }

        return NowCompanyList.m_CompanyList[count].CompanyName;
    }

    public void EventRewardUnLockTeacher(ProfessorStat temp)
    {
        if (temp.m_ProfessorType == StudentType.GameDesigner)
        {
            if (!ObjectManager.Instance.nowProfessor.GameManagerProfessor.Contains(temp))
            {
                ObjectManager.Instance.nowProfessor.GameManagerProfessor.Add(temp);
            }
        }
        else if (temp.m_ProfessorType == StudentType.Art)
        {
            if (!ObjectManager.Instance.nowProfessor.ArtProfessor.Contains(temp))
            {
                ObjectManager.Instance.nowProfessor.ArtProfessor.Add(temp);
            }
        }
        else if (temp.m_ProfessorType == StudentType.Programming)
        {
            if (!ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Contains(temp))
            {
                ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Add(temp);
            }
        }
    }
}