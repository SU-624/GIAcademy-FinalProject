using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 2023. 04. 27 Mang
/// 
/// 이벤트를 실행 시키기 위한 조건 판별, 이벤트 데이터 보유 하는 클래스
/// </summary>
public class EventScheduleSystem : MonoBehaviour
{
    public delegate void SuddenEventDelegate();

    public EventScriptManager eventScriptManager;

    SuddenEventDelegate mySuddenEventDelegate;
    // [SerializeField] private EventScheduleUI MyEventScheduleUI;
    // [SerializeField] private PopUpUI PopUpEventScheduleUI;
    // [SerializeField] private PopOffUI PopOffEventScheduleUI;
    // 
    // [SerializeField] private PopUpUI SimpleExecutionEventPanel;
    // [SerializeField] private PopUpUI OptionChoiceEventPanel;
    // [SerializeField] private PopUpUI TargetSelectEventPanel;

    // [SerializeField] private GameObject m_PossibleEventprefab;

    public List<SuddenEventTableData>
        AllMySuddenEventList = new List<SuddenEventTableData>(); // 돌발&지정 이벤트 모두를 모아놓을 리스트

    public List<SuddenEventTableData>
        SuddenEventList = new List<SuddenEventTableData>(); // 전체 돌발 이벤트 모음

    public List<SuddenEventTableData>
        SelectEventList = new List<SuddenEventTableData>(); // 전체 선택 이벤트 모음

    public List<SuddenEventTableData>
        TodaySuddenEventList = new List<SuddenEventTableData>(); // 이번달 진행 될 돌발이벤트 목록

    //     public List<SuddenEventTableData>
    //         PossibleChooseEventClassList = new List<SuddenEventTableData>(); // 이번달 사용가능한 선택이벤트 목록
    // 
    //     public List<SuddenEventTableData>
    //         ThisMonthMySelectEventList = new List<SuddenEventTableData>(); // 이번달 진행 될 선택이벤트 목록

    public List<UsedEventRepeatData>
        usedEventRepeatDataList = new List<UsedEventRepeatData>(); // 사용한 이벤트에 대한 정보


    int nextMonth;
    int nextWeek;

    float prevTime = 0;
    int limitTime = 1;

    int EventConditionCount = 0; // 각 이벤트의 조건을 판별해 카운트를 나눠 줄 변수

    // Start is called before the first frame update
    void Start()
    {
        // 여기서 필요한 친구들을 Init 해주자
        // 1. (돌발) + () 이벤트 데이터 로드
        FirstLoadThisMonthEventList(); // 이 함수를 제이슨에서 파일 로드할때 부르면 되나 / 우선은 여기다 두자,
        DevideSuddenAndSelectEvent();
        prevTime = Time.time;
        Initailize();
    }

    // Update is called once per frame
    void Update()
    {
        if (TodaySuddenEventList.Count != 0)
        {
            // UI 를 띄우는 함수 

            // 해당 이벤트 클리어
        }

        mySuddenEventDelegate();
    }

    public void Initailize()
    {
        mySuddenEventDelegate += CheckLimitSecond;

        if (Json.Instance.IsSavedDataExists)
        {
            DistributeSuddenEventData();
            DistributeTodaySuddenEventData();
        }
    }

    // 이 함수가 여기 있어야 하는 이유 -> 리스트들을 가져와야 하기 때문이다. & 싱글톤이 아니기 때문이다.
    // 리스트를 밖으로 가져오면 어떨가 싶은데.. 왜냐하면 이 저장함수를 데이터 저장 버튼을 누르는 등 다른곳에서 쓰일 수 있게 하기 위해
    // allInOneData 클래스가 Monobehavior 상속받지 않기 때문에 이걸 잘 쓰기 위한 방법을 찾아야한다.
    // 어디서 이걸 받아서 쓰게 만들면 좋지
    public void CollectSuddenEvent()
    {
        AllInOneData.Instance.CollectSuddenEventData(usedEventRepeatDataList, TodaySuddenEventList);
    }

