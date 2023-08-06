using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AcademyManager : MonoBehaviour
{
    private string m_NowDestination = " ";
    private NavMeshAgent m_Agent;
    private const float DesChangeDis = 1;
    private Animator m_Animator;
    private Transform m_canvas;
    private bool m_IsCoroutineRunning = false;
    private bool m_NowTalking = false;
    private IEnumerator m_TalkCoroutine;
    
    [SerializeField] private Transform m_MyPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_canvas = GameObject.Find("InGameCanvas").transform;
        m_Agent = gameObject.GetComponent<NavMeshAgent>();
        m_Animator = transform.GetChild(1).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameTime.Instance != null)
        {
            // 2월 3주 금요일 방학
            if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 3 && GameTime.Instance.FlowTime.NowDay == 5)
            {
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

                        if (!m_IsCoroutineRunning)
                        {
                            m_TalkCoroutine = VacationMoveTalk();
                            StartCoroutine(m_TalkCoroutine);
                        }
                    }
                }
                else if (m_NowDestination == "VacationPoint")
                {
                    Vector3 myPos = gameObject.transform.position;
                    Vector3 desPos = m_Agent.destination;

                    float dis = Vector3.Distance(myPos, desPos);

                    if (dis < DesChangeDis)
                    {
                        m_Animator.SetBool("IdleToWalk", false);
                        StopCoroutine(m_TalkCoroutine);
                    }
                }
                else
                {
                    m_Animator.SetBool("IdleToWalk", true);
                    m_NowDestination = "AcademyEntrance";
                    Vector3 entrancePos = GameObject.Find("AcademyEntrance").transform.position;
                    m_Agent.SetDestination(entrancePos);

                    if (!m_IsCoroutineRunning)
                    {
                        int randomTalk = Random.Range(1, 19);
                        if (randomTalk <= 6)
                        {
                            m_IsCoroutineRunning = true;
                            StartCoroutine(VacationTalk());
                        }
                    }
                }
            }

            if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 4 &&
                GameTime.Instance.FlowTime.NowDay == 5)
            {
                if (m_NowDestination == "MyPosition")
                {
                    Vector3 myPos = gameObject.transform.position;
                    Vector3 desPos = m_Agent.destination;

                    float dis = Vector3.Distance(myPos, desPos);

                    if (dis < DesChangeDis)
                    {
                        m_NowDestination = " ";
                        m_Animator.SetBool("IdleToWalk", false);
                    }
                }
                else
                {
                    StopCoroutine(m_TalkCoroutine);
                    m_Animator.SetBool("IdleToWalk", true);
                    m_NowDestination = "MyPosition";
                    m_Agent.SetDestination(m_MyPosition.position);
                }
            }
        }
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

        m_IsCoroutineRunning = false;
        InGameObjectPool.ReturnChatObject(chat);
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
