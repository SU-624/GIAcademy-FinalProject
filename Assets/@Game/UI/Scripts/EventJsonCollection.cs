using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// ����ٰ� �̺�Ʈ ���� ���̽� ���ϵ��� ���� Ŭ������ �����
/// </summary>
/// 
// �̺�Ʈ ����Ʈ
public class SuddenEventTableData
{
    public bool IsSuddenEvent;                                              // ����/���� �̺�Ʈ ����
    public int SuddenEventID;                                            // �̺�Ʈ ID
    public string EventName;                                                // �̺�Ʈ �̸�
    public int[] SuddenEventDate = new int[4];                              // �̺�Ʈ�� ����� ��¥ �迭( ��, ��, ��, ����)

    public int EventDelayTime;                                              // �̺�Ʈ�� �����̽ð�(��)
    public int SuddenEventType;                                             // �̺�Ʈ�� ������ ���� �ε���
    public string Where;                                                    // ��������̺�Ʈ - �İ���� ���?
    public int ConditionType;                                               // �̺�Ʈ�� ���� ����

    public bool SuddenEventRepeat;                                          // �̺�Ʈ�� �ݺ� ����
    public Dictionary<string, int> SuddenEventRepeatGap;                    // �̺�Ʈ�� �ݺ� �ֱ�(��)
    public bool SuddenEventOpen;                                            // �̺�Ʈ�� �ر� ����
    public int SuddenEventDuration;                                         // �̺�Ʈ ����Ⱓ     --> �̺�Ʈ�� ����ǰ� �󸶳� �ɸ�������
    public int SuddenEventTime;                                             // �̺�Ʈ ��ȣ�ۿ�ð�   --> �ܼ��̺�Ʈ�� �ð��� �󸶳� �ɸ��� ������
    public int SuddenEventScript;                                           // �̺�Ʈ ���� �ؽ�Ʈ ���� �ε���

    public int Pay;                                                         // �̺�Ʈ �Ҹ� ��ȭ �ε���
    public int Amount;                                                      // �Ҹ� ��ȭ��

    public int SuddenEventCondition1;                                       // ���� 1 index
    public int Amount1N;                                                    // ���� 1 N ��
    public int Amount1M;                                                    // ���� 1 M ��
    public int SeddenEventCondition2;                                       // ���� 2 index
    public int Amount2N;                                                    // ���� 2 N ��
    public int Amount2M;                                                    // ���� 2 M ��
    public int SeddenEventCondition3;                                       // ���� 3 index
    public int Amount3N;                                                    // ���� 3 N ��
    public int Amount3M;                                                    // ���� 3 M ��

    public int SelectRewardIndex1;                                          // ������ ���� 1
    public int SelectRewardIndex2;                                          // ������ ���� 2
    public int SelectRewardIndex3;                                          // ������ ���� 3
    public int RewardIndex1;                                                // �ܼ� ���� ���� 1
    public int RewardIndex2;                                                // �ܼ� ���� ���� 2
    public int RewardIndex3;                                                // �ܼ� ���� ���� 3

    public int Teacher;     // UnLockTeacherID;                                          // �رݵǴ� ���� ID
    public int Company;        // UnLockCompanyID;                                             // �رݵǴ� ȸ�� ID
}

// �ܼ� ���� ����
public class SimpleExecutionEventReward
{
    public int EventID;                        // �ܼ����ຸ�� ID
    public string RewardTaker;                 // ������ ���� ��ü
    public int Reward;                         // ���� �ε���
    public int Amount;                         // ���� ��
}

// ������ ����
public class OptionChoiceEventReward
{
    public int SelectEventID;                   // ������ ���� ID
    public string SelectScript;                 // ������ ����
    public int SelectPay;                       // �Ҹ���ȭ ���� �ε���
    public int PayAmount;                       // �Ҹ� ��ȭ��

    public string RewardTaker;                  // ������ �޴� ��ü

    public int Reward1;                         // ����1 �ε���
    public int Amount1;                         // ����1 ��
    public int Reward2;                         // ����2 �ε���
    public int Amount2;                         // ����2 ��
}

// �̺�Ʈ �ó����� ��ũ��Ʈ
public class EventScript
{
    public int EventScriptID;                    // �̺�Ʈ ��ũ��Ʈ ID
    public string EventScriptTextStart;             // �̺�Ʈ ���� ��ũ��Ʈ
    public string EventScriptTextMiddle;            // �̺�Ʈ �߰� ��ũ��Ʈ
    public string EventScriptTextSelect1;           // �̺�Ʈ ������ 1
    public string EventScriptTextSelect2;           // ������ 2
    public string EventScriptTextSelect3;           // ������ 3
    public string EventScriptTextFin;               // �̺�Ʈ ��ũ��Ʈ ������
}

// ����� �̺�Ʈ�� ������ ������ Ŭ����
public class UsedEventRepeatData
{
    public int SuddenEventID;        // �̺�Ʈ�� ID
    public int YearData;                // ��� ������ �⵵
    public int MonthData;               // ��� ������ ��
    public int WeekData;                // ��� ������ ��
    public int DayData;                 // ��� ������ ��
}

//public class EventJsonCollection : MonoBehaviour
//{
//    // Json ������ �Ľ��ؼ� �� �����͵��� �� ��� �� ����Ʈ ����
//    // �� �����鵵 EventSchedule �� Instance.���� �鿡 �־��ְ� ������ ����
//    // �����̺�Ʈ
//    public List<SuddenEventTableData> AllSuddenEventList = new List<SuddenEventTableData>();                            // ��ü ����(����) �̺�Ʈ
//    //
//    public List<SimpleExecutionEventReward> SimpleEventReward = new List<SimpleExecutionEventReward>();                 //  ��밡���� �����̺�Ʈ ���
//    public List<OptionChoiceEventReward> CHoiceEventReward = new List<OptionChoiceEventReward>();                       // ���� ���� �̺�Ʈ ���
//    // �����̺�Ʈ
//    // public List<> AllSelectEventList = new List<>();                             // ��ü ���� �̺�Ʈ
//    public List<EventScript> PrevIChoosedEvent = new List<EventScript>();                                               // ���� ���� ������ �̺�Ʈ(�ִ� 2��) ��� �� �ӽ� ����
//    public SuddenEventTableData TempIChoosed;                                                                           // �ӽ÷� ���� ��� ���� ��
    
//}