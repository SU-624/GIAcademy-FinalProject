using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public struct NowTime
{
    public int NowYear;
    public int NowMonth;
    public int NowWeek;
    public int NowDay;
}

public class TimeDefine
{
    // const int 
}

/// <summary>
///  2023. 01. 30 Mang
/// 
/// 시간 : string -> int 로 변경. UI 는 보이기만 하도록
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

    private int Year = 1;           // 1 ~ 3년(게임모드) - 무한(무한모드)
    private int Month = 3;          // 1 ~ 12월(12)
    private int Week = 1;           // 1 ~ 4주(4)
    private int Day = 1;            // 월 ~ 금(5)

    Image TimeBarImg;

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
        //Debug.Log(SecondHalfPerSecond);
        // Debug.Log(TimeBarImg.fillAmount);
        //Debug.Log(Day);

        IsGameMode = false;
        //Debug.Log(IsGameMode);

        // Month[11] = "12월";
        // Week[0] = "첫째주";
        m_DrawnowTime.text = Year + "년 " + Month + "월 " + Week + "주";

        FlowTime.NowYear = Year;
        FlowTime.NowMonth = Month;
        FlowTime.NowWeek = Week;
        FlowTime.NowDay = Day;

        //Debug.Log(Year + "년" + " " + Month + " " + Week);

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
        m_DrawnowTime.text = Year + "년 " + Month + "월 " + Week + "주";

        // test씬에서 사용하기 위해 만든 조건문. 나중에 지워주기
        //if (SceneManager.GetActiveScene().name == "TestScene")
        //{
        //    TestDialogue();
        //}
        //else if (SceneManager.GetActiveScene().name == "InGameScene")
        //{
        //}
            CheckPerSecond();

        // 30초 넘어갔을 때 값 변화
        if (isChangeWeek)     // 
        {
            ChangeMonth();

            ChangeWeek();

            //Debug.Log("Time.time : " + Time.time);

            // nowTime = Year + "년 " + Month[MonthIndex] + " " + Week[WeekIndex];
            if (Week != 4)
            {
                m_DrawnowTime.text = Year + "년 " + Month + "월 " + Week + "주";

                FlowTime.NowWeek = Week;
            }

            FlowTime.NowYear = Year;
            FlowTime.NowMonth = Month;

            // 3년이라는 게임 시간이 끝나고 난 후
            if (Year == 3 && Month == 12 && Week == 4)
            {
                IsLimitedGameTimeEnd();
            }

            PrevTime = 0.0f;
            isChangeWeek = false;
        }

        if (PrevTime == 0.0f)
        {
            PrevTime = Time.time;
        }
    }

    public void ChangeDay()
    {
        if (Day < 5)
        {
            Day++;

            //Debug.Log("요일 : " + Day);
        }
        else if (Day >= 5)
        {
            Day = 1;
        }
    }

    // 주 증가
    public void ChangeWeek()
    {
        // Week 증가
        if (Week < 4)
        {
            Week++;

            //Debug.Log("주 : " + Week);
        }
        else if (Week >= 4)
        {
            Week = 1;
        }
    }

    // 년, 월 증가
    public void ChangeMonth()
    {
        // 연도 변경 시 월 초기화
        if (Month >= 12 && Week >= 4)
        {
            Month = 1;

            Year++;
        }
        // 월 증가
        else if (Month < 12 && Week >= 4)
        {
            Month++;
            //Debug.Log("달 : " + Month);
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
            if (Week == 1 || Week == 2)
            {
                // (1 ~ 2 주차)1초마다 시간체크
                if (Time.time - PrevTime >= FirstHalfPerSecond)
                {
                    TimeBarImg.fillAmount += 0.2f;

                    // 1초마다 더해주기
                    ChangeDay();
                    FlowTime.NowDay = Day;

                    i += 1;
                    FirstHalfPerSecond += 1;

                    if (FirstHalfPerSecond > LimitTime1)
                    {
                        TimeBarImg.fillAmount = 0.2f;

                        FirstHalfPerSecond = 1;

                        i = 0;

                        //  Debug.Log(FirstHalfPerSecond + "초 체크 초기화");

                        isChangeWeek = true;
                    }
                    //Debug.Log("초 : " + FirstHalfPerSecond);
                    //Debug.Log("이미지 : " + TimeBarImg.fillAmount);
                    //Debug.Log("데이 : " + Week);
                }
            }
            else if (Week == 3 || Week == 4)
            {
                // (3 ~ 4 주차)6초마다 시간체크
                if (Time.time - PrevTime >= SecondHalfPerSecond)
                {
                    TimeBarImg.fillAmount += 0.2f;

                    i += 1;
                    // 6초마다 더해주기
                    ChangeDay();
                    FlowTime.NowDay = Day;

                    SecondHalfPerSecond += 6;

                    if (SecondHalfPerSecond > LimitTime2)
                    {
                        TimeBarImg.fillAmount = 0.2f;

                        SecondHalfPerSecond = 6;

                        i = 0;

                        // Debug.Log(SecondHalfPerSecond + "초 체크 초기화");

                        isChangeWeek = true;
                    }

                    //Debug.Log(SecondHalfPerSecond);
                    //Debug.Log(TimeBarImg.fillAmount);
                    //Debug.Log(Day[i]);
                }
            }
        }
    }

    // test씬에서 사용하려고 만든 임시 함수. 테스트 후 지우기
    public void TestDialogue()
    {
        // 수업을 시작했을 때 캐릭터는 움직이지만 시간은 흐르지않게 할 때 PrevTime을 갱신해주지 않으면 수업 시작 시 시간이 재빠르게 흐른다.
        if (InGameTest.Instance.m_ClassState == ClassState.ClassStart)
        {
            PrevTime = Time.time;
        }

        // 수업시작 후 반에 도착할 때 까지는 시간이 흐르면 안된다. TimeScale을 멈추면 캐릭터가 움직이지 않으니 다른 방법으로,,
        if (GameTime.Instance.IsGameMode == true && InGameTest.Instance.m_ClassState != ClassState.ClassStart)
        {
            if (Week == 1 || Week == 2)
            {
                // (1 ~ 2 주차)1초마다 시간체크
                if (Time.time - PrevTime >= 0.5f)
                {
                    TimeBarImg.fillAmount += 0.2f;

                    // 1초마다 더해주기
                    ChangeDay();
                    FlowTime.NowDay = Day;

                    i += 1;
                    FirstHalfPerSecond += 1;

                    if (FirstHalfPerSecond > 2f)
                    {
                        TimeBarImg.fillAmount = 0.2f;

                        FirstHalfPerSecond = 1;

                        i = 0;

                        //  Debug.Log(FirstHalfPerSecond + "초 체크 초기화");

                        isChangeWeek = true;
                    }
                    //Debug.Log("초 : " + FirstHalfPerSecond);
                    //Debug.Log("이미지 : " + TimeBarImg.fillAmount);
                    //Debug.Log("데이 : " + Week);
                }
            }
            else if (Week == 3 || Week == 4)
            {
                // (3 ~ 4 주차)6초마다 시간체크
                if (Time.time - PrevTime >= SecondHalfPerSecond)
                {
                    TimeBarImg.fillAmount += 0.2f;

                    i += 1;
                    // 6초마다 더해주기
                    ChangeDay();
                    FlowTime.NowDay = Day;

                    SecondHalfPerSecond += 6;

                    if (SecondHalfPerSecond > LimitTime2)
                    {
                        TimeBarImg.fillAmount = 0.2f;

                        SecondHalfPerSecond = 6;

                        i = 0;

                        // Debug.Log(SecondHalfPerSecond + "초 체크 초기화");

                        isChangeWeek = true;
                    }

                    //Debug.Log(SecondHalfPerSecond);
                    //Debug.Log(TimeBarImg.fillAmount);
                    //Debug.Log(Day[i]);
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