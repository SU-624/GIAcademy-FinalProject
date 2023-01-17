using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public struct NowTime
{
    public int NowYear;
    public string NowMonth;
    public string NowWeek;
    public string NowDay;
}

/// <summary>
///  
/// 
/// 시간 -> enum 클래스 로 바꿔서 UI 적으로 보일때만 한글로 보여지도록 하자
/// </summary>
public class GameTime : MonoBehaviour
{
    private static GameTime instance = null;

    public static GameTime Instance
    {
        get
        {
            return instance;
        }

        set { instance = value; }
    }

    public TextMeshProUGUI m_DrawnowTime;
    public TextMeshProUGUI m_TimeText;


    public NowTime FlowTime;

    const float LimitTime1 = 5.0f;      // 1 ~ 2주 제한시간
    const float LimitTime2 = 30.0f;     // 3 ~ 4주 제한시간
    float PrevTime = 0;

    int Year = 1;
    // string[] Month = new string[12];
    // string[] Week = new string[4];

    public string[] Month = { "1월", "2월", "3월", "4월", "5월", "6월", "7월", "8월", "9월", "10월", "11월", "12월" };
    public string[] Week = { "첫째 주", "둘째 주", "셋째 주", "넷째 주" };


    public string[] Day = { "월요일", "화요일", "수요일", "목요일", "금요일" };

    Image TimeBarImg;

    public int MonthIndex = 2;
    public int WeekIndex = 0;
    public int DayIndex = 0;

    int FirstHalfPerSecond = 1;       //  (1주 - 2주) 하루의 시간 1초(한 주 총 5초)
    int SecondHalfPerSecond = 6;      // (3주 - 4주)하루의 시간은 6초                                       // 

    public bool IsGameMode = false;                 // 메인게임화면 or UI 창 화면 체크해서 각 모드 때만 가능한 것들을 하기 위한 변수

    public bool IsMonthCycleCompleted = false;      // 월 - 한 사이클 돌았는지 체크할 변수
    public bool IsOneSemesterCompleted = false;     // 3개월(한 학기) 사이클 돌았는지 체크
    public bool IsYearCycleCompleted = false;       // 년 - 한 사이클 돌았는지 체크할 변수
    public bool IsGameEnd = false;                  // 게임이 끝났는 지 체크할 변수

