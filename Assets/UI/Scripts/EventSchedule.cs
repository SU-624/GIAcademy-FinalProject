using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// 2023. 01. 08 Mang
/// 
/// 이벤트 선택 창이 다 끝난 뒤 그 데이터를 저장 할 클래스
/// 여기서는 데이터 세이브, 로드만 다루는 걸로
/// </summary>
public class EventSchedule : MonoBehaviour
{
    private static EventSchedule _instance = null;

    public GameObject CalenderObj;                                          // 달력 버튼리스트에 정보 넣기 위한 달력 부모  오브젝트

    // 이 버튼의 존재 의의 -> 고정 이벤트의 정보를 먼저 담아놓기 위한 버튼 
    public List<GameObject> CalenderButton = new List<GameObject>();        // 달력 오브젝트 넣은 달력버튼 리스트

    public SaveEventClassData tempEventList;                                // 내가 선택한 날짜를 받기 위한 임시 변수

    public List<SaveEventClassData> MyEventList = new List<SaveEventClassData>();       // 현재 나의 이벤트 목록

    [Header("PossibleCountImg")]
    public GameObject _nowPossibleCountImg;

    [Space(10f)]
    [Header("Buttons")]
    public GameObject Choose_Button;
    public GameObject SetOK_Button;

    [Header("EventInfoWhiteScreen")]
    public GameObject WhiteScreen;

    [Space(10f)]
    [Header("SelectdEvent&SetOkEvent")]
    public GameObject RewardInfoScreen;
    public GameObject MonthOfRewardInfoScreen;

    const int PossibleSetCount = 2;      // 최대 이벤트 지정 가능 횟수
    [Space(10f)]
    public int nowPossibleCount = 2;            // 현재 이벤트가능횟수

    

    public static EventSchedule Instance
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
        if (tempEventList == null)
        {
            Choose_Button.GetComponent<Button>().interactable = false;
        }

