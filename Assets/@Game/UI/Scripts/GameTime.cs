using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class NowTime
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
///  2023. 04. 03 Mang
/// 
/// �ð� : string -> int �� ����. UI �� ���̱⸸ �ϵ���
/// 
/// (�ð�����)
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


    public NowTime FlowTime = new NowTime();

    const float LimitTime1 = 10.0f;      // 1 ~ 2�� ���ѽð�
    const float LimitTime2 = 100.0f;     // 3 ~ 4�� ���ѽð�
    float PrevTime = 0;

    public int Year;           // 1 ~ 3��(���Ӹ��) - ����(���Ѹ��)
    public int Month;          // 1 ~ 12��(12)
    public int Week;          // 1 ~ 4��(4)
    public int Day;            // �� ~ ��(5)

    public Image TimeBarImg;
    public Button m_ClassOpenButton;

    [SerializeField] private int FirstHalfPerSecond;       //  (1�� - 2��) �Ϸ��� �ð� 2��(�� �� �� 10��)
    [SerializeField] private int SecondHalfPerSecond;      // (3�� - 4��)�Ϸ��� �ð��� 20��                  
    [SerializeField] private int VacationDelay;
    [SerializeField] private int RecommendDelay;

    [SerializeField] private TreeChanger m_TreeChanger;

    public int ThirdFourthWeekTime
    {
        get { return SecondHalfPerSecond; }
    }

    public bool IsGameMode = false;                 // ���ΰ���ȭ�� or UI â ȭ�� üũ�ؼ� �� ��� ���� ������ �͵��� �ϱ� ���� ����

    public bool IsMonthCycleCompleted = false;      // �� - �� ����Ŭ ���Ҵ��� üũ�� ����
    public bool IsOneSemesterCompleted = false;     // 3����(�� �б�) ����Ŭ ���Ҵ��� üũ
    public bool IsYearCycleCompleted = false;       // �� - �� ����Ŭ ���Ҵ��� üũ�� ����
    public bool IsGameEnd = false;                  // ������ ������ �� üũ�� ����

    public bool isChangeWeek = false;
    private bool FirstGameOpen = true;          // ó�� ������ ����Ǿ��ٴ� ���� �ǰ��� �� �� ģ��
    private bool CreateStudent = false;

    public Alarm AlarmControl;

    public UIManager UiManager;
    private bool _autosaveTimer;

    [SerializeField] private PopUpUI m_VacationPopUp;

    public bool m_IsVacationNotice = false;
    public bool m_IsRecommendNotice = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        m_TreeChanger = GameObject.Find("SeasonChanger").GetComponent<TreeChanger>();

        if (GameObject.Find("LoadingCanvas") != null)
        {
            if (GameObject.Find("LoadingCanvas").activeSelf)
            {
                GameObject.Find("LoadingCanvas").SetActive(false);
            }
        }

        Debug.Log("GameTime");

        if (Json.Instance.UseLoadingData)
        {
            Year = AllInOneData.Instance.PlayerData.Year;
            Month = AllInOneData.Instance.PlayerData.Month;
            Week = AllInOneData.Instance.PlayerData.Week;
            Day = AllInOneData.Instance.PlayerData.Day;


        }
        else
        {
            Year = 1;           // 1 ~ 3��(���Ӹ��) - ����(���Ѹ��)
            Month = 3;          // 1 ~ 12��(12)
            Week = 1;           // 1 ~ 4��(4)
            Day = 1;            // �� ~ ��(5)  -> �ð��� �ٷ� �帣�Ƿ� ó���� 0���� �������ش�.

            TimeBarImg.fillAmount = 0.2f;
            
            // Ʃ�丮��� �ʱ�ȭ
            PlayerInfo.Instance.IsFirstClassSetting = true;
            PlayerInfo.Instance.IsFirstGameJam = true;
            PlayerInfo.Instance.IsFirstGameShow = true;
            PlayerInfo.Instance.IsFirstClassEnd = false;
            PlayerInfo.Instance.IsFirstVacation = true;
            PlayerInfo.Instance.IsFirstInJaeRecommend = true;
            PlayerInfo.Instance.IsGameJamMiniGameFirst = true;
        }

        if (Month == 4)
        {
            m_TreeChanger.ChangeMaterial(0);
        }
        else if (Month == 3 || Month == 5 || Month == 6 || Month == 7 || Month == 8 || Month == 9)
        {
            m_TreeChanger.ChangeMaterial(1);
        }
        else if (Month == 10 || Month == 11)
        {
            m_TreeChanger.ChangeMaterial(2);
        }
        else if (Month == 12 || Month == 1 || Month == 2)
        {
            m_TreeChanger.ChangeMaterial(3);
        }

    }

    // Start is called before the first frame update
    public void Start()
    {
        //Debug.Log(SecondHalfPerSecond);
        // Debug.Log(TimeBarImg.fillAmount);
        //Debug.Log(Day);

        IsGameMode = true;
        //Debug.Log(IsGameMode);

        // Month[11] = "12��";
        // Week[0] = "ù°��";
        m_DrawnowTime.text = Year + "�� " + Month + "�� " + Week + "��";

        FlowTime.NowYear = Year;
        FlowTime.NowMonth = Month;
        FlowTime.NowWeek = Week;
        FlowTime.NowDay = Day;

        //Debug.Log(Year + "��" + " " + Month + " " + Week);

        //ShowGameTime();


    }

    // bool call = false;
    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Time.timeScale = InGameUI.Instance.m_NowGameSpeed;
        }

        if (!CreateStudent)
        {
            ObjectManager.Instance.CreateAllStudent();
            CreateStudent = true;
        }

        if (IsGameMode == true && PlayerInfo.Instance.IsFirstAcademySetting)
        {
            FlowtheTime();

            //ShowGameTime();
        }
    }

    public void FlowtheTime()
    {
        m_DrawnowTime.text = Year + "�� " + Month + "�� " + Week + "��";

        CheckPerSecond();

        // 30�� �Ѿ�� �� �� ��ȭ
        if (isChangeWeek)     // 
        {
            ChangeMonth();

            ChangeWeek();

            //Debug.Log("Time.time : " + Time.time);

            // nowTime = Year + "�� " + Month[MonthIndex] + " " + Week[WeekIndex];
            if (Week != 4)
            {
                m_DrawnowTime.text = Year + "�� " + Month + "�� " + Week + "��";

                FlowTime.NowWeek = Week;
            }

            FlowTime.NowYear = Year;
            FlowTime.NowMonth = Month;
            FlowTime.NowWeek = Week;

            // 3���̶�� ���� �ð��� ������ �� ��
            if (Year == 3 && Month == 12 && Week == 4)
            {
                IsLimitedGameTimeEnd();
            }

            PrevTime = 0.0f;
            isChangeWeek = false;
        }


        //if (Month == 3 && Week == 1 && Day == 2)
        //{
        //    AlarmControl.AlarmMessageQ.Enqueue("������ 3�� ù°�� ȭ�����Դϴ�!");
        //}
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
            FlowTime.NowDay = Day;

            PrevTime = Time.time;
            //Debug.Log("���� : " + Day);
        }
        else if (Day >= 5)
        {
            Day = 1;
            FlowTime.NowDay = Day;

            TimeBarImg.fillAmount = 0.2f;
            isChangeWeek = true;

            PrevTime = Time.time;
        }

        if (Month == 2 && Week == 3 && Day == 5)
        {
            AlarmControl.AlarmMessageQ.Enqueue("������õ �Ⱓ�� ����Ǿ����ϴ�.");
            AlarmControl.AlarmMessageQ.Enqueue("������ ���۵Ǿ����ϴ�.");
        }
    }

    // �� ����
    public void ChangeWeek()
    {
        // Week ����
        if (Week < 4)
        {
            Week++;

            //Debug.Log("�� : " + Week);
            if (Month == 2 && Week == 4)
            {
                AlarmControl.AlarmMessageQ.Enqueue("�л����� �����Ͽ����ϴ�.");
            }
        }
        else if (Week >= 4)
        {
            Week = 1;
        }

    }

    // ��, �� ����
    public void ChangeMonth()
    {
        // ���� ���� �� �� �ʱ�ȭ
        if (Month >= 12 && Week >= 4)
        {
            Month = 1;

            Year++;
            //AlarmControl.AlarmMessageQ.Enqueue("�ܿ��� �Խ��ϴ�.");
        }
        // �� ����
        else if (Month < 12 && Week >= 4)
        {
            Month++;
            //Debug.Log("�� : " + Month);
            if (Month == 2)
            {
                AlarmControl.AlarmMessageQ.Enqueue("������õ �Ⱓ�� �ٰ��Խ��ϴ�. (2�� 1����~ 2�� 3����)");
            }
            else if (Month == 3)
            {
                // 3���� ���۵Ǹ� �л��� �����մϴ�.
                CreateStudent = false;
                AlarmControl.AlarmMessageQ.Enqueue("1�бⰡ ���۵Ǿ����ϴ�.");
                m_TreeChanger.ChangeMaterial(1);
            }
            else if (Month == 4)
            {
                //AlarmControl.AlarmMessageQ.Enqueue("���� �Խ��ϴ�.");
                m_TreeChanger.ChangeMaterial(0);
            }
            else if (Month == 5)
            {
                m_TreeChanger.ChangeMaterial(1);
            }
            else if (Month == 6)
            {
                AlarmControl.AlarmMessageQ.Enqueue("2�бⰡ ���۵Ǿ����ϴ�.");
            }
            else if (Month == 7)
            {
                //AlarmControl.AlarmMessageQ.Enqueue("������ �Խ��ϴ�.");
            }
            else if (Month == 9)
            {
                AlarmControl.AlarmMessageQ.Enqueue("3�бⰡ ���۵Ǿ����ϴ�.");
            }
            else if (Month == 10)
            {
                //AlarmControl.AlarmMessageQ.Enqueue("������ �Խ��ϴ�.");
                m_TreeChanger.ChangeMaterial(2);
            }
            else if (Month == 12)
            {
                AlarmControl.AlarmMessageQ.Enqueue("4�бⰡ ���۵Ǿ����ϴ�.");
                m_TreeChanger.ChangeMaterial(3);
            }
        }

    }

    public void CheckPerSecond()
    {
        // ������ �������� �� ĳ���ʹ� ���������� �ð��� �帣���ʰ� �� �� PrevTime�� ���������� ������ ���� ���� �� �ð��� ������� �帥��.
        if ((InGameTest.Instance.m_ClassState == ClassState.nothing && Month == 3 && Week == 1 && Day == 1) || InGameTest.Instance.m_ClassState == ClassState.ClassStart || m_IsVacationNotice)
        {
            PrevTime = Time.time;
        }
        // �������� �� �ݿ� ������ �� ������ �ð��� �帣�� �ȵȴ�. TimeScale�� ���߸� ĳ���Ͱ� �������� ������ �ٸ� �������,,
        if (GameTime.Instance.IsGameMode == true && (InGameTest.Instance.m_ClassState != ClassState.ClassStart || (InGameTest.Instance.m_ClassState == ClassState.nothing && Month == 3 && Week == 1 && Day == 1)))
        {
            if (Week == 1 || Week == 2)
            {
                // (1 ~ 2 ����)2�ʸ��� �ð�üũ
                if (Time.time - PrevTime >= FirstHalfPerSecond)
                {
                    TimeBarImg.fillAmount += 0.2f;


                    // 1�ʸ��� �����ֱ�
                    if (FirstGameOpen == true)
                    {
                        FirstGameOpen = false;
                    }
                    if (FirstGameOpen == false)
                    {
                        ChangeDay();
                    }

                    // FirstHalfPerSecond += 2;

                    // if (FirstHalfPerSecond > LimitTime1)
                    // {
                    //     FirstHalfPerSecond = 2;
                    // }
                    _autosaveTimer = true;
                }
            }
            else if (Week == 3 || Week == 4)
            {
                // ���� ���ۿ�
                if (Month == 2 && Week == 3 && Day == 5 && Time.time - PrevTime >= VacationDelay && !m_IsVacationNotice)
                {
                    m_VacationPopUp.TurnOnUI();
                    m_IsVacationNotice = true;
                    PrevTime = Time.time;
                }
                // ���� ��õ �����
                if (Month == 2 && Week == 4 && Day == 5 && Time.time - PrevTime >= RecommendDelay && !m_IsRecommendNotice)
                {
                    m_IsRecommendNotice = true;
                }
                // (3 ~ 4 ����)6�ʸ��� �ð�üũ
                if (Time.time - PrevTime >= SecondHalfPerSecond)
                {
                    TimeBarImg.fillAmount += 0.2f;

                    // 6�ʸ��� �����ֱ�
                    ChangeDay();
                    _autosaveTimer = true;
                    // FlowTime.NowDay = Day;

                    // SecondHalfPerSecond += 20;

                    // if (SecondHalfPerSecond > LimitTime2)
                    // {
                    //     SecondHalfPerSecond = 20;
                    // }
                }

                // 2���� ���������� ���� �������� ����.
                if (Month != 2)
                {
                    // �ڵ� ������ 3����, 4������ UI�� ����, �ð��� �帣�ٰ� �Ϸ��� �ʹݿ� ���� (4����)
                    if (Time.time - PrevTime >= SecondHalfPerSecond / 5 && _autosaveTimer)
                    {
                        if (Year == 1 && Week == 3 && Day == 1) // ������ Ʃ�� �� �ѹ� ������ �� ��
                        {
                            PlayerInfo.Instance.IsFirstClassEnd = true;
                        }

                        UiManager.CollectDataBtn();
                        Json.Instance.SaveDataInLocal();
                        Debug.Log("�ڵ�����Ǿ����ϴ�."); // �ڵ����� UI�� ������ ������?
                        _autosaveTimer = false;
                    }
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
            Debug.Log("���� ���丮 ��~");

        }
    }

    public void ShowGameTime()
    {
        m_TimeText.text = FlowTime.NowYear.ToString() + FlowTime.NowMonth + FlowTime.NowWeek + FlowTime.NowDay;
    }
}