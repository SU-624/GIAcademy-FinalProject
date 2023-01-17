using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailObjectPool : MonoBehaviour
{
    public static MailObjectPool Instance;

    [SerializeField] private GameObject m_PoolingMailPrefab;

    Queue<GameObject> m_PoolingMailQueue = new Queue<GameObject>();

    [SerializeField] private GameObject m_PoolingPossibleEventPrefab;               // 프리팹을 만들기 위한 public 오브젝트
    [SerializeField] private GameObject m_PoolingFixedEventPrefab;
    [SerializeField] private GameObject m_PoolingSelectedEventPrefab;

    Queue<GameObject> m_PoolingPossibleEventQueue = new Queue<GameObject>();        // 사용가능 이벤트목록 - UI 관련 오브젝트풀 큐
    Queue<GameObject> m_PoolingFixedEventQueue = new Queue<GameObject>();           // 선택된 이벤트목록
    Queue<GameObject> m_PoolingSelectedEventQueue = new Queue<GameObject>();           // 고정된 이벤트목록

    // 변동 가능성 있음
    const int MaxPossibleEventCount = 10;
    const int MaxSelectedEventCount = 2;
    const int MaxFixedEventCount = 3;

    private void Awake()
    {
        Instance = this;
        Initalize(50);
    }

    void Initalize(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            m_PoolingMailQueue.Enqueue(CreateNewMail());
        }

        // 이벤트 관련 프리팹들을 만들어준다
        for (int i = 0; i < MaxPossibleEventCount; i++)
        {
            m_PoolingPossibleEventQueue.Enqueue(CreatePossibleEvents());
        }
        for (int i = 0; i < MaxFixedEventCount; i++)
        {
            m_PoolingFixedEventQueue.Enqueue(CreateFixedEvents());
        }
        for (int i = 0; i < MaxSelectedEventCount; i++)
        {
            m_PoolingSelectedEventQueue.Enqueue(CreateSelectedEvents());

        }
    }

    private GameObject CreateNewMail()
    {
        GameObject _newMail = Instantiate(m_PoolingMailPrefab);
        _newMail.gameObject.SetActive(false);
        _newMail.transform.SetParent(transform);

        return _newMail;
    }

    public static GameObject GetObject(Transform _setTransform)
    {
        if (Instance.m_PoolingMailQueue.Count > 0)
        {
            var _obj = Instance.m_PoolingMailQueue.Dequeue();
            _obj.transform.SetParent(_setTransform);
            _obj.gameObject.SetActive(true);
            return _obj;
        }
        else
        {
            var _newObj = Instance.CreateNewMail();
            _newObj.gameObject.SetActive(true);
            _newObj.transform.SetParent(_setTransform);
            return _newObj;
        }
    }

    public static void ReturnObject(GameObject _mail)
    {
        _mail.gameObject.SetActive(false);
        _mail.transform.SetParent(Instance.transform);
        Instance.m_PoolingMailQueue.Enqueue(_mail);
    }

    // 이벤트 프리팹들을 만들어주는 함수들
    private GameObject CreatePossibleEvents()
    {
        GameObject _PossibleEvent = Instantiate(m_PoolingPossibleEventPrefab);
        _PossibleEvent.gameObject.SetActive(false);
        _PossibleEvent.transform.SetParent(transform);

        return _PossibleEvent;
    }

    private GameObject CreateFixedEvents()
    {
        GameObject _FixedEvnets = Instantiate(m_PoolingFixedEventPrefab);
        _FixedEvnets.gameObject.SetActive(false);
        _FixedEvnets.transform.SetParent(transform);

        return _FixedEvnets;
    }

    private GameObject CreateSelectedEvents()
    {
        GameObject _SelectedEvents = Instantiate(m_PoolingSelectedEventPrefab);
        _SelectedEvents.gameObject.SetActive(false);
        _SelectedEvents.transform.SetParent(transform);

        return _SelectedEvents;
    }
}
