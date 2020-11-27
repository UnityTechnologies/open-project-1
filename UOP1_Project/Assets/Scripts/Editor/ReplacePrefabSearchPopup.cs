using System;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;
using static UnityEditor.EditorJsonUtility;
using static UnityEngine.Application;

namespace UOP1.EditorTools.Replacer
{
	internal class ReplacePrefabSearchPopup : EditorWindow
    {
	    private static Styles styles;

    	private class ViewState : ScriptableObject
    	{
    		public TreeViewState treeViewState = new TreeViewState();
    	}

    	private static ReplacePrefabSearchPopup window;
    	private static string assetPath;

    	private Action<GameObject> onSelectEntry;

    	private SearchField searchField;
    	private ReplacePrefabTreeView tree;
    	private ViewState viewState;

        private Rect itemRect;

    	public static void Show(Rect itemRect, Vector2 position, Vector2 size, Action<GameObject> onSelectEntry)
    	{
    		if (window)
    			return;

    		assetPath = Path.Combine(dataPath.Remove(dataPath.Length - 7, 7), "Library", "ReplacePrefabTreeState.asset");

    		window = CreateInstance<ReplacePrefabSearchPopup>();
    		window.ShowAsDropDown(new Rect(position, Vector2.zero), size);

    		//onSelectEntry += _ => window.Close();
            window.itemRect = itemRect;
    		window.onSelectEntry = onSelectEntry;
    		window.Init();
    	}

    	private void Init()
    	{
    		viewState = CreateInstance<ViewState>();

    		if (File.Exists(assetPath))
    			FromJsonOverwrite(File.ReadAllText(assetPath), viewState);

    		tree = new ReplacePrefabTreeView(viewState.treeViewState);
    		tree.onSelectEntry += onSelectEntry;

    		searchField = new SearchField();
    		searchField.downOrUpArrowKeyPressed += tree.SetFocusAndEnsureSelectedItem;
    		searchField.SetFocus();
    	}

    	private void OnDisable()
    	{
    		File.WriteAllText(assetPath, ToJson(viewState));
    	}

    	private void OnGUI()
    	{
	        if (styles == null)
		        styles = new Styles();

    		DoToolbar();
    		DoTreeView();
    	}

    	void DoToolbar()
    	{
    		tree.searchString = searchField.OnToolbarGUI(tree.searchString);
            GUILayout.Space(-2);

            var headerRect = GUILayoutUtility.GetRect(0, itemRect.width, 0, itemRect.height);

            if (Event.current.type == EventType.Repaint)
				EditorGUI.DrawRect(headerRect, styles.headerColor);

    		GUILayout.Label("Replace With...", styles.headerLabel);
    	}

    	void DoTreeView()
    	{
    		var rect = GUILayoutUtility.GetRect(0, 10000, 0, 10000);
    		rect.x += 2;
    		rect.width -= 4;

    		rect.y += 2;
    		rect.height -= 4;

    		tree.OnGUI(rect);
    	}

        private class Styles
        {
	        public Color headerColor = isProSkin ? new Color32(77, 77, 77, 255) : new Color32(174, 174, 174, 255);

	        public GUIStyle header = new GUIStyle("AC BoldHeader")
	        {
		        fixedHeight = 0, stretchWidth = true,
		        margin = new RectOffset(0, 0, 0, 0)
	        };

	        public GUIStyle headerLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
	        {
		        fontSize = 11, fontStyle = FontStyle.Bold
	        };
        }
    }
}
