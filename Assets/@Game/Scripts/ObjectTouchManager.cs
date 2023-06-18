using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectTouchManager : MonoBehaviour
{
    [Header("장르방들 클릭 인식 할 박스")]
    [SerializeField] private List<GameObject> m_GenreRoomBox;

    [Header("장르방들 애니메이션")]
    [SerializeField] private List<Animator> m_PuzzleRoomAnimation;
    [SerializeField] private List<Animator> m_SimulationRoomAnimation;
    [SerializeField] private List<Animator> m_RhythmRoomAnimation;
    [SerializeField] private List<Animator> m_AdventureRoomAnimation;
    [SerializeField] private List<Animator> m_RPGRoomAnimation;
    [SerializeField] private List<Animator> m_SportsRoomAnimation;
    [SerializeField] private List<Animator> m_ActionRoomAnimation;
    [SerializeField] private List<Animator> m_ShootingRoomAnimation;

    private int m_CameraLayerMask = 1 << 7;
    private int m_UILayerMask = 1 << 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
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

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & ~m_UILayerMask))
            {
                if (hit.transform.tag != "GenreRoomBox")
                    return;

                if (Time.timeScale == 0)
                    return;
                
                switch (hit.transform.name)
                {
                    case "PuzzleCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.PuzzleRoom, 0);
                        break;

                    case "SimulationCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SimulationRoom, 0);
                        break;

                    case "RhythmCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RhythmRoom, 0);
                        break;

                    case "AdventureCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 0);
                        break;

                    case "RPGCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RPGRoom, 0);
                        break;

                    case "SportsCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SportsRoom, 0);
                        break;

                    case "ActionCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ActionRoom, 0);
                        break;

                    case "ShootingCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ShootingRoom, 0);
                        break;
                }
            }
        }
    }

    private void TouchGenreRoomAtAndroid()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Debug.Log("손가락을 뗄 때 들어오는가");
            
            Vector3 touchPos = Input.GetTouch(0).position;
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & ~m_UILayerMask))
            {
                if (hit.transform.tag != "GenreRoomBox")
                    return;

                if (Time.timeScale == 0)
                    return;

                switch (hit.transform.name)
                {
                    case "PuzzleCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.PuzzleRoom, 0);
                        break;

                    case "SimulationCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SimulationRoom, 0);
                        break;

                    case "RhythmCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RhythmRoom, 0);
                        break;

                    case "AdventureCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.AdventureRoom, 0);
                        break;

                    case "RPGCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.RPGRoom, 0);
                        break;

                    case "SportsCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.SportsRoom, 0);
                        break;

                    case "ActionCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ActionRoom, 0);
                        break;

                    case "ShootingCenter":
                        ClickEventManager.Instance.Sound.PlayGenreRoomSound(InteractionManager.SpotName.ShootingRoom, 0);
                        break;
                }
            }
        }
    }
}
