using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("오디오 재생 대상")] 
    [SerializeField] private AudioSource m_MainAudioSource;         // 메인 BGM 용
    [SerializeField] private List<AudioSource> m_SubAudioSources;   // 20개 사용

    [Header("타이틀")]
    [SerializeField] private AudioClip m_TitleAudioClip;

    [Header("인게임")]
    [SerializeField] private AudioClip m_InGameAudioClip;

    [Header("버튼")]
    [SerializeField] private AudioClip m_IconTouch;
    [SerializeField] private AudioClip m_CloseIconTouch;
    [SerializeField] private AudioClip m_TouchEvent;

    [Header("이벤트")]
    [SerializeField] private AudioClip m_EventClear;
    [SerializeField] private AudioClip m_EventDoing;

    [Header("수리")]
    [SerializeField] private AudioClip m_Repair1;
    [SerializeField] private AudioClip m_Repair2;

    [Header("재화 획득")]
    [SerializeField] private AudioClip m_MoneyClip;
    [SerializeField] private AudioClip m_SPClip;
    [SerializeField] private AudioClip m_MoneyJackpotClip;

    [Header("장르방 터치 사운드")]
    [SerializeField] private List<AudioClip> m_PuzzleClip;
    [SerializeField] private List<AudioClip> m_SimulationClip;
    [SerializeField] private List<AudioClip> m_RhythmClip;
    [SerializeField] private List<AudioClip> m_AdventureClip;
    [SerializeField] private List<AudioClip> m_RPGClip;
    [SerializeField] private List<AudioClip> m_SportsClip;
    [SerializeField] private List<AudioClip> m_ActionClip;
    [SerializeField] private List<AudioClip> m_ShootingClip;

    private int m_SubAudioCount;
    private const int m_MaxSubAudioCount = 20;

    // Start is called before the first frame update
    void Start()
    {
        //PlayInGameBgm();
        m_SubAudioCount = 0;
    }

    public void PlayTitleBgm()
    {
        if (m_TitleAudioClip == null)
        {
            Debug.Log("타이틀 bgm이 없습니다");
            return;
        }

        m_MainAudioSource.clip = m_TitleAudioClip;
        m_MainAudioSource.loop = true;
        m_MainAudioSource.Play();
    }

    public void StopTitleBgm()
    {
        if (m_MainAudioSource.isPlaying)
        {
            m_MainAudioSource.Stop();
        }
    }

    public void PlayInGameBgm()
    {
        if (m_InGameAudioClip == null)
        {
            Debug.Log("인게임 bgm이 없습니다");
            return;
        }

        //m_MainAudioSource.PlayOneShot(m_InGameAudioClip);
        m_MainAudioSource.clip = m_InGameAudioClip;
        m_MainAudioSource.loop = true;
        m_MainAudioSource.Play();
    }

    public void StopInGameBgm()
    {
        if (m_MainAudioSource.isPlaying)
        {
            m_MainAudioSource.Stop();
        }
    }

    public void PlayIconTouch()
    {
        if (m_IconTouch == null)
        {
            Debug.Log("버튼1 사운드가 없습니다");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_IconTouch);
        m_SubAudioCount++;
        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayCloseIcon()
    {
        if (m_CloseIconTouch == null)
        {
            Debug.Log("버튼2 사운드가 없습니다");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_CloseIconTouch);
        m_SubAudioCount++;
        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayTouchEvent()
    {
        if (m_TouchEvent == null)
        {
            Debug.Log("버튼3 사운드가 없습니다");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_TouchEvent);
        m_SubAudioCount++;
        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayRepairSound()
    {
        if (m_Repair1 == null)
        {
            Debug.Log("수리1 사운드가 없습니다");
            return;
        }
        if (m_Repair2 == null)
        {
            Debug.Log("수리2 사운드가 없습니다");
            return;
        }
        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_Repair1);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_Repair2);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayMoneySound()
    {
        if (m_MoneyClip == null)
        {
            Debug.Log("돈 획득 사운드가 없습니다");
            return;
        }
        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_MoneyClip);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayMoneyJackpotSound()
    {
        if (m_MoneyJackpotClip == null)
        {
            Debug.Log("잭팟 사운드가 없습니다");
            return;
        }
        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_MoneyJackpotClip);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlaySPSound()
    {
        if (m_SPClip == null)
        {
            Debug.Log("2차재화 획득 사운드가 없습니다");
            return;
        }
        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_SPClip);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayGenreRoomSound(InteractionManager.SpotName genreRoom, int soundNum)
    {
        switch (genreRoom)
        {
            case InteractionManager.SpotName.PuzzleRoom:
                if (m_PuzzleClip == null)
                    return;

                m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_PuzzleClip[soundNum]);
                m_SubAudioCount++;

                if (m_SubAudioCount >= m_MaxSubAudioCount)
                    m_SubAudioCount = 0;
                break;

            case InteractionManager.SpotName.SimulationRoom:
                if (m_SimulationClip == null)
                    return;

                m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_SimulationClip[soundNum]);
                m_SubAudioCount++;

                if (m_SubAudioCount >= m_MaxSubAudioCount)
                    m_SubAudioCount = 0;
                break;

            case InteractionManager.SpotName.RhythmRoom:
                if (m_RhythmClip == null)
                    return;

                m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_RhythmClip[soundNum]);
                m_SubAudioCount++;

                if (m_SubAudioCount >= m_MaxSubAudioCount)
                    m_SubAudioCount = 0;
                break;

            case InteractionManager.SpotName.AdventureRoom:
                if (m_AdventureClip == null)
                    return;

                m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_AdventureClip[soundNum]);
                m_SubAudioCount++;

                if (m_SubAudioCount >= m_MaxSubAudioCount)
                    m_SubAudioCount = 0;
                break;

            case InteractionManager.SpotName.RPGRoom:
                if (m_RPGClip == null)
                    return;

                m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_RPGClip[soundNum]);
                m_SubAudioCount++;

                if (m_SubAudioCount >= m_MaxSubAudioCount)
                    m_SubAudioCount = 0;
                break;

            case InteractionManager.SpotName.SportsRoom:
                if (m_SportsClip == null)
                    return;

                m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_SportsClip[soundNum]);
                m_SubAudioCount++;

                if (m_SubAudioCount >= m_MaxSubAudioCount)
                    m_SubAudioCount = 0;
                break;

            case InteractionManager.SpotName.ActionRoom:
                if (m_ActionClip == null)
                    return;

                m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_ActionClip[soundNum]);
                m_SubAudioCount++;

                if (m_SubAudioCount >= m_MaxSubAudioCount)
                    m_SubAudioCount = 0;
                break;

            case InteractionManager.SpotName.ShootingRoom:
                if (m_ShootingClip == null)
                    return;

                m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_ShootingClip[soundNum]);
                m_SubAudioCount++;

                if (m_SubAudioCount >= m_MaxSubAudioCount)
                    m_SubAudioCount = 0;
                break;
        }
    }
}
