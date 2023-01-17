using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2023. 01. 10 Mang
/// 
/// 이벤트, 수업 등 일정 시간에 발생할 모든 이벤트를 시간 체크 후 발생 시켜 줄 스크립트
/// </summary>
public class ClockScheduler : MonoBehaviour
{

    // 현재 흐르는 시간과 비교해서 변화가 있을 때 이벤트/ 수업 등을 발생 시켜주기 위한 지금 시간 담은 변수
    int nowYear;
    string nowMonth;
    string nowWeek;
    string nowDate;

    EventClassPrefab EventPrefab;

    // Start is called before the first frame update
    void Start()
    {
        nowYear = GameTime.Instance.FlowTime.NowYear;
        nowMonth = GameTime.Instance.FlowTime.NowMonth;
        nowWeek = GameTime.Instance.FlowTime.NowWeek;
        nowDate = GameTime.Instance.FlowTime.NowDay;


        EventPrefab = GameObject.Find("EventClassPrefab").GetComponent<EventClassPrefab>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeTime();       // 시간 변경은 모든 진행 마지막에

        // 리스트를 삭제 해줘야 한다면 여기 아래에서

        // 월 바뀌었을 때
        if (nowDate == "월요일" && nowWeek == "첫째 주" && nowMonth != GameTime.Instance.FlowTime.NowMonth)
        {
            Debug.Log("나의 현 월 이벤트리스트 싹 비우기");

            EventSchedule.Instance.MyEventList.Clear();
        }
    }

    public void ChangeTime()
    {
        if (nowYear != GameTime.Instance.FlowTime.NowYear)
        {
            nowYear = GameTime.Instance.FlowTime.NowYear;
        }

        if (nowMonth != GameTime.Instance.FlowTime.NowMonth)
        {
            nowMonth = GameTime.Instance.FlowTime.NowMonth;
        }

        if (nowWeek != GameTime.Instance.FlowTime.NowWeek)
        {
            nowWeek = GameTime.Instance.FlowTime.NowWeek;
        }

        if (nowDate != GameTime.Instance.FlowTime.NowDay)
        {
            nowDate = GameTime.Instance.FlowTime.NowDay;
        }
    }
}
