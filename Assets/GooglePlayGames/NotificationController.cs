using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    [SerializeField] private NotificationBinder _notificationBinder;

    private void Start()
    {
        // _notificationBinder.RequestNotificationPermission();
        // _notificationBinder.RegisterNotificationChannel();
        // _notificationBinder.SendNotification("Hello world!", "TestText", 3);
    }

    private bool set = false;
    private int min = 60;

    public void Update()
    {
        if (PlayerInfo.Instance.isPushAlarmOn == true && set == false)
        {
            _notificationBinder.RequestNotificationPermission();
            _notificationBinder.RegisterNotificationChannel();
            _notificationBinder.SendNotification("G.I Academy", "�л����� ������� ��ٸ��� �ֽ��ϴ�!", 1 * min);


            for (int i = 1; i < 20; i++)
            {
                _notificationBinder.SendNotification("G.I Academy", "�л����� ������� ��ٸ��� �ֽ��ϴ�!", 5 * min * i);
                _notificationBinder.SendNotification("G.I Academy", "���� 1�� ���� ��ī������ ��ǥ�� ���� �ٽ� �޷������?", 3 * min * i);
            }


            set = true;
        }
    }

    public void PushBtn()
    {
        _notificationBinder.RequestNotificationPermission();
        _notificationBinder.RegisterNotificationChannel();
        _notificationBinder.SendNotification("Botton Push", "Push Push", 3);
    }
}