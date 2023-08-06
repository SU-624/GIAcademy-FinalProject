using System;
using System.Collections;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;

public class NotificationBinder : MonoBehaviour
{
    // 알림 체널을 등록해야 한다.
    //private const string ChannelId = "channel_id";

    //안드로이드 13부터는 알림을 보내기 위해선 권한이 있어야 한다. 권한이 얻어오자.
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

    // Push Alarm 이 동작하는 핵심 함수. 알림 온오프 -> bool 값(PlayerInfo에다가)으로 조건 걸자.
    // 그러고 이 함수를 가져다가 setting Panel 에서 온 누를 시 함수 호출해서 불러주면 되겠다.

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