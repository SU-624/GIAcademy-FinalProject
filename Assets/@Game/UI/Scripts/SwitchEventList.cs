using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SaveEventData
{
    public int[] EventDate = new int[4];                               // 이벤트가 실행될 날짜 배열( 년, 월, 주, 요일)
    public bool IsPossibleUseEvent = false;
    public bool IsFixedEvent;                                               // 고정인지 선택인지 구별할 스트링
    public int EventNumber;                                                 // 해금 이벤트 와 인게임 조건의 번호를 비교해서 같으면 해금
    public string EventClassName;                                           // 이벤트 이름 -> 해금을 위해 조건과 해금의 이름 or 숫자를 갖게? 하면 되지 않을까?
    public string EventInformation;                                         // 이벤트 설명 내용
    public bool IsPopUp = false;

    public const int RewardStatCount = 6;                                   // 보상스탯의 갯수는 6개

    public string[] EventRewardStatName = new string[RewardStatCount];      // 보상 - 스탯이름
    public float[] EventRewardStat = new float[RewardStatCount];            // 보상 - 스탯수치
    public int EventRewardMoney;                                            // 보상 - 머니

}

/// <summary>
/// 2023. 01. 16 Mang
/// 
/// 여기서 이제 만들어둔 프리팹에 데이터를 넣고, 데이터가 들어간 오브젝트를 옮기고, 다 해줄 것이다
/// 싱글턴으로 만들어서, 가져다 쓰기 필요한 곳에서 쓸 수 있도록
/// </summary>
public class SwitchEventList : MonoBehaviour
{
    private static SwitchEventList _instance = null;

    // Inspector 창에 연결해줄 변수들
    [Tooltip("이벤트 선택 창의 사용가능 이벤트목록을 만들기 위한 프리팹변수들")]
    [Header("EventList Prefab")]
    public GameObject m_PossibleEventprefab;     // 
    public Transform m_PossibleParentScroll;      // 

    [Space(10f)]
    [Tooltip("이벤트 선택 창 눌렀을 때 나올 설명 창의 부모")]
    [Header("EventList Prefab")]
    public GameObject m_EventInfoParent;     // 

    [Space(10f)]
    [Tooltip("달력 창의 선택된 이벤트 리스트를 만들 프리팹변수들")]
    [Header("SelectdEvent&SetOkEvent Prefab")]
    public GameObject m_FixedPrefab;
    public GameObject m_SelectedPrefab;
    public Transform m_ParentCalenderScroll;

    [Space(10f)]
    [Tooltip("달력 창의 선택된 이벤트 설명")]
    [Header("SelectdEvent On Calender")]
    public GameObject m_EventInfoOnCaledner;

    [Space(10f)]
    [Tooltip("이벤트 선택 때 취소 할 수 있는 버튼")]
    [Header("Event Setting Screen CancleButton")]
    public GameObject m_CancleEventButton;

    [Space(10f)]
    [Header("EventInfoWhiteScreen")]
    public GameObject WhiteScreen;

    // Json 파일을 파싱해서 그 데이터들을 다 담아 줄 리스트 변수
    // 이 변수들도 EventSchedule 의 Instance.변수 들에 넣어주고 쓰도록 하자
    public List<SaveEventData> SelectEventClassInfo = new List<SaveEventData>();              // 전체 선택 이벤트
    public List<SaveEventData> FixedEventClassInfo = new List<SaveEventData>();               // 전체 고정 이벤트
    //

    public List<SaveEventData> PossibleChooseEventClassList = new List<SaveEventData>();      //  사용가능한 선택이벤트 목록
    public List<SaveEventData> MyEventList = new List<SaveEventData>();                       // 현재 나의 이벤트 목록

    public List<SaveEventData> PrevIChoosedEvent = new List<SaveEventData>();                 // 현재 선택한 이벤트 담아 줄 임시 변수
    public SaveEventData TempIChoosed;

    int month;

    const int FixedEvenetCount = 3;
    const int SelectedEventCount = 2;
    const int PossibleEventCount = 10;

    public bool IsSetEventList = false;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 1. 제이슨 파일 전체 이벤트리스트 변수에 담기

        // 고정이벤트
        SaveEventData TempFixedData = new SaveEventData();
        SaveEventData TempFixedData1 = new SaveEventData();

