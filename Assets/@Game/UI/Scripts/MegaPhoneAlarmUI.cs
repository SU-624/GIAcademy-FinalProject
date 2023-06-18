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

    [Header("�л� ����â�� �а� �ε��� ������")]
    [SerializeField] private Image MegaphoneMessageBG;
    [SerializeField] private TextMeshProUGUI MegaphoneTextMessage;

    [SerializeField] private Button PopUpMegaphoneButton;
    [SerializeField] private Button PopOffMegaphoneButton;

    [SerializeField] private GameObject MegaphoneMessageBox;

    // �� �ָ��� ����, ���� �̺�Ʈ�� ������ ���� �� �� ���� �� ��

    // ��� �˶��� ��� ����Ʈ?
    // List<> 

    // �� ���� �˶��� ��� �� ����Ʈ?
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
        // ���� ��¥�� �̺�Ʈ�� ��¥�� �� ������ �� �ؼ�, �ش� ��¥�� �̺�Ʈ�� ������ �˶��� �ߵ��� �Ѵ�
        // Ȥ�� ���� ������ ����ٸ�, �� �� �����Ͽ� �˶� -> �̹� �ֿ� ���� �˶�

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
