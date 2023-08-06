using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// 2023.03.08 ������ �гο� ���õ� �����յ�
/// 
/// ocean
/// </summary>
public class GameJamSelect_Panel : MonoBehaviour
{
    static Color RedColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    static Color GreyColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    [SerializeField] private GameObject m_GameJamCanvas;                // ������ ���� ���� �� ������ ������ ����� ĵ����
    [SerializeField] private GameObject m_RecruitNoticePanel;           // ���� ���� ������ ���ִ� ��������� ����� �г�
    [SerializeField] private GameObject m_RecruitNoticeInfoPanel;       // Ŭ���� �������� �������� ����� �г�
    [SerializeField] private GameObject m_AwardHistory;                 // ���� ���󳻿��� �ִٸ� ���ֱ�
    [SerializeField] private GameObject m_GameDesignerSecondImage;
    [SerializeField] private GameObject m_ArtSecondImage;
    [SerializeField] private GameObject m_ProgrammingSecondImage;
    [SerializeField] private GameObject m_SelectStudentInfo;
    [SerializeField] private GameObject m_SelectStudent;
    [SerializeField] private GameObject m_EntryButton;

    [Space(5f)]
    [Header("�а��� �л��� ����â")]
    [SerializeField] private GameObject m_GameDesignerNoneObj;
    [SerializeField] private GameObject m_ArtNoneObj;
    [SerializeField] private GameObject m_ProgrammingNoneObj;
    [SerializeField] private GameObject m_GameDesignerSelectCheckBox;
    [SerializeField] private GameObject m_ArtSelectCheckBox;
    [SerializeField] private GameObject m_ProgrammingSelectCheckBox;
    [SerializeField] private GameObject m_GameDesignerStudentPanel;
    [SerializeField] private GameObject m_ArtStudentPanel;
    [SerializeField] private GameObject m_ProgrammingStudentPanel;
    [Space(5f)]

    [SerializeField] private GameObject m_SelectStudentParentObj;       // �л� ������Ʈ���� �θ� ������Ʈ
    [SerializeField] private GameObject m_GameJamListParentObj;         // ������ ����Ʈ���� �θ� ������Ʈ
    [SerializeField] private GameObject m_WarningPanel;
    [SerializeField] private GameObject m_CheckParticipationPanel;
    [SerializeField] private GameObject m_GameJamEntryPanel;
    [SerializeField] private GameObject m_CompleteEntryPanel;
    [SerializeField] private GameObject m_GameJamSelectWarning;
    [SerializeField] private TextMeshProUGUI m_WarningPanelText;

    [SerializeField] private Button m_BackButton;                       // ���� ������ �ߴ� ȭ���� �����ִ� ��ư
    [SerializeField] private Button m_HomeButton;                       // ���� ó���� �ΰ��� ȭ���� �����ִ� ��ư
    [SerializeField] private Button m_InformationButton;                // �������� ������ �����ִ� �г��� ������ ��ư
    [SerializeField] private Button m_StartButton;
    [SerializeField] private Button m_SelectCompleteButton;         // �л��� �� �������� �� ���ÿϷ� ��ư�� Ȱ��ȭ ������� �Ѵ�.

    [SerializeField] private Transform m_RecruitNoticePanelContent;
    [SerializeField] private Transform m_SelectStudentInfoContent;

