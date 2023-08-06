using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using UnityEngine.EventSystems;


/// <summary>
/// 23. 01. 02 Mang
/// 
/// InGameScene 의 메인 화면에서 쓰일 UI 들을 관리 할 스크립트
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

        //var json = GameObject.Find("Json");             // 씬이 다르기 때문에 Json 을 쓰려면 이렇게 해줘야 한다 
        // 저장 데이터가 있다면... 
        if (Json.Instance.UseLoadingData)
        {
            m_nowMoney.text = string.Format("{0:#,0}", PlayerInfo.Instance.MyMoney);

            m_nowAcademyName.text = PlayerInfo.Instance.AcademyName;
            m_nowDirectorName.text = PlayerInfo.Instance.PrincipalName;

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
            m_nowMoney.text = string.Format("{0:#,0}", PlayerInfo.Instance.MyMoney);

            m_nowAcademyName.text = "";
            m_nowDirectorName.text = "";

            //m_nowAwareness.text = PlayerInfo.Instance.Famous.ToString();
            //m_nowTalentDevelopment.text = PlayerInfo.Instance.TalentDevelopment.ToString();
            //m_nowManagement.text = PlayerInfo.Instance.Management.ToString();

            m_TimeBar.fillAmount = 0.2f;
        }
    }

    public delegate void GameShowRsultButtonClicked();
    public static event GameShowRsultButtonClicked OnGameShowRsultButtonClicked;

    // 켜진 UI 들을 다 담아 둘 스택
    public Stack<GameObject> UIStack = new Stack<GameObject>();

    public Camera UICamera;

    [SerializeField]
    PopUpUI m_popUpInstant;     // Title -> InGame 씬으로 이동 시 제일 먼저 띄워질 팝업창
    /// 매년 3월 셋째주 두번째 요일에 랭크 팝업을 띄워주기 위한 변수
    [SerializeField] private RankPopUpPanel m_RankPopUpPanel;

    [SerializeField] private PopUpUI m_RankPopUp;
    [SerializeField] private Button m_GameShowResultButton;

    public bool m_IsPopUpRank;


    // 인게임 배경화면 UI 부분
    public TextMeshProUGUI m_nowAcademyName;
    public TextMeshProUGUI m_nowDirectorName;

    public TextMeshProUGUI m_nowMoney;
    public TextMeshProUGUI m_SpecialPoint;

    //public TextMeshProUGUI m_nowAwareness;
    //public TextMeshProUGUI m_nowTalentDevelopment;
    //public TextMeshProUGUI m_nowManagement;

    public TextMeshProUGUI m_touchcount;

    public Image m_TimeBar;

    public Button SettingPanelButton;                 // Inspector 창에서 연결
    public GameObject SaveButton;

    public GameObject tempPlusMoneyButton;

    // 이벤트 시스템 UI 부분
    public TextMeshProUGUI m_HeadLinenowMonth;
    public TextMeshProUGUI m_CalendernowMonth;

    public TextMeshProUGUI m_TestDay;

    // GameJam캔버스를 켜줄 함수 만드는 중
    public GameObject m_SecondContentUI;

    public Button m_ChangeGameSpeedButton;
    public TextMeshProUGUI m_SpeedButtonText;
    public Button m_ingEventButton;

    public int m_NowGameSpeed = 1;

    public TextMeshProUGUI TempUIstackInfo;

    [Space(10f)]
    [SerializeField] private Button MissionOpenButton;         // 퀘스트 패널을 열기 위한 버튼
    [SerializeField] private Image MissionAlarmImg;         // 퀘스트 패널을 열기 위한 버튼
    [SerializeField] private RectTransform MissionAlarmEffect;         // 퀘스트 패널을 열기 위한 버튼

    public Button GetQuestOpenButton
    {
        get { return MissionOpenButton; }
        set { MissionOpenButton = value; }
    }

    public Image GetMissionAlarmImg
    {
        get { return MissionAlarmImg; }
        set { MissionAlarmImg = value; }
    }

    public RectTransform GetMissionAlarmEffect
    {
        get { return MissionAlarmEffect; }
        set { MissionAlarmEffect = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        MissionAlarmImg.gameObject.SetActive(false);
        MissionAlarmEffect.gameObject.SetActive(false);

        // UI에 그릴 데이터 로드 & 기본값 설정
        // m_nowAcademyName.text = PlayerInfo.Instance.m_AcademyName;
        // m_nowDirectorName.text = PlayerInfo.Instance.m_DirectorName;
        // m_nowMoney.text = PlayerInfo.Instance.MyMoney.ToString();

        // UIStack = new Stack<GameObject>();
        Debug.Log("stack count : " + UIStack.Count);
        // 데이터가 있다? -> 이 버튼은 뜨면 안된다
        //  var json = GameObject.Find("Json");             // 씬이 다르기 때문에 Json 을 쓰려면 이렇게 해줘야 한다 
        // if (json.GetComponent<Json>().IsDataExists == false)
        // {
        //     m_popUpInstant.TurnOnUI();
        // }

        // 데이터저장하기 버튼에 함수를 달기 -> 클릭하면 해당 함수 실행
        //SaveButton.GetComponent<Button>().onClick.AddListener(IfSaveInGameData);
        m_GameShowResultButton.onClick.AddListener(OnGameShowResultClicked);
        tempPlusMoneyButton.GetComponent<Button>().onClick.AddListener(tempPlusMoney);

        m_ChangeGameSpeedButton.onClick.AddListener(ChangeGameSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        GameTime.Instance.DrawTimeBar(m_TimeBar);       // 하루(6초) 체크

        m_nowMoney.text = string.Format("{0:#,0}", PlayerInfo.Instance.MyMoney);
        m_SpecialPoint.text = string.Format("{0:#,0}", PlayerInfo.Instance.SpecialPoint);
        m_TestDay.text = GameTime.Instance.FlowTime.NowDay.ToString();

        //m_nowAwareness.text = PlayerInfo.Instance.Famous.ToString();
        //m_nowTalentDevelopment.text = PlayerInfo.Instance.TalentDevelopment.ToString();
        //m_nowManagement.text = PlayerInfo.Instance.Management.ToString();

        if (!m_IsPopUpRank && GameTime.Instance.FlowTime.NowYear != 1 && GameTime.Instance.FlowTime.NowMonth == 3 && GameTime.Instance.FlowTime.NowWeek == 3 && GameTime.Instance.FlowTime.NowDay == 2)
        {
            m_IsPopUpRank = true;
            string _text = "전국 게임교육원\n랭크 발표날이 되었습니다.\n올해 우리학원은 몇등급일까요?\n궁금하니 얼른 확인해보죠!";
            //m_RankPopUp.TurnOnUI();
            m_RankPopUpPanel.RankPopUpAndOff(true);
            m_RankPopUpPanel.ChangeRankPopUpText(_text);
        }

        // 테스트를 위한 디버깅용
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

        TempUIstackInfo.text = "현재 스택 : " + UIStack.Count;
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
        Debug.Log("데이터 저장 ?!");

        // DontDestroy 로 생성이 되어있지만 바로 Json.instance.함수() 로 부르면 기존의 json과 다른 코드 덩어리가 불려서 새로운 instance를 찾기 때문에 둘이 다르기 때문에 null 이 떠서 안된다.
        // var json = GameObject.Find("Json");
        // json.GetComponent<Json>().SaveAllDataInGameScene();
        //Json.Instance.SaveAllDataInGameScene();
    }

    public void tempPlusMoney()
    {
        GameObject _NowClick = EventSystem.current.currentSelectedGameObject;   // 현재클릭

        if (_NowClick.name == tempPlusMoneyButton.name)
        {
            PlayerInfo.Instance.MyMoney += 300;

            PlayerInfo.Instance.SpecialPoint += 10000;
        }

        Debug.Log("현재 소지금 : " + PlayerInfo.Instance.MyMoney);
    }

    // 이벤트 시스템 UI 그리는 부분
    public void DrawEventScreenUIText()
    {
        string nowmonth = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
        // m_HeadLinenowMonth.text = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
        // m_CalendernowMonth.text = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
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
