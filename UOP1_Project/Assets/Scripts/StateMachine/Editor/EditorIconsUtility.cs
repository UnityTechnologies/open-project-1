using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AV.UnityEditor
{
    [InitializeOnLoad]
    public static class EditorIconsUtility
    {
        private static readonly MethodInfo setIconForObject;

        static EditorIconsUtility()
        {
            setIconForObject =
                typeof(EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
        }

        public static void SetIconForScript(string iconName, Object target)
        {
            var script = target is MonoBehaviour behaviour
                ? MonoScript.FromMonoBehaviour(behaviour)
                : MonoScript.FromScriptableObject(target as ScriptableObject);

            var icon = EditorGUIUtility.IconContent(iconName).image as Texture2D;

            setIconForObject.Invoke(null, new object[] {script, icon});
        }

        public static void SetIconForObject(string iconName, params Object[] targets)
        {
            var icon = EditorGUIUtility.IconContent(iconName).image as Texture2D;

            foreach (var target in targets)
            {
                setIconForObject.Invoke(null, new object[] {target, icon});
            }
        }
    }
}