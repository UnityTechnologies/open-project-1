using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SceneSelectorInternal
{
	internal static class KeyValuePairExtension
	{
		public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
		{
			key = tuple.Key;
			value = tuple.Value;
		}
	}

	internal static class Helper
	{
		public const float kColorMarkerNormalSize = 4.0f;
		public const float kColorMarkerHoveredSize = 6.0f;
		public static readonly Color kColorMarkerDarkTint = Color.gray;
		public static readonly Color kColorMarkerLightTint = new Color(1.0f, 1.0f, 1.0f, 0.32f);

		public struct ReplaceColor : IDisposable
		{
			public static ReplaceColor With(Color color) => new ReplaceColor(color);

			private Color _oldColor;

			private ReplaceColor(Color color)
			{
				_oldColor = GUI.color;
				GUI.color = color;
			}

			void IDisposable.Dispose() => GUI.color = _oldColor;
		}

		private static readonly Dictionary<Type, Color> kDefaultMarkerColors = new Dictionary<Type, Color>()
		{
			{ typeof(PersistentManagersSO), Color.magenta },
			{ typeof(LocationSO), Color.green },
			{ typeof(MenuSO), Color.cyan },
		};

		public static void RepaintOnMouseMove(EditorWindow window)
		{
			if (Event.current.type == EventType.MouseMove)
				window.Repaint();
		}

		public static bool DrawColorMarker(Rect rect, Color color, bool isClickable = false, bool isHoverable = false)
		{
			bool isClicked = false;
			if (isClickable)
				isClicked = GUI.Button(rect, GUIContent.none, GUIStyle.none);

			var currentEvent = Event.current;
			var isHovered = isHoverable && rect.Contains(currentEvent.mousePosition);
			var targetSize = isHovered ? kColorMarkerHoveredSize : kColorMarkerNormalSize;

			var size = rect.size;
			rect.size = new Vector2(targetSize, targetSize);
			rect.position += (size - rect.size) * 0.5f;

			Rect shadowRect = rect;
			shadowRect.position -= Vector2.one;
			shadowRect.size += Vector2.one;
			Rect lightRect = rect;
			lightRect.size += Vector2.one;

			GUIUtility.RotateAroundPivot(45.0f, rect.center);
			EditorGUI.DrawRect(shadowRect, color * kColorMarkerDarkTint);
			EditorGUI.DrawRect(lightRect, kColorMarkerLightTint);
			EditorGUI.DrawRect(rect, color);
			GUIUtility.RotateAroundPivot(-45.0f, rect.center);

			return isClicked;
		}

		public static void OpenSceneSafe(string path)
		{
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				EditorSceneManager.OpenScene(path);
			}
		}

		public static Color GetDefaultColor(GameSceneSO gameScene)
		{
			var type = gameScene.GetType();
			if (kDefaultMarkerColors.TryGetValue(type, out var color))
				return color;
			return Color.red;
		}

		public static void RunOnNextUpdate(Action action)
		{
			void Run()
			{
				action?.Invoke();
				EditorApplication.update -= Run;
			}
			EditorApplication.update += Run;
		}

		public static int FindAssetsByType<T>(List<T> assets) where T : UnityEngine.Object
		{
			int foundAssetsCount = 0;
			var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
			for (int i = 0, count = guids.Length; i < count; ++i)
			{
				var path = AssetDatabase.GUIDToAssetPath(guids[i]);
				T asset = AssetDatabase.LoadAssetAtPath<T>(path);
				if (asset != null)
				{
					assets.Add(asset);
					foundAssetsCount += 1;
				}
			}
			return foundAssetsCount;
		}
	}
}
