using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingAlramPanel : MonoBehaviour
{
    [SerializeField] private EndingPanel m_EndingPanel;
    [SerializeField] private Button m_EndingButton;
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
        _fadeOutImageColor = Color.black;
        _fadeInImageColor = Color.black;

        _fadeOutImageColor.a = 0f;
        _fadeInImageColor.a = 1f;

        m_ChangeEndingScene.onClick.AddListener(ClickEndingSceneButton);
    }

    public IEnumerator PopUpEndinganel()
    {
        m_FadeInOutImage.color = _fadeOutImageColor;

        yield return StartCoroutine(FadeInPanel());

        m_FadeInOutImage.color = _fadeInImageColor;

        yield return StartCoroutine(FadeOutPanel());
    }

    IEnumerator FadeOutPanel()
    {
        m_PopUpEndingPanel.TurnOnUI();

        while (m_FadeInOutImage.color.a > 0f)
        {
            m_FadeInOutImage.color = new Color(m_FadeInOutImage.color.r, m_FadeInOutImage.color.g, m_FadeInOutImage.color.b, m_FadeInOutImage.color.a - (Time.unscaledDeltaTime / m_FadeTime));
            yield return null;
        }
        m_EndingPanelScript.GetComponent<EndingPanel>().enabled = true;
        m_EndingPanel.SetScript();

        m_PopoffEndingAlramPanel.TurnOffUI();
    }

    IEnumerator FadeInPanel()
    {
        while (1f > m_FadeInOutImage.color.a)
        {
            m_FadeInOutImage.color = new Color(m_FadeInOutImage.color.r, m_FadeInOutImage.color.g, m_FadeInOutImage.color.b, m_FadeInOutImage.color.a + (Time.unscaledDeltaTime / m_FadeTime));
            yield return null;
        }

        yield return new WaitForSecondsRealtime(2f);
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
        m_EndingButton.interactable = false;
    }

}
