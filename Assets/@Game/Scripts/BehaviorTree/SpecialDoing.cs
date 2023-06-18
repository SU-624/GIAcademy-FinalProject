using System.Collections;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TMPro;
using UnityEngine.AI;

public class SpecialDoing : Action
{
    private bool doingSomething = false;
    private bool isMinimumStat = false;
    private Transform m_LookTransform;

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        int reward = 0;
        isMinimumStat = false;

        if (doingSomething == false)
        {
            switch (gameObject.GetComponent<Student>().NowRoom)
            {
                case (int)InteractionManager.SpotName.PuzzleRoom:
                    if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", true);
                    }
                    reward = RandomGenreRoomReward();
                    if (reward == 1)
                    {
                        PuzzleRoomStat();
                    }

                    m_LookTransform =
                        InteractionManager.Instance.PuzzleSeats[gameObject.GetComponent<Student>().m_NowSeatNum];
                    break;

                case (int)InteractionManager.SpotName.SimulationRoom:
                    if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", true);
                    }
                    reward = RandomGenreRoomReward();
                    if (reward == 1)
                    {
                        SimulationRoomStat();
                    }
                    m_LookTransform =
                        InteractionManager.Instance.SimulationSeats[gameObject.GetComponent<Student>().m_NowSeatNum];
                    break;

