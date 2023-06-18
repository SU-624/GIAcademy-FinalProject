using System.Collections;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class ProfSpecialDoing : Action
{
    private bool doingSomething = false;

    public override void OnStart()
	{
		
	}

	public override TaskStatus OnUpdate()
	{
        int reward = 0;

        if (doingSomething == false)
        {
            switch (gameObject.GetComponent<Instructor>().NowRoom)
            {
                case (int)InteractionManager.SpotName.Store:
                    InteractionManager.Instance.FacilityList[0].IsCalculating = true;
                    this.gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth -= 1;
                    RandomFacilityReward();
                    break;

                case (int)InteractionManager.SpotName.BookStore:
                    InteractionManager.Instance.FacilityList[1].IsCalculating = true;
                    this.gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth -= 1;
                    this.gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion -= 2;
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
                    gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
                    break;
            }
        }
        
        doingSomething = true;

        return TaskStatus.Success;
    }

    private int RandomObjectReward()
    {
        int spot = gameObject.GetComponent<Instructor>().NowRoom;

        InteractionManager.Instance.EndUseObject(spot, gameObject.GetComponent<Instructor>().UsingObjNum);

        int randomScript = Random.Range(1, 101);

        // 스크립트 진행
        if (randomScript <= 30)
        {
            InteractionManager.Instance.RandomScript(gameObject.GetComponent<Instructor>().NowRoom, this.transform, false);
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
            InteractionManager.Instance.RandomProfScript(gameObject.GetComponent<Instructor>().NowRoom, this.transform);
        }

        int randomEvent = Random.Range(1, 101);

        if (randomEvent <= 20)
        {
            // 아무런 보상 없음
            StartCoroutine(ResetFacility(false));
            return 0;
        }
        else if (randomEvent <= 100)
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

    IEnumerator ResetObject(bool isGetReward)
    {
        yield return new WaitForSeconds(2f);

        if (gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.VendingMachine)
        {
            if (isGetReward)
            {
                int randomIncome = Random.Range(300, 1001);
                int randomPassion = Random.Range(3, 6);

                PlayerInfo.Instance.m_MyMoney += randomIncome;
                MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomIncome;
                gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomPassion;
            }
        }
        else if (gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.Pot)
        {
            if (isGetReward)
            {
                int randomPassion = Random.Range(4, 12);
                gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomPassion;
            }
        }
        else if (gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.AmusementMachine)
        {
            if (isGetReward)
            {
                // todo 게임기 보상 변경 해야함
                int randomInt = Random.Range(20, 41);
                gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth += randomInt;
                if (gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth >= 100)
                {
                    gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth = 100;
                }
                randomInt = Random.Range(500, 1001);
                PlayerInfo.Instance.m_MyMoney += randomInt;
                MonthlyReporter.Instance.m_NowMonth.IncomeSell += randomInt;
            }
        }
        else if (gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.WaterPurifier)
        {
            if (isGetReward)
            {
                int randomHealth = Random.Range(3, 11);
                gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth += randomHealth;
                if (gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth >= 100)
                {
                    gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth = 100;
                }
            }
        }
        else if (gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.NoticeBoard)
        {
            if (isGetReward)
            {
                // todo 게시판 보상 추가 예정
            }
        }

        gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.EndInteracting;
        gameObject.GetComponent<Instructor>().m_IsCoolDown = true;
        //gameObject.GetComponent<Instructor>().InteractingObj = null;
        gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        doingSomething = false;
    }

    IEnumerator ResetFacility(bool isGetReward)
    {
        yield return new WaitForSeconds(2f);

        if (gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.Store || gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.BookStore)
        {
            if (isGetReward)
            {
                InteractionManager.Instance.GetStoreReward(gameObject.GetComponent<Instructor>().NowRoom);
            }

            InteractionManager.Instance.ExitStore((int)gameObject.GetComponent<Instructor>().NowRoom);
        }
        else if (gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.Lounge1)
        {
            if (isGetReward)
            {
                int randomInt = Random.Range(20, 41);
                gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth += randomInt;
                if (gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth >= 100)
                {
                    gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth = 100;
                }
                randomInt = Random.Range(15, 26);
                gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomInt;
                if (gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion >= 100)
                {
                    gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion = 100;
                }
            }

            InteractionManager.Instance.ExitLounge(1);
        }
        else if (gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.Lounge2)
        {
            if (isGetReward)
            {
                int randomInt = Random.Range(20, 41);
                gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth += randomInt;
                if (gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth >= 100)
                {
                    gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorHealth = 100;
                }
                randomInt = Random.Range(15, 26);
                gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomInt;
                if (gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion >= 100)
                {
                    gameObject.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion = 100;
                }
            }

            InteractionManager.Instance.ExitLounge(2);
        }


        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sitting Idle"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToSitting", false);
            gameObject.GetComponent<Animator>().SetBool("IsWalkToIdle", true);
        }

        gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.EndInteracting;
        gameObject.GetComponent<Instructor>().m_IsCoolDown = true;
        //gameObject.GetComponent<Instructor>().InteractingObj = null;
        gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        doingSomething = false;
    }

    IEnumerator ObjectClickEvent()
    {
        yield return new WaitForSeconds(2f);

        // todo 강사 클릭관련 추가 해야함
        ClickEventManager.Instance.InstructorSpecialEvent(this.gameObject.GetComponent<Instructor>(), this.gameObject.transform.position, ClickEventType.Object);

        yield return new WaitForSeconds(3f);


        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sitting Idle"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToSitting", false);
            gameObject.GetComponent<Animator>().SetBool("IsWalkToIdle", true);
        }

        gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.EndInteracting;
        gameObject.GetComponent<Instructor>().m_IsCoolDown = true;
        //gameObject.GetComponent<Instructor>().InteractingObj = null;
        gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        doingSomething = false;
    }

    IEnumerator FacilityClickEvent()
    {
        yield return new WaitForSeconds(2f);
        
        ClickEventManager.Instance.InstructorSpecialEvent(this.gameObject.GetComponent<Instructor>(), this.gameObject.transform.position, ClickEventType.Facility);

        yield return new WaitForSeconds(3f);

        if (gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.Store ||
            gameObject.GetComponent<Instructor>().NowRoom == (int)InteractionManager.SpotName.BookStore)
        {
            InteractionManager.Instance.ExitStore((int)gameObject.GetComponent<Instructor>().NowRoom);
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.Lounge1)
        {
            InteractionManager.Instance.ExitLounge(1);
        }
        else if (gameObject.GetComponent<Student>().NowRoom == (int)InteractionManager.SpotName.Lounge2)
        {
            InteractionManager.Instance.ExitLounge(2);
        }


        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sitting Idle"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToSitting", false);
            gameObject.GetComponent<Animator>().SetBool("IsWalkToIdle", true);
        }

        gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.EndInteracting;
        gameObject.GetComponent<Instructor>().m_IsCoolDown = true;
        //gameObject.GetComponent<Instructor>().InteractingObj = null;
        gameObject.GetComponent<Instructor>().NowRoom = (int)InteractionManager.SpotName.Nothing;
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        doingSomething = false;
    }
}