    [SerializeField] private TextMeshProUGUI m_CurrentMoney;
    [SerializeField] private TextMeshProUGUI m_CurrentSpecialPoint;
    [SerializeField] private TextMeshProUGUI m_RecruitNoticeInfoPanelTitle;
    [SerializeField] private TextMeshProUGUI m_GameJameName;
    [SerializeField] private TextMeshProUGUI m_RewardMoney;
    [SerializeField] private TextMeshProUGUI m_Awareness;
    [SerializeField] private TextMeshProUGUI m_Date;
    [SerializeField] private TextMeshProUGUI m_Health;
    [SerializeField] private TextMeshProUGUI m_Passion;
    [SerializeField] private TextMeshProUGUI m_EntryFee;
    [SerializeField] private Image m_GameDesignerStudentImage;
    [SerializeField] private TextMeshProUGUI m_GameDesignerStudentName;
    [SerializeField] private TextMeshProUGUI m_GameDesignerStudentHealth;
    [SerializeField] private TextMeshProUGUI m_GameDesignerStudentPassion;
    [SerializeField] private Image m_ArtStudentImage;
    [SerializeField] private TextMeshProUGUI m_ArtStudentName;
    [SerializeField] private TextMeshProUGUI m_ArtStudentHealth;
    [SerializeField] private TextMeshProUGUI m_ArtStudentPassion;
    [SerializeField] private Image m_ProgrammingStudentImage;
    [SerializeField] private TextMeshProUGUI m_ProgrammingStudentName;
    [SerializeField] private TextMeshProUGUI m_ProgrammingStudentHealth;
    [SerializeField] private TextMeshProUGUI m_ProgrammingStudentPassion;
    [SerializeField] private TextMeshProUGUI m_ExpectedGenre;
    [SerializeField] private TextMeshProUGUI m_CheckParticipationGameDesignerName;
    [SerializeField] private TextMeshProUGUI m_CheckParticipationArtName;
    [SerializeField] private TextMeshProUGUI m_CheckParticipationProgrammingName;
    [SerializeField] private Image m_CheckParticipationGameDesignerImage;
    [SerializeField] private Image m_CheckParticipationArtImage;
    [SerializeField] private Image m_CheckParticipationProgrammingImage;
    [SerializeField] private TextMeshProUGUI m_ExpectedSuccesPercent;
    [SerializeField] private TextMeshProUGUI m_StartButtonText;

    [SerializeField] private Image m_GameDesignerRequirementStatIcon1;
    [SerializeField] private Image m_GameDesignerRequirementStatIcon2;
    [SerializeField] private Image m_ArtRequirementStatIcon1;
    [SerializeField] private Image m_ArtRequirementStatIcon2;
    [SerializeField] private Image m_ProgrammingRequirementStatIcon1;
    [SerializeField] private Image m_ProgrammingRequirementStatIcon2;

    [Space(5f)]
    [Header("Ʃ�丮�� �뵵")]
    [SerializeField] private RectTransform m_SelectStudentRect;
    [SerializeField] private RectTransform m_FirstRect;
    [SerializeField] private RectTransform m_SecondRect;
    [SerializeField] private RectTransform m_ThirdRect;
    [SerializeField] private RectTransform m_PartInfoRect;
    [SerializeField] private RectTransform m_GameDesignerButton;
    [SerializeField] private RectTransform m_ArtButton;
    [SerializeField] private RectTransform m_ProgrammingButton;
    [SerializeField] private RectTransform m_ParticipationYesButton;
    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private RectTransform m_GameJamButtonRect;

    [Space(5f)]
    [Header("������ �������� �̹�����")]
    [SerializeField] private Image m_MainGenreImage;
    [SerializeField] private Image m_SubGenreImage;
    [SerializeField] private Image m_AwarenessArrowImage;
    [SerializeField] private Image m_HealthArrowImage;
    [SerializeField] private Image m_PassionArrowImage;

    [Space(5f)]
    [Header("���õ� ��ȹ �л��� ���� �̹���")]
    [SerializeField] private Image m_GameDesignerImageInsight;
    [SerializeField] private Image m_GameDesignerImageConcentraction;
    [SerializeField] private Image m_GameDesignerImageSense;
    [SerializeField] private Image m_GameDesignerImageTechnique;
    [SerializeField] private Image m_GameDesignerImageWit;

    [Space(5f)]
    [Header("���õ� ��Ʈ �л��� ���� �̹���")]
    [SerializeField] private Image m_ArtImageInsight;
    [SerializeField] private Image m_ArtImageConcentraction;
    [SerializeField] private Image m_ArtImageSense;
    [SerializeField] private Image m_ArtImageTechnique;
    [SerializeField] private Image m_ArtImageWit;

    [Space(5f)]
    [Header("���õ� �ù� �л��� ���� �̹���")]
    [SerializeField] private Image m_ProgrammingImageInsight;
    [SerializeField] private Image m_ProgrammingImageConcentraction;
    [SerializeField] private Image m_ProgrammingImageSense;
    [SerializeField] private Image m_ProgrammingImageTechnique;
    [SerializeField] private Image m_ProgrammingImageWit;
    [Space(5f)]

    [SerializeField] private Image m_GameDesignerSliderFillImage;
    [SerializeField] private Image m_ArtSliderFillImage;
    [SerializeField] private Image m_ProgrammingSliderFillImage;