        TempFixedData.EventClassName = "FixedTestEvent0";
        TempFixedData.EventDate[0] = 1;
        TempFixedData.EventDate[1] = 3;     // 3월
        TempFixedData.EventDate[2] = 2;     // 2주차
        TempFixedData.EventDate[3] = 4;     // 목요일

        TempFixedData.EventInformation = "이벤트설명";
        TempFixedData.IsFixedEvent = true;
        TempFixedData.IsPossibleUseEvent = true;

        TempFixedData.EventRewardStatName[0] = "StatName0";
        TempFixedData.EventRewardStat[0] = 2 + (1 * 3);

        TempFixedData.EventRewardStatName[1] = "StatName1";
        TempFixedData.EventRewardStat[1] = 8 + (3 * 7);

        TempFixedData.EventRewardStatName[2] = "StatName2";
        TempFixedData.EventRewardStat[2] = 46 + (2 * 2);

        TempFixedData.EventRewardMoney = 386 + (1 * 4);

        FixedEventClassInfo.Add(TempFixedData);
        ////
        TempFixedData1.EventClassName = "FixedTestEvent1";
        TempFixedData1.EventDate[0] = 1;
        TempFixedData1.EventDate[1] = 4;     // 4월
        TempFixedData1.EventDate[2] = 1;      // 1주차
        TempFixedData1.EventDate[3] = 2;      // 화요일

        TempFixedData1.EventInformation = "이벤트설명";
        TempFixedData1.IsFixedEvent = true;
        TempFixedData1.IsPossibleUseEvent = true;

        TempFixedData1.EventRewardStatName[0] = "StatName0";
        TempFixedData1.EventRewardStat[0] = 2 + (1 * 4);

        TempFixedData1.EventRewardStatName[1] = "StatName1";
        TempFixedData1.EventRewardStat[1] = 8 + (3 * 3);

        TempFixedData1.EventRewardStatName[2] = "StatName2";
        TempFixedData1.EventRewardStat[2] = 46 + (2 * 5);

        TempFixedData1.EventRewardMoney = 386 + (1 * 8);

        FixedEventClassInfo.Add(TempFixedData1);
        ////


        // 선택이벤트
        // (임시로 여기서)2. 전체 이벤트 리스트에서 사용가능한 이벤트들 사용가능이벤트 리스트 변수에 담기
        // 제이슨 파일 생성 전 테스트를 위해 만든 변수
        SaveEventData TempEventData = new SaveEventData();
        SaveEventData TempEventData1 = new SaveEventData();
        SaveEventData TempEventData2 = new SaveEventData();
        SaveEventData TempEventData3 = new SaveEventData();


        // 이벤트 struct 관련 초기화 해주기
        TempEventData.EventClassName = "test 0";
        TempEventData.EventDate[0] = 1;

        TempEventData.EventDate[1] = 3;

        TempEventData.IsPossibleUseEvent = false;
        TempEventData.IsFixedEvent = false;      // 고정이벤트인지 선택이벤트인지 구별할 키워드
        TempEventData.EventRewardStatName[0] = "StatName0";
        TempEventData.EventRewardStat[0] = 5 + (1 * 3);

        TempEventData.EventRewardStatName[1] = "StatName1";
        TempEventData.EventRewardStat[1] = 30 + (1 * 7);

        TempEventData.EventRewardStatName[2] = "StatName2";
        TempEventData.EventRewardStat[2] = 100 + (2 * 2);

        TempEventData.EventRewardMoney = 5247 + (3 * 4);

        SelectEventClassInfo.Add(TempEventData);
        ////
        TempEventData1.EventClassName = "test 1";
        TempEventData1.EventDate[0] = 1;

        TempEventData1.EventDate[1] = 3;
        TempEventData1.IsPossibleUseEvent = false;
        TempEventData1.IsFixedEvent = false;      // 고정이벤트인지 선택이벤트인지 구별할 키워드
        TempEventData1.EventRewardStatName[0] = "StatName0";
        TempEventData1.EventRewardStat[0] = 5 + (1 * 3);

        TempEventData1.EventRewardStatName[1] = "StatName1";
        TempEventData1.EventRewardStat[1] = 30 + (1 * 7);

