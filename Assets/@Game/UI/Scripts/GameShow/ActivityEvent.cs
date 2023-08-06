using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivityEvent : MonoBehaviour
{
    [SerializeField] private EventPanel m_EventPanel;
    [SerializeField] private GameObject m_EventPrefab;
    [SerializeField] private TextMeshProUGUI m_PDText;
    [SerializeField] private PopUpUI m_PopUpGameJamMiniGamePanel;
    [SerializeField] private PopUpUI m_PopUpPDPanel;
    [SerializeField] private PopOffUI m_PopOffPDPanel;

    private float m_GameJamTimer;
    private int m_NowDay;
    private Queue<string> m_PDScript = new Queue<string>();
    //private bool m_IsTypingAnmation;
    public bool m_IsCheckGameShow;
    public bool m_IsCheckGameJam;
    public bool m_IsMiniGameStart;

    private void Start()
    {
        m_NowDay = 0;
        m_GameJamTimer = 0;
        //m_IsTypingAnmation = false;
        m_IsCheckGameShow = false;
        m_IsCheckGameJam = true;
        m_IsMiniGameStart = false;
    }

    void Update()
    {
        if (m_NowDay != GameTime.Instance.FlowTime.NowDay)
        {
            if (m_EventPanel.gameObject.activeSelf)
            {
                for (int i = 0; i < m_EventPanel.EventTransform.childCount; i++)
                {
                    string DdayText = m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().DDAyText.text;
                    int Dday = 0;

                    if (!DdayText.Contains("Day"))
                    {
                        Dday = int.Parse(DdayText[2..]) - 1;
                    }

                    if (m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().m_IsGameJam && 
                        !m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().m_IsGameJamMiniGameStart && Dday == 1 && Time.timeScale != 0)
                    {
                        m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().m_IsGameJamMiniGameStart = true;
                        StartGameJamMiniGame();
                    }

                    if (Dday == 0)
                    {
                        m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().DDAyText.text = "D-Day";
                    }
                    else
                    {
                        m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().DDAyText.text = "D-" + Dday.ToString();
                    }

                    if (Dday <= 0)
                    {
                        Dday = 0;

                        if (m_IsCheckGameShow == true && !m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().m_IsGameJam &&
                            m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().m_Month == GameTime.Instance.FlowTime.NowMonth)
                        {
                            m_IsCheckGameShow = false;
                        }
                        else if (m_IsCheckGameJam == true && m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().m_IsGameJam &&
                            m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().m_Month == GameTime.Instance.FlowTime.NowMonth)
                        {
                            m_IsCheckGameJam = false;
                        }

                        m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().m_IsTyping = false;
                    }
                }

                if (m_EventPanel.EventTransform.childCount == 0)
                {
                    m_EventPanel.gameObject.SetActive(false);
                }
            }

            m_NowDay = GameTime.Instance.FlowTime.NowDay;
        }
    }

    public GameObject MakeEventPrefab(string _gameShowName, int _gameShowMonth, int _gameShowWeek, int _gameShowDay, bool _isGameJam, int _month)
    {
        GameObject _eventPrefab = Instantiate(m_EventPrefab, m_EventPanel.EventTransform);

        _eventPrefab.name = _gameShowName;

        // 합산된 D-Day 값
        int totalDDay = ((GameTime.Instance.FlowTime.NowYear - 1) * 12 * 4 * 5) + ((_gameShowMonth - 1) * 4 * 5) + ((_gameShowWeek - 1) * 5) + _gameShowDay;
        int today = (GameTime.Instance.FlowTime.NowYear - 1) * 12 * 4 * 5 + ((GameTime.Instance.FlowTime.NowMonth - 1) * 4 * 5) +
            ((GameTime.Instance.FlowTime.NowWeek - 1) * 5) + GameTime.Instance.FlowTime.NowDay;

        totalDDay -= today;
        _eventPrefab.GetComponent<EventPrefab>().m_IsGameJam = _isGameJam;
        _eventPrefab.GetComponent<EventPrefab>().m_Month = _month;
        _eventPrefab.GetComponent<EventPrefab>().DDAyText.text = "D-" + totalDDay.ToString();
        m_IsCheckGameJam = true;

        return _eventPrefab;
    }

    public GameObject MakeDistributeEventPrefab(string _gameShowName, int _gameShowMonth, int _gameShowWeek, int _gameShowDay, bool _isGameJam, int _month)
    {
        GameObject _eventPrefab = Instantiate(m_EventPrefab, m_EventPanel.EventTransform);

        _eventPrefab.name = _gameShowName;

        // 합산된 D-Day 값
        int totalDDay = ((AllInOneData.Instance.PlayerData.Year - 1) * 12 * 4 * 5) + ((_gameShowMonth - 1) * 4 * 5) + ((_gameShowWeek - 1) * 5) + _gameShowDay;
        int today = (AllInOneData.Instance.PlayerData.Year - 1) * 12 * 4 * 5 + ((AllInOneData.Instance.PlayerData.Month - 1) * 4 * 5) +
                    ((AllInOneData.Instance.PlayerData.Week - 1) * 5) + AllInOneData.Instance.PlayerData.Day;

        totalDDay -= today;
        _eventPrefab.GetComponent<EventPrefab>().m_IsGameJam = _isGameJam;
        _eventPrefab.GetComponent<EventPrefab>().m_Month = _month;
        _eventPrefab.GetComponent<EventPrefab>().DDAyText.text = "D-" + totalDDay.ToString();
        m_IsCheckGameJam = true;
        return _eventPrefab;
    }

    public void SetEventPanelActive(bool _flag)
    {
        m_EventPanel.gameObject.SetActive(_flag);
        //m_IsTypingAnmation = _flag;
    }

    public void ClickEventButton()
    {
        if (m_EventPanel.ScorllViewObj.activeSelf)
        {
            m_EventPanel.ScorllViewObj.SetActive(false);
        }
        else
        {
            m_EventPanel.ScorllViewObj.SetActive(true);
        }
    }

    public IEnumerator TypingText(GameObject _eventPrefab, string _showOrJam)
    {
        string _originalText;
        string _subString = "";
        _eventPrefab.GetComponent<EventPrefab>().m_IsTyping = true;
        _originalText = "...";

        while (_eventPrefab.GetComponent<EventPrefab>().m_IsTyping)
        {
            for (int i = 0; i < _originalText.Length; i++)
            {
                _subString += _originalText.Substring(0, i);

                for (int j = 0; j < m_EventPanel.EventTransform.childCount; j++)
                {
                    _eventPrefab.GetComponent<EventPrefab>().ActivityText.text = _showOrJam + _subString;
                    _subString = "";
                }

                yield return new WaitForSeconds(1.5f);
            }
        }

        Destroy(_eventPrefab, 0.5f);
    }

    private void StartGameJamMiniGame()
    {
        InitScript();

        m_PDText.text = m_PDScript.Dequeue();

        m_PopUpPDPanel.TurnOnUI();
    }

    public void PDScript()
    {
        if (m_PDScript.Count > 0)
        {
            m_PDText.text = m_PDScript.Dequeue();
        }
        else
        {
            m_PopOffPDPanel.TurnOffUI();

            m_PopUpGameJamMiniGamePanel.TurnOnUI();
        }
    }

    private void InitScript()
    {
        m_PDScript.Clear();

        if (PlayerInfo.Instance.IsGameJamMiniGameFirst == true)
        {
            m_PDScript.Enqueue("학생들이 참가한 게임잼이 끝나가고있어요.");
            m_PDScript.Enqueue("원장님께서 서포트 해주시는건 어떨까요?");
            m_PDScript.Enqueue("학생들의 게임 개발을 도우러 가봅시다!");
        }
        else
        {
            m_PDScript.Enqueue("학생들이 참가한 게임잼이 곧 마무리됩니다.");
            m_PDScript.Enqueue("학생들의 게임 개발을 도우기위해 이동합니다.");
        }
    }
}
