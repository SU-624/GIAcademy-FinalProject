using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingPanel : MonoBehaviour
{
    [SerializeField] private Animator m_EndingCredit;
    [SerializeField] private TextMeshProUGUI m_EndingScript;
    [SerializeField] private Button m_NextScriptButton;
    [SerializeField] private Image m_ScreenTouchAble;
    [SerializeField] private PopOffUI m_PopOffEndingPanel;

    private float m_WaittingTime;

    private void OnEnable()
    {
        StartCoroutine(EndingAnim());
        m_EndingCredit.Play("EndingCredits", 0, 0f);
    }

    private void OnDisable()
    {
        m_ScreenTouchAble.gameObject.SetActive(false);
    }

    private void Start()
    {
        m_WaittingTime = 0f;
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
            m_WaittingTime += Time.unscaledDeltaTime;

            if (m_WaittingTime > 20f)
            {
                StartCoroutine(PopOffEndingPanel());
                m_ScreenTouchAble.gameObject.SetActive(true);
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
                gameObject.GetComponent<EndingPanel>().enabled = false;
                m_PopOffEndingPanel.TurnOffUI();
                SceneManager.LoadScene("TitleScene");
                return true;
            }
            else
            {
                return false;
            }
        });
    }
}