        GiveCalenderInfoToButton();
    }

    // int monthArrow = 0;
    // Update is called once per frame
    void Update()
    {
    }

    // 달력(일정) 버튼 받아서 달아주기
    public void GiveCalenderInfoToButton()
    {
        for (int i = 0; i < CalenderObj.transform.childCount; i++)
        {
            CalenderButton.Add(CalenderObj.transform.GetChild(i).gameObject);
        }
    }

    public void MakeFixedEventInfoPrefab()
    {
        foreach (var MyEventList in MyEventList)
        {
            if (MyEventList.IsFixedEvent == true)
            {

            }
        }
    }

    // 선택된 이벤트를 보여준기 위한 목록 프리팹 생성 ( )
    public void MakeSelectEventInfoPrefab()
    {
        foreach (var MyEventList in MyEventList)
        {
            if (MyEventList.IsFixedEvent == false && 
                MyEventList.EventClassName == tempEventList.EventClassName)
            {

            }
        }
    }

    // EventNumber에 따라서 프리팹 정렬이 가능하지 않을까?


    GameObject _PrevClick = null;           // 이전클릭
    int NowIndex;      // 현재 눌린 달력오브젝트를 알기위한 변수 -> 달력의 현재 눌린 인덱스 (하나의 인덱스를 공유해서 문제 생김)
    int PreIndex = 21;       // 예외처리 해야함 0 값이 되면 버튼 0번이 눌리기 때문에 예외처리 
    string preEventName = null;    // 인덱스만으로는 이전에 클릭한 버튼의 정보를 모르기에 여기다 담아두자
    //(선택 이벤트)달력 버튼이 눌림 -> 달력 칸에 이벤트 띄우기
    public void ClickCalenderButton()
    {
        GameObject _NowClick = EventSystem.current.currentSelectedGameObject;   // 현재클릭

        string[] clickdate;

        //string[] clickdate = _NowEvent.name.Split("_");

        if (tempEventList != null)
        {
            //선택한 이벤트의 현재 년 / 월
            tempEventList.EventDay[0] = GameTime.Instance.FlowTime.NowYear.ToString();
            tempEventList.EventDay[1] = GameTime.Instance.FlowTime.NowMonth;


            // 클릭한 버튼 - 버튼 이름 비교 . 일치 시 버튼에 현재 이벤트의 이름 띄우기
            for (int i = 0; i < CalenderObj.transform.childCount; i++)
            {
                if (_NowClick.name == CalenderButton[i].name)
                {
                    _NowClick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tempEventList.EventClassName;
                    NowIndex = i;
                    clickdate = CalenderButton[i].name.Split("_");     // "_" 특정 문자열로 이름 나눠주기

                    GetSelectedTime(clickdate);     // 선택한 달력의 날짜 받기

                    break;
                }
            }

            // 날짜 하나만 선택되도록 체크(잔상 남지 않도록)
            if (_PrevClick != _NowClick)
            {
                if (_PrevClick != null)
                {
                    _PrevClick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                _PrevClick = _NowClick;
                // PreIndex = NowIndex;
            }
            else
            {
                _PrevClick = _NowClick;
                // PreIndex = NowIndex;
            }

            Debug.Log(tempEventList.EventDay[0]);
            Debug.Log(tempEventList.EventDay[1]);
            Debug.Log(tempEventList.EventDay[2]);
            Debug.Log(tempEventList.EventDay[3]);
        }


    }

    // 현재 선택 가능한 이벤트 갯수 세는 함수
    public void CountPossibleEventSetting()
    {
        _PrevClick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

        MyEventList.Add(tempEventList);

        if (nowPossibleCount == 2)
        {
            _nowPossibleCountImg.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (nowPossibleCount == 1)
        {
            _nowPossibleCountImg.transform.GetChild(0).gameObject.SetActive(false);
        }

        nowPossibleCount -= 1;

        SaveEventOnCalender();

        if (nowPossibleCount == 0)
        {
            Choose_Button.transform.GetComponent<Button>().interactable = false;
            _nowPossibleCountImg.transform.GetChild(2).gameObject.SetActive(true);
            Debug.Log("이제 선택 못함");
            //ShowSelectedEventSettingOKButton();
        }
    }

    // 선택된 일정칸 버튼의 데이터를 해당 이벤트의 날짜에 넣어주기
    public void SaveEventOnCalender()
    {
        _PrevClick = null;      // 이전클릭 초기화시켜주기

        for (int i = 0; i < MyEventList.Count; i++)
        {
            if (MyEventList[i].IsFixedEvent == false && tempEventList.EventClassName == MyEventList[i].EventClassName)
            {
                if (PreIndex == 21)
                {
                    PreIndex = NowIndex;
                    preEventName = tempEventList.EventClassName;
                }

                string tempName = MyEventList[i].EventClassName;
                CalenderButton[NowIndex].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tempName;
                CalenderButton[NowIndex].transform.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void IfClickBackButton()
    {
        if (_PrevClick != null)
        {
            _PrevClick.SetActive(true);
            _PrevClick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            WhiteScreen.SetActive(true);
            tempEventList = null;
            Choose_Button.transform.GetComponent<Button>().interactable = false;
            _PrevClick = null;
        }
        else
        {
            WhiteScreen.SetActive(true);
            tempEventList = null;
            Choose_Button.transform.GetComponent<Button>().interactable = false;
            _PrevClick = null;
        }
    }

    // 선택 버튼을 눌렀는지 설정완료 버튼을 눌렀는지 체크해서 보여줄 Info 창을 선택하자
    public void ShowSelectedEventSettingOKButton()
    {
        GameObject _NowClick = EventSystem.current.currentSelectedGameObject;

        if (_NowClick.name == Choose_Button.name)
        {
            RewardInfoScreen.SetActive(true);
            MonthOfRewardInfoScreen.SetActive(false);
        }
        else if (_NowClick.name == SetOK_Button.name)
        {
            tempEventList = null;
            RewardInfoScreen.SetActive(false);
            MonthOfRewardInfoScreen.SetActive(true);
        }
    }

    // 취소는 현재 내가 누른 이벤트목록의 이벤트와 달력의 정보를 알 수 없으므로 작동하지 않음
    // 조건과 데이터를 더 잘 연결해서 만들면 될듯
    // 고정이벤트는 리스트에 들어가고, 현재 리스트에서 고정 키워드를 찾아 달력에 그려주면 그려질듯
    // 선택 이벤트 두개가 선택되면 더이상 버튼이 눌리지 않고, 이벤트 취소를 누르면 횟수가 돌아오고,
    // 이렇게 작동하도록 하자
    public void IfIWantCancleEvent()
    {
        // 요소를 삭제 할 때 인덱스의 변화로 인해 무언가 이상이 생길 수 있으므로 - 를 해가며 체크
        for (int i = MyEventList.Count - 1; i > 0; i--)
        {
            if (MyEventList[i].EventClassName == tempEventList.EventClassName
                && MyEventList[i].IsFixedEvent == false)
            {

                if (preEventName == tempEventList.EventClassName)
                {
                    CalenderButton[PreIndex].transform.GetComponent<Button>().interactable = true;
                    CalenderButton[PreIndex].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                else
                {
                    CalenderButton[NowIndex].transform.GetComponent<Button>().interactable = true;
                    CalenderButton[NowIndex].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }

                if (nowPossibleCount == 1)          // 이벤트 하나만 있을 때
                {
                    _nowPossibleCountImg.transform.GetChild(1).gameObject.SetActive(true);
                    _nowPossibleCountImg.transform.GetChild(2).gameObject.SetActive(false);

                    _PrevClick = null;
                    PreIndex = 21;
                    tempEventList = null;
                }
                else if (nowPossibleCount == 0)     // 이벤트 2개 다 차있을때
                {
                    _nowPossibleCountImg.transform.GetChild(0).gameObject.SetActive(true);
                }

                ShowSelectedEventSettingOKButton();

                // 제한 횟수도 늘려주기
                if (nowPossibleCount < PossibleSetCount)
                {
                    nowPossibleCount += 1;
                }

                Choose_Button.transform.GetComponent<Button>().interactable = false;
                WhiteScreen.SetActive(true);


                MyEventList.Remove(MyEventList[i]);
            }
        }
    }

    public void GetSelectedTime(string[] click)
    {
        //주 구분
        if (click[0] == "1")
        {
            tempEventList.EventDay[2] = GameTime.Instance.Week[0];
        }
        else if (click[0] == "2")
        {
            tempEventList.EventDay[2] = GameTime.Instance.Week[1];
        }
        else if (click[0] == "3")
        {
            tempEventList.EventDay[2] = GameTime.Instance.Week[2];
        }
        else if (click[0] == "4")
        {
            tempEventList.EventDay[2] = GameTime.Instance.Week[3];
        }
        // 요일 구분
        if (click[1] == "Monday")
        {
            tempEventList.EventDay[3] = GameTime.Instance.Day[0];
        }
        else if (click[1] == "Tuesday")
        {
            tempEventList.EventDay[3] = GameTime.Instance.Day[1];
        }
        else if (click[1] == "Wednsday")
        {
            tempEventList.EventDay[3] = GameTime.Instance.Day[2];
        }
        else if (click[1] == "Thursday")
        {
            tempEventList.EventDay[3] = GameTime.Instance.Day[3];
        }
        else if (click[1] == "Friday")
        {
            tempEventList.EventDay[3] = GameTime.Instance.Day[4];
        }
    }

    // 이벤트선택 창에서 선택완료 라는 버튼을 누르면 고정이벤트가 뜬 상태로 뜨게 하기
    // 아니면 그냥 뜨게 하기? 이건 얘기해보기
    public void ShowFixedEventOnCalender()
    {

    }


    // 달력 버튼 눌렀을 때 그 달의 일정을 보여주는 달력창을 만들어야 한다
    // 만들어둔 달력 체크 화면의 것을 옮겨다가 보여주고, 다시 끄면 원래 곳으로 옮겨주고
}
