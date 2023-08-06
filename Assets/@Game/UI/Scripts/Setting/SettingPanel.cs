using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.Serialization;

/// <summary>
/// 2023. 07. 10. Mang
/// 
/// 인게임 설정창 (볼륨, 저장, 게임 등)
/// </summary>
public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Image SettingPanelPrefab; // 전체 설정패널 그 자체

    [Header("좌측 버튼 리스트")]
    [Space(5f)]
    [SerializeField]
    private Button AccountPageButton; // 좌측 계정 버튼

    [SerializeField] private Button SoundPageButton; // 좌측 사운드 버튼
    [SerializeField] private Button GamePageButton; // 좌픅 게임 버튼

    [Header("설정창 계정 페이지")]
    [Space(10f)]
    [SerializeField]
    private RectTransform AccountSetting; // 사운드창 페이지 패널

    [SerializeField] private TextMeshProUGUI AccountNumberText; // 게임회원번호 텍스트
    [Space(5f)] [SerializeField] private Button AccountLinkButton; // 계정 연동하기 버튼
    [SerializeField] private Button SaveDataButton; // 서버 저장하기 버튼
    [SerializeField] private Button AccountWithDrawButton; // 계정 탈퇴하기 버튼
    [SerializeField] private Button LoadDataButton; // 서버 불러오기 버튼


    [Header("설정창 사운드 페이지")]
    [Space(10f)]
    [SerializeField]
    private RectTransform SoundSetting; // 게임창 페이지 패널

    [SerializeField] private Slider BGMSoundSlider; // BGM 사운드 슬라이더
    [SerializeField] private Slider SFXSoundSlider; // SFX 사운드 슬라이더(효과음)


    [Header("설정창 게임 페이지")]
    [Space(10f)]
    [SerializeField]
    private RectTransform GameSetting; // 설정창 페이지 패널

    [SerializeField] private Button PushAlarmOffButton; // 푸시알람 끄기 버튼
    [SerializeField] private Button PushAlarmOnButton; // 푸시알람 켜기 버튼
    [SerializeField] private Button TeamGICreditButton; // 제작진 크레딧 버튼
    [Space(5f)] [SerializeField] private Image PushAlarmImg; // 푸시알람 이미지
    [SerializeField] private TextMeshProUGUI PushAlarmText; // 푸시알람 텍스트

    [Header("설정창 크레딧 확인 버튼")]
    [Space(10f)]
    [SerializeField] private Button EndingCreditButton;
    [SerializeField] private SettingEnding SettingEndingScript;

    [Header("랭킹 확인 버튼")]
    [Space(10f)]
    [SerializeField] private Button RankingButton;
    [SerializeField] private SettingRank SettingRanking;

    [Header("끄기버튼, 각 팝업 창 들")]
    [Space(10f)]
    [SerializeField]
    [Header("데이터 불러오기 알람 창")]
    private Button SettingPanelCloseButton; // 세팅패널 끄기 버튼

    [Space(10f)] [SerializeField] private Image IsAccountExistAlarmImg; // 푸시알람 이미지

    [Header("계정탈퇴 알람 창")]
    [Space(10f)]
    [SerializeField]
    private Image WithdrawAccountAlarmImg; // 푸시알람 이미지

    [SerializeField] private Button DeleteAccountYesButton; // 계정삭제 오케이 버튼
    [SerializeField] private Button DeleteAccountNoButton; // 계정삭제 노 버튼
    [Space(5f)] [SerializeField] private Image WithdrawAccountBufferingImg; // 계정삭제중 텍스트 알림창
    [SerializeField] private Image DeleteAccountCompleteImg; // 계정삭제완료 창

    [Header("데이터 저장하기 알람 창")]
    [Space(10f)]
    [SerializeField]
    private Image SaveDataAlarmImg; // 푸시알람 이미지

    [SerializeField] private Button SaveDataYesButton; // 데이터저장 오케이 버튼
    [SerializeField] private Button SaveDataNoButton; // 데이터저장 노 버튼
    [Space(5f)] [SerializeField] private Image SaveDataBufferingImg; // 데이터 저장중 텍스트 알림창
    [SerializeField] private Image SaveDataCompleteImg; // 데이터 저장완료 창

    [Header("데이터 불러오기 알람 창")]
    [Space(10f)]
    [SerializeField]
    private Image LoadDataAlarmImg; // 푸시알람 이미지

    [SerializeField] private Button LoadDataYesButton; // 데이터불러오기 오케이 버튼
    [SerializeField] private Button LoadDataNoButton; // 데이터불러오기 노 버튼
    [Space(5f)] [SerializeField] private Image LoadDataBufferingImg; // 데이터 불러오기중 텍스트 알림창
    [SerializeField] private Image LoadDataCompleteImg; // 데이터 불러오기완료 창

    //-----------------------------------
    [Space(10f)] [SerializeField] private PopUpUI PopUpSettingPanelPrefab; // 세팅패널 PopUp 연결
    [SerializeField] private PopOffUI PopOffSettingPanelPrefab; // 세팅패널 PopOff 연결

    [FormerlySerializedAs("FireBaseBinder")]
    [SerializeField]
    private FirebaseBinder firebaseBinder;

    [FormerlySerializedAs("GpgsBinder")]
    [SerializeField]
    private GPGSBinder gpgsBinder;

    [FormerlySerializedAs("NotificationManger")]
    [SerializeField]
    private NotificationBinder notificationManger;

    [FormerlySerializedAs("GameSoundManager")]
    [SerializeField]
    private SoundManager gameSoundManager;

    GameObject tempIClicked;

    [Space(10f)]
    // 설정창에서 볼륨을 조정해 주기 위한 변수 추가
    public AudioMixer AudioMixController;

    AudioListener temp;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        firebaseBinder.LoginState += OnChangedState;
        EndingCreditButton.onClick.AddListener(ClickCreditButton);
        RankingButton.onClick.AddListener(ClickRankButton);

        PlayerInfo.Instance.isRankButtonOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 1번째 3월 3번째 주가 된 후에 저장하기 버튼 활성화
        if (SaveDataButton.interactable == false && GameTime.Instance.Year >= 1 && GameTime.Instance.Month >= 3 &&
            GameTime.Instance.Week >= 3 && GameTime.Instance.Day >= 2)
        {
            SaveDataButton.interactable = true;
        }
    }

    private void OnChangedState(bool sign)
    {
        Debug.Log("Called onChangedState");
        AccountNumberText.text = sign ? firebaseBinder.UserId : "SignOut...";
    }

    // 세팅패널 관련 오브젝트들 초기화 함수
    public void Initialize()
    {
        // 게임이 켜진 뒤 초기화 시켜줄 것들 - 좌측 목록중에 계정 창을 띄우기
        SettingPanelPrefab.gameObject.SetActive(false);

        AccountSetting.gameObject.SetActive(true);
        SoundSetting.gameObject.SetActive(false);
        GameSetting.gameObject.SetActive(false);

        PushAlarmImg.gameObject.SetActive(false);

        // 계정 페이지의 각 알람창들 꺼주기
        SaveDataAlarmImg.gameObject.SetActive(false);
        WithdrawAccountBufferingImg.gameObject.SetActive(false);
        DeleteAccountCompleteImg.gameObject.SetActive(false);
        //
        SaveDataAlarmImg.gameObject.SetActive(false);
        SaveDataBufferingImg.gameObject.SetActive(false);
        SaveDataCompleteImg.gameObject.SetActive(false);
        //
        LoadDataAlarmImg.gameObject.SetActive(false);
        LoadDataBufferingImg.gameObject.SetActive(false);
        LoadDataCompleteImg.gameObject.SetActive(false);

        IsAccountExistAlarmImg.gameObject.SetActive(false);

        // 푸시알림 기본상태는 On으로 설정.
        ColorBlock temp = PushAlarmOnButton.colors;
        temp.normalColor = new Color(0.8584906f, 0.5312154f, 0.5312154f);
        PushAlarmOnButton.colors = temp;

        AccountNumberText.text = "SignOut...";

        InGameUI.Instance.SettingPanelButton.onClick.AddListener(IfClickSettingPanelOpenButton);
        SettingPanelCloseButton.onClick.AddListener(IfClickSettingPanelCloseButton);

        AccountPageButton.onClick.AddListener(IfClickSettingPanelIndex);
        SoundPageButton.onClick.AddListener(IfClickSettingPanelIndex);
        GamePageButton.onClick.AddListener(IfClickSettingPanelIndex);

        AccountLinkButton.onClick.AddListener(GoogleAccountLink);
        SaveDataButton.onClick.AddListener(SaveDataOnServer);
        AccountWithDrawButton.onClick.AddListener(GoogleAccountWithDraw);
        LoadDataButton.onClick.AddListener(LoadDataOnServer);

        SaveDataButton.interactable = false; // 첫 저장 전 저장 방지...

        PushAlarmOffButton.onClick.AddListener(IfClickPushAlarmSelectButton);
        PushAlarmOnButton.onClick.AddListener(IfClickPushAlarmSelectButton);

        DeleteAccountYesButton.onClick.AddListener(ClickGoogleAccountWithDrawYesNoButton);
        DeleteAccountNoButton.onClick.AddListener(ClickGoogleAccountWithDrawYesNoButton);

        SaveDataYesButton.onClick.AddListener(ClickSaveDataYesNoButton);
        SaveDataNoButton.onClick.AddListener(ClickSaveDataYesNoButton);

        LoadDataYesButton.onClick.AddListener(ClickLoadDataYesNoButton);
        LoadDataNoButton.onClick.AddListener(ClickLoadDataYesNoButton);

        SettingPanelCloseButton.onClick.AddListener(IfClickSettingPanelCloseButton);
    }

    //세팅 패널의 초기값 설정
    public void InitSettingPanelObjects()
    {
        // 게임이 켜진 뒤 초기화 시켜줄 것들 - 좌측 목록중에 계정 창을 띄우기
        SettingPanelPrefab.gameObject.SetActive(false);

        AccountSetting.gameObject.SetActive(true);
        SoundSetting.gameObject.SetActive(false);
        GameSetting.gameObject.SetActive(false);

        PushAlarmImg.gameObject.SetActive(false);

        // 계정 페이지의 각 알람창들 꺼주기
        SaveDataAlarmImg.gameObject.SetActive(false);
        WithdrawAccountBufferingImg.gameObject.SetActive(false);
        DeleteAccountCompleteImg.gameObject.SetActive(false);
        //
        SaveDataAlarmImg.gameObject.SetActive(false);
        SaveDataBufferingImg.gameObject.SetActive(false);
        SaveDataCompleteImg.gameObject.SetActive(false);
        //
        LoadDataAlarmImg.gameObject.SetActive(false);
        LoadDataBufferingImg.gameObject.SetActive(false);
        LoadDataCompleteImg.gameObject.SetActive(false);
    }

    // 크레딧버튼 클릭
    private void ClickCreditButton()
    {
        StartCoroutine(SettingEndingScript.PopUpEndinganel());
    }

    private void ClickRankButton()
    {
        PlayerInfo.Instance.isRankButtonOn = true;

        SettingRanking.ChangeDay();
        SettingRanking.SetAcademyScore();
        SettingRanking.SetQuarterlyReport();
    }

    // 구글 계정에 연동하기 -> 제이슨 함수 가져오기?
    public void GoogleAccountLink()
    {
        Debug.Log("구글 계정 연동하기 버튼 클릭");

        gpgsBinder.TryGoogleLogin();
    }

    // 서버 저장하기 
    public void SaveDataOnServer()
    {
        Debug.Log("서버 저장하기 버튼 클릭");
        SaveDataAlarmImg.gameObject.SetActive(true);
    }

    public void ClickSaveDataYesNoButton()
    {
        tempIClicked = EventSystem.current.currentSelectedGameObject;

        {
            if (tempIClicked.name == "SaveData_Button_YES")
            {
                Debug.Log("서버 저장하기 Yes 클릭");

                // 계정이 로그인 되어있고, 인터넷에 연결할 준비가 되어있다면
                if (firebaseBinder.UserId != null &&
                    Application.internetReachability != NetworkReachability.NotReachable)
                {
                    Debug.Log("서버 저장하기 Yes 클릭 - 로그인 되어있음");
                    SaveDataAlarmImg.gameObject.SetActive(false);
                    SaveDataBufferingImg.gameObject.SetActive(true);

                    // 서버에 저장하기
                    firebaseBinder.SaveDataInFirestore();

                    StartCoroutine(AlarmScreenPopOff(4, SaveDataBufferingImg, SaveDataCompleteImg));
                }
                else // 로그인 안되어있는 상태
                {
                    SaveDataAlarmImg.gameObject.SetActive(false);
                    IsAccountExistAlarmImg.gameObject.SetActive(true);

                    StartCoroutine(AlarmScreenPopOff(2, IsAccountExistAlarmImg));
                }
            }
            else if (tempIClicked.name == "SaveData_Button_NO")
            {
                Debug.Log("서버 저장하기 No 클릭");
                SaveDataAlarmImg.gameObject.SetActive(false);
            }
        }
    }

    // 계정 탈퇴하기
    public void GoogleAccountWithDraw()
    {
        Debug.Log("계정 탈퇴하기 버튼 클릭");
        WithdrawAccountAlarmImg.gameObject.SetActive(true);
    }

    public void ClickGoogleAccountWithDrawYesNoButton()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;

        if (nowClick.name == "DeleteAccount_Button_YES")
        {
            // 계정이 로그인 되어있고, 인터넷에 연결할 준비가 되어있다면
            if (firebaseBinder.UserId != null && Application.internetReachability != NetworkReachability.NotReachable)
            {
                Debug.Log("계정삭제 Yes 버튼 클릭");
                WithdrawAccountAlarmImg.gameObject.SetActive(false);
                WithdrawAccountBufferingImg.gameObject.SetActive(true);

                // 파이어스토어의 데이터를 삭제하고 gpgs에서 로그아웃 함
                firebaseBinder.DeleteDataInFirestore();
                gpgsBinder.TryLogout();

                StartCoroutine(AlarmScreenPopOff(4, WithdrawAccountBufferingImg, DeleteAccountCompleteImg));
            }
            else // 로그인 안되어있는 상태
            {
                WithdrawAccountAlarmImg.gameObject.SetActive(false);
                IsAccountExistAlarmImg.gameObject.SetActive(true);

                StartCoroutine(AlarmScreenPopOff(2, IsAccountExistAlarmImg));
            }
        }
        else if (nowClick.name == "DeleteAccount_Button_NO")
        {
            Debug.Log("계정삭제 No 버튼 클릭");
            WithdrawAccountAlarmImg.gameObject.SetActive(false);
        }
    }

    // 서버 불러오기
    public void LoadDataOnServer()
    {
        Debug.Log("서버 불러오기 버튼 클릭");
        LoadDataAlarmImg.gameObject.SetActive(true);
    }

    // 서버 불러오기 버튼 YES 후 데이터 다시 로드 시간 후 안내 창 한번 뜬 후 타이틀 씬으로 이동
    public void ClickLoadDataYesNoButton()
    {
        tempIClicked = EventSystem.current.currentSelectedGameObject;

        if (tempIClicked.name == "LoadData_Button_YES")
        {
            // 계정이 로그인 되어있고, 인터넷에 연결할 준비가 되어있다면
            if (firebaseBinder.UserId != null && Application.internetReachability != NetworkReachability.NotReachable)
            {
                Debug.Log("서버 불러오기 Yes 버튼 클릭");

                MoveSceneManager.m_Instance.UseLoadingDataBtn(); // 저장된 데이터 불러오기

                // 불러오기. TODO :: 불러온 이후 타이틀로 갈 수 있도록 동작해야 함
                firebaseBinder.LoadDataForFirebase();

                LoadDataAlarmImg.gameObject.SetActive(false);
                LoadDataBufferingImg.gameObject.SetActive(true);
                StartCoroutine(AlarmScreenPopOff(4, LoadDataBufferingImg, LoadDataCompleteImg));
            }
            else // 로그인 안되어있는 상태
            {
                LoadDataAlarmImg.gameObject.SetActive(false);
                IsAccountExistAlarmImg.gameObject.SetActive(true);

                StartCoroutine(AlarmScreenPopOff(2, IsAccountExistAlarmImg));
            }
        }
        else if (tempIClicked.name == "LoadData_Button_NO")
        {
            Debug.Log("서러 불러오기 No 버튼 클릭");
            LoadDataAlarmImg.gameObject.SetActive(false);
        }
    }

    // 좌측 버튼들 눌렀을 때 각각의 맞는 창이 뜨게 할 함수
    public void IfClickSettingPanelIndex()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;

        if (nowClick.name == "AccountSettingButton")
        {
            AccountSetting.gameObject.SetActive(true);
            SoundSetting.gameObject.SetActive(false);
            GameSetting.gameObject.SetActive(false);
        }
        else if (nowClick.name == "SoundSettingButton")
        {
            AccountSetting.gameObject.SetActive(false);
            SoundSetting.gameObject.SetActive(true);
            GameSetting.gameObject.SetActive(false);
        }
        else if (nowClick.name == "GameSettingButton")
        {
            AccountSetting.gameObject.SetActive(false);
            SoundSetting.gameObject.SetActive(false);
            GameSetting.gameObject.SetActive(true);
        }
    }


    // 볼륨 기본 설정 함수
    // 푸시알람 기본설정 함수
    public void IfClickPushAlarmSelectButton()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;

        if (nowClick.name == "OFF_button")
        {
            // Off 버튼 색변경
            ColorBlock temp = PushAlarmOffButton.colors;
            temp.normalColor = new Color(0.8584906f, 0.5312154f, 0.5312154f);
            PushAlarmOffButton.colors = temp;
            // On 버튼 색변경
            temp = PushAlarmOnButton.colors;
            temp.normalColor = new Color(1f, 1f, 1f);
            PushAlarmOnButton.colors = temp;

            PlayerInfo.Instance.isPushAlarmOn = false;

            string tempNow = System.DateTime.Now.ToString("yyyy년-MM월-dd일 시간 hh:mm:ss");

            PushAlarmText.text = "알림 수신 거부가 정상 처리되었습니다.\n" + tempNow;

            PushAlarmImg.gameObject.SetActive(true);
            StartCoroutine(AlarmScreenPopOff(2, PushAlarmImg));
        }
        else if (nowClick.name == "ON_button")
        {
            // On 버튼 색변경
            ColorBlock temp = PushAlarmOnButton.colors;
            temp.normalColor = new Color(0.8584906f, 0.5312154f, 0.5312154f);
            PushAlarmOnButton.colors = temp;
            // Off 버튼 색변경
            temp = PushAlarmOffButton.colors;
            temp.normalColor = new Color(1f, 1f, 1f);
            PushAlarmOffButton.colors = temp;

            PlayerInfo.Instance.isPushAlarmOn = true;
            //notificationManger.SendNotification("G.I Academy", "알람이 가나요~", 5); // 일회용인것인가

            PushAlarmImg.gameObject.SetActive(false);
        }
    }

    // 
    IEnumerator AlarmScreenPopOff(int delayTime, Image panel, Image NextPanel = null)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        panel.gameObject.SetActive(false);

        if (NextPanel != null)
        {
            NextPanel.gameObject.SetActive(true);

            StartCoroutine(DataSaveLoadAlarmPopOff(2, NextPanel));
        }
    }

    IEnumerator DataSaveLoadAlarmPopOff(int delayTime, Image panel)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        panel.gameObject.SetActive(false);

        if (tempIClicked.name == "SaveData_Button_YES") // 데이터 저장 버튼을 눌렀는지, 데이터 불러오기 버튼을 눌렀는지 이름으로 비교해서 실행해주기
        {
        }
        else if (tempIClicked.name == "LoadData_Button_YES")
        {
            MoveSceneManager.m_Instance.MoveToTitleScene();
        }
    }

    // 세팅 패널 여는 함수
    public void OpenSettingPanel()
    {
    }

    public void IfClickSettingPanelOffButton()
    {
    }

    // 설정창에서 슬라이드 조정 시 Audio Mixer 에서 BGM 이 조절되게 하기 위한 함수
    public void BGMAudioControl()
    {
        float sound = BGMSoundSlider.value;

        if (sound == -40f)
        {
            AudioMixController.SetFloat("BGM", -80);
        }
        else
        {
            AudioMixController.SetFloat("BGM", sound);
        }
    }

    public void SFXAudioControl()
    {
        float sound = SFXSoundSlider.value;

        if (sound == -40f)
        {
            AudioMixController.SetFloat("SFX", -80);
        }
        else
        {
            AudioMixController.SetFloat("SFX", sound);
        }
    }

    public void IfClickSettingPanelOpenButton()
    {
        InitSettingPanelObjects();

        PopUpSettingPanelPrefab.TurnOnUI();
        SettingPanelPrefab.gameObject.SetActive(true);
    }

    public void IfClickSettingPanelCloseButton()
    {
        PopOffSettingPanelPrefab.TurnOffUI();
    }
}