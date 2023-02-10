using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveEventData
{
    public string[] EventDay = new string[4];                               // 이벤트가 실행될 날짜 배열( 년, 월, 주, 요일)
    public bool IsPossibleUseEvent = false;
    public bool IsFixedEvent;                                               // 고정인지 선택인지 구별할 스트링
    public int EventNumber;                                                 // 해금 이벤트 와 인게임 조건의 번호를 비교해서 같으면 해금
    public string EventClassName;                                           // 이벤트 이름 -> 해금을 위해 조건과 해금의 이름 or 숫자를 갖게? 하면 되지 않을까?
    public string EventInformation;                                         // 이벤트 설명 내용

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
    [Tooltip("달력 창의 선택된 이벤트 리스트를 만들 프리팹변수들")]
    [Header("SelectdEvent&SetOkEvent Prefab")]
    public GameObject m_FixedPrefab;
    public GameObject m_SelectedPrefab;
    public Transform m_ParentCalenderScroll;


    // Json 파일을 파싱해서 그 데이터들을 다 담아 줄 리스트 변수
    // 이 변수들도 EventSchedule 의 Instance.변수 들에 넣어주고 쓰도록 하자
    public List<SaveEventClassData> SelectEventClassInfo = new List<SaveEventClassData>();       // 전체 선택 이벤트
    public List<SaveEventClassData> FixedEventClassInfo = new List<SaveEventClassData>();           // 전체 고정 이벤트
    //

    public List<SaveEventClassData> PossibleChooseEventClassList = new List<SaveEventClassData>();      //  사용가능한 선택이벤트 목록

    public SaveEventClassData IfIChoosedEvent;           // 현재 선택한 이벤트 담아 줄 임시 변수

    string month;

    const int FixedEvenetCount = 3;
    const int SelectedEventCount = 2;
    const int PossibleEventCount = 10;

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
        month = GameTime.Instance.FlowTime.NowMonth;
        // 1. 제이슨 파일 전체 이벤트리스트 변수에 담기

        // 고정이벤트
        SaveEventClassData TempFixedData = new SaveEventClassData();
        TempFixedData.EventClassName = "FixedTestEvent0";
        TempFixedData.EventDay[0] = "1";
        TempFixedData.EventDay[1] = GameTime.Instance.Month[2];     // 3월
        TempFixedData.EventDay[2] = GameTime.Instance.Week[1];      // 2주차
        TempFixedData.EventDay[3] = GameTime.Instance.Day[3];       // 목요일

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

        // 선택이벤트
        // (임시로 여기서)2. 전체 이벤트 리스트에서 사용가능한 이벤트들 사용가능이벤트 리스트 변수에 담기
        for (int i = 0; i < 5; i++)
        {
            int index = i;
            // 테스트
            // 제이슨 파일 생성 전 테스트를 위해 만든 변수
            SaveEventClassData TempEventData = new SaveEventClassData();

            // 이벤트 struct 관련 초기화 해주기
            TempEventData.EventClassName = "test" + i;
            // if (TempEventData.EventReward.Length > 1)
            // {
            //     for (int j = 0; j < TempEventData.EventReward.Length; j++)
            //     {
            //     }
            // }

            TempEventData.EventDay[0] = "1년";

            if (4 <= index)
            {
                index = 0;
            }
            TempEventData.EventDay[1] = GameTime.Instance.Month[index];
            TempEventData.EventDay[2] = GameTime.Instance.Week[index];
            TempEventData.EventDay[3] = GameTime.Instance.Day[index];

            TempEventData.IsPossibleUseEvent = true;
            TempEventData.IsFixedEvent = false;      // 고정이벤트인지 선택이벤트인지 구별할 키워드
            TempEventData.EventRewardStatName[0] = "StatName0";
            TempEventData.EventRewardStat[0] = 5 + (i * 3);

            TempEventData.EventRewardStatName[1] = "StatName1";
            TempEventData.EventRewardStat[1] = 30 + (i * 7);

            TempEventData.EventRewardStatName[2] = "StatName2";
            TempEventData.EventRewardStat[2] = 100 + (i * 2);

            TempEventData.EventRewardMoney = 5247 + (i * 4);

            SelectEventClassInfo.Add(TempEventData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 달이 바뀔 때마다 이전과 현재 비교 후 그 달의 고정 이벤트 정보를 받겠지
        if (month != GameTime.Instance.FlowTime.NowMonth)
        {
            month = GameTime.Instance.FlowTime.NowMonth;

            PutOnFixedEventData(month);     //고정이벤트
            PutOnPossibleEventData(month);  // 선택가능이벤트 넣어주기
        }
    }

    // 데이터가 있는지 확인 후 필요한 오브젝트풀의 오브젝트 갯수만큼 옮기기
    public void PutOnFixedEventList(string month)
    {
        // FixedEventClassInfo 여기에 정보가 들어있고
        // if ()
        {

        }
    }


    // 조건에 맞는 고정이벤트데이터를 MyFixedEventList 에 넣어주기
    public void PutOnFixedEventData(string month)
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
            if (month == FixedEventClassInfo[i].EventDay[1]
                && FixedEventClassInfo[i].IsPossibleUseEvent)
            {
                // 부모 안에 객체를 옮기기
                CalenderEventList = MailObjectPool.GetFixedEventObject(m_ParentCalenderScroll);

                // 고정은 필요한 조건 만큼 걸러지므로 바로 ScrollView 에 오브젝트를 넣는다

                CalenderEventList.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = FixedEventClassInfo[i].EventDay[2];
                CalenderEventList.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = FixedEventClassInfo[i].EventDay[3];
                CalenderEventList.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = FixedEventClassInfo[i].EventClassName;

                EventSchedule.Instance.MyEventList.Add(SelectEventClassInfo[i]);
            }
            else
            {
                break;
            }
        }

    }

    // 선택가능 이벤트 리스트에 정보 넣기
    public void PutOnPossibleEventData(string month)
    {
        GameObject PossibleEventList;              // 선택가능이벤트들 부모

        for (int i = 0; i < SelectEventClassInfo.Count; i++)
        {
            PossibleEventList = MailObjectPool.GetPossibleEventObject(m_PossibleParentScroll);

            // 선택 이벤트 & 선택이벤트인지 & 사용가능한지 / 사용가능한지
            if ((month == SelectEventClassInfo[i].EventDay[1]
                && SelectEventClassInfo[i].IsPossibleUseEvent == true)
                || SelectEventClassInfo[i].IsPossibleUseEvent == true)
            {
                PossibleEventList.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventClassName;

                for (int j = 0; j < SelectEventClassInfo[i].EventRewardStat.Length; j++)
                {
                    if (SelectEventClassInfo[i].EventRewardStat != null)
                    {
                        PossibleEventList.transform.GetChild(1).GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = SelectEventClassInfo[i].EventRewardStatName[j];
                    }
                    else
                    {
                        PossibleEventList.transform.GetChild(1).GetChild(j).gameObject.SetActive(false);
                    }
                }


                PossibleChooseEventClassList.Add(SelectEventClassInfo[i]);
            }
            else
            {
                break;
            }
        }
    }

    // 고정, 선택 이벤트를 첫번째 조건 월에 맞춰 넣었으므로 고정은 바로 달력창의 Scroll View에 넣기
    // 선택은 Possible Scrollview 에 넣기
    public void SetSelectedEventOnCalender()
    {
        for (int i = 0; i < PossibleChooseEventClassList.Count; i++)
        {

        }
    }

    // 오브젝트를 다시 오브젝트 풀에 되돌려 줘야하는데 각각의 큐애 인큐 해야함
    public void ReturnFixedEventToPool(string month)
    {
        int _DestroyMailObj = m_ParentCalenderScroll.childCount;        // 

        for (int i = 0; i < _DestroyMailObj; i++)
        {
            MailObjectPool.ReturnFixedEventObject(m_ParentCalenderScroll.transform.GetChild(i).gameObject);
        }
    }


    //  public void 
}
