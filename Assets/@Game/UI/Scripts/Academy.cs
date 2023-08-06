using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using Coffee.UIExtensions;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// 2023. 03. 23 
/// 
/// ÄÚµå ¼öÁ¤ ÈÄ Ä¿¹Ô
/// </summary>
public class Academy : MonoBehaviour
{
    [SerializeField] private EventScriptManager tempScriptManagerObj;

    [SerializeField] private Image SetAcademyPanelBG;

    [SerializeField] private Tutorial SettingAcademyTutorial;

    [SerializeField] private PopUpUI PopUpSetAcademyPanel; // ¾ÆÄ«µ¥¹Ì ¼¼ÆÃÀ» À§ÇÑ ÆÐ³Î ÆË¾÷/¿ÀÇÁ ¿¬°á
    [SerializeField] private PopOffUI PopOffSetAcademyPanel;

    [SerializeField] private GameObject SpriteImg1;
    [SerializeField] private GameObject SpriteImg2;
    [SerializeField] private GameObject ArtNameImg;
    [SerializeField] private GameObject DesignNameImg;
    [SerializeField] private GameObject ProgrammingNameImg;
    [SerializeField] private GameObject BookstoreImg;
    [SerializeField] private GameObject ShopImg;
    [SerializeField] private GameObject GenreImg;
    [SerializeField] private GameObject ArtImg;
    [SerializeField] private GameObject DesignImg;
    [SerializeField] private GameObject ProgrammingImg;
    [SerializeField] private GameObject GameImg;

    [Header("Æ©Åä¸®¾óÀ» À§ÇÑ ½Ã³ª¸®¿À ºÎºÐ")]
    [SerializeField]
    private Image PDTalking;

    [SerializeField] private TextMeshProUGUI PDTalkingText;

    [Header("¿øÀå¸í ¼¼ÆÃ ºÎºÐ")] [SerializeField] private Image DirectorNameSetting;
    [SerializeField] private TMP_InputField m_DirectorInputField;
    [SerializeField] private Button m_DirectorNameSetOKButton;

    [Header("¾ÆÄ«µ¥¹Ì¸í ¼¼ÆÃ ºÎºÐ")]
    [SerializeField]
    private Image AcademyNameSetting;

    [SerializeField] private TMP_InputField m_AcademyInputField;
    [SerializeField] private Button m_AcademyNameSetOKButton;

    [SerializeField] private Button PDTalkingNextButton;

    [SerializeField] private Image m_NoticeImg;
    [SerializeField] private TextMeshProUGUI m_NoticeText; // ¾È³»¹®±¸ ¶ç¿öÁÙ º¯¼ö
    [SerializeField] private ClockScheduler m_Scheduler;
    [SerializeField] private Unmask m_Unmask;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private Image m_TutorialTextImage;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private RectTransform m_UnmaskTarget1;
    [SerializeField] private RectTransform m_UnmaskTarget2;
    [SerializeField] private RectTransform m_UnmaskTarget3;

    [SerializeField] private GameObject m_MainCanvas;
    [SerializeField] private Image m_FadeOutImg;

    [SerializeField] private GameObject m_SkipButton;

    private int AcademySettingPanelCount = 0;
    private int AcademySettingScriptCount = 0;

    private string m_AcademyName;
    private string m_DirectorName;
    private int m_Money;
    private int m_AcademyScore;

    LogData m_NowLogInfo; // ÇöÀç ÀÔ·ÂÇÏ´Â µ¥ÀÌÅÍ¸¦ Àá½Ã ÀúÀåÇÒ º¯¼ö
    private int m_Famous;
    private int m_PracticalTalent;
    private int m_Management;
    private int m_Activity;
    private int m_Goods;

    private float m_LastTime;

    private Vector3 m_velocity = Vector3.zero;
    private Vector3 m_cameraOffset = new Vector3(-20, 45, -23);
    private const int m_CameraZoomOutSize = 28;
    private const int m_CameraBasizZoomSize = 18;
    private const float m_ZoomSpeed = 0.3f;
    private const float m_MoveSpeed = 0.2f;


