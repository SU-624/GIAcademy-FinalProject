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
    None, // �ʱ�ȭ�� ��������
    academy, // ��ī���� ��ü
    allstu, // ������ ��� �л�
    allprof, // ������ ��� ����
    design, // ������ ��ȹ��Ʈ �л� ���
    art, // ������ ��Ʈ��Ʈ �л� ���
    pro, // ������ �ù���Ʈ �л� ���
    alone,   // �л� �̺�Ʈ ������ �ܵ�
    teacherAlone    // ���� �̺�Ʈ ������ �ܵ�
}

public struct EachScript
{
    public List<ESpeakerImgIndex> speaker;
    public List<string> splitText;
}

// ȭ�ڰ� �� �ؽ�Ʈ �� �Ѹ� �϶� 
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

// �̺�Ʈ ���� ���� ������ ��� ����ü
public struct RewardInfo
{
    public int RewardID;                // ���� ���̵��ε���
    public int RewardAmount;            // ���� ��
    public string RewardName;           // ���� �̸� ���
    public string RewardTakerName;      // ����޴� ���
    // TODO : ���� ȸ�� ���� �߰�

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
    public string ChangedName;
    public StudentType type;
}

/// <summary>
/// 2023. 04. 27 Mang
/// 
/// �̺�Ʈ ���� ������ ����ϴ� Ŭ����.
/// 
/// </summary>
public class EventScriptManager : MonoBehaviour
{
    [SerializeField] private SuddenEventPanel suddenEventPanel;
    [SerializeField] private GameObject studentInfo;        // Ÿ�� ������ �ֱ� ����..! �̰� �����ؾ��ϳ�

    [SerializeField] private InJaeRecommend NowCompanyList;

    IEnumerator SuddenEventCheckCoroutine;

    // ID, ������ �ؽ�Ʈ ���� ����ü��ųʸ�
    private Dictionary<int, SplitScript> SplitEventTextDic;
    [SerializeField] private EventScheduleSystem eventScheduleSystem;

    [SerializeField] private ProfessorController m_SelectProfessor;     // ��� ���縦 ���鼭 �ر��� ���縦 ã�� ���� ����Ʈ    [Space(5)]
    [Space(10)]
    [SerializeField] private PopOffUI tempPopOffAllInstructorInfoPanel;
    [SerializeField] private PopOffUI tempPopOffAllStudentInfoPanel;
    [SerializeField] private PopOffUI tempPopOffInstructorInfoPanel;
    [SerializeField] private PopOffUI tempPopOffStudentInfoPanel;
    [Space(5)]
    [SerializeField] private AllInstructorPanel tempAllInstructorInfoPanel;
    [SerializeField] private AllStudentInfoPanel tempAllStudentInfoPanel;
    [SerializeField] private InstructorInfoPanel tempInstructorInfoPanel;
    [SerializeField] private CharacterInfoPanel tempStudentInfoPanel;

    private int nowEventUIClickCount = 0;
    private bool isReadyNowEventPopUp = false;
    // �ӽ� ������
    private bool TempIsReadyNowEventPopUp = false;

    public delegate void dChangeReadyNowEventPopUp(bool value);
    dChangeReadyNowEventPopUp readyPopUpNowEvent;

    public delegate void dReadyEventScript();
    public static dReadyEventScript readyEventScipt;

    private bool StartScriptReady = false;
    private bool MiddelScriptReady = false;
    private bool FinScriptReady = false;

    private int NowEventType = 0;
    private SplitScript nowEventScript;
    private string tempNowSpeakerText;

    int SelectedOptionNumber = 0; // ������ ���󿡼� ���°�� �����ߴ����� �����ϴ� ������ �����ؾ���.

    RewardInfo[] NowEventRewardInfo = new RewardInfo[6]; // �̺�Ʈ �������� ���� ���� ���� ����
    OptionEventPay OptionEventNeedPay;      // ���� �̺�Ʈ�� �����̺�Ʈ/��¥�����̺�Ʈ �϶�? ��ȣ��ȭ�� �����ϱ� ���� ����
    int NowEvnetID = 0;             // ���� �������̺�Ʈ�� ���̵� �����ϱ� ���� ����
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

    Student nowTargetStudent = new Student();           // ����� �̺�Ʈ �� ������ �л��� ������ ����
    ProfessorStat nowTargetTeacher = new ProfessorStat();

    List<NowDepartmentStudentList> EachDepartmentStduentList = new List<NowDepartmentStudentList>();

    string tempStr;                  // ��ũ��Ʈ ��翡 �п����� �÷��̾ ������ �̸����� �ٲ��ֱ� ���� �ӽú���

    Color SelectedDarkColor;

    public Dictionary<int, SplitScript> MySplitEventTextDic
    {
        get { return SplitEventTextDic; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SplitEventTextDic == null)
        {
            SplitEventTextDic = new Dictionary<int, SplitScript>();
        }

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

        // EventTextScriptSplit();

        readyEventScipt += CheckAcademyNameSet;
        readyEventScipt();

        readyPopUpNowEvent += ChangeReadyNowEventPopUp;
        readyPopUpNowEvent(TempIsReadyNowEventPopUp);

        SelectedDarkColor = new Color(0.55f, 0.55f, 0.55f, 1f);
    }

    public void CheckAcademyNameSet()
    {
        if (PlayerInfo.Instance.AcademyName != null)
        {
            EventTextScriptSplit();
        }
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
        if (PlayerInfo.Instance.isRankButtonOn == false)
        {
            FirstCheckReadySuddenEventList();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("���� Time.unscaledTime : " + Time.unscaledTime);
            Debug.Log("���� Time.time : " + Time.time);
            Debug.Log("���� Time.realtimeSinceStartup : " + Time.realtimeSinceStartup);
        }
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("���� Time.unscaledTime : " + Time.unscaledTime);
        //     Debug.Log("���� Time.time : " + Time.time);
        //     Debug.Log("���� Time.realtimeSinceStartup : " + Time.realtimeSinceStartup);
        // }

        // if (suddenEventPanel.GetSimpleEventTimeBarPanel.activeSelf == true)     // �̺�Ʈ�˾� â�� ������ ���� �ð� ����.
        // {
        //     TimeBarStartTime = Time.unscaledTime;
        // 
        //     UpdateLoadingBar(TimeBarDuration);
        // }

        if (suddenEventPanel.GetSimpleEventTimeBarPanel.activeSelf == true)
        {
            // prograss �� ���� ��ġ. �̰��� ������ �ϸ� �Ƿ���
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

            if (prograss >= 1f)          // �ε��ٰ� 100�ۼ�Ʈ �� á���� üũ�ϱ� ���� ��. �� á���� ���� �� ���� �ȵ����� �Ǵϱ� ��� ���� ���⼱
            {
                Debug.Log("�ε��� ���� ��");
                FinScriptReady = true;
                suddenEventPanel.PopOffTimeBarPanel();
                ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
            }
        }

    }