                case (int)InteractionManager.SpotName.RhythmRoom:
                    if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
                    }
                    reward = RandomGenreRoomReward();
                    if (reward == 1)
                    {
                        RhythmRoomStat();
                    }
                    m_LookTransform =
                        InteractionManager.Instance.RhythmSeats[gameObject.GetComponent<Student>().m_NowSeatNum];
                    break;

                case (int)InteractionManager.SpotName.AdventureRoom:
                    if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", true);
                    }
                    reward = RandomGenreRoomReward();
                    if (reward == 1)
                    {
                        AdventureRoomStat();
                    }
                    m_LookTransform =
                        InteractionManager.Instance.AdventureSeats[gameObject.GetComponent<Student>().m_NowSeatNum];
                    break;

                case (int)InteractionManager.SpotName.RPGRoom:
                    if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
                    }
                    reward = RandomGenreRoomReward();
                    if (reward == 1)
                    {
                        RPGRoomStat();
                    }
                    m_LookTransform =
                        InteractionManager.Instance.RPGSeats[gameObject.GetComponent<Student>().m_NowSeatNum];
                    break;

                case (int)InteractionManager.SpotName.SportsRoom:
                    if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", true);
                    }
                    reward = RandomGenreRoomReward();
                    if (reward == 1)
                    {
                        SportsRoomStat();
                    }
                    m_LookTransform =
                        InteractionManager.Instance.SportsSeats[gameObject.GetComponent<Student>().m_NowSeatNum];
                    break;

                case (int)InteractionManager.SpotName.ActionRoom:
                    if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
                    }
                    reward = RandomGenreRoomReward();
                    if (reward == 1)
                    {
                        ActionRoomStat();
                    }
                    m_LookTransform =
                        InteractionManager.Instance.ActionSeats[gameObject.GetComponent<Student>().m_NowSeatNum];
                    break;

                case (int)InteractionManager.SpotName.ShootingRoom:
                    if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
                    }
                    reward = RandomGenreRoomReward();
                    if (reward == 1)
                    {
                        ShootingRoomStat();
                    }
                    m_LookTransform =
                        InteractionManager.Instance.ShootingSeats[gameObject.GetComponent<Student>().m_NowSeatNum];
                    break;

                case (int)InteractionManager.SpotName.Store:
                    InteractionManager.Instance.FacilityList[0].IsCalculating = true;
                    RandomFacilityReward();
                    this.gameObject.GetComponent<Student>().m_StudentStat.m_Health -= 1;
                    break;

                case (int)InteractionManager.SpotName.BookStore:
                    InteractionManager.Instance.FacilityList[1].IsCalculating = true;
                    this.gameObject.GetComponent<Student>().m_StudentStat.m_Health -= 1;
                    this.gameObject.GetComponent<Student>().m_StudentStat.m_Passion -= 2;
                    RandomFacilityReward();
                    break;

                case (int)InteractionManager.SpotName.StudyRoom:
                    this.gameObject.GetComponent<Student>().m_StudentStat.m_Health -= 2;
                    this.gameObject.GetComponent<Student>().m_StudentStat.m_Passion -= 2;
                    RandomFacilityReward();
                    break;

                case (int)InteractionManager.SpotName.Lounge1:
                    RandomFacilityReward();
                    break;

                case (int)InteractionManager.SpotName.Lounge2:
                    RandomFacilityReward();
                    break;

                case (int)InteractionManager.SpotName.VendingMachine:
                    RandomObjectReward();
                    break;

                case (int)InteractionManager.SpotName.Pot:
                    RandomObjectReward();
                    break;

                case (int)InteractionManager.SpotName.AmusementMachine:
                    RandomObjectReward();
                    break;

                case (int)InteractionManager.SpotName.WaterPurifier:
                    RandomObjectReward();
                    break;

                case (int)InteractionManager.SpotName.NoticeBoard:
                    RandomObjectReward();
                    break;

                case (int)InteractionManager.SpotName.Nothing:
                    gameObject.GetComponent<Student>().DoingValue = Student.Doing.FreeWalk;
                    break;
            }

        }

        if (gameObject.GetComponent<Student>().DoingValue == Student.Doing.InGenreRoom)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_LookTransform.forward),
                Time.deltaTime * 10);
        }

        doingSomething = true;

        return TaskStatus.Success;
    }

    private int RandomObjectReward()
    {
        int spot = gameObject.GetComponent<Student>().NowRoom;

        InteractionManager.Instance.EndUseObject(spot, gameObject.GetComponent<Student>().UsingObjNum);

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }

        int randomEvent = Random.Range(1, 101);

        if (spot == (int)InteractionManager.SpotName.VendingMachine)
        {
            // 기본보상
            StartCoroutine(ResetObject(true));
            return 1;
        }
        else if (spot == (int)InteractionManager.SpotName.Pot)
        {
            if (randomEvent <= 20)
            {
                // 아무런 보상 없음
                StartCoroutine(ResetObject(false));
                return 0;
            }
            else if (randomEvent <= 80)
            {
                // 기본보상
                StartCoroutine(ResetObject(true));
                return 1;
            }
            else
            {
                // 터치 이벤트 발동
                StartCoroutine(ObjectClickEvent());
                return 2;
            }
        }
        else if (spot == (int)InteractionManager.SpotName.AmusementMachine)
        {
            if (randomEvent <= 20)
            {
                // 아무런 보상 없음
                StartCoroutine(ResetObject(false));
                return 0;
            }
            else if (randomEvent <= 80)
            {
                // 기본보상
                StartCoroutine(ResetObject(true));
                return 1;
            }
            else
            {
                // 터치 이벤트 발동
                StartCoroutine(ObjectClickEvent());
                return 2;
            }
        }
        else if (spot == (int)InteractionManager.SpotName.WaterPurifier)
        {
            if (randomEvent <= 20)
            {
                // 아무런 보상 없음
                StartCoroutine(ResetObject(false));
                return 0;
            }
            else
            {
                // 기본보상
                StartCoroutine(ResetObject(true));
                return 1;
            }
        }
        else if (spot == (int)InteractionManager.SpotName.NoticeBoard)
        {
            if (randomEvent <= 20)
            {
                // 아무런 보상 없음
                StartCoroutine(ResetObject(false));
                return 0;
            }
            else
            {
                // 기본보상
                StartCoroutine(ResetObject(true));
                return 1;
            }
        }
        else
        {
            return 0;
        }
    }

    private int RandomFacilityReward()
    {
        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }

        int randomEvent = Random.Range(1, 101);

        // 2월엔 자습실 보상이 없다.
        if (GameTime.Instance.FlowTime.NowMonth == 2 &&
            gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.StudyRoom)
        {
            // 아무런 보상 없음
            StartCoroutine(ResetFacility(false));
            return 0;
        }
        else
        {
            if (randomEvent <= 20)
            {
                // 아무런 보상 없음
                StartCoroutine(ResetFacility(false));
                return 0;
            }
            else if (randomEvent <= 80)
            {
                // 기본보상
                StartCoroutine(ResetFacility(true));
                return 1;
            }
            else
            {
                // 터치 이벤트 발동
                StartCoroutine(FacilityClickEvent());
                return 2;
            }
        }
    }

    private int RandomGenreRoomReward()
    {
        this.gameObject.GetComponent<Student>().m_StudentStat.m_Health -= 3;
        this.gameObject.GetComponent<Student>().m_StudentStat.m_Passion -= 1;

        int randomEvent = Random.Range(1, 101);

        if (randomEvent <= 20)
        {
            // 아무런 보상 없음
            StartCoroutine(ResetState());
            return 0;
        }
        else if (randomEvent <= 80)
        {
            // 기본보상
            StartCoroutine(ResetState());
            return 1;
        }
        else
        {
            // 터치 이벤트 발동
            StartCoroutine(GenreClickEvent());
            return 2;
        }
    }

    IEnumerator ResetObject(bool isGetReward)
    {
        yield return new WaitForSeconds(2f);

        if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.VendingMachine)
        {
            if (isGetReward)
            {
                int randomIncome = Random.Range(300, 1001);
                int randomPassion = Random.Range(3, 6);

                PlayerInfo.Instance.m_MyMoney += randomIncome;
                MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomIncome;
                gameObject.GetComponent<Student>().m_StudentStat.m_Passion += randomPassion;
            }
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.Pot)
        {
            if (isGetReward)
            {
                int randomPassion = Random.Range(4, 12);
                gameObject.GetComponent<Student>().m_StudentStat.m_Passion += randomPassion;
            }
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.AmusementMachine)
        {
            if (isGetReward)
            {
                // todo 게임기 보상 변경 해야함
                int randomInt = Random.Range(20, 41);
                gameObject.GetComponent<Student>().m_StudentStat.m_Health += randomInt;
                if (gameObject.GetComponent<Student>().m_StudentStat.m_Health >= 100)
                {
                    gameObject.GetComponent<Student>().m_StudentStat.m_Health = 100;
                }
                randomInt = Random.Range(500, 1001);
                PlayerInfo.Instance.m_MyMoney += randomInt;
                MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomInt;
            }
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.WaterPurifier)
        {
            if (isGetReward)
            {
                int randomHealth = Random.Range(3, 11);
                gameObject.GetComponent<Student>().m_StudentStat.m_Health += randomHealth;
                if (gameObject.GetComponent<Student>().m_StudentStat.m_Health >= 100)
                {
                    gameObject.GetComponent<Student>().m_StudentStat.m_Health = 100;
                }
            }
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.NoticeBoard)
        {
            if (isGetReward)
            {
                // todo 게시판 보상 추가 예정
            }
        }

        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("sittingIdel"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", false);
            gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
        }

        gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
        gameObject.GetComponent<Student>().m_IsCoolDown = true;
        //gameObject.GetComponent<Student>().InteractingObj = null;
        gameObject.GetComponent<Student>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        doingSomething = false;
    }

    IEnumerator ResetFacility(bool isGetReward)
    {
        yield return new WaitForSeconds(2f);

        if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.Store || gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.BookStore)
        {
            if (isGetReward)
            {
                InteractionManager.Instance.GetStoreReward(gameObject.GetComponent<Student>().NowRoom);
            }

            InteractionManager.Instance.ExitStore((int)gameObject.GetComponent<Student>().NowRoom);
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.StudyRoom)
        {
            if (isGetReward)
            {
                StudyRoomReward();
            }

            InteractionManager.Instance.ExitStudyRoom();
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.Lounge1)
        {
            if (isGetReward)
            {
                int randomInt = Random.Range(20, 41);
                gameObject.GetComponent<Student>().m_StudentStat.m_Health += randomInt;
                if (gameObject.GetComponent<Student>().m_StudentStat.m_Health >= 100)
                {
                    gameObject.GetComponent<Student>().m_StudentStat.m_Health = 100;
                }
                randomInt = Random.Range(15, 26);
                gameObject.GetComponent<Student>().m_StudentStat.m_Passion += randomInt;
                if (gameObject.GetComponent<Student>().m_StudentStat.m_Passion >= 100)
                {
                    gameObject.GetComponent<Student>().m_StudentStat.m_Passion = 100;
                }
            }

            InteractionManager.Instance.ExitLounge(1);
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.Lounge2)
        {
            if (isGetReward)
            {
                int randomInt = Random.Range(20, 41);
                gameObject.GetComponent<Student>().m_StudentStat.m_Health += randomInt;
                if (gameObject.GetComponent<Student>().m_StudentStat.m_Health >= 100)
                {
                    gameObject.GetComponent<Student>().m_StudentStat.m_Health = 100;
                }
                randomInt = Random.Range(15, 26);
                gameObject.GetComponent<Student>().m_StudentStat.m_Passion += randomInt;
                if (gameObject.GetComponent<Student>().m_StudentStat.m_Passion >= 100)
                {
                    gameObject.GetComponent<Student>().m_StudentStat.m_Passion = 100;
                }
            }

            InteractionManager.Instance.ExitLounge(2);
        }

        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("sittingIdel"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", false);
            gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
        }

        gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
        gameObject.GetComponent<Student>().m_IsCoolDown = true;
        //gameObject.GetComponent<Student>().InteractingObj = null;
        gameObject.GetComponent<Student>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        doingSomething = false;
    }

    IEnumerator ObjectClickEvent()
    {
        yield return new WaitForSeconds(2f);

        ClickEventManager.Instance.StudentSpecialEvent(this.gameObject.GetComponent<Student>(), this.gameObject.transform.position, ClickEventType.Object);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("sittingIdel"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", false);
            gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
        }

        gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
        gameObject.GetComponent<Student>().m_IsCoolDown = true;
        //gameObject.GetComponent<Student>().InteractingObj = null;
        gameObject.GetComponent<Student>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        doingSomething = false;
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
    }

    IEnumerator FacilityClickEvent()
    {
        yield return new WaitForSeconds(2f);

        ClickEventManager.Instance.StudentSpecialEvent(this.gameObject.GetComponent<Student>(), this.gameObject.transform.position, ClickEventType.Facility);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.Store ||
            gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.BookStore)
        {
            InteractionManager.Instance.ExitStore((int)gameObject.GetComponent<Student>().NowRoom);
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.StudyRoom)
        {
            InteractionManager.Instance.ExitStudyRoom();
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.Lounge1)
        {
            InteractionManager.Instance.ExitLounge(1);
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.Lounge2)
        {
            InteractionManager.Instance.ExitLounge(2);
        }

        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("sittingIdel"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", false);
            gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
        }
        gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
        gameObject.GetComponent<Student>().m_IsCoolDown = true;
        //gameObject.GetComponent<Student>().InteractingObj = null;
        gameObject.GetComponent<Student>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
        doingSomething = false;
    }

    IEnumerator ResetState()
    {
        yield return new WaitForSeconds(2f);

        InteractionManager.Instance.ExitRoom((InteractionManager.SpotName)gameObject.GetComponent<Student>().NowRoom);
        gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
        gameObject.GetComponent<Student>().m_IsCoolDown = true;
        //gameObject.GetComponent<Student>().InteractingObj = null;
        gameObject.GetComponent<Student>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("sittingIdel"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", false);
            gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
        }
        doingSomething = false;
    }

    IEnumerator GenreClickEvent()
    {
        yield return new WaitForSeconds(2f);

        ClickEventManager.Instance.StudentSpecialEvent(this.gameObject.GetComponent<Student>(), this.gameObject.transform.position, ClickEventType.GenreRoom);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        InteractionManager.Instance.ExitRoom((InteractionManager.SpotName)gameObject.GetComponent<Student>().NowRoom);
        gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
        gameObject.GetComponent<Student>().m_IsCoolDown = true;
        //gameObject.GetComponent<Student>().InteractingObj = null;
        gameObject.GetComponent<Student>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("sittingIdel"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToSit", false);
            gameObject.GetComponent<Animator>().SetBool("IsWalkToIdel", true);
        }
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        gameObject.transform.GetChild(3).gameObject.SetActive(true);
        doingSomething = false;
    }

    private void StudyRoomReward()
    {

        int randomReward = Random.Range(1, 101);
        int randIncome = 0;

        if (randomReward <= 25)
        {
            randIncome = Random.Range(10, 31);
            StartCoroutine(ClickEventManager.Instance.SPFadeOutText(transform.position, true, randIncome.ToString()));
            PlayerInfo.Instance.m_SpecialPoint += randIncome;
        }
        else if (randomReward <= 40)
        {
            randIncome = Random.Range(3, 7);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Sense, randIncome.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] += randIncome;
        }
        else if (randomReward <= 55)
        {
            randIncome = Random.Range(3, 7);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Technique, randIncome.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] += randIncome;
        }
        else if (randomReward <= 70)
        {
            randIncome = Random.Range(3, 7);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Insight, randIncome.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] += randIncome;
        }
        else if (randomReward <= 85)
        {
            randIncome = Random.Range(3, 7);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Concentration, randIncome.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randIncome;
        }
        else
        {
            randIncome = Random.Range(3, 7);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Wit, randIncome.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] += randIncome;
        }
    }

    private void RPGRoomStat()
    {
        int randomSelect = Random.Range(1, 101);
        int randomStat;

        if (randomSelect <= 80)
        {
            randomStat = Random.Range(6, 13);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, true, (int)GenreStat.RPG, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_GenreAmountList[(int)GenreStat.RPG] += randomStat;

            if (randomStat == 6)
                isMinimumStat = true;
        }
        else if (randomSelect <= 90)
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Sense, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }
        else
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Insight, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }
    }

    private void SportsRoomStat()
    {
        int randomSelect = Random.Range(1, 101);
        int randomStat;

        if (randomSelect <= 80)
        {
            randomStat = Random.Range(6, 13);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, true, (int)GenreStat.Sports, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_GenreAmountList[(int)GenreStat.Sports] += randomStat;

            if (randomStat == 6)
                isMinimumStat = true;
        }
        else if (randomSelect <= 90)
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Sense, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }
        else
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Technique, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }
    }

    private void SimulationRoomStat()
    {
        int randomSelect = Random.Range(1, 101);
        int randomStat;

        if (randomSelect <= 80)
        {
            randomStat = Random.Range(6, 13);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, true, (int)GenreStat.Simulation, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_GenreAmountList[(int)GenreStat.Simulation] += randomStat;

            if (randomStat == 6)
                isMinimumStat = true;
        }
        else if (randomSelect <= 90)
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Insight, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }
        else
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Wit, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }
    }

    private void ActionRoomStat()
    {
        int randomSelect = Random.Range(1, 101);
        int randomStat;

        if (randomSelect <= 80)
        {
            randomStat = Random.Range(6, 13);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, true, (int)GenreStat.Action, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_GenreAmountList[(int)GenreStat.Action] += randomStat;

            if (randomStat == 6)
                isMinimumStat = true;
        }
        else if (randomSelect <= 90)
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Concentration, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }
        else
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Technique, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }
    }

    private void RhythmRoomStat()
    {
        int randomSelect = Random.Range(1, 101);
        int randomStat;

        if (randomSelect <= 80)
        {
            randomStat = Random.Range(6, 13);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, true, (int)GenreStat.Rhythm, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_GenreAmountList[(int)GenreStat.Rhythm] += randomStat;

            if (randomStat == 6)
                isMinimumStat = true;
        }
        else if (randomSelect <= 90)
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Concentration, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }
        else
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Wit, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }
    }

    private void AdventureRoomStat()
    {
        int randomSelect = Random.Range(1, 101);
        int randomStat;

        if (randomSelect <= 80)
        {
            randomStat = Random.Range(6, 13);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, true, (int)GenreStat.Adventure, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_GenreAmountList[(int)GenreStat.Adventure] += randomStat;

            if (randomStat == 6)
                isMinimumStat = true;
        }
        else if (randomSelect <= 90)
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Wit, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }
        else
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Concentration, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }
    }

    private void ShootingRoomStat()
    {
        int randomSelect = Random.Range(1, 101);
        int randomStat;

        if (randomSelect <= 80)
        {
            randomStat = Random.Range(6, 13);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, true, (int)GenreStat.Shooting, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_GenreAmountList[(int)GenreStat.Shooting] += randomStat;

            if (randomStat == 6)
                isMinimumStat = true;
        }
        else if (randomSelect <= 95)
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Insight, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }
        else
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Concentration, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }
    }

    private void PuzzleRoomStat()
    {
        int randomSelect = Random.Range(1, 101);
        int randomStat;

        if (randomSelect <= 80)
        {
            randomStat = Random.Range(6, 13);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, true, (int)GenreStat.Puzzle, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_GenreAmountList[(int)GenreStat.Puzzle] += randomStat;

            if (randomStat == 6)
                isMinimumStat = true;
        }
        else if (randomSelect <= 90)
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Sense, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }
        else
        {
            randomStat = Random.Range(2, 5);
            StartCoroutine(ClickEventManager.Instance.StatBoxFadeOut(transform.position, false, (int)AbilityType.Technique, randomStat.ToString()));
            gameObject.GetComponent<Student>().m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique] += randomStat;

            if (randomStat == 2)
                isMinimumStat = true;
        }

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Student>().NowRoom, this.transform, isMinimumStat);
        }
    }
}
