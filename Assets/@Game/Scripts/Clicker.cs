using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clicker : MonoBehaviour
{
    private int m_CameraLayerMask = 1 << 7;
    private int m_UiLayerMask = 1 << 5;
    private int m_IgnoreLayerMask = 1 << 2;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & ~m_IgnoreLayerMask))
            {
                if (hit.transform.tag != "Clicker")
                    return;

                int randomPercent = Random.Range(1, 101);
                int incomeMoney = 0;

                if (randomPercent <= 3)
                {
                    incomeMoney = 1000;
                    ClickEventManager.Instance.Sound.PlayMoneyJackpotSound();
                }
                else
                {
                    incomeMoney = 10;
                    ClickEventManager.Instance.Sound.PlayMoneySound();
                }
                PlayerInfo.Instance.m_MyMoney += incomeMoney;
                MonthlyReporter.Instance.m_NowMonth.IncomeActivity += incomeMoney;
                StartCoroutine(ClickEventManager.Instance.ClickerMoneyFadeOutText(this.transform.position, incomeMoney.ToString()));
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
        {
            Touch touch = Input.GetTouch(0); 
            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & ~m_IgnoreLayerMask))
                {
                    if (hit.transform.tag != "Clicker")
                        return;

                    int randomPercent = Random.Range(1, 101);
                    int incomeMoney = 0;

                    if (randomPercent <= 3)
                    {
                        incomeMoney = 1000;
                        ClickEventManager.Instance.Sound.PlayMoneyJackpotSound();
                    }
                    else
                    {
                        incomeMoney = 10;
                        ClickEventManager.Instance.Sound.PlayMoneySound();
                    }
                    PlayerInfo.Instance.m_MyMoney += incomeMoney;
                    MonthlyReporter.Instance.m_NowMonth.IncomeActivity += incomeMoney;
                    StartCoroutine(ClickEventManager.Instance.ClickerMoneyFadeOutText(this.transform.position, incomeMoney.ToString()));
                }
            }
        }
#endif
    }
}
