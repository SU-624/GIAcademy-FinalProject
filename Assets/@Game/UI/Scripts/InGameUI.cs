using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using UnityEngine.EventSystems;


/// <summary>
/// 23. 01. 02 Mang
/// 
/// InGameScene �� ���� ȭ�鿡�� ���� UI ���� ���� �� ��ũ��Ʈ
/// </summary>
public class InGameUI : MonoBehaviour
{
    private static InGameUI instance = null;

    public static InGameUI Instance
    {
        get
        {
            return instance;
        }

        set { instance = value; }
    }
    public void Awake()
    {
        UICamera.gameObject.SetActive(false);

        if (instance == null)
        {
            instance = this;
        }

        var json = GameObject.Find("Json");             // ���� �ٸ��� ������ Json �� ������ �̷��� ����� �Ѵ� 
        // ���� �����Ͱ� �ִٸ�... 
        if (json.GetComponent<Json>().IsSavedDataExists == true)
        {
            PlayerInfo.Instance.m_SpecialPoint = AllInOneData.Instance.PlayerData.SpecialPoint;
            PlayerInfo.Instance.m_MyMoney = AllInOneData.Instance.PlayerData.Money;

            PlayerInfo.Instance.m_AcademyName = AllInOneData.Instance.PlayerData.AcademyName;
            PlayerInfo.Instance.m_TeacherName = AllInOneData.Instance.PlayerData.PrincipalName;

            PlayerInfo.Instance.m_Awareness = AllInOneData.Instance.PlayerData.Famous;
            PlayerInfo.Instance.m_Management = AllInOneData.Instance.PlayerData.Management;
            PlayerInfo.Instance.m_TalentDevelopment = AllInOneData.Instance.PlayerData.TalentDevelopment;
            PlayerInfo.Instance.m_Activity = AllInOneData.Instance.PlayerData.Activity;
            PlayerInfo.Instance.m_Goods = AllInOneData.Instance.PlayerData.Goods;
            //PlayerInfo.Instance.m_CurrentRank = AllInOneData.Instance.player.CurrentRank;

            m_nowMoney.text = AllInOneData.Instance.PlayerData.Money.ToString();

            m_nowAcademyName.text = AllInOneData.Instance.PlayerData.AcademyName;
            m_nowDirectorName.text = AllInOneData.Instance.PlayerData.PrincipalName;

            //m_nowAwareness.text = AllInOneData.Instance.player.m_Famous.ToString();
            //m_nowTalentDevelopment.text = AllInOneData.Instance.player.m_TalentDevelopment.ToString();
            //m_nowManagement.text = AllInOneData.Instance.player.m_Management.ToString();


            switch (AllInOneData.Instance.PlayerData.Day)
            {
                case 1:
                    { m_TimeBar.fillAmount = 0.2f; }
                    break;

                case 2:
                    { m_TimeBar.fillAmount = 0.4f; }
                    break;

                case 3:
                    { m_TimeBar.fillAmount = 0.6f; }
                    break;

                case 4:
                    { m_TimeBar.fillAmount = 0.8f; }
                    break;

                case 5:
                    { m_TimeBar.fillAmount = 1.0f; }
                    break;
            }
        }
        else
        {
            m_nowMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();

            m_nowAcademyName.text = "";
            m_nowDirectorName.text = "";

            //m_nowAwareness.text = PlayerInfo.Instance.m_Awareness.ToString();
            //m_nowTalentDevelopment.text = PlayerInfo.Instance.m_TalentDevelopment.ToString();
            //m_nowManagement.text = PlayerInfo.Instance.m_Management.ToString();

            m_TimeBar.fillAmount = 0.2f;
        }
    }

    public delegate void GameShowRsultButtonClicked();
    public static event GameShowRsultButtonClicked OnGameShowRsultButtonClicked;

    // ���� UI ���� �� ��� �� ����
    public Stack<GameObject> UIStack = new Stack<GameObject>();

