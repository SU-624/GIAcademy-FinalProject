using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;

public class Interaction : Action
{
    public delegate void FriendShipDataChange();
    public static event FriendShipDataChange FriendShipChangedEvent;

    private GameObject m_InterActionObj;
    private GameObject _chatB;
    private GameObject _chatA;              // ChatBox�� 

    private bool m_IsDialogueEnd;           // ��ȭ�� ����ִ� ť�� ������� �� ��ȭ���Ḧ ���ֱ����� bool��
    private int m_friendship;
    private int m_myIndex;
    private int m_otherIndex;
    private bool m_isProfessor;
    private bool m_isLeft;
    //private bool m_IsStatusChange;
    private Vector3 m_myDestination;
    private Vector3 m_otherDestination;
    private string m_myName;
    private string m_otherName;

    private int m_RandomNumTalkAniIndex;

    private Queue<string> m_Dialogue = new Queue<string>();


    private Transform m_Canvas;             // ChatBox�� ĵ������ ����ֱ����� ĵ������ transform�� �������ִ� ����

    IEnumerator m_DialogueIEnumerator;      // �ڷ�ƾ�� �����ֱ����� IEnumerator�� �������ִ� ����

    //private string m_CurrentSentence;

    public override void OnStart()
    {
        m_Canvas = GameObject.Find("InGameCanvas").transform;
        m_InterActionObj = this.gameObject.GetComponent<Student>().InteractingObj;

        m_IsDialogueEnd = false;
    }

