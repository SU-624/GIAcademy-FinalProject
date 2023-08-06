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
/// �ΰ��� ����â (����, ����, ���� ��)
/// </summary>
public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Image SettingPanelPrefab; // ��ü �����г� �� ��ü

    [Header("���� ��ư ����Ʈ")]
    [Space(5f)]
    [SerializeField]
    private Button AccountPageButton; // ���� ���� ��ư

    [SerializeField] private Button SoundPageButton; // ���� ���� ��ư
    [SerializeField] private Button GamePageButton; // ���G ���� ��ư

    [Header("����â ���� ������")]
    [Space(10f)]
    [SerializeField]
    private RectTransform AccountSetting; // ����â ������ �г�

    [SerializeField] private TextMeshProUGUI AccountNumberText; // ����ȸ����ȣ �ؽ�Ʈ
    [Space(5f)] [SerializeField] private Button AccountLinkButton; // ���� �����ϱ� ��ư
    [SerializeField] private Button SaveDataButton; // ���� �����ϱ� ��ư
    [SerializeField] private Button AccountWithDrawButton; // ���� Ż���ϱ� ��ư
    [SerializeField] private Button LoadDataButton; // ���� �ҷ����� ��ư


    [Header("����â ���� ������")]
    [Space(10f)]
    [SerializeField]
    private RectTransform SoundSetting; // ����â ������ �г�

    [SerializeField] private Slider BGMSoundSlider; // BGM ���� �����̴�
    [SerializeField] private Slider SFXSoundSlider; // SFX ���� �����̴�(ȿ����)


    [Header("����â ���� ������")]
    [Space(10f)]
    [SerializeField]
    private RectTransform GameSetting; // ����â ������ �г�

    [SerializeField] private Button PushAlarmOffButton; // Ǫ�þ˶� ���� ��ư
    [SerializeField] private Button PushAlarmOnButton; // Ǫ�þ˶� �ѱ� ��ư
    [SerializeField] private Button TeamGICreditButton; // ������ ũ���� ��ư
    [Space(5f)] [SerializeField] private Image PushAlarmImg; // Ǫ�þ˶� �̹���
    [SerializeField] private TextMeshProUGUI PushAlarmText; // Ǫ�þ˶� �ؽ�Ʈ

    [Header("����â ũ���� Ȯ�� ��ư")]
    [Space(10f)]
    [SerializeField] private Button EndingCreditButton;
    [SerializeField] private SettingEnding SettingEndingScript;

    [Header("��ŷ Ȯ�� ��ư")]
    [Space(10f)]
    [SerializeField] private Button RankingButton;
    [SerializeField] private SettingRank SettingRanking;

    [Header("�����ư, �� �˾� â ��")]
    [Space(10f)]
    [SerializeField]
    [Header("������ �ҷ����� �˶� â")]
    private Button SettingPanelCloseButton; // �����г� ���� ��ư

    [Space(10f)] [SerializeField] private Image IsAccountExistAlarmImg; // Ǫ�þ˶� �̹���

    [Header("����Ż�� �˶� â")]
    [Space(10f)]
    [SerializeField]
    private Image WithdrawAccountAlarmImg; // Ǫ�þ˶� �̹���

    [SerializeField] private Button DeleteAccountYesButton; // �������� ������ ��ư
    [SerializeField] private Button DeleteAccountNoButton; // �������� �� ��ư
    [Space(5f)] [SerializeField] private Image WithdrawAccountBufferingImg; // ���������� �ؽ�Ʈ �˸�â
    [SerializeField] private Image DeleteAccountCompleteImg; // ���������Ϸ� â

    [Header("������ �����ϱ� �˶� â")]
    [Space(10f)]
    [SerializeField]
    private Image SaveDataAlarmImg; // Ǫ�þ˶� �̹���

    [SerializeField] private Button SaveDataYesButton; // ���������� ������ ��ư
    [SerializeField] private Button SaveDataNoButton; // ���������� �� ��ư
    [Space(5f)] [SerializeField] private Image SaveDataBufferingImg; // ������ ������ �ؽ�Ʈ �˸�â
    [SerializeField] private Image SaveDataCompleteImg; // ������ ����Ϸ� â

    [Header("������ �ҷ����� �˶� â")]
    [Space(10f)]
    [SerializeField]
    private Image LoadDataAlarmImg; // Ǫ�þ˶� �̹���

    [SerializeField] private Button LoadDataYesButton; // �����ͺҷ����� ������ ��ư
    [SerializeField] private Button LoadDataNoButton; // �����ͺҷ����� �� ��ư
    [Space(5f)] [SerializeField] private Image LoadDataBufferingImg; // ������ �ҷ������� �ؽ�Ʈ �˸�â
    [SerializeField] private Image LoadDataCompleteImg; // ������ �ҷ�����Ϸ� â

    //-----------------------------------
    [Space(10f)] [SerializeField] private PopUpUI PopUpSettingPanelPrefab; // �����г� PopUp ����
    [SerializeField] private PopOffUI PopOffSettingPanelPrefab; // �����г� PopOff ����

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
    // ����â���� ������ ������ �ֱ� ���� ���� �߰�
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
        // 1��° 3�� 3��° �ְ� �� �Ŀ� �����ϱ� ��ư Ȱ��ȭ
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

    // �����г� ���� ������Ʈ�� �ʱ�ȭ �Լ�
    public void Initialize()
    {
        // ������ ���� �� �ʱ�ȭ ������ �͵� - ���� ����߿� ���� â�� ����
        SettingPanelPrefab.gameObject.SetActive(false);

        AccountSetting.gameObject.SetActive(true);
        SoundSetting.gameObject.SetActive(false);
        GameSetting.gameObject.SetActive(false);

        PushAlarmImg.gameObject.SetActive(false);

        // ���� �������� �� �˶�â�� ���ֱ�
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

        // Ǫ�þ˸� �⺻���´� On���� ����.
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

        SaveDataButton.interactable = false; // ù ���� �� ���� ����...

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

    //���� �г��� �ʱⰪ ����
    public void InitSettingPanelObjects()
    {
        // ������ ���� �� �ʱ�ȭ ������ �͵� - ���� ����߿� ���� â�� ����
        SettingPanelPrefab.gameObject.SetActive(false);

        AccountSetting.gameObject.SetActive(true);
        SoundSetting.gameObject.SetActive(false);
        GameSetting.gameObject.SetActive(false);

        PushAlarmImg.gameObject.SetActive(false);

        // ���� �������� �� �˶�â�� ���ֱ�
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

    // ũ������ư Ŭ��
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

    // ���� ������ �����ϱ� -> ���̽� �Լ� ��������?
    public void GoogleAccountLink()
    {
        Debug.Log("���� ���� �����ϱ� ��ư Ŭ��");

        gpgsBinder.TryGoogleLogin();
    }

    // ���� �����ϱ� 
    public void SaveDataOnServer()
    {
        Debug.Log("���� �����ϱ� ��ư Ŭ��");
        SaveDataAlarmImg.gameObject.SetActive(true);
    }

    public void ClickSaveDataYesNoButton()
    {
        tempIClicked = EventSystem.current.currentSelectedGameObject;

        {
            if (tempIClicked.name == "SaveData_Button_YES")
            {
                Debug.Log("���� �����ϱ� Yes Ŭ��");

                // ������ �α��� �Ǿ��ְ�, ���ͳݿ� ������ �غ� �Ǿ��ִٸ�
                if (firebaseBinder.UserId != null &&
                    Application.internetReachability != NetworkReachability.NotReachable)
                {
                    Debug.Log("���� �����ϱ� Yes Ŭ�� - �α��� �Ǿ�����");
                    SaveDataAlarmImg.gameObject.SetActive(false);
                    SaveDataBufferingImg.gameObject.SetActive(true);

                    // ������ �����ϱ�
                    firebaseBinder.SaveDataInFirestore();

                    StartCoroutine(AlarmScreenPopOff(4, SaveDataBufferingImg, SaveDataCompleteImg));
                }
                else // �α��� �ȵǾ��ִ� ����
                {
                    SaveDataAlarmImg.gameObject.SetActive(false);
                    IsAccountExistAlarmImg.gameObject.SetActive(true);

                    StartCoroutine(AlarmScreenPopOff(2, IsAccountExistAlarmImg));
                }
            }
            else if (tempIClicked.name == "SaveData_Button_NO")
            {
                Debug.Log("���� �����ϱ� No Ŭ��");
                SaveDataAlarmImg.gameObject.SetActive(false);
            }
        }
    }

    // ���� Ż���ϱ�
    public void GoogleAccountWithDraw()
    {
        Debug.Log("���� Ż���ϱ� ��ư Ŭ��");
        WithdrawAccountAlarmImg.gameObject.SetActive(true);
    }

    public void ClickGoogleAccountWithDrawYesNoButton()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;

        if (nowClick.name == "DeleteAccount_Button_YES")
        {
            // ������ �α��� �Ǿ��ְ�, ���ͳݿ� ������ �غ� �Ǿ��ִٸ�
            if (firebaseBinder.UserId != null && Application.internetReachability != NetworkReachability.NotReachable)
            {
                Debug.Log("�������� Yes ��ư Ŭ��");
                WithdrawAccountAlarmImg.gameObject.SetActive(false);
                WithdrawAccountBufferingImg.gameObject.SetActive(true);

                // ���̾����� �����͸� �����ϰ� gpgs���� �α׾ƿ� ��
                firebaseBinder.DeleteDataInFirestore();
                gpgsBinder.TryLogout();

                StartCoroutine(AlarmScreenPopOff(4, WithdrawAccountBufferingImg, DeleteAccountCompleteImg));
            }
            else // �α��� �ȵǾ��ִ� ����
            {
                WithdrawAccountAlarmImg.gameObject.SetActive(false);
                IsAccountExistAlarmImg.gameObject.SetActive(true);

                StartCoroutine(AlarmScreenPopOff(2, IsAccountExistAlarmImg));
            }
        }
        else if (nowClick.name == "DeleteAccount_Button_NO")
        {
            Debug.Log("�������� No ��ư Ŭ��");
            WithdrawAccountAlarmImg.gameObject.SetActive(false);
        }
    }

    // ���� �ҷ�����
    public void LoadDataOnServer()
    {
        Debug.Log("���� �ҷ����� ��ư Ŭ��");
        LoadDataAlarmImg.gameObject.SetActive(true);
    }

    // ���� �ҷ����� ��ư YES �� ������ �ٽ� �ε� �ð� �� �ȳ� â �ѹ� �� �� Ÿ��Ʋ ������ �̵�
    public void ClickLoadDataYesNoButton()
    {
        tempIClicked = EventSystem.current.currentSelectedGameObject;

        if (tempIClicked.name == "LoadData_Button_YES")
        {
            // ������ �α��� �Ǿ��ְ�, ���ͳݿ� ������ �غ� �Ǿ��ִٸ�
            if (firebaseBinder.UserId != null && Application.internetReachability != NetworkReachability.NotReachable)
            {
                Debug.Log("���� �ҷ����� Yes ��ư Ŭ��");

                MoveSceneManager.m_Instance.UseLoadingDataBtn(); // ����� ������ �ҷ�����

                // �ҷ�����. TODO :: �ҷ��� ���� Ÿ��Ʋ�� �� �� �ֵ��� �����ؾ� ��
                firebaseBinder.LoadDataForFirebase();

                LoadDataAlarmImg.gameObject.SetActive(false);
                LoadDataBufferingImg.gameObject.SetActive(true);
                StartCoroutine(AlarmScreenPopOff(4, LoadDataBufferingImg, LoadDataCompleteImg));
            }
            else // �α��� �ȵǾ��ִ� ����
            {
                LoadDataAlarmImg.gameObject.SetActive(false);
                IsAccountExistAlarmImg.gameObject.SetActive(true);

                StartCoroutine(AlarmScreenPopOff(2, IsAccountExistAlarmImg));
            }
        }
        else if (tempIClicked.name == "LoadData_Button_NO")
        {
            Debug.Log("���� �ҷ����� No ��ư Ŭ��");
            LoadDataAlarmImg.gameObject.SetActive(false);
        }
    }

    // ���� ��ư�� ������ �� ������ �´� â�� �߰� �� �Լ�
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


    // ���� �⺻ ���� �Լ�
    // Ǫ�þ˶� �⺻���� �Լ�
    public void IfClickPushAlarmSelectButton()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;

        if (nowClick.name == "OFF_button")
        {
            // Off ��ư ������
            ColorBlock temp = PushAlarmOffButton.colors;
            temp.normalColor = new Color(0.8584906f, 0.5312154f, 0.5312154f);
            PushAlarmOffButton.colors = temp;
            // On ��ư ������
            temp = PushAlarmOnButton.colors;
            temp.normalColor = new Color(1f, 1f, 1f);
            PushAlarmOnButton.colors = temp;

            PlayerInfo.Instance.isPushAlarmOn = false;

            string tempNow = System.DateTime.Now.ToString("yyyy��-MM��-dd�� �ð� hh:mm:ss");

            PushAlarmText.text = "�˸� ���� �źΰ� ���� ó���Ǿ����ϴ�.\n" + tempNow;

            PushAlarmImg.gameObject.SetActive(true);
            StartCoroutine(AlarmScreenPopOff(2, PushAlarmImg));
        }
        else if (nowClick.name == "ON_button")
        {
            // On ��ư ������
            ColorBlock temp = PushAlarmOnButton.colors;
            temp.normalColor = new Color(0.8584906f, 0.5312154f, 0.5312154f);
            PushAlarmOnButton.colors = temp;
            // Off ��ư ������
            temp = PushAlarmOffButton.colors;
            temp.normalColor = new Color(1f, 1f, 1f);
            PushAlarmOffButton.colors = temp;

            PlayerInfo.Instance.isPushAlarmOn = true;
            //notificationManger.SendNotification("G.I Academy", "�˶��� ������~", 5); // ��ȸ���ΰ��ΰ�

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

        if (tempIClicked.name == "SaveData_Button_YES") // ������ ���� ��ư�� ��������, ������ �ҷ����� ��ư�� �������� �̸����� ���ؼ� �������ֱ�
        {
        }
        else if (tempIClicked.name == "LoadData_Button_YES")
        {
            MoveSceneManager.m_Instance.MoveToTitleScene();
        }
    }

    // ���� �г� ���� �Լ�
    public void OpenSettingPanel()
    {
    }

    public void IfClickSettingPanelOffButton()
    {
    }

    // ����â���� �����̵� ���� �� Audio Mixer ���� BGM �� �����ǰ� �ϱ� ���� �Լ�
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