    public Camera UICamera;

    [SerializeField]
    PopUpUI m_popUpInstant;     // Title -> InGame ������ �̵� �� ���� ���� ����� �˾�â
    /// �ų� 3�� ��°�� �ι�° ���Ͽ� ��ũ �˾��� ����ֱ� ���� ����
    [SerializeField] private RankPopUpPanel m_RankPopUpPanel;

    [SerializeField] private PopUpUI m_RankPopUp;
    [SerializeField] private Button m_GameShowResultButton;

    public bool m_IsPopUpRank;


    // �ΰ��� ���ȭ�� UI �κ�
    public TextMeshProUGUI m_nowAcademyName;
    public TextMeshProUGUI m_nowDirectorName;

    public TextMeshProUGUI m_nowMoney;
    public TextMeshProUGUI m_SpecialPoint;

    //public TextMeshProUGUI m_nowAwareness;
    //public TextMeshProUGUI m_nowTalentDevelopment;
    //public TextMeshProUGUI m_nowManagement;

    public TextMeshProUGUI m_touchcount;

    public Image m_TimeBar;

    public GameObject SettingPanel;
    public GameObject SaveButton;

    public GameObject tempPlusMoneyButton;

    // �̺�Ʈ �ý��� UI �κ�
    public TextMeshProUGUI m_HeadLinenowMonth;
    public TextMeshProUGUI m_CalendernowMonth;

    public TextMeshProUGUI m_TestDay;

    // GameJamĵ������ ���� �Լ� ����� ��
    public GameObject m_SecondContentUI;

    public Button m_ChangeGameSpeedButton;
    public TextMeshProUGUI m_SpeedButtonText;
    public Button m_ingEventButton;

    public int m_NowGameSpeed = 1;

    public TextMeshProUGUI TempUIstackInfo;

    [Space(10f)]
    [SerializeField] private Button QuestOpenButton;         // ����Ʈ �г��� ���� ���� ��ư

