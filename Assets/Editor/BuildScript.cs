using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public class BuildScript : MonoBehaviour
{
    private const string APP_NAME = "GI_Academy";
    private const string KEYSTORE_PASSWORD = "123456";
    private const string BUILD_BASIC_PATH = "Builds/";
    private const string BUILD_ANDROID_PATH = BUILD_BASIC_PATH + "Android/";

    [MenuItem("Build/Build Android")]
    public static void BuildForAndroid()
    {
        PlayerSettings.Android.keystorePass = KEYSTORE_PASSWORD;
        PlayerSettings.Android.keyaliasPass = KEYSTORE_PASSWORD;

        string fileName = APP_NAME + ".apk";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        buildPlayerOptions.locationPathName = BUILD_ANDROID_PATH + fileName;
        buildPlayerOptions.scenes = new[] { "Assets/@Game/Scenes/TitleScene.unity", "Assets/@Game/Scenes/InGameScene.unity", "Assets/@Game/Scenes/UIScene.unity" };
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded : " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}