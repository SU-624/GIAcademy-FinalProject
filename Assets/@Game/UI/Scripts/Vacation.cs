using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Vacation : MonoBehaviour
{
    [Header("방학 스크립트 진행용")] 
    [SerializeField] private Button m_ContinueButton;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_PDAlarmText;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private PopOffUI m_PopOffVacation;

    private int m_ScriptCount;
    private int m_ScriptMaxCount;
    private List<string> m_VacationScripts = new List<string>();
    private bool m_ScriptSetting;
    private Tutorial m_Tutorial;
    private float m_LastTime;
    private bool m_IsVacationStart;

    // Start is called before the first frame update
    void Start()
    {
        m_ContinueButton.onClick.AddListener(VacationContinue);
        m_ScriptCount = 0;
        m_ScriptSetting = false;
        m_Tutorial = GetComponent<Tutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsVacationStart)
        {
            if (PlayerInfo.Instance.IsFirstVacation && !m_ScriptSetting)
            {
                m_VacationScripts = m_Tutorial.VacationTutorial;
            }
            else if (!PlayerInfo.Instance.IsFirstVacation && !m_ScriptSetting)
            {
                int randomScript = Random.Range(0, 3);
                m_VacationScripts = m_Tutorial.VacationNormal[randomScript];
            }

            m_ScriptSetting = true; 
            int splitIndex = m_VacationScripts[m_ScriptCount].IndexOf("]");
            if (splitIndex != -1)
            {
                m_PDAlarmText.text = PlayerInfo.Instance.PrincipalName + m_VacationScripts[m_ScriptCount].Substring(splitIndex + 1);
            }
            else
            {
                m_PDAlarmText.text = m_VacationScripts[m_ScriptCount];
            }
            m_ScriptMaxCount = m_VacationScripts.Count;
            m_IsVacationStart = true;
        }

        if (GameTime.Instance.Month == 3)
        {
            m_IsVacationStart = false;
        }
    }

    /// <summary>
    /// 방학 스크립트 진행 함수입니다. 
    /// </summary>
    void VacationContinue()
    {
        m_ScriptCount++;
        if (m_ScriptCount < m_ScriptMaxCount)
        {
            int splitIndex1 = m_VacationScripts[m_ScriptCount].IndexOf("[");
            int splitIndex2 = m_VacationScripts[m_ScriptCount].IndexOf("]");
            int splitText = m_VacationScripts[m_ScriptCount].IndexOf("아카데미명");
            if (splitIndex1 != -1 && splitText != -1)
            {
                char lastText = PlayerInfo.Instance.AcademyName.ElementAt(PlayerInfo.Instance.AcademyName.Length - 1);

                string selectText = (lastText - 0xAC00) % 28 > 0 ? "은" : "는";
                m_PDAlarmText.text = PlayerInfo.Instance.AcademyName + selectText + m_VacationScripts[m_ScriptCount].Substring(splitIndex2 + 2);
            }
            else if (splitIndex1 != -1 && splitText == -1)
            {
                m_PDAlarmText.text = m_VacationScripts[m_ScriptCount].Substring(0, splitIndex1) + PlayerInfo.Instance.PrincipalName + m_VacationScripts[m_ScriptCount].Substring(splitIndex2 + 1);
            }
            else
            {
                m_PDAlarmText.text = m_VacationScripts[m_ScriptCount];
            }
            ClickEventManager.Instance.Sound.PlayIconTouch();
        }
        else if(m_ScriptCount == m_ScriptMaxCount)
        {
            StartCoroutine(FadeOut());
            m_ScriptCount++;
            ClickEventManager.Instance.Sound.PlayIconTouch();
        }
    }

    IEnumerator FadeOut()
    {
        m_PDAlarm.SetActive(false);
        Image blackScreen = m_BlackScreen.GetComponent<Image>();
        m_LastTime = Time.realtimeSinceStartup;
        while (blackScreen.color.a <= 1f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_LastTime;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a + (deltaTime / 2f));
            m_LastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        Time.timeScale = 3;

        yield return new WaitUntil(() => ObjectManager.Instance.m_StudentList.Count == 0);

        Time.timeScale = 0;
        GameTime.Instance.FlowTime.NowWeek = 4;
        GameTime.Instance.FlowTime.NowDay = 4;
        GameTime.Instance.Week = 4;
        GameTime.Instance.Day = 4;
        GameTime.Instance.TimeBarImg.fillAmount = 0.8f;

        m_LastTime = Time.realtimeSinceStartup;
        while (blackScreen.color.a >= 0f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_LastTime;
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a - (deltaTime / 2f));
            m_LastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        //if (PlayerInfo.Instance.IsFirstVacation)
        //{
        //    PlayerInfo.Instance.IsFirstVacation = false;
        //}

        GameTime.Instance.m_IsVacationNotice = false;
        m_ScriptSetting = false;
        m_ScriptCount = 0;
        m_ScriptMaxCount = 0;
        m_VacationScripts = null;
        m_PopOffVacation.TurnOffUI();
    }
}
