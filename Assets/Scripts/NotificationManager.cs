using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private float idleTime = 0f;
    private const float idleThreshold = 300f; // 300 seconds = 5 minutes

    private void Start()
    {
        InitializeNotificationChannel();
    }

    private void Update()
    {
        // Increment idle time if there's no input
        if (Input.anyKey)
        {
            idleTime = 0f; // Reset idle time if there is input
        }
        else
        {
            idleTime += Time.deltaTime;
        }

        // If idle for more than the threshold, schedule notification
        if (idleTime >= idleThreshold)
        {
            ScheduleIdleNotification();
            idleTime = 0f; // Reset idle time to avoid repeated notifications
        }
    }

    private void InitializeNotificationChannel()
    {
        // Create a notification channel for Android
        var channel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Game Notifications",
            Importance = Importance.Default,
            Description = "General notifications for the game",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void ScheduleIdleNotification()
    {
        var notification = new AndroidNotification
        {
            Title = "We miss you!",
            Text = "Come back and play SK Playgrounds now!",
            FireTime = System.DateTime.Now.AddMinutes(5),
            SmallIcon = "tile_0001", // The file name of your icon (without the .png extension)
        };

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
        Debug.Log("Idle notification scheduled.");
    }
}
