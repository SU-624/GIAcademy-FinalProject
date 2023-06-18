using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public struct Alram
{
    int AlramNumber;

}

public class MegaPhoneAlarmUI : MonoBehaviour
{
    public delegate void MyEventDelegate();
    MyEventDelegate myEventDelegate;

    [Header("학생 정보창의 학과 인덱스 변수들")]
    [SerializeField] private Image MegaphoneMessageBG;
    [SerializeField] private TextMeshProUGUI MegaphoneTextMessage;

    [SerializeField] private Button PopUpMegaphoneButton;
    [SerializeField] private Button PopOffMegaphoneButton;

    [SerializeField] private GameObject MegaphoneMessageBox;

    // 매 주마다 고정, 선택 이벤트의 갯수를 저장 해 줄 변수 두 개

    // 모든 알람이 담길 리스트?
    // List<> 

    // 매 달의 알람을 모아 둘 리스트?
    // List<>

    // Start is called before the first frame update
    void Start()
    {
        // Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // myEventDelegate();
    }

    public void Initialize()
    {
        MegaphoneMessageBG.gameObject.SetActive(false);
        PopUpMegaphoneButton.gameObject.SetActive(true);
        PopOffMegaphoneButton.gameObject.SetActive(true);

        PopUpMegaphoneButton.onClick.AddListener(IfClickMegaphoneButton);
        PopOffMegaphoneButton.onClick.AddListener(IfClickMegaphoneButton);

        // myEventDelegate += CheckAndExecuteEventList;
    }

    public void CheckAndExecuteEventList()
    {
        // 현재 날짜와 이벤트의 날짜를 매 프레임 비교 해서, 해당 날짜에 이벤트가 있으면 알람이 뜨도록 한다
        // 혹시 변경 사항이 생긴다면, 매 주 월요일에 알람 -> 이번 주에 있을 알람

        if (SwitchEventList.Instance.ThisMonthMySelectEventList != null)
        {
            for (int i = 0; i < SwitchEventList.Instance.ThisMonthMySelectEventList.Count; i++)
            {
                if (SwitchEventList.Instance.ThisMonthMySelectEventList[i].SuddenEventDate[1] == GameTime.Instance.FlowTime.NowMonth)
                {
                    if (SwitchEventList.Instance.ThisMonthMySelectEventList[i].SuddenEventDate[2] == GameTime.Instance.FlowTime.NowWeek)
                    {
                        if (SwitchEventList.Instance.ThisMonthMySelectEventList[i].SuddenEventDate[3] == GameTime.Instance.FlowTime.NowDay)
                        {
                            MegaphoneTextMessage.text = SwitchEventList.Instance.ThisMonthMySelectEventList[i].SuddenEventID.ToString();

                            if (MegaphoneMessageBG.gameObject.activeSelf == false)
                            {
                                MegaphoneMessageBG.gameObject.SetActive(true);
                                Invoke("SetActiveFalseMessageBox", 1);
                            }
                        }
                    }
                }
            }
        }
    }

    public void SetActiveFalseMessageBox()
    {
        MegaphoneMessageBG.gameObject.SetActive(false);
    }

    public void IfClickMegaphoneButton()
    {
        if (PopUpMegaphoneButton.gameObject.activeSelf == true)
        {
            MegaphoneMessageBG.gameObject.SetActive(false);
            MegaphoneMessageBox.SetActive(true);

            PopUpMegaphoneButton.gameObject.SetActive(false);
            PopOffMegaphoneButton.gameObject.SetActive(true);
        }
        else if (PopOffMegaphoneButton.gameObject.activeSelf == true)
        {
            MegaphoneMessageBG.gameObject.SetActive(false);
            MegaphoneMessageBox.SetActive(false);

            PopUpMegaphoneButton.gameObject.SetActive(true);
            PopOffMegaphoneButton.gameObject.SetActive(false);
        }
    }
}