    [Space(5f)]
    [Header("������ �����뿡�� �䱸�ϴ� �ּ� ������ ��Ÿ���� ��")]
    [SerializeField] private Image m_GameDesignerBar;
    [SerializeField] private Image m_ArtBar;
    [SerializeField] private Image m_ProgrammingBar;
    [Space(5f)]

    [SerializeField] private Slider m_GameDesignerFirstSlider;
    [SerializeField] private Slider m_GameDesignerSecondSlider;
    [SerializeField] private Slider m_ArtFirstSlider;
    [SerializeField] private Slider m_ArtSecondSlider;
    [SerializeField] private Slider m_ProgrammingFirstSlider;
    [SerializeField] private Slider m_ProgrammingSecondSlider;

    [Space(5f)]
    [Header("���õ� ��ȹ �л��� ���ȵ�")]
    [SerializeField] private Slider m_GameDesignerInsightSlider;
    [SerializeField] private Slider m_GameDesignerConcentractionSlider;
    [SerializeField] private Slider m_GameDesignerSenseSlider;
    [SerializeField] private Slider m_GameDesignerTechniqueSlider;
    [SerializeField] private Slider m_GameDesignerWitSlider;

    [Space(5f)]
    [Header("���õ� ��ȹ �л��� ���ȵ�")]
    [SerializeField] private Slider m_ArtInsightSlider;
    [SerializeField] private Slider m_ArtConcentractionSlider;
    [SerializeField] private Slider m_ArtSenseSlider;
    [SerializeField] private Slider m_ArtTechniqueSlider;
    [SerializeField] private Slider m_ArtWitSlider;

    [Space(5f)]
    [Header("���õ� ��ȹ �л��� ���ȵ�")]
    [SerializeField] private Slider m_ProgrammingInsightSlider;
    [SerializeField] private Slider m_ProgrammingConcentractionSlider;
    [SerializeField] private Slider m_ProgrammingSenseSlider;
    [SerializeField] private Slider m_ProgrammingTechniqueSlider;
    [SerializeField] private Slider m_ProgrammingWitSlider;

    [Space(5f)]
    [Header("�������� �����ϴ� �г��� ���ٰ� �Ѵ� �κ�")]
    [SerializeField] private PopUpUI m_TurnOnGameJamSelectPanel;
    [SerializeField] private PopOffUI m_TurnOffGameJamSelectPanel;

    [SerializeField] private Sprite m_SatisfyStat;
    [SerializeField] private Sprite m_StatisStat2;
    [SerializeField] private Sprite m_DisSatisfyStat;

    // ������ ����� �θ� �������� ������ �� �ְ� ���ش�.
    public Transform m_GameJamParent { get { return m_RecruitNoticePanelContent; } }
    public Transform m_StudentInfoParent { get { return m_SelectStudentInfoContent; } }
    public Button m_SetStartButton { get { return m_StartButton; } set { m_StartButton = value; } }
    public GameObject m_GameDesignerNone { get { return m_GameDesignerNoneObj; } set { m_GameDesignerNoneObj = value; } }
    public GameObject m_ArtNone { get { return m_ArtNoneObj; } set { m_ArtNoneObj = value; } }
    public GameObject m_ProgrammingNone { get { return m_ProgrammingNoneObj; } set { m_ProgrammingNoneObj = value; } }
    public GameObject GameDesignerStudentPanel { get { return m_GameDesignerStudentPanel; } set { m_GameDesignerStudentPanel = value; } }
    public GameObject ArtStudentPanel { get { return m_ArtStudentPanel; } set { m_ArtStudentPanel = value; } }
    public GameObject ProgrammingStudentPanel { get { return m_ProgrammingStudentPanel; } set { m_ProgrammingStudentPanel = value; } }

    public GameObject GameJamCanvas { get { return m_GameJamCanvas; } set { m_GameJamCanvas = value; } }

    public TextMeshProUGUI m_GameDesignerName { get { return m_GameDesignerStudentName; } }
    public TextMeshProUGUI m_ArtName { get { return m_ArtStudentName; } }
    public TextMeshProUGUI m_ProgrammingName { get { return m_ProgrammingStudentName; } }
    public TextMeshProUGUI m_ExpectedSuccesPercentgetter { get { return m_ExpectedSuccesPercent; } }
    public TextMeshProUGUI m_GetStartButtonText { get { return m_StartButtonText; } }

