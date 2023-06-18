using System;
using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ClickEventType
{
    Interaction,
    GenreRoom,
    Facility,
    Object,
}

public class ClickEventManager : MonoBehaviour
{
    private static ClickEventManager instance = null;

    public static ClickEventManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public GameObject ClickEventPanel;
    public GameObject RepairEventPrefab;
    public GameObject StudentEventPrefab;
    public GameObject FadeTextPrefab;
    public GameObject MoneyFadeTextPrefab;
    public GameObject SPFadeTextPrefab;
    public GameObject StatBoxPrefab;

    public GameObject TouchEventPrefab;

    public SoundManager Sound;

    private float m_cameraOrthoSize = 18f;

    private List<GameObject> RepairEvents = new List<GameObject>();
    private List<GameObject> StudentEvents = new List<GameObject>();
    private List<GameObject> InstructorEvents = new List<GameObject>();

    private List<GameObject> FadeTextList = new List<GameObject>();
    private List<Vector3> FadeTextMoveUp = new List<Vector3>();

    private List<GameObject> MoneyFadeTextList = new List<GameObject>();
    private List<Vector3> MoneyFadeTextMoveUp = new List<Vector3>();

    private List<GameObject> SPFadeTextList = new List<GameObject>();
    private List<Vector3> SPFadeTextMoveUp = new List<Vector3>();

    private List<GameObject> StatBoxList = new List<GameObject>();
    private List<Vector3> StatBoxMoveUp = new List<Vector3>();


    private List<GameObject> ClickerMoneyFadeTextList = new List<GameObject>();
    private List<Vector3> ClickerMoneyFadeTextMoveUp = new List<Vector3>();

    public List<Vector3> FadeOutTextPosition
    {
        get;
        private set;
    }

    public List<Vector3> MoneyFadeOutTextPosition
    {
        get;
        private set;
    }

    public List<Vector3> SPFadeOutTextPosition
    {
        get;
        private set;
    }

    public List<Vector3> StatBoxPosition
    {
        get;
        private set;
    }

    public List<Vector3> ClickerMoneyFadeOutTextPosition
    {
        get;
        private set;
    }

    public List<Vector3> StudentPosition
    {
        get;
        private set;
    }

    public List<Student> Students
    {
        get;
        private set;
    }
    public List<Vector3> InstructorPosition
    {
        get;
        private set;
    }

    public List<Instructor> Instructors
    {
        get;
        private set;
    }

    public Alarm AlarmControl;

    private int m_studentEventIndex = 0;
    private int m_instructorEventIndex = 0;
    private int m_fadeTextIndex = 0;
    private int m_moneyFadeTextIndex = 0;
    private int m_spFadeTextIndex = 0;
    private int m_clickerMoneyFadeTextIndex = 0;
    private int m_statBoxIndex = 0;
    private int m_cameraMaxOrthoSize = 27;
    private Vector3 m_boxOffset = new Vector3(150, 100, 0);

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        StudentPosition = new List<Vector3>();
        InstructorPosition = new List<Vector3>();
        Students = new List<Student>();
        Instructors = new List<Instructor>();
        FadeOutTextPosition = new List<Vector3>();
        MoneyFadeOutTextPosition = new List<Vector3>();
        SPFadeOutTextPosition = new List<Vector3>();
        ClickerMoneyFadeOutTextPosition = new List<Vector3>();
        StatBoxPosition = new List<Vector3>();

