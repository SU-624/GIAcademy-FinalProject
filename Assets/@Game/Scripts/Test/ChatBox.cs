using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ChatBox : MonoBehaviour
{
    private static ChatBox _instance = null;

    // 대사들이 담긴 json파일등을 담을 리스트
    public List<string> m_DialogueLines = new List<string>();

    // 출력할 대사를 담을 큐
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
        //m_DialogueLines.Add("안녕하세요/반갑습니다/날씨가좋네요");
        m_DialogueLines.Add("오늘은 날씨가 별로네요/빨리 집에 가야겠어요");
        //m_DialogueLines.Add("점심은 먹었나요?");
        m_DialogueLines.Add("역시 고양이가 짱이야!/아니 강아지가 짱이지!");
    }

    /// <summary>
    /// BT 쪽에서 대화를 직접 들고있게 해놔서 안쓰게 된 부분
    /// by.재영
    /// </summary>
    // 대화가 시작되면 담겨있는 대사를 특정 기호로 판별해서 잘라서 큐에 넣어주기.
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