    public Button GetQuestOpenButton
    {
        get { return QuestOpenButton; }
        set { QuestOpenButton = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // UI�� �׸� ������ �ε� & �⺻�� ����
        // m_nowAcademyName.text = PlayerInfo.Instance.m_AcademyName;
        // m_nowDirectorName.text = PlayerInfo.Instance.m_DirectorName;
        // m_nowMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();

        // UIStack = new Stack<GameObject>();
        Debug.Log("stack count : " + UIStack.Count);
        // �����Ͱ� �ִ�? -> �� ��ư�� �߸� �ȵȴ�
        //  var json = GameObject.Find("Json");             // ���� �ٸ��� ������ Json �� ������ �̷��� ����� �Ѵ� 
        // if (json.GetComponent<Json>().IsDataExists == false)
        // {
        //     m_popUpInstant.TurnOnUI();
        // }

        // �����������ϱ� ��ư�� �Լ��� �ޱ� -> Ŭ���ϸ� �ش� �Լ� ����
        //SaveButton.GetComponent<Button>().onClick.AddListener(IfSaveInGameData);
        m_GameShowResultButton.onClick.AddListener(OnGameShowResultClicked);
        tempPlusMoneyButton.GetComponent<Button>().onClick.AddListener(tempPlusMoney);

        m_ChangeGameSpeedButton.onClick.AddListener(ChangeGameSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        GameTime.Instance.DrawTimeBar(m_TimeBar);       // �Ϸ�(6��) üũ

        m_nowMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();
        m_SpecialPoint.text = PlayerInfo.Instance.m_SpecialPoint.ToString();
        m_TestDay.text = GameTime.Instance.FlowTime.NowDay.ToString();

        //m_nowAwareness.text = PlayerInfo.Instance.m_Awareness.ToString();
        //m_nowTalentDevelopment.text = PlayerInfo.Instance.m_TalentDevelopment.ToString();
        //m_nowManagement.text = PlayerInfo.Instance.m_Management.ToString();

        if (!m_IsPopUpRank && GameTime.Instance.FlowTime.NowYear != 1 && GameTime.Instance.FlowTime.NowMonth == 3 && GameTime.Instance.FlowTime.NowWeek == 3 && GameTime.Instance.FlowTime.NowDay == 2)
        {
            m_IsPopUpRank = true;
            string _text = "���� ���ӱ�����\n��ũ ��ǥ���� �Ǿ����ϴ�.\n���� �츮�п��� �����ϱ��?\n�ñ��ϴ� �� Ȯ���غ���!";
            //m_RankPopUp.TurnOnUI();
            m_RankPopUpPanel.RankPopUpAndOff(true);
            m_RankPopUpPanel.ChangeRankPopUpText(_text);
        }

        // �׽�Ʈ�� ���� ������
        //if (Input.touchCount == 0)
        //{
        //    m_touchcount.text = "0";
        //}
        //if (Input.touchCount == 1)
        //{
        //    m_touchcount.text = "1";
        //}
        //if (Input.touchCount == 2)
        //{
        //    m_touchcount.text = "2";
        //}

        TempUIstackInfo.text = "���� ���� : " + UIStack.Count;
    }

    private void OnGameShowResultClicked()
    {
        if (OnGameShowRsultButtonClicked != null)
        {
            OnGameShowRsultButtonClicked();
        }
    }

    public void ChangeGameSpeed()
    {
        if (Time.timeScale == 1)
        {
            m_SpeedButtonText.text = "x3";
            m_NowGameSpeed = 3;
            Time.timeScale = m_NowGameSpeed;
        }
        else if (Time.timeScale == 3)
        {
            m_SpeedButtonText.text = "x5";
            m_NowGameSpeed = 5;
            Time.timeScale = m_NowGameSpeed;
        }
        else if (Time.timeScale == 5)
        {
            m_SpeedButtonText.text = "x1";
            m_NowGameSpeed = 1;
            Time.timeScale = m_NowGameSpeed;
        }
    }

    public void IfSaveInGameData()
    {
        Debug.Log("������ ���� ?!");

        // DontDestroy �� ������ �Ǿ������� �ٷ� Json.instance.�Լ�() �� �θ��� ������ json�� �ٸ� �ڵ� ����� �ҷ��� ���ο� instance�� ã�� ������ ���� �ٸ��� ������ null �� ���� �ȵȴ�.
        // var json = GameObject.Find("Json");
        // json.GetComponent<Json>().SaveAllDataInGameScene();
        //Json.Instance.SaveAllDataInGameScene();
    }

    public void tempPlusMoney()
    {
        GameObject _NowClick = EventSystem.current.currentSelectedGameObject;   // ����Ŭ��

        if (_NowClick.name == tempPlusMoneyButton.name)
        {
            PlayerInfo.Instance.m_MyMoney += 300;

            PlayerInfo.Instance.m_SpecialPoint += 10000;
        }

        Debug.Log("���� ������ : " + PlayerInfo.Instance.m_MyMoney);
    }

    // �̺�Ʈ �ý��� UI �׸��� �κ�
    public void DrawEventScreenUIText()
    {
        string nowmonth = GameTime.Instance.FlowTime.NowMonth.ToString() + "��";
        // m_HeadLinenowMonth.text = GameTime.Instance.FlowTime.NowMonth.ToString() + "��";
        // m_CalendernowMonth.text = GameTime.Instance.FlowTime.NowMonth.ToString() + "��";
    }

    //public void ClickExternalActivityButton()
    //{
    //    if (m_SecondContentUI.gameObject.activeSelf == true)
    //    {
    //        m_SecondContentUI.SetActive(false);
    //    }
    //    else
    //    {
    //        m_SecondContentUI.SetActive(true);
    //    }

    //}

    //public void ClickGameJamButton()
    //{
    //    m_GameJamCanvas.SetActiveGameJamCanvas(true);
    //}
}
