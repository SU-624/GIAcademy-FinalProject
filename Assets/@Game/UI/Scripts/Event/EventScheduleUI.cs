using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EventScheduleUI : MonoBehaviour
{
    // 이벤트 설정 UI
    [Tooltip("이벤트 선택 창의 사용가능 이벤트목록을 만들기 위한 프리팹변수들")]
    [Header("EventList Prefab")]
    [SerializeField] private GameObject m_PossibleEventprefab;     // 
    [SerializeField] private Transform m_PossibleEventParentScroll;      // 
    [SerializeField] private GameObject m_ParentCalenderButton;

    [Space(10f)]
    // 이벤트 창에서 쓰이는 텍스트 들을 다루는 변수들
    [SerializeField] private TextMeshProUGUI HeadLineThisMonthText;
    [SerializeField] private TextMeshProUGUI CalenderThisMonthText;
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI NoticeNeedMoneyText;
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI NowMoneyText;
    [SerializeField] private TextMeshProUGUI NowSpecialPointText;
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI PayMoneyText;
    [SerializeField] private TextMeshProUGUI PaySpecialPointText;
    [Space(5f)]
    [SerializeField] private Button ChoiceResetButton;
    [SerializeField] private Button SetOKButton;

    // 단순실행 이벤트 UI
    // 선택지 이벤트 UI
    // 대상 선택 이벤트 UI

    // [SerializeField] private SuddenEventTableList tempEventList;                                // 내가 선택한 날짜를 받기 위한 임시 변수
    // [SerializeField] private List<SuddenEventTableList> MyEventList = new List<SuddenEventTableList>();       // 현재 나의 이벤트 목록
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
