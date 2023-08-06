using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// 2022. 11. 22 Mang
/// 2023.05.25 Ocean ����� �ε��� Ŭ������ �̰����� �߰�!
/// 
/// ���ӿ��� �� �̵��� ����� Ŭ����
/// </summary>
public class MoveSceneManager : MonoBehaviour
{
    public static MoveSceneManager m_Instance = null;       // Manager ������ �̱������� ���

    [SerializeField] private Slider LoadingBar;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI m_TipScript;
    public GameObject TitleCamera;
    public GameObject LoadingPanel;
    private List<string> m_Tips = new List<string>();
    private int m_ScriptListIndex;
    private string loadSceneName;
    private bool m_IsTitle;

    public bool UseLoadingData = false; // �ҷ����� ����� ����� ������ üũ

    // �̱������� ���� ������ �����ϰ� �����ϰ� ����ϱ� ���� �ʱ�ȭ(?) ���
    private void Awake()
    {
        // Instance �� �����Ѵٸ� gameObject ����
        if (m_Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        // ���۵� �� �ν��Ͻ� �ʱ�ȭ, ���� �Ѿ���� �����Ǳ� ���� ó��
        m_Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        m_ScriptListIndex = 0;
        InitTipScriptList();
        m_ScriptListIndex = m_Tips.Count;

        ChangeTipScript();

        m_IsTitle = true;
    }

    private void InitTipScriptList()
    {
        m_Tips.Add("<line-height=120%>������ �帣�� ���ʽ� ����\n<size=80%>�����뿡�� �䱸�ϴ� �帣�� ���� ������ ������ ���, ���ʽ� ������ ȹ���� �� �ֽ��ϴ�.</size>");
        m_Tips.Add("<line-height=120%>�������� ��ȣ �帣\n<size=80%>�����븶�� ��ȣ�ϴ� ������ �帣�� �����մϴ�.</size>");
        m_Tips.Add("<line-height=120%>���Ӽ� �ɻ�����\n<size=80%>���Ӽ� �ɻ������� �����ܺ�����. ��ȣ�ϴ� �帣�� �������ֽ��ϴ�.</size>");
        m_Tips.Add("<line-height=120%>�л��� ����\n<size=80%>�л����� ������ ������ ������ ���ϰ��ֽ��ϴ�.</size>");

        m_Tips.Add("<line-height=120%>������õ\n<size=80%>2���޿� �л����� ȸ�翡 ��õ�Ͽ� �����ų �� �ֽ��ϴ�.</size>");
        m_Tips.Add("<line-height=120%>������õ\n<size=80%>Ư�� ȸ��� Ư���� �ɷ��� ���� �л��� ���մϴ�.</size>");
        m_Tips.Add("<line-height=120%>�л� �ɷ�\n<size=80%>�л��� Ư�� �ɷ��� �������̳� ���Ӽ� ���� ���� ���� �� �ֽ��ϴ�.</size>");
        m_Tips.Add("<line-height=120%>�����\n<size=80%>������� ������ 20221004�� ȸ���ϰ� �ֽ��ϴ�.</size>");
        m_Tips.Add("<line-height=120%>�׼ǹ�\n<size=80%>�׼ǹ��� ����� 1000��° �̷����� �ֽ��ϴ�.</size>");
        m_Tips.Add("<line-height=120%>�ùķ��̼ǹ�\n<size=80%>��� ��ħ�� �︮�� �ٶ��Ҹ��� �鸮�� �ʳ���?</size>");
        m_Tips.Add("<line-height=120%>��庥�Ĺ�\n<size=80%>������ ������ 150�Ⱓ ���Ⱑ �Ǿ������ �ֽ��ϴ�.</size>");
    }

    public void UseLoadingDataBtn()
    {
        if (!UseLoadingData)
            UseLoadingData = true;
        else if (UseLoadingData)
            UseLoadingData = false;
    }

    public void MoveToInGameScene()
    {
        if (UseLoadingData)
        {
            Json.Instance.UseLoadingData = true;
            Json.Instance.PressLadingBtn();
        }

        Debug.Log("��ư������ �� ��ȯ~");
        PopUpUI pop = new PopUpUI();

        pop.PopUpUIOnTitleScene(LoadingPanel);

        TitleCamera.SetActive(false);
        LoadScene("InGameScene");
        //SceneManager.LoadScene("InGameScene");
    }

    public void MoveToTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        //SceneManager.sceneLoaded += LoadSceneEnd;
        loadSceneName = sceneName;
        StartCoroutine(Load(sceneName));
        //StartCoroutine(LoadingSceneScript());
    }

    private void ChangeTipScript()
    {
        int _index = Random.Range(0, m_ScriptListIndex);

        m_TipScript.text = m_Tips[_index];
        m_ScriptListIndex -= 1;
    }

    private IEnumerator LoadingSceneScript()
    {
        while (m_IsTitle)
        {
            yield return null;

        }
    }

    private IEnumerator Load(string sceneName)
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                if (m_ScriptListIndex == 0)
                {
                    m_ScriptListIndex = m_Tips.Count;
                }

                ChangeTipScript();
            }

            //int _index = Random.Range(0, m_Tips.Count);
            //m_TipScript.text = m_Tips[_index];

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    m_IsTitle = false;
                    yield break;
                }
            }
        }
    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= LoadSceneEnd;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 2f;
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
