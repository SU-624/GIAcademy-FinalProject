using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingAlramPanel : MonoBehaviour
{
    [SerializeField] private Image m_FadeInOutImage;
    [SerializeField] private TextMeshProUGUI m_AlramText;
    [SerializeField] private Button m_ChangeEndingScene;
    [SerializeField] private PopUpUI m_PopUpEndingAlramPanel;
    [SerializeField] private PopOffUI m_PopoffEndingAlramPanel;
    [SerializeField] private PopUpUI m_PopUpEndingPanel;
    [SerializeField] private GameObject m_EndingPanelScript;

    public TextMeshProUGUI AlramText { get { return m_AlramText; } set { m_AlramText = value; } }

    /// fade 효과를 위한 변수들
    private Color _fadeOutImageColor;
    private Color _fadeInImageColor;
    private float m_FadeTime = 5f;

    public void Start()
    {
        m_ChangeEndingScene.onClick.AddListener(ClickEndingSceneButton);
        InitEndingSceneFadeImage();
    }

    public void InitEndingSceneFadeImage()
    {
        _fadeOutImageColor = m_FadeInOutImage.color;
        _fadeOutImageColor.a = 1f;
    }

    IEnumerator PopUpEndinganel()
    {
        m_FadeInOutImage.color = _fadeOutImageColor;

        yield return StartCoroutine(FadeOutPanel());

        m_PopUpEndingPanel.TurnOnUI();

        yield return StartCoroutine(FadeInPanel());
    }

    IEnumerator FadeOutPanel()
    {
        while (m_FadeInOutImage.color.a > 0f)
        {
            m_FadeInOutImage.color = new Color(m_FadeInOutImage.color.r, m_FadeInOutImage.color.g, m_FadeInOutImage.color.b, m_FadeInOutImage.color.a - (Time.unscaledDeltaTime / m_FadeTime));
            yield return null;
        }
    }

    IEnumerator FadeInPanel()
    {
        yield return new WaitForSecondsRealtime(7f);

        while (1f > m_FadeInOutImage.color.a)
        {
            m_FadeInOutImage.color = new Color(m_FadeInOutImage.color.r, m_FadeInOutImage.color.g, m_FadeInOutImage.color.b, m_FadeInOutImage.color.a + (Time.unscaledDeltaTime / m_FadeTime));
            yield return null;
        }

        yield return new WaitForSecondsRealtime(2f);

        _fadeInImageColor.a = 0f;
        m_FadeInOutImage.color = _fadeInImageColor;
        m_EndingPanelScript.GetComponent<EndingPanel>().enabled = true;

        InitEndingSceneFadeImage();

        m_PopoffEndingAlramPanel.TurnOffUI();
    }

    public void SetEndingAlramPanel(bool _flag)
    {
        if (_flag)
        {
            m_PopUpEndingAlramPanel.TurnOnUI();
        }
        else
        {
            m_PopoffEndingAlramPanel.TurnOffUI();
        }
    }

    public void ClickEndingSceneButton()
    {
        StartCoroutine(PopUpEndinganel());
    }

}
