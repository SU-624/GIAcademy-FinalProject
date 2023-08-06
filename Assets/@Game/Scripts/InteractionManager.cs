using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;


/// <summary>
/// 각 장르방을 관리해줄 스크립트
/// 매점과 서점까지 추가
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public enum SpotName : int
    {
        PuzzleRoom = 0,
        SimulationRoom,
        RhythmRoom,
        AdventureRoom,
        RPGRoom,
        SportsRoom,
        ActionRoom,
        ShootingRoom,
        Store = 10,
        BookStore,
        StudyRoom,
        Lounge1,
        Lounge2,
        VendingMachine = 20,
        Pot,
        AmusementMachine,
        WaterPurifier,
        NoticeBoard,
        Nothing,
    }

    // 8가지 장르방
    public class GenreRoomInfo
    {
        public string GenreRoomName;
        public int StudentCount;
        public int SeatNum;
        public int Durability;
        public int Level;
        public int RepairCount;
    }

    // 4가지 시설
    public class FacilityInfo
    {
        public bool IsCalculating;  // 서점, 매점에서만 사용
        public int PointNum;
        public int StudentCount;
    }

    // 5가지 오브젝트
    public class ObjectInfo
    {
        public bool IsUsed;
        public int UseCount;        // 자판기에 사용
    }

    private static InteractionManager instance = null;

    public static InteractionManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    [Header("PuzzleRoom")]
    [SerializeField] private Transform PuzzleEntrance;
    public Transform[] PuzzleSeats;

    [Header("SimulationRoom")]
    [SerializeField] private Transform SimulationEntrance;
    public Transform[] SimulationSeats;

    [Header("RhythmRoom")]
    [SerializeField] private Transform RhythmEntrance;
    public Transform[] RhythmSeats;

    [Header("AdventureRoom")]
    [SerializeField] private Transform AdventureEntrance;
    public Transform[] AdventureSeats;

    [Header("RPGRoom")]
    [SerializeField] private Transform RPGEntrance;
    public Transform[] RPGSeats;

    [Header("SportsRoom")]
    [SerializeField] private Transform SportsEntrance;
    public Transform[] SportsSeats;

    [Header("ActionRoom")]
    [SerializeField] private Transform ActionEntrance;
    public Transform[] ActionSeats;

    [Header("ShootingRoom")]
    [SerializeField] private Transform ShootingEntrance;
    public Transform[] ShootingSeats;

    [Header("Store")]
    public Transform StoreEntrance;
    public Transform[] StorePoints;

    [Header("BookStore")]
    public Transform BookStoreEntrance;
    public Transform[] BookStorePoints;

    [Header("StudyRoom")]
    public Transform StudyRoomEntrance;
    public Transform[] StudyRoomSeats;

    [Header("Lounge1")]
    public Transform LoungeEntrance;
    public Transform[] LoungeSeats;

    [Header("Lounge2")]
    public Transform Lounge2Entrance;
    public Transform[] Lounge2Seats;

    [Header("오브젝트")]
    public Transform[] VendingMachines;
    public Transform[] Pots;
    public Transform[] AmusementMachines;
    public Transform[] WaterPurifier;
    public Transform[] NoticeBoard;


    public List<GenreRoomInfo> GenreRoomList = new List<GenreRoomInfo>();
    public List<Transform> GenreRoomEntrances = new List<Transform>();
    public List<Transform> GenreRoomCenters = new List<Transform>();


    public List<FacilityInfo> FacilityList = new List<FacilityInfo>();

    public List<GameObject> StoreWaitingQ = new List<GameObject>();
    public List<GameObject> BookStoreWaitingQ = new List<GameObject>();

    public List<ObjectInfo> VendingMachineList = new List<ObjectInfo>();
    public List<ObjectInfo> PotList = new List<ObjectInfo>();
    public List<ObjectInfo> AmusementMachineList = new List<ObjectInfo>();
    public List<ObjectInfo> WaterPurifierList = new List<ObjectInfo>();
    public List<ObjectInfo> NoticeBoardList = new List<ObjectInfo>();

    [Header("장르방 업그레이드")]
    [SerializeField] private List<GameObject> GenreRoomUpgrade;
    [SerializeField] private GameObject UpgradeEffectPrefab;

    [SerializeField] private GameObject EmoticonPrefab;
    [SerializeField] private GameObject ChatBoxPrefab;

    private ScriptsManager m_scriptsManager;

    private List<GameObject> m_emoticonBoxList = new List<GameObject>();
    private List<GameObject> m_chatBoxList = new List<GameObject>();

    private Transform m_canvas;
    private int m_emoNum = 0;
    private int m_chatNum = 0;
    public GameObject UpgradeEffect1;
    public GameObject UpgradeEffect2;
    

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        m_canvas = GameObject.Find("InGameCanvas").transform;
        for (int i = 0; i < 8; i++)
        {
            GenreRoomInfo newRoom = new GenreRoomInfo();
            if (i == 0)
            {
                newRoom.GenreRoomName = "퍼즐룸";
            }
            else if (i == 1)
            {
                newRoom.GenreRoomName = "시뮬레이션룸";
            }
            else if (i == 2)
            {
                newRoom.GenreRoomName = "리듬룸";
            }
            else if (i == 3)
            {
                newRoom.GenreRoomName = "어드벤쳐룸";
            }
            else if (i == 4)
            {
                newRoom.GenreRoomName = "RPG룸";
            }
            else if (i == 5)
            {
                newRoom.GenreRoomName = "스포츠룸";
            }
            else if (i == 6)
            {
                newRoom.GenreRoomName = "액션룸";
            }
            else if (i == 7)
            {
                newRoom.GenreRoomName = "슈팅룸";
            }

            newRoom.StudentCount = 0;
            newRoom.SeatNum = 0;
            newRoom.Durability = 100;
            newRoom.Level = 1;
            GenreRoomList.Add(newRoom);
        }
        GenreRoomEntrances.Add(PuzzleEntrance);
        GenreRoomEntrances.Add(SimulationEntrance);
        GenreRoomEntrances.Add(RhythmEntrance);
        GenreRoomEntrances.Add(AdventureEntrance);
        GenreRoomEntrances.Add(RPGEntrance);
        GenreRoomEntrances.Add(SportsEntrance);
        GenreRoomEntrances.Add(ActionEntrance);
        GenreRoomEntrances.Add(ShootingEntrance);

        FacilityInfo store = new FacilityInfo();
        store.PointNum = 0;

        FacilityInfo bookStore = new FacilityInfo();
        bookStore.PointNum = 0;

        FacilityInfo studyRoom = new FacilityInfo();
        studyRoom.PointNum = 0;
        studyRoom.StudentCount = 0;

        FacilityInfo lounge1 = new FacilityInfo();
        lounge1.PointNum = 0;
        lounge1.StudentCount = 0;

        FacilityInfo lounge2 = new FacilityInfo();
        lounge2.PointNum = 0;
        lounge2.StudentCount = 0;

        FacilityList.Add(store);
        FacilityList.Add(bookStore);
        FacilityList.Add(studyRoom);
        FacilityList.Add(lounge1);
        FacilityList.Add(lounge2);

        for (int i = 0; i < 20; i++)
        {
            GameObject newEmoticon = Instantiate(EmoticonPrefab);
            m_emoticonBoxList.Add(newEmoticon);
            newEmoticon.SetActive(false);
            newEmoticon.transform.SetParent(m_canvas);

            GameObject newChatBox = Instantiate(ChatBoxPrefab);
            m_chatBoxList.Add(newChatBox);
            newChatBox.SetActive(false);
            newChatBox.transform.SetParent(m_canvas);
        }

        for (int i = 0; i < VendingMachines.Length; i++)
        {
            ObjectInfo vendingMachine = new ObjectInfo();
            vendingMachine.IsUsed = false;
            vendingMachine.UseCount = 0;
            VendingMachineList.Add(vendingMachine);
        }

        for (int i = 0; i < Pots.Length; i++)
        {
            ObjectInfo pot = new ObjectInfo();
            pot.IsUsed = false;
            pot.UseCount = 0;
            PotList.Add(pot);
        }

        for (int i = 0; i < AmusementMachines.Length; i++)
        {
            ObjectInfo amusementMachine = new ObjectInfo();
            amusementMachine.IsUsed = false;
            amusementMachine.UseCount = 0;
            AmusementMachineList.Add(amusementMachine);
        }

        for (int i = 0; i < WaterPurifier.Length; i++)
        {
            ObjectInfo water = new ObjectInfo();
            water.IsUsed = false;
            water.UseCount = 0;
            WaterPurifierList.Add(water);
        }

        for (int i = 0; i < NoticeBoard.Length; i++)
        {
            ObjectInfo notice = new ObjectInfo();
            notice.IsUsed = false;
            notice.UseCount = 0;
            NoticeBoardList.Add(notice);
        }

        m_scriptsManager = ScriptsManager.Instance;
    }

    private int CheckRoom(SpotName room)
    {
        if (GenreRoomList[(int)room].StudentCount >= 6)
        {
            return -1;
        }
        if (GenreRoomList[(int)room].Durability <= 0)
        {
            return -2;
        }

        return 0;
    }

    public int EnterRoom(SpotName room)
    {
        int checkResult = CheckRoom(room);
        if (checkResult == 0)
        {
            GenreRoomList[(int)room].StudentCount++;
            int seatNum = GenreRoomList[(int)room].SeatNum;
            GenreRoomList[(int)room].SeatNum++;
            if (GenreRoomList[(int)room].SeatNum >= 6)
            {
                GenreRoomList[(int)room].SeatNum = 0;
            }

            return seatNum;
        }
        else
        {
            return checkResult;
        }
    }

    public void ExitRoom(SpotName room)
    {
        if (room == SpotName.Nothing)
            return;

        GenreRoomList[(int)room].StudentCount--;
        GenreRoomList[(int)room].Durability -= 10;
    }

    // 서점, 매점에 해당
    public int EnterStore(int num)
    {
        int storeNum = num - 10;
        int pointNum = FacilityList[storeNum].PointNum;
        FacilityList[storeNum].PointNum++;
        if (FacilityList[storeNum].PointNum >= 3)
        {
            FacilityList[storeNum].PointNum = 0;
        }

        return pointNum;
    }

    // 서점, 매점에 해당
    public void ExitStore(int num)
    {
        int storeNum = num - 10;
        FacilityList[storeNum].IsCalculating = false;
        bool isProfessor = false;

        if (storeNum == 0)
        {
            if (StoreWaitingQ.Count != 0)
            {
                GameObject nowStudent = StoreWaitingQ[0];
                if (nowStudent.tag == "Instructor")
                    isProfessor = true;

                if (isProfessor)
                {
                    nowStudent.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
                }
                else
                {
                    nowStudent.GetComponent<Student>().DoingValue = Student.Doing.FreeWalk;
                }
                nowStudent.GetComponent<NavMeshAgent>().isStopped = false;
                StoreWaitingQ.RemoveAt(0);
            }

            for (int i = 0; i < StoreWaitingQ.Count; i++)
            {
                if (i == 0)
                {
                    StoreWaitingQ[0].GetComponent<NavMeshAgent>().SetDestination(StorePoints[4].position);
                }
                else if (i == 1)
                {
                    StoreWaitingQ[1].GetComponent<NavMeshAgent>().SetDestination(StorePoints[5].position);
                }
                else
                    break;
            }
        }
        else if (storeNum == 1)
        {
            if (BookStoreWaitingQ.Count != 0)
            {
                GameObject nowStudent = BookStoreWaitingQ[0];
                if (nowStudent.tag == "Instructor")
                    isProfessor = true;

                if (isProfessor)
                {
                    nowStudent.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
                }
                else
                {
                    nowStudent.GetComponent<Student>().DoingValue = Student.Doing.FreeWalk;
                }
                nowStudent.GetComponent<NavMeshAgent>().isStopped = false;
                BookStoreWaitingQ.RemoveAt(0);
            }

            for (int i = 0; i < BookStoreWaitingQ.Count; i++)
            {
                if (i == 0)
                {
                    BookStoreWaitingQ[0].GetComponent<NavMeshAgent>().SetDestination(BookStorePoints[4].position);
                }
                else if (i == 1)
                {
                    BookStoreWaitingQ[1].GetComponent<NavMeshAgent>().SetDestination(BookStorePoints[5].position);
                }
                else
                    break;
            }
        }
    }

    public void GetStoreReward(int spot)
    {
        int randomReward = Random.Range(1, 101);
        int randIncome = 0;

        if (spot == (int)SpotName.Store)
        {
            if (randomReward <= 70)
            {
                randIncome = Random.Range(300, 4001);
                StartCoroutine(ClickEventManager.Instance.MoneyFadeOutText(StorePoints[3].position, true, randIncome.ToString()));
                PlayerInfo.Instance.MyMoney += randIncome;
                MonthlyReporter.Instance.m_NowMonth.IncomeSell += randIncome;
            }
            else
            {
                randIncome = Random.Range(20, 131);
                StartCoroutine(ClickEventManager.Instance.SPFadeOutText(StorePoints[3].position, true, randIncome.ToString()));
                PlayerInfo.Instance.SpecialPoint += randIncome;
            }
        }
        else
        {
            if (randomReward <= 70)
            {
                randIncome = Random.Range(300, 4001);
                StartCoroutine(ClickEventManager.Instance.MoneyFadeOutText(BookStorePoints[3].position, true, randIncome.ToString()));
                PlayerInfo.Instance.MyMoney += randIncome;
                MonthlyReporter.Instance.m_NowMonth.IncomeSell += randIncome;
            }
            else
            {
                randIncome = Random.Range(20, 131);
                StartCoroutine(ClickEventManager.Instance.SPFadeOutText(BookStorePoints[3].position, true, randIncome.ToString()));
                PlayerInfo.Instance.SpecialPoint += randIncome;
            }
        }
    }

    public int EnterStudyRoom()
    {
        // 좌석 최대치 초과
        if (FacilityList[2].StudentCount > 12)
        {
            return -1;
        }

        int seatNum = FacilityList[2].PointNum;
        FacilityList[2].PointNum++;
        FacilityList[2].StudentCount++;
        if (FacilityList[2].PointNum >= 12)
        {
            FacilityList[2].PointNum = 0;
        }

        return seatNum;
    }

    public void ExitStudyRoom()
    {
        FacilityList[2].StudentCount--;
    }

    public int EnterLounge(int loungeNum)
    {
        if (loungeNum == 1)
        {
            if (FacilityList[3].StudentCount > 12)
            {
                return -1;
            }

            int seatNum = FacilityList[3].PointNum;
            FacilityList[3].PointNum++;
            FacilityList[3].StudentCount++;
            if (FacilityList[3].PointNum >= 12)
            {
                FacilityList[3].PointNum = 0;
            }

            return seatNum;
        }
        else if (loungeNum == 2)
        {
            if (FacilityList[4].StudentCount > 10)
            {
                return -1;
            }

            int seatNum = FacilityList[4].PointNum;
            FacilityList[4].PointNum++;
            FacilityList[4].StudentCount++;
            if (FacilityList[4].PointNum >= 10)
            {
                FacilityList[4].PointNum = 0;
            }

            return seatNum;
        }

        return -1;
    }

    public void ExitLounge(int loungeNum)
    {
        FacilityList[2 + loungeNum].StudentCount--;
    }

    public void UseObject(int spot, int objectNum)
    {
        if (spot == (int)SpotName.VendingMachine)
        {
            VendingMachineList[objectNum].IsUsed = true;
            VendingMachineList[objectNum].UseCount++;
        }
        else if (spot == (int)SpotName.Pot)
        {
            PotList[objectNum].IsUsed = true;
            PotList[objectNum].UseCount++;
        }
        else if (spot == (int)SpotName.AmusementMachine)
        {
            AmusementMachineList[objectNum].IsUsed = true;
            AmusementMachineList[objectNum].UseCount++;
        }
        else if (spot == (int)SpotName.WaterPurifier)
        {
            WaterPurifierList[objectNum].IsUsed = true;
            WaterPurifierList[objectNum].UseCount++;
        }
        else if (spot == (int)SpotName.NoticeBoard)
        {
            NoticeBoardList[objectNum].IsUsed = true;
            NoticeBoardList[objectNum].UseCount++;
        }
    }

    public void EndUseObject(int spot, int objectNum)
    {
        if (spot == (int)SpotName.VendingMachine)
        {
            VendingMachineList[objectNum].IsUsed = false;
        }
        else if (spot == (int)SpotName.Pot)
        {
            PotList[objectNum].IsUsed = false;
        }
        else if (spot == (int)SpotName.AmusementMachine)
        {
            AmusementMachineList[objectNum].IsUsed = false;
        }
        else if (spot == (int)SpotName.WaterPurifier)
        {
            WaterPurifierList[objectNum].IsUsed = false;
        }
        else if (spot == (int)SpotName.NoticeBoard)
        {
            NoticeBoardList[objectNum].IsUsed = false;
        }
    }

    public void RandomScript(int room, Transform studentPos, bool isMinimumStat, bool isProfessor = false)
    {
        int randomSelect = 0;

        switch (room)
        {
            case (int)SpotName.PuzzleRoom:
                if (isMinimumStat)
                {
                    randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                        ShowScripts(room, m_scriptsManager.GenreRoomScripts[room].Count - 1, studentPos);
                    else
                        ShowEmoticon(0, studentPos);
                }
                else
                {
                    randomSelect = Random.Range(0, m_scriptsManager.GenreRoomScripts[room].Count);
                    if (randomSelect < m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowScripts(room, randomSelect, studentPos);
                    else if (randomSelect == m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowEmoticon(4, studentPos);
                }
                break;

            case (int)SpotName.SimulationRoom:
                if (isMinimumStat)
                {
                    randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                        ShowScripts(room, m_scriptsManager.GenreRoomScripts[room].Count - 1, studentPos);
                    else
                        ShowEmoticon(0, studentPos);
                }
                else
                {
                    randomSelect = Random.Range(0, m_scriptsManager.GenreRoomScripts[room].Count);
                    if (randomSelect < m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowScripts(room, randomSelect, studentPos);
                    else if (randomSelect == m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowEmoticon(6, studentPos);
                }
                break;

            case (int)SpotName.RhythmRoom:
                if (isMinimumStat)
                {
                    randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                        ShowScripts(room, m_scriptsManager.GenreRoomScripts[room].Count - 1, studentPos);
                    else
                        ShowEmoticon(0, studentPos);
                }
                else
                {
                    randomSelect = Random.Range(0, m_scriptsManager.GenreRoomScripts[room].Count);
                    if (randomSelect < m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowScripts(room, randomSelect, studentPos);
                    else if (randomSelect == m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowEmoticon(7, studentPos);
                }
                break;

            case (int)SpotName.AdventureRoom:
                if (isMinimumStat)
                {
                    randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                        ShowScripts(room, m_scriptsManager.GenreRoomScripts[room].Count - 1, studentPos);
                    else
                        ShowEmoticon(0, studentPos);
                }
                else
                {
                    randomSelect = Random.Range(0, m_scriptsManager.GenreRoomScripts[room].Count);
                    if (randomSelect < m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowScripts(room, randomSelect, studentPos);
                    else if (randomSelect == m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowEmoticon(6, studentPos);
                }
                break;

            case (int)SpotName.RPGRoom:
                if (isMinimumStat)
                {
                    randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                        ShowScripts(room, m_scriptsManager.GenreRoomScripts[room].Count - 1, studentPos);
                    else
                        ShowEmoticon(0, studentPos);
                }
                else
                {
                    randomSelect = Random.Range(0, m_scriptsManager.GenreRoomScripts[room].Count);
                    if (randomSelect < m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowScripts(room, randomSelect, studentPos);
                    else if (randomSelect == m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowEmoticon(3, studentPos);
                }
                break;

            case (int)SpotName.SportsRoom:
                if (isMinimumStat)
                {
                    randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                        ShowScripts(room, m_scriptsManager.GenreRoomScripts[room].Count - 1, studentPos);
                    else
                        ShowEmoticon(1, studentPos);
                }
                else
                {
                    randomSelect = Random.Range(0, m_scriptsManager.GenreRoomScripts[room].Count);
                    if (randomSelect < m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowScripts(room, randomSelect, studentPos);
                    else if (randomSelect == m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowEmoticon(3, studentPos);
                }
                break;

            case (int)SpotName.ActionRoom:
                if (isMinimumStat)
                {
                    randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                        ShowScripts(room, m_scriptsManager.GenreRoomScripts[room].Count - 1, studentPos);
                    else
                        ShowEmoticon(1, studentPos);
                }
                else
                {
                    randomSelect = Random.Range(0, m_scriptsManager.GenreRoomScripts[room].Count);
                    if (randomSelect < m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowScripts(room, randomSelect, studentPos);
                    else if (randomSelect == m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowEmoticon(6, studentPos);
                }
                break;

            case (int)SpotName.ShootingRoom:
                if (isMinimumStat)
                {
                    randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                        ShowScripts(room, m_scriptsManager.GenreRoomScripts[room].Count - 1, studentPos);
                    else
                        ShowEmoticon(1, studentPos);
                }
                else
                {
                    randomSelect = Random.Range(0, m_scriptsManager.GenreRoomScripts[room].Count);
                    if (randomSelect < m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowScripts(room, randomSelect, studentPos);
                    else if (randomSelect == m_scriptsManager.GenreRoomScripts[room].Count - 1)
                        ShowEmoticon(6, studentPos);
                }
                break;

            case (int)SpotName.Store:
                randomSelect = Random.Range(m_scriptsManager.FacilityScripts[room - 10].Count - 2, m_scriptsManager.FacilityScripts[room - 10].Count);
                ShowScripts(room, randomSelect, studentPos);
                break;

            case (int)SpotName.BookStore:
                randomSelect = Random.Range(m_scriptsManager.FacilityScripts[room - 10].Count - 2, m_scriptsManager.FacilityScripts[room - 10].Count);
                ShowScripts(room, randomSelect, studentPos);
                break;

            case (int)SpotName.StudyRoom:
                randomSelect = Random.Range(0, m_scriptsManager.FacilityScripts[room - 10].Count + 2);
                if (randomSelect < m_scriptsManager.FacilityScripts[room - 10].Count)
                    ShowScripts(room, randomSelect, studentPos);
                else if (randomSelect == m_scriptsManager.FacilityScripts[room - 10].Count)
                    ShowEmoticon(2, studentPos);
                else if (randomSelect == m_scriptsManager.FacilityScripts[room - 10].Count + 1)
                    ShowEmoticon(9, studentPos);
                break;

            case (int)SpotName.Lounge1:
                randomSelect = Random.Range(0, m_scriptsManager.FacilityScripts[room - 10].Count + 2);
                if (randomSelect < m_scriptsManager.FacilityScripts[room - 10].Count)
                    ShowScripts(room, randomSelect, studentPos);
                else if (randomSelect == m_scriptsManager.FacilityScripts[room - 10].Count)
                    ShowEmoticon(3, studentPos);
                else if (randomSelect == m_scriptsManager.FacilityScripts[room - 10].Count + 1)
                    ShowEmoticon(9, studentPos);
                break;

            case (int)SpotName.Lounge2:
                randomSelect = Random.Range(0, m_scriptsManager.FacilityScripts[room - 10].Count + 2);
                if (randomSelect < m_scriptsManager.FacilityScripts[room - 10].Count)
                    ShowScripts(room, randomSelect, studentPos);
                else if (randomSelect == m_scriptsManager.FacilityScripts[room - 10].Count)
                    ShowEmoticon(3, studentPos);
                else if (randomSelect == m_scriptsManager.FacilityScripts[room - 10].Count + 1)
                    ShowEmoticon(9, studentPos);
                break;

            case (int)SpotName.VendingMachine:
                randomSelect = Random.Range(0, m_scriptsManager.ObjectScripts[room - 20].Count + 1);
                if (randomSelect < m_scriptsManager.ObjectScripts[room - 20].Count)
                    ShowScripts(room, randomSelect, studentPos);
                else if (randomSelect == m_scriptsManager.ObjectScripts[room - 20].Count)
                    ShowEmoticon(3, studentPos);
                break;

            case (int)SpotName.Pot:
                randomSelect = Random.Range(0, m_scriptsManager.ObjectScripts[room - 20].Count); // 이모지 + 1, 클릭이벤트 - 1
                if (randomSelect < m_scriptsManager.ObjectScripts[room - 20].Count - 1)
                    ShowScripts(room, randomSelect, studentPos);
                else if (randomSelect == m_scriptsManager.ObjectScripts[room - 20].Count - 1)
                    ShowEmoticon(4, studentPos);
                break;

            case (int)SpotName.AmusementMachine:
                randomSelect = Random.Range(0, m_scriptsManager.ObjectScripts[room - 20].Count + 1);
                if (randomSelect < m_scriptsManager.ObjectScripts[room - 20].Count)
                    ShowScripts(room, randomSelect, studentPos);
                else if (randomSelect == m_scriptsManager.ObjectScripts[room - 20].Count)
                    ShowEmoticon(3, studentPos);
                break;

            case (int)SpotName.WaterPurifier:
                randomSelect = Random.Range(0, m_scriptsManager.ObjectScripts[room - 20].Count + 1);
                if (randomSelect < m_scriptsManager.ObjectScripts[room - 20].Count)
                    ShowScripts(room, randomSelect, studentPos);
                else if (randomSelect == m_scriptsManager.ObjectScripts[room - 20].Count)
                    ShowEmoticon(3, studentPos);
                break;

            case (int)SpotName.NoticeBoard:
                if (isProfessor)
                {
                    if (GameTime.Instance.Month != 2)
                    {
                        randomSelect = Random.Range(0, m_scriptsManager.ProNoticeBoardScripts[0].Count);
                        ShowScripts(room, randomSelect, studentPos, true);
                    }
                    else
                    {
                        randomSelect = Random.Range(0, m_scriptsManager.ProNoticeBoardScripts[1].Count);
                        ShowScripts(room, randomSelect, studentPos, true , 1);
                    }
                }
                else
                {
                    if (GameTime.Instance.Month != 2)
                    {
                        randomSelect = Random.Range(0, m_scriptsManager.ObjectScripts[room - 20].Count);
                        ShowScripts(room, randomSelect, studentPos);
                    }
                    else
                    {
                        randomSelect = Random.Range(0, m_scriptsManager.ObjectScripts[room - 20 + 1].Count);
                        ShowScripts(room + 1, randomSelect, studentPos);
                    }
                }
                break;

            default:
                Debug.Log("특수상호작용 오류");
                break;
        }
    }

    public void RandomProfScript(int room, Transform professorPos)
    {
        int randomSelect = 0;

        switch (room)
        {
            case (int)SpotName.Store:
                randomSelect = Random.Range(m_scriptsManager.FacilityScripts[room - 10].Count - 2, m_scriptsManager.FacilityScripts[room - 10].Count);
                ShowScripts(room, randomSelect, professorPos);
                break;

            case (int)SpotName.BookStore:
                randomSelect = Random.Range(m_scriptsManager.FacilityScripts[room - 10].Count - 2, m_scriptsManager.FacilityScripts[room - 10].Count);
                ShowScripts(room, randomSelect, professorPos);
                break;
        }
    }

    public void ShowEmoticon(int num, Transform studentPos)
    {
        int nowBoxNum = m_emoNum;
        m_emoticonBoxList[nowBoxNum].GetComponent<Image>().sprite = m_scriptsManager.Emoticons[num];
        m_emoticonBoxList[nowBoxNum].GetComponent<FollowTarget>().m_Target = studentPos;
        m_emoticonBoxList[nowBoxNum].SetActive(true);
        m_emoNum++;
        if (m_emoNum >= 20)
            m_emoNum = 0;

        StartCoroutine(HideEmoticon(nowBoxNum));
    }

    IEnumerator HideEmoticon(int num)
    {
        yield return new WaitForSeconds(1f);

        m_emoticonBoxList[num].SetActive(false);
    }

    public void ShowScripts(int room, int scriptNum, Transform studentPos, bool isProfessor = false, int noticeBoard = 0)
    {
        int nowBoxNum = m_chatNum;
        if (room < 10)
        {
            m_chatBoxList[nowBoxNum].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                m_scriptsManager.GenreRoomScripts[room][scriptNum];
        }
        else if (room < 20)
        {
            m_chatBoxList[nowBoxNum].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                m_scriptsManager.FacilityScripts[room - 10][scriptNum];
        }
        else
        {
            if (isProfessor)
            {
                m_chatBoxList[nowBoxNum].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    m_scriptsManager.ProNoticeBoardScripts[noticeBoard][scriptNum];
            }
            else
            {
                m_chatBoxList[nowBoxNum].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    m_scriptsManager.ObjectScripts[room - 20][scriptNum];
            }
        }
        m_chatBoxList[nowBoxNum].GetComponent<FollowTarget>().m_Target = studentPos;
        m_chatBoxList[nowBoxNum].SetActive(true);
        m_chatNum++;
        if (m_chatNum >= 20)
            m_chatNum = 0;

        StartCoroutine(HideChat(nowBoxNum));
    }

    IEnumerator HideChat(int num)
    {
        yield return new WaitForSeconds(1f);

        m_chatBoxList[num].SetActive(false);
    }

    public void ShowClassScript(StudentType studentType, Transform studentPos, bool isProfessor = false)
    {
        int nowBoxNum = m_chatNum;

        int randomShow = Random.Range(1, 3);

        if (randomShow == 1)
        {
            int randomScript;
            if (isProfessor)
            {
                randomScript = Random.Range(0, ScriptsManager.Instance.ProfScripts[(int)studentType].Count - 3);
                m_chatBoxList[nowBoxNum].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ScriptsManager.Instance.ProfScripts[(int)studentType][randomScript];
            }
            else
            {
                randomScript = Random.Range(0, ScriptsManager.Instance.ClassScripts[(int)studentType].Count);
                m_chatBoxList[nowBoxNum].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ScriptsManager.Instance.ClassScripts[(int)studentType][randomScript];
            }

            m_chatBoxList[nowBoxNum].GetComponent<FollowTarget>().m_Target = studentPos;
            m_chatBoxList[nowBoxNum].SetActive(true);
            m_chatNum++;

            if (m_chatNum >= 20)
                m_chatNum = 0;

            StartCoroutine(HideChat(nowBoxNum));
        }
        else
        {
            int randomEmoticon = Random.Range(0, 10);
            ShowEmoticon(randomEmoticon, studentPos);
        }
    }

    public void GenreRoomLevelUp(int genreRoom)
    {
        GenreRoomList[genreRoom].Level++;
        PlayerInfo.Instance.SpecialPoint -= 2000;

#if UNITY_EDITOR || UNITY_EDITOR_WIN
        Camera.main.GetComponent<TestCamera>().IsFixed = true;
        Camera.main.GetComponent<TestCamera>().FixedObject =
            GenreRoomCenters[genreRoom].gameObject;
        //effect.transform.position = GenreRoomCenters[genreRoom].position;
        //effect.transform.LookAt(Camera.main.transform.position);
        StartCoroutine(ActiveObject(genreRoom));
#elif UNITY_ANDROID
        Camera.main.GetComponent<InGameCamera>().IsFixed = true;
        Camera.main.GetComponent<InGameCamera>().FixedObject =
            GenreRoomCenters[genreRoom].gameObject;
        StartCoroutine(ActiveObject(genreRoom));
#endif
    }

    IEnumerator ActiveObject(int genreRoom)
    {
        yield return new WaitForSeconds(1.0f);

        ClickEventManager.Instance.Sound.PlayGenreRoomUpgradeSound();
        //이펙트 재생
        GameObject effect = Instantiate(UpgradeEffectPrefab, this.transform);
        //effect.transform.SetParent(GameObject.Find("ClickEvents").transform);
        effect.transform.position = GenreRoomCenters[genreRoom].position;
        Destroy(effect, 2f);
        GenreRoomUpgrade[genreRoom].SetActive(true);

        StartCoroutine(ResetCamera());
    }

    IEnumerator ResetCamera()
    {
        yield return new WaitForSeconds(2.0f);

#if UNITY_EDITOR || UNITY_EDITOR_WIN
        Camera.main.GetComponent<TestCamera>().ResetCameraPosition();
#elif UNITY_ANDROID
        Camera.main.GetComponent<InGameCamera>().ResetCameraPosition();
#endif
    }
}
