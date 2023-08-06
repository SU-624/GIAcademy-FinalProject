using System.Collections;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class RocketTouch : MonoBehaviour
{
    [SerializeField] private GameObject m_NormalEffect1;
    [SerializeField] private GameObject m_NormalEffect2;
    [SerializeField] private GameObject m_NormalEffect3;

    [SerializeField] private GameObject m_TouchEffect;
    [SerializeField] private GameObject m_BangEffect;
    [SerializeField] private float m_TouchResetTime;
    [SerializeField] private List<GameObject> m_GoalEffect = new List<GameObject>();
    [SerializeField] private List<PathCreator> m_PathList = new List<PathCreator>();

    private PathFollower m_PathFollower;
    private MoveAlongBezier m_MoveAlong;

    private bool m_FireOnce;
    private ParticleSystem m_TouchFireEffect;

    private int m_TouchCount;

    private float m_Timer;

    private bool m_IsLauncing;


    // Start is called before the first frame update
    void Start()
    {
        m_FireOnce = false;
        m_TouchFireEffect = m_TouchEffect.GetComponent<ParticleSystem>();
        m_TouchCount = 0;
        m_Timer = 0;
        m_IsLauncing = false;
        m_PathFollower = this.gameObject.GetComponent<PathFollower>();
        m_MoveAlong = this.gameObject.GetComponent<MoveAlongBezier>();
        m_PathFollower.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_FireOnce && m_TouchEffect.activeSelf && !m_TouchFireEffect.isPlaying)
        {
            m_NormalEffect1.SetActive(true);
            m_NormalEffect2.SetActive(true);
            m_NormalEffect3.SetActive(true);

            m_TouchEffect.SetActive(false);
        }

        if (!m_IsLauncing)
        {
            if (m_TouchCount >= 5)
            {
                StartCoroutine(RocketLaunch());
            }
            else if (m_TouchCount > 0)
            {
                m_Timer += Time.deltaTime;

                if (m_Timer >= m_TouchResetTime)
                {
                    m_TouchCount = 0;
                    m_Timer = 0;
                }
            }
            else
            {
                m_Timer = 0;
            }
        }
    }

    public void TouchOnce()
    {
        if (!m_IsLauncing)
        {
            m_NormalEffect1.SetActive(false);
            m_NormalEffect2.SetActive(false);
            m_NormalEffect3.SetActive(false);

            m_TouchEffect.SetActive(true);

            var fireEffectMain = m_TouchFireEffect.main;
            fireEffectMain.loop = false;
            m_TouchFireEffect.Play();

            m_FireOnce = true;
            m_TouchCount++;

            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.PuzzleRoom, 0);
        }
    }

    IEnumerator RocketLaunch()
    {
        m_IsLauncing = true;
        m_NormalEffect1.SetActive(false);
        m_NormalEffect2.SetActive(false);
        m_NormalEffect3.SetActive(false);

        m_TouchEffect.SetActive(true);
        var fireEffectMain = m_TouchFireEffect.main;
        fireEffectMain.loop = true;

        int randomPath = Random.Range(0, m_PathList.Count);
        m_PathFollower.distanceTravelled = 0;
        m_PathFollower.pathCreator = m_PathList[randomPath];
        m_PathList[randomPath].gameObject.SetActive(true);
        m_MoveAlong.enabled = false;
        m_PathFollower.enabled = true;
        this.transform.position = m_PathList[randomPath].bezierPath.GetPoint(0);
        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.PuzzleRoom, 1);

        float waitSecond = 0;

        switch (randomPath)
        {
            case 0:
                waitSecond = 2.7f;
                break;
            case 1:
                waitSecond = 2.2f;
                break;
            case 2:
                waitSecond = 2.9f;
                break;
            case 3:
                waitSecond = 2.2f;
                break;
            case 4:
                waitSecond = 3.5f;
                break;
            case 5:
                waitSecond = 3.2f;
                break;
            case 6:
                waitSecond = 3.0f;
                break;
        }

        yield return new WaitForSeconds(waitSecond);

        m_BangEffect.SetActive(true);
        m_PathFollower.enabled = false;
        m_MoveAlong.enabled = true;

        yield return new WaitUntil(() => !m_BangEffect.GetComponent<ParticleSystem>().isPlaying);
        
        m_BangEffect.SetActive(false);
        m_PathList[randomPath].gameObject.SetActive(false);
        m_PathFollower.pathCreator = null;
        m_IsLauncing = false;
        m_TouchCount = 0;
    }
}
