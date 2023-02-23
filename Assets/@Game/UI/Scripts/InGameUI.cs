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
        if (instance == null)
        {
            instance = this;
        }

        var json = GameObject.Find("Json");             // ���� �ٸ��� ������ Json �� ������ �̷��� ����� �Ѵ� 
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

    // ���� UI ���� �� ��� �� ����
    public Stack<GameObject> UIStack;

    [SerializeField]
    PopUpUI m_popUpInstant;     // Title -> InGame ������ �̵� �� ���� ���� ����� �˾�â

    // �ΰ��� ���ȭ�� UI �κ�
    public TextMeshProUGUI m_nowAcademyName;
    public TextMeshProUGUI m_nowDirectorName;

    public TextMeshProUGUI m_nowMoney;
    public TextMeshProUGUI m_SpecialPoint;

    public TextMeshProUGUI m_touchcount;

    public Image m_TimeBar;

    public GameObject SettingPanel;
    public GameObject SaveButton;

    public GameObject tempPlusMoneyButton;

    // �̺�Ʈ �ý��� UI �κ�
    public TextMeshProUGUI m_HeadLinenowMonth;
    public TextMeshProUGUI m_CalendernowMonth;

    // Start is called before the first frame update
    void Start()
    {
        // UI�� �׸� ������ �ε� & �⺻�� ����
        // m_nowAcademyName.text = PlayerInfo.Instance.m_AcademyName;
        // m_nowDirectorName.text = PlayerInfo.Instance.m_DirectorName;
        // m_nowMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();

        UIStack = new Stack<GameObject>();
        Debug.Log("stack count : " + UIStack.Count);
        // �����Ͱ� �ִ�? -> �� ��ư�� �߸� �ȵȴ�
        var json = GameObject.Find("Json");             // ���� �ٸ��� ������ Json �� ������ �̷��� ����� �Ѵ� 
        if (json.GetComponent<Json>().IsDataExists == false)
        {
            m_popUpInstant.DelayTurnOnUI();
        }

        // �����������ϱ� ��ư�� �Լ��� �ޱ� -> Ŭ���ϸ� �ش� �Լ� ����
        SaveButton.GetComponent<Button>().onClick.AddListener(IfSaveInGameData);

        tempPlusMoneyButton.GetComponent<Button>().onClick.AddListener(tempPlusMoney);
    }

    // Update is called once per frame
    void Update()
    {
        GameTime.Instance.DrawTimeBar(m_TimeBar);       // �Ϸ�(6��) üũ

        m_nowMoney.text = PlayerInfo.Instance.m_MyMoney.ToString();

        // �׽�Ʈ�� ���� ������
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
        Debug.Log("������ ���� ?!");

        // DontDestroy �� ������ �Ǿ������� �ٷ� Json.instance.�Լ�() �� �θ��� ������ json�� �ٸ� �ڵ� ����� �ҷ��� ���ο� instance�� ã�� ������ ���� �ٸ��� ������ null �� ���� �ȵȴ�.
        var json = GameObject.Find("Json");

        json.GetComponent<Json>().SaveAllDataInGameScene();
    }

    public void tempPlusMoney()
    {
        GameObject _NowClick = EventSystem.current.currentSelectedGameObject;   // ����Ŭ��

        if (_NowClick.name == tempPlusMoneyButton.name)
        {
            PlayerInfo.Instance.m_MyMoney += 3;

            PlayerInfo.Instance.m_MyMoney *= 5;
        }

        Debug.Log("���� ������ : " + PlayerInfo.Instance.m_MyMoney);
    }

    // �̺�Ʈ �ý��� UI �׸��� �κ�
    public void DrawEventScreenUIText()
    {
        string nowmonth = GameTime.Instance.FlowTime.NowMonth.ToString() + "��";
        m_HeadLinenowMonth.text = GameTime.Instance.FlowTime.NowMonth.ToString() + "��";
        m_CalendernowMonth.text = GameTime.Instance.FlowTime.NowMonth.ToString() + "��";
}
}
