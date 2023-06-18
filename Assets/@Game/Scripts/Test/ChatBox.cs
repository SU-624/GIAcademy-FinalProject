using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ChatBox : MonoBehaviour
{
    private static ChatBox _instance = null;

    // ������ ��� json���ϵ��� ���� ����Ʈ
    public List<string> m_DialogueLines = new List<string>();

    // ����� ��縦 ���� ť
    public ConcurrentQueue<string> m_Dialogue = new ConcurrentQueue<string>();

    object obj = new object();

    public static ChatBox Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        //m_DialogueLines.Add("�ȳ��ϼ���/�ݰ����ϴ�/���������׿�");
        m_DialogueLines.Add("������ ������ ���γ׿�/���� ���� ���߰ھ��");
        //m_DialogueLines.Add("������ �Ծ�����?");
        m_DialogueLines.Add("���� ����̰� ¯�̾�!/�ƴ� �������� ¯����!");
    }

    /// <summary>
    /// BT �ʿ��� ��ȭ�� ���� ����ְ� �س��� �Ⱦ��� �� �κ�
    /// by.�翵
    /// </summary>
    // ��ȭ�� ���۵Ǹ� ����ִ� ��縦 Ư�� ��ȣ�� �Ǻ��ؼ� �߶� ť�� �־��ֱ�.
    public void OnDialogue()
    {
        lock (obj)
        {
            int _tempnum = Random.Range(0, m_DialogueLines.Count);

            m_Dialogue.Clear();

            string[] m_Temp = m_DialogueLines[_tempnum].Split('/');

            foreach (var line in m_Temp)
            {
                m_Dialogue.Enqueue(line);
            }

        }
    }

}
