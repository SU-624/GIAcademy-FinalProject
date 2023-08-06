using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Text;
using Newtonsoft.Json;


/// <summary>
/// 2023. 01. 16 Mang
/// 
/// ���⼭ ���� ������ �����տ� �����͸� �ְ�, �����Ͱ� �� ������Ʈ�� �ű��, �� ���� ���̴�
/// �̱������� ����, ������ ���� �ʿ��� ������ �� �� �ֵ���
/// </summary>
public class SwitchEventList : MonoBehaviour
{
    private static SwitchEventList _instance = null;

    // Inspector â�� �������� ������
    [Tooltip("�̺�Ʈ ���� â�� ��밡�� �̺�Ʈ����� ����� ���� �����պ�����")] [Header("EventList Prefab")]
    public GameObject m_PossibleEventprefab; // 

    public Transform m_PossibleParentScroll; // 
    public GameObject m_ParentCalenderButton;

    [Space(10f)] [Tooltip("�޷� â�� ���õ� �̺�Ʈ ����Ʈ�� ���� �����պ�����")] [Header("SelectdEvent&SetOkEvent Prefab")]
    public GameObject m_FixedPrefab;

    public GameObject m_SelectedPrefab;
    public Transform m_ParentCalenderScroll;

    // �̺�Ʈ â���� ���̴� �ؽ�Ʈ ���� �ٷ�� ������
    public TextMeshProUGUI NoticeMoneyText;

    public TextMeshProUGUI NowMoneyText;
    public TextMeshProUGUI NowSpecialPointText;

    public TextMeshProUGUI PayMoneyText;
    public TextMeshProUGUI PaySpecialPointText;

    public GameObject EventSetPanel;


    // Json ������ �Ľ��ؼ� �� �����͵��� �� ��� �� ����Ʈ ����
    // �� �����鵵 EventSchedule �� Instance.���� �鿡 �־��ְ� ������ ����
    public List<SuddenEventTableData> SuddenEventClassInfo = new List<SuddenEventTableData>(); // ��ü ���� �̺�Ʈ

    public List<SuddenEventTableData> SelectEventClassInfo = new List<SuddenEventTableData>(); // ��ü ���� �̺�Ʈ
    //

    public List<SuddenEventTableData>
        PossibleChooseEventClassList = new List<SuddenEventTableData>(); //  ��밡���� �����̺�Ʈ ���

    public List<SuddenEventTableData> ThisMonthMySelectEventList = new List<SuddenEventTableData>(); // �̹��� ���� �����̺�Ʈ ���
    public List<SuddenEventTableData> ThisMonthMySuddenEventList = new List<SuddenEventTableData>(); // �̹��� ���� �����̺�Ʈ ���

    public List<SuddenEventTableData>
        PrevIChoosedEvent = new List<SuddenEventTableData>(); // ���� ���� ������ �̺�Ʈ(�ִ� 2��) ��� �� �ӽ� ����

    public SuddenEventTableData TempIChoosed; // �ӽ÷� ���� ��� ���� ��

    int month;

    const int FixedEvenetCount = 3;
    const int SelectedEventCount = 2;
    const int PossibleEventCount = 10;

    public bool IsSetEventList = false;

    GameObject _PrevSelect = null;

    [SerializeField] PopOffUI _PopOfEventCalenderPanel;

    public static SwitchEventList Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // woodpie9 0615 �̻�� �ϴ� ������ �ּ� ó��
        // for (int i = 0; i < AllOriginalJsonData.Instance.OriginalSuddenEventDataList.Count; i++)
        // {
        //     ThisMonthMySuddenEventList.Add(AllOriginalJsonData.Instance.OriginalSuddenEventDataList[i]);
        // }


        // Debug.Log("���� �̺�Ʈ �ε� �Ϸ�?" + SelectEventClassInfo);
        Debug.Log("���� �̺�Ʈ �ε� �Ϸ�?" + SuddenEventClassInfo);

        #region ���̽� ����Ƿ� ������ �ֱ� �ּ�ó���Ѱ�

