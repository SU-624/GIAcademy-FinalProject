using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEngine.AI;
using System.Collections;
using TMPro;

public class Interaction : Action
{
    private GameObject m_InterActionObj;
    private GameObject _chatB;
    private GameObject _chatA;              // ChatBox를 

    private bool m_IsPlaying;               // 쿨다운이 끝났고 상호작용을 하는 중이 아닐 때만 대화진행을 시키기위한 bool값
    private bool m_IsPlayingB;
    private bool m_CoolTime;                // 한번 상호작용을 하면 일정시간동안 못하게 해주기위한 CoolTime bool값
    private bool m_IsInteracting;           // 현재 캐릭터가 상호작용을 하는지 확인해주는 bool값
    private bool m_IsDialogueEnd;           // 대화가 들어있는 큐가 비어있을 때 대화종료를 해주기위한 bool값
    private bool m_IsTargetObjInteracting;
    private bool m_IsTargetObjCoolTime;
    //private bool m_IsStatusChange;


    private Transform m_Canvas;             // ChatBox를 캔버스에 띄워주기위해 캔버스의 transform을 저장해주는 변수

    IEnumerator m_DialogueIEnumerator;      // 코루틴을 멈춰주기위해 IEnumerator를 저장해주는 변수

    private string m_CurrentSentence;

    public override void OnStart()
    {
        m_Canvas = GameObject.Find("InGameCanvas").transform;
        //m_DialogueIEnumerator = DialogueRoutine();
        //m_IsStatusChange = false;
        SharedGameObject m_Interaction = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable("InteractionObject").ConvertTo<SharedGameObject>();
        m_InterActionObj = m_Interaction.Value;
    }