        TempEventData1.EventRewardStatName[2] = "StatName2";
        TempEventData1.EventRewardStat[2] = 100 + (2 * 2);

        TempEventData1.EventRewardMoney = 5247 + (3 * 4);

        SelectEventClassInfo.Add(TempEventData1);
        ////
        TempEventData2.EventClassName = "test 2";
        TempEventData2.EventDate[0] = 1;

        TempEventData2.EventDate[1] = 3;

        TempEventData2.IsPossibleUseEvent = false;
        TempEventData2.IsFixedEvent = false;      // 고정이벤트인지 선택이벤트인지 구별할 키워드
        TempEventData2.EventRewardStatName[0] = "StatName0";
        TempEventData2.EventRewardStat[0] = 5 + (1 * 3);

        TempEventData2.EventRewardStatName[1] = "StatName1";
        TempEventData2.EventRewardStat[1] = 30 + (1 * 7);

        TempEventData2.EventRewardStatName[2] = "StatName2";
        TempEventData2.EventRewardStat[2] = 100 + (2 * 2);

        TempEventData2.EventRewardMoney = 5247 + (3 * 4);

        SelectEventClassInfo.Add(TempEventData2);
        ////
        TempEventData3.EventClassName = "test 3";
        TempEventData3.EventDate[0] = 1;

        TempEventData3.EventDate[1] = 5;

        TempEventData3.IsPossibleUseEvent = false;
        TempEventData3.IsFixedEvent = false;      // 고정이벤트인지 선택이벤트인지 구별할 키워드
        TempEventData3.EventRewardStatName[0] = "StatName0";
        TempEventData3.EventRewardStat[0] = 5 + (1 * 3);

        TempEventData3.EventRewardStatName[1] = "StatName1";
        TempEventData3.EventRewardStat[1] = 30 + (1 * 7);

        TempEventData3.EventRewardStatName[2] = "StatName2";
        TempEventData3.EventRewardStat[2] = 100 + (2 * 2);

        TempEventData3.EventRewardMoney = 5247 + (3 * 4);

