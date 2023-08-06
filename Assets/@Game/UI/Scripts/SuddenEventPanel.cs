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
/// ���� �̺�Ʈ���� UI �гε��� ���� ���� �� Ŭ����.
/// UI ����, �ʱ�ȭ�Լ�, ���� ���
/// </summary>
public class SuddenEventPanel : MonoBehaviour
{
    // [Space(5f)]
    [Header("���� �̺�Ʈ �� - ���ð��� �̺�Ʈ ������")]         // ���� ����
    [SerializeField] private EventScheduleUI MyEventScheduleUI;             // ���� �̺�Ʈ �� �� �ʿ��ҵ�
    [SerializeField] private PopUpUI PopUpEventScheduleUI;
    [SerializeField] private PopOffUI PopOffEventScheduleUI;
    // --------------------
    [Space(10f)]
    [Header("���� �̺�Ʈ �� - ù��° �˾� �� �ó����� ������")]
    [SerializeField] private Button ScenarioPanelNextButton;

    [SerializeField] private PopUpUI PopUpEvenScenarioPanel;                 // ��� �̺�Ʈ�� ù��°�� �� ������
    [SerializeField] private PopOffUI PopOffEvenScenarioPanel;

    [Space(5f)]
    [SerializeField] private Image ScenarioSpeakerImageMask;
    [SerializeField] private Image ScenarioSpeakerImage;

    [SerializeField] private TextMeshProUGUI EventScenarioText;

    // [SerializeField] private Button TempScenarioPanelCloseButton;
    // --------------------
    [Space(10f)]
    [Header("���� �̺�Ʈ �� - �б� Ÿ�� ������ ������ �̺�Ʈ Ÿ��")]
    [Header("�ܼ����� �̺�Ʈ")]
    [SerializeField] private GameObject SimpleExecutionTimeBarPanel;      // �ܼ����� �̺�Ʈ UI �г�
    [SerializeField] private PopUpUI PopUpSimpleExecutionTimeBarPanel;      // �ܼ����� �̺�Ʈ UI �г�
    [SerializeField] private PopOffUI PopOffSimpleExecutionTimeBarPanel;
    [Space(5f)]
    [SerializeField] private Image SimpleExecutionEventImage;               // �ܼ����� Ÿ�ӹ� ���� �� �̹���(����?)
    [SerializeField] private TextMeshProUGUI SimpleExecutionEventText;      // �ܼ����� Ÿ�ӹ� ���� �� �ؽ�Ʈ
    [SerializeField] private Slider SimpleExecutionSlideBar;                // �����̵�� ���
    [SerializeField] private Image SimpleExecutionFillBar;                  // ������ Ÿ�ӿ� ���� ������ �����̵��

    // [SerializeField] private Button TempSimpleExecutionEventPanelCloseButton;
    [Space(10f)]
    [Header("������ �̺�Ʈ")]
    [SerializeField] private PopUpUI PopUpOptionChoiceEventPanel;           // ������ �̺�Ʈ UI �г�
    [SerializeField] private PopOffUI PopOffOptionChoiceEventPanel;
    [Space(5f)]
    [SerializeField] private Image OptionEventSpeakerImage;                 // ������ �̺�Ʈ ȭ��
    [SerializeField] private TextMeshProUGUI TempOptionEventSpeakerText;    // �ӽ� ���ϴ� ��� �̸�
    [SerializeField] private TextMeshProUGUI NowEventScriptText;            // �ӽ� ���ϴ� ��� �̸�
    [Space(5f)]
    [SerializeField] private GameObject Option1Box;                         // ������ 1�� ������Ʈ
    [SerializeField] private Button Option1Button;                          // ������ 1�� ��ư
    [SerializeField] private TextMeshProUGUI Option1ScriptText;             // ������ 1�� ��ũ��Ʈ
    [SerializeField] private TextMeshProUGUI Option1RewardText;             // ������ 1 ���� ����
    [SerializeField] private Image Option1NeedPayImage;                     // �ʿ� �� �̹���
    [SerializeField] private TextMeshProUGUI Option1NeedPayText;            // �ʿ� �� �ؽ�Ʈ
    [Space(5f)]
    [SerializeField] private GameObject Option2Box;                         // ������ 2�� ������Ʈ
    [SerializeField] private Button Option2Button;                          // ������ 2�� ��ư
    [SerializeField] private TextMeshProUGUI Option2ScriptText;             // ������ 2�� ��ũ��Ʈ
    [SerializeField] private TextMeshProUGUI Option2RewardText;             // ������ 2 ���� ����
    [SerializeField] private Image Option2NeedPayImage;                     // �ʿ� �� �̹���
    [SerializeField] private TextMeshProUGUI Option2NeedPayText;            // �ʿ� �� �ؽ�Ʈ
    [Space(5f)]
    [SerializeField] private GameObject Option3Box;                         // ������ 3�� ������Ʈ
    [SerializeField] private Button Option3Button;                          // ������ 3�� ��ư
    [SerializeField] private TextMeshProUGUI Option3ScriptText;             // ������ 3�� ��ũ��Ʈ
    [SerializeField] private TextMeshProUGUI Option3RewardText;             // ������ 3 ���� ����
    [SerializeField] private Image Option3NeedPayImage;                     // �ʿ� �� �̹���
    [SerializeField] private TextMeshProUGUI Option3NeedPayText;            // �ʿ� �� �ؽ�Ʈ
    [Space(5f)]
    [SerializeField] private Button GiveUpSelectEventButton;
    // [SerializeField] private Button TempOptionChoiceEventPanelCloseButton;