        // 1. ���̽� ���� ��ü �̺�Ʈ����Ʈ ������ ���
        //
        // �����̺�Ʈ
        // SaveEventData TempFixedData = new SaveEventData();
        // SaveEventData TempFixedData1 = new SaveEventData();
        // 
        // TempFixedData.EventClassName = "FixedTestEvent0";
        // TempFixedData.EventDate[0] = 1;
        // TempFixedData.EventDate[1] = 3;     // 3��
        // TempFixedData.EventDate[2] = 2;     // 2����
        // TempFixedData.EventDate[3] = 4;     // �����
        // 
        // TempFixedData.EventInformation = "�̺�Ʈ����";
        // TempFixedData.IsFixedEvent = true;
        // TempFixedData.IsPossibleUseEvent = true;
        // 
        // TempFixedData.EventRewardStatName[0] = "StatName0";
        // TempFixedData.EventRewardStat[0] = 2 + (1 * 3);
        // 
        // TempFixedData.EventRewardStatName[1] = "StatName1";
        // TempFixedData.EventRewardStat[1] = 8 + (3 * 7);
        // 
        // TempFixedData.EventRewardStatName[2] = "StatName2";
        // TempFixedData.EventRewardStat[2] = 46 + (2 * 2);
        // 
        // TempFixedData.EventRewardMoney = 386 + (1 * 4);
        // 
        // FixedEventClassInfo.Add(TempFixedData);
        // ////
        // TempFixedData1.EventClassName = "FixedTestEvent1";
        // TempFixedData1.EventDate[0] = 1;
        // TempFixedData1.EventDate[1] = 4;     // 4��
        // TempFixedData1.EventDate[2] = 1;      // 1����
        // TempFixedData1.EventDate[3] = 2;      // ȭ����
        // 
        // TempFixedData1.EventInformation = "�̺�Ʈ����";
        // TempFixedData1.IsFixedEvent = true;
        // TempFixedData1.IsPossibleUseEvent = true;
        // 
        // TempFixedData1.EventRewardStatName[0] = "StatName0";
        // TempFixedData1.EventRewardStat[0] = 2 + (1 * 4);
        // 
        // TempFixedData1.EventRewardStatName[1] = "StatName1";
        // TempFixedData1.EventRewardStat[1] = 8 + (3 * 3);
        // 
        // TempFixedData1.EventRewardStatName[2] = "StatName2";
        // TempFixedData1.EventRewardStat[2] = 46 + (2 * 5);
        // 
        // TempFixedData1.EventRewardMoney = 386 + (1 * 8);
        // 
        // FixedEventClassInfo.Add(TempFixedData1);
        // ////
        // 
        // 
        // // �����̺�Ʈ
        // // (�ӽ÷� ���⼭)2. ��ü �̺�Ʈ ����Ʈ���� ��밡���� �̺�Ʈ�� ��밡���̺�Ʈ ����Ʈ ������ ���
        // // ���̽� ���� ���� �� �׽�Ʈ�� ���� ���� ����
        // SaveEventData TempEventData = new SaveEventData();
        // SaveEventData TempEventData1 = new SaveEventData();
        // SaveEventData TempEventData2 = new SaveEventData();
        // SaveEventData TempEventData3 = new SaveEventData();
        // 
        // 
        // // �̺�Ʈ struct ���� �ʱ�ȭ ���ֱ�
        // TempEventData.EventClassName = "test 0";
        // TempEventData.EventDate[0] = 1;
        // 
        // TempEventData.EventDate[1] = 3;
        // 
        // TempEventData.IsPossibleUseEvent = false;
        // TempEventData.IsFixedEvent = false;      // �����̺�Ʈ���� �����̺�Ʈ���� ������ Ű����
        // TempEventData.EventRewardStatName[0] = "StatName0";
        // TempEventData.EventRewardStat[0] = 5 + (1 * 3);
        // 
        // TempEventData.EventRewardStatName[1] = "StatName1";
        // TempEventData.EventRewardStat[1] = 30 + (1 * 7);
        // 
        // TempEventData.EventRewardStatName[2] = "StatName2";
        // TempEventData.EventRewardStat[2] = 100 + (2 * 2);
        // 
        // TempEventData.EventRewardMoney = 5247 + (3 * 4);
        // 
        // SelectEventClassInfo.Add(TempEventData);
        // ////
        // TempEventData1.EventClassName = "test 1";
        // TempEventData1.EventDate[0] = 1;
        // 
        // TempEventData1.EventDate[1] = 3;
        // TempEventData1.IsPossibleUseEvent = false;
        // TempEventData1.IsFixedEvent = false;      // �����̺�Ʈ���� �����̺�Ʈ���� ������ Ű����
        // TempEventData1.EventRewardStatName[0] = "StatName0";
        // TempEventData1.EventRewardStat[0] = 5 + (1 * 3);
        // 
        // TempEventData1.EventRewardStatName[1] = "StatName1";
        // TempEventData1.EventRewardStat[1] = 30 + (1 * 7);
        // 
        // TempEventData1.EventRewardStatName[2] = "StatName2";
        // TempEventData1.EventRewardStat[2] = 100 + (2 * 2);
        // 
        // TempEventData1.EventRewardMoney = 5247 + (3 * 4);
        // 
        // SelectEventClassInfo.Add(TempEventData1);
        // ////
        // TempEventData2.EventClassName = "test 2";
        // TempEventData2.EventDate[0] = 1;
        // 
        // TempEventData2.EventDate[1] = 3;
        // 
        // TempEventData2.IsPossibleUseEvent = false;
        // TempEventData2.IsFixedEvent = false;      // �����̺�Ʈ���� �����̺�Ʈ���� ������ Ű����
        // TempEventData2.EventRewardStatName[0] = "StatName0";
        // TempEventData2.EventRewardStat[0] = 5 + (1 * 3);
        // 
        // TempEventData2.EventRewardStatName[1] = "StatName1";
        // TempEventData2.EventRewardStat[1] = 30 + (1 * 7);
        // 
        // TempEventData2.EventRewardStatName[2] = "StatName2";
        // TempEventData2.EventRewardStat[2] = 100 + (2 * 2);
        // 
        // TempEventData2.EventRewardMoney = 5247 + (3 * 4);
        // 
        // SelectEventClassInfo.Add(TempEventData2);
        // ////
        // TempEventData3.EventClassName = "test 3";
        // TempEventData3.EventDate[0] = 1;
        // 
        // TempEventData3.EventDate[1] = 5;
        // 
        // TempEventData3.IsPossibleUseEvent = false;
        // TempEventData3.IsFixedEvent = false;      // �����̺�Ʈ���� �����̺�Ʈ���� ������ Ű����
        // TempEventData3.EventRewardStatName[0] = "StatName0";
        // TempEventData3.EventRewardStat[0] = 5 + (1 * 3);
        // 
        // TempEventData3.EventRewardStatName[1] = "StatName1";
        // TempEventData3.EventRewardStat[1] = 30 + (1 * 7);
        // 
        // TempEventData3.EventRewardStatName[2] = "StatName2";
        // TempEventData3.EventRewardStat[2] = 100 + (2 * 2);
        // 
        // TempEventData3.EventRewardMoney = 5247 + (3 * 4);
        // 
        // SelectEventClassInfo.Add(TempEventData3);

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �ٲ� ������ ������ ���� �� �� �� ���� ���� �̺�Ʈ ������ �ް���
        if (IsSetEventList == false && month != GameTime.Instance.FlowTime.NowMonth)
        {
            month = GameTime.Instance.FlowTime.NowMonth;

            PutOnFixedEventData(month); //�����̺�Ʈ
            PutOnPossibleEventData(month); // ���ð����̺�Ʈ �־��ֱ�
            InGameUI.Instance.DrawEventScreenUIText();
        }

