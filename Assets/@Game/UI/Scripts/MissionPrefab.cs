using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionPrefab : MonoBehaviour
{
    [SerializeField] private Image NowMissionPrefab;                // ���� �̼� ������
    [SerializeField] private Image CheckBoxCheckedImg;                // �̼� Ŭ���� ���� üũ �̹���
    [Header("�̼� �̸�")]
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI QuestName;             // �̼� �̸�

    [SerializeField] private Slider QuestSlideBar;                  // �̼� ���൵ �����̵��
    [SerializeField] private TextMeshProUGUI MissionAmountText;     // �̼� ���� �ۼ������� ������ �ؽ�Ʈ

    [SerializeField] private Button ClearQuestButton;              // �̼� Ŭ���� ��ư
    [SerializeField] private Image QuestClearImg;                  // �̼� Ŭ���� �̹���


    // �̼��� ������ �ؽ�Ʈ ������ ��
    [Space(10f)]
    [Header("�̼� ���� �κ� �׸���")]
    [SerializeField] private RectTransform MissionRewardText;                  // �̼� ������ �ؽ�Ʈ ��ü

    [SerializeField] private Image MissionReward1;                             // �̼� ������ 1 
    [SerializeField] private Image MissionReward2;                             // �̼� ������ 2

    [Space(5f)]
    [SerializeField] private Image MissionReward1Img;                          // �̼� ������ 1 �̹���
    [SerializeField] private TextMeshProUGUI MissionReward1AmountText;         // �̼� ������ 1 �ؽ�Ʈ

    [SerializeField] private Image MissionReward2Img;                          // �̼� ������ 2 �̹���
    [SerializeField] private TextMeshProUGUI MissionReward2AmountText;         // �̼� ������ 2 �ؽ�Ʈ

    #region ������Ƽ��
    public Image GetNowMissionPrefab
    {
        get { return NowMissionPrefab; }
        set { NowMissionPrefab = value; }
    }

    public Image GetCheckBoxCheckedImg
    {
        get { return CheckBoxCheckedImg; }
        set { CheckBoxCheckedImg = value; }
    }
    // -------------------------
    public TextMeshProUGUI GetQuestName
    {
        get { return QuestName; }
        set { QuestName = value; }
    }
    // -------------------------
    public Slider GetQuestSlideBar
    {
        get { return QuestSlideBar; }
        set { QuestSlideBar = value; }
    }

    public TextMeshProUGUI GetMissionAmountText
    {
        get { return MissionAmountText; }
        set { MissionAmountText = value; }
    }
    // -------------------------
    public Button GetClearQuestButton
    {
        get { return ClearQuestButton; }
        set { ClearQuestButton = value; }
    }
    public Image GetQuestClearImg
    {
        get { return QuestClearImg; }
        set { QuestClearImg = value; }
    }

    // -------------------------
    public RectTransform GetMissionRewardText
    {
        get { return MissionRewardText; }
        set { MissionRewardText = value; }
    }
    // -------------------------
    public Image GetMissionReward1
    {
        get { return MissionReward1; }
        set { MissionReward1 = value; }
    }
    public Image GetMissionReward2
    {
        get { return MissionReward2; }
        set { MissionReward2 = value; }
    }
    // -------------------------
    public Image GetMissionReward1Img
    {
        get { return MissionReward1Img; }
        set { MissionReward1Img = value; }
    }
    public TextMeshProUGUI GetMissionReward1AmountText
    {
        get { return MissionReward1AmountText; }
        set { MissionReward1AmountText = value; }
    }
    // -------------------------
    public Image GetMissionReward2Img
    {
        get { return MissionReward2Img; }
        set { MissionReward2Img = value; }
    }
    public TextMeshProUGUI GetMissionReward2AmountText
    {
        get { return MissionReward2AmountText; }
        set { MissionReward2AmountText = value; }
    }

    #endregion
}