    public void DistributeSuddenEventData()
    {
        // 전체 사용 돌발 이벤트 정보 순회
        foreach (var nowUsedEventData in AllInOneData.Instance.SuddenEventData)
        {
            UsedEventRepeatData usedEventRepeatData = new UsedEventRepeatData();
            // SuddenEventData 의 정보를 usedEventRepeatDataList 안에다가 넣어주기
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var setFields = typeof(UsedEventRepeatData).GetFields(flags);
            var dataFields = typeof(UsedEventSaveData).GetProperties(flags);

            foreach (var newStat in setFields)
            {
                foreach (var saveStat in dataFields)
                {
                    if (newStat.Name == saveStat.Name)
                    {
                        var value = saveStat.GetValue(nowUsedEventData);
                        newStat.SetValue(usedEventRepeatData, value);
                        break;
                    }
                }
            }

            usedEventRepeatDataList.Add(usedEventRepeatData);
        }
    }

    public void DistributeTodaySuddenEventData()
    {
        // TodaySuddenEventData 에 저장된 ID로 전체 이벤트중에서 검색. TodaySuddenEventList 에다가 이벤트 정보를 넣어주자.
        foreach (var nowTodayEventData in AllInOneData.Instance.TodaySuddenEventData)
        {
            foreach (var nowAllEventData in AllMySuddenEventList)
            {
                if (nowTodayEventData.SuddenEventID == nowAllEventData.SuddenEventID)
                {
                    TodaySuddenEventList.Add(nowAllEventData);
                }
            }
        }
    }

    // 1초마다 이벤트 판별
    public void CheckLimitSecond()
    {
        if (Time.time - prevTime >= limitTime) // limitTime 마다 이벤트의 조건 판별함 
        {
            CheckConditonAllSuddenEvent();

            prevTime = Time.time;
        }
    }

    // 처음 게임 실행 시 세이브데이터 X -> 원본데이터 로드, 세이브데이터O -> 세이브데이터 로드
    public void FirstLoadThisMonthEventList()
    {
        int SuddenEventcount = AllOriginalJsonData.Instance.OriginalSuddenEventDataList.Count;

        for (int i = 0; i < SuddenEventcount; i++)
        {
            AllMySuddenEventList.Add(AllOriginalJsonData.Instance.OriginalSuddenEventDataList[i]);
        }
    }

    // 돌발이벤트와 지정이벤트 리스트를 나눠두기
    public void DevideSuddenAndSelectEvent()
    {
        for (int i = 0; i < AllMySuddenEventList.Count; i++)
        {
            if (AllMySuddenEventList[i].IsSuddenEvent == true) // 이 이벤트가 돌발이벤트인가
            {
                SuddenEventList.Add(AllMySuddenEventList[i]);
            }
            else
            {
                SelectEventList.Add(AllMySuddenEventList[i]);
            }
        }
    }

    // 매 초마다 이벤트 타입 따라 조건 체크
    // 1 - 조건에 상관없이 처음 발생 날짜에 무조건 등장. 이후 조건 충족 될 때마다 실행
    // 이벤트를 다시 쓸 수 있는지 확인하는 함수
    // 2 - 처음발생날짜는 없고 조건이 충족될때마다 실행 -> 충족 될 떄마다
    // 3 - 모든조건 중 1개이상 충족되면 실행
    // 4 - 모든조건이 충족되면 실행
    // 5 - 조건충족에 따라 1회만 실행
    public void CheckConditonAllSuddenEvent()
    {
        for (int i = 0; i < SuddenEventList.Count; i++) // 전체 이벤트 리스트 돌면서 조건들 체크
        {
            // 이벤트 금지 기간이 끝났는지 확인
            if (!UsedSuddenEventRepeatGapChaker(SuddenEventList[i])) continue;

            int tempConditonIndex = SuddenEventList[i].ConditionType;

            if (tempConditonIndex == 3)
            {
                // 이벤트 조건이 맞는지 확인
                if (!SuddenEventDispatcher(SuddenEventList[i], true)) continue; // or 조건일땐 true
            }
            else
            {
                // 이벤트 조건이 맞는지 확인
                if (!SuddenEventDispatcher(SuddenEventList[i], false)) continue; // and 조건일땐 false
            }

            TodaySuddenEventList.Add(SuddenEventList[i]); // 이벤트를 리스트 넣는다.
            UsedSuddenEventRepeatGapSet(SuddenEventList[i]); // 사용했다고 체크한다.
        }
    }