    // // ��ü������ ������ ������ �־��ִ� ��
    public void EventTextScriptSplit()
    {
        if (SplitEventTextDic == null)
        {
            Start();
            SplitEventTextDic.Clear();
        }
        else
        {
            SplitEventTextDic.Clear();
        }

        if (AllOriginalJsonData.Instance.EventScriptListData != null)
        {
            // ���� ������ ��ü ���鼭 �ؽ�Ʈ�� ������ ���� �� �� ����
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
            }
        }
        else
        {
            Debug.Log("���̽� �����Ͱ� �ε���� �ʾҵ�");
        }
    }

    // �� �ؽ�Ʈ ����� "/" �� "#" �� ������ ���
    public EachScript SplitSpeaker(string tempScriptText)
    {
        string[] temptext;      // ���޹��� ��ũ��Ʈ�� �����ؼ� ����ֱ� ���� �迭

        EachScript tempScript = new EachScript();
        tempScript.splitText = new List<string>();
        tempScript.speaker = new List<ESpeakerImgIndex>();

        if (tempScriptText != "")
        {
            if (tempScriptText.Contains("��������") == true)        // �������� -> �÷��̾ ���� ��ī���� �̸����� ����
            {
                string tempNameChangestr = tempScriptText.Replace("��������", PlayerInfo.Instance.AcademyName);

                temptext = tempNameChangestr.Split("/");        // ������ �ؽ�Ʈ ������ ���� ����
            }
            else
            {
                temptext = tempScriptText.Split("/");        // ������ �ؽ�Ʈ ������ ���� ����
            }

            for (int i = 0; i < temptext.Length; i++)
            {
                tempScript.speaker.Add(ESpeakerImgIndex.None);

                string temp = temptext[i];
                int tempIndex = temp.IndexOf("#");

                // ���⼭ �� ������ ��翡 ���� �� ���忡 GI���� ��� ���� �� �ִ��� üũ�Ѵ�
                //if(temp.Contains("GI����") == true)
                //{
                //    string tempChangeStr = temptext.
                //}

                if (tempIndex >= 0)         // speaker �� ���� ��
                {
                    tempScript.speaker[i] = (ESpeakerImgIndex)ESpeakerImgIndex.Parse(typeof(ESpeakerImgIndex), temp.Substring(0, tempIndex));
                    // tempScript.speaker.Add((Speaker)Speaker.Parse(typeof(Speaker), tempText.Substring(0, tempIndex)));  // string ȭ�ڸ� enum���� �ٲ��ֱ�
                    tempScript.splitText.Add(temp.Substring(tempIndex + 1));
                }
                else
                {
                    if ((i - 1) >= 0)
                    {
                        if (tempScript.speaker[i - 1] != ESpeakerImgIndex.None)              // ���� ���� speaker ����. ���� speaker�� ����Ұ�
                        {
                            tempScript.speaker[i] = tempScript.speaker[i - 1];
                        }
                    }

                    tempScript.splitText.Add(temp);
                }
                // ���⼭ �� ������ ��翡 ���� �� ���忡 GI���� ��� ���� �� �ִ��� üũ�Ѵ�
            }
        }
        return tempScript;
    }

    // ������ ���� �Լ��� ���� ������
    // ���� popUP �Լ� ��� �ؼ�...

    // update������ ����� �̺�Ʈ ����Ʈ�� ����ִ��� ��� üũ -> �� ó���� �� �ó����� �����̤�
    public void FirstCheckReadySuddenEventList()
    {
        // ��ũ��Ʈ ���̵�, �̺�Ʈ ���̵�, ���� ���̵�
        // �̺�Ʈ ����Ʈ�� ��� üũ -> �̺�Ʈ�� �����?
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

                // �켱�� ������ ���� -> �� ���ð������ 
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

                PeekOrSetEventReward(true); // ���� ���� true �� �� -> UI �� �� �ؽ�Ʈ, �̹���, �����ġ ����

                StartScriptReady = true;
                // NowEventReward = SplitEventTextDic[eventScheduleSystem.ReadySuddenEventList[0].RewardIndex1];

                if (eventScheduleSystem.TodaySuddenEventList[0].EventDelayTime != 0)
                {
                    // ReadyScenarioPanel(nowEventScript.SplitEventScriptTextStart);

                    Invoke("tempDelay", eventScheduleSystem.TodaySuddenEventList[0].EventDelayTime);
                }
                else
                {
                    ReadyScenarioPanel(nowEventScript.SplitEventScriptTextStart);
                }

                // ���� -> ������ ���� ��ư�� ���� �� isReadyNowEventPopUp = false ���ְ�(�̰� �˿��� �ɶ����� ���ֱ�) ����Ʈ�� ����ֱ�
                // eventScheduleSystem.ReadySuddenEventList.RemoveAt(0);
            }
        }
    }

    public void tempDelay()
    {
        ReadyScenarioPanel(nowEventScript.SplitEventScriptTextStart);
    }

    public void ReadyScenarioPanel(EachScript nowScript)
    {
        nowEventUIClickCount = 0;

        suddenEventPanel.GettempNowSpeakerImgIndex = 1;

        // ���� �̺�Ʈ�� ��ũ��Ʈ      

        // �ܼ� or ��� ���� �̺�Ʈ�� start���
        string nowtempScript = nowScript.splitText[nowEventUIClickCount];
        string nowTempScriptSpeaker = nowScript.speaker[nowEventUIClickCount].ToString();
        suddenEventPanel.CheckNowScenarioScriptData(1, nowTempScriptSpeaker, nowtempScript, nowTargetStudent);

        if (tempAllInstructorInfoPanel.gameObject.activeSelf == true && (InGameUI.Instance.UIStack.Count == 0 || InGameUI.Instance.UIStack.Count != 0))
        {
            tempPopOffAllInstructorInfoPanel.TurnOffUI();
        }

        if (tempAllStudentInfoPanel.gameObject.activeSelf == true && (InGameUI.Instance.UIStack.Count == 0 || InGameUI.Instance.UIStack.Count != 0))
        {
            tempPopOffAllStudentInfoPanel.TurnOffUI();
        }

        if (tempInstructorInfoPanel.GetAllInstructorPanel.activeSelf == true && (InGameUI.Instance.UIStack.Count == 0 || InGameUI.Instance.UIStack.Count != 0))
        {
            tempPopOffInstructorInfoPanel.TurnOffUI();
        }

        if (tempStudentInfoPanel.gameObject.activeSelf == true && (InGameUI.Instance.UIStack.Count == 0 || InGameUI.Instance.UIStack.Count != 0))
        {
            tempPopOffStudentInfoPanel.TurnOffUI();
        }



        // UI �����ִ� ó��
        suddenEventPanel.PopUpStartEventScriptPanel();
    }

    public void PutOnScreenRewardPanel()
    {
        PeekOrSetEventReward(true); // �̺�Ʈ ���� ������ �����´�.
                                    // �����ر� - �ܼ����� �������θ� Ǯ����?
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
            // �����尡 ������ or �����ر��϶� or ȸ�� �ر��϶� ���� â ����ֱ�
            if (NowEventRewardInfo[i].RewardAmount != 0 || eventScheduleSystem.TodaySuddenEventList[0].Teacher != 0 || eventScheduleSystem.TodaySuddenEventList[0].Company != 0)
            {
                suddenEventPanel.PopUpRewardPanel();
                suddenEventPanel.FindSoundRewardResult();

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

    // ��ŸƮ��ũ��Ʈ�� ������ ��� �� ��ư Ŭ�� �� �������� �������� �гη� �˸°� ã�ư��� ���� �Լ�
    public void ClickScenarioPanelNextButton()
    {
        ClickEventManager.Instance.Sound.PlayIconTouch();

        nowEventUIClickCount++; // "/" �� ������ ��ȭ�� üũ �ϱ� ���� ���� 

        // start�� �̺�Ʈ�� ���۽�Ű�� ������ true��.
        if (StartScriptReady)
        {
            // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextStart.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextStart.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextStart.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(1, nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
            }
            else
            {
                if (NowEventType == 2) // ������
                {
                    StartScriptReady = false;

                    ReadyChoiceOptionScriptText();
                }
                else if (NowEventType == 1) // �ܼ�����
                {
                    StartScriptReady = false;
                    nowEventUIClickCount = 0;
                    suddenEventPanel.PopOffStartEventScriptPanel();

                    ReadyTimeBarPanel();
                }
                else if (NowEventType == 3) // �����
                {
                    StartScriptReady = false;

                    ReadyTargetSelectScriptText();
                }
            }
        }
        else if (MiddelScriptReady)
        {
            // �̵� �ִ� �� üũ
            // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextMiddle.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextMiddle.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextMiddle.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(1, nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
            }
            else
            {
                // 
                MiddelScriptReady = false;
                suddenEventPanel.PopOffStartEventScriptPanel();
                //FinScriptReady = true;
            }
        }
        // �̵� ���� �� �ڷ�ƾ ȣ���ؾ� �ϴµ�?


        else if (FinScriptReady)
        {
            // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextFin.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextFin.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextFin.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(1, nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
            }
            else // ���� â ����
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
                    // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
                    if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextSelect1.splitText.Count)
                    {
                        string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextSelect1.speaker[nowEventUIClickCount].ToString();
                        string nowtempScript = nowEventScript.SplitEventScriptTextSelect1.splitText[nowEventUIClickCount];

                        suddenEventPanel.CheckNowScenarioScriptData(1, nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
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
                    // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
                    if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextSelect2.splitText.Count)
                    {
                        string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextSelect2.speaker[nowEventUIClickCount].ToString();
                        string nowtempScript = nowEventScript.SplitEventScriptTextSelect2.splitText[nowEventUIClickCount];

                        suddenEventPanel.CheckNowScenarioScriptData(1, nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
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
                    // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
                    if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextSelect3.splitText.Count)
                    {
                        string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextSelect3.speaker[nowEventUIClickCount].ToString();
                        string nowtempScript = nowEventScript.SplitEventScriptTextSelect3.splitText[nowEventUIClickCount];

                        suddenEventPanel.CheckNowScenarioScriptData(1, nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
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

        #region ������ �����̺�Ʈ ���� ���� �ڵ�
        /*
                 if (FinScriptReady) // ���� ��ȭ�� start ���� fin ���� middle���� üũ
        {
            if (MiddelScriptReady)
            {
                // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
                if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextFin.splitText.Count)
                {
                    string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextFin.speaker[nowEventUIClickCount].ToString();
                    string nowtempScript = nowEventScript.SplitEventScriptTextFin.splitText[nowEventUIClickCount];

                    suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript);
                }
                else // ���� â ����
                {
                    // ����� �̺�Ʈ�� ����ð��� ���������� �Ǻ� -> ��ũ��Ʈ�� �ƴ϶� �ð����� �Ǻ��ؾ��ϳ�? �ƴϸ� �̹� �̺�Ʈ�� Ÿ���� ����� �̺�Ʈ��°��� üũ�� �� �ؾ��ϳ�?
                    if (eventScheduleSystem.ReadySuddenEventList[0].SuddenEventDuration != 0)
                    {
                        suddenEventPanel.PopOffStartEventScriptPanel(); // ����ð� ���� �� �׳� â ���ֱ�

                        // if ((((GameTime.Instance.FlowTime.NowYear - 1) * 12 * 4 * 5) +
                        // ((GameTime.Instance.FlowTime.NowMonth - 1) * 4 * 5) + ((GameTime.Instance.FlowTime.NowWeek - 1) * 5) + GameTime.Instance.FlowTime.NowDay) =>
                        // eventEndDay)

                        if (nowEventUIClickCount <= nowEventScript.SplitEventScriptTextFin.splitText.Count)       // ��ȭ ���� ����
                        {
                            PeekOrSetEventReward(true); // �̺�Ʈ ���� ������ �����´�.
                            // �����ر� - �ܼ����� �������θ� Ǯ����?
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
                                suddenEventPanel.PopOffStartEventScriptPanel(); // ����ð� ���� �� �׳� â ���ֱ�

                                // ����
                                // suddenEventPanel.ReadyReawrdPanelText(NowEventRewardInfo);
                                // suddenEventPanel.PopOffStartEventScriptPanel();
                                // suddenEventPanel.PopUpRewardPanel();
                            }
                            // else
                            // {
                            // �߰��� ���� fin�� �ؽ�Ʈ�� �����


                            // ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
                            // suddenEventPanel.PopOffStartEventScriptPanel();
                            // }
                        }
                        else
                        {
                            PeekOrSetEventReward(true); // �̺�Ʈ ���� ������ �����´�.
                                                        // �����ر� - �ܼ����� �������θ� Ǯ����?
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
                // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
                if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextFin.splitText.Count)
                {
                    string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextFin.speaker[nowEventUIClickCount].ToString();
                    string nowtempScript = nowEventScript.SplitEventScriptTextFin.splitText[nowEventUIClickCount];

                    suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript);
                }
                else // ���� â ����
                {
                    PeekOrSetEventReward(true);
                    // �����ر� - �ܼ����� �������θ� Ǯ����?
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
            // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextStart.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextStart.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextStart.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript);
            }
            else
            {
                if (NowEventType == 2) // ������
                {
                    FinScriptReady = true;
                    MiddelScriptReady = true;

                    ReadyChoiceOptionScriptText();
                }
                else if (NowEventType == 1) // �ܼ�����
                {
                    nowEventUIClickCount = 0;
                    FinScriptReady = true;
                    MiddelScriptReady = true;
                    suddenEventPanel.PopOffStartEventScriptPanel();

                    ReadyTimeBarPanel();
                }
                else if (NowEventType == 3) // �����
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

    // �� ������ �̺�Ʈ�� ������ ��ư�鿡 �޾Ƴ���, �̵� üũ �� ������ �׳� ����, ������ �ó������г� �˾� �ϱ�
    public void CheckMiddelScenarioPanelPop()
    {
        // ���� �������� �̺�Ʈ�� ��ũ��Ʈ�� middle�� �ִ��� üũ�Ѵ�. "" �� ���, �ƹ� �͵� ���� �ʰ� ���� â���� �Ѿ���� �Ѵ�
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

    // �������̺�Ʈ 1, 2, 3�� �Ǻ� �ϱ� ���� �Լ� -> �ܼ����࿡���� Ÿ�ӹ��г��� ��縦 ���� ���Ѱ�, ������������ ������ �������� ���� ���� ��ũ��Ʈ
    public void ReadyTimeBarPanel()
    {
        if (eventScheduleSystem.TodaySuddenEventList[0].SuddenEventTime != 0) // �̺�Ʈ ����ð��� ������
        {
            int TimeBarScriptTime = eventScheduleSystem.TodaySuddenEventList[0].SuddenEventTime;
            suddenEventPanel.GetSimpleExecutionFillBar.fillAmount = 0;

            // ���� �� Ÿ�ӹ� �̺�Ʈ�� �´� ��������Ʈ�� �־��ش�.
            for (int i = 0; i < UISpriteLists.Instance.GetTimeBarSpriteList.Count; i++)
            {
                if (eventScheduleSystem.TodaySuddenEventList[0].SuddenEventID.ToString() == "1103000")
                {
                    suddenEventPanel.GetSimpleExecutionEventImage.GetComponent<Animator>().enabled = true;
                    suddenEventPanel.GetSimpleExecutionEventImage.sprite = UISpriteLists.Instance.GetTimeBarSpriteList[i];
                }
                else if (UISpriteLists.Instance.GetTimeBarSpriteList[i].name == eventScheduleSystem.TodaySuddenEventList[0].SuddenEventID.ToString())
                {
                    suddenEventPanel.GetSimpleExecutionEventImage.GetComponent<Animator>().enabled = false;
                    suddenEventPanel.GetSimpleExecutionEventImage.sprite = UISpriteLists.Instance.GetTimeBarSpriteList[i];
                }
            }

            suddenEventPanel.PopUpTimeBarPanel();
            TimeBarStartTime = Time.unscaledTime;       // ���⼭ Ÿ�ӹ��� ���� �ð��� �ޱ�

            if (nowEventScript.SplitEventScriptTextSelect1.splitText.Count != 0) // ��簡 ���� ��
            {
                suddenEventPanel.CheckTimeBarScriptText(nowEventScript, TimeBarScriptTime);
            }
            // while���� ���鼭 �ð�üũ�� ���Ŀ� ������ �ϳ�
            // ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
        }
        else // ����ð� ����, end �ó������� ������? �ó����� ���� �� �� �˾� ���� â���� �Ѿ��
        {
            if (nowEventScript.SplitEventScriptTextFin.splitText.Count != 0)
            {
                ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);

                FinScriptReady = true;
            }
            else                        // ���� â ����
            {
                PeekOrSetEventReward(true); // �̺�Ʈ ���� ������ �����´�.

                // �����ر� - �ܼ����� �������θ� Ǯ����?
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
                        suddenEventPanel.FindSoundRewardResult();

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

    // Time.time�� 0 �϶�, Ÿ�ӹٸ� �帣�� �ϱ� ���� �Լ�
    public void UpdateLoadingBar(float prograss, float[] thresholds, string[] texts)
    {
        // ���ڷ� ���� durationTime �� �ð���ŭ �ε��ٸ� �帣�� �Ѵ�.
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

    // ������ �̺�Ʈ�� ��縦 ���� ���� �غ�
    public void ReadyChoiceOptionScriptText()
    {
        int SpeakerIndex = nowEventScript.SplitEventScriptTextStart.speaker.Count;
        string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextStart.speaker[SpeakerIndex - 1].ToString();        // ������ ����� ȭ��
        string nowtempScript = ""; // ��ũ��Ʈ

        // Ŭ���� ���� �迭 üũ �ϴϱ� �迭 ũ�� �Ѿ�� ����
        // split �ص� ��� ��縦 �ϳ��� ��ģ �� UI�� ���

        // for (int i = 0; i < nowEventScript.SplitEventScriptTextStart.splitText.Count; i++)
        // {
        //     nowtempScript += nowEventScript.SplitEventScriptTextStart.splitText[i] + "\n";
        // }

        nowtempScript = nowEventScript.SplitEventScriptTextStart.splitText[SpeakerIndex - 1];

        string[] OptionScript = new string[3];      // ������ ������ ���� ��
        OptionScript[0] = "";
        OptionScript[1] = "";
        OptionScript[2] = "";

        // ������ ���鼭 �˸��� ��ũ��Ʈ�� ã�� �Լ� �ʿ�
        ReadyOptionsChoiceEvent(OptionScript);

        suddenEventPanel.IfHaveLowMoneyNow(OptionEventNeedPay, 1);
        suddenEventPanel.IfHaveLowMoneyNow(OptionEventNeedPay, 2);
        suddenEventPanel.IfHaveLowMoneyNow(OptionEventNeedPay, 3);
        // nowEventUIClickCount = 0;
        suddenEventPanel.ReadyEventRewardText(OptionEventNeedPay);
        suddenEventPanel.ReadyOptionChoicePanelText(nowTempScriptSpeaker, nowtempScript, OptionScript, NowEventRewardInfo, nowTargetStudent);
        suddenEventPanel.PopOffStartEventScriptPanel();
        suddenEventPanel.PopUpOptionChoiceEvent();
    }

    // ������ �̺�Ʈ�� �гο� �����͸� ä��� ���� �Լ�
    public void ReadyOptionsChoiceEvent(string[] OptionScript)
    {
        for (int i = 0; i < AllOriginalJsonData.Instance.ChoiceEventReward.Count; i++)
        {
            // �ش� �̺�Ʈ�� ������ID 3���� ������DT�� ID �� ���ؼ� �����͸� �־�����
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

    // ���� ���� ��ư�� �����ϱ� ���� ���� ���ֱ�, ��ư Ŭ�� �� ȭ���� ��ȯ
    public void IfClickOptionButton()
    {
        // �Ҹ���ȭ���� ���� ���� ��ȭ�� �� �ؾ� �Ѵ�.
        // �Ҹ� ��ȭ�� 2����. 
        GameObject nowClickOption = EventSystem.current.currentSelectedGameObject;

        if (nowClickOption.name == suddenEventPanel.GetOption1Button.name) // ������ ����             1�� ������
        {
            SelectedOptionNumber = 1;

            PlayerInfo.Instance.MyMoney -= OptionEventNeedPay.PayAmount1;
            if (OptionEventNeedPay.PayAmount1 < 0)
            {
                MonthlyReporter.Instance.m_NowMonth.ExpensesEventCost += -(OptionEventNeedPay.PayAmount1);
            }
            else
            {
                MonthlyReporter.Instance.m_NowMonth.ExpensesEventCost += OptionEventNeedPay.PayAmount1;
            }

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
                            suddenEventPanel.FindSoundRewardResult();

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
        else if (nowClickOption.name == suddenEventPanel.GetOption2Button.name)                 //     2�� ������
        {
            SelectedOptionNumber = 2;

            PlayerInfo.Instance.MyMoney -= OptionEventNeedPay.PayAmount2;
            if (OptionEventNeedPay.PayAmount2 < 0)
            {
                MonthlyReporter.Instance.m_NowMonth.ExpensesEventCost += -(OptionEventNeedPay.PayAmount2);
            }
            else
            {
                MonthlyReporter.Instance.m_NowMonth.ExpensesEventCost += OptionEventNeedPay.PayAmount2;
            }

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
                            suddenEventPanel.FindSoundRewardResult();

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
        else if (nowClickOption.name == suddenEventPanel.GetOption3Button.name)                 //     3�� ������
        {
            SelectedOptionNumber = 3;

            PlayerInfo.Instance.MyMoney -= OptionEventNeedPay.PayAmount3;

            if (OptionEventNeedPay.PayAmount3 < 0)
            {
                MonthlyReporter.Instance.m_NowMonth.ExpensesEventCost += -(OptionEventNeedPay.PayAmount3);
            }
            else
            {
                MonthlyReporter.Instance.m_NowMonth.ExpensesEventCost += OptionEventNeedPay.PayAmount3;
            }

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
                            suddenEventPanel.FindSoundRewardResult();

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

    // ��� ���� �̺�Ʈ�� ��縦 ���� ���� �غ�
    public void ReadyTargetSelectScriptText()
    {
        // �г� ����, �ش� �̺�Ʈ ������ ����, �л����� �ȵ�����
        // �л� �ε� �ؾ��� -> ������ ���� ���ϰ� 6ĭ�� ������ �ٸ��� �ߵ���
        // �л� Ŭ�� �� �ش� �л� ���� ��

        // ����� �̺�Ʈ �˾� �� ó���� ������ �����ֱ� ���� �л� ��� ����
        SetTargetEventStudentPanel();

        // ����� �̺�Ʈ �г� ����
        suddenEventPanel.PopUpTargetSelectEvent();
        suddenEventPanel.PopOffStartEventScriptPanel();

    }

    // Ÿ���̺�Ʈ�� �� ��, ������ �����ؼ� ����� �Լ�
    public void SetTargetEventStudentPanel()
    {
        List<Student> tempData = ObjectManager.Instance.m_StudentList;
        int studentCount = ObjectManager.Instance.m_StudentList.Count;

        string tempTerm = eventScheduleSystem.TodaySuddenEventList[0].SuddenEventDuration + "��";
        string DispatchSpot = eventScheduleSystem.TodaySuddenEventList[0].Where;

        Image[] tempImg = new Image[6];
        string[] tempName = { };

        for (int i = 0; i < studentCount; i++)
        {
            if (tempData[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
            {
                NowDepartmentStudentList tempStudent = new NowDepartmentStudentList();

                if (tempData[i].m_StudentStat.m_UserSettingName != "")
                {
                    tempStudent.ChangedName = tempData[i].m_StudentStat.m_UserSettingName;
                }
                else
                {
                    tempStudent.ChangedName = "";
                }

                tempStudent.studentName = tempData[i].m_StudentStat.m_StudentName;
                tempStudent.studentImg = tempData[i].StudentProfileImg;
                tempStudent.type = tempData[i].m_StudentStat.m_StudentType;

                EachDepartmentStduentList.Add(tempStudent);
            }
        }

        float anchoredPotisionX = suddenEventPanel.GetStudentListParent.GetComponent<RectTransform>().anchoredPosition.x;
        suddenEventPanel.GetStudentListParent.GetComponent<RectTransform>().anchoredPosition = new Vector3(anchoredPotisionX, 0, 0);

        suddenEventPanel.TargetEventStudentListSetData(EachDepartmentStduentList);
        suddenEventPanel.SetTargetEventInfo(NowEventRewardInfo, tempTerm, DispatchSpot);
        suddenEventPanel.InitTargetEventEachStudentInfo();
        //suddenEventPanel.PopUpTargetEventPanelSetData();
    }

    // �� �а���ư Ŭ���� �а��� �´� �л� ������ �ߵ��� �ؾ���
    public void IfClickEachStduentDepartmentIndexButton()
    {
        ClickEventManager.Instance.Sound.PlayIconTouch();

        // ���� Ŭ���� �а��� �̸��� �˰�, �� �а��� �л��� ã�Ƽ� �����͸� �����ֵ��� ����
        GameObject nowClickButton = EventSystem.current.currentSelectedGameObject;
        List<Student> tempData = ObjectManager.Instance.m_StudentList;
        int studentCount = ObjectManager.Instance.m_StudentList.Count;

        float anchoredPotisionX = suddenEventPanel.GetStudentListParent.GetComponent<RectTransform>().anchoredPosition.x;
        suddenEventPanel.GetStudentListParent.GetComponent<RectTransform>().anchoredPosition = new Vector3(anchoredPotisionX, 0, 0);

        // �а� �ε����� ������ ������ ���õ� �л� ����, ���õ� �л� ����� ǥ�õ� ���� �ʴ´�.
        // for (int i = 0; i < suddenEventPanel.GetCheckedStudentImgList.Count; i++)
        // {
        //     suddenEventPanel.GetCheckedStudentImgList[i].gameObject.SetActive(false);
        // }
        ColorBlock tempGDColor = suddenEventPanel.GetGameDesignerIndex.colors;
        tempGDColor.normalColor = new Color(1f, 1f, 1f, 1f);
        tempGDColor.selectedColor = new Color(1f, 1f, 1f, 1f);
        suddenEventPanel.GetGameDesignerIndex.colors = tempGDColor;

        ColorBlock tempArtColor = suddenEventPanel.GetArtIndex.colors;
        tempArtColor.normalColor = new Color(1f, 1f, 1f, 1f);
        tempArtColor.selectedColor = new Color(1f, 1f, 1f, 1f);
        suddenEventPanel.GetArtIndex.colors = tempArtColor;

        ColorBlock tempProColor = suddenEventPanel.GetProgrammerIndex.colors;
        tempProColor.normalColor = new Color(1f, 1f, 1f, 1f);
        tempProColor.selectedColor = new Color(1f, 1f, 1f, 1f);
        suddenEventPanel.GetProgrammerIndex.colors = tempProColor;

        switch (nowClickButton.name)
        {
            case "GameDesignerIndex":
            {
                tempGDColor = suddenEventPanel.GetGameDesignerIndex.colors;
                tempGDColor.normalColor = SelectedDarkColor;
                tempGDColor.selectedColor = SelectedDarkColor;
                suddenEventPanel.GetGameDesignerIndex.colors = tempGDColor;

                // suddenEventPanel.GetGameDesignerIndex.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 96);
                // suddenEventPanel.GetArtIndex.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 80);
                // suddenEventPanel.GetProgrammerIndex.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 80);

                EachDepartmentStduentList.Clear();

                for (int i = 0; i < studentCount; i++)
                {
                    if (tempData[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                    {
                        NowDepartmentStudentList tempStudent = new NowDepartmentStudentList();

                        if (tempData[i].m_StudentStat.m_UserSettingName != "")
                        {
                            tempStudent.ChangedName = tempData[i].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            tempStudent.ChangedName = "";
                        }
                        tempStudent.studentName = tempData[i].m_StudentStat.m_StudentName;
                        tempStudent.studentImg = tempData[i].StudentProfileImg;
                        tempStudent.type = tempData[i].m_StudentStat.m_StudentType;

                        EachDepartmentStduentList.Add(tempStudent);
                    }
                }
                // nowTargetStudent = null;

                for (int i = 0; i < EachDepartmentStduentList.Count; i++)
                {
                    if (nowTargetStudent != null)            // ���� ���õ� �л��� ���� ��
                    {
                        if (nowTargetStudent.m_StudentStat.m_StudentName == EachDepartmentStduentList[i].studentName)
                        {
                            suddenEventPanel.GetCheckedStudentImgList[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            suddenEventPanel.GetCheckedStudentImgList[i].gameObject.SetActive(false);
                        }
                    }
                }

                suddenEventPanel.TargetEventStudentListSetData(EachDepartmentStduentList);
                // suddenEventPanel.PopUpTargetEventPanelSetData(nowTargetStudent);
            }
            break;
            case "ArtIndex":
            {
                tempArtColor = suddenEventPanel.GetArtIndex.colors;
                tempArtColor.normalColor = SelectedDarkColor;
                tempArtColor.selectedColor = SelectedDarkColor;
                suddenEventPanel.GetArtIndex.colors = tempArtColor;

                EachDepartmentStduentList.Clear();

                for (int i = 0; i < studentCount; i++)
                {
                    if (tempData[i].m_StudentStat.m_StudentType == StudentType.Art)
                    {
                        NowDepartmentStudentList tempStudent = new NowDepartmentStudentList();

                        if (tempData[i].m_StudentStat.m_UserSettingName != "")
                        {
                            tempStudent.ChangedName = tempData[i].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            tempStudent.ChangedName = "";
                        }
                        tempStudent.studentName = tempData[i].m_StudentStat.m_StudentName;
                        tempStudent.studentImg = tempData[i].StudentProfileImg;
                        tempStudent.type = tempData[i].m_StudentStat.m_StudentType;

                        EachDepartmentStduentList.Add(tempStudent);
                    }
                }
                // nowTargetStudent = null;

                for (int i = 0; i < EachDepartmentStduentList.Count; i++)
                {
                    if (nowTargetStudent != null)            // ���� ���õ� �л��� ���� ��
                    {
                        if (nowTargetStudent.m_StudentStat.m_StudentName == EachDepartmentStduentList[i].studentName)
                        {
                            suddenEventPanel.GetCheckedStudentImgList[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            suddenEventPanel.GetCheckedStudentImgList[i].gameObject.SetActive(false);
                        }
                    }
                }

                suddenEventPanel.TargetEventStudentListSetData(EachDepartmentStduentList);
                //suddenEventPanel.PopUpTargetEventPanelSetData(nowTargetStudent);
            }
            break;
            case "ProgrammingIndex":
            {
                tempProColor = suddenEventPanel.GetProgrammerIndex.colors;
                tempProColor.normalColor = SelectedDarkColor;
                tempProColor.selectedColor = SelectedDarkColor;
                suddenEventPanel.GetProgrammerIndex.colors = tempProColor;

                EachDepartmentStduentList.Clear();

                for (int i = 0; i < studentCount; i++)
                {
                    if (tempData[i].m_StudentStat.m_StudentType == StudentType.Programming)
                    {
                        NowDepartmentStudentList tempStudent = new NowDepartmentStudentList();

                        if (tempData[i].m_StudentStat.m_UserSettingName != "")
                        {
                            tempStudent.ChangedName = tempData[i].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            tempStudent.ChangedName = "";
                        }
                        tempStudent.studentName = tempData[i].m_StudentStat.m_StudentName;
                        tempStudent.studentImg = tempData[i].StudentProfileImg;
                        tempStudent.type = tempData[i].m_StudentStat.m_StudentType;

                        EachDepartmentStduentList.Add(tempStudent);
                    }
                }
                // nowTargetStudent = null;

                for (int i = 0; i < EachDepartmentStduentList.Count; i++)
                {
                    if (nowTargetStudent != null)            // ���� ���õ� �л��� ���� ��
                    {
                        if (nowTargetStudent.m_StudentStat.m_StudentName == EachDepartmentStduentList[i].studentName)
                        {
                            suddenEventPanel.GetCheckedStudentImgList[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            suddenEventPanel.GetCheckedStudentImgList[i].gameObject.SetActive(false);
                        }
                    }
                }

                suddenEventPanel.TargetEventStudentListSetData(EachDepartmentStduentList);
                //suddenEventPanel.PopUpTargetEventPanelSetData(nowTargetStudent);
            }
            break;
        }
    }

    // �� �л��� �̹����� Ŭ�� �� -> �л� ������ ����ֱ�
    public void IfClickEachStudent()
    {
        ClickEventManager.Instance.Sound.PlayIconTouch();

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

    // ����� �̺�Ʈ ���ÿϷ� ��ư ������? -> �ó������г� ��� Ȯ��, ������ �ó����� �г� ���� / ������ �г� ���� �ð� ������
    public void IfClickTargetEventSetOKButton()
    {
        if (nowTargetStudent != null)
        {
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (nowTargetStudent.m_StudentStat.m_StudentName == ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentName)
                {
                    ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_IsActiving = false;
                    // �̺�Ʈ�� ���õ� �л��� ���¸� �ٲ��ش�
                    NowTime eventEndTime = new NowTime();
                    eventEndTime.NowYear = GameTime.Instance.FlowTime.NowYear;
                    eventEndTime.NowMonth = GameTime.Instance.FlowTime.NowMonth;
                    eventEndTime.NowWeek = GameTime.Instance.FlowTime.NowWeek;
                    eventEndTime.NowDay = GameTime.Instance.FlowTime.NowDay;

                    int eventDay = 5;       // �̺�Ʈ (��) �����ֱ�
                    eventEndTime.NowDay += eventDay;
                    if (eventEndTime.NowDay / 5 >= 1)
                    {
                        eventEndTime.NowWeek += eventEndTime.NowDay / 5;
                        eventEndTime.NowDay %= 5;
                    }


                    int EndTime = ((eventEndTime.NowYear - 1) * 12 * 4 * 5) +
                        ((eventEndTime.NowMonth - 1) * 4 * 5) + ((eventEndTime.NowWeek - 1) * 5) + eventEndTime.NowDay;

                    StartCoroutine(CheckTargetEventDispatchedStudent(EndTime));     // startCoroutine�� �ϸ� ȣ�� �� �� �ѹ� �Ҹ���.
                }
            }

            suddenEventPanel.PopOffTargetSelectEvent();

            // �л� ��Ͽ� ���õǾ��ٴ� �̹��� ���ֱ�
            for (int i = 0; i < suddenEventPanel.GetCheckedStudentImgList.Count; i++)
            {
                suddenEventPanel.GetCheckedStudentImgList[i].gameObject.SetActive(false);
            }

            // �̵� �ؽ�Ʈ ���
            MiddelScriptReady = true;
            ReadyScenarioPanel(nowEventScript.SplitEventScriptTextMiddle);
        }
    }

    // �ڷ�ƾ? ������Ʈ? �ڷ�ƾ ���鼭
    // �̺�Ʈ ����ð��� üũ, �ð� �Ѿ�� �Լ� �ߵ�
    IEnumerator CheckTargetEventDispatchedStudent(int eventEndDay)
    {
        // �Լ��� ������ ���ؼ�, ������ �¾�����  yeild return �Ʒ��� ���� ����ȴ�
        yield return new WaitUntil(/*����*/() =>
        (((GameTime.Instance.FlowTime.NowYear - 1) * 12 * 4 * 5) +
                    ((GameTime.Instance.FlowTime.NowMonth - 1) * 4 * 5) + ((GameTime.Instance.FlowTime.NowWeek - 1) * 5) + GameTime.Instance.FlowTime.NowDay) ==
        eventEndDay);

        // �˾�â �� �� ���� �� �ó����� �г� �غ�
        if (GameTime.Instance.IsGameMode == true && isReadyNowEventPopUp == true)
        {
            FinScriptReady = true;
            ReadyScenarioPanel(nowEventScript.SplitEventScriptTextFin);
        }
        // fin ��ũ��Ʈ�� �ִ� �� üũ, ������ ����ֱ�
        // �л� ���� ��ȭ


    }

    public void IfIClickGetRewardButton()
    {
        // Ʃ�丮�� ���� �ʿ� -> �帣�� ���׷��̵� 
        if (eventScheduleSystem.TodaySuddenEventList[0].SuddenEventID == 1105002 && eventScheduleSystem.TodaySuddenEventList.Count != 0)
        {
            PlayerInfo.Instance.IsUpgradePossible = true; // Ʃ�丮�� ���� �� �ӽ� ����
        }

        isReadyNowEventPopUp = false;
        PeekOrSetEventReward(false); // ���� ���� false �� �� -> ���� �ް� ���� �� ����
        suddenEventPanel.PopOffRewardPanel();
        SelectedOptionNumber = 0;

        eventScheduleSystem.TodaySuddenEventList.RemoveAt(0);
        nowTargetStudent = null;
        EachDepartmentStduentList.Clear();
    }

    // ���� ���� ����ü�� �����ϴ� �κ��� ���� �Լ�
    private void RewardInfoSetup(int index, int rewardID, int rewardAmount, string rewardName/*, string photoID = null, string companyName = null*/)
    {
        NowEventRewardInfo[index].RewardID = rewardID;
        NowEventRewardInfo[index].RewardAmount = rewardAmount;
        NowEventRewardInfo[index].RewardName = rewardName;
    }

    // true �� �� -> UI �� �� �ؽ�Ʈ, �̹���, �����ġ ���� (���� ���� ��������� ����)
    // false �϶� -> �̺�Ʈ�� ���� ���� �����Ѵ�.
    public void PeekOrSetEventReward(bool peek)
    {
        // TODO :: rewordText�� ����ϱ� ���� �ʱ�ȭ�� ����� �Ѵ�.
        // ���� ���� ��� �ؽ�Ʈ ����� ���� ����
        string[] rewardText = new string[2];
        int[] rewardID = new int[2]; // � ��������
        int[] amount = new int[2]; // �󸶳� ��������

        // ����� ������ ����ü �迭 �ʱ�ȭ
        for (int i = 0; i < NowEventRewardInfo.Length; i++)
        {
            NowEventRewardInfo[i].RewardID = 0;
            NowEventRewardInfo[i].RewardAmount = 0;
            NowEventRewardInfo[i].RewardName = "�ȳ�";
        }

        var nowEvent = eventScheduleSystem.TodaySuddenEventList[0];

        // �ܼ� ���� ���� ���� (100 ~ 199) �Լ��� �ϳ��� ���󾿸� ���
        if (nowEvent.RewardIndex3 != 0) // ������ 3���� ��
        {
            // if (nowEvent.SuddenEventID == 1105002)
            // {
            //     PlayerInfo.Instance.IsUpgradePossible = true;
            // }

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
        else if (nowEvent.RewardIndex2 != 0) // ������ 2���� ��
        {
            ApplyEventReword(nowEvent.RewardIndex2, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
                RewardInfoSetup(1, rewardID[0], amount[0], rewardText[0]);

            ApplyEventReword(nowEvent.RewardIndex1, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
                RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
        }
        else if (nowEvent.RewardIndex1 != 0) // ������ 1���� ��
        {
            ApplyEventReword(nowEvent.RewardIndex1, ref rewardText, ref rewardID, ref amount, peek);
            RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
        }

        // ���� �������� ���� ���� ���� (200 ~ 300)
        else if (nowEvent.RewardIndex1 == 0 && SelectedOptionNumber == 0) // ���� �ܼ� �̺�Ʈ�� �ƴϰ�, ���� ������ �޾ƿ��� ������
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
        else if (nowEvent.RewardIndex1 == 0 && SelectedOptionNumber == 1) // ���� �ܼ� �̺�Ʈ�� �ƴϰ�, ù��° �������� ������
        {
            ApplyEventReword(nowEvent.SelectRewardIndex1, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
            {
                RewardInfoSetup(1, rewardID[1], amount[1], rewardText[1]);
                RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
            }
        }
        else if (nowEvent.RewardIndex1 == 0 && SelectedOptionNumber == 2) // �ι�° �������� ������
        {
            ApplyEventReword(nowEvent.SelectRewardIndex2, ref rewardText, ref rewardID, ref amount, peek);
            if (peek)
            {
                RewardInfoSetup(1, rewardID[1], amount[1], rewardText[1]);
                RewardInfoSetup(0, rewardID[0], amount[0], rewardText[0]);
            }
        }
        else if (nowEvent.RewardIndex1 == 0 && SelectedOptionNumber == 3) // ����° �������� ������
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
        RewardTaker[] rewardTaker = { RewardTaker.None, RewardTaker.None }; // ���� �޴���

        // ����_�ܼ�����,����ú��� (100 ~ 199��)
        if (rewardID < 199)
        {
            foreach (var eventReward in AllOriginalJsonData.Instance.SimpleEventReward)
            {
                if (eventReward.EventID == rewardID)
                {
                    rewardTaker[0] = (RewardTaker)Enum.Parse(typeof(RewardTaker), eventReward.RewardTaker); // ���� ������
                    rewardIndex[0] = eventReward.Reward; // ���� �ε���
                    amount[0] = eventReward.Amount; // ���� ��
                }
            }
        }

        // ����_������ ���� (200 ~ 299��)
        else if (rewardID < 299)
        {
            foreach (var eventReward in AllOriginalJsonData.Instance.ChoiceEventReward)
            {
                if (eventReward.SelectEventID == rewardID)
                {
                    rewardTaker[0] = (RewardTaker)Enum.Parse(typeof(RewardTaker), eventReward.RewardTaker1); // ����1 ������
                    rewardIndex[0] = eventReward.Reward1; // ����1 �ε���
                    amount[0] = eventReward.Amount1; // ����1 ��
                    rewardTaker[1] = (RewardTaker)Enum.Parse(typeof(RewardTaker), eventReward.RewardTaker2); // ����2 ������
                    rewardIndex[1] = eventReward.Reward2; // ����2 �ε���
                    amount[1] = eventReward.Amount2; // ����2 ��

                    // if (rewardIndex[0] == 600) // ���� �ر��� �մϴ�
                    // {
                    //     rewardTextArr[0] = ApplyUnLockTeacherReward(check);
                    // }
                    // else if (rewardIndex[0] == 601)
                    // {
                    //     rewardTextArr[0] = ApplyUnLockCompanyReward(check);
                    // }

                    break;
                }
            }
        }

        for (int i = 0; i <= 1; i++)
        {
            switch (rewardTaker[i])
            {
                case RewardTaker.None: // �ʱ�ȭ�� ��������
                    rewardTextArr[i] = "�̺�Ʈ�� �����ϴ�...";
                    if (rewardID == 600) // ���� �ر��� �մϴ�
                    {
                        rewardTextArr[i] = ApplyUnLockTeacherReward(check);
                    }
                    else if (rewardID == 601)
                    {
                        rewardTextArr[i] = ApplyUnLockCompanyReward(check);
                    }
                    break;

                case RewardTaker.academy: // ��ī���� ��ü

                //else
                {
                    rewardTextArr[i] = ApplyAcademyEventReword(rewardIndex[i], amount[i], check);
                }
                break;

                case RewardTaker.allprof: // ������ ��� ����
                    foreach (var professor in Professor.Instance.GameManagerProfessor)
                    {
                        rewardTextArr[i] = ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                    }
                    foreach (var professor in Professor.Instance.ArtProfessor)
                    {
                        rewardTextArr[i] = ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                    }
                    foreach (var professor in Professor.Instance.ProgrammingProfessor)
                    {
                        rewardTextArr[i] = ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                    }

                    break;

                case RewardTaker.allstu: // ������ ��� �л�
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        // ��� �л� �ɷ�ġ ������Ű��...
                        rewardTextArr[i] =
                            ApplyStudentEventReword(student.m_StudentStat, rewardIndex[i], amount[i], check);
                    }

                    break;


                case RewardTaker.design: // ������ ��ȹ��Ʈ �л� ���
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        if (student.m_StudentStat.m_StudentType == StudentType.GameDesigner)
                        {
                            rewardTextArr[i] =
                                ApplyStudentEventReword(student.m_StudentStat, rewardIndex[i], amount[i], check);
                        }
                    }

                    break;

                case RewardTaker.art: // ������ ��Ʈ��Ʈ �л� ���
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        if (student.m_StudentStat.m_StudentType == StudentType.Art)
                        {
                            rewardTextArr[i] =
                                ApplyStudentEventReword(student.m_StudentStat, rewardIndex[i], amount[i], check);
                        }
                    }

                    break;

                case RewardTaker.pro: // ������ �ù���Ʈ �л� ���
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
                case RewardTaker.alone: // �̺�Ʈ ������ �ܵ�
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

                case RewardTaker.teacherAlone: // �̺�Ʈ ������ �ܵ�
                    foreach (var professor in Professor.Instance.GameManagerProfessor)
                    {
                        if (nowTargetTeacher.m_ProfessorName == professor.m_ProfessorName)
                        {
                            rewardTextArr[i] =
                            ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                        }
                    }
                    foreach (var professor in Professor.Instance.ArtProfessor)
                    {
                        if (nowTargetTeacher.m_ProfessorName == professor.m_ProfessorName)
                        {
                            rewardTextArr[i] =
                            ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                        }
                    }
                    foreach (var professor in Professor.Instance.ProgrammingProfessor)
                    {
                        if (nowTargetTeacher.m_ProfessorName == professor.m_ProfessorName)
                        {
                            rewardTextArr[i] =
                            ApplyProfEventReword(professor, rewardIndex[i], amount[i], check);
                        }
                    }
                    break;
            }
        }
    }

    // �̺�Ʈ�� ��ī������ ������ �����մϴ�.
    private string ApplyAcademyEventReword(int rewardIndex, int amount, bool check)
    {
        switch (rewardIndex)
        {
            case 0:
                return "���� ����";

            case 100: //	1����ȭ
                if (!check)
                {
                    PlayerInfo.Instance.MyMoney += amount;
                    if (amount < 0)
                    {
                        MonthlyReporter.Instance.m_NowMonth.ExpensesEventResult -= amount;      // ����
                    }
                    else
                    {
                        MonthlyReporter.Instance.m_NowMonth.IncomeEventResult += amount;        // ����
                    }
                }
                return "���";

            case 101: //	2����ȭ
                if (!check)
                {
                    PlayerInfo.Instance.SpecialPoint += amount;
                }
                return "���";

            case 300: //	�����		��ũ�� �ݿ�
                if (!check)
                {
                    PlayerInfo.Instance.Management += amount;
                    MonthlyReporter.Instance.m_NowMonth.GoodsScore += amount;
                }
                return "�����";

            case 301: //	���� ����		��ũ�� �ݿ�
                if (!check)
                {
                    PlayerInfo.Instance.Famous += amount;
                    MonthlyReporter.Instance.m_NowMonth.FamousScore += amount;
                }
                return "��������";

            case 302: //	Ȱ�� ����		��ũ�� �ݿ�
                if (!check)
                {
                    PlayerInfo.Instance.Activity += amount;
                    MonthlyReporter.Instance.m_NowMonth.ActivityScore += amount;
                }
                return "Ȱ������";

            // ���� �߰�
            case 303: //	����缺
                if (!check)
                {
                    PlayerInfo.Instance.TalentDevelopment += amount;
                    MonthlyReporter.Instance.m_NowMonth.TalentDevelopmentScore += amount;
                }

                return "����缺";

            default:
                return "��ī���� �߸��� ���� �ε���";
        }
    }

    // �̺�Ʈ�� Ư�� ������ �ɷ�ġ�� �����մϴ�.
    private string ApplyProfEventReword(ProfessorStat professorStat, int rewardIndex, int amount, bool check)
    {
        switch (rewardIndex)
        {
            case 0:
                return "���� ����";

            case 200: //	ü��
                if (!check)
                    professorStat.m_ProfessorHealth += amount;
                return "ü��";

            case 201: //	����
                if (!check)
                    professorStat.m_ProfessorPassion += amount;
                return "����";

            case 400: //	����ġ
                if (!check)
                    professorStat.m_ProfessorExperience += amount;
                return "����ġ";

            case 401: //	���޻��
                if (!check)
                    professorStat.m_ProfessorPay += amount;
                return "���޻��";

            default:
                return "����-�߸��� ���� �ε���";
        }
    }


    // �̺�Ʈ�� Ư�� �л��� �ɷ�ġ�� �����մϴ�.
    private string ApplyStudentEventReword(StudentStat studentStat, int rewardIndex, int amount, bool check)
    {
        switch (rewardIndex)
        {
            case 0:
                return "���� ����";

            case 200: //	ü��
                if (!check)
                    studentStat.m_Health += amount;
                return "ü��";

            case 201: //	����
                if (!check)
                    studentStat.m_Passion += amount;
                return "����";

            case 500: //	����
                if (!check)
                    studentStat.m_AbilityAmountArr[(int)AbilityType.Sense] += amount;
                return "����";

            case 501: //	����
                if (!check)
                    studentStat.m_AbilityAmountArr[(int)AbilityType.Concentration] += amount;
                return "����";

            case 502: //	��ġ
                if (!check)
                    studentStat.m_AbilityAmountArr[(int)AbilityType.Wit] += amount;
                return "��ġ";

            case 503: //	���
                if (!check)
                    studentStat.m_AbilityAmountArr[(int)AbilityType.Technique] += amount;
                return "���";

            case 504: //	����
                if (!check)
                    studentStat.m_AbilityAmountArr[(int)AbilityType.Insight] += amount;
                return "����";

            case 505: //	�׼�
                if (!check)
                    studentStat.m_GenreAmountArr[(int)GenreStat.Action] += amount;
                return "�׼�";

            case 506: //	�ùķ��̼�
                if (!check)
                    studentStat.m_GenreAmountArr[(int)GenreStat.Simulation] += amount;
                return "�ùķ��̼�";

            case 507: //	��庥��
                if (!check)
                    studentStat.m_GenreAmountArr[(int)GenreStat.Adventure] += amount;
                return "��庥ó";

            case 508: //	����
                if (!check)
                    studentStat.m_GenreAmountArr[(int)GenreStat.Shooting] += amount;
                return "����";

            case 509: //	RPG
                if (!check)
                    studentStat.m_GenreAmountArr[(int)GenreStat.RPG] += amount;
                return "RPG";

            case 510: //	����
                if (!check)
                    studentStat.m_GenreAmountArr[(int)GenreStat.Puzzle] += amount;
                return "����";

            case 511: //	����
                if (!check)
                    studentStat.m_GenreAmountArr[(int)GenreStat.Rhythm] += amount;
                return "����";

            case 512: //	������
                if (!check)
                    studentStat.m_GenreAmountArr[(int)GenreStat.Sports] += amount;
                return "������";

            default:
                return "�л�-�߸��� ���� �ε���";
        }
    }

    // ����, ȸ�� �ر� ������ (index 600) 
    private string ApplyUnLockTeacherReward(bool check)
    {
        ProfessorStat tempTeacherInfo = new ProfessorStat(); // �ر��� ������ ����

        foreach (var eventReward in AllOriginalJsonData.Instance.OriginalSuddenEventDataList) // ��ü �̺�Ʈ Ž��
        {
            if (eventReward.SuddenEventID == eventScheduleSystem.TodaySuddenEventList[0].SuddenEventID) // ���� �̺�Ʈ�� ��ü �̺�Ʈ ��
            {
                int teacherId = eventReward.Teacher;

                // TODO : ���縦 ��� �ؾ� �� �� �� �����غ���
                //List<ProfessorStat> tempData = new List<ProfessorStat>();// ��� ���� ���� ����
                //tempData.Clear();

                for (int j = 0; j < m_SelectProfessor.professorData.Count; j++)
                {
                    if (m_SelectProfessor.professorData.ElementAt(j).Value.m_ProfessorID == teacherId) // ��� ���� �������� �ر��� ���縦 ã�´�.
                    {
                        tempTeacherInfo = m_SelectProfessor.professorData.ElementAt(j).Value;
                    }
                }
            }
        }

        if (!check)     // ������ �ر� �� �� �ڵ�
        {
            EventRewardUnLockTeacher(tempTeacherInfo);

            PlayerInfo.Instance.UnLockTeacherCount++;       // ���� �ر� � ����� ī��Ʈ
        }

        return tempTeacherInfo.m_ProfessorName;        // ���� �̸�
    }

    // ȸ�� �ر� �ϴ� �κ�
    private string ApplyUnLockCompanyReward(bool check) //    (index 601) 
    {
        int count = 0;

        foreach (var eventReward in AllOriginalJsonData.Instance.OriginalSuddenEventDataList) // ��ü �̺�Ʈ Ž��
        {
            if (eventReward.SuddenEventID == eventScheduleSystem.TodaySuddenEventList[0].SuddenEventID) // ���� �̺�Ʈ�� ��ü �̺�Ʈ ��
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

        if (!check)     // ������ �ر� �� �� �ڵ�
        {
            NowCompanyList.m_CompanyList[count].IsUnLockCompany = true;
        }

        return NowCompanyList.m_CompanyList[count].CompanyName;
    }

    public void EventRewardUnLockTeacher(ProfessorStat temp)
    {
        if (temp.m_ProfessorType == StudentType.GameDesigner)
        {
            if (!Professor.Instance.GameManagerProfessor.Contains(temp))
            {
                Professor.Instance.GameManagerProfessor.Add(temp);
            }
        }
        else if (temp.m_ProfessorType == StudentType.Art)
        {
            if (!Professor.Instance.ArtProfessor.Contains(temp))
            {
                Professor.Instance.ArtProfessor.Add(temp);
            }
        }
        else if (temp.m_ProfessorType == StudentType.Programming)
        {
            if (!Professor.Instance.ProgrammingProfessor.Contains(temp))
            {
                Professor.Instance.ProgrammingProfessor.Add(temp);
            }
        }
    }
}