    public Image GameDesignerRequirementStatIcon1 { get { return m_GameDesignerRequirementStatIcon1; } set { m_GameDesignerRequirementStatIcon1 = value; } }
    public Image GameDesignerRequirementStatIcon2 { get { return m_GameDesignerRequirementStatIcon2; } set { m_GameDesignerRequirementStatIcon2 = value; } }
    public Image ArtRequirementStatIcon1 { get { return m_ArtRequirementStatIcon1; } set { m_ArtRequirementStatIcon1 = value; } }
    public Image ArtRequirementStatIcon2 { get { return m_ArtRequirementStatIcon2; } set { m_ArtRequirementStatIcon2 = value; } }
    public Image ProgrammingRequirementStatIcon1 { get { return m_ProgrammingRequirementStatIcon1; } set { m_ProgrammingRequirementStatIcon1 = value; } }
    public Image ProgrammingRequirementStatIcon2 { get { return m_ProgrammingRequirementStatIcon2; } set { m_ProgrammingRequirementStatIcon2 = value; } }
    public GameObject RecruitNoticePanel { get { return m_RecruitNoticePanel; } set { m_RecruitNoticePanel = value; } }
    public GameObject RecruitNoticeInfoPanel { get { return m_RecruitNoticeInfoPanel; } set { m_RecruitNoticeInfoPanel = value; } }
    public GameObject GameJamListParentObj { get { return m_GameJamListParentObj; } set { m_GameJamListParentObj = value; } }
    public RectTransform SelectStudentRect { get { return m_SelectStudentRect; } set { m_SelectStudentRect = value; } }
    public RectTransform FirstRect { get { return m_FirstRect; } set { m_FirstRect = value; } }
    public RectTransform SecondRect { get { return m_SecondRect; } set { m_SecondRect = value; } }
    public RectTransform ThirdRect { get { return m_ThirdRect; } set { m_ThirdRect = value; } }
    public RectTransform PartInfoRect { get { return m_PartInfoRect; } set { m_PartInfoRect = value; } }
    public RectTransform GameDesignerButton { get { return m_GameDesignerButton; } set { m_GameDesignerButton = value; } }
    public RectTransform ArtButton { get { return m_ArtButton; } set { m_ArtButton = value; } }
    public RectTransform ProgrammingButton { get { return m_ProgrammingButton; } set { m_ProgrammingButton = value; } }
    public Button SelectCompleteButton { get { return m_SelectCompleteButton; } set { m_SelectCompleteButton = value; } }
    public RectTransform ParticipationYesButton { get { return m_ParticipationYesButton; } set { m_ParticipationYesButton = value; } }
    public RectTransform GameJamButtonRect { get { return m_GameJamButtonRect; } set { m_GameJamButtonRect = value; } }
    public GameObject SelectStudentParentObj { get { return m_SelectStudentParentObj; } set { m_SelectStudentParentObj = value; } }

    
    // GameJamCanvas�� ���� �״� ���� �� �ִ� �Լ�
    public void SetActiveGameJamCanvas(bool _isActive = true)
    {
        if (_isActive)
        {
            m_TurnOnGameJamSelectPanel.TurnOnUI();
            SetUIMonryAndSpecialPoint();
        }
        else
        {
            m_TurnOffGameJamSelectPanel.TurnOffUI();
        }
    }

    public void SetActiveEntryCountWarningPanel(bool _isTrue, string _text)
    {
        m_GameJamSelectWarning.SetActive(_isTrue);
        m_WarningPanelText.text = _text;
    }

    // Ȩ��ư�� ������ �� ������ �ؾ��Ѵ�. �׸��� ó������ ��������Ѵ�.
    public void SetActiveHomeButton()
    {
        m_WarningPanel.SetActive(true);
    }

    public void ClickWarningYesButton()
    {
        m_WarningPanel.SetActive(false);
        ResetGameJamCanvas();
    }

    public void SetActivePartSelectedCheckBox(bool _gm, bool _art, bool _programming)
    {
        m_GameDesignerSelectCheckBox.SetActive(_gm);
        m_ArtSelectCheckBox.SetActive(_art);
        m_ProgrammingSelectCheckBox.SetActive(_programming);
    }