    public void SuddenEventConditionIndex1(SuddenEventTableData tempEvent)
    {
        // 이벤트 금지 기간이 끝났는지 확인
        if (!UsedSuddenEventRepeatGapChaker(tempEvent)) return;
        // 이벤트 조건이 맞는지 확인
        if (!SuddenEventDispatcher(tempEvent, false)) return; // and 조건일땐 false

        TodaySuddenEventList.Add(tempEvent); // 이벤트를 리스트 넣는다.
        UsedSuddenEventRepeatGapSet(tempEvent); // 사용했다고 체크한다.
    }

    // 누적 날자를 숫자로 변경해서 비교하기
    public int CalculateTotalDaysPasser(int year, int month, int week, int day)
    {
        // 1년은 12달, 1달은 4주, 1주는 5일
        int totalDaysPassed = ((year - 1) * 12 * 4 * 5) + ((month - 1) * 4 * 5) + ((week - 1) * 5) + day;
        return totalDaysPassed;
    }

    // 게임 내의 날자와 이벤트의 사용 가능한 날자를 확인하는 함수
    private bool UsedSuddenEventRepeatGapChaker(SuddenEventTableData tempEvent)
    {
        // 날자 조건이 없거나 날자를 비교해서 이벤트가  가능하면 true 리턴.

        int eventRepeatYear = 0;
        int eventRepeatMonth = 0;
        int eventRepeatWeek = 0;
        int eventRepeatDay = 0;
        int eventRepeatTotalDays = 0;

        // 게임 시간 접근 방법
        int nowYear = GameTime.Instance.FlowTime.NowYear;
        int nowMonth = GameTime.Instance.FlowTime.NowMonth;
        int nowWeek = GameTime.Instance.FlowTime.NowWeek;
        int nowDay = GameTime.Instance.FlowTime.NowDay;

        int gameTotalDays = CalculateTotalDaysPasser(nowYear, nowMonth, nowWeek, nowDay);

        // 우리 게임은 한달이 무조껀 4주
        // 1달짜리 Gap 이면 현실시간의 한달 처럼 만으로 1달이 차이나야 한다.
        foreach (var usedEventRepeatData in usedEventRepeatDataList)
        {
            // 사용한 이벤트 중 이번 이벤트를 찾는다.
            if (usedEventRepeatData.SuddenEventID != tempEvent.SuddenEventID) continue;

            // 적중한다면 사용한 이벤트 이다.
            // 이벤트가 사용 가능한 날인지 확인한다.
            eventRepeatYear = usedEventRepeatData.YearData;
            eventRepeatMonth = usedEventRepeatData.MonthData;
            eventRepeatWeek = usedEventRepeatData.WeekData;
            eventRepeatDay = usedEventRepeatData.DayData;
            eventRepeatTotalDays =
                CalculateTotalDaysPasser(eventRepeatYear, eventRepeatMonth, eventRepeatWeek, eventRepeatDay);

            if (gameTotalDays >= eventRepeatTotalDays) return true; // 사용 가능한 날이라면 
            return false; // 아직 사용 가능한 날자가 아니라면
        }

        // 사용하지 않은 이벤트일떄
        // 이벤트가 사용 가능한 날인지 확인한다.
        eventRepeatYear = tempEvent.SuddenEventDate[0];
        eventRepeatMonth = tempEvent.SuddenEventDate[1];
        eventRepeatWeek = tempEvent.SuddenEventDate[2];
        eventRepeatDay = tempEvent.SuddenEventDate[3];
        eventRepeatTotalDays =
            CalculateTotalDaysPasser(eventRepeatYear, eventRepeatMonth, eventRepeatWeek, eventRepeatDay);

        if (gameTotalDays >= eventRepeatTotalDays) return true; // 사용 가능한 날이라면 
        return false; // 아직 사용 가능한 날자가 아니라면
    }