    // �� �����Ӹ��� �л����� ��Ÿ���� üũ�ϸ鼭 ��Ÿ���� ������ �� ��ȭ�� �������ش�.
    public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance == null)
            return TaskStatus.Failure;

        if (InGameTest.Instance.m_ClassState != ClassState.nothing)
            return TaskStatus.Failure;

        if (m_InterActionObj == null)
            return TaskStatus.Failure;

        if (this.gameObject.GetComponent<Student>().DoingValue != Student.Doing.FreeWalk)
            return TaskStatus.Failure;

        if (m_InterActionObj.tag == "Instructor")
            m_isProfessor = true;

        else if (m_InterActionObj.tag == "Student")
            m_isProfessor = false;

        bool _coolTime = this.gameObject.GetComponent<Student>().m_IsCoolDown;
        bool _isInteracting = this.gameObject.GetComponent<Student>().m_IsInteracting;

        bool _isTargetObjCoolTime;
        bool _isTargetObjInteracting;
        if (m_isProfessor)
        {
            _isTargetObjCoolTime = m_InterActionObj.GetComponent<Instructor>().m_IsCoolDown;
            _isTargetObjInteracting = m_InterActionObj.GetComponent<Instructor>().m_IsCoolDown;
        }
        else
        {
            _isTargetObjCoolTime = m_InterActionObj.GetComponent<Student>().m_IsCoolDown;
            _isTargetObjInteracting = m_InterActionObj.GetComponent<Student>().m_IsCoolDown;
        }


        if (!_coolTime && !_isInteracting && !_isTargetObjCoolTime && !_isTargetObjInteracting)
        {
            ChangeStatus();
            m_Dialogue.Clear();
            ScriptPlay();
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    private void ChangeStatus()
    {
        this.gameObject.GetComponent<Student>().m_IsInteracting = true;
        this.gameObject.GetComponent<Student>().DoingValue = Student.Doing.StartInteracting;
        m_myName = this.gameObject.GetComponent<Student>().m_StudentStat.m_StudentName;
        m_myIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(this.gameObject.GetComponent<Student>()));

        if (m_isProfessor)
        {
            m_InterActionObj.GetComponent<Instructor>().m_IsInteracting = true;
            m_InterActionObj.GetComponent<Instructor>().DoingValue = Instructor.Doing.StartInteracting;
            m_otherIndex = 18 + ObjectManager.Instance.m_InstructorList.FindIndex(x => x.Equals(m_InterActionObj.GetComponent<Instructor>()));
            m_otherName = m_InterActionObj.GetComponent<Instructor>().m_InstructorData.professorName;
        }
        else
        {
            m_InterActionObj.GetComponent<Student>().m_IsInteracting = true;
            m_InterActionObj.GetComponent<Student>().DoingValue = Student.Doing.StartInteracting;
            m_otherIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(m_InterActionObj.GetComponent<Student>()));
            m_otherName = m_InterActionObj.GetComponent<Student>().m_StudentStat.m_StudentName;
        }
        m_friendship = ObjectManager.Instance.m_Friendship[m_myIndex][m_otherIndex];
    }

    public void ScriptPlay()
    {
        /// �л����� ���ο��� �ٰ����� �ڷ�ƾ�� ȣ��....

        StartCoroutine(Move_ToAnother());
    }

    /// ���θ� ������ �ٰ������� �Ѵ�.
    IEnumerator Move_ToAnother()
    {
        // ��
        ///gameObject;

        // �����
        ///m_InterActionObj;

        Debug.Log(this.gameObject.name + "�̵� ����");

        float _dist = float.MaxValue;
        float _range = 15.0f;

        m_myDestination = gameObject.GetComponent<NavMeshAgent>().destination;
        m_otherDestination = m_InterActionObj.GetComponent<NavMeshAgent>().destination;

        // �ڷ�ƾ ������ ������ ���鼭, ���� ���� �Ÿ��� �Ǵ��� üũ�Ѵ�.
        while (_range < _dist)
        {
            // �� ���� ��ü�� ��ġ�� �� ���͸� �����.
            // A - B �� ���� �� ������ ���ʹ�, B���� A�� �̵��ϴ� ���ʹ�.
            Vector3 _distVec = gameObject.transform.position - m_InterActionObj.transform.position;

            //// �� ������ ���̰� �� �Ÿ���.
            _dist = _distVec.sqrMagnitude;

            // ���� ��������� �����.
            //Vector3 _moveVec = _distVec.normalized;

            //// �������� * �����ð��� �ϸ� �ð��� ����ؼ� ������ �����δ�.
            //m_InterActionObj.transform.Translate(_moveVec * Time.deltaTime);

            //// ���� ����� ������ �������� �Ѵ�.
            //gameObject.transform.Translate(-1.0f * _moveVec * Time.deltaTime);


            gameObject.GetComponent<NavMeshAgent>().SetDestination(m_InterActionObj.transform.position);
            m_InterActionObj.GetComponent<NavMeshAgent>().SetDestination(gameObject.transform.position);

            yield return null;
        }

        // ������� �Դٸ� ������� ���̴�.
        yield return null;

        // �����ֱ�
        this.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        m_InterActionObj.GetComponent<NavMeshAgent>().isStopped = true;

        this.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        m_InterActionObj.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

        // ���θ� �ٶ󺸰� ���ֱ�
        this.gameObject.GetComponent<Transform>().LookAt(m_InterActionObj.transform);
        m_InterActionObj.GetComponent<Transform>().LookAt(this.gameObject.transform);

        Debug.Log(this.gameObject.name + "�̵� ����");

        m_DialogueIEnumerator = DialogueRoutine();
        StartCoroutine(m_DialogueIEnumerator);
    }

    IEnumerator DialogueRoutine()
    {
        int _tempnum = Random.Range(0, ChatBox.Instance.m_DialogueLines.Count);

        //ConcurrentQueue<string> m_Dialogue = new ConcurrentQueue<string>();

        //string[] m_Temp = ChatBox.Instance.m_DialogueLines[_tempnum].Split('/');
        string[] m_Temp = { };
        int randomScript = 0;

        if (m_friendship < 150)
        {
            if (m_isProfessor)
            {
                if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                    this.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.CStuProScripts[0].Count);
                    m_Temp = ScriptsManager.Instance.CStuProScripts[0][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                         !this.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.IStuProScripts[0].Count);
                    m_Temp = ScriptsManager.Instance.IStuProScripts[0][randomScript].Split('/');
                }
                else
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.StuProScripts[0].Count);
                    m_Temp = ScriptsManager.Instance.StuProScripts[0][randomScript].Split('/');
                }
            }
            else
            {
                if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                    this.GetComponent<Student>().m_StudentStat.m_IsRecommend && m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.CStuCStuScripts[0].Count);
                    m_Temp = ScriptsManager.Instance.CStuCStuScripts[0][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                    this.GetComponent<Student>().m_StudentStat.m_IsRecommend && !m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.CStuIStuScripts[0].Count);
                    m_Temp = ScriptsManager.Instance.CStuIStuScripts[0][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                         !this.GetComponent<Student>().m_StudentStat.m_IsRecommend && m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.IStuCStuScripts[0].Count);
                    m_Temp = ScriptsManager.Instance.IStuCStuScripts[0][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                         !this.GetComponent<Student>().m_StudentStat.m_IsRecommend && !m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.IStuIStuScripts[0].Count);
                    m_Temp = ScriptsManager.Instance.IStuIStuScripts[0][randomScript].Split('/');
                }
                else
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.InteractionScripts[0].Count);
                    m_Temp = ScriptsManager.Instance.InteractionScripts[0][randomScript].Split('/');
                }
            }
        }
        else if (m_friendship < 300)
        {
            if (m_isProfessor)
            {
                if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                    this.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.CStuProScripts[1].Count);
                    m_Temp = ScriptsManager.Instance.CStuProScripts[1][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                         !this.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.IStuProScripts[1].Count);
                    m_Temp = ScriptsManager.Instance.IStuProScripts[1][randomScript].Split('/');
                }
                else
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.StuProScripts[1].Count);
                    m_Temp = ScriptsManager.Instance.StuProScripts[1][randomScript].Split('/');
                }
            }
            else
            {
                if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                    this.GetComponent<Student>().m_StudentStat.m_IsRecommend && m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.CStuCStuScripts[1].Count);
                    m_Temp = ScriptsManager.Instance.CStuCStuScripts[1][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                    this.GetComponent<Student>().m_StudentStat.m_IsRecommend && !m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.CStuIStuScripts[1].Count);
                    m_Temp = ScriptsManager.Instance.CStuIStuScripts[1][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                         !this.GetComponent<Student>().m_StudentStat.m_IsRecommend && m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.IStuCStuScripts[1].Count);
                    m_Temp = ScriptsManager.Instance.IStuCStuScripts[1][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                         !this.GetComponent<Student>().m_StudentStat.m_IsRecommend && !m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.IStuIStuScripts[1].Count);
                    m_Temp = ScriptsManager.Instance.IStuIStuScripts[1][randomScript].Split('/');
                }
                else
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.InteractionScripts[1].Count);
                    m_Temp = ScriptsManager.Instance.InteractionScripts[1][randomScript].Split('/');
                }
            }
        }
        else
        {
            if (m_isProfessor)
            {
                if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                    this.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.CStuProScripts[2].Count);
                    m_Temp = ScriptsManager.Instance.CStuProScripts[2][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                         !this.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.IStuProScripts[2].Count);
                    m_Temp = ScriptsManager.Instance.IStuProScripts[2][randomScript].Split('/');
                }
                else
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.StuProScripts[2].Count);
                    m_Temp = ScriptsManager.Instance.StuProScripts[2][randomScript].Split('/');
                }
            }
            else
            {
                if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                    this.GetComponent<Student>().m_StudentStat.m_IsRecommend && m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.CStuCStuScripts[2].Count);
                    m_Temp = ScriptsManager.Instance.CStuCStuScripts[2][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                    this.GetComponent<Student>().m_StudentStat.m_IsRecommend && !m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.CStuIStuScripts[2].Count);
                    m_Temp = ScriptsManager.Instance.CStuIStuScripts[2][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                         !this.GetComponent<Student>().m_StudentStat.m_IsRecommend && m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.IStuCStuScripts[2].Count);
                    m_Temp = ScriptsManager.Instance.IStuCStuScripts[2][randomScript].Split('/');
                }
                else if (GameTime.Instance.FlowTime.NowMonth == 2 &&
                         !this.GetComponent<Student>().m_StudentStat.m_IsRecommend && !m_InterActionObj.GetComponent<Student>().m_StudentStat.m_IsRecommend)
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.IStuIStuScripts[2].Count);
                    m_Temp = ScriptsManager.Instance.IStuIStuScripts[2][randomScript].Split('/');
                }
                else
                {
                    randomScript = Random.Range(0, ScriptsManager.Instance.InteractionScripts[2].Count);
                    m_Temp = ScriptsManager.Instance.InteractionScripts[2][randomScript].Split('/');
                }
            }
        }

        Vector3 myPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 otherPos = Camera.main.WorldToScreenPoint(m_InterActionObj.transform.position);
        if (myPos.x < otherPos.x)
        {
            m_isLeft = true;
        }
        else
        {
            m_isLeft = false;
        }

        m_RandomNumTalkAniIndex = Random.Range(0, 2);
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idel_1"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsIdelToTalk", true);
            gameObject.GetComponent<Animator>().SetInteger("TalkAniNumber", m_RandomNumTalkAniIndex);
        }
        else if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToTalk", true);
            gameObject.GetComponent<Animator>().SetInteger("TalkAniNumber", m_RandomNumTalkAniIndex);
        }


        if (!m_isProfessor)
        {
            if (m_InterActionObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idel_1"))
            {
                m_InterActionObj.GetComponent<Animator>().SetBool("IsIdelToTalk", true);
                gameObject.GetComponent<Animator>().SetInteger("TalkAniNumber", m_RandomNumTalkAniIndex);
            }
            else if (m_InterActionObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                m_InterActionObj.GetComponent<Animator>().SetBool("IsWalkToTalk", true);
                m_InterActionObj.GetComponent<Animator>().SetInteger("TalkAniNumber", m_RandomNumTalkAniIndex);
            }
        }



        foreach (var line in m_Temp)
        {
            m_Dialogue.Enqueue(line);
        }

        while (m_Dialogue.Count > 0)
        {
            _chatA = InGameObjectPool.GetChatObject(m_Canvas);
            if (m_isLeft == true)
            {
                _chatA.GetComponent<FollowTarget>().IsLeft = true;
            }
            else
            {
                _chatA.GetComponent<FollowTarget>().IsLeft = false;
            }

            _chatA.GetComponent<FollowTarget>().m_Target = gameObject.transform.GetChild(0).transform;

            ADialogueFlow(_chatA, false);

            yield return new WaitForSeconds(1f);

            // ������� ���� ���� ������ ���ϰ� �ϱ�
            if (m_Dialogue.Count != 0)
            {
                _chatB = InGameObjectPool.GetChatObject(m_Canvas);
                if (m_isLeft == true)
                {
                    _chatB.GetComponent<FollowTarget>().IsLeft = false;
                }
                else
                {
                    _chatB.GetComponent<FollowTarget>().IsLeft = true;
                }
                _chatB.GetComponent<FollowTarget>().m_Target = m_InterActionObj.gameObject.transform.GetChild(0).transform;
                ADialogueFlow(_chatB, true);

                yield return new WaitForSeconds(3f);

                InGameObjectPool.ReturnChatObject(_chatA);
                InGameObjectPool.ReturnChatObject(_chatB);
            }
            // ��������� ������ ���� ���ѰŴ� �� ��ǳ���� ����������
            else
            {
                yield return new WaitForSeconds(3f);

                InGameObjectPool.ReturnChatObject(_chatA);
            }
        }

        if (!m_IsDialogueEnd)
        {
            int randomReward = Random.Range(1, 101);
            int randomSelect = Random.Range(1, 101);
            int randomFriendship = 0;
            int randomSpecialPoint = 0;

            // �ƴ»���
            if (m_friendship < 150)
            {
                if (randomReward <= 40) // �ӽ� ����
                {
                    // ���� X
                    ScriptEnd();
                }
                else if (randomReward <= 90)
                {
                    // ��ȣ���� ģ�е� ����
                    randomFriendship = Random.Range(20, 61);
                    if (m_isProfessor)
                    {
                        ObjectManager.Instance.m_Friendship[m_myIndex][m_otherIndex] += randomFriendship;
                    }
                    else
                    {
                        ObjectManager.Instance.m_Friendship[m_myIndex][m_otherIndex] += randomFriendship;
                        ObjectManager.Instance.m_Friendship[m_otherIndex][m_myIndex] += randomFriendship;
                    }

                    ScriptEnd();
                }
                else
                {
                    ClickEventManager.Instance.StudentSpecialEvent(this.gameObject.GetComponent<Student>(), this.gameObject.transform.position, ClickEventType.Interaction);
                    StartCoroutine(CoScriptEnd());
                }

            }
            // ģ�ѻ���
            else if (m_friendship < 300)
            {
                if (randomReward <= 30)
                {
                    // ���� X
                    ScriptEnd();
                }
                else if (randomReward <= 80)
                {
                    if (randomSelect <= 80)
                    {
                        // ��ȣ���� ģ�е� ����
                        randomFriendship = Random.Range(30, 81);
                        if (m_isProfessor)
                        {
                            ObjectManager.Instance.m_Friendship[m_myIndex][m_otherIndex] += randomFriendship;
                        }
                        else
                        {
                            ObjectManager.Instance.m_Friendship[m_myIndex][m_otherIndex] += randomFriendship;
                            ObjectManager.Instance.m_Friendship[m_otherIndex][m_myIndex] += randomFriendship;
                        }

                        if (m_friendship + randomFriendship >= 300)
                        {
                            GameTime.Instance.AlarmControl.AlarmMessageQ.Enqueue("<color=#00EAFF>" + m_myName + "</color> ��(��)" +
                                "<color=#00EAFF>" + m_otherName + "</color> �� ������ �Ǿ����ϴ�!");
                        }
                    }
                    else
                    {
                        if (m_isProfessor)
                        {
                            int randomPassion = Random.Range(3, 9);
                            this.gameObject.GetComponent<Student>().m_StudentStat.m_Passion += randomPassion;
                            m_InterActionObj.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomPassion;
                        }
                        else
                        {
                            // 2�� ��ȭ
                            randomSpecialPoint = Random.Range(7, 15);
                            PlayerInfo.Instance.m_SpecialPoint += randomSpecialPoint;
                        }
                    }
                    ScriptEnd();
                }
                else
                {
                    ClickEventManager.Instance.StudentSpecialEvent(this.gameObject.GetComponent<Student>(), this.gameObject.transform.position, ClickEventType.Interaction);
                    StartCoroutine(CoScriptEnd());
                }
            }
            // ����
            else
            {
                if (randomReward <= 20)
                {
                    // ���� X
                    ScriptEnd();
                }
                else if (randomReward <= 60)
                {
                    if (m_isProfessor)
                    {
                        int randomPassion = Random.Range(7, 16);
                        this.gameObject.GetComponent<Student>().m_StudentStat.m_Passion += randomPassion;
                        m_InterActionObj.GetComponent<Instructor>().m_InstructorData.m_ProfessorPassion += randomPassion;
                    }
                    else
                    {
                        // 2�� ��ȭ
                        randomSpecialPoint = Random.Range(7, 16);
                        PlayerInfo.Instance.m_SpecialPoint += randomSpecialPoint;
                    }
                    ScriptEnd();
                }
                else
                {
                    ClickEventManager.Instance.StudentSpecialEvent(this.gameObject.GetComponent<Student>(), this.gameObject.transform.position, ClickEventType.Interaction);
                    StartCoroutine(CoScriptEnd());
                }
            }
        }
        /// ���ʽ� ��ų �߰�
        FriendShipChangedEvent?.Invoke();
        m_IsDialogueEnd = true;
    }

    void ADialogueFlow(GameObject _chatBox, bool isOther)
    {
        if (m_Dialogue.Count > 0)
        {
            // ��Ƽ�����忡�� ������ �� ������ü�� ���ÿ� ť�� �����ϴ°� �����ϴ� ����
            //m_Dialogue.Dequeue(m_CurrentSentence);

            int emoticonNum = 0;
            if (int.TryParse(m_Dialogue.Peek(), out emoticonNum))
            {
                m_Dialogue.Dequeue();
                if (isOther)
                {
                    InteractionManager.Instance.ShowEmoticon(emoticonNum - 10, m_InterActionObj.transform);
                }
                else
                {
                    InteractionManager.Instance.ShowEmoticon(emoticonNum - 10, this.transform);
                }
                _chatBox.SetActive(false);
            }
            else
            {
                _chatBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_Dialogue.Dequeue();
            }
        }


        //if (m_Dialogue.Count == 0)
        //{
        //    m_IsDialogueEnd = true;
        //}
    }

    // ��ȭ���� ������ �ʸ� �־��ֱ�.
    void ScriptEnd()
    {
        m_InterActionObj.GetComponent<NavMeshAgent>().isStopped = false;
        m_InterActionObj.GetComponent<NavMeshAgent>().SetDestination(m_otherDestination);
        if (m_isProfessor)
        {
            m_InterActionObj.GetComponent<Instructor>().m_IsInteracting = false;
            m_InterActionObj.GetComponent<Instructor>().DoingValue = Instructor.Doing.EndInteracting;
            //m_InterActionObj.GetComponent<Instructor>().InteractingObj = null;
        }
        else
        {
            m_InterActionObj.GetComponent<Student>().m_IsInteracting = false;
            m_InterActionObj.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
            //m_InterActionObj.GetComponent<Student>().InteractingObj = null;
        }

        this.gameObject.GetComponent<Student>().m_IsInteracting = false;
        this.gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
        this.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        //this.gameObject.GetComponent<Student>().InteractingObj = null;
        this.gameObject.GetComponent<NavMeshAgent>().SetDestination(m_myDestination);

        m_isProfessor = false;

        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Talking1") || gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Talking3"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToTalk", false);
            gameObject.GetComponent<Animator>().SetInteger("TalkAniNumber", m_RandomNumTalkAniIndex);
        }

        if (m_InterActionObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Talking1") || m_InterActionObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Talking3"))
        {
            m_InterActionObj.GetComponent<Animator>().SetBool("IsWalkToTalk", false);
            m_InterActionObj.GetComponent<Animator>().SetInteger("TalkAniNumber", m_RandomNumTalkAniIndex);
        }


        StopCoroutine(m_DialogueIEnumerator);
    }

    IEnumerator CoScriptEnd()
    {
        //yield return new WaitForSeconds(5f);
        yield return new WaitUntil(() => ClickEventManager.Instance.StudentPosition.Find(x => x.Equals(this.gameObject.transform.position)) == Vector3.zero);

        m_InterActionObj.GetComponent<NavMeshAgent>().isStopped = false;
        m_InterActionObj.GetComponent<NavMeshAgent>().SetDestination(m_otherDestination);
        if (m_isProfessor)
        {
            m_InterActionObj.GetComponent<Instructor>().m_IsInteracting = false;
            m_InterActionObj.GetComponent<Instructor>().DoingValue = Instructor.Doing.EndInteracting;
            //m_InterActionObj.GetComponent<Instructor>().InteractingObj = null;
        }
        else
        {
            m_InterActionObj.GetComponent<Student>().m_IsInteracting = false;
            m_InterActionObj.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
            //m_InterActionObj.GetComponent<Student>().InteractingObj = null;
        }

        this.gameObject.GetComponent<Student>().m_IsInteracting = false;
        this.gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
        this.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        this.gameObject.GetComponent<Student>().InteractingObj = null;
        this.gameObject.GetComponent<NavMeshAgent>().SetDestination(m_myDestination);

        m_isProfessor = false;

        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Talking1") || gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Talking3"))
        {
            gameObject.GetComponent<Animator>().SetBool("IsWalkToTalk", false);
            gameObject.GetComponent<Animator>().SetInteger("TalkAniNumber", m_RandomNumTalkAniIndex);
        }

        if (!m_isProfessor)
        {
            if (m_InterActionObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Talking1") || m_InterActionObj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Talking3"))
            {
                m_InterActionObj.GetComponent<Animator>().SetBool("IsWalkToTalk", false);
                m_InterActionObj.GetComponent<Animator>().SetInteger("TalkAniNumber", m_RandomNumTalkAniIndex);
            }
        }


        StopCoroutine(m_DialogueIEnumerator);
    }
}