using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class GameTime : MonoBehaviour
{
    public TextMeshProUGUI m_nowTime;


    public float LimitTime = 30.0f;
    float PrevTime = 0.0f;

    int Year;
    // string[] Month = new string[12];
    // string[] Week = new string[4];

    string[] Month = { "1��", "2��", "3��", "4��", "5��", "6��", "7��", "8��", "9��", "10��", "11��", "12��" };
    string[] Week = { "ù° ��", "��° ��", "��° ��", "��° ��" };


    int MonthIndex = 2;
    int WeekIndex = 0;

    

    // Start is called before the first frame update
    void Start()
    {

        Year = 1;
        // Month[11] = "12��";
        // Week[0] = "ù°��";
        m_nowTime.text = Year + "�� " + Month[MonthIndex] + " " + Week[WeekIndex];

        Debug.Log(Year + "�� " + Month[MonthIndex] + " " + Week[WeekIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        FlowtheTime();

        if (Input.GetKeyDown(KeyCode.A))
        {
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = 1;
        }

    }

    public void FlowtheTime()
    {

        if (PrevTime == 0.0f)
        {
            PrevTime = Time.time;
        }

        if (LimitTime < (Time.time - PrevTime))
        {
            Debug.Log(Time.time);

            ChangeWeek();

            m_nowTime.text = Year + "�� " + Month[MonthIndex] + " " + Week[WeekIndex];

            Debug.Log(Year + "�� " + Month[MonthIndex] + " " + Week[WeekIndex]);

            // 3���̶�� ���� �ð��� ������ �� ��
            if (Year == 3 && MonthIndex == 11 && WeekIndex == 3)
            {
                IsLimitedGameTimeEnd();
            }

            ChangeYear();
            ChangeMonth();

            PrevTime = 0.0f;
        }

    }

    // �� �� ����
    public void ChangeWeek()
    {
        // Week ����
        if (WeekIndex != 3)
        {
            WeekIndex++;
        }
        else if (WeekIndex == 3)
        {
            WeekIndex = 0;
        }
    }

    // �� ����
    public void ChangeMonth()
    {
        // ���� ���� �� �� �ʱ�ȭ
        if (MonthIndex == 11 && WeekIndex == 3)
        {
            MonthIndex = 0;
        }
        // �� ����
        else if (MonthIndex != 11 && WeekIndex == 3)
        {
            MonthIndex++;
        }
    }

    public void ChangeYear()
    {
        // Year ����
        if (MonthIndex == 11 && WeekIndex == 3)
        {
            Year++;
        }
    }

    public void IsLimitedGameTimeEnd()
    {
        Debug.Log("���� ���丮 ��~");

    }
}
