using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatData.Runtime;

public struct SaveEventClassData
{
    public string EventClassName;       // 이벤트 이름
    public int RewardStat;              // 보상 스탯
    public int RewardMoney;             // 보상 머니
    public int PossibleSetCount;        // 지정 가능 횟수
    public string EventInformation;     // 이벤트 설명 내용
}

// 보상이 스탯인 경우 : 마지막에 ObjectManager.Instance.m_StudentList 의 스탯을 변경 해주면 된다
// 보상이 재화인 경우 : PlayerInfo.Instance.m_MyMoney 값을 마지막으로 바꿔주면 된다

public class EventClassPrefab : MonoBehaviour
{
    // 제이슨 파일 생성 전 테스트를 위해 만든 변수
    SaveEventClassData TempEventData = new SaveEventClassData();

    // Json 파일을 파싱해서 그 데이터들을 다 담아 줄 리스트 변수
    public List<SaveEventClassData> SelectEventClassInfo = new List<SaveEventClassData>();      // 선택 이벤트
    public List<SaveEventClassData> FixedEventClassInfo = new List<SaveEventClassData>();       // 고정 이벤트

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            // 테스트
            // 이벤트 struct 관련 초기화 해주기
            TempEventData.EventClassName = "test" + i;
            TempEventData.RewardStat = 3;
            TempEventData.RewardMoney = 1000;

            SelectEventClassInfo.Add(TempEventData);

            TempEventData.RewardStat += 1;
            TempEventData.RewardMoney += 200;
        }
        // 리스트에 제이슨 파싱 할 때 : .Add() -> 한번에 다 파싱하는게 아니라 1. 제이슨파일 하나 읽어서 임시변수에 담고 리스트에 임시변수 넣기
        // 그다임 하나 읽고 임시변수 담고 리스트 넣기 반복! 아하...
        // 

        // ObjectManager.Instance.m_StudentList[i].m_StudentData.m_StudentContentsValue += 3;       이렇게 추가
    }

    // Update is called once per frame
    void Update()
    {

    }
}