    public void BackButton()
    {
        if (m_SelectStudent.activeSelf == true)
        {
            m_RecruitNoticePanel.SetActive(true);
            //m_EntryFeePanel.SetActive(true);
            m_EntryButton.SetActive(false);
            m_SelectStudentInfo.SetActive(false);
            m_SelectStudent.SetActive(false);

            m_GameDesignerNoneObj.SetActive(true);
            m_ArtNoneObj.SetActive(true);
            m_ProgrammingNoneObj.SetActive(true);

            m_GameDesignerStudentPanel.SetActive(false);
            m_ArtStudentPanel.SetActive(false);
            m_ProgrammingStudentPanel.SetActive(false);

            m_GameDesignerSliderFillImage.sprite = m_StatisStat2;
            m_ArtSliderFillImage.sprite = m_StatisStat2;
            m_ProgrammingSliderFillImage.sprite = m_StatisStat2;
        }
        else
        {
            SetActiveHomeButton();
        }
    }

    public void ChangeStartButtonName(string _name)
    {
        m_StartButtonText.text = _name;
    }

    public void ChangeGenreSprite(Sprite _mainGenre, Sprite _subGenre)
    {
        m_MainGenreImage.sprite = _mainGenre;
        m_SubGenreImage.sprite = _subGenre;
    }

