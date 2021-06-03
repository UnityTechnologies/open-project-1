using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.Localization;
public enum selectionType
{
	Questline,
	Quest,
	Step,
	Dialogue,

}
public class QuestEditorWindow : EditorWindow
{
	private Image actorPreview;
	private QuestSO currentSeletedQuest;
	QuestlineSO selectedQuestLine;
	int idQuestlineSelected;
	int idQuestSelected;
	[MenuItem("ChopChop/QuestEditorWindow")]
	public static void ShowWindow()
	{
		Debug.Log("Show Window");
		QuestEditorWindow wnd = GetWindow<QuestEditorWindow>();
		wnd.titleContent = new GUIContent("QuestEditorWindow");

		// Sets a minimum size to the window.
		wnd.minSize = new Vector2(250, 250);
	}

	public static void ShowArtistToolWindow()
	{
		// Opens the window, otherwise focuses it if it’s already open.
		QuestEditorWindow window = GetWindow<QuestEditorWindow>();
		// Adds a title to the window.

		Debug.Log("Show Window");
		window.titleContent = new GUIContent("QuestEditorWindow");

		// Sets a minimum size to the window.
		window.minSize = new Vector2(250, 250);

		//window.SetTool(); 
	}
	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;
		Debug.Log("Create GUI");
		// Import UXML
		var visualTree = Resources.Load<VisualTreeAsset>("QuestEditorWindow");
		root.Add(visualTree.CloneTree());

		//Add Image
		VisualElement preview = root.Q<VisualElement>("actor-preview");
		actorPreview = new Image();
		preview.Add(actorPreview);

		//Import USS
		var styleSheet = Resources.Load<StyleSheet>("QuestEditorWindow");
		root.styleSheets.Add(styleSheet);

		//Register button event
		Button refreshQuestPreviewBtn = root.Q<Button>("refresh-preview-btn");
		refreshQuestPreviewBtn.RegisterCallback<ClickEvent>((evt) => SetUpQuestPreview(currentSeletedQuest));

		Button createQuestline = rootVisualElement.Q<Button>("CreateQL");
		createQuestline.clicked += AddQuestline;
		createQuestline.SetEnabled(true);

		Button createQuest = rootVisualElement.Q<Button>("CreateQ");
		createQuest.clicked += AddQuest;
		createQuest.SetEnabled(false);

		Button createStep = rootVisualElement.Q<Button>("CreateS");
		createStep.clicked += AddStep;
		createStep.SetEnabled(false);

