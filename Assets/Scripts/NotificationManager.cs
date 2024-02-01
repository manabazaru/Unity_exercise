using System;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationManager : MonoBehaviour
{
    void Start()
    {
        CreateNotificationChannel();
        ScheduleNotifications();
        ScheduleRegularNotifications();
    }

    void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Task Reminders"
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    void ScheduleNotifications()
    {
        foreach (var task in TaskManager.Instance.Tasks)
        {
            int daysBefore = 1; // 期限の何日前に通知するか
            var notificationTime = new System.DateTime(task.Year, task.Month, task.Day, task.Hour, task.Minute, 0).AddDays(-daysBefore);
            var timeUntilNotification = notificationTime - System.DateTime.Now;

            if (timeUntilNotification > System.TimeSpan.Zero)
            {
                var notification = new AndroidNotification()
                {
                    Title = "Task Reminder",
                    Text = $"Your task '{task.Name}' deadline is tomorrow!",
                    FireTime = notificationTime
                };

                AndroidNotificationCenter.SendNotification(notification, "channel_id");
            }
        }
    }
    void ScheduleRegularNotifications()
    {
        string lastTaskDateString = PlayerPrefs.GetString("LastTaskDate", "");
        if (!string.IsNullOrEmpty(lastTaskDateString))
        {
            DateTime lastTaskDate = DateTime.Parse(lastTaskDateString);
            TimeSpan timeSinceLastTask = DateTime.Now - lastTaskDate;

            if (timeSinceLastTask.TotalDays > 7)
            {
                for (int i = 1; i <= 7; i++) // 1週間分の通知をスケジュール
                {
                    var notificationTime = DateTime.Now.AddDays(i);
                    if (notificationTime.Day % 2 == 0) // 偶数日に通知
                    {
                        var notification = new AndroidNotification()
                        {
                            Title = "Task Reminder",
                            Text = "You haven't added any tasks in a while. Check your ToDo list.",
                            FireTime = notificationTime
                        };

                        AndroidNotificationCenter.SendNotification(notification, "channel_id");
                    }
                }
            }
        }
    }

}
