using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;


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

    public SaveEventData tempEventList;                                // 내가 선택한 날짜를 받기 위한 임시 변수

    // public List<SaveEventClassData> MyEventList = new List<SaveEventClassData>();       // 현재 나의 이벤트 목록

    [Header("PossibleCountImg")] public GameObject _nowPossibleCountImg;

    [Space(10f)]
    [Header("Buttons")]
    public GameObject Choose_Button;
    public GameObject SetOK_Button;

    public GameObject Go_Button;
    public GameObject Go_Button_1;

    [Header("EventInfoWhiteScreen")] public GameObject WhiteScreen;

    [Space(10f)]
    [Header("SelectdEvent&SetOkEvent")]
    public GameObject RewardInfoScreen;
    public GameObject MonthOfRewardInfoScreen;

    [Space(10f)] public GameObject CalenderMonthText;
                 public GameObject CalenderMonthText_1;

    const int PossibleSetCount = 2;                         // 최대 이벤트 지정 가능 횟수
    [Space(10f)] public int nowPossibleCount = 2;           // 현재 이벤트가능횟수

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
            // tempEventList.EventDate[2] = _NowClick;
            // tempEventList.EventDate[3] = GameTime.Instance.FlowTime.NowMonth;

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

            Debug.Log(tempEventList.EventDate[0]);
            Debug.Log(tempEventList.EventDate[1]);
            Debug.Log(tempEventList.EventDate[2]);
            Debug.Log(tempEventList.EventDate[3]);
        }
    }

    // 현재 선택 가능한 이벤트 갯수 세기, 
    public void CountPossibleEventSetting()
    {
        if (tempEventList.EventDate[2] != 0)
        {
            _PrevClick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

            SwitchEventList.Instance.MyEventList.Add(tempEventList);

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
    }

    // 선택된 일정칸 버튼의 데이터를 해당 이벤트의 날짜에 넣어주기
    public void SaveEventOnCalender()
    {
        _PrevClick = null;      // 이전클릭 초기화시켜주기

        for (int i = 0; i < SwitchEventList.Instance.MyEventList.Count; i++)
        {
            if (SwitchEventList.Instance.MyEventList[i].IsFixedEvent == false && tempEventList.EventClassName == SwitchEventList.Instance.MyEventList[i].EventClassName)
            {
                if (PreIndex == 21)
                {
                    PreIndex = NowIndex;
                    preEventName = tempEventList.EventClassName;
                }

                string tempName = SwitchEventList.Instance.MyEventList[i].EventClassName;
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
            tempEventList.EventDate[2] = 0;
            tempEventList.EventDate[3] = 0;
            Choose_Button.transform.GetComponent<Button>().interactable = false;
            _PrevClick = null;
        }
        else
        {
            WhiteScreen.SetActive(true);

            if (tempEventList != null)
            {
                tempEventList.EventDate[2] = 0;
                tempEventList.EventDate[3] = 0;
            }

            Choose_Button.transform.GetComponent<Button>().interactable = false;
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
            Go_Button.SetActive(true);
            Go_Button_1.SetActive(false);
        }
        else if (_NowClick.name == SetOK_Button.name)
        {
            tempEventList = null;
            RewardInfoScreen.SetActive(false);
            MonthOfRewardInfoScreen.SetActive(true);
            Go_Button.SetActive(false);
            Go_Button_1.SetActive(true);
        }
    }

    // 선택 이벤트만 있을때 취소 안되던 문제 해결 -> for문에 >= 0 이어야 전부 확인하는데 > 0 이었어서 들어가지 않아서 일어나는 문제였음
    public void IfIWantCancleEvent()
    {
        // 요소를 삭제 할 때 인덱스의 변화로 인해 무언가 이상이 생길 수 있으므로 - 를 해가며 체크
        for (int i = 0; i < SwitchEventList.Instance.MyEventList.Count; i++)
        {
            if (SwitchEventList.Instance.MyEventList[i].EventClassName == tempEventList.EventClassName
                && SwitchEventList.Instance.MyEventList[i].IsFixedEvent == false)
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

                // CountPossibleEventSetting();

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
                    _nowPossibleCountImg.transform.GetChild(2).gameObject.SetActive(false);
                }

                ShowSelectedEventSettingOKButton();

                // 제한 횟수도 늘려주기
                if (nowPossibleCount < PossibleSetCount)
                {
                    nowPossibleCount += 1;
                }

                Choose_Button.transform.GetComponent<Button>().interactable = false;
                WhiteScreen.SetActive(true);


                SwitchEventList.Instance.MyEventList.Remove(SwitchEventList.Instance.MyEventList[i]);
                SwitchEventList.Instance.PushCancleButton();
                // 여기서 선택된 이벤트 목록(2개) 중 현재클릭한 이벤트취소를 한다
            }
        }


    }

    public void GetSelectedTime(string[] click)
    {
        //주 구분
        if (click[0] == "1")
        {
            tempEventList.EventDate[2] = 1;
        }
        else if (click[0] == "2")
        {
            tempEventList.EventDate[2] = 2;
        }
        else if (click[0] == "3")
        {
            tempEventList.EventDate[2] = 3;
        }
        else if (click[0] == "4")
        {
            tempEventList.EventDate[2] = 4;
        }
        // 요일 구분
        if (click[1] == "Monday")
        {
            tempEventList.EventDate[3] = 1;
        }
        else if (click[1] == "Tuesday")
        {
            tempEventList.EventDate[3] = 2;
        }
        else if (click[1] == "Wednsday")
        {
            tempEventList.EventDate[3] = 3;
        }
        else if (click[1] == "Thursday")
        {
            tempEventList.EventDate[3] = 4;
        }
        else if (click[1] == "Friday")
        {
            tempEventList.EventDate[3] = 5;
        }
    }

    // 이번달의 고정이벤트를 달력에 미리 넣기
    // 
    public void ShowFixedEventOnCalender()
    {
        int WeekIndex = 0;
        int DayIndex = 0;

        for (int i = 0; i < SwitchEventList.Instance.MyEventList.Count; i++)
        {
            if (SwitchEventList.Instance.MyEventList[i].IsFixedEvent == true)
            {
                WeekIndex = SwitchEventList.Instance.MyEventList[i].EventDate[2] - 1;
                DayIndex = SwitchEventList.Instance.MyEventList[i].EventDate[3] - 1;

                CalenderButton[(WeekIndex * 5) + DayIndex].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = SwitchEventList.Instance.MyEventList[i].EventClassName;
                //if(SwitchEventList.Instance.MyEventList[i].EventDay[2]  == "첫째 주")       //주
                CalenderButton[(WeekIndex * 5) + DayIndex].GetComponent<Button>().interactable = false;
            }
        }
    }

    // 이벤트 -> 한 달이 지날 때 모든 조건을 리셋해 줄 함수들
    public void ResetPossibleCount()
    {
        nowPossibleCount = 2;

        _nowPossibleCountImg.transform.GetChild(0).gameObject.SetActive(true);
        _nowPossibleCountImg.transform.GetChild(1).gameObject.SetActive(true);
        _nowPossibleCountImg.transform.GetChild(2).gameObject.SetActive(false);

        for (int i = 0; i < CalenderButton.Count; i++)
        {
            CalenderButton[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            CalenderButton[i].GetComponent<Button>().interactable = true;
        }

        SwitchEventList.Instance.MyEventList.Clear();
    }

    public void SetEventCalenderMonthText()
    {
        CalenderMonthText.transform.GetComponent<TextMeshProUGUI>().text = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
        CalenderMonthText_1.transform.GetComponent<TextMeshProUGUI>().text = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
    }
}
