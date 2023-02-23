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
    private GameObject _chatA;              // ChatBox�� 

    private bool m_IsPlaying;               // ��ٿ��� ������ ��ȣ�ۿ��� �ϴ� ���� �ƴ� ���� ��ȭ������ ��Ű������ bool��
    private bool m_IsPlayingB;
    private bool m_CoolTime;                // �ѹ� ��ȣ�ۿ��� �ϸ� �����ð����� ���ϰ� ���ֱ����� CoolTime bool��
    private bool m_IsInteracting;           // ���� ĳ���Ͱ� ��ȣ�ۿ��� �ϴ��� Ȯ�����ִ� bool��
    private bool m_IsDialogueEnd;           // ��ȭ�� ����ִ� ť�� ������� �� ��ȭ���Ḧ ���ֱ����� bool��
    private bool m_IsTargetObjInteracting;
    private bool m_IsTargetObjCoolTime;
    //private bool m_IsStatusChange;


    private Transform m_Canvas;             // ChatBox�� ĵ������ ����ֱ����� ĵ������ transform�� �������ִ� ����

    IEnumerator m_DialogueIEnumerator;      // �ڷ�ƾ�� �����ֱ����� IEnumerator�� �������ִ� ����

    private string m_CurrentSentence;

    public override void OnStart()
    {
        m_Canvas = GameObject.Find("InGameCanvas").transform;
        //m_DialogueIEnumerator = DialogueRoutine();
        //m_IsStatusChange = false;
        SharedGameObject m_Interaction = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable("InteractionObject").ConvertTo<SharedGameObject>();
        m_InterActionObj = m_Interaction.Value;
    }

    // �� �����Ӹ��� �л����� ��Ÿ���� üũ�ϸ鼭 ��Ÿ���� ������ �� ��ȭ�� �������ش�.
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

        // ��ٿ��� ������ ��ȣ�ۿ� ���� �ƴ� �� 
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

    //    Debug.Log("LateUpDate�������� Ȯ��");

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

        float _dist = float.MaxValue;
        float _range = 4.0f;

        // �ڷ�ƾ ������ ������ ���鼭, ���� ���� �Ÿ��� �Ǵ��� üũ�Ѵ�.
        while (_range < _dist)
        {
            // �� ���� ��ü�� ��ġ�� �� ���͸� �����.
            // A - B �� ���� �� ������ ���ʹ�, B���� A�� �̵��ϴ� ���ʹ�.
            Vector3 _distVec = gameObject.transform.position - m_InterActionObj.transform.position;

            // �� ������ ���̰� �� �Ÿ���.
            _dist = _distVec.sqrMagnitude;

            // ���� ��������� �����.
            Vector3 _moveVec = _distVec.normalized;

            // �������� * �����ð��� �ϸ� �ð��� ����ؼ� ������ �����δ�.
            m_InterActionObj.transform.Translate(_moveVec * Time.deltaTime /** movespeed*/);

            // ���� ����� ������ �������� �Ѵ�.
            gameObject.transform.Translate(-1.0f * _moveVec * Time.deltaTime);

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

            // ������� ���� ���� ������ ���ϰ� �ϱ�
            if (ChatBox.Instance.m_Dialogue.Count != 0)
            {
                _chatB = InGameObjectPool.GetChatObject(m_Canvas);
                _chatB.GetComponent<FollowTarget>().m_Target = m_InterActionObj.gameObject.transform.GetChild(0).transform;
                ADialogueFlow(_chatB);

                yield return new WaitForSeconds(1f);

                InGameObjectPool.ReturnChatObject(_chatA);
                InGameObjectPool.ReturnChatObject(_chatB);
            }
            // ��������� ������ ���� ���ѰŴ� �� ��ǳ���� ����������
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
            // ��Ƽ�����忡�� ������ �� ������ü�� ���ÿ� ť�� �����ϴ°� �����ϴ� ����
            ChatBox.Instance.m_Dialogue.TryDequeue(out m_CurrentSentence);
        }

        _chatBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_CurrentSentence;

        if (ChatBox.Instance.m_Dialogue.Count == 0)
        {
            m_IsDialogueEnd = true;
        }
    }

    // ��ȭ���� ������ �ʸ� �־��ֱ�.
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