        SelectEventClassInfo.Add(TempEventData3);

    }

    // Update is called once per frame
    void Update()
    {
        // 달이 바뀔 때마다 이전과 현재 비교 후 그 달의 고정 이벤트 정보를 받겠지
        if (IsSetEventList == false && month != GameTime.Instance.FlowTime.NowMonth)
        {
            month = GameTime.Instance.FlowTime.NowMonth;

            PutOnFixedEventData(month);     //고정이벤트
            PutOnPossibleEventData(month);  // 선택가능이벤트 넣어주기
        }
    }

    // 조건에 맞는 고정이벤트데이터를 MyFixedEventList 에 넣어주기
    public void PutOnFixedEventData(int month)
    {
        GameObject CalenderEventList;               // 고정이벤트들 부모

        // 오브젝트풀에 다시 넣기
        // 이미 null이면 넘어가고
        if (m_ParentCalenderScroll.transform.childCount != 0)
        {
            for (int i = 0; i < m_ParentCalenderScroll.childCount; i++)
            {
                MailObjectPool.ReturnFixedEventObject(m_ParentCalenderScroll.gameObject);
            }
        }

        // 고정이벤트 -> 월 조건만 맞춰서 MyFixedEventList에 넣으면 된다
        for (int i = 0; i < FixedEventClassInfo.Count; i++)
        {
            // - 월별 & 고정이벤트인지 & 사용가능한지
            if (month == FixedEventClassInfo[i].EventDate[1]
                && FixedEventClassInfo[i].IsPossibleUseEvent)
            {
                // 부모 안에 객체를 옮기기
                CalenderEventList = MailObjectPool.GetFixedEventObject(m_ParentCalenderScroll);
                CalenderEventList.transform.localScale = new Vector3(1f, 1f, 1f);
                // 고정은 필요한 조건 만큼 걸러지므로 바로 ScrollView 에 오브젝트를 넣는다
                CalenderEventList.name = FixedEventClassInfo[i].EventClassName;

                CalenderEventList.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = FixedEventClassInfo[i].EventDate[2].ToString() + "주차";

                int DayInedx = FixedEventClassInfo[i].EventDate[3];
                // 여기서 숫자에 비교해 요일을 적어준다.        // 여기서 설정완료 버튼 눌렀을 때 왼쪽에 ScrollView 의 고정이벤트 UI 글씨를 나타내준다.
                switch (DayInedx)
                {
                    case 1:
                        CalenderEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "월요일";
                        break;

                    case 2:
                        CalenderEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "화요일";
                        break;

                    case 3:
                        CalenderEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "수요일";
                        break;

                    case 4:
                        CalenderEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "목요일";
                        break;

                    case 5:
                        CalenderEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "금요일";
                        break;
                }
                CalenderEventList.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = FixedEventClassInfo[i].EventClassName;

                MyEventList.Add(FixedEventClassInfo[i]);

                EventSchedule.Instance.ShowFixedEventOnCalender();     // 고정 이벤트 만들어두기
            }
        }
    }

    // 선택가능 이벤트 리스트에 정보 넣기
    public void PutOnPossibleEventData(int month)
    {
        GameObject PossibleEventList;              // 선택가능이벤트들 부모

        m_CancleEventButton.SetActive(false);
        WhiteScreen.SetActive(true);
        int tempScrollChild = 0;

        for (int i = 0; i < SelectEventClassInfo.Count; i++)
        {
            int statArrow = 0;

            // 선택 이벤트 & 선택이벤트인지 & 사용가능한지 / 사용가능한지
            if ((month == SelectEventClassInfo[i].EventDate[1] && SelectEventClassInfo[i].IsPossibleUseEvent == true)
                || SelectEventClassInfo[i].IsPossibleUseEvent == true
                || month == SelectEventClassInfo[i].EventDate[1])
            {
                PossibleEventList = MailObjectPool.GetPossibleEventObject(m_PossibleParentScroll);
                PossibleEventList.transform.localScale = new Vector3(1f, 1f, 1f);

                PossibleEventList.name = SelectEventClassInfo[i].EventClassName;
                PossibleEventList.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventClassName;

                if (SelectEventClassInfo[i].EventRewardMoney != 0)
                {
                    PossibleEventList.transform.GetChild(1).GetChild(statArrow).GetChild(0).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventRewardMoney.ToString();

                    statArrow += 1;
                }

                for (int j = 0; j < SelectEventClassInfo[i].EventRewardStat.Length; j++)
                {
                    if (SelectEventClassInfo[i].EventRewardStat[j] != 0)
                    {
                        PossibleEventList.transform.GetChild(1).GetChild(statArrow).GetChild(0).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventRewardStatName[j];
                        statArrow += 1;
                    }
                    else
                    {
                        PossibleEventList.transform.GetChild(1).GetChild(statArrow).gameObject.SetActive(false);
                        statArrow += 1;
                    }
                }

                // 선택가능 이벤트 스크롤뷰에서 버튼을 클릭시 지정 함수로 넘어가도록 한다.
                PossibleEventList.GetComponent<Button>().onClick.AddListener(ShowISelectedPossibleEvent);

                PossibleChooseEventClassList.Add(SelectEventClassInfo[i]);

                tempScrollChild += 1;
            }

            statArrow = 0;
        }

        IsSetEventList = true;
    }

    // 내가 선택가능한 이벤트를 클릭 했을 때 바로옆에 이벤트 설명창에 선택한 이벤트의 정보를 보여주고, 
    public void ShowISelectedPossibleEvent()
    {
        GameObject _NowEvent = EventSystem.current.currentSelectedGameObject;
        WhiteScreen.SetActive(false);

        Debug.Log("이벤트 선택");

        for (int i = 0; i < PossibleChooseEventClassList.Count; i++)
        {
            int statArrow = 0;

            if (_NowEvent.name == PossibleChooseEventClassList[i].EventClassName)
            {
                m_EventInfoParent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PossibleChooseEventClassList[i].EventClassName;
                m_EventInfoParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PossibleChooseEventClassList[i].EventInformation;

                if (PossibleChooseEventClassList[i].EventRewardMoney != 0)
                {
                    m_EventInfoParent.transform.GetChild(2).GetChild(statArrow).GetChild(0).GetComponent<TextMeshProUGUI>().text = "머니";
                    m_EventInfoParent.transform.GetChild(2).GetChild(statArrow).GetChild(1).GetComponent<TextMeshProUGUI>().text = PossibleChooseEventClassList[i].EventRewardMoney.ToString();

                    statArrow += 1;
                }

                for (int j = 0; j < PossibleChooseEventClassList[i].EventRewardStat.Length; j++)
                {
                    if (SelectEventClassInfo[i].EventRewardStat[j] != 0)
                    {
                        m_EventInfoParent.transform.GetChild(2).GetChild(statArrow).GetChild(0).GetComponent<TextMeshProUGUI>().text = PossibleChooseEventClassList[i].EventRewardStatName[j];
                        m_EventInfoParent.transform.GetChild(2).GetChild(statArrow).GetChild(1).GetComponent<TextMeshProUGUI>().text = PossibleChooseEventClassList[i].EventRewardStat[j].ToString();
                    }
                    else
                    {
                        m_EventInfoParent.transform.GetChild(2).GetChild(statArrow).gameObject.SetActive(false);
                    }
                    statArrow += 1;
                }
                //// 여기서 임시로 내가 선택한 데이터를 담아둔다
                TempIChoosed = PossibleChooseEventClassList[i];
                EventSchedule.Instance.tempEventList = PossibleChooseEventClassList[i];
                EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = true;
            }
        }

        // 내가 선택한 이벤트의 갯수에 따라 선택 여부가 달라지는 조건문
        if (PrevIChoosedEvent.Count != 0)
        {
            for (int i = 0; i < PrevIChoosedEvent.Count; i++)
            {
                if (_NowEvent.name == PrevIChoosedEvent[i].EventClassName)
                {
                    EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = false;
                    m_CancleEventButton.SetActive(true);

                    break;
                }
                else if (_NowEvent.name != PrevIChoosedEvent[i].EventClassName)
                {
                    EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = true;
                    m_CancleEventButton.SetActive(false);
                }
            }

            if (PrevIChoosedEvent.Count == 2)
            {
                EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = false;
            }
        }
        else if (PrevIChoosedEvent.Count == 0)
        {
            EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = true;
            m_CancleEventButton.SetActive(false);
        }


        // 이벤트 설명 창을 깔끔하게 안보이게 하다가 내용을 보이게 하기 위해 어거지로 만든 배경
        if (WhiteScreen.activeSelf == true)
        {
            WhiteScreen.SetActive(false);
        }
    }

    // 선택 이벤트 - 내가 전에 선택했던 이벤트들을 모아 둔 리스트에서 삭제하기 /  달력창 정보에서 프리팹 삭제하기
    public void PushCancleButton()
    {
        for (int i = 0; i < PrevIChoosedEvent.Count; i++)
        {
            if (PrevIChoosedEvent[i].EventClassName == TempIChoosed.EventClassName)
            {
                EventSchedule.Instance.Choose_Button.transform.GetComponent<Button>().interactable = true;
                PrevIChoosedEvent.RemoveAt(i);

                for (int j = 0; j < m_ParentCalenderScroll.childCount; j++)
                {
                    if (m_ParentCalenderScroll.GetChild(j).name == TempIChoosed.EventClassName)
                    {
                        MailObjectPool.ReturnSelectedEventObject(m_ParentCalenderScroll.GetChild(j).gameObject);

                        break;
                    }
                }
            }
        }
    }

    // 고정, 선택 이벤트를 첫번째 조건 월에 맞춰 넣었으므로 고정은 바로 달력창의 Scroll View에 넣기
    // 선택은 Possible Scrollview 에 넣기
    public void PutMySelectedEventOnCalender()
    {
        // PrevIChoosedEvent.Add(TempIChoosed);        // 여기서 임시로 내가 선택한 데이터를 담아둔다

        // 선택이벤트 정보
        for (int i = 0; i < PossibleChooseEventClassList.Count; i++)
        {
            int statArrow = 0;

            m_EventInfoOnCaledner.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = month.ToString();       // 해당 월
            m_EventInfoOnCaledner.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = TempIChoosed.EventClassName;      // 선택 이벤트 이름
            m_EventInfoOnCaledner.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = TempIChoosed.EventInformation;    // 선택 이벤트 설명

            if (TempIChoosed.EventRewardMoney != 0)
            {
                m_EventInfoOnCaledner.transform.GetChild(4).GetChild(statArrow).GetChild(0).GetComponent<TextMeshProUGUI>().text = "머니";
                m_EventInfoOnCaledner.transform.GetChild(4).GetChild(statArrow).GetChild(1).GetComponent<TextMeshProUGUI>().text = TempIChoosed.EventRewardMoney.ToString();
                statArrow += 1;
            }

            for (int j = 0; j < TempIChoosed.EventRewardStat.Length; j++)
            {
                if (TempIChoosed.EventRewardStat[j] != 0)
                {

                    m_EventInfoOnCaledner.transform.GetChild(4).GetChild(statArrow).GetChild(0).GetComponent<TextMeshProUGUI>().text = TempIChoosed.EventRewardStatName[j];
                    m_EventInfoOnCaledner.transform.GetChild(4).GetChild(statArrow).GetChild(1).GetComponent<TextMeshProUGUI>().text = TempIChoosed.EventRewardStat[j].ToString();
                    statArrow += 1;
                }
                else
                {
                    m_EventInfoOnCaledner.transform.GetChild(4).GetChild(statArrow).gameObject.SetActive(false);
                    statArrow += 1;
                }
            }
        }
    }

    // 오브젝트를 다시 오브젝트 풀에 되돌려 줘야하는데 각각의 큐애 인큐 해야함
    public void ReturnEventPrefabToPool()
    {
        int _DestroyMailObj = m_ParentCalenderScroll.childCount;        // 

        for (int i = _DestroyMailObj - 1; i >= 0; i--)
        {
            if (m_ParentCalenderScroll.GetChild(i).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "고정")
            {
                MailObjectPool.ReturnFixedEventObject(m_ParentCalenderScroll.GetChild(i).gameObject);
            }
            else if (m_ParentCalenderScroll.GetChild(i).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "선택")
            {
                MailObjectPool.ReturnSelectedEventObject(m_ParentCalenderScroll.GetChild(i).gameObject);
            }
        }
    }

    //  선택이벤트 가라를 두개 만들어 둔다 - 이거 아니다!
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

    // public void PutSelectEventOnCalender()
    // {
    //     for (int i = 0; i < MyEventList.Count; i++)
    //     {
    //         if (MyEventList[i].IsFixedEvent == false)
    //         {
    //             string temp = m_ParentCalenderScroll.GetChild(i).name;
    //             if (temp == MyEventList[i].EventClassName)
    //             {
    //                 m_ParentCalenderScroll.transform.GetChild(i).gameObject.SetActive(true);
    //             }
    //         }
    //     }
    // }

    // 날짜를 클릭 후 실행 버튼을 눌러 내가 이벤트를 사용할지 확정을 짓는다
    public void IfIGaveTheEventDate()
    {
        if (EventSchedule.Instance.tempEventList.EventDate[2] != 0)
        {
            GameObject SetEventList;
            SetEventList = MailObjectPool.GetSelectedEventObject(m_ParentCalenderScroll);       //선택한 이벤트 프리팹 생성했으니
            SetEventList.transform.localScale = new Vector3(1f, 1f, 1f);

            PrevIChoosedEvent.Add(TempIChoosed);        // 여기서 임시로 내가 선택한 데이터를 담아둔다

            // 선택 이벤트 정보 넣기
            SetEventList.name = TempIChoosed.EventClassName;
            SetEventList.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = TempIChoosed.EventDate[2].ToString() + "주차";       // 해당 주차

            int dayIndex = TempIChoosed.EventDate[3];
            switch(dayIndex)
                {
                case 1: SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "월요일"; break;
                case 2: SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "화요일"; break;
                case 3: SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "수요일"; break;
                case 4: SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "목요일"; break;
                case 5: SetEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "금요일"; break;

            }
            SetEventList.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = TempIChoosed.EventClassName;


            // 데이터 넣은 후에 해당 패널 setActive(false) 해주기
            _PopOfEventCalenderPanel.TurnOffUI();

            WhiteScreen.SetActive(true);
        }
    }

    public void ResetMyEventList()
    {
        // ReturnEventPrefabToPool();

        int possibleEvent = m_PossibleParentScroll.childCount;
        // 
        for (int i = possibleEvent - 1; i >= 0; i--)
        {
            MailObjectPool.ReturnPossibleEventObject(m_PossibleParentScroll.GetChild(i).gameObject);
        }

        PossibleChooseEventClassList.Clear();
    }
}