        for (int i = 0; i < InteractionManager.Instance.GenreRoomList.Count; i++)
        {
            GameObject newRepairEvent = Instantiate(RepairEventPrefab);
            RepairEvents.Add(newRepairEvent);
            newRepairEvent.SetActive(false);
            newRepairEvent.transform.SetParent(ClickEventPanel.transform);
            newRepairEvent.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ClickedRepair);
        }

        for (int i = 0; i < 20; i++)
        {
            GameObject newStudentEvent = Instantiate(StudentEventPrefab);
            StudentEvents.Add(newStudentEvent);
            newStudentEvent.SetActive(false);
            newStudentEvent.transform.SetParent(ClickEventPanel.transform);
            newStudentEvent.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ClickedStudentEvent);

            StudentPosition.Add(Vector3.zero);
            Students.Add(null);
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject newInstructorEvent = Instantiate(StudentEventPrefab);
            InstructorEvents.Add(newInstructorEvent);
            newInstructorEvent.SetActive(false);
            newInstructorEvent.transform.SetParent(ClickEventPanel.transform);
            newInstructorEvent.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ClickedInstructorEvent);

            InstructorPosition.Add(Vector3.zero);
            Instructors.Add(null);
        }

        for (int i = 0; i < 20; i++)
        {
            GameObject newFadeText = Instantiate(FadeTextPrefab);
            FadeTextList.Add(newFadeText);
            newFadeText.SetActive(false);
            newFadeText.transform.SetParent(ClickEventPanel.transform);

            FadeOutTextPosition.Add(Vector3.zero);
            FadeTextMoveUp.Add(Vector3.zero);

            GameObject newMoneyFadeText = Instantiate(MoneyFadeTextPrefab);
            MoneyFadeTextList.Add(newMoneyFadeText);
            newMoneyFadeText.SetActive(false);
            newMoneyFadeText.transform.SetParent(ClickEventPanel.transform);

            MoneyFadeOutTextPosition.Add(Vector3.zero);
            MoneyFadeTextMoveUp.Add(Vector3.zero);

            GameObject newSPFadeText = Instantiate(SPFadeTextPrefab);
            SPFadeTextList.Add(newSPFadeText);
            newSPFadeText.SetActive(false);
            newSPFadeText.transform.SetParent(ClickEventPanel.transform);

            SPFadeOutTextPosition.Add(Vector3.zero);
            SPFadeTextMoveUp.Add(Vector3.zero);

            GameObject newStatBox = Instantiate(StatBoxPrefab);
            StatBoxList.Add(newStatBox);
            newStatBox.SetActive(false);
            newStatBox.transform.SetParent(ClickEventPanel.transform);

            StatBoxPosition.Add(Vector3.zero);
            StatBoxMoveUp.Add(Vector3.zero);
        }

        for (int i = 0; i < 100; i++)
        {
            GameObject newMoneyFadeText = Instantiate(MoneyFadeTextPrefab);
            ClickerMoneyFadeTextList.Add(newMoneyFadeText);
            newMoneyFadeText.SetActive(false);
            newMoneyFadeText.transform.SetParent(ClickEventPanel.transform);

            ClickerMoneyFadeOutTextPosition.Add(Vector3.zero);
            ClickerMoneyFadeTextMoveUp.Add(Vector3.zero);
        }

        Sound.PlayInGameBgm();
    }

    // Update is called once per frame
    void Update()
    {
        if (InteractionManager.Instance != null)
        {
            for (int i = 0; i < InteractionManager.Instance.GenreRoomList.Count; i++)
            {
                if (InteractionManager.Instance.GenreRoomList[i].Durability <= 0 && RepairEvents[i].activeSelf == false)
                {
                    RepairEvents[i].SetActive(true);
                    AlarmControl.AlarmMessageQ.Enqueue("<color=#00EAFF>" + InteractionManager.Instance.GenreRoomList[i].GenreRoomName + "</color> 수리가 필요합니다.");
                }

                if (RepairEvents[i].activeSelf)
                {
                    RepairEvents[i].transform.position =
                        Camera.main.WorldToScreenPoint(InteractionManager.Instance.GenreRoomCenters[i].position);

                    RepairEvents[i].transform.localScale = Vector3.one + (Vector3.one * (-Camera.main.orthographicSize + m_cameraOrthoSize) * 0.2f);
                }
            }
        }

        if (StudentPosition.Count != 0)
        {
            for (int i = 0; i < StudentPosition.Count; i++)
            {
                if (StudentPosition[i] != Vector3.zero)
                {
                    StudentEvents[i].SetActive(true);
                    StudentEvents[i].transform.position =
                        Camera.main.WorldToScreenPoint(StudentPosition[i] + new Vector3(0, 1, 0));
                    StudentEvents[i].transform.localScale = Vector3.one + (Vector3.one * (-Camera.main.orthographicSize + m_cameraOrthoSize) * 0.2f);
                }
            }
        }

        if (InstructorPosition.Count != 0)
        {
            for (int i = 0; i < InstructorPosition.Count; i++)
            {
                if (InstructorPosition[i] != Vector3.zero)
                {
                    InstructorEvents[i].SetActive(true);
                    InstructorEvents[i].transform.position =
                        Camera.main.WorldToScreenPoint(InstructorPosition[i] + new Vector3(1, 1, 0));
                    InstructorEvents[i].transform.localScale = Vector3.one + (Vector3.one * (-Camera.main.orthographicSize + m_cameraOrthoSize) * 0.2f);
                }
            }
        }

        if (FadeOutTextPosition.Count != 0)
        {
            for (int i = 0; i < FadeOutTextPosition.Count; i++)
            {
                if (FadeOutTextPosition[i] != Vector3.zero)
                {
                    FadeTextList[i].transform.localScale = Vector3.one + (Vector3.one * (-Camera.main.orthographicSize + m_cameraMaxOrthoSize) * 0.1f);
                    FadeTextList[i].SetActive(true);
                    FadeTextList[i].transform.position =
                        Camera.main.WorldToScreenPoint(FadeOutTextPosition[i]) + m_boxOffset;
                    FadeTextList[i].transform.position += FadeTextMoveUp[i];
                }
            }
        }

        if (MoneyFadeOutTextPosition.Count != 0)
        {
            for (int i = 0; i < MoneyFadeOutTextPosition.Count; i++)
            {
                if (MoneyFadeOutTextPosition[i] != Vector3.zero)
                {
                    MoneyFadeTextList[i].transform.localScale = Vector3.one + (Vector3.one * (-Camera.main.orthographicSize + m_cameraMaxOrthoSize) * 0.1f);
                    MoneyFadeTextList[i].SetActive(true);
                    MoneyFadeTextList[i].transform.position =
                        Camera.main.WorldToScreenPoint(MoneyFadeOutTextPosition[i]) + m_boxOffset;
                    MoneyFadeTextList[i].transform.position += MoneyFadeTextMoveUp[i];
                }
            }
        }

        if (SPFadeOutTextPosition.Count != 0)
        {
            for (int i = 0; i < SPFadeOutTextPosition.Count; i++)
            {
                if (SPFadeOutTextPosition[i] != Vector3.zero)
                {
                    SPFadeTextList[i].transform.localScale = Vector3.one + (Vector3.one * (-Camera.main.orthographicSize + m_cameraMaxOrthoSize) * 0.1f);
                    SPFadeTextList[i].SetActive(true);
                    SPFadeTextList[i].transform.position =
                        Camera.main.WorldToScreenPoint(SPFadeOutTextPosition[i]) + m_boxOffset;
                    SPFadeTextList[i].transform.position += SPFadeTextMoveUp[i];
                }
            }
        }

        if (ClickerMoneyFadeOutTextPosition.Count != 0)
        {
            for (int i = 0; i < ClickerMoneyFadeOutTextPosition.Count; i++)
            {
                if (ClickerMoneyFadeOutTextPosition[i] != Vector3.zero)
                {
                    ClickerMoneyFadeTextList[i].transform.localScale = Vector3.one + (Vector3.one * (-Camera.main.orthographicSize + m_cameraMaxOrthoSize) * 0.1f);
                    ClickerMoneyFadeTextList[i].SetActive(true);
                    ClickerMoneyFadeTextList[i].transform.position =
                        Camera.main.WorldToScreenPoint(ClickerMoneyFadeOutTextPosition[i]) + m_boxOffset;
                    ClickerMoneyFadeTextList[i].transform.position += ClickerMoneyFadeTextMoveUp[i];
                }
            }
        }

        if (StatBoxPosition.Count != 0)
        {
            for (int i = 0; i < StatBoxPosition.Count; i++)
            {
                if (StatBoxPosition[i] != Vector3.zero)
                {
                    StatBoxList[i].transform.localScale = Vector3.one + (Vector3.one * (-Camera.main.orthographicSize + m_cameraMaxOrthoSize) * 0.1f);
                    StatBoxList[i].SetActive(true);
                    StatBoxList[i].transform.position =
                        Camera.main.WorldToScreenPoint(StatBoxPosition[i]) + m_boxOffset;
                    StatBoxList[i].transform.position += StatBoxMoveUp[i];
                }
            }
        }
    }

    private void ClickedRepair()
    {
        Sound.PlayRepairSound();

        int num = RepairEvents.IndexOf(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        InteractionManager.Instance.GenreRoomList[num].Durability = 100;
        InteractionManager.Instance.GenreRoomList[num].RepairCount++;
        RepairEvents[num].SetActive(false);
        PlayerInfo.Instance.m_MyMoney -= 10000;
        MonthlyReporter.Instance.m_NowMonth.ExpensesEventResult += 10000;
        StartCoroutine(MoneyFadeOutText(InteractionManager.Instance.GenreRoomCenters[num].position + new Vector3(3, 0, 0), false, "10000"));
    }

    public IEnumerator FadeOutText(Vector3 pos, int type, bool isIncome, params string[] text)
    {
        GameObject nowFadeTextObj = FadeTextList[m_fadeTextIndex];
        FadeOutTextPosition[m_fadeTextIndex] = pos;

        int nowIndex = m_fadeTextIndex;

        m_fadeTextIndex++;
        if (m_fadeTextIndex >= 20)
        {
            m_fadeTextIndex = 0;
        }

        TextMeshProUGUI nowText = nowFadeTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nowText.text = "";
        Image nowTextBoxImage = nowFadeTextObj.transform.GetChild(1).GetComponent<Image>();
        nowTextBoxImage.color = new Color(1, 1, 1, 1);

        TextMeshProUGUI nowText2 = nowFadeTextObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        nowText2.text = "";
        Image nowTextBoxImage2 = nowFadeTextObj.transform.GetChild(3).GetComponent<Image>();
        nowTextBoxImage2.color = new Color(1, 1, 1, 1);

        // 1 : 친밀도, 2 : 체력, 열정
        if (type == 1)
        {
            nowTextBoxImage.sprite = ScriptsManager.Instance.OtherSprites[0];
            nowText2.gameObject.SetActive(false);
            nowTextBoxImage2.gameObject.SetActive(false);
        }
        else
        {
            nowTextBoxImage.sprite = ScriptsManager.Instance.OtherSprites[1];
            nowTextBoxImage2.sprite = ScriptsManager.Instance.OtherSprites[2];
            nowText2.gameObject.SetActive(true);
            nowTextBoxImage2.gameObject.SetActive(true);
        }

        //if (isIncome)
        //{
        //    nowText.color = Color.blue;
        //    nowText.text += "+";
        //}
        //else
        //{
        //    nowText.color = Color.red;
        //    nowText.text += "-";
        //}

        //for (int i = 0; i < text.Length; i++)
        //{
        //    nowText.text += text[i];
        //}

        if (text.Length == 1)
        {
            nowText.text = text[0];
        }
        else
        {
            nowText.text = text[0];
            nowText2.text = text[1];
        }

        nowFadeTextObj.SetActive(true);

        while (nowText.color.a > 0f)
        {
            FadeTextMoveUp[nowIndex] += new Vector3(0, 0.3f, 0);
            nowText.color = new Color(nowText.color.r, nowText.color.g, nowText.color.b, nowText.color.a - (Time.deltaTime / 2.0f));
            nowTextBoxImage.color = new Color(nowTextBoxImage.color.r, nowTextBoxImage.color.g, nowTextBoxImage.color.b, nowTextBoxImage.color.a - (Time.deltaTime / 2.0f));
            nowText2.color = new Color(nowText2.color.r, nowText2.color.g, nowText2.color.b, nowText2.color.a - (Time.deltaTime / 2.0f));
            nowTextBoxImage2.color = new Color(nowTextBoxImage2.color.r, nowTextBoxImage2.color.g, nowTextBoxImage2.color.b, nowTextBoxImage2.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }


        FadeTextMoveUp[nowIndex] = Vector3.zero;
        FadeOutTextPosition[nowIndex] = Vector3.zero;
        nowFadeTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        nowFadeTextObj.transform.position = Vector3.zero;
        nowFadeTextObj.SetActive(false);
    }

    public IEnumerator MoneyFadeOutText(Vector3 pos, bool isIncome, params string[] text)
    {
        GameObject nowFadeTextObj = MoneyFadeTextList[m_moneyFadeTextIndex];
        MoneyFadeOutTextPosition[m_moneyFadeTextIndex] = pos;

        int nowIndex = m_moneyFadeTextIndex;

        m_moneyFadeTextIndex++;
        if (m_moneyFadeTextIndex >= 20)
        {
            m_moneyFadeTextIndex = 0;
        }

        TextMeshProUGUI nowText = nowFadeTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nowText.text = "";
        Image nowMoneyImage = nowFadeTextObj.transform.GetChild(1).GetComponent<Image>();
        nowMoneyImage.color = new Color(1, 1, 1, 1);

        if (isIncome)
        {
            nowText.color = Color.blue;
            nowText.text += "+";
        }
        else
        {
            nowText.color = Color.red;
            nowText.text += "-";
        }

        for (int i = 0; i < text.Length; i++)
        {
            nowText.text += text[i];
        }

        nowFadeTextObj.SetActive(true);

        while (nowText.color.a > 0f)
        {
            MoneyFadeTextMoveUp[nowIndex] += new Vector3(0, 0.3f, 0);
            nowText.color = new Color(nowText.color.r, nowText.color.g, nowText.color.b, nowText.color.a - (Time.deltaTime / 2.0f));
            nowMoneyImage.color = new Color(nowMoneyImage.color.r, nowMoneyImage.color.g, nowMoneyImage.color.b, nowMoneyImage.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }

        MoneyFadeTextMoveUp[nowIndex] = Vector3.zero;
        MoneyFadeOutTextPosition[nowIndex] = Vector3.zero;
        nowFadeTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        nowFadeTextObj.transform.position = Vector3.zero;
        nowFadeTextObj.SetActive(false);
    }

    public IEnumerator SPFadeOutText(Vector3 pos, bool isIncome, params string[] text)
    {
        GameObject nowFadeTextObj = SPFadeTextList[m_spFadeTextIndex];
        SPFadeOutTextPosition[m_spFadeTextIndex] = pos;

        int nowIndex = m_spFadeTextIndex;

        m_spFadeTextIndex++;
        if (m_spFadeTextIndex >= 20)
        {
            m_spFadeTextIndex = 0;
        }

        TextMeshProUGUI nowText = nowFadeTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nowText.text = "";
        Image nowSPImage = nowFadeTextObj.transform.GetChild(1).GetComponent<Image>();
        nowSPImage.color = new Color(1, 1, 1, 1);

        if (isIncome)
        {
            nowText.color = Color.blue;
            nowText.text += "+";
        }
        else
        {
            nowText.color = Color.red;
            nowText.text += "-";
        }

        for (int i = 0; i < text.Length; i++)
        {
            nowText.text += text[i];
        }

        nowFadeTextObj.SetActive(true);

        while (nowText.color.a > 0f)
        {
            SPFadeTextMoveUp[nowIndex] += new Vector3(0, 0.3f, 0);
            nowText.color = new Color(nowText.color.r, nowText.color.g, nowText.color.b, nowText.color.a - (Time.deltaTime / 2.0f));
            nowSPImage.color = new Color(nowSPImage.color.r, nowSPImage.color.g, nowSPImage.color.b, nowSPImage.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }


        SPFadeTextMoveUp[nowIndex] = Vector3.zero;
        SPFadeOutTextPosition[nowIndex] = Vector3.zero;
        nowFadeTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        nowFadeTextObj.transform.position = Vector3.zero;
        nowFadeTextObj.SetActive(false);
    }

    public IEnumerator ClickerMoneyFadeOutText(Vector3 pos, params string[] text)
    {
        GameObject nowMoneyFadeTextObj = ClickerMoneyFadeTextList[m_clickerMoneyFadeTextIndex];
        ClickerMoneyFadeOutTextPosition[m_clickerMoneyFadeTextIndex] = pos;

        int nowIndex = m_clickerMoneyFadeTextIndex;

        m_clickerMoneyFadeTextIndex++;
        if (m_clickerMoneyFadeTextIndex >= 100)
        {
            m_clickerMoneyFadeTextIndex = 0;
        }

        TextMeshProUGUI nowText = nowMoneyFadeTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nowText.text = "";
        Image nowMoneyImage = nowMoneyFadeTextObj.transform.GetChild(1).GetComponent<Image>();
        nowMoneyImage.color = new Color(1, 1, 1, 1);

        nowText.color = Color.blue;
        nowText.text += "+";

        for (int i = 0; i < text.Length; i++)
        {
            nowText.text += text[i];
        }

        nowMoneyFadeTextObj.SetActive(true);

        while (nowText.color.a > 0f)
        {
            ClickerMoneyFadeTextMoveUp[nowIndex] += new Vector3(0, 2f, 0);
            nowText.color = new Color(nowText.color.r, nowText.color.g, nowText.color.b, nowText.color.a - (Time.deltaTime / 1.0f));
            nowMoneyImage.color = new Color(nowMoneyImage.color.r, nowMoneyImage.color.g, nowMoneyImage.color.b, nowMoneyImage.color.a - (Time.deltaTime / 1.0f));
            yield return null;
        }

        ClickerMoneyFadeTextMoveUp[nowIndex] = Vector3.zero;
        ClickerMoneyFadeOutTextPosition[nowIndex] = Vector3.zero;
        nowMoneyFadeTextObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        nowMoneyFadeTextObj.transform.position = Vector3.zero;
        nowMoneyFadeTextObj.SetActive(false);
    }
    public IEnumerator StatBoxFadeOut(Vector3 pos, bool isGenre, int statType, params string[] text)
    {
        GameObject nowStatBoxObj = StatBoxList[m_statBoxIndex];
        StatBoxPosition[m_statBoxIndex] = pos;

        int nowIndex = m_statBoxIndex;

        m_statBoxIndex++;
        if (m_statBoxIndex >= 20)
        {
            m_statBoxIndex = 0;
        }

        TextMeshProUGUI nowText = nowStatBoxObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nowText.text = "";
        Image nowStatBoxImage = nowStatBoxObj.transform.GetChild(1).GetComponent<Image>();
        nowStatBoxImage.color = new Color(1, 1, 1, 1);

        // 장르 스탯
        if (isGenre)
        {
            nowStatBoxImage.sprite = ScriptsManager.Instance.GenreSprites[statType];
        }
        // 기본 스탯
        else
        {
            nowStatBoxImage.sprite = ScriptsManager.Instance.StatSprites[statType];
        }

        nowText.color = Color.blue;
        nowText.text += "+";

        for (int i = 0; i < text.Length; i++)
        {
            nowText.text += text[i];
        }

        nowStatBoxObj.SetActive(true);

        while (nowText.color.a > 0f)
        {
            StatBoxMoveUp[nowIndex] += new Vector3(0, 0.3f, 0);
            nowText.color = new Color(nowText.color.r, nowText.color.g, nowText.color.b, nowText.color.a - (Time.deltaTime / 2.0f));
            nowStatBoxImage.color = new Color(nowStatBoxImage.color.r, nowStatBoxImage.color.g, nowStatBoxImage.color.b, nowStatBoxImage.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }


        StatBoxMoveUp[nowIndex] = Vector3.zero;
        StatBoxPosition[nowIndex] = Vector3.zero;
        nowStatBoxObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        nowStatBoxObj.transform.position = Vector3.zero;
        nowStatBoxObj.SetActive(false);
    }

    public void StudentEvent(Vector3 studentPos, ClickEventType eventType)
    {
        StudentPosition[m_studentEventIndex] = studentPos;
        StudentEvents[m_studentEventIndex].GetComponent<ClickEventData>().eventType = eventType;

        StartCoroutine(DeleteStudentEvent(m_studentEventIndex));
        m_studentEventIndex++;
        if (m_studentEventIndex >= 20)
        {
            m_studentEventIndex = 0;
        }
    }

    public void StudentSpecialEvent(Student student, Vector3 studentPos, ClickEventType eventType)
    {
        StudentPosition[m_studentEventIndex] = studentPos;
        Students[m_studentEventIndex] = student;
        StudentEvents[m_studentEventIndex].GetComponent<ClickEventData>().eventType = eventType;

        StartCoroutine(DeleteStudentSpecialEvent(m_studentEventIndex));
        m_studentEventIndex++;
        if (m_studentEventIndex >= 20)
        {
            m_studentEventIndex = 0;
        }
    }

    public void InstructorSpecialEvent(Instructor instructor, Vector3 instructorPos, ClickEventType eventType)
    {
        InstructorPosition[m_instructorEventIndex] = instructorPos;
        Instructors[m_instructorEventIndex] = instructor;
        InstructorEvents[m_instructorEventIndex].GetComponent<ClickEventData>().eventType = eventType;

        StartCoroutine(DeleteInstructorSpecialEvent(m_instructorEventIndex));
        m_instructorEventIndex++;
        if (m_instructorEventIndex >= 3)
        {
            m_instructorEventIndex = 0;
        }
    }

    IEnumerator DeleteStudentEvent(int index)
    {
        yield return new WaitForSeconds(5f);

        StudentPosition[index] = Vector3.zero;
        StudentEvents[index].SetActive(false);
    }

    IEnumerator DeleteStudentSpecialEvent(int index)
    {
        yield return new WaitForSeconds(3f);

        Students[index] = null;
        StudentPosition[index] = Vector3.zero;
        StudentEvents[index].SetActive(false);
    }

    IEnumerator DeleteInstructorSpecialEvent(int index)
    {
        yield return new WaitForSeconds(3f);

        Instructors[index] = null;
        InstructorPosition[index] = Vector3.zero;
        InstructorEvents[index].SetActive(false);
    }

    public void ClickedStudentEvent()
    {
        int index = StudentEvents.IndexOf(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        StudentEvents[index].SetActive(false);

        if (StudentEvents[index].GetComponent<ClickEventData>().eventType == ClickEventType.Interaction)
        {
            GameObject otherObject = Students[index].InteractingObj;
            bool isProfessor = false;

            string myName = Students[index].m_StudentStat.m_StudentName;
            string otherName;

            if (otherObject.tag == "Instructor")
            {
                isProfessor = true;
                otherName = otherObject.GetComponent<Instructor>().m_InstructorData.professorName;
            }
            else
            {
                otherName = otherObject.GetComponent<Student>().m_StudentStat.m_StudentName;
            }

            //Student otherStudent = Students[index].InteractingObj.GetComponent<Student>();

            int myIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(Students[index]));
            int otherIndex;
            if (isProfessor)
            {
                otherIndex = 18 + ObjectManager.Instance.m_InstructorList.FindIndex(x => x.Equals(otherObject.GetComponent<Instructor>()));
            }
            else
            {
                otherIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(otherObject.GetComponent<Student>()));
            }

            int friendship = ObjectManager.Instance.m_Friendship[myIndex][otherIndex];

            int randomSelect = Random.Range(1, 101);
            int randomFriendship = 0;
            int randomSpecialPoint = 0;

            // 아는사이
            if (friendship < 150)
            {
                // 상호간의 친밀도 증가
                randomFriendship = Random.Range(28, 85);
                if (isProfessor)
                {
                    ObjectManager.Instance.m_Friendship[myIndex][otherIndex] += randomFriendship;
                }
                else
                {
                    ObjectManager.Instance.m_Friendship[myIndex][otherIndex] += randomFriendship;
                    ObjectManager.Instance.m_Friendship[otherIndex][myIndex] += randomFriendship;
                }
                StartCoroutine(FadeOutText(StudentPosition[index], 1, true, randomFriendship.ToString()));
            }
            // 친한사이
            else if (friendship < 300)
            {
                if (randomSelect <= 80)
                {
                    // 상호간의 친밀도 증가
                    randomFriendship = Random.Range(42, 113);
                    if (isProfessor)
                    {
                        ObjectManager.Instance.m_Friendship[myIndex][otherIndex] += randomFriendship;
                    }
                    else
                    {
                        ObjectManager.Instance.m_Friendship[myIndex][otherIndex] += randomFriendship;
                        ObjectManager.Instance.m_Friendship[otherIndex][myIndex] += randomFriendship;
                    }
                    if (friendship + randomFriendship >= 300)
                    {
                        GameTime.Instance.AlarmControl.AlarmMessageQ.Enqueue("<color=#00EAFF>" + myName + "</color> 와(과)" +
                                                                             "<color=#00EAFF>" + otherName + "</color> 이 베프가 되었습니다!");
                    }
                    StartCoroutine(FadeOutText(StudentPosition[index], 1, true, randomFriendship.ToString()));
                }
                else
                {
                    randomSpecialPoint = Random.Range(10, 22);
                    PlayerInfo.Instance.m_SpecialPoint += randomSpecialPoint;
                    StartCoroutine(SPFadeOutText(StudentPosition[index], true, randomSpecialPoint.ToString()));
                    Sound.PlaySPSound();
                }
            }
            // 베프
            else
            {
                if (isProfessor)
                {
                    if (GameTime.Instance.FlowTime.NowMonth == 2)
                    {
                        int randomReward = Random.Range(5, 11);
                        if (randomSelect <= 65)
                        {
                            randomFriendship = Random.Range(70, 183);
                            ObjectManager.Instance.m_Friendship[myIndex][otherIndex] += randomFriendship;
                        }
                        else
                        {
                            randomReward = Random.Range(10, 22);
                            otherObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomReward;
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_Passion += randomReward;
                        }
                    }
                    else
                    {
                        int randomReward = Random.Range(5, 11);
                        if (randomSelect <= 30)
                        {
                            randomFriendship = Random.Range(70, 183);
                            ObjectManager.Instance.m_Friendship[myIndex][otherIndex] += randomFriendship;
                        }
                        else if (randomSelect <= 40)
                        {
                            randomReward = Random.Range(10, 22);
                            otherObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomReward;
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_Passion += randomReward;
                        }
                        else if (randomSelect <= 52)
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] +=
                                randomReward;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Sense, randomReward.ToString()));
                        }
                        else if (randomSelect <= 64)
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] +=
                                randomReward;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Technique, randomReward.ToString()));
                        }
                        else if (randomSelect <= 76)
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] +=
                                randomReward;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Insight, randomReward.ToString()));
                        }
                        else if (randomSelect <= 88)
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] +=
                                randomReward;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Concentration, randomReward.ToString()));
                        }
                        else
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] +=
                                randomReward;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Wit, randomReward.ToString()));
                        }
                    }
                }
                else
                {

                    if (GameTime.Instance.FlowTime.NowMonth == 2)
                    {
                        randomSpecialPoint = Random.Range(10, 22);
                        PlayerInfo.Instance.m_SpecialPoint += randomSpecialPoint;
                        StartCoroutine(SPFadeOutText(StudentPosition[index], true, randomSpecialPoint.ToString()));
                        Sound.PlaySPSound();
                    }
                    else
                    {
                        int randomStat = Random.Range(3, 8);
                        if (randomSelect <= 50)
                        {
                            randomSpecialPoint = Random.Range(10, 22);
                            PlayerInfo.Instance.m_SpecialPoint += randomSpecialPoint;
                            StartCoroutine(SPFadeOutText(StudentPosition[index], true, randomSpecialPoint.ToString()));
                            Sound.PlaySPSound();
                        }
                        else if (randomSelect <= 60)
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] +=
                                randomStat;
                            otherObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] +=
                                randomStat;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Sense, randomStat.ToString()));
                        }
                        else if (randomSelect <= 70)
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] +=
                                randomStat;
                            otherObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] +=
                                randomStat;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Technique, randomStat.ToString()));
                        }
                        else if (randomSelect <= 80)
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] +=
                                randomStat;
                            otherObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] +=
                                randomStat;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Insight, randomStat.ToString()));
                        }
                        else if (randomSelect <= 90)
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] +=
                                randomStat;
                            otherObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] +=
                                randomStat;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Concentration, randomStat.ToString()));
                        }
                        else
                        {
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] +=
                                randomStat;
                            otherObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] +=
                                randomStat;
                            StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Wit, randomStat.ToString()));
                        }
                    }
                }
            }
        }

        else if (StudentEvents[index].GetComponent<ClickEventData>().eventType == ClickEventType.GenreRoom)
        {
            int randomReward = Random.Range(1, 101);
            int randomStat;

            switch (Students[index].NowRoom)
            {
                case (int)InteractionManager.SpotName.ActionRoom:
                    if (randomReward <= 80)
                    {
                        randomStat = Random.Range(9, 19);
                        StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(StudentPosition[index], true, (int)GenreStat.Action, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat
                            .m_GenreAmountList[(int)GenreStat.Action] += randomStat;
                    }
                    else if (randomReward <= 90)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Concentration, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randomStat;
                    }
                    else
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Technique, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] += randomStat;
                    }
                    break;

                case (int)InteractionManager.SpotName.AdventureRoom:
                    if (randomReward <= 80)
                    {
                        randomStat = Random.Range(9, 19);
                        StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(StudentPosition[index], true, (int)GenreStat.Adventure, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat
                            .m_GenreAmountList[(int)GenreStat.Adventure] += randomStat;
                    }
                    else if (randomReward <= 90)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Wit, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] += randomStat;
                    }
                    else
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Concentration, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randomStat;
                    }
                    break;

                case (int)InteractionManager.SpotName.PuzzleRoom:
                    if (randomReward <= 80)
                    {
                        randomStat = Random.Range(9, 19);
                        StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(StudentPosition[index], true, (int)GenreStat.Puzzle, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat
                            .m_GenreAmountList[(int)GenreStat.Puzzle] += randomStat;
                    }
                    else if (randomReward <= 90)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Sense, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] += randomStat;
                    }
                    else
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Technique, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] += randomStat;
                    }
                    break;

                case (int)InteractionManager.SpotName.RPGRoom:
                    if (randomReward <= 80)
                    {
                        randomStat = Random.Range(9, 19);
                        StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(StudentPosition[index], true, (int)GenreStat.RPG, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat
                            .m_GenreAmountList[(int)GenreStat.RPG] += randomStat;
                    }
                    else if (randomReward <= 90)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Sense, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] += randomStat;
                    }
                    else
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Insight, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] += randomStat;
                    }
                    break;

                case (int)InteractionManager.SpotName.RhythmRoom:
                    if (randomReward <= 80)
                    {
                        randomStat = Random.Range(9, 19);
                        StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(StudentPosition[index], true, (int)GenreStat.Rhythm, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat
                            .m_GenreAmountList[(int)GenreStat.Rhythm] += randomStat;
                    }
                    else if (randomReward <= 90)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Concentration, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randomStat;
                    }
                    else
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Wit, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] += randomStat;
                    }
                    break;

                case (int)InteractionManager.SpotName.ShootingRoom:
                    if (randomReward <= 80)
                    {
                        randomStat = Random.Range(9, 19);
                        StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(StudentPosition[index], true, (int)GenreStat.Shooting, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat
                            .m_GenreAmountList[(int)GenreStat.Shooting] += randomStat;
                    }
                    else if (randomReward <= 95)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Insight, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] += randomStat;
                    }
                    else
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Concentration, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randomStat;
                    }
                    break;

                case (int)InteractionManager.SpotName.SimulationRoom:
                    if (randomReward <= 80)
                    {
                        randomStat = Random.Range(9, 19);
                        StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(StudentPosition[index], true, (int)GenreStat.Simulation, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat
                            .m_GenreAmountList[(int)GenreStat.Simulation] += randomStat;
                    }
                    else if (randomReward <= 90)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Insight, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] += randomStat;
                    }
                    else
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Wit, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] += randomStat;
                    }
                    break;

                case (int)InteractionManager.SpotName.SportsRoom:
                    if (randomReward <= 80)
                    {
                        randomStat = Random.Range(9, 19);
                        StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(StudentPosition[index], true, (int)GenreStat.Sports, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat
                            .m_GenreAmountList[(int)GenreStat.Sports] += randomStat;
                    }
                    else if (randomReward <= 90)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Sense, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] += randomStat;
                    }
                    else
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Technique, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] += randomStat;
                    }
                    break;
            }
        }
        else if (StudentEvents[index].GetComponent<ClickEventData>().eventType == ClickEventType.Facility)
        {
            int randomReward = Random.Range(1, 101);
            int randomStat = 0;
            int randomStat2 = 0;

            switch (Students[index].NowRoom)
            {
                case (int)InteractionManager.SpotName.Store:
                    if (randomReward <= 70)
                    {
                        randomStat = Random.Range(390, 5201);
                        PlayerInfo.Instance.m_MyMoney += randomStat;
                        MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomStat;
                        StartCoroutine(MoneyFadeOutText(StudentPosition[index], true, randomStat.ToString()));
                        Sound.PlayMoneySound();
                    }
                    else
                    {
                        randomStat = Random.Range(26, 170);
                        PlayerInfo.Instance.m_SpecialPoint += randomStat;
                        StartCoroutine(SPFadeOutText(StudentPosition[index], true, randomStat.ToString()));
                        Sound.PlaySPSound();
                    }
                    break;

                case (int)InteractionManager.SpotName.BookStore:
                    if (randomReward <= 70)
                    {
                        randomStat = Random.Range(390, 5201);
                        PlayerInfo.Instance.m_MyMoney += randomStat;
                        MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomStat;
                        StartCoroutine(MoneyFadeOutText(StudentPosition[index], true, randomStat.ToString()));
                        Sound.PlayMoneySound();
                    }
                    else
                    {
                        randomStat = Random.Range(26, 170);
                        PlayerInfo.Instance.m_SpecialPoint += randomStat;
                        StartCoroutine(SPFadeOutText(StudentPosition[index], true, randomStat.ToString()));
                        Sound.PlaySPSound();
                    }
                    break;

                case (int)InteractionManager.SpotName.StudyRoom:
                    if (randomReward <= 25)
                    {
                        randomStat = Random.Range(10, 31);
                        StartCoroutine(SPFadeOutText(StudentPosition[index], true, randomStat.ToString()));
                        PlayerInfo.Instance.m_SpecialPoint += randomStat;
                        Sound.PlaySPSound();
                    }
                    else if (randomReward <= 40)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Sense, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] += randomStat;
                    }
                    else if (randomReward <= 55)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Technique, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] += randomStat;
                    }
                    else if (randomReward <= 70)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Insight, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] += randomStat;
                    }
                    else if (randomReward <= 85)
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Concentration, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randomStat;
                    }
                    else
                    {
                        randomStat = Random.Range(3, 7);
                        StartCoroutine(StatBoxFadeOut(StudentPosition[index], false, (int)AbilityType.Wit, randomStat.ToString()));
                        Students[index].GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] += randomStat;
                    }
                    break;

                case (int)InteractionManager.SpotName.Lounge1:
                    randomStat = Random.Range(30, 61);
                    Students[index].GetComponent<Student>().m_StudentStat.m_Health += randomStat;
                    if (Students[index].GetComponent<Student>().m_StudentStat.m_Health >= 100)
                    {
                        Students[index].GetComponent<Student>().m_StudentStat.m_Health = 100;
                    }
                    randomStat2 = Random.Range(23, 39);
                    Students[index].GetComponent<Student>().m_StudentStat.m_Passion += randomStat2;
                    if (Students[index].GetComponent<Student>().m_StudentStat.m_Passion >= 100)
                    {
                        Students[index].GetComponent<Student>().m_StudentStat.m_Passion = 100;
                    }
                    StartCoroutine(FadeOutText(StudentPosition[index], 2, true, randomStat.ToString(), randomStat2.ToString()));
                    break;

                case (int)InteractionManager.SpotName.Lounge2:
                    randomStat = Random.Range(30, 61);
                    Students[index].GetComponent<Student>().m_StudentStat.m_Health += randomStat;
                    if (Students[index].GetComponent<Student>().m_StudentStat.m_Health >= 100)
                    {
                        Students[index].GetComponent<Student>().m_StudentStat.m_Health = 100;
                    }
                    randomStat2 = Random.Range(23, 39);
                    Students[index].GetComponent<Student>().m_StudentStat.m_Passion += randomStat2;
                    if (Students[index].GetComponent<Student>().m_StudentStat.m_Passion >= 100)
                    {
                        Students[index].GetComponent<Student>().m_StudentStat.m_Passion = 100;
                    }
                    StartCoroutine(FadeOutText(StudentPosition[index], 2, true, randomStat.ToString(), randomStat2.ToString()));
                    break;
            }
        }
        else if (StudentEvents[index].GetComponent<ClickEventData>().eventType == ClickEventType.Object)
        {
            int randomIncome;
            switch (Students[index].NowRoom)
            {
                case (int)InteractionManager.SpotName.Pot:
                    randomIncome = Random.Range(50, 101);
                    PlayerInfo.Instance.m_MyMoney += randomIncome;
                    MonthlyReporter.Instance.m_NowMonth.IncomeEventResult += randomIncome;
                    StartCoroutine(MoneyFadeOutText(StudentPosition[index], true, randomIncome.ToString()));
                    Sound.PlayMoneySound();
                    break;

                case (int)InteractionManager.SpotName.AmusementMachine:
                    randomIncome = Random.Range(1500, 2001);
                    PlayerInfo.Instance.m_MyMoney += randomIncome;
                    MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomIncome;
                    StartCoroutine(MoneyFadeOutText(StudentPosition[index], true, randomIncome.ToString()));
                    Sound.PlayMoneySound();
                    break;
            }
        }

        Sound.PlayTouchEvent();

        GameObject newParticle = Instantiate(TouchEventPrefab);
        newParticle.transform.parent = ClickEventPanel.transform;
        newParticle.transform.position = Camera.main.WorldToScreenPoint(StudentPosition[index]);
        newParticle.GetComponent<ParticleImage>().Play();
        StartCoroutine(DeleteParticle(newParticle));

        StopCoroutine(DeleteStudentSpecialEvent(index));
        Students[index] = null;
        StudentPosition[index] = Vector3.zero;
    }

    IEnumerator DeleteParticle(GameObject clickEvent)
    {
        yield return new WaitForSeconds(3.0f);

        Destroy(clickEvent);
    }

    private void ClickedInstructorEvent()
    {
        int index = InstructorEvents.IndexOf(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        InstructorEvents[index].SetActive(false);

        if (InstructorEvents[index].GetComponent<ClickEventData>().eventType == ClickEventType.Facility)
        {
            int randomReward = Random.Range(1, 101);
            int randomStat = 0;
            int randomStat2 = 0;

            switch (Instructors[index].NowRoom)
            {
                case (int)InteractionManager.SpotName.Store:
                    if (randomReward <= 70)
                    {
                        randomStat = Random.Range(390, 5201);
                        PlayerInfo.Instance.m_MyMoney += randomStat;
                        MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomStat;
                        StartCoroutine(MoneyFadeOutText(InstructorPosition[index], true, randomStat.ToString()));
                        Sound.PlayMoneySound();
                    }
                    else
                    {
                        randomStat = Random.Range(26, 170);
                        PlayerInfo.Instance.m_SpecialPoint += randomStat;
                        StartCoroutine(SPFadeOutText(InstructorPosition[index], true, randomStat.ToString()));
                        Sound.PlaySPSound();
                    }
                    break;

                case (int)InteractionManager.SpotName.BookStore:
                    if (randomReward <= 70)
                    {
                        randomStat = Random.Range(390, 5201);
                        PlayerInfo.Instance.m_MyMoney += randomStat;
                        MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomStat;
                        StartCoroutine(MoneyFadeOutText(InstructorPosition[index], true, randomStat.ToString()));
                        Sound.PlayMoneySound();
                    }
                    else
                    {
                        randomStat = Random.Range(26, 170);
                        PlayerInfo.Instance.m_SpecialPoint += randomStat;
                        StartCoroutine(SPFadeOutText(InstructorPosition[index], true, randomStat.ToString()));
                        Sound.PlaySPSound();
                    }
                    break;

                case (int)InteractionManager.SpotName.Lounge1:
                    randomStat = Random.Range(30, 61);
                    Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth += randomStat;
                    if (Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth >= 100)
                    {
                        Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth = 100;
                    }
                    randomStat2 = Random.Range(30, 61);
                    Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomStat;
                    if (Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion >= 100)
                    {
                        Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion = 100;
                    }
                    StartCoroutine(FadeOutText(InstructorPosition[index], 2, true, randomStat.ToString(), randomStat2.ToString()));
                    break;

                case (int)InteractionManager.SpotName.Lounge2:
                    randomStat = Random.Range(30, 61);
                    Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth += randomStat;
                    if (Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth >= 100)
                    {
                        Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth = 100;
                    }
                    randomStat2 = Random.Range(30, 61);
                    Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomStat;
                    if (Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion >= 100)
                    {
                        Instructors[index].GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion = 100;
                    }
                    StartCoroutine(FadeOutText(InstructorPosition[index], 2, true, randomStat.ToString(), randomStat2.ToString()));
                    break;
            }
        }
        else if (InstructorEvents[index].GetComponent<ClickEventData>().eventType == ClickEventType.Object)
        {
            int randomIncome;
            switch (Instructors[index].NowRoom)
            {
                case (int)InteractionManager.SpotName.Pot:
                    randomIncome = Random.Range(50, 101);
                    PlayerInfo.Instance.m_MyMoney += randomIncome;
                    MonthlyReporter.Instance.m_NowMonth.IncomeEventResult += randomIncome;
                    StartCoroutine(MoneyFadeOutText(InstructorPosition[index], true, randomIncome.ToString()));
                    Sound.PlayMoneySound();
                    break;

                case (int)InteractionManager.SpotName.AmusementMachine:
                    randomIncome = Random.Range(1500, 2001);
                    PlayerInfo.Instance.m_MyMoney += randomIncome;
                    MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomIncome;
                    StartCoroutine(MoneyFadeOutText(InstructorPosition[index], true, randomIncome.ToString()));
                    Sound.PlayMoneySound();
                    break;
            }
        }

        Sound.PlayTouchEvent();

        GameObject newParticle = Instantiate(TouchEventPrefab);
        newParticle.transform.parent = ClickEventPanel.transform;
        newParticle.transform.position = Camera.main.WorldToScreenPoint(InstructorPosition[index]);
        newParticle.GetComponent<ParticleImage>().Play();
        StartCoroutine(DeleteParticle(newParticle));

        StopCoroutine(DeleteInstructorSpecialEvent(index));
        Instructors[index] = null;
        InstructorPosition[index] = Vector3.zero;
    }
}
