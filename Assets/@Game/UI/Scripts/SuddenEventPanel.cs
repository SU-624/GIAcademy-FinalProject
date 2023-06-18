using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.IO;
using System;

/// <summary>
/// 2023. 04. 17 Mang
/// 
/// 각종 이벤트들의 UI 패널들을 연결 시켜 줄 클래스.
/// UI 연결, 초기화함수, 등을 담당
/// </summary>
public class SuddenEventPanel : MonoBehaviour
{
    // [Space(5f)]
    [Header("돌발 이벤트 중 - 선택가능 이벤트 유아이")]         // 아직 없음
    [SerializeField] private EventScheduleUI MyEventScheduleUI;             // 선택 이벤트 할 때 필요할듯
    [SerializeField] private PopUpUI PopUpEventScheduleUI;
    [SerializeField] private PopOffUI PopOffEventScheduleUI;
    // --------------------
    [Space(10f)]
    [Header("돌발 이벤트 중 - 첫번째 팝업 될 시나리오 유아이")]
    [SerializeField] private Button ScenarioPanelNextButton;

    [SerializeField] private PopUpUI PopUpEvenScenarioPanel;                 // 모든 이벤트의 첫번째에 뜰 유아이
    [SerializeField] private PopOffUI PopOffEvenScenarioPanel;

    [Space(5f)]
    [SerializeField] private Image ScenarioSpeakerImage;
    [SerializeField] private TextMeshProUGUI TempScenarioSpeakerText;        // 이미지 들어오기 전 글로 보여줄 임시 텍스트

    [SerializeField] private TextMeshProUGUI EventScenarioText;

    // [SerializeField] private Button TempScenarioPanelCloseButton;
    // --------------------
    [Space(10f)]
    [Header("돌발 이벤트 중 - 분기 타서 갈라질 세가지 이벤트 타입")]
    [Header("단순실행 이벤트")]
    [SerializeField] private GameObject SimpleExecutionTimeBarPanel;      // 단순실행 이벤트 UI 패널
    [SerializeField] private PopUpUI PopUpSimpleExecutionTimeBarPanel;      // 단순실행 이벤트 UI 패널
    [SerializeField] private PopOffUI PopOffSimpleExecutionTimeBarPanel;
    [Space(5f)]
    [SerializeField] private Image SimpleExecutionEventImage;               // 단순실행 타임바 지날 때 이미지(영상?)
    [SerializeField] private TextMeshProUGUI SimpleExecutionEventText;      // 단순실행 타임바 지날 때 텍스트
    [SerializeField] private Slider SimpleExecutionSlideBar;                // 슬라이드바 배경
    [SerializeField] private Image SimpleExecutionFillBar;                  // 정해진 타임에 따라 움직일 슬라이드바

    // [SerializeField] private Button TempSimpleExecutionEventPanelCloseButton;
    [Space(10f)]
    [Header("선택지 이벤트")]
    [SerializeField] private PopUpUI PopUpOptionChoiceEventPanel;           // 선택지 이벤트 UI 패널
    [SerializeField] private PopOffUI PopOffOptionChoiceEventPanel;
    [Space(5f)]
    [SerializeField] private Image OptionEventSpeakerImage;                 // 선택지 이벤트 화자
    [SerializeField] private TextMeshProUGUI TempOptionEventSpeakerText;    // 임시 말하는 사람 이름
    [SerializeField] private TextMeshProUGUI NowEventScriptText;            // 임시 말하는 사람 이름
    [Space(5f)]
    [SerializeField] private GameObject Option1Box;                         // 선택지 1번 오브젝트
    [SerializeField] private Button Option1Button;                          // 선택지 1번 버튼
    [SerializeField] private TextMeshProUGUI Option1ScriptText;             // 선택지 1번 스크립트
    [SerializeField] private TextMeshProUGUI Option1RewardText;             // 선택지 1 보상 설명
    [SerializeField] private Image Option1NeedPayImage;                     // 필요 돈 이미지
    [SerializeField] private TextMeshProUGUI Option1NeedPayText;            // 필요 돈 텍스트
    [Space(5f)]
    [SerializeField] private GameObject Option2Box;                         // 선택지 2번 오브젝트
    [SerializeField] private Button Option2Button;                          // 선택지 2번 버튼
    [SerializeField] private TextMeshProUGUI Option2ScriptText;             // 선택지 2번 스크립트
    [SerializeField] private TextMeshProUGUI Option2RewardText;             // 선택지 2 보상 설명
    [SerializeField] private Image Option2NeedPayImage;                     // 필요 돈 이미지
    [SerializeField] private TextMeshProUGUI Option2NeedPayText;            // 필요 돈 텍스트
    [Space(5f)]
    [SerializeField] private GameObject Option3Box;                         // 선택지 3번 오브젝트
    [SerializeField] private Button Option3Button;                          // 선택지 3번 버튼
    [SerializeField] private TextMeshProUGUI Option3ScriptText;             // 선택지 3번 스크립트
    [SerializeField] private TextMeshProUGUI Option3RewardText;             // 선택지 3 보상 설명
    [SerializeField] private Image Option3NeedPayImage;                     // 필요 돈 이미지
    [SerializeField] private TextMeshProUGUI Option3NeedPayText;            // 필요 돈 텍스트
    [Space(5f)]
    [SerializeField] private Button GiveUpSelectEventButton;
    // [SerializeField] private Button TempOptionChoiceEventPanelCloseButton;

