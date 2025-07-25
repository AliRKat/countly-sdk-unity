﻿using Notifications;
using Plugins.CountlySDK;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountlyEntryPoint : MonoBehaviour, INotificationListener
{
    private Countly countly;

    private void Awake()
    {
        if (Countly.Instance.IsSDKInitialized) {
            return;
        }

        string _appKey = "YOUR_APP_KEY";
        string _serverUrl = "https://your.server.ly/";

        if (_serverUrl.Equals("https://your.server.ly/") || _appKey.Equals("YOUR_APP_KEY")) {
            Debug.LogWarning("Please do not use default set of app key and server url");
        }

        CountlyConfiguration configuration = new CountlyConfiguration(_appKey, _serverUrl)
            .EnableLogging()
            .SetParameterTamperingProtectionSalt("test-salt-checksum")
            .EnableForcedHttpPost()
            .SetRequiresConsent(true)
            .SetEventQueueSizeToSend(1)
            .SetNotificationMode(TestMode.AndroidTestToken);

        string countryCode = "us";
        string city = "Boston’ 墨尔本";
        string latitude = "29.634933";
        string longitude = "-95.220255";
        string ipAddress = "10.2.33.12";

        configuration.SetLocation(countryCode, city, latitude + "," + longitude, ipAddress);
        configuration.GiveConsent(new Consents[] { Consents.Crashes, Consents.Events, Consents.Clicks, Consents.StarRating, Consents.Views, Consents.Users, Consents.Sessions, Consents.Push, Consents.RemoteConfig, Consents.Location, Consents.Feedback });
        configuration.AddNotificationListener(this);

        Countly.Instance.Init(configuration);
        countly = Countly.Instance;
    }

    private void OnApplicationQuit()
    {
        Countly.Instance?.Notifications?.RemoveListener(this);
    }

    public void TestWithMultipleThreads()
    {
        int participants = 13;
        Barrier barrier = new Barrier(participantCount: participants, postPhaseAction: (bar) => {
            Debug.Log("All threads reached the barrier at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        });

        Thread[] threads = new Thread[participants];
        threads[0] = new Thread(delegate () {
            {
                barrier.SignalAndWait();
                Debug.Log("Thread[00] executing at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                _ = Countly.Instance.Events.RecordEventAsync("Event With Segmentation", sum: 10);
                Debug.Log("Thread[00] finished at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        });


        threads[1] = new Thread(delegate () {
            {
                barrier.SignalAndWait();
                Debug.Log("Thread[01] executing at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

                Dictionary<string, object> segment = new Dictionary<string, object> {
                { "Time Spent", "60"},
                { "Retry Attempts", "10"}
                };

                _ = Countly.Instance.Events.RecordEventAsync("Event With Segmentation", segmentation: segment);

                Debug.Log("Thread[01] finished at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        });

        threads[2] = new Thread(delegate () {
            {
                barrier.SignalAndWait();
                Debug.Log("Thread[02] executing at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

                Dictionary<string, object> segment = new Dictionary<string, object> {
                { "Time Spent", "60"},
                { "Retry Attempts", "10"}
                };

                _ = Countly.Instance.Events.RecordEventAsync("Event With Segmentation", segmentation: segment, sum: 5);

                Debug.Log("Thread[02] finished at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        });

        threads[3] = new Thread(delegate () {
            {
                barrier.SignalAndWait();
                Debug.Log("Thread[03] executing at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                _ = Countly.Instance.Views.RecordOpenViewAsync("View A");
                Debug.Log("Thread[03] finished at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        });

        threads[4] = new Thread(delegate () {
            {
                barrier.SignalAndWait();
                Debug.Log("Thread[04] executing at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

                Dictionary<string, object> seg = new Dictionary<string, object>{
                { "Time Spent", "1234455"},
                { "Retry Attempts", "10"}
                };
                _ = Countly.Instance.CrashReports.SendCrashReportAsync("Exception", "Stacktrace", seg);


                Debug.Log("Thread[04] finished at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        });
        threads[5] = new Thread(delegate () {
            {
                barrier.SignalAndWait();
                Debug.Log("Thread[05] executing at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                //  RecordMultiple();
                Debug.Log("Thread[05] finished at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        });

        for (int i = 0; i < participants; i++) {
            threads[i].Start();
        }


        for (int i = 0; i < participants; i++) {
            threads[i].Join();
        }

        Debug.Log("All threads completed at: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    public void SetLocation()
    {
        string countryCode = "uk";
        string city = "런던";
        string latitude = "51.5072";
        string longitude = "0.1276";
        string ipAddress = null;

        countly.Location.SetLocation(countryCode, city, latitude + "," + longitude, ipAddress);
    }

    public void DisableLocation()
    {
        countly.Location.DisableLocation();
    }

    public async void SetRating()
    {
        await countly.StarRating.ReportStarRatingAsync("unity", "0.1", 3);
    }

    public async void RemoteConfigAsync()
    {
        await countly.RemoteConfigs.Update();

        Dictionary<string, object> config = countly.RemoteConfigs.Configs;
        Debug.Log("RemoteConfig: " + config?.ToString());
    }

    public void OnNotificationReceived(string message)
    {
        Debug.Log("[Example] OnNotificationReceived: " + message);
    }

    public void OnNotificationClicked(string message, int index)
    {
        Debug.Log("[Example] OnNotificationClicked: " + message + ", index: " + index);
    }
}
