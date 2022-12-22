using System;
using System.IO;
using UnityEngine;
// 로그가 저장되는 local 위치
// (사용자)\AppData\LocalLow\GIAcademy\GIAcademy\Log


public class Logger
{
    internal static readonly string DefaultLogFileName = "Log";

    // Append = 추가
    internal static void AppendLog(string tag, string message, LogLevel logLevel = LogLevel.Debug)
    {
        // 로그데이터를 파일에 저장한다.
        // 로그 파일의 형식은 다음과 같다.
        // ('yy-MM-dd-HH-mm-ss'), 'tag', 'Log Level', 'Log Text')
        var filePath = GetOrCreateFilePath(tag);
        FileStream fileStream = new FileStream(filePath, FileMode.Append);
        StreamWriter writer = new StreamWriter(fileStream);

        string[] logTexts = message.Split('\n');     // 특정 문자를 기준으로 잘라준다.
        foreach (var text in logTexts)
        {
            string writeText = MakeLogText(tag, text, logLevel);
            
            writer.WriteLine(writeText);
        }

        writer.Flush();
        writer.DisposeAsync();
        fileStream.DisposeAsync();
    }

    private static string MakeLogText(string tag, string text, LogLevel logLevel)
    {
        return $"('{GetCurrentTime()}', '{tag}', '{logLevel}', '{text}')";
    }


    // 로그를 저장할 파일을 생성한다.
    // 파일은 하루에 하나씩 생성한다.
    // 파일 이름의 형식은 Log_LogTag_yy-MM-dd.log
    private static string GetOrCreateFilePath(string tag)
    {
        // 매일 새로운 폴더를 만들자.
        var directoryPath = Path.Combine(Application.persistentDataPath, "Log");
        directoryPath = Path.Combine(directoryPath, GetCurrentDay());
        var fileName = $"{DefaultLogFileName}_{tag}_{GetCurrentDay()}.txt";
        var filePath = Path.Combine(directoryPath, fileName);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (!File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Append);
            StreamWriter writer = new StreamWriter(fileStream);

            writer.Flush();
            writer.DisposeAsync();
            fileStream.DisposeAsync();
        }

        return filePath;
    }

    private static string GetCurrentTime()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmss");
    }

    private static string GetCurrentDay()
    {
        return DateTime.Now.ToString("yyyyMMdd");
    }
}