    public void ResetGameJamCanvas()
    {
        for (int i = 0; i < m_SelectStudentInfoContent.childCount; i++)
        {
            m_SelectStudentInfoContent.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

        m_ProgrammingStudentName.text = "";
        m_ArtStudentName.text = "";
        m_GameDesignerStudentName.text = "";
        m_ExpectedGenre.text = "???";
        m_ExpectedSuccesPercent.text = "???";


        //InGameUI.Instance.m_SecondContentUI.SetActive(false);

        m_SelectStudentInfo.SetActive(false);
        m_SelectStudent.SetActive(false);
        m_EntryButton.SetActive(false);
        m_GameDesignerSelectCheckBox.SetActive(false);
        m_ArtSelectCheckBox.SetActive(false);
        m_ProgrammingSelectCheckBox.SetActive(false);
        m_RecruitNoticePanel.SetActive(true);
        //m_EntryFeePanel.SetActive(true);

        m_GameDesignerNoneObj.SetActive(true);
        m_ArtNoneObj.SetActive(true);
        m_ProgrammingNoneObj.SetActive(true);

        m_GameDesignerStudentPanel.SetActive(false);
        m_ArtStudentPanel.SetActive(false);
        m_ProgrammingStudentPanel.SetActive(false);

        m_GameDesignerSliderFillImage.sprite = m_StatisStat2;
        m_ArtSliderFillImage.sprite = m_StatisStat2;
        m_ProgrammingSliderFillImage.sprite = m_StatisStat2;
        m_TurnOffGameJamSelectPanel.TurnOffUI();

        //m_GameDesignerFirstSlider.transform.GetChild(0).GetComponent<Image>().color = RedColor;
        //m_ArtFirstSlider.transform.GetChild(0).GetComponent<Image>().color = RedColor;
        //m_ProgrammingFirstSlider.transform.GetChild(0).GetComponent<Image>().color = RedColor;
    }

    public void ClickWarningNoButton()
    {
        m_WarningPanel.SetActive(false);
    }

    public void ClickSelectCompleteButton()
    {
        m_CheckParticipationPanel.SetActive(true);

        m_CheckParticipationGameDesignerName.text = m_GameDesignerStudentName.text;
        m_CheckParticipationGameDesignerImage.sprite = m_GameDesignerStudentImage.sprite;

        m_CheckParticipationArtName.text = m_ArtStudentName.text;
        m_CheckParticipationArtImage.sprite = m_ArtStudentImage.sprite;

        m_CheckParticipationProgrammingName.text = m_ProgrammingStudentName.text;
        m_CheckParticipationProgrammingImage.sprite = m_ProgrammingStudentImage.sprite;
    }

    public IEnumerator CloseParticipationPanel()
    {
        yield return new WaitForSecondsRealtime(3.5f);

        m_GameJamEntryPanel.SetActive(false);
        ResetGameJamCanvas();
    }

    public void ClickParticipationNoButton()
    {
        m_CheckParticipationPanel.SetActive(false);
    }

    public void ClickParticipationYesButton()
    {
        m_CheckParticipationPanel.SetActive(false);
        m_GameJamEntryPanel.SetActive(true);

        //if (PlayerInfo.Instance.IsFirstGameJam)
        //{
        //    m_TutorialPanel.SetActive(false);
        //    PlayerInfo.Instance.IsFirstGameJam = false;
        //}
    }

    public void ClickCloseGameJamEntryPanelButton()
    {
        m_EntryButton.SetActive(false);

        m_CompleteEntryPanel.SetActive(true);
    }

    // ���� UI�� �ִ� �����ݾ��� �Ȱ��� �־��ش�.
    public string SetCurrentMoney()
    {
        int _ingameMoney = int.Parse(InGameUI.Instance.m_nowMoney.text.Replace(",", ""));
        string _money = string.Format("{0:#,0}", _ingameMoney);
        //m_InGameUICurrentMoney.text = string.Format("{0:#,0}", _money);

        //string _money = InGameUI.Instance.m_nowMoney.text;

        m_CurrentMoney.text = _money;

        return _ingameMoney.ToString();
    }

    /// ���� �κк��� ��ġ��
    public void SetUIMonryAndSpecialPoint()
    {
        int _ingameMoney = int.Parse(InGameUI.Instance.m_nowMoney.text.Replace(",", ""));
        string _money = string.Format("{0:#,0}", _ingameMoney);

        int _ingameSpecialMoney = int.Parse(InGameUI.Instance.m_SpecialPoint.text.Replace(",", ""));
        string _specialMoney = string.Format("{0:#,0}", _ingameSpecialMoney);

        m_CurrentMoney.text = _money;
        m_CurrentSpecialPoint.text = _specialMoney;
    }

    public void SetActiveObj(GameObject _obj, bool _isTrue)
    {
        _obj.SetActive(_isTrue);
    }

    public void ResetSlider()
    {
        m_GameDesignerFirstSlider.value = 0;
        m_GameDesignerSecondSlider.value = 0;
        m_ArtFirstSlider.value = 0;
        m_ArtSecondSlider.value = 0;
        //m_ArtSecondSlider.value = 0;
        m_ProgrammingFirstSlider.value = 0;
        m_ProgrammingSecondSlider.value = 0;
        m_GameDesignerSliderFillImage.sprite = m_StatisStat2;
        m_ArtSliderFillImage.sprite = m_StatisStat2;
        m_ProgrammingSliderFillImage.sprite = m_StatisStat2;

    }

    public void CheckSelectStudent(GameObject _noneObj1, GameObject _noneObj2)
    {
        if (_noneObj1.activeSelf == false && _noneObj2.activeSelf == false)
        {
            m_SelectCompleteButton.interactable = true;
        }
    }

    // �л��� Ŭ������ �� �л��� ���ȸ�ŭ �����̴��� �ٲ��ش�.
    /// TODO : �ʼ� ������ 2���� �� ���� ó�����ֱ�
    public void ChangeSlider(StudentType _type, int _value)
    {
        if (_type == StudentType.GameDesigner)
        {
            m_GameDesignerFirstSlider.value = _value;
        }
        else if (_type == StudentType.Art)
        {
            m_ArtFirstSlider.value = _value;
        }
        else if (_type == StudentType.Programming)
        {
            m_ProgrammingFirstSlider.value = _value;
        }
    }

    public void ChangeSliderFillSprite(StudentType _type, int _sliderValue, int _needValue)
    {
        if (_type == StudentType.GameDesigner)
        {
            if (_sliderValue > _needValue)
            {
                m_GameDesignerSliderFillImage.sprite = m_SatisfyStat;        // �Ķ���
            }
            else
            {
                m_GameDesignerSliderFillImage.sprite = m_DisSatisfyStat;        // ������
            }
        }
        else if (_type == StudentType.Art)
        {
            if (_sliderValue > _needValue)
            {
                m_ArtSliderFillImage.sprite = m_SatisfyStat;        // �Ķ���
            }
            else
            {
                m_ArtSliderFillImage.sprite = m_DisSatisfyStat;        // ������
            }

        }
        else if (_type == StudentType.Programming)
        {
            if (_sliderValue > _needValue)
            {
                m_ProgrammingSliderFillImage.sprite = m_SatisfyStat;        // �Ķ���
            }
            else
            {
                m_ProgrammingSliderFillImage.sprite = m_DisSatisfyStat;        // ������
            }
        }
    }

    // �����뿡 �����ϴ� ������ �����ϸ� ��ư�� Ȱ��ȭ ��Ű�� �ƴ϶�� ��Ȱ��ȭ ��Ų��.
    public void SetButton(Button _button, bool _isActive, Sprite _buttonSprite)
    {
        _button.GetComponent<Image>().sprite = _buttonSprite;
        _button.interactable = _isActive;
    }

    public void ChangeColorMoneyText(Color _newColor)
    {
        m_EntryFee.color = _newColor;
    }

    // ���� ��ư�� ������ �� ����� �ϴ� �� ���ִ� �Լ�
    public void SetRecruitNoticeActive(bool _isActive)
    {
        m_RecruitNoticePanel.SetActive(_isActive);
        // m_EntryFeePanel.SetActive(_isActive);
    }

    public void SetSelectActive(bool _isActive)
    {
        m_SelectStudentInfo.SetActive(_isActive);
        m_SelectStudent.SetActive(_isActive);
        m_EntryButton.SetActive(_isActive);
    }

    // ������ ��ư���� �θ� Content�� �ٲ��ش�.
    public void MoveObj(GameObject _gameObject, Transform _parent)
    {
        _gameObject.transform.parent = _parent;
        _gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void DestroyStudentObj()
    {
        Transform[] _childCount = m_SelectStudentParentObj.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != m_SelectStudentParentObj.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }

    public void DestroyGameJamListObj()
    {
        Transform[] _childCount = m_GameJamListParentObj.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != m_GameJamListParentObj.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }

    public void ChangeRecruitNoticeInfo(string _title, string _subTitle, string _reward,
        string _awareness, string _date, string _health, string _passion, string _entryFee)
    {
        m_RecruitNoticeInfoPanelTitle.text = _title;
        m_GameJameName.text = _subTitle;
        m_RewardMoney.text = _reward;
        m_Awareness.text = _awareness;
        m_Date.text = _date;
        m_Health.text = _health;
        m_Passion.text = _passion;
        m_EntryFee.text = _entryFee;
    }

    public void ChangeStudentInfo(StudentType _type, string _name, string _health, string _passion, int[] _stat, Sprite _profile)
    {
        switch (_type)
        {
            case StudentType.GameDesigner:
            {
                m_GameDesignerStudentImage.sprite = _profile;
                m_GameDesignerStudentName.text = _name;
                m_GameDesignerStudentHealth.text = _health;
                m_GameDesignerStudentPassion.text = _passion;

                m_GameDesignerInsightSlider.value = _stat[(int)AbilityType.Insight];
                m_GameDesignerConcentractionSlider.value = _stat[(int)AbilityType.Concentration];
                m_GameDesignerSenseSlider.value = _stat[(int)AbilityType.Sense];
                m_GameDesignerTechniqueSlider.value = _stat[(int)AbilityType.Technique];
                m_GameDesignerWitSlider.value = _stat[(int)AbilityType.Wit];
            }
            break;
            case StudentType.Art:
            {
                m_ArtStudentImage.sprite = _profile;
                m_ArtStudentName.text = _name;
                m_ArtStudentHealth.text = _health;
                m_ArtStudentPassion.text = _passion;

                m_ArtInsightSlider.value = _stat[(int)AbilityType.Insight];
                m_ArtConcentractionSlider.value = _stat[(int)AbilityType.Concentration];
                m_ArtSenseSlider.value = _stat[(int)AbilityType.Sense];
                m_ArtTechniqueSlider.value = _stat[(int)AbilityType.Technique];
                m_ArtWitSlider.value = _stat[(int)AbilityType.Wit];
            }
            break;
            case StudentType.Programming:
            {
                m_ProgrammingStudentImage.sprite = _profile;
                m_ProgrammingStudentName.text = _name;
                m_ProgrammingStudentHealth.text = _health;
                m_ProgrammingStudentPassion.text = _passion;

                m_ProgrammingInsightSlider.value = _stat[(int)AbilityType.Insight];
                m_ProgrammingConcentractionSlider.value = _stat[(int)AbilityType.Concentration];
                m_ProgrammingSenseSlider.value = _stat[(int)AbilityType.Sense];
                m_ProgrammingTechniqueSlider.value = _stat[(int)AbilityType.Technique];
                m_ProgrammingWitSlider.value = _stat[(int)AbilityType.Wit];
            }
            break;
        }
    }

    public void SetAwarenessPlusMinus(string _data, Sprite _upArrow, Sprite _downArrow)
    {
        m_AwarenessArrowImage.sprite = _upArrow;
    }

    public void SetHealthPlusMinus(string _data, Sprite _upArrow, Sprite _downArrow)
    {
        //if (int.Parse(_data) > 0)
        //{
        //    m_HealthArrowImage.sprite = _upArrow;
        //}
        //else
        //{
        //}
        m_HealthArrowImage.sprite = _downArrow;
    }

    public void SetPassionPlusMinus(string _data, Sprite _upArrow, Sprite _downArrow)
    {
        //if (int.Parse(_data) > 0)
        //{
        //    m_PassionArrowImage.sprite = _upArrow;
        //}
        //else
        //{
        //}
        m_PassionArrowImage.sprite = _downArrow;
    }

    public void SetPartsNeedStat(string _part, int _data, int _index, int _data2 = 0)
    {
        // _data�� �� ū ���ڸ� ���� ����
        if (_index <= 1)
        {
            float _posX = (_data / 150f) * 705f;

            switch (_part)
            {
                case "��ȹ":
                {
                    // �ʼ������� �ϳ��� �ִ�
                    m_GameDesignerSecondImage.SetActive(false);
                    m_GameDesignerFirstSlider.value = _data;
                    m_GameDesignerSecondSlider.value = _data2;
                    m_GameDesignerBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
                }
                break;

                case "��Ʈ":
                {
                    m_ArtSecondImage.SetActive(false);
                    m_ArtFirstSlider.value = _data;
                    m_ArtSecondSlider.value = _data2;
                    m_ArtBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
                }
                break;

                case "�ù�":
                {
                    m_ProgrammingSecondImage.SetActive(false);
                    m_ProgrammingFirstSlider.value = _data;
                    m_ProgrammingSecondSlider.value = _data2;
                    m_ProgrammingBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
                }
                break;
            }
        }
        else
        {
            float _posX = (_data2 / 150f) * 705f;

            switch (_part)
            {
                case "��ȹ":
                {
                    m_GameDesignerSecondImage.SetActive(true);
                    m_GameDesignerFirstSlider.value = _data;
                    m_GameDesignerSecondSlider.value = _data2;
                    m_GameDesignerBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
                }
                break;

                case "��Ʈ":
                {
                    m_ArtSecondImage.SetActive(true);
                    m_ArtFirstSlider.value = _data;
                    m_ArtSecondSlider.value = _data2;
                    m_ArtBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
                }
                break;

                case "�ù�":
                {
                    m_ProgrammingSecondImage.SetActive(true);
                    m_ProgrammingFirstSlider.value = _data;
                    m_ProgrammingSecondSlider.value = _data2;
                    m_ProgrammingBar.rectTransform.anchoredPosition = new Vector3(_posX, 0, 0);
                }
                break;
            }
        }
    }

    // ���� �帣�� �ٲ��ش�.
    public void ChangeExpectedGenreText(GenreStat _genre)
    {
        switch (_genre)
        {
            case GenreStat.Action:
            {
                m_ExpectedGenre.text = "�׼�";
            }
            break;
            case GenreStat.Simulation:
            {
                m_ExpectedGenre.text = "�ùķ��̼�";
            }
            break;
            case GenreStat.Adventure:
            {
                m_ExpectedGenre.text = "��庥ó";
            }
            break;
            case GenreStat.Shooting:
            {
                m_ExpectedGenre.text = "����";
            }
            break;
            case GenreStat.RPG:
            {
                m_ExpectedGenre.text = "RPG";
            }
            break;
            case GenreStat.Puzzle:
            {
                m_ExpectedGenre.text = "����";
            }
            break;
            case GenreStat.Rhythm:
            {
                m_ExpectedGenre.text = "����";
            }
            break;
            case GenreStat.Sports:
            {
                m_ExpectedGenre.text = "������";
            }
            break;
        }
    }

    public void ChangeExpectedSuccess(string _sucsess)
    {
        m_ExpectedSuccesPercent.text = _sucsess;
    }
}
