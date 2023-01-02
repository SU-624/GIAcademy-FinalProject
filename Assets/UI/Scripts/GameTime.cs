using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///  
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

    public TextMeshProUGUI m_nowTime;


    public float LimitTime = 30.0f;
    float PrevTime = 0.0f;

    int Year = 1;
    // string[] Month = new string[12];
    // string[] Week = new string[4];

    string[] Month = { "1월", "2월", "3월", "4월", "5월", "6월", "7월", "8월", "9월", "10월", "11월", "12월" };
    string[] Week = { "첫째 주", "둘째 주", "셋째 주", "넷째 주" };


    public string[] Day = { "월요일", "화요일", "수요일", "목요일", "금요일" };

    int MonthIndex = 2;
    int WeekIndex = 0;

    int perSecond = 6;

    public bool IsGameMode = false;        // 메인게임화면 or UI 창 화면 체크해서 각 모드 때만 가능한 것들을 하기 위한 변수

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
        IsGameMode = false;
        Debug.Log(IsGameMode);

        // Month[11] = "12월";
        // Week[0] = "첫째주";
        m_nowTime.text = Year + "년 " + Month[MonthIndex] + " " + Week[WeekIndex];

        Debug.Log(Year + "년 " + Month[MonthIndex] + " " + Week[WeekIndex]);
    }

    // Update is called once per frame
    public void Update()
    {
        if (IsGameMode == true)
        {
            ShowGameTime();

            FlowtheTime();
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
        if (PrevTime == 0.0f)
        {
            PrevTime = Time.time;
        }

        if (LimitTime < (Time.time - PrevTime))
        {
            Debug.Log(Time.time);

            ChangeWeek();

            m_nowTime.text = Year + "년 " + Month[MonthIndex] + " " + Week[WeekIndex];

            Debug.Log(Year + "년 " + Month[MonthIndex] + " " + Week[WeekIndex]);

            // 3년이라는 게임 시간이 끝나고 난 후
            if (Year == 3 && MonthIndex == 11 && WeekIndex == 3)
            {
                IsLimitedGameTimeEnd();
            }

            ChangeYear();
            ChangeMonth();

            PrevTime = 0.0f;
        }

    }

    // 년 주 증가
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

    public void ChangeYear()
    {
        // Year 증가
        if (MonthIndex == 11 && WeekIndex == 3)
        {
            Year++;
        }
    }

    int i = 0;
    public void CheckPerSecond(Image img)
    {
        if (GameTime.Instance.IsGameMode == true)
        {
            // 6초마다 시간체크
            if (Time.time - PrevTime >= perSecond)
            {
                Debug.Log(perSecond);
                Debug.Log(img.fillAmount);
                Debug.Log(Day[i]);

                img.fillAmount += 0.2f;

                // 6초마다 더해주기
                perSecond += 6;
                i += 1;

                if (Time.time - PrevTime >= 30)
                {
                    img.fillAmount = 0.2f;

                    perSecond = 6;

                    i = 0;

                    Debug.Log(perSecond + "초 체크 초기화");
                }
            }
        }
    }

    public void IsLimitedGameTimeEnd()
    {
        Debug.Log("게임 스토리 끝~");

    }

    public void ShowGameTime()
    {
        //Debug.Log("시간 진행 : " + IsGameMode);

    }
}