		LoadAllQuestsData();


	}
	private void ClearElements(selectionType type)
	{
		List<string> listElements = new List<string>();
		listElements.Clear();
		Button createQuest = rootVisualElement.Q<Button>("CreateQ");
		Button createStep = rootVisualElement.Q<Button>("CreateS");

		switch (type)
		{
			case selectionType.Questline:
				listElements.Add("steps-list");
				listElements.Add("actor-conversations");
				listElements.Add("steps-list");
				listElements.Add("dialogues-list");

				listElements.Add("step-info-scroll");
				listElements.Add("dialogue-info-scroll");
				if (createQuest != null)
				{
					createQuest.SetEnabled(true);
				}
				if (createStep != null)
				{
					createStep.SetEnabled(false);
				}

				break;
			case selectionType.Quest:
				listElements.Add("dialogues-list");

				listElements.Add("step-info-scroll");
				listElements.Add("dialogue-info-scroll");

				if (createStep != null)
				{
					createStep.SetEnabled(true);
				}

				break;
			case selectionType.Step:

				listElements.Add("dialogue-info-scroll");

				break;
		}
		foreach (string elementName in listElements)
		{
			VisualElement element = rootVisualElement.Q<VisualElement>(elementName);
			element.Clear();

		}


	}


	private void LoadAllQuestsData()
	{
		Debug.Log("LoadAllQuestsData");
		//Load all questlines
		FindAllSOByType(out QuestlineSO[] questLineSOs);
		RefreshListView(out ListView allQuestlinesListView, "questlines-list", questLineSOs);

		allQuestlinesListView.onSelectionChange += (questlineEnumerable) =>
		{
			selectedQuestLine = GetDataFromListViewItem<QuestlineSO>(questlineEnumerable);
			ClearElements(selectionType.Questline);
			idQuestlineSelected = allQuestlinesListView.selectedIndex;

			if (selectedQuestLine.Quests != null)
			{
				RefreshListView(out ListView allQuestsListView, "quests-list", selectedQuestLine.Quests.ToArray());




				allQuestsListView.onSelectionChange += (questEnumerable) =>
				{
					idQuestSelected = allQuestsListView.selectedIndex;

					currentSeletedQuest = GetDataFromListViewItem<QuestSO>(questEnumerable);
					ClearElements(selectionType.Quest);
					if (currentSeletedQuest != null && currentSeletedQuest.Steps != null)
					{
						RefreshListView(out ListView allStepsListView, "steps-list", currentSeletedQuest.Steps.ToArray());

						SetUpQuestPreview(currentSeletedQuest);

						allStepsListView.onSelectionChange += (stepEnumerable) =>
						{
							StepSO step = GetDataFromListViewItem<StepSO>(stepEnumerable);
							DisplayAllProperties(step, "step-info-scroll");
							ClearElements(selectionType.Step);
							//Find all DialogueDataSOs in the same folder of the StepSO
							FindAllDialogueInStep(step, out DialogueDataSO[] dialogueDataSOs);
							if (dialogueDataSOs != null)
							{

								RefreshListView(out ListView dialoguesListView, "dialogues-list", dialogueDataSOs);

								dialoguesListView.onSelectionChange += (dialogueEnumerable) =>
								{
									DialogueDataSO dialogueData = GetDataFromListViewItem<DialogueDataSO>(dialogueEnumerable);

									DisplayAllProperties(dialogueData, "dialogue-info-scroll");
								};
							}

						};

					}
				};
			}
		};
	}

	private T GetDataFromListViewItem<T>(IEnumerable<object> enumberable) where T : ScriptableObject
	{
		T data = default;
		foreach (var item in enumberable)
		{
			data = item as T;
		}
		return data;
	}
	private void FindAllDialogueInStep(StepSO step, out DialogueDataSO[] AllDialogue)
	{
		AllDialogue = null;
		List<DialogueDataSO> AllDialogueList = new List<DialogueDataSO>();
		if (step != null)
		{
			if (step.DialogueBeforeStep != null)
			{

				AllDialogueList.Add(step.DialogueBeforeStep);
			}
			if (step.CompleteDialogue != null)
			{
				AllDialogueList.Add(step.CompleteDialogue);


			}
			if (step.IncompleteDialogue != null)
			{

				AllDialogueList.Add(step.IncompleteDialogue);


			}

		}
		Debug.Log("AllDialogueList" + AllDialogueList.ToArray());
		if (AllDialogueList != null)
			AllDialogue = AllDialogueList.ToArray();

	}
	private void SetUpQuestPreview(QuestSO quest)
	{
		if (quest == null)
			return;
		if (quest.Steps == null)
			return;
		if (quest.Steps.Count > 0 && quest.Steps[0].Actor != null)
			LoadActorImage(quest.Steps[0].Actor.name);

		//Clear actor conversations area
		rootVisualElement.Q<VisualElement>("actor-conversations").Clear();

		foreach (StepSO step in quest.Steps)
			LoadAndInitStepUXML(step);
	}

	private void LoadAndInitStepUXML(StepSO step)
	{
		//Clear actor conversations area
		VisualElement actorConversationsVE = rootVisualElement.Q<VisualElement>("actor-conversations");

		// Import UXML
		var stepVisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Quests/Editor/StepDetail.uxml");
		VisualElement stepVE = stepVisualTree.CloneTree();
		VisualElement dialogueAreaVE = stepVE.Q<VisualElement>("dialogue-area");

		//Title
		stepVE.Q<Label>("step-title-label").text = "Step" + step.name[1];

		//IsDone
		Toggle isDoneToggle = stepVE.Q<Toggle>("step-done-toggle");
		isDoneToggle.value = step.IsDone;
		isDoneToggle.SetEnabled(false);

		//SD
		if (step.DialogueBeforeStep != null)
			LoadAndInitStartDialogueLineUXML(step.DialogueBeforeStep, dialogueAreaVE);

		//CD ID if any
		if (step.CompleteDialogue != null)
			LoadAndInitOptionsDialogueLineUXML(step.CompleteDialogue, step.IncompleteDialogue, dialogueAreaVE);

		//Type (Check Item etc)
		if (step.Type == StepType.Dialogue)
		{
			VisualElement itemValidateVE = stepVE.Q<VisualElement>("item-validate");
			itemValidateVE.style.display = DisplayStyle.None;
		}
		else
		{
			stepVE.Q<Label>("step-type").text = step.Type + ":";
			if (step.Item != null)
				stepVE.Q<Label>("item-to-validate").text = step.Item.ToString();
		}

		actorConversationsVE.Add(stepVE);
	}

	private void LoadAndInitStartDialogueLineUXML(DialogueDataSO startDialogue, VisualElement parent)
	{
		// Import UXML
		var dialogueVisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Quests/Editor/DialogueLine.uxml");

		// Set line
		foreach (LocalizedString line in startDialogue.DialogueLines)
		{
			VisualElement dialogueVE = dialogueVisualTree.CloneTree();

			Label leftLineLabel = dialogueVE.Q<Label>("left-line-label");
			leftLineLabel.text = line.GetLocalizedStringImmediateSafe();

			Label rightLineLabel = dialogueVE.Q<Label>("right-line-label");
			rightLineLabel.style.display = DisplayStyle.None;

			// Set options
			VisualElement buttonArea = dialogueVE.Q<VisualElement>("buttons");
			if (startDialogue.Choices.Count == 0)
			{
				buttonArea.style.display = DisplayStyle.None;
			}
			else if (startDialogue.Choices.Count <= 2)
			{
				for (int i = 0; i < 2; i++)
				{
					Button btn = buttonArea.Q<Button>($"btn-{i}");
					if (i < startDialogue.Choices.Count)
						btn.text = startDialogue.Choices[i].Response.GetLocalizedStringImmediateSafe();
					else
						btn.style.display = DisplayStyle.None;
				}
			}
			parent.Add(dialogueVE);
		}
	}

	private void LoadAndInitOptionsDialogueLineUXML(DialogueDataSO completeDialogue, DialogueDataSO incompleteDialogue, VisualElement parent)
	{
		// Import UXML
		var dialogueVisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Quests/Editor/DialogueLine.uxml");
		VisualElement dialogueVE = dialogueVisualTree.CloneTree();

		// Set line
		Label leftLineLabel = dialogueVE.Q<Label>("left-line-label");
		Label rightLineLabel = dialogueVE.Q<Label>("right-line-label");

		leftLineLabel.text = completeDialogue.DialogueLines[0].GetLocalizedStringImmediateSafe();
		if (incompleteDialogue != null)
			rightLineLabel.text = incompleteDialogue.DialogueLines[0].GetLocalizedStringImmediateSafe();

		// hide options
		VisualElement buttonArea = dialogueVE.Q<VisualElement>("buttons");
		buttonArea.style.display = DisplayStyle.None;

		parent.Add(dialogueVE);
	}

	private void FindAllSOsInTargetFolder<T>(Object target, out T[] foundSOs) where T : ScriptableObject
	{
		var guids = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] { Path.GetDirectoryName(AssetDatabase.GetAssetPath(target)) });

		foundSOs = new T[guids.Length];
		for (int i = 0; i < guids.Length; i++)
		{
			var path = AssetDatabase.GUIDToAssetPath(guids[i]);
			foundSOs[i] = AssetDatabase.LoadAssetAtPath<T>(path);
		}
	}

	private void FindAllSOByType<T>(out T[] foundSOs) where T : ScriptableObject
	{
		var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

		foundSOs = new T[guids.Length];
		for (int i = 0; i < guids.Length; i++)
		{
			var path = AssetDatabase.GUIDToAssetPath(guids[i]);
			foundSOs[i] = AssetDatabase.LoadAssetAtPath<T>(path);
		}
	}

	private void InitListView<T>(ListView listview, T[] itemsSource)
	{
		listview.makeItem = () => new Label();
		listview.bindItem = (element, i) =>
		{
			var nameProperty = itemsSource[i].GetType().GetProperty("name");
			if (nameProperty != null)
			{
				(element as Label).text = nameProperty.GetValue(itemsSource[i]) as string;
			}
		};
		listview.itemsSource = itemsSource;
		listview.itemHeight = 16;
		listview.selectionType = SelectionType.Single;
		listview.style.flexGrow = 1.0f;

		listview.Refresh();
		if (itemsSource.Length > 0)
			listview.selectedIndex = 0;
	}

	private void RefreshListView<T>(out ListView listview, string visualElementName, T[] itemsSource)
	{
		listview = new ListView();
		VisualElement parentVE = rootVisualElement.Q<VisualElement>(visualElementName);
		parentVE.Clear();
		InitListView(listview, itemsSource);
		parentVE.Add(listview);


	}

	private void LoadActorImage(string actorName)
	{
		Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath($"Assets/Scripts/Quests/Editor/ActorImages/{actorName}.png", typeof(Texture2D));
		actorPreview.image = texture;
	}

	private void DisplayAllProperties(Object data, string visualElementName)
	{
		//Clear panel
		VisualElement parentVE = rootVisualElement.Q<VisualElement>(visualElementName);
		parentVE.Clear();

		//Add new scrollview
		ScrollView scrollView = new ScrollView();

		SerializedObject dataObject = new SerializedObject(data);
		SerializedProperty dataProperty = dataObject.GetIterator();
		dataProperty.Next(true);

		while (dataProperty.NextVisible(false))
		{
			PropertyField prop = new PropertyField(dataProperty);
			prop.SetEnabled(dataProperty.name != "m_Script");
			prop.Bind(dataObject);
			scrollView.Add(prop);
		}
		parentVE.Add(scrollView);
	}
	void AddQuestline()
	{
		//get questline id
		FindAllSOByType(out QuestlineSO[] questLineSOs);
		int id = questLineSOs.Length;
		id++;
		QuestlineSO asset = ScriptableObject.CreateInstance<QuestlineSO>();
		asset.SetQuestlineId(id);
		AssetDatabase.CreateFolder("Assets/ScriptableObjects/Quests", "Questline" + id);
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Quests/Questline" + id + "/QL" + id + ".asset");
		AssetDatabase.SaveAssets();
		//refresh
		LoadAllQuestsData();
	}
	void RemoveQuestline()
	{


	}
	void AddQuest()
	{
		QuestSO asset = ScriptableObject.CreateInstance<QuestSO>();
		int questlineId = 0;
		questlineId = selectedQuestLine.IdQuestline;
		int questId = 0;
		questId = selectedQuestLine.Quests.Count + 1;
		AssetDatabase.CreateFolder("Assets/ScriptableObjects/Quests/Questline" + questlineId, "Quest" + questId);
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId + "/Q" + questId + "-QL" + questlineId + ".asset");
		AssetDatabase.SaveAssets();
		asset.SetQuestId(questId);
		selectedQuestLine.Quests.Add(asset);
		//refresh
		rootVisualElement.Q<VisualElement>("questlines-list").Q<ListView>().SetSelection(idQuestlineSelected);

	}
	void RemoveQuest()
	{


	}
	void AddStep()
	{
		StepSO asset = ScriptableObject.CreateInstance<StepSO>();
		int questlineId = 0;
		questlineId = selectedQuestLine.IdQuestline;
		int questId = 0;
		questId = currentSeletedQuest.IdQuest;
		int stepId = 0;
		stepId = currentSeletedQuest.Steps.Count + 1;
		AssetDatabase.CreateFolder("Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId, "Step" + stepId);
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId + "/Step" + stepId + "/S" + stepId + "-Q" + questId + "-QL" + questlineId + ".asset");
		AssetDatabase.SaveAssets();
		currentSeletedQuest.Steps.Add(asset);
		//refresh
		rootVisualElement.Q<VisualElement>("quests-list").Q<ListView>().SetSelection(idQuestSelected);


	}
	void RemoveStep()
	{


	}
}
