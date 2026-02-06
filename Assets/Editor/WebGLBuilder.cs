using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// WebGLビルド用のエディタスクリプト
/// コマンドラインからのバッチモードビルドに対応
/// Usage: Unity -batchmode -executeMethod WebGLBuilder.Build -quit
/// </summary>
public class WebGLBuilder
{
    private static readonly string DefaultBuildPath = "Build/WebGL";

    /// <summary>
    /// WebGLビルドを実行（バッチモード対応）
    /// </summary>
    [MenuItem("Build/WebGL Build")]
    public static void Build()
    {
        string buildPath = GetBuildPath();
        Debug.Log($"[WebGLBuilder] Starting WebGL build to: {buildPath}");

        // シーンの自動セットアップ
        EnsureSceneExists();

        // ビルドオプション設定
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetScenePaths(),
            locationPathName = buildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        // WebGL固有の設定
        ConfigureWebGLSettings();

        // ビルド実行
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var summary = report.summary;

        if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"[WebGLBuilder] Build succeeded! Size: {summary.totalSize / (1024 * 1024)} MB");
            Debug.Log($"[WebGLBuilder] Output: {buildPath}");
        }
        else
        {
            Debug.LogError($"[WebGLBuilder] Build failed with {summary.totalErrors} error(s)");
            // バッチモードでは非ゼロ終了コードを返す
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }
        }
    }

    /// <summary>
    /// 開発用ビルド（デバッグ情報付き）
    /// </summary>
    [MenuItem("Build/WebGL Build (Development)")]
    public static void BuildDevelopment()
    {
        string buildPath = GetBuildPath();
        Debug.Log($"[WebGLBuilder] Starting WebGL development build to: {buildPath}");

        EnsureSceneExists();

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetScenePaths(),
            locationPathName = buildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.Development | BuildOptions.ConnectWithProfiler
        };

        ConfigureWebGLSettings();

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var summary = report.summary;

        if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"[WebGLBuilder] Development build succeeded! Size: {summary.totalSize / (1024 * 1024)} MB");
        }
        else
        {
            Debug.LogError($"[WebGLBuilder] Development build failed with {summary.totalErrors} error(s)");
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }
        }
    }

    /// <summary>
    /// WebGL用のPlayer設定
    /// </summary>
    private static void ConfigureWebGLSettings()
    {
        PlayerSettings.productName = "English Quest";
        PlayerSettings.companyName = "EnglishQuest";
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.template = "APPLICATION:Default";
        PlayerSettings.WebGL.emscriptenArgs = "";
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WebGL, ScriptingImplementation.IL2CPP);

        // メモリ設定
#if UNITY_2020_1_OR_NEWER
        PlayerSettings.WebGL.initialMemorySize = 32;
        PlayerSettings.WebGL.maximumMemorySize = 512;
#endif
    }

    /// <summary>
    /// ビルドに含めるシーンパスを取得
    /// </summary>
    private static string[] GetScenePaths()
    {
        var scenes = EditorBuildSettings.scenes;
        if (scenes.Length == 0)
        {
            // EditorBuildSettingsにシーンがない場合、MainSceneを探す
            string mainScenePath = "Assets/Scenes/MainScene.unity";
            if (File.Exists(mainScenePath))
            {
                return new string[] { mainScenePath };
            }

            // Assetsフォルダ内の全シーンを検索
            string[] allScenes = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
            if (allScenes.Length > 0)
            {
                return allScenes;
            }

            Debug.LogError("[WebGLBuilder] No scenes found! Please add scenes to Build Settings.");
            return new string[0];
        }

        var enabledScenes = new System.Collections.Generic.List<string>();
        foreach (var scene in scenes)
        {
            if (scene.enabled)
            {
                enabledScenes.Add(scene.path);
            }
        }
        return enabledScenes.ToArray();
    }

    /// <summary>
    /// シーンが存在しない場合に自動作成
    /// </summary>
    private static void EnsureSceneExists()
    {
        string scenePath = "Assets/Scenes/MainScene.unity";
        if (!File.Exists(scenePath))
        {
            Debug.Log("[WebGLBuilder] MainScene not found. Running GameSetup first...");
            // GameSetupを実行してシーンを構築
            GameSetup.Setup();

            // シーンを保存
            if (!Directory.Exists("Assets/Scenes"))
            {
                Directory.CreateDirectory("Assets/Scenes");
            }

            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene(),
                scenePath
            );

            // EditorBuildSettingsにシーンを追加
            var scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene(scenePath, true)
            };
            EditorBuildSettings.scenes = scenes;

            Debug.Log($"[WebGLBuilder] Created and saved scene: {scenePath}");
        }
    }

    /// <summary>
    /// コマンドライン引数からビルドパスを取得
    /// </summary>
    private static string GetBuildPath()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "-buildPath")
            {
                return args[i + 1];
            }
        }
        return DefaultBuildPath;
    }
}
