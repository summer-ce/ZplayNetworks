using UnityEditor;
using Assets.ZplaySDK.Scripts.Services.Scriptable;

namespace Assets.ZplaySDK.Editor
{
    public static class MenuExtentions
    {
        [MenuItem("Zplay/Data/Game settings", false, 10)]
        public static void OpenGameSettings()
        {
            Selection.activeObject = GameSettings.Instance;
        }
    }
}
