using System;
using System.Collections;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;

public class NotificationBinder : MonoBehaviour
{
    // �˸� ü���� ����ؾ� �Ѵ�.
    //private const string ChannelId = "channel_id";

    //�ȵ���̵� 13���ʹ� �˸��� ������ ���ؼ� ������ �־�� �Ѵ�. ������ ������.
    public void RequestNotificationPermission()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    // IEnumerator RequestNotificationPermission2()
    // {
    //     var request = new PermissionRequest();
    //     while (request.Status == PermissionStatus.RequestPending)
    //         yield return null;
    //     // here use request.Status to determine users response
    // }


    public void RegisterNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    // Push Alarm �� �����ϴ� �ٽ� �Լ�. �˸� �¿��� -> bool ��(PlayerInfo���ٰ�)���� ���� ����.
    // �׷��� �� �Լ��� �����ٰ� setting Panel ���� �� ���� �� �Լ� ȣ���ؼ� �ҷ��ָ� �ǰڴ�.

    public void SendNotification(string title, string text, int fireTimeInSeconds)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddHours(fireTimeInSeconds);

        notification.SmallIcon = "app_icon_id";
        notification.LargeIcon = "app_large_icon_id";

        notification.IntentData = "{\"title\": \"Notification 1\", \"data\": \"200\"}";

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }
}