    #region AcademyData

    public string MyAcademy
    {
        get { return m_AcademyName; }
        private set { m_AcademyName = value; }
    }

    public string MyName
    {
        get { return m_DirectorName; }
        private set { m_DirectorName = value; }
    }

    public int MyMoney
    {
        get { return m_Money; }
        private set { m_Money = value; }
    }

    #endregion

    public void OnEnable()
    {
        if (Json.Instance.UseLoadingData == true)
        {
            PDTalkingNextButton.gameObject.SetActive(false);
        }
    }

    // ¹öÆ°¿¡ ¾ÆÄ«µ¥¹Ì¸í, ¿øÀå¸í Ã¼Å© ÈÄ ¾Àº¯°æ ÇÏ´Â ÇÔ¼ö ´Þ¾ÆµÎ±â
    private void Start()
    {
        m_NowLogInfo = new LogData(); // ¸ðµç ·Î±×ÀÎ µ¥ÀÌÅÍ°¡ ÀúÀåµÉ µñ¼Å³Ê¸® ÃÊ±âÈ­?

        m_DirectorNameSetOKButton.onClick.AddListener(IfClickSetOKtButton);
        m_AcademyNameSetOKButton.onClick.AddListener(IfClickSetOKtButton);

        PDTalkingNextButton.onClick.AddListener(IfClickPDTalkingNextButton);

        //PopUpSetAcademyPanel.TurnOnUI();
        m_Famous = 0;
        m_PracticalTalent = 0;
        m_Management = 0;
        m_Activity = 0;
        m_Goods = 0;
        m_AcademyScore = 0;


        if (!Json.Instance.UseLoadingData)
        {
            PlayerInfo.Instance.IsFirstAcademySetting = false;
            // Æ©Åä¸®¾óÀ» À§ÇÑ ¸Ç Ã³À½ ¶ß°Ô µÉ °Í
            InitAcademySetting();
        }
        else
        {
            if (!PlayerInfo.Instance.IsFirstAcademySetting)
            {
                InitAcademySetting();
            }
            this.gameObject.SetActive(false);
            m_SkipButton.SetActive(false);

            // ½ºÅ©¸³Æ®ÀÇ ¾ÆÄ«µ¥¹Ì¸íÀ» ÁöÁ¤ÇÑ ¾ÆÄ«µ¥¹ÌÀÌ¸§À¸·Î ¹Ù²Ù´Â µ¨¸®°ÔÀÌÆ®
            EventScriptManager.dReadyEventScript SetAcademynameByPlayer = new EventScriptManager.dReadyEventScript(tempScriptManagerObj.CheckAcademyNameSet);
            SetAcademynameByPlayer();
        }
    }

    public void InitAcademySetting()
    {
        if (PlayerInfo.Instance.IsFirstAcademySetting == false)
        {
            if (AcademySettingPanelCount == 0)
            {
                m_MainCanvas.SetActive(false);

                //PDTalkingText.text = SettingAcademyTutorial.NameTutorial[AcademySettingScriptCount];
                PDTalkingText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

                SetAcademyPanelBG.gameObject.SetActive(true);

                PDTalking.gameObject.SetActive(true);

                DirectorNameSetting.gameObject.SetActive(false);
                AcademyNameSetting.gameObject.SetActive(false);

                PopUpSetAcademyPanel.TurnOnUI();

                AcademySettingPanelCount++;
                AcademySettingScriptCount++;
            }
        }
    }

    public void SettingPDTalkingText()
    {
        switch (AcademySettingPanelCount)
        {
            case 1: // ´ë»ç ³¡³ª°í ¿øÀå¸í Á¤ÇÏ´Â ºÎºÐ
            {
                PDTalkingText.text = SettingAcademyTutorial.NameTutorial[1];
            }
            break;
            case 2: // ´ë»ç ³¡³ª°í ¿øÀå¸í Á¤ÇÏ´Â ºÎºÐ
            {
                PDTalkingText.text = PlayerInfo.Instance.PrincipalName + " " + SettingAcademyTutorial.NameTutorial[1];
            }
            break;
        }
    }

