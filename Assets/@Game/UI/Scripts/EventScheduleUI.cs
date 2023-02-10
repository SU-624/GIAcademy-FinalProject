using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScheduleUI : MonoBehaviour
{
    public SaveEventData tempEventList;                                // 내가 선택한 날짜를 받기 위한 임시 변수
    public List<SaveEventData> MyEventList = new List<SaveEventData>();       // 현재 나의 이벤트 목록

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void GetISelectedEvent()
    {
        //MyeventListSwitchEventList.Instance.IfIChoosedEvent;
    }
}
