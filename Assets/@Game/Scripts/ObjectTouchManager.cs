using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectTouchManager : MonoBehaviour
{
    [Header("장르방들 클릭 인식 할 박스")]
    [SerializeField] private List<GameObject> m_GenreRoomBox;

    [Header("장르방들 애니메이션, 이펙트")]
    [SerializeField] private List<GameObject> m_PuzzleRoomObject;
    [SerializeField] private List<GameObject> m_AdventureRoomObject;
    [SerializeField] private List<GameObject> m_SportsRoomObject;
    [SerializeField] private List<GameObject> m_ActionRoomObject;
    [SerializeField] private List<GameObject> m_ShootingRoomObject;

    [SerializeField] private List<ParticleSystem> m_RhythmRoomEffects;
    [SerializeField] private ParticleSystem m_ShootingEffect;
    [SerializeField] private ParticleSystem m_AdventureEffect1;
    [SerializeField] private ParticleSystem m_AdventureEffect2;

    [SerializeField] private Animator m_Dragon;
    [SerializeField] private Animator m_Knight;


    private int m_CameraLayerMask = 1 << 7;
    private int m_UILayerMask = 1 << 5;

    [SerializeField] private float m_BasketBallShootSpeed;

    private List<IEnumerator> m_AnimationResetCoroutines = new List<IEnumerator>();
    private List<bool> m_IsCoroutineRunning = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
        m_AnimationResetCoroutines.Add(null);
        m_AnimationResetCoroutines.Add(null);
        m_AnimationResetCoroutines.Add(null);
        m_AnimationResetCoroutines.Add(null);
        m_AnimationResetCoroutines.Add(null);
        m_AnimationResetCoroutines.Add(null);
        m_AnimationResetCoroutines.Add(null);
        m_AnimationResetCoroutines.Add(null);

        m_IsCoroutineRunning.Add(false);
        m_IsCoroutineRunning.Add(false);
        m_IsCoroutineRunning.Add(false);
        m_IsCoroutineRunning.Add(false);
        m_IsCoroutineRunning.Add(false);
        m_IsCoroutineRunning.Add(false);
        m_IsCoroutineRunning.Add(false);
        m_IsCoroutineRunning.Add(false);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
        TouchGenreRoomAtEditor();

#elif UNITY_ANDROID
        TouchGenreRoomAtAndroid();
#endif
    }

    private void TouchGenreRoomAtEditor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask))
            {
                if (EventSystem.current.IsPointerOverGameObject() == true)
                    return;

                if (hit.transform.tag != "GenreRoomBox")
                    return;

                if (Time.timeScale == 0)
                    return;

                int randomSound = 0;
                GameObject effect1;
                GameObject effect2;
                switch (hit.transform.name)
                {
                    case "PuzzleCenter":
                        //randomSound = Random.Range(0, ClickEventManager.Instance.Sound.PuzzleClips.Count);
                        //ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.PuzzleRoom, randomSound);
                        m_PuzzleRoomObject[0].GetComponent<RocketTouch>().TouchOnce();
                        PlayerInfo.Instance.PuzzleCenterCount++;
                        break;

                    case "SimulationCenter":
                        randomSound = Random.Range(0, ClickEventManager.Instance.Sound.SimulationClips.Count);
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SimulationRoom, randomSound);
                        PlayerInfo.Instance.SimulationCenterCount++;
                        break;

                    case "RhythmCenter":
                        int randomTouchEvent = Random.Range(1, 4);
                        if (randomTouchEvent == 1)
                        {
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RhythmRoom, 0);
                        }
                        else if (randomTouchEvent == 2)
                        {
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RhythmRoom, 1);
                        }
                        else if (randomTouchEvent == 3)
                        {
                            foreach (var rhythmEffect in m_RhythmRoomEffects)
                            {
                                rhythmEffect.Play();
                            }
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RhythmRoom, 2);
                        }
                        PlayerInfo.Instance.RhythmCenterCount++;
                        break;

                    case "AdventureCenter":
                        int randomTouch;
                        if (InteractionManager.Instance.GenreRoomList[(int)InteractionManager.SpotName.AdventureRoom]
                                .Level == 1)
                        {
                            randomTouch = Random.Range(1, 3);
                            if (randomTouch == 1)
                            {
                                m_AdventureRoomObject[0].GetComponent<Animator>().Rebind();
                                m_AdventureRoomObject[0].GetComponent<Animator>().enabled = false;
                                m_AdventureRoomObject[0].GetComponent<Animator>().enabled = true;

                                m_AdventureEffect1.Play();
                                ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 0);
                            }
                            else
                            {
                                ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 1);
                            }
                        }
                        else
                        {
                            randomTouch = Random.Range(1, 4);
                            if (randomTouch == 1)
                            {
                                m_AdventureRoomObject[0].GetComponent<Animator>().Rebind();
                                m_AdventureRoomObject[0].GetComponent<Animator>().enabled = false;
                                m_AdventureRoomObject[0].GetComponent<Animator>().enabled = true;

                                m_AdventureEffect1.Play();
                                ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 0);
                            }
                            else if (randomTouch == 2)
                            {
                                m_AdventureRoomObject[1].GetComponent<Animator>().Rebind();
                                m_AdventureRoomObject[1].GetComponent<Animator>().enabled = false;
                                m_AdventureRoomObject[1].GetComponent<Animator>().enabled = true;

                                m_AdventureEffect2.Play();
                                ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 0);
                            }
                            else
                            {
                                ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 1);
                            }
                        }
                        PlayerInfo.Instance.AdventureCenterCount++;
                        break;

                    case "RPGCenter":
                        if (m_IsCoroutineRunning[4])
                        {
                            m_Dragon.Rebind();
                            m_Knight.Rebind();
                            StopCoroutine(m_AnimationResetCoroutines[4]);
                            m_AnimationResetCoroutines[6] = null;
                            m_Dragon.enabled = false;
                            m_Dragon.enabled = true;
                            m_Knight.enabled = false;
                            m_Knight.enabled = true;
                        }
                        m_Dragon.SetBool("IdleToAttack", true);
                        m_Knight.SetBool("IdleToAttack", true);

                        m_AnimationResetCoroutines[4] = ResetRPGRoom();

                        StartCoroutine(m_AnimationResetCoroutines[4]);
                        m_IsCoroutineRunning[4] = true;
                        PlayerInfo.Instance.RPGCenterCount++;
                        break;

                    case "SportsCenter":
                        randomSound = Random.Range(0, ClickEventManager.Instance.Sound.SportsClips.Count);
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SportsRoom, randomSound);
                        float randomX = Random.Range(-1f, 1f);
                        float randomY = Random.Range(-1f, 1f);
                        float randomZ = Random.Range(-1f, 1f);
                        Vector3 randomShoot = new Vector3(randomX, randomY, randomZ).normalized;
                        m_SportsRoomObject[0].GetComponent<Rigidbody>().velocity = randomShoot * m_BasketBallShootSpeed;
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SportsRoom, 0);
                        PlayerInfo.Instance.SportsCenterCount++;
                        break;

                    case "ActionCenter":
                        int randomSports = Random.Range(1, 3);
                        if (randomSports == 1)
                        {
                            if (m_IsCoroutineRunning[6])
                            {
                                m_ActionRoomObject[0].GetComponent<Animator>().Rebind();
                                m_ActionRoomObject[1].GetComponent<Animator>().Rebind();
                                StopCoroutine(m_AnimationResetCoroutines[6]);
                                m_AnimationResetCoroutines[6] = null;
                                m_ActionRoomObject[0].GetComponent<Animator>().enabled = false;
                                m_ActionRoomObject[0].GetComponent<Animator>().enabled = true;
                                m_ActionRoomObject[1].GetComponent<Animator>().enabled = false;
                                m_ActionRoomObject[1].GetComponent<Animator>().enabled = true;
                            }
                            m_ActionRoomObject[0].GetComponent<Animator>().SetBool("IdleToHook", true);
                            m_ActionRoomObject[1].GetComponent<Animator>().SetBool("IdleToBoxing", true);
                            m_AnimationResetCoroutines[6] = ResetActionRoom();
                            StartCoroutine(m_AnimationResetCoroutines[6]);
                            m_IsCoroutineRunning[6] = true;
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ActionRoom, 0);
                        }
                        else
                        {
                            if (m_IsCoroutineRunning[6])
                            {
                                StopCoroutine(m_AnimationResetCoroutines[6]);
                                m_ActionRoomObject[0].GetComponent<Animator>().SetBool("IdleToHook", false);
                                m_ActionRoomObject[1].GetComponent<Animator>().SetBool("IdleToBoxing", false);
                                m_IsCoroutineRunning[6] = false;
                            }
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ActionRoom, 1);
                        }
                        PlayerInfo.Instance.ActionCenterCount++;
                        break;

                    case "ShootingCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ShootingRoom, 0);
                        m_ShootingEffect.Play();
                        PlayerInfo.Instance.ShootingCenterCount++;
                        break;
                }
                PlayerInfo.Instance.GenreRoomClickCount++;
            }
        }
    }
    

    IEnumerator ResetActionRoom()
    {
        yield return new WaitForSeconds(3.0f);

        m_ActionRoomObject[0].GetComponent<Animator>().SetBool("IdleToHook", false);
        m_ActionRoomObject[1].GetComponent<Animator>().SetBool("IdleToBoxing", false);
        m_IsCoroutineRunning[6] = false;
    }

    IEnumerator ResetRPGRoom()
    {
        yield return new WaitForSeconds(1.0f);

        m_Dragon.SetBool("IdleToAttack", false);
        m_Knight.GetComponent<Animator>().SetBool("IdleToAttack", false);
        m_IsCoroutineRunning[4] = false;
    }

    private void TouchGenreRoomAtAndroid()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector3 touchPos = Input.GetTouch(0).position;
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask))
            {
                if (EventSystem.current.IsPointerOverGameObject(0) == true)
                    return;

                if (hit.transform.tag != "GenreRoomBox")
                    return;

                if (Time.timeScale == 0)
                    return;

                int randomSound = 0;
                GameObject effect1;
                GameObject effect2;
                switch (hit.transform.name)
                {
                    case "PuzzleCenter":
                    //randomSound = Random.Range(0, ClickEventManager.Instance.Sound.PuzzleClips.Count);
                    //ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.PuzzleRoom, randomSound);
                    m_PuzzleRoomObject[0].GetComponent<RocketTouch>().TouchOnce();
                    PlayerInfo.Instance.PuzzleCenterCount++;
                    break;

                    case "SimulationCenter":
                    randomSound = Random.Range(0, ClickEventManager.Instance.Sound.SimulationClips.Count);
                    ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SimulationRoom, randomSound);
                    PlayerInfo.Instance.SimulationCenterCount++;
                    break;

                    case "RhythmCenter":
                    int randomTouchEvent = Random.Range(1, 4);
                    if (randomTouchEvent == 1)
                    {
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RhythmRoom, 0);
                    }
                    else if (randomTouchEvent == 2)
                    {
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RhythmRoom, 1);
                    }
                    else if (randomTouchEvent == 3)
                    {
                        foreach (var rhythmEffect in m_RhythmRoomEffects)
                        {
                            rhythmEffect.Play();
                        }
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RhythmRoom, 2);
                    }
                    PlayerInfo.Instance.RhythmCenterCount++;
                    break;

                    case "AdventureCenter":
                    int randomTouch;
                    if (InteractionManager.Instance.GenreRoomList[(int)InteractionManager.SpotName.AdventureRoom]
                            .Level == 1)
                    {
                        randomTouch = Random.Range(1, 3);
                        if (randomTouch == 1)
                        {
                            m_AdventureRoomObject[0].GetComponent<Animator>().Rebind();
                            m_AdventureRoomObject[0].GetComponent<Animator>().enabled = false;
                            m_AdventureRoomObject[0].GetComponent<Animator>().enabled = true;

                            m_AdventureEffect1.Play();
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 0);
                        }
                        else
                        {
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 1);
                        }
                    }
                    else
                    {
                        randomTouch = Random.Range(1, 4);
                        if (randomTouch == 1)
                        {
                            m_AdventureRoomObject[0].GetComponent<Animator>().Rebind();
                            m_AdventureRoomObject[0].GetComponent<Animator>().enabled = false;
                            m_AdventureRoomObject[0].GetComponent<Animator>().enabled = true;

                            m_AdventureEffect1.Play();
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 0);
                        }
                        else if (randomTouch == 2)
                        {
                            m_AdventureRoomObject[1].GetComponent<Animator>().Rebind();
                            m_AdventureRoomObject[1].GetComponent<Animator>().enabled = false;
                            m_AdventureRoomObject[1].GetComponent<Animator>().enabled = true;

                            m_AdventureEffect2.Play();
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 0);
                        }
                        else
                        {
                            ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 1);
                        }
                    }
                    PlayerInfo.Instance.AdventureCenterCount++;
                    break;

                    case "RPGCenter":
                    if (m_IsCoroutineRunning[4])
                    {
                        m_Dragon.Rebind();
                        m_Knight.Rebind();
                        StopCoroutine(m_AnimationResetCoroutines[4]);
                        m_AnimationResetCoroutines[6] = null;
                        m_Dragon.enabled = false;
                        m_Dragon.enabled = true;
                        m_Knight.enabled = false;
                        m_Knight.enabled = true;
                    }
                    m_Dragon.SetBool("IdleToAttack", true);
                    m_Knight.SetBool("IdleToAttack", true);

                    m_AnimationResetCoroutines[4] = ResetRPGRoom();

                    StartCoroutine(m_AnimationResetCoroutines[4]);
                    m_IsCoroutineRunning[4] = true;
                    PlayerInfo.Instance.RPGCenterCount++;
                    break;

                    case "SportsCenter":
                    randomSound = Random.Range(0, ClickEventManager.Instance.Sound.SportsClips.Count);
                    ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SportsRoom, randomSound);
                    float randomX = Random.Range(-1f, 1f);
                    float randomY = Random.Range(-1f, 1f);
                    float randomZ = Random.Range(-1f, 1f);
                    Vector3 randomShoot = new Vector3(randomX, randomY, randomZ).normalized;
                    m_SportsRoomObject[0].GetComponent<Rigidbody>().velocity = randomShoot * m_BasketBallShootSpeed;
                    ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SportsRoom, 0);
                    PlayerInfo.Instance.SportsCenterCount++;
                    break;

                    case "ActionCenter":
                    int randomSports = Random.Range(1, 3);
                    if (randomSports == 1)
                    {
                        if (m_IsCoroutineRunning[6])
                        {
                            m_ActionRoomObject[0].GetComponent<Animator>().Rebind();
                            m_ActionRoomObject[1].GetComponent<Animator>().Rebind();
                            StopCoroutine(m_AnimationResetCoroutines[6]);
                            m_AnimationResetCoroutines[6] = null;
                            m_ActionRoomObject[0].GetComponent<Animator>().enabled = false;
                            m_ActionRoomObject[0].GetComponent<Animator>().enabled = true;
                            m_ActionRoomObject[1].GetComponent<Animator>().enabled = false;
                            m_ActionRoomObject[1].GetComponent<Animator>().enabled = true;
                        }
                        m_ActionRoomObject[0].GetComponent<Animator>().SetBool("IdleToHook", true);
                        m_ActionRoomObject[1].GetComponent<Animator>().SetBool("IdleToBoxing", true);
                        m_AnimationResetCoroutines[6] = ResetActionRoom();
                        StartCoroutine(m_AnimationResetCoroutines[6]);
                        m_IsCoroutineRunning[6] = true;
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ActionRoom, 0);
                    }
                    else
                    {
                        if (m_IsCoroutineRunning[6])
                        {
                            StopCoroutine(m_AnimationResetCoroutines[6]);
                            m_ActionRoomObject[0].GetComponent<Animator>().SetBool("IdleToHook", false);
                            m_ActionRoomObject[1].GetComponent<Animator>().SetBool("IdleToBoxing", false);
                            m_IsCoroutineRunning[6] = false;
                        }
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ActionRoom, 1);
                    }
                    PlayerInfo.Instance.ActionCenterCount++;
                    break;

                    case "ShootingCenter":
                    ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ShootingRoom, 0);
                    m_ShootingEffect.Play();
                    PlayerInfo.Instance.ShootingCenterCount++;
                    break;
                }
                PlayerInfo.Instance.GenreRoomClickCount++;
            }
        }
    }
}
