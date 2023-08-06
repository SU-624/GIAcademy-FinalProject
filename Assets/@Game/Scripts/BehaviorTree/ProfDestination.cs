using System.Collections;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TMPro;
using UnityEngine.AI;


public class ProfDestination : Action
{
    private string m_NowDestination = " ";
    private Category m_goingCategory;
    private bool m_isWaiting = false;
    private bool m_isTalking = false;

    private Transform m_canvas;

    private const int IdlePercent = 20;

    private const float DesChangeDis = 2;
    private const float DesStoreDis = 1;

    private NavMeshAgent m_Agent;
    private Animator m_Animator;

    private bool m_IsCoroutineRunning = false;
    private bool m_NowTalking = false;

    private IEnumerator m_MoveTalk;
    public override void OnStart()
    {
        m_canvas = GameObject.Find("InGameCanvas").transform;
        m_Agent = gameObject.GetComponent<NavMeshAgent>();
        m_Animator = gameObject.GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        bool _bNewDestination = false;

        if (InGameTest.Instance == null)
        {
            return TaskStatus.Failure;
        }

        if (InGameTest.Instance.m_ClassState != ClassState.nothing)
        {
            return TaskStatus.Failure;
        }

        if (gameObject.GetComponent<Instructor>().DoingValue != Instructor.Doing.FreeWalk)
        {
            return TaskStatus.Failure;
        }

        if (!PlayerInfo.Instance.IsFirstAcademySetting)
        {
            return TaskStatus.Failure;
        }

        // 2월 3주 방학
        if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 3 && GameTime.Instance.FlowTime.NowDay == 5)
        {
            _bNewDestination = SetVacation();
        }
        // 2월 4주 방학 끝
        else if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 4 && GameTime.Instance.FlowTime.NowDay == 5)
        {
            m_NowDestination = " ";
            _bNewDestination = SetClassRoom();
        }
        else
        {
            // 갈 곳이 정해지지 않은경우
            if (m_NowDestination == " ")
            {
                int randGoing = Random.Range(1, 101);

                // 시설(서점, 자습실, 매점, 라운지)로 이동
                if (randGoing <= 20)
                {
                    _bNewDestination = SetFacility();
                }
                // 교실로 이동
                else if (randGoing <= 80)
                {
                    _bNewDestination = SetClassRoom();
                }
                // 오브젝트(정수기, 자판기, 소파, 의자, 화분)로 이동
                else if (randGoing <= 90)
                {
                    _bNewDestination = SetObject();
                }
            }
            // 가는 곳이 정해져 있는 경우
            else
            {
                switch (m_goingCategory)
                {
                    case Category.Facility:
                        _bNewDestination = SetFacility();
                        break;

                    case Category.ClassRoom:
                        _bNewDestination = SetClassRoom();
                        break;

                    case Category.Object:
                        _bNewDestination = SetObject();
                        break;

                    default:
                        Debug.Log("목적지 설정 오류");
                        break;
                }
            }
        }
       

        return _bNewDestination ? TaskStatus.Success : TaskStatus.Failure;
    }

    private bool SetVacation()
    {
        m_goingCategory = Category.Vacation;

        if (m_NowDestination == "AcademyEntrance")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                m_NowDestination = "VacationPoint";
                int randomPoint = Random.Range(1, 4);
                Vector3 pointPos = GameObject.Find("VacationPoint" + randomPoint).transform.position;
                m_Agent.SetDestination(pointPos);
                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;

                if (!m_IsCoroutineRunning)
                {
                    m_MoveTalk = VacationMoveTalk();
                    StartCoroutine(m_MoveTalk);
                }
            }
            return true;

        }
        else if (m_NowDestination == "VacationPoint")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                if (m_MoveTalk != null)
                    StopCoroutine(m_MoveTalk);

                m_Animator.SetTrigger("ToIdle");
                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.Vacation;
            }
            return true;
        }
        else
        {
            m_Animator.SetTrigger("ToWalk");
            m_NowDestination = "AcademyEntrance";
            Vector3 entrancePos = GameObject.Find("AcademyEntrance").transform.position;
            m_Agent.SetDestination(entrancePos);
            gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;

            if (!m_IsCoroutineRunning)
            {
                int randomTalk = Random.Range(1, 19);
                if (randomTalk <= 6)
                {
                    m_IsCoroutineRunning = true;
                    StartCoroutine(VacationTalk());
                }
            }

            return true;
        }
    }

    bool SetFacility()
    {
        string entrance = " ";
        if (m_NowDestination.Length > 8)
        {
            entrance = m_NowDestination.Substring(m_NowDestination.Length - 8);
        }

        if (entrance == "Entrance")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                if (m_NowDestination == "StoreEntrance")
                {
                    int pointNum = InteractionManager.Instance.EnterStore((int)InteractionManager.SpotName.Store);
                    if (pointNum < 0)
                    {
                        return false;
                    }

                    m_Agent.SetDestination(InteractionManager.Instance.StorePoints[pointNum].position);
                    gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Store;
                    m_NowDestination = "StorePoint";
                    m_goingCategory = Category.Facility;
                    return true;
                }
                else if (m_NowDestination == "BookEntrance")
                {
                    int pointNum = InteractionManager.Instance.EnterStore((int)InteractionManager.SpotName.BookStore);
                    if (pointNum < 0)
                    {
                        return false;
                    }

                    m_Agent.SetDestination(InteractionManager.Instance.BookStorePoints[pointNum].position);
                    gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.BookStore;
                    m_NowDestination = "BookPoint";
                    m_goingCategory = Category.Facility;
                    return true;
                }
                else if (m_NowDestination == "1LoungeEntrance")
                {
                    int seatNum = InteractionManager.Instance.EnterLounge(1);
                    if (seatNum < 0)
                    {
                        ProfessorTalk("라운지 자리가 없네");
                        return false;
                    }

                    m_Agent.SetDestination(InteractionManager.Instance.LoungeSeats[seatNum].position);
                    gameObject.GetComponent<Instructor>().LookTransform = InteractionManager.Instance.LoungeSeats[seatNum];
                    gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Lounge1;
                    m_NowDestination = "1LoungeSeat";
                    m_goingCategory = Category.Facility;
                    return true;
                }
                else if (m_NowDestination == "2LoungeEntrance")
                {
                    int seatNum = InteractionManager.Instance.EnterLounge(2);
                    if (seatNum < 0)
                    {
                        ProfessorTalk("라운지 자리가 없네");
                        return false;
                    }

                    m_Agent.SetDestination(InteractionManager.Instance.Lounge2Seats[seatNum].position);
                    gameObject.GetComponent<Instructor>().LookTransform = InteractionManager.Instance.Lounge2Seats[seatNum];
                    gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Lounge1;
                    m_NowDestination = "2LoungeSeat";
                    m_goingCategory = Category.Facility;
                    return true;
                }
            }
            else
            {
                if (m_Agent.velocity == Vector3.zero)
                {
                    m_Agent.SetDestination(desPos);
                }
            }
            return true;
        }

        if (m_NowDestination == "StorePoint")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesStoreDis)
            {
                int randomAnim = Random.Range(1, 3);
                if (randomAnim == 1)
                {
                    m_Animator.SetTrigger("ToIdle");
                }
                else
                {
                    m_Animator.SetTrigger("ToPointing");
                }

                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.Idle;
                m_Agent.isStopped = true;

                // 30퍼 확률로 이모티콘 또는 스크립트 출력
                int randomScript = Random.Range(1, 101);
                if (randomScript <= 20)
                {
                    int randomSelect = Random.Range(0,
                        ScriptsManager.Instance.FacilityScripts[(int)InteractionManager.SpotName.Store - 10].Count - 2);
                    StoreTalk(ScriptsManager.Instance.FacilityScripts[(int)InteractionManager.SpotName.Store - 10][randomSelect]);
                }
                else if (randomScript <= 30)
                {
                    int randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                    {
                        InteractionManager.Instance.ShowEmoticon(6, this.transform);
                    }
                    else
                    {
                        InteractionManager.Instance.ShowEmoticon(3, this.transform);
                    }
                }

                StartCoroutine(ChooseSomething());
            }
            return true;
        }

        else if (m_NowDestination == "StoreCounter")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesStoreDis)
            {
                m_Agent.isStopped = true;

                if (InteractionManager.Instance.FacilityList[0].IsCalculating)
                {
                    gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.Idle;
                    int waitingNum = InteractionManager.Instance.StoreWaitingQ.Count;
                    InteractionManager.Instance.StoreWaitingQ.Add(this.gameObject);
                    if (waitingNum >= 3)
                        waitingNum = 2;

                    m_Animator.SetTrigger("ToIdle");
                    m_Agent.SetDestination(InteractionManager.Instance.StorePoints[4 + waitingNum].position);
                    m_Agent.isStopped = true;
                }
                else
                {
                    gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.InFacility;
                    gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Store;
                    m_NowDestination = " ";
                    m_goingCategory = Category.Nothing;
                }
            }
            return true;
        }

        else if (m_NowDestination == "BookPoint")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesStoreDis)
            {
                int randomAnim = Random.Range(1, 3);
                if (randomAnim == 1)
                {
                    m_Animator.SetTrigger("ToIdle");
                }
                else
                {
                    m_Animator.SetTrigger("ToPointing");
                }

                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.Idle;
                m_Agent.isStopped = true;

                // 30퍼 확률로 이모티콘 또는 스크립트 출력
                int randomScript = Random.Range(1, 101);
                if (randomScript <= 20)
                {
                    int randomSelect = Random.Range(0,
                        ScriptsManager.Instance.FacilityScripts[(int)InteractionManager.SpotName.BookStore - 10].Count - 2);
                    StoreTalk(ScriptsManager.Instance.FacilityScripts[(int)InteractionManager.SpotName.BookStore - 10][randomSelect]);
                }
                else if (randomScript <= 30)
                {
                    int randomSelect = Random.Range(1, 3);
                    if (randomSelect == 1)
                    {
                        InteractionManager.Instance.ShowEmoticon(2, this.transform);
                    }
                    else
                    {
                        InteractionManager.Instance.ShowEmoticon(8, this.transform);
                    }
                }

                StartCoroutine(ChooseBook());
            }
            return true;
        }

        else if (m_NowDestination == "BookCounter")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesStoreDis)
            {
                m_Agent.isStopped = true;

                if (InteractionManager.Instance.FacilityList[1].IsCalculating)
                {
                    gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.Idle;
                    int waitingNum = InteractionManager.Instance.BookStoreWaitingQ.Count;
                    InteractionManager.Instance.BookStoreWaitingQ.Add(this.gameObject);
                    if (waitingNum >= 3)
                        waitingNum = 2;

                    m_Animator.SetTrigger("ToIdle");
                    m_Agent.SetDestination(InteractionManager.Instance.BookStorePoints[4 + waitingNum].position);
                    m_Agent.isStopped = true;
                }
                else
                {
                    gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.InFacility;
                    gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.BookStore;
                    m_NowDestination = " ";
                    m_goingCategory = Category.Nothing;
                }
            }
            return true;
        }

        else if (m_NowDestination == "1LoungeSeat")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesStoreDis)
            {
                m_Agent.isStopped = true;
                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.InFacility;
                gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Lounge1;
                m_NowDestination = " ";
                m_goingCategory = Category.Nothing;
            }
            return true;
        }

        else if (m_NowDestination == "2LoungeSeat")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesStoreDis)
            {
                m_Agent.isStopped = true;
                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.InFacility;
                gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Lounge2;
                m_NowDestination = " ";
                m_goingCategory = Category.Nothing;
            }
            return true;
        }

        else
        {
            // 시설 3종류 랜덤
            float randomGoing = Random.Range(1f, 100f);
            // 매점 이동
            if (randomGoing <= 50 - (100 - gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth) / 2f)
            {
                m_NowDestination = "StoreEntrance";
                m_goingCategory = Category.Facility;
                m_Agent.SetDestination(InteractionManager.Instance.StoreEntrance.position);
            }
            // 서점 이동
            else if (randomGoing <= 100 - (100 - gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth))
            {
                m_NowDestination = "BookEntrance";
                m_Agent.SetDestination(InteractionManager.Instance.BookStoreEntrance.position);
            }
            // 라운지 이동
            else
            {
                int randomLounge = Random.Range(1, 3);
                m_NowDestination = randomLounge + "LoungeEntrance";
                if (randomLounge == 1)
                {
                    m_Agent.SetDestination(InteractionManager.Instance.LoungeEntrance.position);
                }
                else
                {
                    m_Agent.SetDestination(InteractionManager.Instance.Lounge2Entrance.position);
                }
            }
            m_goingCategory = Category.Facility;
            m_Animator.SetTrigger("ToWalk");
            gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
            return true;
        }
    }

    bool SetClassRoom()
    {
        m_goingCategory = Category.ClassRoom;

        if (m_NowDestination == " ")
        {
            m_NowDestination = "ClassSeat";
            Vector3 seatPos = (Vector3)gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable("MyClassSeat").GetValue();
            m_Animator.SetTrigger("ToWalk");
            m_Agent.SetDestination(seatPos);
            gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;

            return true;
        }
        else
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                int randomScript;

                int scriptsPlay = Random.Range(1, 101);

                int randomAnim = Random.Range(1, 4);
                if (randomAnim == 1)
                {
                    // 아트강사는 LookAround 애니메이션이 2개이다.
                    if (gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorType == StudentType.Art)
                    {
                        int randomLook = Random.Range(1, 3);
                        m_Animator.SetTrigger("ToLookAround" + randomLook);
                    }
                    else
                    {
                        m_Animator.SetTrigger("ToLookAround");
                    }
                }
                else if (randomAnim == 2)
                {
                    m_Animator.SetTrigger("ToThinking");
                }
                else if (randomAnim == 3)
                {
                    // 그냥 대기
                    m_Animator.SetTrigger("ToIdle");
                }

                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.Idle;

                m_Agent.isStopped = true;
                if (scriptsPlay <= 30)
                {
                    if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek != 4)
                    {
                        randomScript = Random.Range(1, 8);
                        ProfessorTalk(ScriptsManager.Instance.ProfScripts[(int)gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorType + 3][randomScript]);
                    }
                    else
                    {
                        randomScript = Random.Range(5, 8);
                        ProfessorTalk(ScriptsManager.Instance.ProfScripts[(int)gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorType][randomScript]);
                    }
                }
                else
                {
                    StartCoroutine(ProfessorWait());
                }
            }
            return true;
        }
    }


    bool SetObject()
    {
        m_goingCategory = Category.Object;

        if (m_NowDestination == "VendingMachine")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.ObjectInteraction;
                gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.VendingMachine;
                m_NowDestination = " ";
                m_goingCategory = Category.Object;
            }
            return true;
        }
        else if (m_NowDestination == "Pot")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.ObjectInteraction;
                gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Pot;
                m_NowDestination = " ";
                m_goingCategory = Category.Object;
            }
            return true;
        }
        else if (m_NowDestination == "AmusementMachine")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.ObjectInteraction;
                gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.AmusementMachine;
                m_NowDestination = " ";
                m_goingCategory = Category.Object;
            }
            return true;
        }
        else if (m_NowDestination == "WaterPurifier")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.ObjectInteraction;
                gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.WaterPurifier;
                m_NowDestination = " ";
                m_goingCategory = Category.Object;
            }
            return true;
        }
        else if (m_NowDestination == "NoticeBoard")
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.ObjectInteraction;
                gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.NoticeBoard;
                m_NowDestination = " ";
                m_goingCategory = Category.Object;
            }
            return true;
        }
        else
        {
            int randomObject = Random.Range(0, 5);      // 5종류 오브젝트 중 랜덤 선택
            int randomSelect;

            Vector3 objectPoint = Vector3.zero;

            if (randomObject == 0)
            {
                randomSelect = Random.Range(0, InteractionManager.Instance.VendingMachines.Length);
                if (!InteractionManager.Instance.VendingMachineList[randomSelect].IsUsed)
                {
                    objectPoint = InteractionManager.Instance.VendingMachines[randomSelect].position;
                    gameObject.GetComponent<Instructor>().LookTransform = InteractionManager.Instance.VendingMachines[randomSelect];
                    m_NowDestination = "VendingMachine";
                    InteractionManager.Instance.UseObject((int)InteractionManager.SpotName.VendingMachine, randomSelect);
                    gameObject.GetComponent<Instructor>().UsingObjNum = randomSelect;
                }
            }
            else if (randomObject == 1)
            {
                randomSelect = Random.Range(0, InteractionManager.Instance.Pots.Length);
                if (!InteractionManager.Instance.PotList[randomSelect].IsUsed)
                {
                    objectPoint = InteractionManager.Instance.Pots[randomSelect].position;
                    gameObject.GetComponent<Instructor>().LookTransform = InteractionManager.Instance.Pots[randomSelect];
                    m_NowDestination = "Pot";
                    InteractionManager.Instance.UseObject((int)InteractionManager.SpotName.Pot, randomSelect);
                    gameObject.GetComponent<Instructor>().UsingObjNum = randomSelect;
                }
            }
            else if (randomObject == 2)
            {
                randomSelect = Random.Range(0, InteractionManager.Instance.AmusementMachines.Length);
                if (!InteractionManager.Instance.AmusementMachineList[randomSelect].IsUsed)
                {
                    objectPoint = InteractionManager.Instance.AmusementMachines[randomSelect].position;
                    gameObject.GetComponent<Instructor>().LookTransform = InteractionManager.Instance.AmusementMachines[randomSelect];
                    m_NowDestination = "AmusementMachine";
                    InteractionManager.Instance.UseObject((int)InteractionManager.SpotName.AmusementMachine, randomSelect);
                    gameObject.GetComponent<Instructor>().UsingObjNum = randomSelect;
                }
            }
            else if (randomObject == 3)
            {
                randomSelect = Random.Range(0, InteractionManager.Instance.WaterPurifier.Length);
                if (!InteractionManager.Instance.WaterPurifierList[randomSelect].IsUsed)
                {
                    objectPoint = InteractionManager.Instance.WaterPurifier[randomSelect].position;
                    gameObject.GetComponent<Instructor>().LookTransform = InteractionManager.Instance.WaterPurifier[randomSelect];
                    m_NowDestination = "WaterPurifier";
                    InteractionManager.Instance.UseObject((int)InteractionManager.SpotName.WaterPurifier, randomSelect);
                    gameObject.GetComponent<Instructor>().UsingObjNum = randomSelect;
                }
            }
            else if (randomObject == 4)
            {
                randomSelect = Random.Range(0, InteractionManager.Instance.NoticeBoard.Length);
                if (!InteractionManager.Instance.NoticeBoardList[randomSelect].IsUsed)
                {
                    objectPoint = InteractionManager.Instance.NoticeBoard[randomSelect].position;
                    gameObject.GetComponent<Instructor>().LookTransform = InteractionManager.Instance.NoticeBoard[randomSelect];
                    m_NowDestination = "NoticeBoard";
                    InteractionManager.Instance.UseObject((int)InteractionManager.SpotName.NoticeBoard, randomSelect);
                    gameObject.GetComponent<Instructor>().UsingObjNum = randomSelect;
                }
            }
            m_Agent.SetDestination(objectPoint);
            m_Animator.SetTrigger("ToWalk");
            gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;

            return true;
        }
        return false;
    }

    bool SetFree()
    {
        m_goingCategory = Category.Free;

        if (m_NowDestination == " ")
        {
            int randomFreePoint = Random.Range(0, InGameTest.Instance.FreeWalkPointList.Count);

            Vector3 resPoint = InGameTest.Instance.FreeWalkPointList[randomFreePoint];

            m_NowDestination = "FreeWalk" + (randomFreePoint + 1);
            m_Agent.SetDestination(resPoint);
            gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;

            m_Animator.SetTrigger("ToWalk");

            return true;
        }
        else
        {
            Vector3 myPos = gameObject.transform.position;
            Vector3 desPos = m_Agent.destination;

            float dis = Vector3.Distance(myPos, desPos);

            if (dis < DesChangeDis)
            {
                int randomAnim = Random.Range(1, 3);

                if (randomAnim == 1)
                {
                    m_Animator.SetTrigger("ToIdle");
                }
                else
                {
                    m_Animator.SetTrigger("ToStandingIdle");
                }

                int randomValue = Random.Range(0, 101);
                if (randomValue <= IdlePercent)
                {
                    gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.Idle;
                    m_Agent.isStopped = true;
                    StartCoroutine(ProfessorWait());
                }
                else
                {
                    gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
                    m_NowDestination = " ";
                    m_goingCategory = Category.Nothing;
                }
            }
            return true;
        }
    }

    private void StoreTalk(string sentence)
    {
        GameObject chat = InGameObjectPool.GetChatObject(m_canvas);

        chat.GetComponent<FollowTarget>().m_Target = gameObject.transform.GetChild(0).transform;
        chat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sentence;
        m_Agent.isStopped = true;
        
        StartCoroutine(StoreTalkEnd(chat));
    }

    IEnumerator StoreTalkEnd(GameObject chatBox)
    {
        yield return new WaitForSeconds(2f);

        InGameObjectPool.ReturnChatObject(chatBox);
    }

    private void ProfessorTalk(string sentence)
    {
        GameObject chat = InGameObjectPool.GetChatObject(m_canvas);

        chat.GetComponent<FollowTarget>().m_Target = gameObject.transform.GetChild(0).transform;
        chat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sentence;
        m_Agent.isStopped = true;
        m_NowDestination = " ";
        m_goingCategory = Category.Nothing;
        StartCoroutine(ProfessorTalkEnd(chat));
    }

    IEnumerator ProfessorTalkEnd(GameObject chatBox)
    {
        yield return new WaitForSeconds(2f);

        InGameObjectPool.ReturnChatObject(chatBox);
        m_Agent.isStopped = false;
        gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
    }

    IEnumerator ChooseSomething()
    {
        yield return new WaitForSeconds(2f);
        
        m_Animator.SetTrigger("ToWalk");
        m_Agent.isStopped = false;
        gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
        m_NowDestination = "StoreCounter";
        m_goingCategory = Category.Facility;
        m_Agent.SetDestination(InteractionManager.Instance.StorePoints[3].position);
        gameObject.GetComponent<Instructor>().LookTransform = InteractionManager.Instance.StorePoints[3];
    }

    IEnumerator ChooseBook()
    {
        yield return new WaitForSeconds(2f);

        m_Agent.isStopped = false;
        gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
        m_NowDestination = "BookCounter";
        m_goingCategory = Category.Facility;
        m_Agent.SetDestination(InteractionManager.Instance.BookStorePoints[3].position);
        gameObject.GetComponent<Instructor>().LookTransform = InteractionManager.Instance.BookStorePoints[3];
    }

    IEnumerator ProfessorWait()
    {
        yield return new WaitForSeconds(3f);

        m_Agent.isStopped = false;
        gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
        m_NowDestination = " ";
        m_goingCategory = Category.Nothing;
    }
    IEnumerator VacationTalk()
    {
        yield return new WaitForSeconds(2f);

        int randomScript = Random.Range(0, ScriptsManager.Instance.ProManVacationScripts[0].Count);

        GameObject chat = InGameObjectPool.GetChatObject(m_canvas);

        chat.GetComponent<FollowTarget>().m_Target = gameObject.transform.GetChild(0).transform;
        chat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ScriptsManager.Instance.ProManVacationScripts[0][randomScript];

        StartCoroutine(VacationTalkEnd(chat));
    }

    IEnumerator VacationTalkEnd(GameObject chat)
    {
        yield return new WaitForSeconds(2f);

        InGameObjectPool.ReturnChatObject(chat);
        m_IsCoroutineRunning = false;
    }

    IEnumerator VacationMoveTalk()
    {
        m_IsCoroutineRunning = true;

        while (true)
        {
            int random = Random.Range(1, 101);

            float randomCoolTime = Random.Range(1f, 3f);
            if (random <= 30 && !m_NowTalking)
            {
                int randomScript = Random.Range(0, ScriptsManager.Instance.ProManVacationScripts[1].Count);

                GameObject chat = InGameObjectPool.GetChatObject(m_canvas);

                chat.GetComponent<FollowTarget>().m_Target = gameObject.transform.GetChild(0).transform;
                chat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ScriptsManager.Instance.ProManVacationScripts[1][randomScript];
                m_NowTalking = true;

                StartCoroutine(VacationMoveTalkEnd(chat, randomCoolTime));
            }

            yield return new WaitForSeconds(randomCoolTime);
        }
    }

    IEnumerator VacationMoveTalkEnd(GameObject chat, float returnTime)
    {
        yield return new WaitForSeconds(returnTime);

        InGameObjectPool.ReturnChatObject(chat);
        m_NowTalking = false;
        m_IsCoroutineRunning = false;
    }

}