using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingEnding : MonoBehaviour
{
    [SerializeField] private Image m_FadeInOutImage;
    [SerializeField] private PopUpUI m_PopUpEndingPanel;
    [SerializeField] private EndingPanel m_EndingPanel;
    [SerializeField] private GameObject m_EndingPanelScript;
    [SerializeField] private Animator m_EndingCredit;
    [SerializeField] private TextMeshProUGUI m_EndingScript;
    [SerializeField] private Image m_ScreenTouchAble;
    [SerializeField] private PopOffUI m_PopOffEndingPanel;

    /// fade 효과를 위한 변수들
    private Color _fadeOutImageColor;
    private Color _fadeInImageColor;
    private float m_FadeTime = 2f;
    private float m_Timer;

    public void Start()
    {
        _fadeOutImageColor = Color.black;
        _fadeInImageColor = Color.black;
        m_Timer = 0;
        _fadeOutImageColor.a = 0f;
        _fadeInImageColor.a = 1f;
    }

    public IEnumerator PopUpEndinganel()
    {
        m_FadeInOutImage.color = _fadeOutImageColor;

        yield return StartCoroutine(FadeInPanel());

        m_FadeInOutImage.color = _fadeInImageColor;

        yield return StartCoroutine(FadeOutPanel());

        m_EndingCredit.Play("EndingCredits", 0, 0f);

        yield return StartCoroutine(EndingAnim());
    }

    IEnumerator FadeInPanel()
    {
        while (1f > m_FadeInOutImage.color.a)
        {
            m_FadeInOutImage.color = new Color(m_FadeInOutImage.color.r, m_FadeInOutImage.color.g, m_FadeInOutImage.color.b, m_FadeInOutImage.color.a + (Time.unscaledDeltaTime / m_FadeTime));
            yield return null;
        }

        yield return new WaitForSecondsRealtime(2f);

        m_EndingPanel.SetScript();
    }

    IEnumerator FadeOutPanel()
    {
        m_PopUpEndingPanel.TurnOnUI();

        while (m_FadeInOutImage.color.a > 0f)
        {
            m_FadeInOutImage.color = new Color(m_FadeInOutImage.color.r, m_FadeInOutImage.color.g, m_FadeInOutImage.color.b, m_FadeInOutImage.color.a - (Time.unscaledDeltaTime / m_FadeTime));
            yield return null;
        }
    }

    public void SetScript()
    {
        string _script = m_EndingScript.text;
        m_EndingScript.text = _script.Replace("[유저지정]", PlayerInfo.Instance.AcademyName);
    }

    IEnumerator EndingAnim()
    {
        yield return new WaitUntil(() =>
        {
            m_Timer += Time.unscaledDeltaTime;

            if (m_Timer > 20f)
            {
                StartCoroutine(PopOffEndingPanel());
                m_ScreenTouchAble.gameObject.SetActive(true);
                m_Timer = 0;
                return true;
            }
            else
            {
                return false;
            }
        });
    }

    IEnumerator PopOffEndingPanel()
    {
        yield return new WaitUntil(() =>
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                m_ScreenTouchAble.gameObject.SetActive(false);
                m_PopOffEndingPanel.TurnOffUI();
                return true;
            }
            else
            {
                return false;
            }
        });
    }
}
