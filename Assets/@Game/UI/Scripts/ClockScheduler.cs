using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 2023. 01. 10 Mang
/// 
/// 이벤트, 수업 등 일정 시간에 발생할 모든 이벤트를 시간 체크 후 발생 시켜 줄 스크립트
/// </summary>
public class ClockScheduler : MonoBehaviour
{
    [SerializeField] PopUpUI _popUpClassPanel;

    [SerializeField] PopUpUI _PopUpEventPanel;
    [SerializeField] PopUpUI _TempEventPanel;
    [SerializeField] GameObject _tempEventPanelforText;

    public GameObject EventStartButton;
    public PopUpUI EventSchedulePanel;      // 새로 바꾼 이벤트 선택 패널

    // public PopUpUI EventPanel;

    int checkMonth = 3;       // 현재 Date랑 Week랑 바뀌는 시간이 달라 Date는 바꼈지만 Week는 바뀌지않아 발생하는 오류를 잡기 위해 만든 변수

    bool isAlreadySetEvent = false;

    bool IsResetEventList = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 시간 변경은 모든 진행 마지막에

        // 월 바뀌었을 때
        if (GameTime.Instance.FlowTime.NowDay == 1 && GameTime.Instance.FlowTime.NowWeek == 1 && checkMonth != GameTime.Instance.FlowTime.NowMonth)
        {
            if (checkMonth != 0)
            {
                _popUpClassPanel.TurnOnUI();
            }

            if (checkMonth == 12)
            {
                checkMonth = 1;
            }
            else
            {
                checkMonth += 1;
            }
        }

        if (GameTime.Instance.IsGameMode == true)
        {
            AutoEventPopUp();           // 정해진 날마다 자동으로 이벤트 창 팝업
        }

        // 월 바뀌었을 때
        if (GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 1 && IsResetEventList == false)
        {
            Debug.Log("나의 현 월 이벤트리스트 싹 비우기");
            isAlreadySetEvent = false;
            EventSchedule.Instance.ResetPossibleCount();
            SwitchEventList.Instance.ReturnEventPrefabToPool();
            IsResetEventList = true;
            SwitchEventList.Instance.IsSetEventList = false;
            SwitchEventList.Instance.PrevIChoosedEvent.Clear();
        }

        if (GameTime.Instance.FlowTime.NowWeek != 1 && GameTime.Instance.FlowTime.NowDay != 1)
        {
            IsResetEventList = false;
        }
    }

    // 월마다 이벤트 창이 자동으로 뜨게 하기
    public void AutoEventPopUp()
    {
        //foreach (var thisMonthEvent in SwitchEventList.Instance.MyEventList)
        for (int i = 0; i < SwitchEventList.Instance.MyEventList.Count; i++)
        {
            if (SwitchEventList.Instance.MyEventList[i].IsPopUp == false)
            {
                if (SwitchEventList.Instance.MyEventList[i].EventDate[1] == GameTime.Instance.FlowTime.NowMonth)
                {
                    if (SwitchEventList.Instance.MyEventList[i].EventDate[2] == GameTime.Instance.FlowTime.NowWeek)
                    {
                        if (SwitchEventList.Instance.MyEventList[i].EventDate[3] == GameTime.Instance.FlowTime.NowDay)
                        {
                            _tempEventPanelforText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SwitchEventList.Instance.MyEventList[i].EventClassName + " 이벤트 발생!";
                            _TempEventPanel.TurnOnUI();
                            SwitchEventList.Instance.MyEventList[i].IsPopUp = true;
                        }
                    }
                }
            }
        }
    }

    public void PopUpEventStartButton()
    {
        // 월 -> 3, 6, 9, 12월 일때 분기별 수업 후 이벤트 진행 되도록

        EventStartButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
        _PopUpEventPanel.TurnOnUI();

        isAlreadySetEvent = true;

        InGameTest.Instance._isSelectClassNotNull = false;

    }
}