    [Space(10f)]
    [Header("대상선택 이벤트")]
    [SerializeField] private PopUpUI PopUpTargetSelectEventPanel;           // 대상선택 이벤트 UI 패널
    [SerializeField] private PopOffUI PopOffTargetSelectEventPanel;
    [Space(5f)]
    [Header("개별학생 이름 사진")]
    [SerializeField] private Image StudentImage;
    [SerializeField] private TextMeshProUGUI StudentNameText;
    [Space(5f)]
    [Header("개별 학생의 스탯")]
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private TextMeshProUGUI PassionText;
    [SerializeField] private TextMeshProUGUI SenseText;
    [SerializeField] private TextMeshProUGUI WitText;
    [SerializeField] private TextMeshProUGUI ConcentrationText;
    [SerializeField] private TextMeshProUGUI TechniqueText;
    [SerializeField] private TextMeshProUGUI InsightText;
    [Space(5f)]
    [Header("개별 학생의 장르스탯")]
    [SerializeField] private TextMeshProUGUI PuzzleText;
    [SerializeField] private TextMeshProUGUI SimulationText;
    [SerializeField] private TextMeshProUGUI RhythmText;
    [SerializeField] private TextMeshProUGUI AdventureText;
    [SerializeField] private TextMeshProUGUI RPGText;
    [SerializeField] private TextMeshProUGUI SportsText;
    [SerializeField] private TextMeshProUGUI ActionText;
    [SerializeField] private TextMeshProUGUI ShootingText;
    [Space(5f)]
    [Header("이벤트의 설명")]
    [SerializeField] private TextMeshProUGUI PositionText;
    [SerializeField] private TextMeshProUGUI TermText;
    [SerializeField] private TextMeshProUGUI RewardText;
    [Space(5f)]
    [Header("학생목록 각 과 인덱스")]
    [SerializeField] private Button GameDesignerIndex;
    [SerializeField] private Button ArtIndex;
    [SerializeField] private Button ProgrammerIndex;
    [Space(5f)]
    [Header("학생목록리스트")]
    [SerializeField] private Transform StudentListParent;

    [SerializeField] private Image StudentListImage1;
    [SerializeField] private TextMeshProUGUI StudentListName1;
    [SerializeField] private Button StudentListButton1;
    [SerializeField] private Image SelectedStudentImage1;

    [SerializeField] private Image StudentListImage2;
    [SerializeField] private TextMeshProUGUI StudentListName2;
    [SerializeField] private Button StudentListButton2;
    [SerializeField] private Image SelectedStudentImage2;

    [SerializeField] private Image StudentListImage3;
    [SerializeField] private TextMeshProUGUI StudentListName3;
    [SerializeField] private Button StudentListButton3;
    [SerializeField] private Image SelectedStudentImage3;

    [SerializeField] private Image StudentListImage4;
    [SerializeField] private TextMeshProUGUI StudentListName4;
    [SerializeField] private Button StudentListButton4;
    [SerializeField] private Image SelectedStudentImage4;

    [SerializeField] private Image StudentListImage5;
    [SerializeField] private TextMeshProUGUI StudentListName5;
    [SerializeField] private Button StudentListButton5;
    [SerializeField] private Image SelectedStudentImage5;

    [SerializeField] private Image StudentListImage6;
    [SerializeField] private TextMeshProUGUI StudentListName6;
    [SerializeField] private Button StudentListButton6;
    [SerializeField] private Image SelectedStudentImage6;

    [Space(5f)]
    [Header("대상선택 이벤트 선택완료 & 포기하기 버튼")]
    [SerializeField] private Button TargetSetOkButton;
    [SerializeField] private Button GiveUpTargetEventButton;

    [SerializeField] private Button TempTargetSelectEventPanelCloseButton;
    // --------------------
    [Space(10f)]
    [Header("돌발 이벤트 중 - 이벤트 후 보상 창")]
    [SerializeField] private PopUpUI PopUpRewardEventPanel;                 // 이벤트보상 UI 패널
    [SerializeField] private PopOffUI PopOffRewardEventPanel;
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI RewardHeadlineText;
    [Space(5f)]
    [SerializeField] private GameObject Option1Object;                      // 보상 1
    [SerializeField] private TextMeshProUGUI Reward1Name;
    [SerializeField] private Image Reward1Image;
    [SerializeField] private TextMeshProUGUI Reward1Amount;

