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
        m_ScriptListIndex = m_Tips.Count;
        m_IsTitle = true;
        m_Tips.Add("<line-height=120%>������ �帣�� ���ʽ� ����\n<size=80%>�����뿡�� �䱸�ϴ� �帣�� ���� ������ ������ ���, ���ʽ� ������ ȹ���� �� �ֽ��ϴ�.</size>");
        m_Tips.Add("<line-height=120%>�������� ��ȣ �帣\n<size=80%>�����븶�� ��ȣ�ϴ� ������ �帣�� �����մϴ�.</size>");
        m_Tips.Add("<line-height=120%>���Ӽ� �ɻ�����\n<size=80%>���Ӽ� �ɻ������� �����ܺ�����. ��ȣ�ϴ� �帣�� �������ֽ��ϴ�.</size>");
        m_Tips.Add("<line-height=120%>�л��� ����\n<size=80%>�л����� ������ ������ ������ ���ϰ��ֽ��ϴ�.</size>");
    }    

    public void MoveToInGameScene()
    {
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

                int _index = Random.Range(0, m_ScriptListIndex);

                m_TipScript.text = m_Tips[_index];

                m_ScriptListIndex -= 1;
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
