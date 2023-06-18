using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


/// <summary>
/// Mang 2022.10.12
/// 
/// ���� UI ��ũ��Ʈ���� ���� �� UI �Ŵ���
///
/// woodpie9 2023.06.13
/// MonoBehaviour�� ���� AllInOneData�� �Լ��� �����ϱ� ���� UI�� ����ϴ� �Լ�
/// </summary>
public class UIManager : MonoBehaviour
{
    public EventScheduleSystem eventScheduleSystem;
    public MailManagement mailManagement;

    public void CollectDataBtn()
    {
        AllInOneData.Instance.CollectAllGameData();
        eventScheduleSystem.CollectSuddenEvent();
        mailManagement.CollectMailList();
    }
    
    public void LocalSaveBtn()
    {
        CollectDataBtn();
        Json.Instance.SaveDataInLocal();
    }

    public void LocalLoadBtn()
    {
        
    }

    public void GoTitleBtn()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