    [Space(5f)]
    [SerializeField] private GameObject Option2Object;                      // 보상 2
    [SerializeField] private TextMeshProUGUI Reward2Name;
    [SerializeField] private Image Reward2Image;
    [SerializeField] private TextMeshProUGUI Reward2Amount;
    [Space(5f)]
    [SerializeField] private GameObject Option3Object;                      // 보상 3
    [SerializeField] private TextMeshProUGUI Reward3Name;
    [SerializeField] private Image Reward3Image;
    [SerializeField] private TextMeshProUGUI Reward3Amount;
    [Space(5f)]
    [SerializeField] private Button SaveRewardButton;

    // --------------------
    List<Image> studentImgList = new List<Image>();
    List<TextMeshProUGUI> studentNameTextList = new List<TextMeshProUGUI>();
    List<Button> studentButtonList = new List<Button>();

    List<Image> CheckedStudentImgList = new List<Image>();
    // --------------------

    // List<Sprite> speakerSpriteList = new List<Sprite>();

    public Button GetScenarioPanelNextButton
    {
        get { return ScenarioPanelNextButton; }
    }

    public Image GetSimpleExecutionFillBar
    {
        get { return SimpleExecutionFillBar; }
        set { SimpleExecutionFillBar = value; }
    }
    public Slider GetSimpleExecutionSlideBar
    {
        get { return SimpleExecutionSlideBar; }
        set { SimpleExecutionSlideBar = value; }
    }

    public TextMeshProUGUI GetSimpleExecutionEventText
    {
        get { return SimpleExecutionEventText; }
    }

    public Button GetOption1Button
    {
        get { return Option1Button; }
    }

    public Button GetOption2Button
    {
        get { return Option2Button; }
    }

    public Button GetOption3Button
    {
        get { return Option3Button; }
    }

    public Button GetSaveRewardButton
    {
        get { return SaveRewardButton; }
    }

    public Button GetGameDesignerIndex
    {
        get { return GameDesignerIndex; }
    }

    public Button GetArtIndex
    {
        get { return ArtIndex; }
    }

    public Button GetProgrammerIndex
    {
        get { return ProgrammerIndex; }
    }

    public Button GetStudentListButton1
    {
        get { return StudentListButton1; }
    }

    public Button GetStudentListButton2
    {
        get { return StudentListButton2; }
    }

    public Button GetStudentListButton3
    {
        get { return StudentListButton3; }
    }

    public Button GetStudentListButton4
    {
        get { return StudentListButton4; }
    }

    public Button GetStudentListButton5
    {
        get { return StudentListButton5; }
    }

    public Button GetStudentListButton6
    {
        get { return StudentListButton6; }
    }

    public Button GetTargetSetOKButton
    {
        get { return TargetSetOkButton; }
    }

    public GameObject GetSimpleEventTimeBarPanel
    {
        get { return SimpleExecutionTimeBarPanel; }
    }

    public Button GetGiveUpSelectEventButton
    {
        get { return GiveUpSelectEventButton; }
    }
    public Button GetGiveUpTargetEventButton
    {
        get { return GiveUpTargetEventButton; }
    }

    public List<Image> GetCheckedStudentImgList
    {
        get { return CheckedStudentImgList; }
        set { CheckedStudentImgList = value; }
    }

