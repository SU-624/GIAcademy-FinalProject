using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clicker : MonoBehaviour
{
    private int m_CameraLayerMask = 1 << 7;
    private int m_ClickerLayerMask = 1 << 9;
    private int m_IgnoreLayerMask = 1 << 2;
    public GameObject effect_jackpot1;
    public GameObject effect_jackpot2;
    public GameObject effect_normal1;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & m_ClickerLayerMask))
            {
                if (Time.timeScale == 0)
                    return;

                if (EventSystem.current.IsPointerOverGameObject() == true)
                    return;

                if (hit.transform.tag != "Clicker")
                    return;

                if (hit.transform.name != this.gameObject.name)
                    return;

                int randomPercent = Random.Range(1, 101);
                int incomeMoney = 0;

                if (randomPercent <= 3)
                {
                    incomeMoney = 1000;
                    ClickEventManager.Instance.Sound.PlayMoneyJackpotSound();
                    //터치시 이펙트(프리팹 인스턴스화)
                    GameObject effect1 = Instantiate(effect_jackpot1, transform);
                    Destroy(effect1, 2f);
                    GameObject effect2 = Instantiate(effect_jackpot2, transform);
                    Destroy(effect2, 2f);
                    PlayerInfo.Instance.LuckyBox++;
                }
                else
                {
                    incomeMoney = 10;
                    ClickEventManager.Instance.Sound.PlayMoneySound();
                    //터치시 이펙트(프리팹 인스턴스화)
                    GameObject effect1 = Instantiate(effect_normal1, transform);
                    Destroy(effect1, 2f);
                }
                PlayerInfo.Instance.MyMoney += incomeMoney;
                MonthlyReporter.Instance.m_NowMonth.IncomeActivity += incomeMoney;
                StartCoroutine(ClickEventManager.Instance.ClickerMoneyFadeOutText(this.transform.position, incomeMoney.ToString()));
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject(0))
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~m_CameraLayerMask & ~m_IgnoreLayerMask))
                {
                    if (Time.timeScale == 0)
                        return;

                    if (EventSystem.current.IsPointerOverGameObject(0) == true)
                        return;

                    if (hit.transform.tag != "Clicker")
                        return;

                    if (hit.transform.name != this.gameObject.name)
                        return;

                    int randomPercent = Random.Range(1, 101);
                    int incomeMoney = 0;

                    if (randomPercent <= 3)
                    {
                        incomeMoney = 1000;
                        ClickEventManager.Instance.Sound.PlayMoneyJackpotSound();
                        //터치시 이펙트(프리팹 인스턴스화)
                        GameObject effect1 = Instantiate(effect_jackpot1, transform);
                        Destroy(effect1, 2f);
                        GameObject effect2 = Instantiate(effect_jackpot2, transform);
                        Destroy(effect2, 2f);
                        PlayerInfo.Instance.LuckyBox++;
                    }
                    else
                    {
                        incomeMoney = 10;
                        ClickEventManager.Instance.Sound.PlayMoneySound();
                        //터치시 이펙트(프리팹 인스턴스화)
                        GameObject effect1 = Instantiate(effect_normal1, transform);
                        Destroy(effect1, 2f);
                    }
                    PlayerInfo.Instance.MyMoney += incomeMoney;
                    MonthlyReporter.Instance.m_NowMonth.IncomeActivity += incomeMoney;
                    StartCoroutine(ClickEventManager.Instance.ClickerMoneyFadeOutText(this.transform.position, incomeMoney.ToString()));
                }
            }
        }
#endif
    }
}
