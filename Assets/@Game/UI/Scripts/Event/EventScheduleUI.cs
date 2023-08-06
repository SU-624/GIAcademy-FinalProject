using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EventScheduleUI : MonoBehaviour
{
    // �̺�Ʈ ���� UI
    [Tooltip("�̺�Ʈ ���� â�� ��밡�� �̺�Ʈ����� ����� ���� �����պ�����")]
    [Header("EventList Prefab")]
    [SerializeField] private GameObject m_PossibleEventprefab;     // 
    [SerializeField] private Transform m_PossibleEventParentScroll;      // 
    [SerializeField] private GameObject m_ParentCalenderButton;

    [Space(10f)]
    // �̺�Ʈ â���� ���̴� �ؽ�Ʈ ���� �ٷ�� ������
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

    // �ܼ����� �̺�Ʈ UI
    // ������ �̺�Ʈ UI
    // ��� ���� �̺�Ʈ UI

    // [SerializeField] private SuddenEventTableList tempEventList;                                // ���� ������ ��¥�� �ޱ� ���� �ӽ� ����
    // [SerializeField] private List<SuddenEventTableList> MyEventList = new List<SuddenEventTableList>();       // ���� ���� �̺�Ʈ ���
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