    [Space(10f)]
    [Header("����� �̺�Ʈ")]
    [SerializeField] private PopUpUI PopUpTargetSelectEventPanel;           // ����� �̺�Ʈ UI �г�
    [SerializeField] private PopOffUI PopOffTargetSelectEventPanel;
    [Space(5f)]
    [Header("�����л� �̸� ����")]
    [SerializeField] private Image StudentImage;
    [SerializeField] private TextMeshProUGUI StudentNameText;
    [Space(5f)]
    [Header("���� �л��� ����")]
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private TextMeshProUGUI PassionText;
    [SerializeField] private TextMeshProUGUI SenseText;
    [SerializeField] private TextMeshProUGUI WitText;
    [SerializeField] private TextMeshProUGUI ConcentrationText;
    [SerializeField] private TextMeshProUGUI TechniqueText;
    [SerializeField] private TextMeshProUGUI InsightText;
    [Space(5f)]
    [Header("���� �л��� �帣����")]
    [SerializeField] private TextMeshProUGUI PuzzleText;
    [SerializeField] private TextMeshProUGUI SimulationText;
    [SerializeField] private TextMeshProUGUI RhythmText;
    [SerializeField] private TextMeshProUGUI AdventureText;
    [SerializeField] private TextMeshProUGUI RPGText;
    [SerializeField] private TextMeshProUGUI SportsText;
    [SerializeField] private TextMeshProUGUI ActionText;
    [SerializeField] private TextMeshProUGUI ShootingText;
    [Space(5f)]
    [Header("�̺�Ʈ�� ����")]
    [SerializeField] private TextMeshProUGUI PositionText;
    [SerializeField] private TextMeshProUGUI TermText;
    [SerializeField] private TextMeshProUGUI RewardText;
    [Space(5f)]
    [Header("�л���� �� �� �ε���")]
    [SerializeField] private Button GameDesignerIndex;
    [SerializeField] private Button ArtIndex;
    [SerializeField] private Button ProgrammerIndex;
    [Space(5f)]
    [Header("�л���ϸ���Ʈ")]
    [SerializeField] private Transform StudentListParent;

    [SerializeField] private Image StudentListImage1;
    [SerializeField] private Image StudentListNameTagImage1;
    [SerializeField] private TextMeshProUGUI StudentListName1;
    [SerializeField] private Button StudentListButton1;
    [SerializeField] private Image SelectedStudentImage1;

    [SerializeField] private Image StudentListImage2;
    [SerializeField] private Image StudentListNameTagImage2;
    [SerializeField] private TextMeshProUGUI StudentListName2;
    [SerializeField] private Button StudentListButton2;
    [SerializeField] private Image SelectedStudentImage2;

    [SerializeField] private Image StudentListImage3;
    [SerializeField] private Image StudentListNameTagImage3;
    [SerializeField] private TextMeshProUGUI StudentListName3;
    [SerializeField] private Button StudentListButton3;
    [SerializeField] private Image SelectedStudentImage3;

    [SerializeField] private Image StudentListImage4;
    [SerializeField] private Image StudentListNameTagImage4;
    [SerializeField] private TextMeshProUGUI StudentListName4;
    [SerializeField] private Button StudentListButton4;
    [SerializeField] private Image SelectedStudentImage4;

    [SerializeField] private Image StudentListImage5;
    [SerializeField] private Image StudentListNameTagImage5;
    [SerializeField] private TextMeshProUGUI StudentListName5;
    [SerializeField] private Button StudentListButton5;
    [SerializeField] private Image SelectedStudentImage5;

    [SerializeField] private Image StudentListImage6;
    [SerializeField] private Image StudentListNameTagImage6;
    [SerializeField] private TextMeshProUGUI StudentListName6;
    [SerializeField] private Button StudentListButton6;
    [SerializeField] private Image SelectedStudentImage6;

    [Space(5f)]
    [Header("����� �̺�Ʈ ���ÿϷ� & �����ϱ� ��ư")]
    [SerializeField] private Button TargetSetOkButton;
    [SerializeField] private Button GiveUpTargetEventButton;

    [SerializeField] private Button TempTargetSelectEventPanelCloseButton;
    // --------------------
    [Space(10f)]
    [Header("���� �̺�Ʈ �� - �̺�Ʈ �� ���� â")]
    [SerializeField] private PopUpUI PopUpRewardEventPanel;                 // �̺�Ʈ���� UI �г�
    [SerializeField] private PopOffUI PopOffRewardEventPanel;

    [SerializeField] private Image RewardEventPanel;                        // ���� ������ ���� �̺�Ʈ�г�

    [Space(5f)]
    [SerializeField] private TextMeshProUGUI RewardHeadlineText;
    [Space(5f)]
    [SerializeField] private GameObject Option1Object;                      // ���� 1
    [SerializeField] private TextMeshProUGUI Reward1Name;
    [SerializeField] private Image Reward1Image;
    [SerializeField] private TextMeshProUGUI Reward1Amount;

    [Space(5f)]
    [SerializeField] private GameObject Option2Object;                      // ���� 2
    [SerializeField] private TextMeshProUGUI Reward2Name;
    [SerializeField] private Image Reward2Image;
    [SerializeField] private TextMeshProUGUI Reward2Amount;
    [Space(5f)]
    [SerializeField] private GameObject Option3Object;                      // ���� 3
    [SerializeField] private TextMeshProUGUI Reward3Name;
    [SerializeField] private Image Reward3Image;
    [SerializeField] private TextMeshProUGUI Reward3Amount;
    [Space(5f)]
    [SerializeField] private Button SaveRewardButton;