    // 이벤트를 사용한 후 이벤트 반복 주기와 해금 정보를 설정하는 함수
    private void UsedSuddenEventRepeatGapSet(SuddenEventTableData tempEvent)
    {
        // 사용 했던 이벤트를 다시 설정하는건지 확인
        for (int i = 0; i < usedEventRepeatDataList.Count; i++)
        {
            // 사용한 이벤트 중 이번 이벤트를 찾는다.
            if (usedEventRepeatDataList[i].SuddenEventID == tempEvent.SuddenEventID)
            {
                // 사용한 정보 제거
                usedEventRepeatDataList.RemoveAt(i);
                break;
            }
        }


        // 처음 사용한 이벤트의 반복 주기 설정
        UsedEventRepeatData tempData = new UsedEventRepeatData();

        // 인게임 현재시간
        int nowYear = GameTime.Instance.FlowTime.NowYear;
        int nowMonth = GameTime.Instance.FlowTime.NowMonth;
        int nowWeek = GameTime.Instance.FlowTime.NowWeek;
        int nowDay = GameTime.Instance.FlowTime.NowDay;
        int gapYear = tempEvent.SuddenEventRepeatGap["Year"];
        int gapMonth = tempEvent.SuddenEventRepeatGap["Month"];


        tempData.SuddenEventID = tempEvent.SuddenEventID;
        tempData.YearData = nowYear + gapYear;
        tempData.MonthData = nowMonth + gapMonth;
        tempData.WeekData = nowWeek;
        tempData.DayData = nowDay;

        // 13월이 되면 년도를 하나 증가시키자.
        if (tempData.MonthData >= 13)
        {
            tempData.MonthData -= 12;
            tempData.YearData++;
        }

        // 반복하지 않는 이벤트는 년도를 999 증가시키기
        if (!tempEvent.SuddenEventRepeat)
        {
            tempData.YearData = 999;
        }

        usedEventRepeatDataList.Add(tempData);
        // TODO :: 해금되는 회사, 강사 정보를 확인해서 활성화 시켜주기


        // 강사가 해금되면 간단 돌발로 바로 보여줘야 한다.
    }

    // 이벤트(옵션, 타겟 )포기하기 버튼을 눌렀을 때 리스트에서 빼주기
    public void IfGiveUpButtonClick()
    {
        TodaySuddenEventList.RemoveAt(0);
    }

