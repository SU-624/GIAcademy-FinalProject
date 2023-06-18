using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Narrative : MonoBehaviour
{
    public List<Image> NarrartiveImg;
    public TextMeshProUGUI NarrativeText;

    private int m_tempNarrativeCount;
    private int m_tempScriptCount;
    private bool m_isScriptClear;
    private bool m_isImgClear;
    private bool m_isAllScriptClear;
    private bool m_isAllImgClear;

    private bool m_isClicked;

    private float m_lastTime;
    private List<string> m_scripts = new List<string>();
    private List<int> m_scriptsLength = new List<int>();

    void Awake()
    {
        m_tempNarrativeCount = 0;
        m_tempScriptCount = 0;
        m_scripts.Add("�����Ҿƹ��������� ��ϴ� ����� ������ GI��ī���̴� ���� ž���� ����� �������縦 �缺�ϴ� ���̾���.");
        m_scripts.Add("������ �ô밡 �帣�� �ް��� ��ȭ�� ���ӻ���� �޼��忡 ������ ����� �� ������ �ٴ����� �߶��� ����!");
        m_scripts.Add("�ƹ�������� ���� ���ߴ� 1�� Żȯ�� ��, ���� ���� �� ���� �̷Ｍ ������ ������ ��ã�ھ�!");
        m_scripts.Add("�� ���� �����̾�~ ������~");
        m_scriptsLength.Add(m_scripts[0].Length);
        m_scriptsLength.Add(m_scripts[1].Length);
        m_scriptsLength.Add(m_scripts[2].Length);
        m_scriptsLength.Add(m_scripts[3].Length);

        m_lastTime = Time.realtimeSinceStartup;
        NarrativeText.text = "";
        //StartCoroutine(NarrativeContinue());
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_isImgClear && m_isScriptClear)
            {
                m_isClicked = true;
                m_isImgClear = false;
                m_isScriptClear = false;
            }
        }

        if (Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (m_isImgClear && m_isScriptClear)
                {
                    m_isClicked = true;
                    m_isImgClear = false;
                    m_isScriptClear = false;
                }
            }
        }

        if (m_isAllImgClear && m_isAllScriptClear)
        {
            this.gameObject.SetActive(false);
        }
    }

    public IEnumerator NarrativeImgChange()
    {
        Time.timeScale = 0;
        NarrartiveImg[0].gameObject.SetActive(true);
        m_lastTime = Time.realtimeSinceStartup;
        while (NarrartiveImg[0].color.a < 1f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_lastTime;
            NarrartiveImg[0].color = new Color(NarrartiveImg[0].color.r, NarrartiveImg[0].color.g, NarrartiveImg[0].color.b, NarrartiveImg[0].color.a + (deltaTime / 0.5f));
            m_lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        m_isImgClear = true;

        yield return new WaitUntil(() => m_isClicked);

        m_lastTime = Time.realtimeSinceStartup;
        m_isImgClear = false;
        while (NarrartiveImg[0].color.a > 0f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_lastTime;
            NarrartiveImg[0].color = new Color(NarrartiveImg[0].color.r, NarrartiveImg[0].color.g, NarrartiveImg[0].color.b, NarrartiveImg[0].color.a - (deltaTime / 0.5f));
            m_lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        m_isClicked = false;
        NarrartiveImg[0].gameObject.SetActive(false);
        m_tempNarrativeCount = 1;
        NarrartiveImg[1].gameObject.SetActive(true);

        while (NarrartiveImg[1].color.a < 1f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_lastTime;
            NarrartiveImg[1].color = new Color(NarrartiveImg[1].color.r, NarrartiveImg[1].color.g, NarrartiveImg[1].color.b, NarrartiveImg[1].color.a + (deltaTime / 0.5f));
            m_lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        m_isImgClear = true;

        yield return new WaitUntil(() => m_isClicked);

        m_lastTime = Time.realtimeSinceStartup;
        m_isImgClear = false;
        while (NarrartiveImg[1].color.a > 0f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_lastTime;
            NarrartiveImg[1].color = new Color(NarrartiveImg[1].color.r, NarrartiveImg[1].color.g, NarrartiveImg[1].color.b, NarrartiveImg[1].color.a - (deltaTime / 0.5f));
            m_lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        m_isClicked = false;
        NarrartiveImg[1].gameObject.SetActive(false);
        m_tempNarrativeCount = 2;
        NarrartiveImg[2].gameObject.SetActive(true);

        while (NarrartiveImg[2].color.a < 1f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_lastTime;
            NarrartiveImg[2].color = new Color(NarrartiveImg[2].color.r, NarrartiveImg[2].color.g, NarrartiveImg[2].color.b, NarrartiveImg[2].color.a + (deltaTime / 0.5f));
            m_lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        m_isImgClear = true;
        yield return new WaitUntil(() => m_isClicked);

        m_lastTime = Time.realtimeSinceStartup;
        m_isImgClear = false;
        while (NarrartiveImg[2].color.a > 0f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_lastTime;
            NarrartiveImg[2].color = new Color(NarrartiveImg[2].color.r, NarrartiveImg[2].color.g, NarrartiveImg[2].color.b, NarrartiveImg[2].color.a - (deltaTime / 0.5f));
            m_lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        m_isClicked = false;
        NarrartiveImg[2].gameObject.SetActive(false);
        m_tempNarrativeCount = 3;
        NarrartiveImg[3].gameObject.SetActive(true);

        while (NarrartiveImg[3].color.a < 1f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_lastTime;
            NarrartiveImg[3].color = new Color(NarrartiveImg[3].color.r, NarrartiveImg[3].color.g, NarrartiveImg[3].color.b, NarrartiveImg[3].color.a + (deltaTime / 0.5f));
            m_lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        m_isImgClear = true;
        yield return new WaitUntil(() => m_isClicked);

        m_lastTime = Time.realtimeSinceStartup;
        m_isImgClear = false;
        while (NarrartiveImg[3].color.a > 0f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_lastTime;
            NarrartiveImg[3].color = new Color(NarrartiveImg[3].color.r, NarrartiveImg[3].color.g, NarrartiveImg[3].color.b, NarrartiveImg[3].color.a - (deltaTime / 0.5f));
            m_lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        m_isAllImgClear = true;
    }

    public IEnumerator NarrativeTextChange()
    {
        while (NarrativeText.text.Length < m_scriptsLength[0])
        {
            NarrativeText.text += m_scripts[0][m_tempScriptCount];
            m_tempScriptCount++;
            yield return null;
        }

        m_isScriptClear = true;

        yield return new WaitUntil(() => m_tempNarrativeCount == 1);

        NarrativeText.text = "";
        m_tempScriptCount = 0;
        m_isScriptClear = false;
        while (NarrativeText.text.Length < m_scriptsLength[1])
        {
            NarrativeText.text += m_scripts[1][m_tempScriptCount];
            m_tempScriptCount++;
            yield return null;
        }
        m_isClicked = false;

        m_isScriptClear = true;

        yield return new WaitUntil(() => m_tempNarrativeCount == 2);
        
        NarrativeText.text = "";
        m_tempScriptCount = 0;
        m_isScriptClear = false;
        while (NarrativeText.text.Length < m_scriptsLength[2])
        {
            NarrativeText.text += m_scripts[2][m_tempScriptCount];
            m_tempScriptCount++;
            yield return null;
        }
        m_isClicked = false;

        m_isScriptClear = true;

        yield return new WaitUntil(() => m_tempNarrativeCount == 3);
        
        NarrativeText.text = "";
        m_tempScriptCount = 0;
        m_isScriptClear = false;
        while (NarrativeText.text.Length < m_scriptsLength[3])
        {
            NarrativeText.text += m_scripts[3][m_tempScriptCount];
            m_tempScriptCount++;
            yield return null;
        }
        m_isClicked = false;
        m_isScriptClear = true;

        m_isAllScriptClear = true;
    }

    public IEnumerator NarrativeContinue()
    {
        yield return new WaitUntil(() =>
        {
            if (Input.touchCount >= 1 || Input.GetMouseButtonDown(0))
            {
                if (m_isImgClear && m_isScriptClear)
                {
                    NarrartiveImg[m_tempNarrativeCount].color = new Color(NarrartiveImg[m_tempNarrativeCount].color.r, NarrartiveImg[m_tempNarrativeCount].color.g,
                        NarrartiveImg[m_tempNarrativeCount].color.b, 1);
                    NarrativeText.text = m_scripts[m_tempNarrativeCount];
                    StartCoroutine(NarrativeContinue());
                }
                return true;
            }
            else
            {
                return false;
            }

        });
    }
}
