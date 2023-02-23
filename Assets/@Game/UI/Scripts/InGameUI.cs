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
        if (instance == null)
        {
            instance = this;
        }

        var json = GameObject.Find("Json");             // 씬이 다르기 때문에 Json 을 쓰려면 이렇게 해줘야 한다 
        if (json.GetComponent<Json>().IsDataExists == true)
        {
            PlayerInfo.Instance.m_SpecialPoint = AllInOneData.Instance.player.m_SpecialPoint;
            PlayerInfo.Instance.m_MyMoney = AllInOneData.Instance.player.m_Money;

            PlayerInfo.Instance.m_AcademyName = AllInOneData.Instance.player.m_AcademyName;
            PlayerInfo.Instance.m_PrincipalName = AllInOneData.Instance.player.m_PrincipalName;


            m_nowMoney.text = AllInOneData.Instance.player.m_Money.ToString();

            m_nowAcademyName.text = AllInOneData.Instance.player.m_AcademyName;
            m_nowDirectorName.text = AllInOneData.Instance.player.m_PrincipalName;

            switch (AllInOneData.Instance.InGameData.day)
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

            m_nowAcademyName.text = PlayerInfo.Instance.m_AcademyName;
            m_nowDirectorName.text = PlayerInfo.Instance.m_PrincipalName;

            m_TimeBar.fillAmount = 0.2f;
        }
    }

    // 켜진 UI 들을 다 담아 둘 스택
    public Stack<GameObject> UIStack;

    [SerializeField]
    PopUpUI m_popUpInstant;     // Title -> InGame 씬으로 이동 시 제일 먼저 띄워질 팝업창

    // 인게임 배경화면 UI 부분
    public TextMeshProUGUI m_nowAcademyName;
    public TextMeshProUGUI m_nowDirectorName;

    public TextMeshProUGUI m_nowMoney;
    public TextMeshProUGUI m_SpecialPoint;

    public TextMeshProUGUI m_touchcount;

    public Image m_TimeBar;

    public GameObject SettingPanel;
    public GameObject SaveButton;

    public GameObject tempPlusMoneyButton;

    // 이벤트 시스템 UI 부분
    public TextMeshProUGUI m_HeadLinenowMonth;
    public TextMeshProUGUI m_CalendernowMonth;

    // Start is called before the first frame update
    void Start()
    {
        // UI에 그릴 데이터 로드 & 기본값 설정
        // m_nowAcademyName.text = PlayerInfo.Instance.m_AcademyName;
        // m_nowDirectorName.text = PlayerInfo.Instance.m_DirectorName;
        // m_nowMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();

        UIStack = new Stack<GameObject>();
        Debug.Log("stack count : " + UIStack.Count);
        // 데이터가 있다? -> 이 버튼은 뜨면 안된다
        var json = GameObject.Find("Json");             // 씬이 다르기 때문에 Json 을 쓰려면 이렇게 해줘야 한다 
        if (json.GetComponent<Json>().IsDataExists == false)
        {
            m_popUpInstant.DelayTurnOnUI();
        }

        // 데이터저장하기 버튼에 함수를 달기 -> 클릭하면 해당 함수 실행
        SaveButton.GetComponent<Button>().onClick.AddListener(IfSaveInGameData);

        tempPlusMoneyButton.GetComponent<Button>().onClick.AddListener(tempPlusMoney);
    }

    // Update is called once per frame
    void Update()
    {
        GameTime.Instance.DrawTimeBar(m_TimeBar);       // 하루(6초) 체크

        m_nowMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();

        // 테스트를 위한 디버깅용
        if (Input.touchCount == 0)
        {
            m_touchcount.text = "0";
        }
        if (Input.touchCount == 1)
        {
            m_touchcount.text = "1";


            // m_touchpointX.text = ;
        }
        if (Input.touchCount == 2)
        {
            m_touchcount.text = "2";
        }

    }

    public void IfSaveInGameData()
    {
        Debug.Log("데이터 저장 ?!");

        // DontDestroy 로 생성이 되어있지만 바로 Json.instance.함수() 로 부르면 기존의 json과 다른 코드 덩어리가 불려서 새로운 instance를 찾기 때문에 둘이 다르기 때문에 null 이 떠서 안된다.
        var json = GameObject.Find("Json");

        json.GetComponent<Json>().SaveAllDataInGameScene();
    }

    public void tempPlusMoney()
    {
        GameObject _NowClick = EventSystem.current.currentSelectedGameObject;   // 현재클릭

        if (_NowClick.name == tempPlusMoneyButton.name)
        {
            PlayerInfo.Instance.m_MyMoney += 3;

            PlayerInfo.Instance.m_MyMoney *= 5;
        }

        Debug.Log("현재 소지금 : " + PlayerInfo.Instance.m_MyMoney);
    }

    // 이벤트 시스템 UI 그리는 부분
    public void DrawEventScreenUIText()
    {
        string nowmonth = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
        m_HeadLinenowMonth.text = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
        m_CalendernowMonth.text = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
}
}
