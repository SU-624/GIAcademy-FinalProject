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
    [SerializeField] private Image SetAcademyPanelBG;

    [SerializeField] private Tutorial SettingAcademyTutorial;
    [SerializeField] private PopUpUI _popUpClassPanel;

    [SerializeField] private PopUpUI PopUpSetAcademyPanel; // ¾ÆÄ«µ¥¹Ì ¼¼ÆÃÀ» À§ÇÑ ÆÐ³Î ÆË¾÷/¿ÀÇÁ ¿¬°á
    [SerializeField] private PopOffUI PopOffSetAcademyPanel;

    [Header("Æ©Åä¸®¾óÀ» À§ÇÑ ½Ã³ª¸®¿À ºÎºÐ")] [SerializeField]
    private Image PDTalking;

    [SerializeField] private TextMeshProUGUI PDTalkingText;

    [Header("¿øÀå¸í ¼¼ÆÃ ºÎºÐ")] [SerializeField] private Image DirectorNameSetting;
    [SerializeField] private TMP_InputField m_DirectorInputField;
    [SerializeField] private Button m_DirectorNameSetOKButton;

    [Header("¾ÆÄ«µ¥¹Ì¸í ¼¼ÆÃ ºÎºÐ")] [SerializeField]
    private Image AcademyNameSetting;

    [SerializeField] private TMP_InputField m_AcademyInputField;
    [SerializeField] private Button m_AcademyNameSetOKButton;

    [SerializeField] private Button PDTalkingNextButton;

    [SerializeField] private Image m_NoticeImg;
    [SerializeField] private TextMeshProUGUI m_NoticeText; // ¾È³»¹®±¸ ¶ç¿öÁÙ º¯¼ö
    [SerializeField] private ClockScheduler m_Scheduler;
    private int AcademySettingPanelCount = 0;
    private int AcademySettingSciptCount = 0;

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

    #region AcademyData

    public string MyAcademy
    {
        get { return m_AcademyName; }
        private set { m_AcademyName = value; }
    }

    public string MyTeacher
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


        if (!Json.Instance.IsSavedDataExists)
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
        }
    }

    public void InitAcademySetting()
    {
        if (PlayerInfo.Instance.IsFirstAcademySetting == false)
        {
            if (AcademySettingPanelCount == 0)
            {
                PDTalkingText.text = SettingAcademyTutorial.NameTutorial[AcademySettingPanelCount];

                SetAcademyPanelBG.gameObject.SetActive(true);

                PDTalking.gameObject.SetActive(true);

                DirectorNameSetting.gameObject.SetActive(false);
                AcademyNameSetting.gameObject.SetActive(false);

                PopUpSetAcademyPanel.TurnOnUI();

                AcademySettingPanelCount++;
                AcademySettingSciptCount++;
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
                PDTalkingText.text = PlayerInfo.Instance.m_TeacherName + " " + SettingAcademyTutorial.NameTutorial[1];
            }
                break;
        }
    }

    public void IfClickPDTalkingNextButton()
    {
        if (AcademySettingPanelCount == 1 || AcademySettingPanelCount == 4 || AcademySettingPanelCount == 5 ||
            AcademySettingPanelCount == 6 || AcademySettingPanelCount == 10)
        {
            PDTalkingText.text = SettingAcademyTutorial.NameTutorial[AcademySettingSciptCount];

            AcademySettingPanelCount++;
            AcademySettingSciptCount++;
        }
        else if (AcademySettingPanelCount == 2) // ¿øÀå ÀÌ¸§Á¤ÇÏ±â Ã¢ ÆË¾÷
        {
            DirectorNameSetting.gameObject.SetActive(true);

            PDTalking.gameObject.SetActive(false);
            AcademyNameSetting.gameObject.SetActive(false);


            AcademySettingPanelCount++;
        }
        else if (AcademySettingPanelCount == 7) // ¾ÆÄ«µ¥¹Ì ÀÌ¸§ Á¤ÇÏ±â Ã¢ ÆË¾÷
        {
            AcademyNameSetting.gameObject.SetActive(true);

            PDTalking.gameObject.SetActive(false);
            DirectorNameSetting.gameObject.SetActive(false);

            AcademySettingPanelCount++;
        }
        else if (AcademySettingPanelCount == 9)
        {
            int splitIndex1 = SettingAcademyTutorial.NameTutorial[AcademySettingSciptCount].IndexOf("[");
            int splitIndex2 = SettingAcademyTutorial.NameTutorial[AcademySettingSciptCount].IndexOf("]");

            MyAcademy = m_AcademyInputField.text;

            PDTalkingText.text =
                SettingAcademyTutorial.NameTutorial[AcademySettingSciptCount].Substring(0, splitIndex1) + MyAcademy +
                SettingAcademyTutorial.NameTutorial[AcademySettingSciptCount].Substring(splitIndex2 + 1);

            AcademySettingPanelCount++;
            AcademySettingSciptCount++;
        }
        else if (AcademySettingPanelCount == 11)
        {
            // IfSetOKSaveJsonFile();           ÀúÀåÀº 1Â÷ ¹èÆ÷ Àü Áö¿ö³õ±â

            InGameUI.Instance.m_nowAcademyName.text = MyAcademy;
            InGameUI.Instance.m_nowDirectorName.text = MyTeacher;

            SetAcademyPanelBG.gameObject.SetActive(false);

            PopOffSetAcademyPanel.TurnOffUI();

            PlayerInfo.Instance.IsFirstAcademySetting = true;
        }
    }

    public void IfClickSetOKtButton()
    {
        if (AcademySettingPanelCount == 3) // ¿øÀå¸í ÁöÁ¤ ÈÄ
        {
            string Check = Regex.Replace(m_DirectorInputField.text, @"[^a-zA-Z0-9°¡-ÆR]", "", RegexOptions.Singleline);
            Check = Regex.Replace(m_DirectorInputField.text, @"[^\w\.@-]", "", RegexOptions.Singleline);

            if (m_DirectorInputField.text != "" && m_DirectorInputField.text.Equals(Check) == true)
            {
                int splitIndex = SettingAcademyTutorial.NameTutorial[AcademySettingSciptCount].IndexOf("]");

                MyTeacher = m_DirectorInputField.text;
                PlayerInfo.Instance.m_TeacherName = MyTeacher;

                PDTalkingText.text = MyTeacher + " " + SettingAcademyTutorial.NameTutorial[AcademySettingSciptCount]
                    .Substring(splitIndex + 1);
                PDTalking.gameObject.SetActive(true);

                AcademyNameSetting.gameObject.SetActive(false);
                DirectorNameSetting.gameObject.SetActive(false);

                AcademySettingPanelCount++;
                AcademySettingSciptCount++;
            }
            else
            {
                m_NoticeText.text = "»ç¿ëÇÒ ¼ö ¾ø´Â ¹®ÀÚ°¡ Æ÷ÇÔ µÇ¾î ÀÖ½À´Ï´Ù.\n( ex) Æ¯¼ö¹®ÀÚ, ¶ç¾î¾²±â)";

                // ¹®ÀÚÀÔ·Â¿À·ù °æ°í
                m_NoticeImg.gameObject.SetActive(true);

                StartCoroutine(AlarmScreenPopOff(1));
            }
        }
        else if (AcademySettingPanelCount == 8) // ¾ÆÄ«µ¥¹Ì¸í ÁöÁ¤ ÈÄ
        {
            string Check = Regex.Replace(m_AcademyInputField.text, @"[^a-zA-Z0-9°¡-ÆR]", "", RegexOptions.Singleline);
            Check = Regex.Replace(m_AcademyInputField.text, @"[^\w\.@-]", "", RegexOptions.Singleline);

            if (m_AcademyInputField.text != "" && m_AcademyInputField.text.Equals(Check) == true)
            {
                MyAcademy = m_AcademyInputField.text;
                PlayerInfo.Instance.m_AcademyName = MyAcademy;

                PDTalking.gameObject.SetActive(true);

                PDTalkingText.text = SettingAcademyTutorial.NameTutorial[AcademySettingSciptCount];

                AcademyNameSetting.gameObject.SetActive(false);
                DirectorNameSetting.gameObject.SetActive(false);

                AcademySettingPanelCount++;
                AcademySettingSciptCount++;
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

    // ¾ÆÀÌµð·Î ·Î±×ÀÎ ÇÒ ¶§ (¹Ì»ç¿ë)
    // ¾ÆÄ«µ¥¹ÌÀÌ¸§ & ¿øÀå ÀÌ¸§ ¼¼ÆÃ ( ÀÎ°ÔÀÓ UI - SetOK ¹öÆ° ´©¸£¸é ½ÇÇà)
    public void SetAcademyData()
    {
        MyAcademy = m_AcademyInputField.text;
        MyTeacher = m_DirectorInputField.text;

        Debug.Log("AcademyName : " + MyAcademy);
        Debug.Log("DirectorName : " + MyTeacher);
        PlayerInfo.Instance.m_AcademyName = MyAcademy;
        PlayerInfo.Instance.m_TeacherName = MyTeacher;
        ///////////////////////

        //AllInOneData.Instance.player.PlayerID = PlayerInfo.Instance.m_PlayerID;
        AllInOneData.Instance.PlayerData.AcademyName = MyAcademy;
        AllInOneData.Instance.PlayerData.PrincipalName = MyTeacher;
        AllInOneData.Instance.PlayerData.AcademyScore = m_AcademyScore;
        AllInOneData.Instance.PlayerData.Famous = m_Famous;
        AllInOneData.Instance.PlayerData.Management = m_Management;
        AllInOneData.Instance.PlayerData.TalentDevelopment = m_PracticalTalent;
        AllInOneData.Instance.PlayerData.Activity = m_Activity;
        AllInOneData.Instance.PlayerData.Goods = m_Goods;

        // IfSetOKSaveJsonFile();

        InGameUI.Instance.m_nowAcademyName.text = MyAcademy;
        InGameUI.Instance.m_nowDirectorName.text = MyTeacher;
    }

    // ÀÔ·ÂÇÑ µ¥ÀÌÅÍ¸¦ ÀúÀåÇÏ±âÀ§ÇÑ ÇÔ¼ö
    public void IfSetOKSaveJsonFile()
    {
        // 5.15 woodpie9 PlayerID Á¤º¸ »èÁ¦
        // ÀÓ½ÃÁ¤º¸ ºÎ¿©
        // m_NowLogInfo.MyID = "@GuestLogin";
        // Debug.Log("Guest : " + m_NowLogInfo.MyID);
        //
        // PlayerInfo.Instance.m_PlayerID = m_NowLogInfo.MyID;

#if UNITY_EDITOR
        // var json = GameObject.Find("Json");             // ¾ÀÀÌ ´Ù¸£±â ¶§¹®¿¡ Json À» ¾²·Á¸é ÀÌ·¸°Ô ÇØÁà¾ß ÇÑ´Ù 
        // json.GetComponent<Json>().SaveDataInSetAcademyPanel();
        //Json.Instance.SaveDataInSetAcademyPanel();
#elif UNITY_ANDROID
        // var json = GameObject.Find("Json");             // ¾ÀÀÌ ´Ù¸£±â ¶§¹®¿¡ Json À» ¾²·Á¸é ÀÌ·¸°Ô ÇØÁà¾ß ÇÑ´Ù 
        // json.GetComponent<Json>().SaveDataInSetAcademyPanel();

        // string str = JsonUtility.ToJson(m_NowLogInfo.MyID);     // µ¥ÀÌÅÍ¸¦ Á¦ÀÌ½¼ Çü½ÄÀ¸·Î º¯È¯

        // Debug.Log("To Json? :" + str);
#endif
    }
}