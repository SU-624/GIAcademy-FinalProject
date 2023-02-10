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
    [SerializeField] PopUpUI _popUpClassPanel;

    // 현재 흐르는 시간과 비교해서 변화가 있을 때 이벤트/ 수업 등을 발생 시켜주기 위한 지금 시간 담은 변수
    int nowYear;
    string nowMonth;
    string nowWeek;
    string nowDate;
    string checkMonth;       // 현재 Date랑 Week랑 바뀌는 시간이 달라 Date는 바꼈지만 Week는 바뀌지않아 발생하는 오류를 잡기 위해 만든 변수

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
        checkMonth = nowMonth;
        // 시간 변경은 모든 진행 마지막에
        ChangeTime();      
        // 리스트를 삭제 해줘야 한다면 여기 아래에서

        // 월 바뀌었을 때
        if (nowDate == "월요일" && nowWeek == "첫째 주" && checkMonth != GameTime.Instance.FlowTime.NowMonth)
        {
            Debug.Log("나의 현 월 이벤트리스트 싹 비우기");

            EventSchedule.Instance.MyEventList.Clear();

            // 내가 수업 선택을 완료하고 난 다음달과 그 다음달에도 수업을 실행시켜야한다. 
            if (InGameTest.Instance._isRepeatClass == true)
            {
                InGameTest.Instance.NextClassStart();
                InGameTest.Instance._classCount++;
            }
            else if(InGameTest.Instance._isRepeatClass == false && checkMonth != null)
            {
                _popUpClassPanel.TurnOnUI();
            }

            checkMonth = nowMonth;
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