    // 최대 3개의 조건이 모두 충족되는지 판별 or 조건인지는 인자로 받는다.
    // 아 함수 조건 방향 바꿔야해.............
    // 성공 여부를 bool 배열에다가 넣고, 
    private bool SuddenEventDispatcher(SuddenEventTableData tempEvent, bool isAnyConditionMet)
    {
        // 필요한것
        // 1. 이벤트를 적용할 콘텐츠 그 자체가 없음 (랭크, 취업률 등등)
        // 2. 학생 정보 검색을 위하여 이벤트가 들어오는 순서가 확정되야 함.
        int eventCondition = 0;
        int amountN = 0;
        int amountM = 0;
        int tempamount = 50; // TODO :: 

        // 각 이벤트의 조건 판별 결과 저장
        bool[] resultArr = new bool[4] { false, false, false, false };
        // 이벤트 조건 index가 0 이면 false
        bool[] usedIndexArr = new bool[4] { false, true, true, true };

        // 캐릭터가 소유한 정보 탐색용
        int studentCount = 0;
        bool searchSkill = false;
        string skillID = "";

        List<InteractionManager.GenreRoomInfo> genreRooms = InteractionManager.Instance.GenreRoomList;

        // 0번은 사용하지 않음. index의 숫자와 배열의 숫자를 맞추기 위하여.
        for (int eventTypeindex = 3; eventTypeindex >= 1; eventTypeindex--)
        {
            if (eventTypeindex == 1)
            {
                eventCondition = tempEvent.SuddenEventCondition1;
                amountN = tempEvent.Amount1N;
                amountM = tempEvent.Amount1M;
            }
            else if (eventTypeindex == 2)
            {
                eventCondition = tempEvent.SeddenEventCondition2;
                amountN = tempEvent.Amount2N;
                amountM = tempEvent.Amount2M;
            }
            else if (eventTypeindex == 3)
            {
                eventCondition = tempEvent.SeddenEventCondition3;
                amountN = tempEvent.Amount3N;
                amountM = tempEvent.Amount3M;
            }
            else
            {
                Debug.LogError("SuddenEventDispatcher :: error event index");
                return false;
            }

            switch (eventCondition)
            {
                case 0: // 조건이 없다면 그 인덱스를 사용하지 않는다.
                    usedIndexArr[eventTypeindex] = false;
                    break;

                case 100: // 조건이 없다면 그 인덱스를 사용하지 않는다.
                    resultArr[eventTypeindex] = true;
                    break;

                #region 보유량 변화 이벤트

                case 200: // 1차 재화 N이상 보유
                    if (PlayerInfo.Instance.m_MyMoney >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                case 201: // 2차 재화 N 이상 보유
                    if (PlayerInfo.Instance.m_SpecialPoint >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                case 202: // 1차재화 N이하 보유
                    if (PlayerInfo.Instance.m_MyMoney <= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                case 203: //2차재화 N이하 보유
                    if (PlayerInfo.Instance.m_SpecialPoint <= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                #endregion

                #region 달성 이벤트

                case 300: // 분기순위 N위 달성
                {
                    // 분기순위 정보 필요
                    if ((GameTime.Instance.FlowTime.NowMonth == 3 || GameTime.Instance.FlowTime.NowMonth == 6 ||
                         GameTime.Instance.FlowTime.NowMonth == 9 ||
                         GameTime.Instance.FlowTime.NowMonth == 12) && GameTime.Instance.FlowTime.NowDay == 5)
                    {
                        if (QuarterReport.m_MyAcademyQuarterRank >= amountN)
                            resultArr[eventTypeindex] = true;
                    }
                }
                    break;

                case 301: // 랭크 N등급 달성
                    // 랭크 정보 필요
                {
                    if ((int)PlayerInfo.Instance.m_CurrentRank - 1 >= amountN)
                    {
                        resultArr[eventTypeindex] = true;
                    }
                }
                    break;

                case 302: // 취업률 N% 달성
                    // 취업률 정보 필요
                    if (tempamount >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                case 303: // 유명도점수 N점 달성
                    // 유명도 점수 필요
                    if (PlayerInfo.Instance.m_Awareness >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                case 304: // 재화점수 N점 달성
                    // 재화점수 필요
                    if (PlayerInfo.Instance.m_Goods >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                case 305: // 인재양성점수 N점 달성
                    //  인재양성 점수 필요
                    if (PlayerInfo.Instance.m_TalentDevelopment >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                case 306: // 활동점수 N점 달성
                    // 활동 점수 필요
                    if (PlayerInfo.Instance.m_Activity >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                case 307: // 운영점수 N점 이상
                    // 운영 점수 필요
                    if (PlayerInfo.Instance.m_Management >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                //case 308: // 유명점수 N점 이상
                //    // 유명 점수 필요
                //    if (tempamount >= amountN)
                //        resultArr[eventTypeindex] = true;
                //    break;

                //case 309: // 재화점수 N점 이상
                //    // 재화점수 필요
                //    if (tempamount >= amountN)
                //        resultArr[eventTypeindex] = true;
                //    break;

                //case 310: // 인재점수 N점 이상
                //    // 인재점수 필요
                //    if (tempamount >= amountN)
                //        resultArr[eventTypeindex] = true;
                //    break;

                //case 311: // 활동점수 N점 이상
                //    // 활동 점수 필요
                //    if (tempamount >= amountN)
                //        resultArr[eventTypeindex] = true;
                //    break;

                //case 312: // 운영점수 N점 이상
                //    // 운영 점수 필요
                //    if (tempamount >= amountN)
                //        resultArr[eventTypeindex] = true;
                //    break;

                #endregion

                #region 학생 소유 정보 이벤트

                // 각 학과의 학생들 중 N명이 무언가를 가지고 있는가...
                case 400: // 보유한 기획파트 모든 학생 중 N명이 스킬 혹은 물건, 능력치 등등을 얼마나 가지고 있는지?
                {
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        if (student.m_StudentStat.m_StudentType == StudentType.GameDesigner)
                        {
                            if (searchSkill) // 스킬 정보를 찾고 싶어요.
                            {
                                foreach (var skill in student.m_StudentStat.m_Skills)
                                {
                                    if (skill == skillID)
                                    {
                                        studentCount++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (studentCount >= amountN) // 스킬을 보유한 학생의 수가 충분하면
                    {
                        resultArr[eventTypeindex] = true;
                    }
                }
                    studentCount = 0;

                    break;

                case 401: // 보유한 아트파트 학생 중 N명
                {
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        if (student.m_StudentStat.m_StudentType == StudentType.Art)
                        {
                            if (searchSkill) // 스킬 정보를 찾고 싶어요.
                            {
                                foreach (var skill in student.m_StudentStat.m_Skills)
                                {
                                    if (skill == skillID)
                                    {
                                        studentCount++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (studentCount >= amountN) // 스킬을 보유한 학생의 수가 충분하면
                    {
                        resultArr[eventTypeindex] = true;
                    }
                }

                    studentCount = 0;
                    break;

                case 402: // 보유한 프로그래밍파트 학생 중 N명
                {
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        if (student.m_StudentStat.m_StudentType == StudentType.Programming)
                        {
                            if (searchSkill) // 스킬 정보를 찾고 싶어요.
                            {
                                foreach (var skill in student.m_StudentStat.m_Skills)
                                {
                                    if (skill == skillID)
                                    {
                                        studentCount++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (studentCount >= amountN) // 스킬을 보유한 학생의 수가 충분하면
                    {
                        resultArr[eventTypeindex] = true;
                    }
                }

                    studentCount = 0;

                    break;

                case 403: // 특정 학생 ID(N) 보유
                    foreach (var student in ObjectManager.Instance.m_StudentList)
                    {
                        // 특정 번호를 가진 학생을 가지고 있다면 true
                        if (student.m_StudentStat.m_StudentID == amountN.ToString()) // TODO : 이렇게 하는게 맞을 지 다시 보자
                        {
                            resultArr[eventTypeindex] = true;
                        }
                    }

                    break;


                case 406: // 보너스스킬 N 보유
                    searchSkill = true; // 스킬 탐색 시작
                    skillID = amountN.ToString(); // 스킬 ID를 저장해두자.
                    resultArr[eventTypeindex] = true; // 스킬에 대한 정보 자체는 판단 조건이 아님.
                    break;

                #endregion

                #region 실행, 참여 여부 이벤트

                case 500: // 이벤트ID N실행완료
                    foreach (var usedEventListStruct in usedEventRepeatDataList) // 사용한 이벤트 리스트 구조체에서하나씩 뽑아서
                    {
                        // 그 이벤트의 ID가 지금 찾는 이벤트 아이디와 같은지 확인
                        if (usedEventListStruct.SuddenEventID == amountN)
                        {
                            //이벤트를 사용했으니 추가
                            resultArr[eventTypeindex] = true;
                        }
                    }

                    break;

                case 501: // 이벤트ID N미실행
                    foreach (var usedEventListStruct in usedEventRepeatDataList) // 사용한 이벤트 리스트 구조체에서하나씩 뽑아서
                    {
                        // 그 이벤트의 ID가 지금 찾는 이벤트 아이디와 같은지 확인
                        if (usedEventListStruct.SuddenEventID == amountN)
                        {
                            // 사용한 이벤트 목록에 있다면 변경하지 않음.
                            break;
                        }

                        // 이벤트를 사용한 적 없으니 체크.
                        resultArr[eventTypeindex] = true;
                    }

                    break;

                case 502: // 수업ID N실행
                    // 실행한 수업에 대한 ID 리스트 가 필요
                    break;

                case 600: // 게임잼 N회 참여
                    // 참여한 게임잼의 수 정보 필요
                    if (tempamount >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                case 601: // 공모전 N회 참여
                    // 참여한 공모전의 수 정보 필요
                    if (tempamount >= amountN)
                        resultArr[eventTypeindex] = true;
                    break;

                // TODO :  조건 추가하기
                case 602: // 게임잼 ID N에 참여
                    break;
                case 603: // 공모전 ID N에 참여
                    break;
                case 604: // 특정 게임잼에서 1위 N회 달성
                    break;
                case 605: // 모든 게임잼 통틀어 1위 N회 달성
                    break;

                #endregion

                #region 내구도 수리 N회 이벤트

                case 700: // RPG 장르방 내구도 수리 N회
                    if (tempEvent.Amount1N <= genreRooms[(int)InteractionManager.SpotName.RPGRoom].RepairCount)
                    {
                        resultArr[eventTypeindex] = true;
                    }

                    break;
                case 701: // 시뮬레이션 장르방 내구도 수리 N회 
                    if (tempEvent.Amount1N <= genreRooms[(int)InteractionManager.SpotName.SimulationRoom].RepairCount)
                    {
                        resultArr[eventTypeindex] = true;
                    }

                    break;
                case 702: // 퍼즐 장르방 내구도 수리 N회
                    if (tempEvent.Amount1N <= genreRooms[(int)InteractionManager.SpotName.PuzzleRoom].RepairCount)
                    {
                        resultArr[eventTypeindex] = true;
                    }

                    break;
                case 703: // 슈팅 장르방 내구도 수리 N회
                    if (tempEvent.Amount1N <= genreRooms[(int)InteractionManager.SpotName.ShootingRoom].RepairCount)
                    {
                        resultArr[eventTypeindex] = true;
                    }

                    break;
                case 704: // 액션 장르방 내구도 수리 N회
                    if (tempEvent.Amount1N <= genreRooms[(int)InteractionManager.SpotName.ActionRoom].RepairCount)
                    {
                        resultArr[eventTypeindex] = true;
                    }

                    break;
                case 705: // 스포츠 장르방 내구도 수리 N회
                    if (tempEvent.Amount1N <= genreRooms[(int)InteractionManager.SpotName.SportsRoom].RepairCount)
                    {
                        resultArr[eventTypeindex] = true;
                    }

                    break;
                case 706: // 어드벤쳐 장르방 내구도 수리 N회
                    if (tempEvent.Amount1N <= genreRooms[(int)InteractionManager.SpotName.AdventureRoom].RepairCount)
                    {
                        resultArr[eventTypeindex] = true;
                    }

                    break;
                case 707: // 리듬 장르방 내구도 수리 N회
                    if (tempEvent.Amount1N <= genreRooms[(int)InteractionManager.SpotName.RhythmRoom].RepairCount)
                    {
                        resultArr[eventTypeindex] = true;
                    }

                    break;
                case 800: // 자판기 N회 이용
                    Debug.Log("자판기 미구현");
                    break;
                case 9999: // 테스트용으로 만든?
                    break;

                #endregion

                default:
                    // Debug.LogError("SuddenEventDispatcher :: no setting sudden evnet code.");
                    return false;
            }
        }

        // 조건 3개를 다 판별한 후 결과를 반환한다.
        if (isAnyConditionMet) // or 조건이라면
        {
            for (int i = 3; i > 0; i--)
            {
                if (usedIndexArr[i]) // 확인한 이벤트의 인덱스만 접근
                {
                    if (resultArr[i]) // 하나의 조건이라도 만족한다면
                        return true;
                }
            }

            return false; // 어느 조건도 만족하지 못했다면
        }

        // and 조건이라면
        for (int i = 3; i > 0; i--)
        {
            if (usedIndexArr[i])
            {
                if (!resultArr[i]) // 하나의 조건이라도 불만족하면
                    return false;
            }
        }

        return true; // 모든 조건을 만족한다면
    }
}