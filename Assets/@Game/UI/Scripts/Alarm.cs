using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : MonoBehaviour
{
    [SerializeField] private GameObject AlarmPrefab;
    private int m_textCount;
    private List<float> m_eraseTimer = new List<float>();
    private int m_activeCount;
    private List<GameObject> AlarmList = new List<GameObject>();
    private List<TextMeshProUGUI> TextList = new List<TextMeshProUGUI>();

    public Queue<string> AlarmMessageQ = new Queue<string>();

    // Start is called before the first frame update
    void Start()
    {
        m_textCount = 0;

        for (int i = 0; i < 10; i++)
        {
            GameObject newAlarm = Instantiate(AlarmPrefab, this.transform);
            newAlarm.transform.SetAsFirstSibling();
            AlarmList.Add(newAlarm);
            TextList.Add(newAlarm.transform.GetChild(1).GetComponent<TextMeshProUGUI>());
            m_eraseTimer.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AlarmMessageQ.Count != 0)
        {
            TextList[m_textCount].text = AlarmMessageQ.Dequeue();
            AddAlarm(m_textCount);
            StartCoroutine(TextErase(TextList[m_textCount], m_textCount));
            m_textCount++;
            m_activeCount++;
            if (m_textCount >= AlarmList.Count)
            {
                m_textCount = 0;
            }
        }

        for (int i = 0; i < AlarmList.Count; i++)
        {
            if (TextList[i].text != "")
            {
                AlarmList[i].SetActive(true);
            }
            else
            {
                AlarmList[i].transform.localPosition = new Vector3(AlarmList[i].transform.localPosition.x, 0, AlarmList[i].transform.localPosition.z);
                AlarmList[i].SetActive(false);
            }
        }
    }

    private void AddAlarm(int textCount)
    {
        if (m_activeCount >= 1)
        {
            for (int i = 0; i < m_activeCount; i++)
            {
                int nowTextNum = textCount - m_activeCount;
                if (nowTextNum < 0)
                {
                    nowTextNum = AlarmList.Count + nowTextNum;
                }

                AlarmList[nowTextNum].transform.localPosition = new Vector3(AlarmList[nowTextNum].transform.localPosition.x, m_activeCount * 70, AlarmList[nowTextNum].transform.localPosition.z); 
            }
        }
    }

    IEnumerator TextErase(TextMeshProUGUI alarmText, int count)
    {
        while (m_eraseTimer[count] <= 3.0f)
        {
            m_eraseTimer[count] += Time.deltaTime;

            yield return null;
        }
        
        alarmText.text = "";
        m_eraseTimer[count] = 0;
        m_activeCount--;
    }
}