    public void IfClickPDTalkingNextButton()
    {
        if (AcademySettingPanelCount == 1 || AcademySettingPanelCount == 2 || AcademySettingPanelCount == 5 || AcademySettingPanelCount == 6 || AcademySettingPanelCount == 12 || AcademySettingPanelCount == 16 || AcademySettingPanelCount == 35 || AcademySettingPanelCount == 36)
        {
            PDTalkingText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 14 || AcademySettingPanelCount == 22 || AcademySettingPanelCount == 23 || AcademySettingPanelCount == 24 || AcademySettingPanelCount == 33)
        {
            m_TutorialText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 3)
        {
            PDTalkingNextButton.gameObject.SetActive(false);
            DirectorNameSetting.gameObject.SetActive(true);

            PDTalking.gameObject.SetActive(false);
            AcademyNameSetting.gameObject.SetActive(false);

            AcademySettingPanelCount++;
        }
        else if (AcademySettingPanelCount == 7)
        {
            m_BlackScreen.SetActive(true);
            // todo : ±âÈ¹ ÀÌ¹ÌÁö µîÀå
            DesignImg.SetActive(true);
            PDTalking.gameObject.SetActive(false);

            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialTextImage.transform.position = new Vector3(Screen.width / 2f, Screen.height / 5f, 0);
            m_TutorialText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 8)
        {
            // todo : ¾ÆÆ® ÀÌ¹ÌÁö µîÀå
            ArtImg.SetActive(true);
            m_TutorialText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 9)
        {
            // todo : ÇÃ¹Ö ÀÌ¹ÌÁö µîÀå
            ProgrammingImg.SetActive(true);
            m_TutorialText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 10)
        {
            // todo : ÆÄÆ® ÀÌ¹ÌÁö ¸ðµÎ »ç¶óÁö°í °ÔÀÓ¾ÆÀÌÄÜ µîÀå
            DesignImg.SetActive(false);
            ArtImg.SetActive(false);
            ProgrammingImg.SetActive(false);
            GameImg.SetActive(true);
            m_TutorialText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 11)
        {
            PDTalking.gameObject.SetActive(true);
            GameImg.SetActive(false);

            m_BlackScreen.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(false);
            PDTalkingText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 13)
        {
            PDTalking.gameObject.SetActive(false);

            m_BlackScreen.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];
            SpriteImg1.SetActive(true);

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 15)
        {
            PDTalking.gameObject.SetActive(true);

            m_BlackScreen.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(false);
            int splitIndex1 = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].IndexOf("[");
            int splitIndex2 = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].IndexOf("]");

            PDTalkingText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].Substring(0, splitIndex1) + MyName + " " + SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].Substring(splitIndex2 + 1);
            SpriteImg1.SetActive(false);

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 17)
        {
            int splitIndex1 = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].IndexOf("[");
            int splitIndex2 = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].IndexOf("]");

            PDTalkingText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].Substring(0, splitIndex1) + MyName + " " + SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].Substring(splitIndex2 + 1);

            AcademySettingPanelCount++;
            AcademySettingScriptCount++;
        }
        else if (AcademySettingPanelCount == 18)
        {
            PDTalkingNextButton.gameObject.SetActive(false);
            DirectorNameSetting.gameObject.SetActive(false);

            PDTalking.gameObject.SetActive(false);
            AcademyNameSetting.gameObject.SetActive(true);

            AcademySettingPanelCount++;
        }
        else if (AcademySettingPanelCount == 20)
        {
            PDTalkingNextButton.gameObject.SetActive(true);

            PDTalking.gameObject.SetActive(true);
            PDTalkingText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

            AcademySettingScriptCount++;
            AcademySettingPanelCount++;
        }
        else if (AcademySettingPanelCount == 21)
        {
            PDTalking.gameObject.SetActive(false);
            m_BlackScreen.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];
            m_TutorialTextImage.transform.position = new Vector3(Screen.width / 2f, Screen.height / 5f - 50f, 0);
            SpriteImg2.SetActive(true);

            AcademySettingScriptCount++;
            AcademySettingPanelCount++;
        }
        else if (AcademySettingPanelCount == 25)
        {
            SpriteImg2.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_BlackScreen.SetActive(false);

            AcademySettingPanelCount++;
            StartCoroutine(CameraZoomOut());
        }
        else if (AcademySettingPanelCount == 27)
        {
            m_TutorialTextImage.gameObject.SetActive(false);

            AcademySettingPanelCount++;
            StartCoroutine(MoveCamera(1));
        }
        else if (AcademySettingPanelCount == 29)
        {
            m_TutorialTextImage.gameObject.SetActive(false);

            AcademySettingPanelCount++;
            StartCoroutine(MoveCamera(2));
        }
        else if (AcademySettingPanelCount == 31)
        {
            m_TutorialTextImage.gameObject.SetActive(false);

            AcademySettingPanelCount++;
            StartCoroutine(MoveCamera(3));
        }
        else if (AcademySettingPanelCount == 34)
        {
            m_Unmask.gameObject.SetActive(false);
            ShopImg.SetActive(false);
            BookstoreImg.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_BlackScreen.gameObject.SetActive(false);

            PDTalking.gameObject.SetActive(true);
            PDTalkingText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];

            AcademySettingScriptCount++;
            AcademySettingPanelCount++;
        }
        else if (AcademySettingPanelCount == 37)
        {
            PDTalkingNextButton.gameObject.SetActive(false);
            StartCoroutine(FadeOut());
        }

        ClickEventManager.Instance.Sound.PlayIconTouch();
    }

    IEnumerator CameraZoomOut()
    {
        while (Camera.main.orthographicSize < m_CameraZoomOutSize)
        {
            Camera.main.orthographicSize += m_ZoomSpeed;

            yield return null;
        }

        m_TutorialTextImage.transform.position = new Vector3(Screen.width / 4f * 3, Screen.height / 5f * 4, 0);
        m_TutorialText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];
        m_TutorialTextImage.gameObject.SetActive(true);

        AcademySettingScriptCount++;
        AcademySettingPanelCount++;
    }

    IEnumerator MoveCamera(int targetNum)
    {
        Transform tempTarget = this.transform;
        RectTransform unmaskTarget = null;
        if (targetNum == 1)
        {
            tempTarget = Camera.main.GetComponent<InGameCamera>().TutorialTarget1;
            unmaskTarget = m_UnmaskTarget1;
            m_TutorialTextImage.transform.position = new Vector3((Screen.width / 5f) * 4.15f, Screen.height / 2f, 0);
        }
        else if (targetNum == 2)
        {
            ArtNameImg.SetActive(false);
            DesignNameImg.SetActive(false);
            ProgrammingNameImg.SetActive(false);
            tempTarget = Camera.main.GetComponent<InGameCamera>().TutorialTarget2;
            unmaskTarget = m_UnmaskTarget2;
            m_TutorialTextImage.transform.position = new Vector3((Screen.width / 5f) * 4, Screen.height / 2f, 0);
        }
        else if (targetNum == 3)
        {
            GenreImg.SetActive(false);
            tempTarget = Camera.main.GetComponent<InGameCamera>().TutorialTarget3;
            unmaskTarget = m_UnmaskTarget3;
            m_TutorialTextImage.transform.position = new Vector3((Screen.width / 6f) * 2, Screen.height / 2f, 0);
        }

        m_BlackScreen.SetActive(false);
        m_TutorialTextImage.gameObject.SetActive(false);

        float distance = Vector3.Distance(Camera.main.transform.position, tempTarget.position);

        while (distance > 1)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, tempTarget.position + m_cameraOffset, m_MoveSpeed);
            distance = Vector3.Distance(Camera.main.transform.position, tempTarget.position + m_cameraOffset);
            if (Camera.main.orthographicSize > m_CameraBasizZoomSize)
                Camera.main.orthographicSize -= m_ZoomSpeed * 2;

            yield return null;
        }

        while (Camera.main.orthographicSize > m_CameraBasizZoomSize)
        {
            Camera.main.orthographicSize -= m_ZoomSpeed * 2;
        }

        m_Unmask.gameObject.SetActive(true);
        m_Unmask.fitTarget = unmaskTarget;
        m_BlackScreen.SetActive(true);
        m_TutorialText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];
        m_TutorialTextImage.gameObject.SetActive(true);

        if (targetNum == 1)
        {
            ArtNameImg.SetActive(true);
            DesignNameImg.SetActive(true);
            ProgrammingNameImg.SetActive(true);
        }
        else if (targetNum == 2)
        {
            GenreImg.SetActive(true);
        }
        else if (targetNum == 3)
        {
            BookstoreImg.SetActive(true);
            ShopImg.SetActive(true);
        }

        AcademySettingScriptCount++;
        AcademySettingPanelCount++;
    }

    IEnumerator CameraReset()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, Camera.main.GetComponent<TestCamera>().BaseCameraPosition);

        while (distance > 1)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.GetComponent<TestCamera>().BaseCameraPosition, m_MoveSpeed);
            distance = Vector3.Distance(Camera.main.transform.position, Camera.main.GetComponent<TestCamera>().BaseCameraPosition);

            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        Time.timeScale = 0;
        m_FadeOutImg.gameObject.SetActive(true);
        m_LastTime = Time.realtimeSinceStartup;
        while (m_FadeOutImg.color.a < 1f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_LastTime;
            m_FadeOutImg.color = new Color(m_FadeOutImg.color.r, m_FadeOutImg.color.g, m_FadeOutImg.color.b, m_FadeOutImg.color.a + (deltaTime / 0.5f));
            m_LastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        StartCoroutine(CameraReset());

        InGameUI.Instance.m_nowAcademyName.text = MyAcademy;
        InGameUI.Instance.m_nowDirectorName.text = MyName;

        SetAcademyPanelBG.gameObject.SetActive(false);

        PopOffSetAcademyPanel.TurnOffUI();
        m_MainCanvas.SetActive(true);

        PlayerInfo.Instance.IsFirstAcademySetting = true;
        m_SkipButton.SetActive(false);
        //m_LastTime = Time.realtimeSinceStartup;
        //while (m_FadeOutImg.color.a > 0f)
        //{
        //    float deltaTime = Time.realtimeSinceStartup - m_LastTime;
        //    m_FadeOutImg.color = new Color(m_FadeOutImg.color.r, m_FadeOutImg.color.g, m_FadeOutImg.color.b, m_FadeOutImg.color.a - (deltaTime / 0.5f));
        //    m_LastTime = Time.realtimeSinceStartup;
        //    yield return null;
        //}

        //m_FadeOutImg.gameObject.SetActive(false);
    }

    public void ClickSkipButton()
    {
        MyName = "Å×½ºÆ®";
        MyAcademy = "Å×½ºÆ®";

        PlayerInfo.Instance.PrincipalName = MyName;
        PlayerInfo.Instance.AcademyName = MyAcademy;

        InGameUI.Instance.m_nowAcademyName.text = MyAcademy;
        InGameUI.Instance.m_nowDirectorName.text = MyName;

        SetAcademyPanelBG.gameObject.SetActive(false);

        m_SkipButton.SetActive(false);
        PopOffSetAcademyPanel.TurnOffUI();
        m_MainCanvas.SetActive(true);
        PlayerInfo.Instance.IsFirstAcademySetting = true;
        PlayerInfo.Instance.IsFirstClassSetting = false;
        PlayerInfo.Instance.IsFirstGameJam = false;
        PlayerInfo.Instance.IsFirstGameShow = false;
    }

    public void IfClickSetOKtButton()
    {
        if (AcademySettingPanelCount == 4) // ¿øÀå¸í ÁöÁ¤ ÈÄ
        {
            string Check = Regex.Replace(m_DirectorInputField.text, @"[^a-zA-Z0-9°¡-ÆR]", "", RegexOptions.Singleline);
            Check = Regex.Replace(m_DirectorInputField.text, @"[^\w\.@-]", "", RegexOptions.Singleline);

            if (m_DirectorInputField.text != "" && m_DirectorInputField.text.Equals(Check) == true)
            {
                //int splitIndex = SettingAcademyTutorial.NameTutorial[AcademySettingScriptCount].IndexOf("]");

                MyName = m_DirectorInputField.text;
                PlayerInfo.Instance.PrincipalName = MyName;

                //PDTalkingText.text = MyName + " " + SettingAcademyTutorial.NameTutorial[AcademySettingScriptCount].Substring(splitIndex + 1);
                PDTalkingText.text = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount];
                PDTalking.gameObject.SetActive(true);

                AcademyNameSetting.gameObject.SetActive(false);
                DirectorNameSetting.gameObject.SetActive(false);
                PDTalkingNextButton.gameObject.SetActive(true);

                AcademySettingPanelCount++;
                AcademySettingScriptCount++;
            }
            else
            {
                m_NoticeText.text = "»ç¿ëÇÒ ¼ö ¾ø´Â ¹®ÀÚ°¡ Æ÷ÇÔ µÇ¾î ÀÖ½À´Ï´Ù.\n( ex) Æ¯¼ö¹®ÀÚ, ¶ç¾î¾²±â)";

                // ¹®ÀÚÀÔ·Â¿À·ù °æ°í
                m_NoticeImg.gameObject.SetActive(true);

                StartCoroutine(AlarmScreenPopOff(1));
            }
        }
        else if (AcademySettingPanelCount == 19) // ¾ÆÄ«µ¥¹Ì¸í ÁöÁ¤ ÈÄ
        {
            string Check = Regex.Replace(m_AcademyInputField.text, @"[^a-zA-Z0-9°¡-ÆR]", "", RegexOptions.Singleline);
            Check = Regex.Replace(m_AcademyInputField.text, @"[^\w\.@-]", "", RegexOptions.Singleline);

            if (m_AcademyInputField.text != "" && m_AcademyInputField.text.Equals(Check) == true)
            {
                int splitIndex = SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].IndexOf("]");

                MyAcademy = m_AcademyInputField.text;
                PlayerInfo.Instance.AcademyName = MyAcademy;

                // ¾ÆÄ«µ¥¹Ì ÀÌ¸§À» ÁöÀº ÈÄ, ÇÃ·¹ÀÌ¾î°¡ Á¤ÇÑ ÀÌ¸§À¸·Î ¸ðµç ½ºÅ©¸³Æ®ÀÇ ¾ÆÄ«µ¥¹Ì¸íÀ» ¹Ù²Ù´Â µ¨¸®°ÔÀÌÆ®
                EventScriptManager.dReadyEventScript SetAcademynameByPlayer = new EventScriptManager.dReadyEventScript(tempScriptManagerObj.CheckAcademyNameSet);
                SetAcademynameByPlayer();

                PDTalking.gameObject.SetActive(true);

                PDTalkingText.text = MyAcademy + " " + SettingAcademyTutorial.StartTutorial[AcademySettingScriptCount].Substring(splitIndex + 1);

                AcademyNameSetting.gameObject.SetActive(false);
                DirectorNameSetting.gameObject.SetActive(false);
                PDTalkingNextButton.gameObject.SetActive(true);

                AcademySettingPanelCount++;
                AcademySettingScriptCount++;

                //temp.GetComponent<EventScriptManager>().EventTextScriptSplit();

                // temp.EventTextScriptSplit();
            }
            else
            {
                m_NoticeText.text = "»ç¿ëÇÒ ¼ö ¾ø´Â ¹®ÀÚ°¡ Æ÷ÇÔ µÇ¾î ÀÖ½À´Ï´Ù.\n( ex) Æ¯¼ö¹®ÀÚ, ¶ç¾î¾²±â)";

                // ¹®ÀÚÀÔ·Â¿À·ù °æ°í
                m_NoticeImg.gameObject.SetActive(true);

                StartCoroutine(AlarmScreenPopOff(1));
            }
        }
    }

    IEnumerator AlarmScreenPopOff(int delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        m_NoticeImg.gameObject.SetActive(false);
    }
}