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
    //public Button NowStudentButton;
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

    [SerializeField] private ProfessorController m_SelectProfessor;     // ��� ���縦 ���鼭 �ر��� ���縦 ã�� ���� ����Ʈ

    private int nowEventUIClickCount = 0;
    private bool isReadyNowEventPopUp = false;
    // �ӽ� ������
    private bool TempIsReadyNowEventPopUp = false;

    public delegate void DelegateFunction(bool value);
    DelegateFunction delegateF;

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

                //cfDebug.Log("���̽� ������ �ε尡 �ȉ��");
            }
        }
        else
        {
            Debug.Log("���̽� �����Ͱ� �ε���� �ʾҵ�");
        }
    }

    // �� �ؽ�Ʈ ����� "/" �� "#" �� ������ ���
    public EachScript SplitSpeaker(string tempText)
    {
        EachScript tempScript = new EachScript();
        tempScript.splitText = new List<string>();
        tempScript.speaker = new List<ESpeakerImgIndex>();

        if (tempText != "")
        {
            string[] temptext = tempText.Split("/");        // ������ �ؽ�Ʈ ������ ���� ����

            for (int i = 0; i < temptext.Length; i++)
            {
                tempScript.speaker.Add(ESpeakerImgIndex.None);

                string temp = temptext[i];
                int tempIndex = temp.IndexOf("#");

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
                ReadyScenarioPanel(nowEventScript.SplitEventScriptTextStart);

                // ���� -> ������ ���� ��ư�� ���� �� isReadyNowEventPopUp = false ���ְ�(�̰� �˿��� �ɶ����� ���ֱ�) ����Ʈ�� ����ֱ�
                // eventScheduleSystem.ReadySuddenEventList.RemoveAt(0);
            }
        }
    }

    public void ReadyScenarioPanel(EachScript nowScript)
    {
        nowEventUIClickCount = 0;

        // ���� �̺�Ʈ�� ��ũ��Ʈ      

        // �ܼ� or ��� ���� �̺�Ʈ�� start���
        string nowtempScript = nowScript.splitText[nowEventUIClickCount];
        string nowTempScriptSpeaker = nowScript.speaker[nowEventUIClickCount].ToString();
        suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);

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

    // ��ŸƮ��ũ��Ʈ�� ������ ��� �� ��ư Ŭ�� �� �������� �������� �гη� �˸°� ã�ư��� ���� �Լ�
    public void ClickScenarioPanelNextButton()
    {
        nowEventUIClickCount++; // "/" �� ������ ��ȭ�� üũ �ϱ� ���� ���� 

        // start�� �̺�Ʈ�� ���۽�Ű�� ������ true��.
        if (StartScriptReady)
        {
            // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextStart.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextStart.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextStart.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
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
        // �̵� ���� �� �ڷ�ƾ ȣ���ؾ� �ϴµ�?


        else if (FinScriptReady)
        {
            // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
            if (nowEventUIClickCount < nowEventScript.SplitEventScriptTextFin.splitText.Count)
            {
                string nowTempScriptSpeaker = nowEventScript.SplitEventScriptTextFin.speaker[nowEventUIClickCount].ToString();
                string nowtempScript = nowEventScript.SplitEventScriptTextFin.splitText[nowEventUIClickCount];

                suddenEventPanel.CheckNowScenarioScriptData(nowTempScriptSpeaker, nowtempScript, nowTargetStudent);
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
                        // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
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
                        // �߸� ��ȭ �ִ��� üũ, ������ ��ȭ �ְ� ����, ������ ���� UI�� ����
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

        if (OptionEventNeedPay.SelectPay1 == 100)           // �Ҹ���ȭ�� �� �϶�
        {
            suddenEventPanel.IfHaveLowMoneyNow(OptionEventNeedPay, 1);
        }

        if (OptionEventNeedPay.SelectPay2 == 100)           // �Ҹ���ȭ�� �� �϶�
        {
            suddenEventPanel.IfHaveLowMoneyNow(OptionEventNeedPay, 2);
        }

        if (OptionEventNeedPay.SelectPay3 == 100)           // �Ҹ���ȭ�� �� �϶�
        {
            suddenEventPanel.IfHaveLowMoneyNow(OptionEventNeedPay, 3);
        }

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
        else if (nowClickOption.name == suddenEventPanel.GetOption2Button.name)                 //     2�� ������
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
        else if (nowClickOption.name == suddenEventPanel.GetOption3Button.name)                 //     3�� ������
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

    // �� �а���ư Ŭ���� �а��� �´� �л� ������ �ߵ��� �ؾ���
    public void IfClickEachStduentDepartmentIndexButton()
    {
        // ���� Ŭ���� �а��� �̸��� �˰�, �� �а��� �л��� ã�Ƽ� �����͸� �����ֵ��� ����
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

    // �� �л��� �̹����� Ŭ�� �� -> �л� ������ ����ֱ�
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
                    rewardTaker[0] = (RewardTaker)Enum.Parse(typeof(RewardTaker), eventReward.RewardTaker); // ����1 ������
                    rewardIndex[0] = eventReward.Reward1; // ����1 �ε���
                    amount[0] = eventReward.Amount1; // ����1 ��
                    rewardTaker[1] = (RewardTaker)Enum.Parse(typeof(RewardTaker), eventReward.RewardTaker); // ����2 ������
                    rewardIndex[1] = eventReward.Reward2; // ����2 �ε���
                    amount[1] = eventReward.Amount2; // ����2 ��

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
                    PlayerInfo.Instance.m_MyMoney += amount;
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
                    PlayerInfo.Instance.m_SpecialPoint += amount;
                }
                return "���";

            case 300: //	�����		��ũ�� �ݿ�
                if (!check)
                {
                    PlayerInfo.Instance.m_Management += amount;
                    MonthlyReporter.Instance.m_NowMonth.GoodsScore += amount;
                }
                return "�����";

            case 301: //	���� ����		��ũ�� �ݿ�
                if (!check)
                {
                    PlayerInfo.Instance.m_Awareness += amount;
                    MonthlyReporter.Instance.m_NowMonth.FamousScore += amount;
                }
                return "��������";

            case 302: //	Ȱ�� ����		��ũ�� �ݿ�
                if (!check)
                {
                    PlayerInfo.Instance.m_Activity += amount;
                    MonthlyReporter.Instance.m_NowMonth.ActivityScore += amount;
                }
                return "Ȱ������";

            // ���� �߰�
            case 303: //	����缺
                if (!check)
                {
                    PlayerInfo.Instance.m_TalentDevelopment += amount;
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
                    professorStat.professorHealth += amount;
                return "����-ü��";

            case 201: //	����
                if (!check)
                    professorStat.professorPassion += amount;
                return "����-����";

            case 400: //	����ġ
                if (!check)
                    professorStat.m_ProfessorExperience += amount;
                return "����-����ġ";

            case 401: //	���޻��
                if (!check)
                    professorStat.m_ProfessorPay += amount;
                return "����-���޻��";

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
                return "�л�-ü��";

            case 201: //	����
                if (!check)
                    studentStat.m_Passion += amount;
                return "�л�-����";

            case 500: //	����
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Sense] += amount;
                return "�л�-����";

            case 501: //	����
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += amount;
                return "�л�-����";

            case 502: //	��ġ
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Wit] += amount;
                return "�л�-��ġ";

            case 503: //	���
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Technique] += amount;
                return "�л�-���";

            case 504: //	����
                if (!check)
                    studentStat.m_AbilityAmountList[(int)AbilityType.Insight] += amount;
                return "�л�-����";

            case 505: //	�׼�
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Action] += amount;
                return "�л�-�׼�";

            case 506: //	�ùķ��̼�
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Simulation] += amount;
                return "�л�-�ùķ��̼�";

            case 507: //	��庥��
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Adventure] += amount;
                return "�л�-��庥ó";

            case 508: //	����
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Shooting] += amount;
                return "�л�-����";

            case 509: //	RPG
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.RPG] += amount;
                return "�л�-RPG";

            case 510: //	����
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Puzzle] += amount;
                return "�л�-����";

            case 511: //	����
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Rhythm] += amount;
                return "�л�-����";

            case 512: //	������
                if (!check)
                    studentStat.m_GenreAmountList[(int)GenreStat.Sports] += amount;
                return "�л�-������";

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
                    if (m_SelectProfessor.professorData.ElementAt(j).Value.professorID == teacherId) // ��� ���� �������� �ر��� ���縦 ã�´�.
                    {
                        tempTeacherInfo = m_SelectProfessor.professorData.ElementAt(j).Value;
                    }
                }
            }
        }

        if (!check)     // ������ �ر� �� �� �ڵ�
        {
            // ���� �̸�
            tempTeacherInfo.m_IsUnLockProfessor = true;

            EventRewardUnLockTeacher(tempTeacherInfo);

            PlayerInfo.Instance.UnLockTeacherCount++;       // ���� �ر� � ����� ī��Ʈ
        }

        return tempTeacherInfo.m_ProfessorNameValue;        // ���� �̸�
    }

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