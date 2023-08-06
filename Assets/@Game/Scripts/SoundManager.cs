using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("오디오 재생 대상")]
    [SerializeField] private AudioSource m_MainAudioSource;         // 메인 BGM 용
    [SerializeField] private AudioSource m_Main2AudioSource;        // 미니게임 BGM 용
    [SerializeField] private List<AudioSource> m_SubAudioSources;   // 20개 사용

    [Header("타이틀")]
    [SerializeField] private AudioClip m_TitleAudioClip;

    [Header("인게임")]
    [SerializeField] private AudioClip m_InGameAudioClip;

    [Header("홍보 미니게임")]
    [SerializeField] private AudioClip m_LeafletBGM;
    [SerializeField] private AudioClip m_WrongTouch;
    [SerializeField] private AudioClip m_CorrectTouch;
    [SerializeField] private AudioClip m_Result;
    [SerializeField] private AudioClip m_SpeedUp;
    [SerializeField] private AudioClip m_Finish;
    [SerializeField] private AudioClip m_Dash;
    [SerializeField] private AudioClip m_Ready;
    [SerializeField] private AudioClip m_Start;

    [Header("청소 미니게임")]
    [SerializeField] private AudioClip m_StickCleaning;
    [SerializeField] private AudioClip m_Dust2Touch;
    [SerializeField] private AudioClip m_Dust1Touch;

    [Header("게임잼 미니게임")]
    [SerializeField] private AudioClip m_GameJameMiniGame_BGM;
    [SerializeField] private AudioClip m_GameJameMiniGame_BugCatch;
    [SerializeField] private AudioClip m_GameJameMiniGame_ClockTicking;
    [SerializeField] private AudioClip m_GameJameMiniGame_TouchWrong;
    [SerializeField] private AudioClip m_GameJameMiniGame_Excellent;
    [SerializeField] private AudioClip m_GameJameMiniGame_Good;
    [SerializeField] private AudioClip m_GameJameMiniGame_Bad;


    [Header("게임쇼")]
    [SerializeField] private AudioClip m_GameShowBGM;

    [Header("버튼")]
    [SerializeField] private AudioClip m_IconTouch;
    [SerializeField] private AudioClip m_CloseIconTouch;
    [SerializeField] private AudioClip m_TouchEvent;

    [Header("보상 실패")]
    [SerializeField] private AudioClip m_FailSound;

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

    [Header("강사 업그레이드 사운드")]
    [SerializeField] private AudioClip m_ProfessorUpgradeClip;

    [Header("알람 사운드")]
    [SerializeField] private AudioClip m_AlarmClip;

    [Header("장르방 업그레이드 사운드")]
    [SerializeField] private AudioClip m_GenreRoomUpgradeClip;

    [Header("장르방 터치 사운드")]
    [SerializeField] private List<AudioClip> m_PuzzleClip = new List<AudioClip>();
    [SerializeField] private List<AudioClip> m_SimulationClip = new List<AudioClip>();
    [SerializeField] private List<AudioClip> m_RhythmClip = new List<AudioClip>();
    [SerializeField] private List<AudioClip> m_AdventureClip = new List<AudioClip>();
    [SerializeField] private List<AudioClip> m_RPGClip = new List<AudioClip>();
    [SerializeField] private List<AudioClip> m_SportsClip = new List<AudioClip>();
    [SerializeField] private List<AudioClip> m_ActionClip = new List<AudioClip>();
    [SerializeField] private List<AudioClip> m_ShootingClip = new List<AudioClip>();

    public AudioSource MainAudioSource
    {
        get { return m_MainAudioSource; }
        set { m_MainAudioSource = value; }
    }

    public List<AudioSource> SubAudioSources
    {
        get { return m_SubAudioSources; }
        set { m_SubAudioSources = value; }
    }

    public List<AudioClip> PuzzleClips
    {
        get { return m_PuzzleClip; }
    }
    public List<AudioClip> SimulationClips
    {
        get { return m_SimulationClip; }
    }
    public List<AudioClip> RhythmClips
    {
        get { return m_RhythmClip; }
    }
    public List<AudioClip> AdventureClips
    {
        get { return m_AdventureClip; }
    }
    public List<AudioClip> RPGClips
    {
        get { return m_RPGClip; }
    }
    public List<AudioClip> SportsClips
    {
        get { return m_SportsClip; }
    }
    public List<AudioClip> ActionClips
    {
        get { return m_ActionClip; }
    }
    public List<AudioClip> ShootingClips
    {
        get { return m_ShootingClip; }
    }

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

        m_MainAudioSource.clip = m_InGameAudioClip;
        m_MainAudioSource.loop = true;
        m_MainAudioSource.Play();
    }

    public void StopInGameBgm()
    {
        if (m_MainAudioSource.isPlaying)
        {
            m_MainAudioSource.Pause();
            //m_MainAudioSource.Stop();
        }
    }

    public void ReplayInGameBgm()
    {
        m_MainAudioSource.Play();
    }

    public void PlayGameShowBgm()
    {
        if (m_GameShowBGM == null)
        {
            Debug.Log("게임쇼 bgm이 없습니다");
            return;
        }

        //m_MainAudioSource.PlayOneShot(m_InGameAudioClip);
        m_Main2AudioSource.clip = m_GameShowBGM;
        m_Main2AudioSource.loop = true;
        m_Main2AudioSource.Play();
    }

    public void StopMain2Audio()
    {
        if (m_Main2AudioSource.isPlaying)
        {
            m_Main2AudioSource.Stop();
        }
    }

    /// 미니게임 Bgm
    public void PlayGameJamMiniGameBgm()
    {
        if (m_GameJameMiniGame_BGM == null)
        {
            Debug.Log("게임잼 미니게임 bgm이 없습니다");
            return;
        }
        
        m_Main2AudioSource.clip = m_GameJameMiniGame_BGM;
        m_Main2AudioSource.loop = true;
        m_Main2AudioSource.Play();
    }

    public void PlayBugCatch()
    {
        if (m_GameJameMiniGame_BugCatch == null)
        {
            Debug.Log("버그캐치 사운드가 없습니다");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_GameJameMiniGame_BugCatch);
        m_SubAudioCount++;
        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayGameJamTouchWrong()
    {
        if (m_GameJameMiniGame_TouchWrong == null)
        {
            Debug.Log("버그캐치 사운드가 없습니다");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_GameJameMiniGame_TouchWrong);
        m_SubAudioCount++;
        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayExcellent()
    {
        if (m_GameJameMiniGame_Excellent == null)
        {
            Debug.Log("엑셀런트 사운드가 없습니다");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_GameJameMiniGame_Excellent);
        m_SubAudioCount++;
        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayGood()
    {
        if (m_GameJameMiniGame_Good== null)
        {
            Debug.Log("엑셀런트 사운드가 없습니다");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_GameJameMiniGame_Good);
        m_SubAudioCount++;
        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayBad()
    {
        if (m_GameJameMiniGame_Bad == null)
        {
            Debug.Log("엑셀런트 사운드가 없습니다");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_GameJameMiniGame_Bad);
        m_SubAudioCount++;
        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
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

    public void PlayFailSound()
    {
        if (m_FailSound == null)
        {
            Debug.Log("실패 사운드가 없습니다.");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_FailSound);
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

    public void PlayAlarmSound()
    {
        if (m_AlarmClip == null)
        {
            Debug.Log("버튼3 사운드가 없습니다");
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_AlarmClip);
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

    public void PlayProfessorUpgradeSound()
    {
        if (m_ProfessorUpgradeClip == null)
        {
            Debug.Log("강사 업그레이드 사운드가 없습니다.");
            return;
        }
        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_ProfessorUpgradeClip);
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

    public void PlayLeafletBGM()
    {
        if (m_LeafletBGM == null)
        {
            return;
        }

        m_Main2AudioSource.clip = m_LeafletBGM;
        m_Main2AudioSource.loop = true;
        m_Main2AudioSource.Play();
    }

    public void PlayWrongTouchSound()
    {
        if (m_WrongTouch == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_WrongTouch);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayCorrectTouchSound()
    {
        if (m_CorrectTouch == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_CorrectTouch);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayDashSound()
    {
        if (m_Dash == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_Dash);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayResultSound()
    {
        if (m_Result == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_Result);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    // gamejam : clockticking
    public void PlayClockTickingSound()
    {
        if (m_GameJameMiniGame_ClockTicking == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_GameJameMiniGame_ClockTicking);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlaySpeedUpSound()
    {
        if (m_SpeedUp == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_SpeedUp);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }


    public void PlayLeafletFinishSound()
    {
        if (m_Finish == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_Finish);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;

        StopInGameBgm();
    }

    public void PlayReadySound()
    {
        if (m_Ready == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_Ready);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayStartSound()
    {
        if (m_Start == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_Start);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayStickCleaning()
    {
        if (m_StickCleaning == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_StickCleaning);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayDust1Touch()
    {
        if (m_Dust1Touch == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_Dust1Touch);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayDust2Touch()
    {
        if (m_Dust2Touch == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_Dust2Touch);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }

    public void PlayGenreRoomUpgradeSound()
    {
        if (m_GenreRoomUpgradeClip == null)
        {
            return;
        }

        m_SubAudioSources[m_SubAudioCount].PlayOneShot(m_GenreRoomUpgradeClip);
        m_SubAudioCount++;

        if (m_SubAudioCount >= m_MaxSubAudioCount)
            m_SubAudioCount = 0;
    }
}
