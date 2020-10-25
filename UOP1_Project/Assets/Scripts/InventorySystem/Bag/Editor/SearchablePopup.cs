/*
MIT License
Copyright (c) 2019 Miłosz Matkowski(arimger)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Source: https://github.com/arimger/Unity-Editor-Toolbox
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


/// <summary>
/// Searchable popup content that allows user to filter items using a provided string value.
/// </summary>
public class SearchablePopup : PopupWindowContent
{
    public static void Show(Rect activatorRect, int current, string[] options, Action<int> onSelect)
    {
        PopupWindow.Show(activatorRect, new SearchablePopup(current, options, onSelect));
    }

    private readonly Action<int> onSelect;

    private readonly SearchArray searchArray;
    private readonly SearchField searchField;

    private readonly int startIndex;

    private int optionIndex = -1;
    private int scrollIndex = -1;

    private Vector2 scroll;

    private Rect toolbarRect;
    private Rect contentRect;


    private SearchablePopup(int startIndex, string[] options, Action<int> onSelect)
    {
        this.startIndex = startIndex;

        searchArray = new SearchArray(options);
        searchField = new SearchField();

        this.onSelect = onSelect;
        this.onSelect += (i) => { editorWindow.Close(); };
    }


    private void SelectItem(int index)
    {
        onSelect(index);
    }

    private void HandleKeyboard()
    {
        var currentEvent = Event.current;

        if (currentEvent.type == EventType.KeyDown)
        {
            if (currentEvent.keyCode == KeyCode.DownArrow)
            {
                GUI.FocusControl(null);
                optionIndex = Mathf.Min(searchArray.ItemsCount - 1, optionIndex + 1);
                scrollIndex = optionIndex;
                currentEvent.Use();
            }

            if (currentEvent.keyCode == KeyCode.UpArrow)
            {
                GUI.FocusControl(null);
                optionIndex = Mathf.Max(0, optionIndex - 1);
                scrollIndex = optionIndex;
                currentEvent.Use();
            }

            if (currentEvent.keyCode == KeyCode.Return)
            {
                GUI.FocusControl(null);
                if (optionIndex >= 0 && optionIndex < searchArray.ItemsCount)
                {
                    SelectItem(searchArray.GetItemAt(optionIndex).index);
                }

                currentEvent.Use();
            }

            if (currentEvent.keyCode == KeyCode.Escape)
            {
                GUI.FocusControl(null);
                editorWindow.Close();
            }
        }
    }

    private void DrawToolbar(Rect rect)
    {
        if (Event.current.type == EventType.Repaint)
        {
            Style.toolbarStyle.Draw(rect, false, false, false, false);
        }

        rect.xMin += Style.padding;
#if !UNITY_2019_3_OR_NEWER
            rect.xMax -= Style.padding;
#endif
        rect.yMin += Style.spacing;
        rect.yMax -= Style.spacing;

        searchArray.Search(searchField.OnGUI(rect, searchArray.Filter, Style.searchBoxStyle,
            Style.showCancelButtonStyle,
            Style.hideCancelButtonStyle));
    }

    private void DrawContent(Rect rect)
    {
        var currentEvent = Event.current;

        //prepare base rects for the whole content and a particular element
        var contentRect = new Rect(0, 0, rect.width - Style.scrollbarStyle.fixedWidth, searchArray.ItemsCount * Style.height);
        var elementRect = new Rect(0, 0, rect.width, Style.height);

        scroll = GUI.BeginScrollView(rect, scroll, contentRect);

        //iterate over all searched and available items
        for (var i = 0; i < searchArray.ItemsCount; i++)
        {
            if (currentEvent.type == EventType.Repaint && scrollIndex == i)
            {
                GUI.ScrollTo(elementRect);
                scrollIndex = -1;
            }

            if (elementRect.Contains(currentEvent.mousePosition))
            {
                if (currentEvent.type == EventType.MouseMove || currentEvent.type == EventType.ScrollWheel)
                {
                    optionIndex = i;
                }

                if (currentEvent.type == EventType.MouseDown)
                {
                    SelectItem(searchArray.GetItemAt(i).index);
                }
            }

            if (optionIndex == i)
            {
                GUI.Box(elementRect, GUIContent.none, Style.selectionStyle);
            }

            //draw proper label for the associated item
            elementRect.xMin += Style.indent;
            GUI.Label(elementRect, searchArray.GetItemAt(i).label);
            elementRect.xMin -= Style.indent;
            elementRect.y = elementRect.yMax;
        }

        GUI.EndScrollView();
    }


    public override void OnOpen()
    {
        EditorApplication.update += editorWindow.Repaint;
    }

    public override void OnClose()
    {
        EditorApplication.update -= editorWindow.Repaint;
    }

    public override void OnGUI(Rect rect)
    {
        //set toolbar rect based on the built-in toolbar style
        toolbarRect = new Rect(0, 0, rect.width, Style.toolbarStyle.fixedHeight);
        //set content rect adjusted to the toolbar container
        contentRect = Rect.MinMaxRect(0, toolbarRect.yMax, rect.xMax, rect.yMax);

        HandleKeyboard();
        DrawToolbar(toolbarRect);
        DrawContent(contentRect);
        //additionally disable all GUI controls
        GUI.enabled = false;
    }


    private class SearchArray
    {
        public struct Item
        {
            public int index;

            public string label;

            public Item(int index, string label)
            {
                this.index = index;
                this.label = label;
            }
        }


        private readonly List<Item> items;
        private readonly string[] options;


        public SearchArray(string[] options)
        {
            this.options = options;
            items = new List<Item>();
            Search("");
        }


        public bool Search(string filter)
        {
            if (Filter == filter)
            {
                return false;
            }

            items.Clear();

            for (var i = 0; i < options.Length; i++)
            {
                if (string.IsNullOrEmpty(filter) || options[i].ToLower().Contains(filter.ToLower()))
                {
                    var item = new Item(i, options[i]);

                    if (string.Equals(options[i], filter, StringComparison.CurrentCultureIgnoreCase))
                    {
                        items.Insert(0, item);
                    }
                    else
                    {
                        items.Add(item);
                    }
                }
            }

            Filter = filter;
            return true;
        }

        public Item GetItemAt(int index)
        {
            return items[index];
        }


        public int ItemsCount => items.Count;

        public string Filter { get; private set; }
    }


    internal static class Style
    {
        internal static readonly float indent = 8.0f;
        internal static readonly float height = EditorGUIUtility.singleLineHeight;
        internal static readonly float padding = 6.0f;
        internal static readonly float spacing = 2.0f;

        internal static GUIStyle toolbarStyle;
        internal static GUIStyle scrollbarStyle;
        internal static GUIStyle selectionStyle;
        internal static GUIStyle searchBoxStyle;
        internal static GUIStyle showCancelButtonStyle;
        internal static GUIStyle hideCancelButtonStyle;

        static Style()
        {
            toolbarStyle = new GUIStyle(EditorStyles.toolbar);
            scrollbarStyle = new GUIStyle(GUI.skin.verticalScrollbar);
            selectionStyle = new GUIStyle("SelectionRect");
            searchBoxStyle = new GUIStyle("ToolbarSeachTextField");
            showCancelButtonStyle = new GUIStyle("ToolbarSeachCancelButton");
            hideCancelButtonStyle = new GUIStyle("ToolbarSeachCancelButtonEmpty");
        }
    }
}