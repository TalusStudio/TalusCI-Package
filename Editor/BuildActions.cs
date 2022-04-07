using System.Linq;

using UnityEditor;

using Unity.EditorCoroutines.Editor;

using TalusCI.Editor.Models;

namespace TalusCI.Editor
{
    public static class BuildActions
    {
        private static readonly FetchAppInfo _FetchedAppInfo = new FetchAppInfo();

        public static void IOSDevelopment()
        {
            EditorUserBuildSettings.development = true;
            EditorCoroutineUtility.StartCoroutineOwnerless(_FetchedAppInfo.GetAppInfo(CreateBuild));
        }

        public static void IOSRelease()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(_FetchedAppInfo.GetAppInfo(CreateBuild));
        }

        private static void CreateBuild(AppModel app)
        {
            if (PlayerSettings.SplashScreen.showUnityLogo)
            {
                PlayerSettings.SplashScreen.showUnityLogo = false;
            }

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, app.app_bundle);
            PlayerSettings.productName = app.app_name;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);

            AssetDatabase.SaveAssets();

            BuildPipeline.BuildPlayer(GetScenes(), iOSAppBuildInfo.IOSFolder, BuildTarget.iOS, BuildOptions.CompressWithLz4HC);
            EditorApplication.Exit(0);
        }

        private static string[] GetScenes()
        {
            return (from t in EditorBuildSettings.scenes where t.enabled select t.path).ToArray();
        }
    }
}