    // 매 프레임마다 학생들의 쿨타임을 체크하면서 쿨타임이 끝났을 때 대화를 시작해준다.
    public override TaskStatus OnUpdate()
    {
        m_CoolTime = this.gameObject.GetComponent<Student>().m_IsCoolDown;
        m_IsInteracting = this.gameObject.GetComponent<Student>().m_IsInteracting;

        m_IsPlaying = this.gameObject.GetComponent<Student>().m_IsDialoguePlaying;
        m_IsPlayingB = m_InterActionObj.GetComponent<Student>().m_IsDialoguePlaying;

        m_IsTargetObjCoolTime = m_InterActionObj.GetComponent<Student>().m_IsCoolDown;
        m_IsTargetObjInteracting = m_InterActionObj.GetComponent<Student>().m_IsInteracting;

        if (m_CoolTime && m_IsInteracting && m_IsTargetObjInteracting && m_IsTargetObjCoolTime)
        {
            if (m_IsPlaying && m_IsPlayingB)
            {
                ScriptPlay();
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

        //if (InGameTest.Instance.m_ClassState == ClassState.Studying)
        //{
        //    m_IsInteracting = false;
        //}

        // 쿨다운이 끝났고 상호작용 중이 아닐 때 
        //if (!m_CoolTime && !m_IsInteracting && !m_IsTargetObjInteracting && !m_IsTargetObjCoolTime)
        //{
        //}
        //    if (m_IsPlaying)
        //    {
        //    }
        //ScriptPlay();

        return TaskStatus.Success;
    }

    //public override void OnLateUpdate()
    //{
    //    base.OnLateUpdate();

    //    Debug.Log("LateUpDate들어오는지 확인");

    //    if (m_IsStatusChange)
    //    {
    //        ScriptPlay();
    //    }

    //}

    //private void ChangeStatus()
    //{
    //    this.gameObject.GetComponent<Student>().m_IsInteracting = true;
    //    m_InterActionObj.GetComponent<Student>().m_IsInteracting = true;

    //    this.gameObject.GetComponent<Student>().m_Doing = Student.Doing.StartInteracting;
    //    m_InterActionObj.GetComponent<Student>().m_Doing = Student.Doing.StartInteracting;

    //    m_IsStatusChange = true;
    //}

    public void ScriptPlay()
    {
        /// 학생들이 서로에게 다가가는 코루틴을 호출....

        StartCoroutine(Move_ToAnother());
    }

    /// 서로를 가까이 다가가도록 한다.
    IEnumerator Move_ToAnother()
    {
        // 나
        ///gameObject;

        // 상대편
        ///m_InterActionObj;

        float _dist = float.MaxValue;
        float _range = 4.0f;

        // 코루틴 내에서 루프를 돌면서, 일정 이하 거리가 되는지 체크한다.
        while (_range < _dist)
        {
            // 두 개의 물체의 위치의 차 벡터를 만든다.
            // A - B 를 했을 때 나오는 벡터는, B에서 A로 이동하는 벡터다.
            Vector3 _distVec = gameObject.transform.position - m_InterActionObj.transform.position;

            // 그 벡터의 길이가 곧 거리다.
            _dist = _distVec.sqrMagnitude;

            // 서로 가까워지게 만든다.
            Vector3 _moveVec = _distVec.normalized;

            // 단위벡터 * 단위시간을 하면 시간에 비례해서 적당히 움직인다.
            m_InterActionObj.transform.Translate(_moveVec * Time.deltaTime /** movespeed*/);

            // 나도 상대편 쪽으로 움직여야 한다.
            gameObject.transform.Translate(-1.0f * _moveVec * Time.deltaTime);

            yield return null;
        }

        // 여기까지 왔다면 가까워진 것이다.
        yield return null;

        // 멈춰주기
        this.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        m_InterActionObj.GetComponent<NavMeshAgent>().isStopped = true;

        this.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        m_InterActionObj.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

        // 서로를 바라보게 해주기
        this.gameObject.GetComponent<Transform>().LookAt(m_InterActionObj.transform);
        m_InterActionObj.GetComponent<Transform>().LookAt(this.gameObject.transform);

        ChatBox.Instance.OnDialogue();

        this.gameObject.GetComponent<Student>().m_IsDialoguePlaying = false;
        m_InterActionObj.GetComponent<Student>().m_IsDialoguePlaying = false;

        m_DialogueIEnumerator = DialogueRoutine();
        StartCoroutine(m_DialogueIEnumerator);
    }

    IEnumerator DialogueRoutine()
    {
        while (ChatBox.Instance.m_Dialogue.Count > 0)
        {
            _chatA = InGameObjectPool.GetChatObject(m_Canvas);

            _chatA.GetComponent<FollowTarget>().m_Target = gameObject.transform.GetChild(0).transform;

            ADialogueFlow(_chatA);

            yield return new WaitForSeconds(2f);

            // 비어있지 않을 때만 상대방이 말하게 하기
            if (ChatBox.Instance.m_Dialogue.Count != 0)
            {
                _chatB = InGameObjectPool.GetChatObject(m_Canvas);
                _chatB.GetComponent<FollowTarget>().m_Target = m_InterActionObj.gameObject.transform.GetChild(0).transform;
                ADialogueFlow(_chatB);

                yield return new WaitForSeconds(1f);

                InGameObjectPool.ReturnChatObject(_chatA);
                InGameObjectPool.ReturnChatObject(_chatB);
            }
            // 비어있으면 상대방은 말을 안한거니 내 말풍선만 돌려보내기
            else
            {
                yield return new WaitForSeconds(1f);

                InGameObjectPool.ReturnChatObject(_chatA);
            }
        }

        if (m_IsDialogueEnd)
        {
            ScriptEnd();
        }
    }

    void ADialogueFlow(GameObject _chatBox)
    {
        if (ChatBox.Instance.m_Dialogue.Count > 0)
        {
            // 멀티쓰레드에서 동작할 때 여러개체가 동시에 큐에 접근하는걸 방지하는 역할
            ChatBox.Instance.m_Dialogue.TryDequeue(out m_CurrentSentence);
        }

        _chatBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_CurrentSentence;

        if (ChatBox.Instance.m_Dialogue.Count == 0)
        {
            m_IsDialogueEnd = true;
        }
    }

    // 대화마다 정해진 초를 넣어주기.
    void ScriptEnd()
    {
        this.gameObject.GetComponent<Student>().m_IsInteracting = false;
        m_InterActionObj.GetComponent<Student>().m_IsInteracting = false;

        this.gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
        m_InterActionObj.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;

        this.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        m_InterActionObj.GetComponent<NavMeshAgent>().isStopped = false;

        StopCoroutine(m_DialogueIEnumerator);
    }

}