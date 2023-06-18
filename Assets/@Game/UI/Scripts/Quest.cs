using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 2023. 06. 12 Mang
/// 
/// ��ȹ������ ������ ���⼺�� �����ϴ� �� ������ ����Ʈ�� ����.
/// 
/// </summary>
public class Quest : MonoBehaviour
{
    [Header("�˾� ����Ʈ �г�")]
    // [SerializeField] private PopUpUI PopUpQuestPanel;
    // [SerializeField] private PopOffUI PopOffQuestPanel;

    [SerializeField] private Image QuestPanel;         // ����Ʈ �г�

    [Space(5f)]
    [SerializeField] private Image QuestStepImg1;           // ����Ʈ �г� 1
    [SerializeField] private Image QuestStepImg2;           // ����Ʈ �г� 2
    [SerializeField] private Image QuestStepImg3;           // ����Ʈ �г� 3
    [SerializeField] private Image QuestStepImg4;           // ����Ʈ �г� 4
    [SerializeField] private Image QuestStepImg5;           // ����Ʈ �г� 5
    [Space(5f)]
    [SerializeField] private Image Quest1;                  // ����Ʈ 1
    [SerializeField] private Image Quest2;                  // ����Ʈ 2
    [SerializeField] private Image Quest3;                  // ����Ʈ 3
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI QuestName1;    // ����Ʈ 1 �̸�
    [SerializeField] private TextMeshProUGUI QuestName2;    // ����Ʈ 2 �̸�
    [SerializeField] private TextMeshProUGUI QuestName3;    // ����Ʈ 3 �̸�
    [Space(5f)]
    [SerializeField] private Image CheckedImgQuest1;        // ����Ʈ 1 üũ�� �̹���
    [SerializeField] private Image CheckedImgQuest2;        // ����Ʈ 2 üũ�� �̹���
    [SerializeField] private Image CheckedImgQuest3;        // ����Ʈ 3 üũ�� �̹���
    [Space(5f)]
    [SerializeField] private Slider QuestSlideBar1;         // ����Ʈ 1 ���൵ �����̵��
    [SerializeField] private Slider QuestSlideBar2;         // ����Ʈ 2 ���൵ �����̵��
    [SerializeField] private Slider QuestSlideBar3;         // ����Ʈ 3 ���൵ �����̵��

    // ����Ʈ�� ���൵�� üũ�ϰ�, ���� ���� �� �� ������
    private int QuestStepNum = 1;           //  1~ 5�ܰ� ������ ����Ʈ ����
    private int NowStepQuestClear = 0;      // ���� �ܰ��� ����Ʈ�� �� �� ������ üũ
    private int TargetValueQuestNnm = 0;    // ���� �ܰ迡�� ���� �� ����Ʈ�� ����

    // ����Ʈ ���� ��ư�� InGameUI ���� �ҷ��ͼ� ����

    private bool isQuestPanelOpen = false;

    public void Start()
    {
        InGameUI.Instance.GetQuestOpenButton.onClick.AddListener(IfClickQuestPanelButton);
    }

    // �ΰ����г��� ����Ʈ ��ư�� Ŭ�� �� ���� ����Ʈ ����� ����Ʈ�� ���� �� �Լ�
    public void IfClickQuestPanelButton()
    {
        if (QuestPanel.gameObject.activeSelf)
        {
            QuestPanel.gameObject.SetActive(false);
        }
        else
        {
            QuestPanel.gameObject.SetActive(true);
        }
    }

    public void SetOriginalQuestData()
    {
    }
}