    public bool isChangeWeek = false;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


    }

    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log(SecondHalfPerSecond);
        // Debug.Log(TimeBarImg.fillAmount);
        Debug.Log(Day[i]);

        IsGameMode = false;
        Debug.Log(IsGameMode);

        // Month[11] = "12월";
        // Week[0] = "첫째주";
        m_DrawnowTime.text = Year + "년 " + Month[MonthIndex] + " " + Week[WeekIndex];

        FlowTime.NowYear = Year;
        FlowTime.NowMonth = Month[MonthIndex];
        FlowTime.NowWeek = Week[WeekIndex];
        FlowTime.NowDay = Day[DayIndex];

        Debug.Log(Year + "년" + " " + Month[MonthIndex] + " " + Week[WeekIndex]);

        ShowGameTime();
    }

    bool call = false;
    // Update is called once per frame
    public void Update()
    {
        if (IsGameMode == true)
        {
            FlowtheTime();

            ShowGameTime();
        }

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Time.timeScale = 0;
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Time.timeScale = 1;
        //}

    }

    public void FlowtheTime()
    {
        // 30초 넘어갔을 때 값 변화
        if (isChangeWeek)     // 
        {
            ChangeWeek();

            Debug.Log("Time.time : " + Time.time);

            // nowTime = Year + "년 " + Month[MonthIndex] + " " + Week[WeekIndex];
            m_DrawnowTime.text = Year + "년 " + Month[MonthIndex] + " " + Week[WeekIndex];

            FlowTime.NowYear = Year;
            FlowTime.NowMonth = Month[MonthIndex];
            FlowTime.NowWeek = Week[WeekIndex];

            // m_DrawnowTime.text = nowTime;


            // 3년이라는 게임 시간이 끝나고 난 후
            if (Year == 3 && MonthIndex == 11 && WeekIndex == 3)
            {
                IsLimitedGameTimeEnd();
            }

            ChangeYear();
            ChangeMonth();

            PrevTime = 0.0f;
            isChangeWeek = false;
        }

        if (PrevTime == 0.0f)
        {
            PrevTime = Time.time;
        }

        CheckPerSecond();
    }

    public void ChangeDay()
    {
        if (DayIndex != 4)
        {
            DayIndex++;
        }
        else if (DayIndex == 4)
        {
            DayIndex = 0;
        }
    }

    // 주 증가
    public void ChangeWeek()
    {
        // Week 증가
        if (WeekIndex != 3)
        {
            WeekIndex++;
        }
        else if (WeekIndex == 3)
        {
            WeekIndex = 0;
        }
    }

    // 월 증가
    public void ChangeMonth()
    {
        // 연도 변경 시 월 초기화
        if (MonthIndex == 11 && WeekIndex == 3)
        {
            MonthIndex = 0;
        }
        // 월 증가
        else if (MonthIndex != 11 && WeekIndex == 3)
        {
            MonthIndex++;
        }
    }

    // 년 증가
    public void ChangeYear()
    {
        // Year 증가
        if (MonthIndex == 11 && WeekIndex == 3)
        {
            Year++;
        }
    }

    int i = 0;
    public void CheckPerSecond()
    {
        // 수업을 시작했을 때 캐릭터는 움직이지만 시간은 흐르지않게 할 때 PrevTime을 갱신해주지 않으면 수업 시작 시 시간이 재빠르게 흐른다.
        if (InGameTest.Instance.m_ClassState == ClassState.ClassStart)
        {
            PrevTime = Time.time;
        }

        // 수업시작 후 반에 도착할 때 까지는 시간이 흐르면 안된다. TimeScale을 멈추면 캐릭터가 움직이지 않으니 다른 방법으로,,
        if (GameTime.Instance.IsGameMode == true && InGameTest.Instance.m_ClassState != ClassState.ClassStart)
        {
            if (Week[WeekIndex] == "첫째 주" || Week[WeekIndex] == "둘째 주")
            {
                // (1 ~ 2 주차)1초마다 시간체크
                if (Time.time - PrevTime >= FirstHalfPerSecond)
                {
                    TimeBarImg.fillAmount += 0.2f;

                    // 1초마다 더해주기
                    ChangeDay();
                    FlowTime.NowDay = Day[DayIndex];

                    i += 1;
                    FirstHalfPerSecond += 1;

                    if (FirstHalfPerSecond > LimitTime1)
                    {
                        TimeBarImg.fillAmount = 0.2f;

                        FirstHalfPerSecond = 1;

                        i = 0;

                        Debug.Log(FirstHalfPerSecond + "초 체크 초기화");

                        isChangeWeek = true;
                    }
                    Debug.Log("초 : " + FirstHalfPerSecond);
                    Debug.Log("이미지 : " + TimeBarImg.fillAmount);
                    Debug.Log("데이 : " + Week[WeekIndex]);
                }

            }
            else if (Week[WeekIndex] == "셋째 주" || Week[WeekIndex] == "넷째 주")
            {
                // (3 ~ 4 주차)6초마다 시간체크
                if (Time.time - PrevTime >= SecondHalfPerSecond)
                {
                    TimeBarImg.fillAmount += 0.2f;

                    i += 1;
                    // 6초마다 더해주기
                    ChangeDay();
                    FlowTime.NowDay = Day[DayIndex];

                    SecondHalfPerSecond += 6;

                    if (SecondHalfPerSecond > LimitTime2)
                    {
                        TimeBarImg.fillAmount = 0.2f;

                        SecondHalfPerSecond = 6;

                        i = 0;

                        Debug.Log(SecondHalfPerSecond + "초 체크 초기화");

                        isChangeWeek = true;
                    }

                    Debug.Log(SecondHalfPerSecond);
                    Debug.Log(TimeBarImg.fillAmount);
                    Debug.Log(Day[i]);
                }
            }
        }
    }

    //
    public void DrawTimeBar(Image img)
    {
        TimeBarImg = img;
    }

    public void IsLimitedGameTimeEnd()
    {
        if (IsGameEnd == true)
        {
            Debug.Log("게임 스토리 끝~");

        }
    }

    public void ShowGameTime()
    {
        m_TimeText.text = FlowTime.NowYear.ToString() + FlowTime.NowMonth + FlowTime.NowWeek + FlowTime.NowDay;
    }
}