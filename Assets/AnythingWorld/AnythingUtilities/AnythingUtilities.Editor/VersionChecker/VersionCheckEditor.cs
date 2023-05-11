#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine;

namespace AnythingWorld.Utilities.Editor
{
    public class VersionCheckEditor : UnityEditor.Editor
    {
        private static string versionCheckOptOut = "anythingWorldVersionCheckOptOut";
        private static string lastVersionOptedOut = "anythingWorldVersionCheckedOptOut";

        public static void DisplayUpdateDialogue()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                CoroutineExtension.StartEditorCoroutine(GetVersion($"{NetworkConfig.ApiUrlStem}/version?v=1"));
            }
        }

        private static IEnumerator GetVersion(string uri)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(uri))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    VersionResponse response = VersionResponse.CreateFromJson(uwr.downloadHandler.text);

                    if (response.version!=AnythingSettings.PackageVersion && !AnythingSettings.PackageVersion.Contains("a") && AnythingSettings.PackageVersion.Contains("BETA"))
                    {
                        //If user has opted out of seeing the upgrade dialogue
                        if (CheckDialogueEnabled(response))
                        {
                            var choice = EditorUtility.DisplayDialogComplex("Version Upgrade", response.message, "Get Package", "Exit", "Don't Show Me Again");
                            switch (choice)
                            {
                                case 0:
                                    Application.OpenURL(response.downloadLink);
                                    break;
                                case 1:
                                    break;
                                case 2:
                                    //Set editor prefs to opt out of seeing this dialogue again until the version changes. 
                                    EditorPrefs.SetBool(versionCheckOptOut, true);
                                    EditorPrefs.SetString(lastVersionOptedOut, response.version);
                                    break;
                            }
                        }
                    }
                }
            }
            yield return null;
        }

        private static bool CheckDialogueEnabled(VersionResponse response)
        {
            if (EditorPrefs.HasKey(versionCheckOptOut) && EditorPrefs.GetBool(versionCheckOptOut) == true)
            {
                //If version is higher than the version opted out of, show to user again
                if (EditorPrefs.HasKey(lastVersionOptedOut) && EditorPrefs.GetString(lastVersionOptedOut) != response.version)
                {
                    EditorPrefs.SetBool(versionCheckOptOut, false);
                    EditorPrefs.DeleteKey(lastVersionOptedOut);
                    return true;
                }
                else
                {
                    //If opt out matches version then do not show
                    return false;
                }
            }
            else
            {
                //If no opt out stored, show dialogue to users
                return true;
            }
        }

        [System.Serializable]
        public class VersionResponse
        {
            public string downloadLink;
            public string version;
            public string message;

            public static VersionResponse CreateFromJson(string jsonString)
            {
                return JsonUtility.FromJson<VersionResponse>(jsonString);
            }
        }

    }

}

#endif