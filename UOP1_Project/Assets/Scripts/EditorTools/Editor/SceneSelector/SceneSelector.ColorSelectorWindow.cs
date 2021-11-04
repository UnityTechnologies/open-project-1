using UnityEditor;
using UnityEngine;
using SceneSelectorInternal;

public partial class SceneSelector : EditorWindow
{
	private class ColorSelectorWindow : EditorWindow
	{
		private static readonly float kCellSize = PreferencesWindow.kColorMarkerFieldSize * 2.0f;
		private static readonly Color kCellBackColor = new Color(0.0f, 0.0f, 0.0f, 0.1f);
		private static readonly Vector2 kCellOffset = new Vector2(1.0f, 1.0f);
		private static readonly Vector2Int kCount = new Vector2Int(5, 5);

		private PreferencesWindow _owner;
		private Color[,] _colors;
		private Item _item;

		public static ColorSelectorWindow Open(Rect rect, PreferencesWindow owner, Item item)
		{
			var window = CreateInstance<ColorSelectorWindow>();
			window.Init(rect, owner, item);
			return window;
		}

		private void Init(Rect rect, PreferencesWindow owner, Item item)
		{
			var size = (Vector2)kCount * kCellSize;
			ShowAsDropDown(rect, size);
			_owner = owner;
			_item = item;
		}

		private void OnEnable()
		{
			wantsMouseMove = true;
			InitColors();
		}

		private void OnGUI()
		{
			Helper.RepaintOnMouseMove(this);
			DrawMarkers();
		}

		private void DrawMarkers()
		{
			var size = new Vector2(kCellSize, kCellSize);
			for (int x = 0; x < kCount.x; ++x)
			{
				for (int y = 0; y < kCount.y; ++y)
				{
					var color = _colors[x, y];
					var position = size * new Vector2(x, y);
					var rect = new Rect(position, size);
					{
						var cellBackRect = rect;
						cellBackRect.position += kCellOffset;
						cellBackRect.size -= kCellOffset * 2.0f;
						EditorGUI.DrawRect(cellBackRect, kCellBackColor);
					}
					if (Helper.DrawColorMarker(rect, color, true, true))
					{
						_item.color = color;
						_owner.RepaintAll();
						Close();
					}
				}
			}
		}

		private void InitColors()
		{
			var count = kCount.x * kCount.y;
			_colors = new Color[kCount.x, kCount.y];
			for (int x = 0; x < kCount.x; ++x)
			{
				var h = x * kCount.y;
				for (int y = 0; y < kCount.y; ++y)
				{
					float hue = (float)(h + y) / count;
					_colors[x, y] = Color.HSVToRGB(hue, 1.0f, 1.0f);
				}
			}
		}
	}
}