    // 이 친구들을 이벤트스크립트매니저로 옮겨야 하나
    void Start()
    {
        // 여기서 모든 스피커 이미지를 로드하자.
        // LoadAllSpeakerImg();

        // TempScenarioPanelCloseButton.onClick.AddListener(PopOffStartEventScriptPanel);
        // TempSimpleExecutionEventPanelCloseButton.onClick.AddListener(PopOffTimeBarPanel);
        // TempOptionChoiceEventPanelCloseButton.onClick.AddListener(PopOffOptionChoiceEvent);
        // TempTargetSelectEventPanelCloseButton.onClick.AddListener(PopOffTargetSelectEvent);
        // GetRewardButton.onClick.AddListener(PopOffRewardPanel);

        studentImgList.Add(StudentListImage1);
        studentImgList.Add(StudentListImage2);
        studentImgList.Add(StudentListImage3);
        studentImgList.Add(StudentListImage4);
        studentImgList.Add(StudentListImage5);
        studentImgList.Add(StudentListImage6);

        studentNameTextList.Add(StudentListName1);
        studentNameTextList.Add(StudentListName2);
        studentNameTextList.Add(StudentListName3);
        studentNameTextList.Add(StudentListName4);
        studentNameTextList.Add(StudentListName5);
        studentNameTextList.Add(StudentListName6);

        studentButtonList.Add(StudentListButton1);
        studentButtonList.Add(StudentListButton2);
        studentButtonList.Add(StudentListButton3);
        studentButtonList.Add(StudentListButton4);
        studentButtonList.Add(StudentListButton5);
        studentButtonList.Add(StudentListButton6);

        CheckedStudentImgList.Add(SelectedStudentImage1);
        CheckedStudentImgList.Add(SelectedStudentImage2);
        CheckedStudentImgList.Add(SelectedStudentImage3);
        CheckedStudentImgList.Add(SelectedStudentImage4);
        CheckedStudentImgList.Add(SelectedStudentImage5);
        CheckedStudentImgList.Add(SelectedStudentImage6);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 준비 된 이벤트의 화자 이미지를 넣기 위한 것
    public void CheckNowScenarioScriptData(string nowSpeaker, string NowEventScript, Student nowTarget = null)
    {
        if (FindCorrectSpeakerSprite(nowSpeaker, nowTarget) == null)
        {
            ScenarioSpeakerImage.sprite = FindCorrectSpeakerSprite("NoneTemp");
        }
        else
        {
            ScenarioSpeakerImage.sprite = FindCorrectSpeakerSprite(nowSpeaker, nowTarget);
        }

        TempScenarioSpeakerText.text = nowSpeaker;
        EventScenarioText.text = NowEventScript;
    }

    // 스타트 시나리오 패널이 뜨고, 버튼을 눌렀을 때 이루어질 행동
    // public void IfClickScenarioPanelNextButton(string nowSpeaker, string NowEventScript)
    // {
    //     TempScenarioSpeakerText.text = nowSpeaker;
    //     EventScenarioText.text = NowEventScript;
    // }

    // 단순실행 이벤트 타임바 지날 때 보일 텍스트
    public void CheckTimeBarScriptText(SplitScript nowEventScript, int timeBarScriptTime)
    {
        string select1 = nowEventScript.SplitEventScriptTextSelect1.ToString();
        string select2 = nowEventScript.SplitEventScriptTextSelect2.ToString();
        string select3 = nowEventScript.SplitEventScriptTextSelect3.ToString();


        // 
        // 1초마다 대사를 바꿔가며 보여주게 하기?
        // 시간 체크를 하는 변수가 있고, 그 변수가 돌때마다 시간을 받아서 체크
    }

    // 필요한 돈 표시
    public void ReadyEventRewardText(OptionEventPay OptionEventNeedPay)
    {
        Option1NeedPayText.text = "0";
        Option2NeedPayText.text = "0";
        Option3NeedPayText.text = "0";


        // TODO :: 보상 다시 만지기 -> 보상 이름이 아닌 이미지로 넣어야함
        if (OptionEventNeedPay.SelectPay1 != 0)
        {
            // Option1NeedPayImage = ;
            Option1NeedPayText.text = "- " + OptionEventNeedPay.PayAmount1.ToString();
        }

        if (OptionEventNeedPay.SelectPay2 != 0)
        {
            // Option1NeedPayImage = ;
            Option2NeedPayText.text = "- " + OptionEventNeedPay.PayAmount2.ToString();
        }

        if (OptionEventNeedPay.SelectPay3 != 0)
        {
            // Option1NeedPayImage = ;
            Option3NeedPayText.text = "- " + OptionEventNeedPay.PayAmount3.ToString();
        }
    }

    // 선택지이벤트 패널의 텍스트 넣기
    public void ReadyOptionChoicePanelText(string nowSpeaker, string nowEventScript, string[] OptionScript, RewardInfo[] nowrewardInfo, Student nowTarget = null)
    {
        Option1Box.SetActive(true);
        Option2Box.SetActive(true);
        Option3Box.SetActive(true);

        // OptionEventSpeakerImage = Image;     -> 알맞은 화자 이미지 넣기 -> 알맞은 이미지 찾는 함수 필요할듯
        OptionEventSpeakerImage.sprite = FindCorrectSpeakerSprite(nowSpeaker, nowTarget);
        TempOptionEventSpeakerText.text = nowSpeaker;

        string TextSplit = nowEventScript; // .Split('/');
        NowEventScriptText.text = TextSplit;

        // NowEventScriptText.text = TextSplit[TextSplit.Length - 1];

        if (OptionScript[2] != "")
        {
            #region 과거의 옵션 마다의 보상내용
            // 선택지의 보상
            // if (nowrewardInfo[1].RewardID == 0)
            // {
            //     //  nowrewardInfo[4].RewardTakerName -> 원래 보상받는 주체를 적어주려 했는데 이거 삭제. rewardName에 포함
            //     Option1RewardText.text = nowrewardInfo[0].RewardName + " " + nowrewardInfo[0].RewardAmount;
            // }
            // else
            // {
            //     Option1RewardText.text = nowrewardInfo[0].RewardName + " " + nowrewardInfo[0].RewardAmount +
            //     ", \n" + nowrewardInfo[1].RewardTakerName + " - " + nowrewardInfo[1].RewardName + " " + nowrewardInfo[1].RewardAmount;
            // }
            // 
            // if (nowrewardInfo[3].RewardID == 0)
            // {
            //     Option2RewardText.text = nowrewardInfo[2].RewardName + " " + nowrewardInfo[2].RewardAmount;
            // }
            // else
            // {
            //     Option2RewardText.text = nowrewardInfo[2].RewardName + " " + nowrewardInfo[2].RewardAmount +
            //    ", \n" + nowrewardInfo[3].RewardTakerName + " - " + nowrewardInfo[3].RewardName + " " + nowrewardInfo[3].RewardAmount;
            // }
            // 
            // if (nowrewardInfo[5].RewardID == 0)
            // {
            //     Option3RewardText.text = nowrewardInfo[4].RewardName + " " + nowrewardInfo[4].RewardAmount;
            // }
            // else
            // {
            //     Option3RewardText.text = nowrewardInfo[4].RewardName + " " + nowrewardInfo[4].RewardAmount +
            //      ", \n" + nowrewardInfo[5].RewardTakerName + " - " + nowrewardInfo[5].RewardName + " " + nowrewardInfo[5].RewardAmount;
            // }
            #endregion

            //선택지의 대사
            Option1ScriptText.text = OptionScript[0];
            Option2ScriptText.text = OptionScript[1];
            Option3ScriptText.text = OptionScript[2];

            // 선택지의 소모재화

        }
        else if (OptionScript[1] != "")
        {
            Option1ScriptText.text = OptionScript[0];
            Option2ScriptText.text = OptionScript[1];
            Option3Box.SetActive(false);
        }
        else if (OptionScript[0] != "")
        {
            Option1ScriptText.text = OptionScript[0];

            Option1Box.SetActive(false);
            Option2Box.SetActive(false);
            Option3Box.SetActive(false);
        }
        else            // 오류 처리용
        {
            Debug.Log("오류 처리용");
        }

        Debug.Log("선택이벤트 디버그");
    }

    // 현재 선택지들의 소모재화량보다 나의 소지금액이 적을 때 유아이를 선택하지 못하도록 하기
    public void IfHaveLowMoneyNow(OptionEventPay NeedeventPay, int optionNum)
    {
        Color Normalcolor = new Color32(0, 0, 0, 255);

        Option1Button.interactable = true;
        Option2Button.interactable = true;
        Option3Button.interactable = true;

        switch (optionNum)
        {
            case 1:
                {
                    if (PlayerInfo.Instance.m_MyMoney <= NeedeventPay.PayAmount1)
                    {
                        Option1Button.interactable = false;

                        Color color = new Color32(255, 0, 0, 255);
                        Option1NeedPayText.color = color;
                    }
                    else
                    {
                        Option1NeedPayText.color = Normalcolor;
                    }
                }
                break;
            case 2:
                {
                    if (PlayerInfo.Instance.m_MyMoney <= NeedeventPay.PayAmount1)
                    {
                        Option2Button.interactable = false;

                        Color color = new Color32(255, 0, 0, 255);
                        Option2NeedPayText.color = color;
                    }
                    else
                    {
                        Option2NeedPayText.color = Normalcolor;
                    }
                }
                break;
            case 3:
                {
                    if (PlayerInfo.Instance.m_MyMoney <= NeedeventPay.PayAmount1)
                    {
                        Option3Button.interactable = false;

                        Color color = new Color32(255, 0, 0, 255);
                        Option3NeedPayText.color = color;
                    }
                    else
                    {
                        Option3NeedPayText.color = Normalcolor;
                    }
                }
                break;
        }


    }

    public void ReadyReawrdPanelText(RewardInfo[] reward)
    {
        Option1Object.SetActive(true);
        Option2Object.SetActive(true);
        Option3Object.SetActive(true);

        // RewardHeadlineText.text = "보상 획득!";
        // 보상 3개 -> 레이아웃 그룹 묶기, 보상 갯수에 따라 UI가 가운데에 위치하도록, 
        Reward1Name.text = "1";
        Reward2Name.text = "2";
        Reward3Name.text = "3";

        Reward1Amount.text = "";
        Reward2Amount.text = "";
        Reward3Amount.text = "";

        if (reward[2].RewardID != 0)            // 보상 3개
        {
            Reward1Name.text = reward[0].RewardName;
            Reward2Name.text = reward[1].RewardName;
            Reward3Name.text = reward[2].RewardName;

            Reward1Amount.text = reward[0].RewardAmount.ToString();
            Reward2Amount.text = reward[1].RewardAmount.ToString();
            Reward3Amount.text = reward[2].RewardAmount.ToString();

            Reward1Image.sprite = ReadyEventRewardImgCheck(Reward1Name.text);
            Reward2Image.sprite = ReadyEventRewardImgCheck(Reward2Name.text);
            Reward3Image.sprite = ReadyEventRewardImgCheck(Reward3Name.text);

            // Option1NeedPayImage = rew                // 이벤트 이미지 넣기 -> 이미지를 찾아서 넣어주는 것도 필요할듯
        }
        else if (reward[1].RewardID != 0)       // 보상 2개
        {
            Reward1Name.text = reward[0].RewardName;
            Reward2Name.text = reward[1].RewardName;

            Reward1Amount.text = reward[0].RewardAmount.ToString();
            Reward2Amount.text = reward[1].RewardAmount.ToString();

            Reward1Image.sprite = ReadyEventRewardImgCheck(Reward1Name.text);
            Reward2Image.sprite = ReadyEventRewardImgCheck(Reward2Name.text);

            Option3Object.SetActive(false);
        }
        else if (reward[0].RewardID != 0)       // 보상 1개
        {
            Reward1Name.text = reward[0].RewardName;

            Reward1Amount.text = reward[0].RewardAmount.ToString();

            Reward1Image.sprite = ReadyEventRewardImgCheck(Reward1Name.text);

            Option2Object.SetActive(false);
            Option3Object.SetActive(false);
        }
        else            // 오류 처리용
        {
            Debug.Log("오류 처리용");
        }
    }

    public Sprite ReadyEventRewardImgCheck(string rewardName)
    {
        Sprite tempSprite;

        switch (rewardName)
        {
            case "골드":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Gold];
                    return tempSprite;
                }
            case "루비":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Rubby];
                    return tempSprite;
                }
            case "운영점수":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Management];
                    return tempSprite;
                }
            case "유명점수":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Awareness];
                    return tempSprite;
                }
            case "활동점수":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Activity];
                    return tempSprite;
                }
            case "인재양성":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.TalentDevelopment];
                    return tempSprite;
                }
            // 교사
            case "교사-체력":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.ProfessorHealth];
                    return tempSprite;
                }
            case "교사-열정":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.ProfessorPassion];
                    return tempSprite;
                }
            case "교사-경험치":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.ProfessorExperience];
                    return tempSprite;
                }
            // 학생
            case "학생-체력":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Health];
                    return tempSprite;
                }
            case "학생-열정":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Passion];
                    return tempSprite;
                }
            case "학생-감각":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Sense];
                    return tempSprite;
                }
            case "학생-집중":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Concentration];
                    return tempSprite;
                }
            case "학생-재치":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Wit];
                    return tempSprite;
                }
            case "학생-기술":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Technique];
                    return tempSprite;
                }
            case "학생-통찰":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Insight];
                    return tempSprite;
                }

            case "학생-액션":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Action];
                    return tempSprite;
                }
            case "학생-시뮬레이션":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Simulation];
                    return tempSprite;
                }
            case "학생-어드벤처":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Adventure];
                    return tempSprite;
                }
            case "학생-슈팅":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Shooting];
                    return tempSprite;
                }
            case "학생-RPG":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.RPG];
                    return tempSprite;
                }
            case "학생-퍼즐":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Puzzle];
                    return tempSprite;
                }
            case "학생-리듬":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Rhythm];
                    return tempSprite;
                }
            case "학생-스포츠":
                {
                    tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Sports];
                    return tempSprite;
                }
        }

        return null;
    }


    public void ReadyTeacherUnLockPanel(RewardInfo[] reward)
    {
        Option1Object.SetActive(true);
        Option2Object.SetActive(false);
        Option3Object.SetActive(false);

        Reward1Name.text = "강사 해금!";
        Reward1Amount.text = reward[0].RewardName;

        foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
        {
            if (reward[0].RewardName == temp.ToString())
            {
                Reward1Image.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
            }
        }

        Option1Object.SetActive(true);
        Option2Object.SetActive(false);
        Option3Object.SetActive(false);
    }

    public void ReadyCompanyUnLockPanel(RewardInfo[] reward, InJaeRecommend NowCompanyList)
    {
        Option1Object.SetActive(true);
        Option2Object.SetActive(false);
        Option3Object.SetActive(false);

        Reward1Name.text = "회사 해금!";
        Reward1Amount.text = reward[0].RewardName;

        //foreach (var temp in NowCompanyList.m_CompanyList)
        //{
        //    if (reward[0].RewardName == temp.CompanyName)
        //    {
        //        // 회사 이미지 -> 임의의 이미지. 회사 아이콘 나오면 아이콘으로 교체
        //        Reward1Image.sprite = UISpriteLists.Instance.GetEmotionEmojiSpriteList[0];
        //    }
        //}

        Option1Object.SetActive(true);
        Option2Object.SetActive(false);
        Option3Object.SetActive(false);
    }

    // 인자 초기값 공부하기 - 선택적 매개변수
    public void PopUpTargetEventPanelSetData(Student student = null)
    {
        if (student != null)
        {
            StudentImage.sprite = student.StudentProfileImg;
            // StudentImage.sprite = FindCorrectSpeakerSprite(student.m_StudentStat.m_StudentName);

            StudentNameText.text = student.m_StudentStat.m_StudentName;

            HealthText.text = student.m_StudentStat.m_Health.ToString();
            PassionText.text = student.m_StudentStat.m_Passion.ToString();

            SenseText.text = student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense].ToString();
            WitText.text = student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit].ToString();
            ConcentrationText.text = student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration].ToString();
            TechniqueText.text = student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique].ToString();
            InsightText.text = student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight].ToString();

            PuzzleText.text = student.m_StudentStat.m_GenreAmountList[(int)GenreStat.Puzzle].ToString();
            SimulationText.text = student.m_StudentStat.m_GenreAmountList[(int)GenreStat.Simulation].ToString();
            RhythmText.text = student.m_StudentStat.m_GenreAmountList[(int)GenreStat.Rhythm].ToString();
            AdventureText.text = student.m_StudentStat.m_GenreAmountList[(int)GenreStat.Adventure].ToString();
            RPGText.text = student.m_StudentStat.m_GenreAmountList[(int)GenreStat.RPG].ToString();
            SportsText.text = student.m_StudentStat.m_GenreAmountList[(int)GenreStat.Sports].ToString();
            ActionText.text = student.m_StudentStat.m_GenreAmountList[(int)GenreStat.Action].ToString();
            ShootingText.text = student.m_StudentStat.m_GenreAmountList[(int)GenreStat.Shooting].ToString();

            for (int i = 0; i < studentNameTextList.Count; i++)
            {
                CheckedStudentImgList[i].gameObject.SetActive(false);

                if (studentNameTextList[i].text == student.m_StudentStat.m_StudentName)
                {
                    CheckedStudentImgList[i].gameObject.SetActive(true);       // 체크되었다고 표시
                }
            }

        }
        else
        {
            StudentImage.sprite = null;
            StudentNameText.text = "";

            HealthText.text = "";
            PassionText.text = "";
            SenseText.text = "";
            WitText.text = "";
            ConcentrationText.text = "";
            TechniqueText.text = "";
            InsightText.text = "";

            PuzzleText.text = "";
            SimulationText.text = "";
            RhythmText.text = "";
            AdventureText.text = "";
            RPGText.text = "";
            SportsText.text = "";
            ActionText.text = "";
            ShootingText.text = "";

        }
    }

    // 타겟이벤트의 정보창을 
    public void SetTargetEventInfo(RewardInfo[] nowEventReward, string eventTerm)
    {
        // PositionText.text = noweventTerm;
        TermText.text = eventTerm;

        // 만약 강사 or 회사  해금 정보가 true이면 그 페널을 띄운다.

        if (nowEventReward[2].RewardID != 0)            // 보상 3개
        {
            string reward1 = nowEventReward[0].RewardName + " + " + nowEventReward[0].RewardAmount.ToString() + "\n";
            string reward2 = nowEventReward[1].RewardName + " + " + nowEventReward[1].RewardAmount.ToString() + "\n";
            string reward3 = nowEventReward[2].RewardName + " + " + nowEventReward[2].RewardAmount.ToString();

            RewardText.text = reward1 + ", " + reward2 + ", " + reward3;
        }
        else if (nowEventReward[1].RewardID != 0)       // 보상 2개
        {
            string reward1 = nowEventReward[0].RewardName + " + " + nowEventReward[0].RewardAmount.ToString() + "\n";
            string reward2 = nowEventReward[1].RewardName + " + " + nowEventReward[1].RewardAmount.ToString();

            RewardText.text = reward1 + ", " + reward2;
        }
        else if (nowEventReward[0].RewardID != 0)       // 보상 1개
        {
            string reward1 = nowEventReward[0].RewardName + " + " + nowEventReward[0].RewardAmount.ToString();

            RewardText.text = reward1;
        }
        else            // 오류 처리용
        {
            Debug.Log("오류 처리용");
        }
    }

    // 학생 리스트에 데이터를 채워넣기 위한 작업
    public void TargetEventStudentListSetData(/*Image[] studentImg, string[] studentName*/List<NowDepartmentStudentList> nowstudentList)
    {
        int studentCount = ObjectManager.Instance.m_StudentList.Count;
        int StudentListCount = 0;

        for (int i = 0; i < studentImgList.Count; i++)
        {
            studentImgList[StudentListCount].sprite = nowstudentList[i].studentImg;                     // 이미지는 나중에 생긴다면
            studentNameTextList[StudentListCount].text = nowstudentList[i].studentName;                 // 학생 이름
            studentButtonList[StudentListCount].gameObject.name = nowstudentList[i].studentName;        // 버튼 이름 학생 이름으로 변경

            StudentListCount += 1;
        }
    }

    // 팝업, 팝오프
    public void PopUpStartEventScriptPanel()
    {
        PopUpEvenScenarioPanel.TurnOnUI();
    }

    public void PopOffStartEventScriptPanel()
    {
        PopOffEvenScenarioPanel.TurnOffUI();
    }

    public void PopUpTimeBarPanel()
    {
        PopUpSimpleExecutionTimeBarPanel.TurnOnUI();
    }

    public void PopOffTimeBarPanel()
    {
        PopOffSimpleExecutionTimeBarPanel.TurnOffUI();
    }

    public void PopUpOptionChoiceEvent()
    {
        PopUpOptionChoiceEventPanel.TurnOnUI();
    }

    public void PopOffOptionChoiceEvent()
    {
        PopOffOptionChoiceEventPanel.TurnOffUI();
    }

    public void PopUpTargetSelectEvent()
    {
        PopUpTargetSelectEventPanel.TurnOnUI();
    }

    public void PopOffTargetSelectEvent()
    {
        PopOffTargetSelectEventPanel.TurnOffUI();
    }

    public void PopUpRewardPanel()
    {
        PopUpRewardEventPanel.TurnOnUI();
    }

    public void PopOffRewardPanel()
    {
        PopOffRewardEventPanel.TurnOffUI();
    }

    public Sprite FindCorrectSpeakerSprite(string nowSpeaker, Student nowTarget = null)
    {
        //선택된 학생
        if (nowSpeaker == ESpeakerImgIndex.SelectedStu.ToString())
        {
            if (nowTarget != null)
            {
                for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
                {
                    if (nowTarget.m_StudentStat.m_StudentName == ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentName)
                    {
                        Sprite tempSprite = ObjectManager.Instance.m_StudentList[i].StudentProfileImg;

                        return tempSprite;
                    }
                }
            }
        }
        else if (nowSpeaker == ESpeakerImgIndex.DesignRan.ToString())
        {
            List<Student> tempStudentList = new List<Student>();
            System.Random temRandom = new System.Random();

            // 학생은 안나눠져있고 전체이기 때문에 학과 별로 임시 리스트를 만들어준다.
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                {
                    tempStudentList.Add(ObjectManager.Instance.m_StudentList[i]);
                }
            }

            int IndexNum = temRandom.Next(tempStudentList.Count);     // 학생리스트의 인덱스를 뽑아서 그 학생의 아이디 찾기

            Sprite tempSprite = ObjectManager.Instance.m_StudentList[IndexNum].StudentProfileImg;

            return tempSprite;
        }
        else if (nowSpeaker == ESpeakerImgIndex.ArtRan.ToString())
        {
            List<Student> tempStudentList = new List<Student>();
            System.Random temRandom = new System.Random();

            // 학생은 안나눠져있고 전체이기 때문에 학과 별로 임시 리스트를 만들어준다.
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.Art)
                {
                    tempStudentList.Add(ObjectManager.Instance.m_StudentList[i]);
                }
            }

            int IndexNum = temRandom.Next(tempStudentList.Count);     // 학생리스트의 인덱스를 뽑아서 그 학생의 아이디 찾기

            Sprite tempSprite = ObjectManager.Instance.m_StudentList[IndexNum].StudentProfileImg;

            return tempSprite;


        }
        else if (nowSpeaker == ESpeakerImgIndex.ProRan.ToString())
        {
            List<Student> tempStudentList = new List<Student>();
            System.Random temRandom = new System.Random();

            // 학생은 안나눠져있고 전체이기 때문에 학과 별로 임시 리스트를 만들어준다.
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.Programming)
                {
                    tempStudentList.Add(ObjectManager.Instance.m_StudentList[i]);
                }
            }

            int IndexNum = temRandom.Next(tempStudentList.Count);     // 학생리스트의 인덱스를 뽑아서 그 학생의 아이디 찾기

            Sprite tempSprite = ObjectManager.Instance.m_StudentList[IndexNum].StudentProfileImg;

            return tempSprite;
        }
        else
        {
            foreach (var tempSprite in UISpriteLists.Instance.GetspeakerSpriteList)
            {
                if (tempSprite.name == nowSpeaker)
                {
                    return tempSprite;
                }
            }
        }

        return null;
    }
}