        if (EventSetPanel.activeSelf == true)
        {
            NowMoneyText.text = PlayerInfo.Instance.MyMoney.ToString();
            NowSpecialPointText.text = PlayerInfo.Instance.SpecialPoint.ToString();
        }
    }

    // ���ǿ� �´� �����̺�Ʈ�����͸� MyFixedEventList �� �־��ֱ�
    public void PutOnFixedEventData(int month)
    {
        GameObject CalenderEventList; // �����̺�Ʈ�� �θ�

        // ������ƮǮ�� �ٽ� �ֱ�
        // �̹� null�̸� �Ѿ��
        if (m_ParentCalenderScroll.transform.childCount != 0)
        {
            for (int i = 0; i < m_ParentCalenderScroll.childCount; i++)
            {
                MailObjectPool.ReturnFixedEventObject(m_ParentCalenderScroll.gameObject);
            }
        }

        // �����̺�Ʈ -> �� ���Ǹ� ���缭 MyFixedEventList�� ������ �ȴ�
        for (int i = 0; i < SuddenEventClassInfo.Count; i++)
        {
            // - ���� & �����̺�Ʈ���� & ��밡������
            if (month == SuddenEventClassInfo[i].SuddenEventDate[1]
                && SuddenEventClassInfo[i].SuddenEventOpen)
            {
                // �θ� �ȿ� ��ü�� �ű��
                CalenderEventList = MailObjectPool.GetFixedEventObject(m_ParentCalenderScroll);
                CalenderEventList.transform.localScale = new Vector3(1f, 1f, 1f);
                ThisMonthMySelectEventList.Add(SuddenEventClassInfo[i]);

                EventSchedule.Instance.ShowFixedEventOnCalender(); // ���� �̺�Ʈ �����α�
            }
        }
    }

    // ���ð��� �̺�Ʈ ����Ʈ�� ���� �ֱ�
    public void PutOnPossibleEventData(int month)
    {
        GameObject PossibleEventList; // ���ð����̺�Ʈ�� �θ�

        int tempScrollChild = 0;

        for (int i = 0; i < SelectEventClassInfo.Count; i++)
        {
            // int statArrow = 0;

            // ���� �̺�Ʈ -> �����̺�Ʈ && �� �޿� ��� ��������
            if ((month == SelectEventClassInfo[i].SuddenEventDate[1] &&
                 SelectEventClassInfo[i].SuddenEventOpen == true))
            {
                PossibleEventList = MailObjectPool.GetPossibleEventObject(m_PossibleParentScroll); // ���⼭ �ٲ���� ��?
                PossibleEventList.transform.localScale = new Vector3(1f, 1f, 1f);

                PossibleEventList.name = SelectEventClassInfo[i].SuddenEventID.ToString();
                PossibleEventList.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    SelectEventClassInfo[i].SuddenEventID.ToString();

                // �̺�Ʈ ��뿡 ��� ��� �߰� �ʿ�

                int eventStatCount = 0;

                //for (int j = 0; j < SelectEventClassInfo[i].EventRewardStat.Length; j++)
                //{
                //    if (SelectEventClassInfo[i].EventRewardStat[j] != 0)
                //    {
                //        // ������ ������ ���� ������ ���� ��� �켱�� �����ϰ� �ϰ���. ���̱⸸ �ϴ� ���̹Ƿ� �켱�� ����
                //        // PossibleEventList.transform.GetChild(3).GetChild(statArrow).GetChild(1).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventRewardStatName[j];
                //        PossibleEventList.transform.GetChild(3).GetChild(statArrow).GetChild(2).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventRewardStat[j].ToString();
                //        statArrow += 1;
                //
                //        eventStatCount += 1;
                //    }
                //}

                // ������ �ʴ� stat ĭ�� ���ֱ� ���� ó��
                if (eventStatCount == 0)
                {
                    PossibleEventList.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                    PossibleEventList.transform.GetChild(3).GetChild(1).gameObject.SetActive(false); //  �� �̷��� �´ٸ� �°���
                }
                else if (eventStatCount == 1)
                {
                    PossibleEventList.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                }

                // �Ӵ϶� ����� ����Ʈ ������ ó��
                // if (SelectEventClassInfo[i].EventRewardMoney != 0 && SelectEventClassInfo[i].EventRewardSpecialPoint == 0)
                // {
                //     PossibleEventList.transform.GetChild(3).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventRewardMoney.ToString();
                //     PossibleEventList.transform.GetChild(3).GetChild(2).GetChild(3).gameObject.SetActive(false);
                // }
                // else if (SelectEventClassInfo[i].EventRewardMoney == 0 && SelectEventClassInfo[i].EventRewardSpecialPoint != 0)
                // {
                //     PossibleEventList.transform.GetChild(3).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventRewardSpecialPoint.ToString();
                //     PossibleEventList.transform.GetChild(3).GetChild(2).GetChild(3).gameObject.SetActive(false);
                // }
                // else if (SelectEventClassInfo[i].EventRewardMoney != 0 && SelectEventClassInfo[i].EventRewardSpecialPoint != 0)
                // {
                //     PossibleEventList.transform.GetChild(3).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventRewardMoney.ToString();
                //     PossibleEventList.transform.GetChild(3).GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventRewardSpecialPoint.ToString();
                // }
                // else if (SelectEventClassInfo[i].EventRewardMoney == 0 && SelectEventClassInfo[i].EventRewardSpecialPoint == 0)
                // {
                //     PossibleEventList.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
                // }

                // ���ð��� �̺�Ʈ ��ũ�Ѻ信�� ��ư�� Ŭ���� ���� �Լ��� �Ѿ���� �Ѵ�.
                PossibleEventList.GetComponent<Button>().onClick.AddListener(WhenISelectedPossibleEvent);

                PossibleChooseEventClassList.Add(SelectEventClassInfo[i]);

                tempScrollChild += 1;
            }

            // statArrow = 0;
        }

        IsSetEventList = true;
    }


    /// <summary>
    /// ���⼭ �ؾ� �� �� : ���� ������ MyEventList üũ�ؼ� �ִ� ������ ���� ���� ���������� ����. 
    /// �ƿ����� ���� / �����ϴ°� 
    /// �ڵ� ����
    /// </summary>
    /// <param name="_NowEvent"></param>
    public void AfterCheckEventListDecideAddDataOrDeleteData(GameObject _NowEvent)
    {
        if (ThisMonthMySelectEventList.Count != 0) // �ش� ��� �̺�Ʈ�� �����Ͱ� ���� �Ǿ�������
        {
            for (int i = 0; i < ThisMonthMySelectEventList.Count; i++)
            {
                Color color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
                _NowEvent.GetComponent<Outline>().effectColor = color;
                _NowEvent.GetComponent<Outline>().effectDistance = new Vector2(10f, 10f);

                if (ClickedEventDate != null)
                {
                    ClickedEventDate.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    ClickedEventDate.transform.GetComponent<Button>().interactable = true;
                }

                ThisMonthMySelectEventList.RemoveAt(i); // �����ϱ�
                _PrevSelect = null;
            }
        }
        else // �ش� ��� �̺�Ʈ�� �����Ͱ� ���� �ȵǾ�������
        {
            // ������ ��� �����ų �Լ�
            for (int i = 0; i < PossibleChooseEventClassList.Count; i++)
            {
                //if (_NowEvent.name == PossibleChooseEventClassList[i].EventClassName)
                //{
                //    //// ���⼭ �ӽ÷� ���� ������ �����͸� ��Ƶд�
                //    TempIChoosed = PossibleChooseEventClassList[i];
                //    EventSchedule.Instance.tempEventList = PossibleChooseEventClassList[i];
                //
                //    _NowEvent.transform.GetComponent<TextMeshProUGUI>().text = TempIChoosed.EventClassName;
                //}
            }
        }
    }

    public GameObject ClickedEventDate;
    int nowSelectedEventListCount = 0;

    public void ResetSelectedEvent()
    {
        for (int i = 0; i < m_PossibleParentScroll.childCount; i++)
        {
            Color color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
            m_PossibleParentScroll.GetChild(i).gameObject.GetComponent<Outline>().effectColor = color;
            m_PossibleParentScroll.GetChild(i).gameObject.GetComponent<Outline>().effectDistance =
                new Vector2(10f, 10f);

            PayMoneyText.text = "";
            PaySpecialPointText.text = "";
        }

        for (int i = 0; i < m_PossibleParentScroll.childCount; i++)
        {
            m_PossibleParentScroll.GetChild(i).GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < m_ParentCalenderButton.transform.childCount; i++)
        {
            for (int j = 0; j < PossibleChooseEventClassList.Count; j++)
            {
                // if (PossibleChooseEventClassList[j].EventClassName == m_ParentCalenderButton.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text)
                {
                    m_ParentCalenderButton.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    m_ParentCalenderButton.transform.GetChild(i).GetComponent<Button>().interactable = true;
                }
            }
        }

        PrevIChoosedEvent.Clear();

        for (int i = 0; i < ThisMonthMySelectEventList.Count; i++)
        {
            //if (ThisMonthMySelectEventList[i].IsFixedEvent == false)
            {
                ThisMonthMySelectEventList.RemoveAt(i);
            }
        }


        EventSchedule.Instance.TempEventData = null;

        EventSchedule.Instance.nowPossibleCount = 2; // ���� ���� Ƚ��
        EventSchedule.Instance._nowPossibleCountImg.transform.GetChild(0).gameObject.SetActive(true);
        EventSchedule.Instance._nowPossibleCountImg.transform.GetChild(1).gameObject.SetActive(true);
    }

    void CheckListOutLine(GameObject nowclick, GameObject prevselect)
    {
        // �ƿ����� ǥ��
        if (_PrevSelect == null)
        {
            Debug.Log("���缱�� �ƿ����� �۵�!");
            Color color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            nowclick.GetComponent<Outline>().effectColor = color;
            nowclick.GetComponent<Outline>().effectDistance = new Vector2(10f, 10f);
        }
        else if (nowclick != _PrevSelect)
        {
            Color color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            nowclick.GetComponent<Outline>().effectColor = color;
            nowclick.GetComponent<Outline>().effectDistance = new Vector2(10f, 10f);

            color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
            _PrevSelect.GetComponent<Outline>().effectColor = color;
            _PrevSelect.GetComponent<Outline>().effectDistance = new Vector2(10f, 10f);
        }
    }

    // ���� ���ð����� �̺�Ʈ�� Ŭ�� ���� �� �ƿ����� üũ, 
    public void WhenISelectedPossibleEvent()
    {
        GameObject _NowEvent = EventSystem.current.currentSelectedGameObject;

        Debug.Log("�̺�Ʈ ����");

        if (PrevIChoosedEvent.Count == 0 || PrevIChoosedEvent.Count == 1) // ���� �̺�Ʈ ���� ����
        {
            // �ƿ����� ǥ��
            CheckListOutLine(_NowEvent, _PrevSelect);

            // ������ �̺�Ʈ �ӽ�����
            for (int i = 0; i < PossibleChooseEventClassList.Count; i++)
            {
                // if (_NowEvent.name == PossibleChooseEventClassList[i].EventClassName)
                // {
                // 
                //     if (PossibleChooseEventClassList[i].NeedMoney < PlayerInfo.Instance.MyMoney &&
                //         PossibleChooseEventClassList[i].NeedSpecialPoint < PlayerInfo.Instance.SpecialPoint)
                //     {
                //         NoticeMoneyText.gameObject.SetActive(false);
                //         // PayMoneyText.text = PossibleChooseEventClassList[i].NeedMoney.ToString();
                //         // PaySpecialPointText.text = PossibleChooseEventClassList[i].NeedSpecialPoint.ToString();
                // 
                //         _PrevSelect = _NowEvent;
                // 
                //         //// ���⼭ �ӽ÷� ���� ������ �����͸� ��Ƶд�
                //         TempIChoosed = PossibleChooseEventClassList[i];
                // 
                //         EventSchedule.Instance.tempEventList = PossibleChooseEventClassList[i];
                //     }
                //     else
                //     {
                //         NoticeMoneyText.gameObject.SetActive(true);
                //     }
                // }
            }
        }
        else if (PrevIChoosedEvent.Count == 2) // ���� �̺�Ʈ ���Ѱ��� 2�� �� ��
        {
            // �ƿ����� ǥ��
            CheckListOutLine(_NowEvent, _PrevSelect);

            _PrevSelect = _NowEvent;
        }
    }

    // ��밡�� �̺�Ʈ�� ��� �ߴ��� üũ�ؼ� �����ִ� �Լ�
    public void CountPossibleEventSetting(GameObject _NowEvent)
    {
        // ���� ������ �̺�Ʈ�� ������ ���� ���� ���ΰ� �޶����� ���ǹ�
        if (PrevIChoosedEvent.Count != 0)
        {
            for (int i = 0; i < PrevIChoosedEvent.Count; i++)
            {
                // if (_NowEvent.name == PrevIChoosedEvent[i].EventClassName)
                // {
                //     EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = false;
                //     break;
                // }
                // else if (_NowEvent.name != PrevIChoosedEvent[i].EventClassName)
                // {
                //     EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = true;
                // }
            }

            if (PrevIChoosedEvent.Count == 2)
            {
                EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = false;
            }
        }
        else if (PrevIChoosedEvent.Count == 0)
        {
            EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = true;
        }
    }

    // ���� �̺�Ʈ - ���� ���� �����ߴ� �̺�Ʈ���� ��� �� ����Ʈ���� �����ϱ� /  �޷�â �������� ������ �����ϱ�
    public void PushCancleButton()
    {
        Color color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
        _PrevSelect.GetComponent<Outline>().effectColor = color;
        _PrevSelect.GetComponent<Outline>().effectDistance = new Vector2(10f, 10f);

        for (int i = 0; i < PrevIChoosedEvent.Count; i++)
        {
            // if (PrevIChoosedEvent[i].EventClassName == TempIChoosed.EventClassName)
            // {
            //     EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = true;
            //     PrevIChoosedEvent.RemoveAt(i);
            // 
            //     for (int j = 0; j < m_ParentCalenderScroll.childCount; j++)
            //     {
            //         if (m_ParentCalenderScroll.GetChild(j).name == TempIChoosed.EventClassName)
            //         {
            //             MailObjectPool.ReturnSelectedEventObject(m_ParentCalenderScroll.GetChild(j).gameObject);
            // 
            //             break;
            //         }
            //     }
            // }
        }
    }

    // ����, ���� �̺�Ʈ�� ù��° ���� ���� ���� �־����Ƿ� ������ �ٷ� �޷�â�� Scroll View�� �ֱ�
    // ������ Possible Scrollview �� �ֱ�
    public void PutMySelectedEventOnCalender()
    {
        // PrevIChoosedEvent.Add(TempIChoosed);        // ���⼭ �ӽ÷� ���� ������ �����͸� ��Ƶд�

        // �����̺�Ʈ ����
        for (int i = 0; i < PossibleChooseEventClassList.Count; i++)
        {
            int statArrow = 0;

            // if (TempIChoosed.EventRewardMoney != 0)
            // {
            //     statArrow += 1;
            // }
            // 
            // for (int j = 0; j < TempIChoosed.EventRewardStat.Length; j++)
            // {
            //     if (TempIChoosed.EventRewardStat[j] != 0)
            //     {
            // 
            //         statArrow += 1;
            //     }
            //     else
            //     {
            //         statArrow += 1;
            //     }
            // }
        }
    }

    // ������Ʈ�� �ٽ� ������Ʈ Ǯ�� �ǵ��� ����ϴµ� ������ ť�� ��ť �ؾ���
    public void ReturnEventPrefabToPool()
    {
        int _DestroyMailObj = m_PossibleParentScroll.childCount; // 

        for (int i = _DestroyMailObj - 1; i >= 0; i--)
        {
            MailObjectPool.ReturnSelectedEventObject(m_PossibleParentScroll.GetChild(0).gameObject);
        }
    }

    //  �����̺�Ʈ ���� �ΰ� ����� �д� - �̰� �ƴϴ�!
    public void PutMyEventListOnCalenderPage()
    {
        GameObject SetEventList;

        for (int i = 0; i < 2; i++)
        {
            SetEventList = MailObjectPool.GetPossibleEventObject(m_ParentCalenderScroll);
            SetEventList.transform.localScale = new Vector3(1f, 1f, 1f);

            SetEventList.transform.gameObject.SetActive(false);
        }
    }

    // ��¥�� Ŭ�� �� ���� ��ư�� ���� ���� �̺�Ʈ�� ������� Ȯ���� ���´�
    public void IfIGaveTheEventDate()
    {
        if (EventSchedule.Instance.TempEventData.SuddenEventDate[2] != 0)
        {
            GameObject SetEventList;
            SetEventList = MailObjectPool.GetSelectedEventObject(m_ParentCalenderScroll); //������ �̺�Ʈ ������ ����������
            SetEventList.transform.localScale = new Vector3(1f, 1f, 1f);

            PrevIChoosedEvent.Add(TempIChoosed); // ���⼭ �ӽ÷� ���� ������ �����͸� ��Ƶд�

            // ���� �̺�Ʈ ���� �ֱ�
            // SetEventList.name = TempIChoosed.EventClassName;
            SetEventList.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                TempIChoosed.SuddenEventDate[2].ToString() + "����"; // �ش� ����

            int dayIndex = TempIChoosed.SuddenEventDate[3];
            switch (dayIndex)
            {
                case 1:
                    SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "������";
                    break;
                case 2:
                    SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ȭ����";
                    break;
                case 3:
                    SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "������";
                    break;
                case 4:
                    SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "�����";
                    break;
                case 5:
                    SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "�ݿ���";
                    break;
            }
            // SetEventList.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TempIChoosed.EventClassName;

            // ������ ���� �Ŀ� �ش� �г� setActive(false) ���ֱ�
            _PopOfEventCalenderPanel.TurnOffUI();
        }
    }

    public void ResetMyEventList()
    {
        int possibleEvent = m_PossibleParentScroll.childCount;
        // 
        for (int i = possibleEvent - 1; i >= 0; i--)
        {
            MailObjectPool.ReturnPossibleEventObject(m_PossibleParentScroll.GetChild(i).gameObject);
        }

        PossibleChooseEventClassList.Clear();
    }
}