    // --------------------
    List<Image> studentImgList = new List<Image>();
    List<Image> studentNameTagImgList = new List<Image>();
    List<TextMeshProUGUI> studentNameTextList = new List<TextMeshProUGUI>();
    List<Button> studentButtonList = new List<Button>();

    List<Image> CheckedStudentImgList = new List<Image>();
    // --------------------

    int tempNowSpeakerImgIndex = 0;     // 1 -> ��ũ��Ʈ �̹���, 2 -> �ɼǼ��� �̺�Ʈ �̹���

    public int RandomImgIndex = 0;

    Color SelectedDarkColor;

    public int GettempNowSpeakerImgIndex
    {
        get { return tempNowSpeakerImgIndex; }
        set { tempNowSpeakerImgIndex = value; }
    }

    public Button GetScenarioPanelNextButton
    {
        get { return ScenarioPanelNextButton; }
    }

    public Image GetSimpleExecutionEventImage
    {
        get { return SimpleExecutionEventImage; }
        set { SimpleExecutionEventImage = value; }
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

    public Transform GetStudentListParent
    {
        get { return StudentListParent; }
        set { StudentListParent = value; }
    }

    // �� ģ������ �̺�Ʈ��ũ��Ʈ�Ŵ����� �Űܾ� �ϳ�
    void Start()
    {
        // ���⼭ ��� ����Ŀ �̹����� �ε�����.
        // LoadAllSpeakerImg();

        // TempScenarioPanelCloseButton.onClick.AddListener(PopOffStartEventScriptPanel);
        // TempSimpleExecutionEventPanelCloseButton.onClick.AddListener(PopOffTimeBarPanel);
        // TempOptionChoiceEventPanelCloseButton.onClick.AddListener(PopOffOptionChoiceEvent);
        // TempTargetSelectEventPanelCloseButton.onClick.AddListener(PopOffTargetSelectEvent);
        // GetRewardButton.onClick.AddListener(PopOffRewardPanel);

        SelectedDarkColor = new Color(0.55f, 0.55f, 0.55f, 1f);

        ColorBlock tempGDColor = GameDesignerIndex.colors;
        tempGDColor.normalColor = SelectedDarkColor;
        tempGDColor.selectedColor = SelectedDarkColor;
        GameDesignerIndex.colors = tempGDColor;

        ColorBlock tempArtColor = ArtIndex.colors;
        tempArtColor.normalColor = new Color(1f, 1f, 1f, 1f);
        tempGDColor.selectedColor = new Color(1f, 1f, 1f, 1f);
        ArtIndex.colors = tempArtColor;

        ColorBlock tempProColor = ProgrammerIndex.colors;
        tempProColor.normalColor = new Color(1f, 1f, 1f, 1f);
        tempGDColor.selectedColor = new Color(1f, 1f, 1f, 1f);
        ProgrammerIndex.colors = tempProColor;

        studentImgList.Add(StudentListImage1);
        studentImgList.Add(StudentListImage2);
        studentImgList.Add(StudentListImage3);
        studentImgList.Add(StudentListImage4);
        studentImgList.Add(StudentListImage5);
        studentImgList.Add(StudentListImage6);

        studentNameTagImgList.Add(StudentListNameTagImage1);
        studentNameTagImgList.Add(StudentListNameTagImage2);
        studentNameTagImgList.Add(StudentListNameTagImage3);
        studentNameTagImgList.Add(StudentListNameTagImage4);
        studentNameTagImgList.Add(StudentListNameTagImage5);
        studentNameTagImgList.Add(StudentListNameTagImage6);

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

    // �غ� �� �̺�Ʈ�� ȭ�� �̹����� �ֱ� ���� ��
    public void CheckNowScenarioScriptData(int num, string nowSpeaker, string NowEventScript, Student nowTarget = null)
    {
        if (FindCorrectSpeakerSprite(num, nowSpeaker, nowTarget) == null)
        {
            // ȭ���� �̹����� ��� �����Ƿ翧�� �־��ٶ�
            ScenarioSpeakerImage.sprite = FindCorrectSpeakerSprite(num, "NoneTemp");
        }
        else
        {
            // �л�, ȭ�� �� �̹��� ã�Ƽ� �־��ֱ� ���� �κ�
            ScenarioSpeakerImage.sprite = FindCorrectSpeakerSprite(num, nowSpeaker, nowTarget);
        }

        EventScenarioText.text = NowEventScript;
    }

    // ��ŸƮ �ó����� �г��� �߰�, ��ư�� ������ �� �̷���� �ൿ
    // public void IfClickScenarioPanelNextButton(string nowSpeaker, string NowEventScript)
    // {
    //     TempScenarioSpeakerText.text = nowSpeaker;
    //     EventScenarioText.text = NowEventScript;
    // }

    // �ܼ����� �̺�Ʈ Ÿ�ӹ� ���� �� ���� �ؽ�Ʈ
    public void CheckTimeBarScriptText(SplitScript nowEventScript, int timeBarScriptTime)
    {
        string select1 = nowEventScript.SplitEventScriptTextSelect1.ToString();
        string select2 = nowEventScript.SplitEventScriptTextSelect2.ToString();
        string select3 = nowEventScript.SplitEventScriptTextSelect3.ToString();


        // 
        // 1�ʸ��� ��縦 �ٲ㰡�� �����ְ� �ϱ�?
        // �ð� üũ�� �ϴ� ������ �ְ�, �� ������ �������� �ð��� �޾Ƽ� üũ
    }

    // �ʿ��� �� ǥ��
    public void ReadyEventRewardText(OptionEventPay OptionEventNeedPay)
    {
        //RewardEventPanel.GetComponent<AudioSource>().clip = ClickEventManager.Instance.Sound.PlayFailSound;

        // TODO :: ���� �ٽ� ������ -> ���� �̸��� �ƴ� �̹����� �־����
        if (OptionEventNeedPay.SelectPay1 != 0)
        {
            string tempAmount;
            tempAmount = string.Format("{0:#,###}", OptionEventNeedPay.PayAmount1);

            // Option1NeedPayImage = ;
            Option1NeedPayText.text = "- " + tempAmount;
        }
        else
        {
            Option1NeedPayText.text = "0";
        }

        if (OptionEventNeedPay.SelectPay2 != 0)
        {
            string tempAmount;
            tempAmount = string.Format("{0:#,###}", OptionEventNeedPay.PayAmount2);

            // Option1NeedPayImage = ;
            Option2NeedPayText.text = "- " + tempAmount;
        }
        else
        {
            Option2NeedPayText.text = "0";
        }

        if (OptionEventNeedPay.SelectPay3 != 0)
        {
            string tempAmount;
            tempAmount = string.Format("{0:#,###}", OptionEventNeedPay.PayAmount3);

            // Option1NeedPayImage = ;
            Option3NeedPayText.text = "- " + tempAmount;
        }
        else
        {
            Option3NeedPayText.text = "0";
        }
    }

    // �������̺�Ʈ �г��� �ؽ�Ʈ �ֱ�
    public void ReadyOptionChoicePanelText(string nowSpeaker, string nowEventScript, string[] OptionScript, RewardInfo[] nowrewardInfo, Student nowTarget = null)
    {
        tempNowSpeakerImgIndex = 2;

        Option1Box.SetActive(true);
        Option2Box.SetActive(true);
        Option3Box.SetActive(true);

        // OptionEventSpeakerImage = Image;     -> �˸��� ȭ�� �̹��� �ֱ� -> �˸��� �̹��� ã�� �Լ� �ʿ��ҵ�
        // OptionEventSpeakerImage.sprite = FindCorrectSpeakerSprite(2, nowSpeaker, nowTarget);

        if (FindCorrectSpeakerSprite(2, nowSpeaker, nowTarget) == null)
        {
            // ȭ���� �̹����� ��� �����Ƿ翧�� �־��ٶ�
            OptionEventSpeakerImage.sprite = FindCorrectSpeakerSprite(2, "NoneTemp");
        }
        else
        {
            // �л�, ȭ�� �� �̹��� ã�Ƽ� �־��ֱ� ���� �κ�
            OptionEventSpeakerImage.sprite = FindCorrectSpeakerSprite(2, nowSpeaker, nowTarget);
        }

        TempOptionEventSpeakerText.text = nowSpeaker;

        string TextSplit = nowEventScript; // .Split('/');
        NowEventScriptText.text = TextSplit;

        if (OptionScript[2] != "")
        {
            #region ������ �ɼ� ������ ���󳻿�
            // �������� ����
            // if (nowrewardInfo[1].RewardID == 0)
            // {
            //     //  nowrewardInfo[4].RewardTakerName -> ���� ����޴� ��ü�� �����ַ� �ߴµ� �̰� ����. rewardName�� ����
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

            //�������� ���
            Option1ScriptText.text = OptionScript[0];
            Option2ScriptText.text = OptionScript[1];
            Option3ScriptText.text = OptionScript[2];

            // �������� �Ҹ���ȭ

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
        else            // ���� ó����
        {
            Debug.Log("���� ó����");
        }

        Debug.Log("�����̺�Ʈ �����");
    }

    // ���� ���������� �Ҹ���ȭ������ ���� �����ݾ��� ���� �� �����̸� �������� ���ϵ��� �ϱ�
    public void IfHaveLowMoneyNow(OptionEventPay NeedeventPay, int optionNum)
    {
        Color Normalcolor = new Color32(0, 0, 0, 255);

        // Option1Button.interactable = true;
        // Option2Button.interactable = true;
        // Option3Button.interactable = true;

        switch (optionNum)
        {
            case 1:
            {
                if (NeedeventPay.SelectPay1 == 100)
                {
                    if (PlayerInfo.Instance.MyMoney < NeedeventPay.PayAmount1)
                    {
                        Option1Button.interactable = false;

                        Color color = new Color32(255, 0, 0, 255);
                        Option1NeedPayText.color = color;
                    }
                    else
                    {
                        Option1Button.interactable = true;
                        Option1NeedPayText.color = Normalcolor;
                    }
                }
                else
                {
                    Option1Button.interactable = true;
                    Option1NeedPayText.color = Normalcolor;
                }
            }
            break;
            case 2:
            {
                if (NeedeventPay.SelectPay2 == 100)
                {
                    if (PlayerInfo.Instance.MyMoney < NeedeventPay.PayAmount1)
                    {
                        Option2Button.interactable = false;

                        Color color = new Color32(255, 0, 0, 255);
                        Option2NeedPayText.color = color;
                    }
                    else
                    {
                        Option2Button.interactable = true;
                        Option2NeedPayText.color = Normalcolor;
                    }
                }
                else
                {
                    Option2Button.interactable = true;
                    Option2NeedPayText.color = Normalcolor;
                }
            }
            break;
            case 3:
            {
                if (NeedeventPay.SelectPay3 == 100)
                {
                    if (PlayerInfo.Instance.MyMoney < NeedeventPay.PayAmount1)
                    {
                        Option3Button.interactable = false;

                        Color color = new Color32(255, 0, 0, 255);
                        Option3NeedPayText.color = color;
                    }
                    else
                    {
                        Option3Button.interactable = true;
                        Option3NeedPayText.color = Normalcolor;
                    }
                }
                else
                {
                    Option3Button.interactable = true;
                    Option3NeedPayText.color = Normalcolor;
                }
            }
            break;
        }
    }

    // �̺�Ʈ ���� ���� ���� - �� ���ö��� fail ���� ������ֱ� ���� �Լ�
    public void FindSoundRewardResult()
    {
        // ������ 3��
        if (Reward3Amount.text != "")
        {
            if (Reward1Amount.text.Contains("-") && Reward2Amount.text.Contains("-") && Reward3Amount.text.Contains("-"))
            {
                ClickEventManager.Instance.Sound.PlayFailSound();
            }
            else
            {
                ClickEventManager.Instance.Sound.PlayMoneyJackpotSound();
            }
        }
        // ������ 2��
        else if (Reward2Amount.text != "")
        {
            if (Reward1Amount.text.Contains("-") && Reward2Amount.text.Contains("-"))
            {
                ClickEventManager.Instance.Sound.PlayFailSound();
            }
            else
            {
                ClickEventManager.Instance.Sound.PlayMoneyJackpotSound();
            }
        }
        else if (Reward1Amount.text != "")
        {
            if (Reward1Amount.text.Contains("-"))
            {
                ClickEventManager.Instance.Sound.PlayFailSound();
            }
            else
            {
                ClickEventManager.Instance.Sound.PlayMoneyJackpotSound();
            }
        }
    }

    public void ReadyReawrdPanelText(RewardInfo[] reward)
    {
        Option1Object.SetActive(true);
        Option2Object.SetActive(true);
        Option3Object.SetActive(true);

        // RewardHeadlineText.text = "���� ȹ��!";
        // ���� 3�� -> ���̾ƿ� �׷� ����, ���� ������ ���� UI�� ����� ��ġ�ϵ���, 
        Reward1Name.text = "1";
        Reward2Name.text = "2";
        Reward3Name.text = "3";

        Reward1Amount.text = "";
        Reward2Amount.text = "";
        Reward3Amount.text = "";

        Vector2 newsize = new Vector2(167, 167);

        Reward1Image.rectTransform.sizeDelta = newsize;
        Reward2Image.rectTransform.sizeDelta = newsize;
        Reward3Image.rectTransform.sizeDelta = newsize;

        if (reward[2].RewardID != 0)            // ���� 3��
        {
            Reward1Name.text = reward[0].RewardName;
            Reward2Name.text = reward[1].RewardName;
            Reward3Name.text = reward[2].RewardName;

            string tempAmount1;
            tempAmount1 = string.Format("{0:#,###}", reward[0].RewardAmount);
            string tempAmount2;
            tempAmount2 = string.Format("{0:#,###}", reward[1].RewardAmount);
            string tempAmount3;
            tempAmount3 = string.Format("{0:#,###}", reward[2].RewardAmount);

            Reward1Amount.text = tempAmount1;
            Reward2Amount.text = tempAmount2;
            Reward3Amount.text = tempAmount3;

            Reward1Image.sprite = ReadyEventRewardImgCheck(Reward1Name.text);
            Reward2Image.sprite = ReadyEventRewardImgCheck(Reward2Name.text);
            Reward3Image.sprite = ReadyEventRewardImgCheck(Reward3Name.text);

            // Option1NeedPayImage = rew                // �̺�Ʈ �̹��� �ֱ� -> �̹����� ã�Ƽ� �־��ִ� �͵� �ʿ��ҵ�
        }
        else if (reward[1].RewardID != 0)       // ���� 2��
        {
            Reward1Name.text = reward[0].RewardName;
            Reward2Name.text = reward[1].RewardName;

            string tempAmount1;
            tempAmount1 = string.Format("{0:#,###}", reward[0].RewardAmount);
            string tempAmount2;
            tempAmount2 = string.Format("{0:#,###}", reward[1].RewardAmount);

            Reward1Amount.text = tempAmount1;
            Reward2Amount.text = tempAmount2;

            Reward1Image.sprite = ReadyEventRewardImgCheck(Reward1Name.text);
            Reward2Image.sprite = ReadyEventRewardImgCheck(Reward2Name.text);

            Option3Object.SetActive(false);
        }
        else if (reward[0].RewardID != 0)       // ���� 1��
        {
            Reward1Name.text = reward[0].RewardName;

            string tempAmount1;
            tempAmount1 = string.Format("{0:#,###}", reward[0].RewardAmount);

            Reward1Amount.text = tempAmount1;

            Reward1Image.sprite = ReadyEventRewardImgCheck(Reward1Name.text);

            Option2Object.SetActive(false);
            Option3Object.SetActive(false);
        }
        else            // ���� ó����
        {
            Debug.Log("���� ó����");
        }
    }

    public Sprite ReadyEventRewardImgCheck(string rewardName)
    {
        Sprite tempSprite;

        switch (rewardName)
        {
            case "���":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Gold];
                return tempSprite;
            }
            case "���":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Rubby];
                return tempSprite;
            }
            case "�����":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Management];
                return tempSprite;
            }
            case "��������":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Awareness];
                return tempSprite;
            }
            case "Ȱ������":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Activity];
                return tempSprite;
            }
            case "����缺":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.TalentDevelopment];
                return tempSprite;
            }
            // ���� (+ ü��, ����)
            case "����ġ":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.ProfessorExperience];
                return tempSprite;
            }
            // �л�
            case "ü��":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Health];
                return tempSprite;
            }
            case "����":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Passion];
                return tempSprite;
            }
            case "����":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Sense];
                return tempSprite;
            }
            case "����":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Concentration];
                return tempSprite;
            }
            case "��ġ":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Wit];
                return tempSprite;
            }
            case "���":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Technique];
                return tempSprite;
            }
            case "����":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Insight];
                return tempSprite;
            }

            case "�׼�":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Action];
                return tempSprite;
            }
            case "�ùķ��̼�":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Simulation];
                return tempSprite;
            }
            case "��庥ó":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Adventure];
                return tempSprite;
            }
            case "����":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Shooting];
                return tempSprite;
            }
            case "RPG":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.RPG];
                return tempSprite;
            }
            case "����":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Puzzle];
                return tempSprite;
            }
            case "����":
            {
                tempSprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Rhythm];
                return tempSprite;
            }
            case "������":
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

        Reward1Name.text = "���� �ر�!";
        Reward1Amount.text = reward[0].RewardName;

        Vector2 newSize = new Vector2(167, 200);
        Reward1Image.rectTransform.sizeDelta = newSize;

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

        Reward1Name.text = "ȸ�� �ر�!";
        Reward1Amount.text = reward[0].RewardName;

        Reward1Image.sprite = UISpriteLists.Instance.GetRewardSpriteList[(int)ERewardImg.Management];
        //foreach (var temp in NowCompanyList.m_CompanyList)
        //{
        //    if (reward[0].RewardName == temp.CompanyName)
        //    {
        //        // ȸ�� �̹��� -> ������ �̹���. ȸ�� ������ ������ ���������� ��ü
        //        Reward1Image.sprite = UISpriteLists.Instance.GetEmotionEmojiSpriteList[0];
        //    }
        //}

        Option1Object.SetActive(true);
        Option2Object.SetActive(false);
        Option3Object.SetActive(false);
    }

    // Ÿ���̺�Ʈ - �̺�Ʈ â�� ���õ� ���� �л��� ���� �����͸� ��Ÿ���� ���� �κ�
    public void PopUpTargetEventPanelSetData(Student student = null)
    {
        if (student != null)
        {
            ColorBlock tempGDColor = GameDesignerIndex.colors;
            GameDesignerIndex.colors = tempGDColor;

            ColorBlock tempArtColor = ArtIndex.colors;
            ArtIndex.colors = tempArtColor;

            ColorBlock tempProColor = ProgrammerIndex.colors;
            ProgrammerIndex.colors = tempProColor;

            StudentImage.sprite = student.StudentProfileImg;
            StudentImage.gameObject.SetActive(true);

            if (student.m_StudentStat.m_UserSettingName != "")
            {
                StudentNameText.text = student.m_StudentStat.m_UserSettingName;
            }
            else
            {
                StudentNameText.text = student.m_StudentStat.m_StudentName;
            }

            HealthText.text = student.m_StudentStat.m_Health.ToString();
            PassionText.text = student.m_StudentStat.m_Passion.ToString();

            SenseText.text = student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Sense].ToString();
            WitText.text = student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Wit].ToString();
            ConcentrationText.text = student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Concentration].ToString();
            TechniqueText.text = student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Technique].ToString();
            InsightText.text = student.m_StudentStat.m_AbilityAmountArr[(int)AbilityType.Insight].ToString();

            PuzzleText.text = student.m_StudentStat.m_GenreAmountArr[(int)GenreStat.Puzzle].ToString();
            SimulationText.text = student.m_StudentStat.m_GenreAmountArr[(int)GenreStat.Simulation].ToString();
            RhythmText.text = student.m_StudentStat.m_GenreAmountArr[(int)GenreStat.Rhythm].ToString();
            AdventureText.text = student.m_StudentStat.m_GenreAmountArr[(int)GenreStat.Adventure].ToString();
            RPGText.text = student.m_StudentStat.m_GenreAmountArr[(int)GenreStat.RPG].ToString();
            SportsText.text = student.m_StudentStat.m_GenreAmountArr[(int)GenreStat.Sports].ToString();
            ActionText.text = student.m_StudentStat.m_GenreAmountArr[(int)GenreStat.Action].ToString();
            ShootingText.text = student.m_StudentStat.m_GenreAmountArr[(int)GenreStat.Shooting].ToString();

            for (int i = 0; i < studentNameTextList.Count; i++)
            {
                CheckedStudentImgList[i].gameObject.SetActive(false);

                if (studentNameTextList[i].text == student.m_StudentStat.m_StudentName)
                {
                    CheckedStudentImgList[i].gameObject.SetActive(true);       // üũ�Ǿ��ٰ� ǥ��
                }
            }

        }
    }

    public void InitTargetEventEachStudentInfo()
    {
        StudentImage.sprite = null;
        StudentImage.gameObject.SetActive(false);
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

    // Ÿ���̺�Ʈ�� ����â�� 
    public void SetTargetEventInfo(RewardInfo[] nowEventReward, string eventTerm, string dispatchSpot)
    {
        //GameDesignerIndex.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 96);
        //ArtIndex.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 80);
        //ProgrammerIndex.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 80);

        PositionText.text = dispatchSpot;
        TermText.text = eventTerm;

        // ���� ���� or ȸ��  �ر� ������ true�̸� �� ����� ����.

        if (nowEventReward[2].RewardID != 0)            // ���� 3��
        {
            string reward1 = nowEventReward[0].RewardName + " + " + nowEventReward[0].RewardAmount.ToString();
            string reward2 = nowEventReward[1].RewardName + " + " + nowEventReward[1].RewardAmount.ToString();
            string reward3 = nowEventReward[2].RewardName + " + " + nowEventReward[2].RewardAmount.ToString();

            RewardText.text = reward1 + ", " + reward2 + ", " + reward3;
        }
        else if (nowEventReward[1].RewardID != 0)       // ���� 2��
        {
            string reward1 = nowEventReward[0].RewardName + " + " + nowEventReward[0].RewardAmount.ToString();
            string reward2 = nowEventReward[1].RewardName + " + " + nowEventReward[1].RewardAmount.ToString();

            RewardText.text = reward1 + ", " + reward2;
        }
        else if (nowEventReward[0].RewardID != 0)       // ���� 1��
        {
            string reward1 = nowEventReward[0].RewardName + " + " + nowEventReward[0].RewardAmount.ToString();

            RewardText.text = reward1;
        }
        else            // ���� ó����
        {
            Debug.Log("���� ó����");
        }
    }

    // �л� ����Ʈ�� �����͸� ä���ֱ� ���� �۾�
    public void TargetEventStudentListSetData(List<NowDepartmentStudentList> nowstudentList)
    {
        int studentCount = ObjectManager.Instance.m_StudentList.Count;
        int StudentListCount = 0;

        Sprite tempNameTagSprite;

        if (nowstudentList[0].type == StudentType.GameDesigner)
        {
            tempNameTagSprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
        }
        else if (nowstudentList[0].type == StudentType.Art)
        {
            tempNameTagSprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];
        }
        else if (nowstudentList[0].type == StudentType.Programming)
        {
            tempNameTagSprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];
        }
        else        // �⺻...?
        {
            tempNameTagSprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
        }

        for (int i = 0; i < studentImgList.Count; i++)
        {
            if (nowstudentList[i].ChangedName != "")
            {
                studentNameTextList[StudentListCount].text = nowstudentList[i].ChangedName;                 // �л� �̸�
            }
            else
            {
                studentNameTextList[StudentListCount].text = nowstudentList[i].studentName;                 // �л� �̸�
            }
            studentImgList[StudentListCount].sprite = nowstudentList[i].studentImg;                     // �̹����� ���߿� ����ٸ�

            studentNameTagImgList[StudentListCount].sprite = tempNameTagSprite;
            studentButtonList[StudentListCount].gameObject.name = nowstudentList[i].studentName;        // ��ư �̸� �л� �̸����� ����

            StudentListCount += 1;
        }
    }

    // �˾�, �˿���
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

    public Sprite FindCorrectSpeakerSprite(int num, string nowSpeaker, Student nowTarget = null)
    {
        int ScenarioImgrectX = 400;
        int ScenarioImgrectY = 500;

        int OptionImgX = 300;
        int OptionImgY = 375;

        //���õ� �л�
        if (nowSpeaker == ESpeakerImgIndex.SelectedStu.ToString())
        {
            if (nowTarget != null)
            {
                for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
                {
                    if (nowTarget.m_StudentStat.m_StudentName == ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentName)
                    {
                        Sprite tempSprite = ObjectManager.Instance.m_StudentList[i].StudentProfileImg;

                        if (num == 1)        // �ó����� ���ö� �л� �̹���
                        {
                            Vector2 newSize = new Vector2(ScenarioImgrectX, ScenarioImgrectY);
                            ScenarioSpeakerImage.rectTransform.sizeDelta = newSize;
                            ScenarioSpeakerImageMask.rectTransform.sizeDelta = newSize;

                            ScenarioSpeakerImageMask.rectTransform.localPosition = new Vector3(-422, 0, 0);

                            ScenarioSpeakerImageMask.enabled = true;
                        }
                        else if (num == 2)   // �ɼ� ���� ���ö� �л� �̹���
                        {
                            Vector2 newSize = new Vector2(OptionImgX, OptionImgY);
                            OptionEventSpeakerImage.rectTransform.sizeDelta = newSize;

                            OptionEventSpeakerImage.rectTransform.localPosition = new Vector3(-347, 120, 0);
                        }

                        return tempSprite;
                    }
                }
            }
        }
        else if (nowSpeaker == ESpeakerImgIndex.DesignRan.ToString())
        {
            List<Student> tempStudentList = new List<Student>();
            System.Random temRandom = new System.Random();

            // �л��� �ȳ������ְ� ��ü�̱� ������ �а� ���� �ӽ� ����Ʈ�� ������ش�.
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                {
                    tempStudentList.Add(ObjectManager.Instance.m_StudentList[i]);
                }
            }

            if (RandomImgIndex == 0)
            {
                int IndexNum = temRandom.Next(tempStudentList.Count);     // �л�����Ʈ�� �ε����� �̾Ƽ� �� �л��� ���̵� ã��
                RandomImgIndex = IndexNum;
            }

            Sprite tempSprite = ObjectManager.Instance.m_StudentList[RandomImgIndex].StudentProfileImg;

            if (num == 1)        // �ó����� ���ö� �л� �̹���
            {
                Vector2 newSize = new Vector2(ScenarioImgrectX, ScenarioImgrectY);
                ScenarioSpeakerImage.rectTransform.sizeDelta = newSize;
                ScenarioSpeakerImageMask.rectTransform.sizeDelta = newSize;

                ScenarioSpeakerImageMask.rectTransform.localPosition = new Vector3(-422, 0, 0);

                ScenarioSpeakerImageMask.enabled = true;
            }
            else if (num == 2)   // �ɼ� ���� ���ö� �л� �̹���
            {
                Vector2 newSize = new Vector2(OptionImgX, OptionImgY);
                OptionEventSpeakerImage.rectTransform.sizeDelta = newSize;

                OptionEventSpeakerImage.rectTransform.localPosition = new Vector3(-347, 120, 0);
            }

            return tempSprite;
        }
        else if (nowSpeaker == ESpeakerImgIndex.ArtRan.ToString())
        {
            List<Student> tempStudentList = new List<Student>();
            System.Random temRandom = new System.Random();

            // �л��� �ȳ������ְ� ��ü�̱� ������ �а� ���� �ӽ� ����Ʈ�� ������ش�.
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.Art)
                {
                    tempStudentList.Add(ObjectManager.Instance.m_StudentList[i]);
                }
            }

            if (RandomImgIndex == 0)
            {
                int IndexNum = temRandom.Next(tempStudentList.Count);     // �л�����Ʈ�� �ε����� �̾Ƽ� �� �л��� ���̵� ã��
                RandomImgIndex = IndexNum;
            }

            Sprite tempSprite = ObjectManager.Instance.m_StudentList[RandomImgIndex].StudentProfileImg;

            if (num == 1)        // �ó����� ���ö� �л� �̹���
            {
                Vector2 newSize = new Vector2(ScenarioImgrectX, ScenarioImgrectY);
                ScenarioSpeakerImage.rectTransform.sizeDelta = newSize;
                ScenarioSpeakerImageMask.rectTransform.sizeDelta = newSize;

                ScenarioSpeakerImageMask.rectTransform.localPosition = new Vector3(-422, 0, 0);

                ScenarioSpeakerImageMask.enabled = true;
            }
            else if (num == 2)   // �ɼ� ���� ���ö� �л� �̹���
            {
                Vector2 newSize = new Vector2(OptionImgX, OptionImgY);
                OptionEventSpeakerImage.rectTransform.sizeDelta = newSize;

                OptionEventSpeakerImage.rectTransform.localPosition = new Vector3(-347, 120, 0);
            }

            return tempSprite;
        }
        else if (nowSpeaker == ESpeakerImgIndex.ProRan.ToString())
        {
            List<Student> tempStudentList = new List<Student>();
            System.Random temRandom = new System.Random();

            // �л��� �ȳ������ְ� ��ü�̱� ������ �а� ���� �ӽ� ����Ʈ�� ������ش�.
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType == StudentType.Programming)
                {
                    tempStudentList.Add(ObjectManager.Instance.m_StudentList[i]);
                }
            }

            if (RandomImgIndex == 0)
            {
                int IndexNum = temRandom.Next(tempStudentList.Count);     // �л�����Ʈ�� �ε����� �̾Ƽ� �� �л��� ���̵� ã��
                RandomImgIndex = IndexNum;
            }

            Sprite tempSprite = ObjectManager.Instance.m_StudentList[RandomImgIndex].StudentProfileImg;

            if (num == 1)        // �ó����� ���ö� �л� �̹���
            {
                Vector2 newSize = new Vector2(ScenarioImgrectX, ScenarioImgrectY);
                ScenarioSpeakerImage.rectTransform.sizeDelta = newSize;
                ScenarioSpeakerImageMask.rectTransform.sizeDelta = newSize;

                ScenarioSpeakerImageMask.rectTransform.localPosition = new Vector3(-422, 0, 0);

                ScenarioSpeakerImageMask.enabled = true;
            }
            else if (num == 2)   // �ɼ� ���� ���ö� �л� �̹���
            {
                Vector2 newSize = new Vector2(OptionImgX, OptionImgY);
                OptionEventSpeakerImage.rectTransform.sizeDelta = newSize;

                OptionEventSpeakerImage.rectTransform.localPosition = new Vector3(-347, 120, 0);

            }

            return tempSprite;
        }
        else            // �����л�, ���õ� �л� �� �ƴ� ��
        {
            foreach (var tempSprite in UISpriteLists.Instance.GetspeakerSpriteList)
            {
                if (tempSprite.name == nowSpeaker)
                {
                    Vector2 newSize;

                    if (num == 1)        // �ó����� ���ö� ȭ�� �̹���
                    {
                        if (nowSpeaker == "Gletter")
                        {
                            newSize = new Vector2(800, 800);
                            ScenarioSpeakerImageMask.rectTransform.localPosition = new Vector3(-522, -40, 0);
                        }
                        else
                        {
                            newSize = new Vector2(1000, 1000);
                            ScenarioSpeakerImageMask.rectTransform.localPosition = new Vector3(-522, -40, 0);
                        }

                        ScenarioSpeakerImage.rectTransform.sizeDelta = newSize;
                        ScenarioSpeakerImageMask.rectTransform.sizeDelta = newSize;

                        ScenarioSpeakerImageMask.enabled = false;
                    }
                    else if (num == 2)   // �ɼ� ���� ���ö� ȭ�� �̹���
                    {
                        newSize = new Vector2(500, 500);
                        OptionEventSpeakerImage.rectTransform.sizeDelta = newSize;

                        OptionEventSpeakerImage.rectTransform.localPosition = new Vector3(-347, 40, 0);
                    }
                    return tempSprite;
                }
            }
        }
        return null;
    }
}
