using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ChopChop.EditorTools.jandd661
{
    public class SwapPrefabs : EditorWindow
    {
        private const string menuPath = "Tools/Prefab Swap Tool";
        private string greatingMessage = "This is the first pass of Chop Chop Prefab Swap. Please save your project before continuing!\nWARNING: Replacing prefabs that have scripts with references to other scene objects will break those references!";
        private string nonPrefabChildMainMessage = "There are object warnings! Please review the below list.";
        private string nonPrefabChildObjectMessage = "This prefab has one or more non-prefab object(s) as a child.These objects will be retained and reatached to the new prefab.This may break references! ({0})";
        private float prefabButtonScale = 0.8f;
        private string prefabButtonScaleToolTip = "Size of the preview images.";
        private float buttonSize = 100f;
        private float transformLableWidth = 70f;
        private float windowMinSizeWidth = 500f;
        private float windowMinSizeHeight = 500f;
        private int floatRoundingDecimals = 2;
        private int objectsPerBatch = 20;
        private string objectsPerBatchToolTip = "The max number of objects to list in the tool. Reduce this number if you have frame rate issues.";
        private string noNewPrefabMessage = "Please select a new prefab to swap with!";
        //----------------------------------------------------------------
        // Don't mess around below here unless you know what your doing =)
        //----------------------------------------------------------------
        [SerializeField] private GameObject sourcePrefab;
        [SerializeField] private GameObject newPrefab;
        private Vector2 svPosition;
        private List<GameObject> currentDerivedList = new List<GameObject>();
        private GameObject lastSelectedPrefab = null;
        private bool matchRotation = true;
        private bool matchScale = true;
        private bool swapInprogress = false;
        private bool showMainWarningMessage = false;
        private bool showNoNewPrefabMessage = false;


        [MenuItem(menuPath)]
        public static void ShowWindow()
        {
            SwapPrefabs window = GetWindow<SwapPrefabs>("Prefab Swap", true);
            window.name = "Chop Chop Prefab Swap";
            window.Show();
        }

        public void Awake()
        {
            Undo.undoRedoPerformed += UndoCallback;
            this.minSize = new Vector2(windowMinSizeWidth, windowMinSizeHeight);
        }

		private void OnDestroy()
		{
			CleanUp();
		}

		private void OnDisable()
        {
			CleanUp();
		}

        private void OnHierarchyChange()
        {
            UpdatAndRepaint("OnHierarchyChange");
        }

        private void OnSelectionChange()
        {
			UpdatAndRepaint("OnSelectionChange");
        }

        private void OnGUI()
        {
			if (swapInprogress == false)
			{
				DrawGui();
			}
		}

        private void UpdatAndRepaint(string arg1)
        {
			//Debug.Log("CheckRepaint from " + arg1);
            if (swapInprogress == false)
            {
                swapInprogress = true;
                SetCurrentDerivedList(GetDerivatives(sourcePrefab));
                swapInprogress = false;
            }
        }

		private void CleanUp()
		{
			Undo.undoRedoPerformed -= UndoCallback;
			currentDerivedList = null;
		}

        private void DrawGui()
        {
			if (swapInprogress == true)
			{
				return;
			}

            List<GameObject> localDerivitiveList = GetCurrentDerivedList();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.EndVertical();
            //-----------------------------------
            // save for later
            //-----------------------------------
            //currentTab = GUILayout.Toolbar(currentTab, new string[] { "Prefab Swap", "Object Swap" });

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox(greatingMessage, MessageType.Info, true);
            EditorGUILayout.EndHorizontal();

            if (showMainWarningMessage == true)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(nonPrefabChildMainMessage, MessageType.Warning, true);
                EditorGUILayout.EndHorizontal();
            }

            if (showNoNewPrefabMessage == true && newPrefab == null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(noNewPrefabMessage, MessageType.Warning, true);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                showNoNewPrefabMessage = false;
            }


            // PREVIEW IMAGE SCALE SLIDER
            EditorGUILayout.BeginHorizontal();
            prefabButtonScale = EditorGUILayout.Slider(new GUIContent("Image Scale: ", prefabButtonScaleToolTip), prefabButtonScale, 0.5f, 1.5f);
            buttonSize = 100f * prefabButtonScale;
            EditorGUILayout.EndHorizontal();
            // BATCH SIZE
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(150));
            objectsPerBatch = EditorGUILayout.IntField(new GUIContent("Objects per batch: ", objectsPerBatchToolTip), objectsPerBatch);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.Width(50));
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.Width(this.position.width - 200));
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            // LINE
            EditorGUILayout.BeginHorizontal();
            DrawUILine(Color.gray);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(buttonSize));
            // PREVIEW IMAGE
            EditorGUILayout.BeginHorizontal();
            if (sourcePrefab != null)
            {
                Texture2D tex = AssetPreview.GetAssetPreview(sourcePrefab);
                if (GUILayout.Button(tex, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(sourcePrefab));
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
			// SEARCH FOR BOX
			DrawObjectsDerivedFromBox(localDerivitiveList);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            // LINE
            EditorGUILayout.BeginHorizontal();
            DrawUILine(Color.gray);
            EditorGUILayout.EndHorizontal();
            // REPLACE WITH BOX
            DrawReplaceWithBox(localDerivitiveList);
            // LINE
            EditorGUILayout.BeginHorizontal();
            DrawUILine(Color.gray);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
			//---------------------------------------
			// DERIVITIVES LIST
			//---------------------------------------
			// Check if any objects have been deleted from the scene since the list was loaded
			List<GameObject> currentPageOfObjects = new List<GameObject>();
			for (int i = 0; i < localDerivitiveList.Count; i++)
			{
				if (localDerivitiveList[i] == null)
				{
					localDerivitiveList.RemoveAt(i);
				}
			}
			if (localDerivitiveList.Count > 0 && swapInprogress != true)
			{
				svPosition = EditorGUILayout.BeginScrollView(svPosition);
                showMainWarningMessage = false;
                for (int i = 0; i < localDerivitiveList.Count && i < objectsPerBatch; i++)
				{
					if (localDerivitiveList[i] != null)
					{
						currentPageOfObjects.Add(localDerivitiveList[i]);
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.BeginVertical(GUILayout.Width(buttonSize));
						EditorGUILayout.BeginHorizontal();
						Texture2D tex = AssetPreview.GetAssetPreview(localDerivitiveList[i]);
						if (GUILayout.Button(tex, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
						{
							Selection.activeObject = localDerivitiveList[i];
							SceneView.FrameLastActiveSceneViewWithLock();
						}
						EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Replace"))
                        {
                            if (newPrefab != null)
                            {
                                ReplaceWithPrefab(localDerivitiveList[i], newPrefab, matchRotation, matchScale, "", true);
                                showMainWarningMessage = false;
                                showNoNewPrefabMessage = false;
                            }
                            else
                            {
                                showNoNewPrefabMessage = true;
                            }

                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.EndVertical();
						EditorGUILayout.BeginVertical();
						// DERIVED OBJECT FIELD
						GUI.enabled = false;
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.ObjectField("", localDerivitiveList[i], typeof(GameObject), false);
						EditorGUILayout.EndHorizontal();
						GUI.enabled = true;
						// DERIVED OBJECT TRANSFORM
						DrawTransformLable(localDerivitiveList[i], 50f);

                        if(localDerivitiveList[i] != null)
                        {
                            bool hasNonPrefabObjectAsChild = false;
                            string objectWarningMessage = "";
                            List<GameObject> childList = GetChildRecursive(localDerivitiveList[i]);
                            for (int j = 0; j < childList.Count; j++)
                            {
                                GameObject childDerivedObject = PrefabUtility.GetCorrespondingObjectFromSource(childList[j]);
                                if (childDerivedObject == null)
                                {
                                    if (objectWarningMessage != "")
                                    {
                                        objectWarningMessage += ", ";
                                    }
                                    objectWarningMessage += childList[j].name;
                                    hasNonPrefabObjectAsChild = true;
                                }

                            }
                            if (hasNonPrefabObjectAsChild == true)
                            {
                                showMainWarningMessage = true;
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.HelpBox(string.Format(nonPrefabChildObjectMessage, objectWarningMessage), MessageType.Warning, true);
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            DrawUILine(Color.gray);
                            EditorGUILayout.EndHorizontal();
                        }
                    }
				}
				EditorGUILayout.EndScrollView();
			}
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (localDerivitiveList.Count > 0)
            {
                if (GUILayout.Button("Replace all"))
                {
                    if(newPrefab != null)
                    {
                        ReplaceSceneObjects(currentPageOfObjects, newPrefab, matchRotation, matchScale);
                        showMainWarningMessage = false;
                        showNoNewPrefabMessage = false;
                    }
                    else
                    {
                        showNoNewPrefabMessage = true;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            localDerivitiveList = null;
		}

        private List<GameObject> GetChildRecursive(GameObject inObject, List<GameObject> inList = null)
        {
            List<GameObject> outList = null;
            outList = inList == null ? new List<GameObject>() : inList;
            if (inObject == null)
            {
                return outList;
            }

            foreach (Transform child in inObject.transform)
            {
                if (child == null)
                {
                    continue;
                }
                outList.Add(child.gameObject);
                GetChildRecursive(child.gameObject, outList);
            }
            return outList;
        }

        private void DrawReplaceWithBox(List<GameObject> localDerivitiveList)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(buttonSize));
            // PREVIEW IMAGE
            EditorGUILayout.BeginHorizontal();
            if (newPrefab != null)
            {
                Texture2D tex = AssetPreview.GetAssetPreview(newPrefab);
                if (GUILayout.Button(tex, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(newPrefab));
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            // NEW PREFAB LABLE
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Replace with this prefab:");
            EditorGUILayout.EndHorizontal();
            // NEW PREFAB INPUT BOX
            EditorGUILayout.BeginHorizontal();
            newPrefab = (GameObject)EditorGUILayout.ObjectField("", newPrefab, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();
            // CHECK BOXES
            EditorGUILayout.BeginHorizontal();
            matchRotation = GUILayout.Toggle(matchRotation, "Match scene object rotation");
            matchScale = GUILayout.Toggle(matchScale, "Match scene object scale");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawObjectsDerivedFromBox(List<GameObject> localDerivitiveList)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Find scene objects derived from:");
            EditorGUILayout.EndHorizontal();
            // SOURCE PREFAB INPUT BOX
            EditorGUILayout.BeginHorizontal();
            sourcePrefab = (GameObject)EditorGUILayout.ObjectField("", sourcePrefab, typeof(GameObject), false);
            if ((sourcePrefab == null || sourcePrefab != lastSelectedPrefab))
            {
                localDerivitiveList.Clear();
            }
            EditorGUILayout.EndHorizontal();
            // FIND DERIVITIVES BUTTON
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Find Derivatives"))
            {
                if (sourcePrefab != null)
                {
                    SetCurrentDerivedList(GetDerivatives(sourcePrefab));
                    lastSelectedPrefab = sourcePrefab;
                }
                else
                {
                    localDerivitiveList.Clear();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(" Total objects found: " + localDerivitiveList.Count);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

        }

        private List<GameObject> GetDerivatives(GameObject sourcePrefab)
        {
			swapInprogress = true;
			if (sourcePrefab == null)
            {
                return null;
            }

            List<GameObject> outList = new List<GameObject>();
            foreach (GameObject sceneObject in FindObjectsOfType(typeof(GameObject)))
            {
                GameObject derivedObject = PrefabUtility.GetCorrespondingObjectFromSource(sceneObject);
                if (derivedObject == sourcePrefab)
                {
                    outList.Add(sceneObject);
                }
            }
			swapInprogress = false;
			return outList;
        }

        private void ReplaceSceneObjects(List<GameObject> objectList, GameObject prefab, bool matchRotation, bool matchScale)
        {
            swapInprogress = true;
			if(objectList == null || prefab == null)
			{
				Debug.LogError("[ReplaceSceneObjects] is missing required peramiter! (objectList=" + objectList + ", prefab=" + prefab + ")");
				return;
			}
            for (int i = 0; i < objectList.Count; i++)
            {
                string name = prefab.name + " (" + (i + 1) + ")";
                ReplaceWithPrefab(objectList[i], prefab, matchRotation, matchScale, name, false);
            }
            swapInprogress = false;

        }

        private GameObject ReplaceWithPrefab(GameObject objectToReplace, GameObject prefab, bool matchRotation, bool matchScale, string name = "", bool setInprogress = true)
        {

            if (objectToReplace == null || prefab == null)
            {
                return null;
            }

            if (setInprogress == true)
            {
                swapInprogress = true;
            }

            List<GameObject> nonPrefabs = new List<GameObject>();
            List<GameObject> childList = GetChildRecursive(objectToReplace);

            for (int i = 0; i < childList.Count; i++)
            {
                GameObject childDerivedObject = PrefabUtility.GetCorrespondingObjectFromSource(childList[i]);
                if (childDerivedObject == null)
                {
                    nonPrefabs.Add(childList[i]);
                }
            }

            Undo.RegisterCompleteObjectUndo(objectToReplace, objectToReplace.name + " object state");
            GameObject newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            Undo.RegisterCreatedObjectUndo(newObject, "Spawn new prefab");
            Undo.SetTransformParent(newObject.transform, objectToReplace.transform.parent, newObject.name + " root parenting");
            Undo.RegisterCompleteObjectUndo(newObject, newObject.name + " object state");
            newObject.transform.localPosition = objectToReplace.transform.localPosition;
            newObject.name = name != "" ? name : prefab.name;
            if (matchRotation == true) newObject.transform.localRotation = objectToReplace.transform.localRotation;
            if (matchScale == true) newObject.transform.localScale = objectToReplace.transform.localScale;

            for (int i = 0; i < nonPrefabs.Count; i++)
            {
                Undo.RegisterCompleteObjectUndo(nonPrefabs[i], nonPrefabs[i].name + " object state");
                Undo.SetTransformParent(nonPrefabs[i].transform, newObject.transform, nonPrefabs[i].name + " parent change");
            }
            Undo.DestroyObjectImmediate(objectToReplace);
			if (setInprogress == true)
            {
                swapInprogress = false;
            }
            return newObject;
        }

        private List<GameObject> GetCurrentDerivedList()
        {
            return currentDerivedList;
        }

        private List<GameObject> SetCurrentDerivedList(List<GameObject> inList)
        {
			currentDerivedList = inList;
			this.Repaint();
			return inList;
        }

        private void DrawTransformLable(GameObject gameObject, float lableWidth)
        {
			if(gameObject != null)
			{
				Transform inTransform = gameObject.transform;
				// POSITION
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical(GUILayout.Width(transformLableWidth));
				GUILayout.Label("Position:");
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(lableWidth));
				GUILayout.Label(RoundFloat(inTransform.localPosition.x, floatRoundingDecimals).ToString());
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(lableWidth));
				GUILayout.Label(RoundFloat(inTransform.localPosition.y, floatRoundingDecimals).ToString());
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(lableWidth));
				GUILayout.Label(RoundFloat(inTransform.localPosition.z , floatRoundingDecimals).ToString());
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				// ROTATION
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical(GUILayout.Width(transformLableWidth));
				GUILayout.Label("Rotation:");
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(lableWidth));
				GUILayout.Label(RoundFloat(inTransform.localRotation.eulerAngles.x, floatRoundingDecimals).ToString());
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(lableWidth));
				GUILayout.Label(RoundFloat(inTransform.localRotation.eulerAngles.y, floatRoundingDecimals).ToString());
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(lableWidth));
				GUILayout.Label(RoundFloat(inTransform.localRotation.eulerAngles.z, floatRoundingDecimals).ToString());
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				// SCALE
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical(GUILayout.Width(transformLableWidth));
				GUILayout.Label("Scale:");
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(lableWidth));
				GUILayout.Label(RoundFloat(inTransform.localScale.x, floatRoundingDecimals).ToString());
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(lableWidth));
				GUILayout.Label(RoundFloat(inTransform.localScale.y, floatRoundingDecimals).ToString());
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(GUILayout.Width(lableWidth));
				GUILayout.Label(RoundFloat(inTransform.localScale.z, floatRoundingDecimals).ToString());
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
        }
        private void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        public void UndoCallback()
        {
            currentDerivedList.Clear();
            UpdatAndRepaint("UndoCallback");
        }

        private float RoundFloat(float value, int digits)
        {
            float mult = Mathf.Pow(10.0f, (float)digits);
            return Mathf.Round(value * mult) / mult;
        }

    }
}




