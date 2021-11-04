using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public enum SelectionType
{
	Questline,
	Quest,
	Step,
	Dialogue,
}

public class QuestEditorWindow : EditorWindow
{
	private StepSO _currentSelectedStep = default;
	private QuestSO _currentSeletedQuest = default;
	private QuestlineSO _currentSelectedQuestLine = default;
	private int _idQuestlineSelected = default;
	private int _idQuestSelected = default;
	private int _idStepSelected = default;

	//Note: Hidden from the tools because it's not fully functional at the moment
	//[MenuItem("ChopChop/Quest Editor")]
	public static void ShowWindow()
	{
		QuestEditorWindow wnd = GetWindow<QuestEditorWindow>();
		wnd.titleContent = new GUIContent("Quest Editor");

		// Sets a minimum size to the window.
		wnd.minSize = new Vector2(250, 250);
	}
	public static void ShowArtistToolWindow()
	{
		// Opens the window, otherwise focuses it if it’s already open.
		QuestEditorWindow window = GetWindow<QuestEditorWindow>();
		// Adds a title to the window.

		window.titleContent = new GUIContent("Quest Editor");

		// Sets a minimum size to the window.
		window.minSize = new Vector2(250, 250);

		//window.SetTool(); 
	}

	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;

		// Import UXML
		var visualTree = Resources.Load<VisualTreeAsset>("QuestEditorWindow");
		root.Add(visualTree.CloneTree());

		//Import USS
		var styleSheet = Resources.Load<StyleSheet>("QuestEditorWindow");
		root.styleSheets.Add(styleSheet);

		//Register button event
		Button refreshQuestPreviewBtn = root.Q<Button>("refresh-preview-btn");
		refreshQuestPreviewBtn.RegisterCallback<ClickEvent>((evt) => SetUpQuestPreview(_currentSeletedQuest));

		Button createQuestlineButton = rootVisualElement.Q<Button>("create-QL-btn");
		createQuestlineButton.clicked += AddQuestline;
		createQuestlineButton.SetEnabled(true);

		Button createQuestButton = rootVisualElement.Q<Button>("create-Q-btn");
		createQuestButton.clicked += AddQuest;
		createQuestButton.SetEnabled(false);

		Button createStepButton = rootVisualElement.Q<Button>("create-S-btn");
		createStepButton.clicked += AddStep;
		createStepButton.SetEnabled(false);

		Button removeQuestlineButton = rootVisualElement.Q<Button>("remove-QL-btn");
		removeQuestlineButton.clicked += RemoveQuestline;
		removeQuestlineButton.SetEnabled(true);

		Button removeQuestButton = rootVisualElement.Q<Button>("remove-Q-btn");
		removeQuestButton.clicked += RemoveQuest;
		removeQuestButton.SetEnabled(false);


		Button removeStepButton = rootVisualElement.Q<Button>("remove-S-btn");
		removeStepButton.clicked += RemoveStep;
		removeStepButton.SetEnabled(false);

		LoadAllQuestsData();


	}

	private void ClearElements(SelectionType type)
	{
		List<string> listElements = new List<string>();
		listElements.Clear();
		Button createQuestButton = rootVisualElement.Q<Button>("create-Q-btn");
		Button createStepButton = rootVisualElement.Q<Button>("create-S-btn");
		Button removeQuestButton = rootVisualElement.Q<Button>("remove-Q-btn");
		Button removeStepButton = rootVisualElement.Q<Button>("remove-S-btn");

		switch (type)
		{
			case SelectionType.Questline:
				listElements.Add("quests-list");
				listElements.Add("steps-list");
				listElements.Add("actor-conversations");
				listElements.Add("steps-list");
				listElements.Add("dialogues-list");

				listElements.Add("step-info-scroll");
				listElements.Add("dialogue-info-scroll");
				if (createQuestButton != null)
				{
					createQuestButton.SetEnabled(true);
					removeQuestButton.SetEnabled(true);
				}
				if (createStepButton != null)
				{
					createStepButton.SetEnabled(false);
					removeStepButton.SetEnabled(false);
				}

				break;
			case SelectionType.Quest:
				listElements.Add("dialogues-list");

				listElements.Add("step-info-scroll");
				listElements.Add("dialogue-info-scroll");

				if (createStepButton != null)
				{
					createStepButton.SetEnabled(true);
					removeStepButton.SetEnabled(true);
				}

				break;
			case SelectionType.Step:

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
		//Load all questlines
		FindAllSOByType(out QuestlineSO[] questLineSOs);
		RefreshListView(out ListView allQuestlinesListView, "questlines-list", questLineSOs);
		ClearElements(SelectionType.Questline);
		allQuestlinesListView.onSelectionChange += (questlineEnumerable) =>
		{
			_currentSelectedQuestLine = GetDataFromListViewItem<QuestlineSO>(questlineEnumerable);
			ClearElements(SelectionType.Questline);
			_idQuestlineSelected = allQuestlinesListView.selectedIndex;

			if (_currentSelectedQuestLine.Quests != null)
			{
				RefreshListView(out ListView allQuestsListView, "quests-list", _currentSelectedQuestLine.Quests.ToArray());

				allQuestsListView.onSelectionChange += (questEnumerable) =>
				{
					_idQuestSelected = allQuestsListView.selectedIndex;

					_currentSeletedQuest = GetDataFromListViewItem<QuestSO>(questEnumerable);
					ClearElements(SelectionType.Quest);
					if (_currentSeletedQuest != null && _currentSeletedQuest.Steps != null)
					{
						RefreshListView(out ListView allStepsListView, "steps-list", _currentSeletedQuest.Steps.ToArray());

						SetUpQuestPreview(_currentSeletedQuest);

						allStepsListView.onSelectionChange += (stepEnumerable) =>
						{
							_idStepSelected = allStepsListView.selectedIndex;
							_currentSelectedStep = GetDataFromListViewItem<StepSO>(stepEnumerable);
							DisplayAllProperties(_currentSelectedStep, "step-info-scroll");
							ClearElements(SelectionType.Step);
							//Find all DialogueDataSOs in the same folder of the StepSO
							FindAllDialogueInStep(_currentSelectedStep, out DialogueDataSO[] dialogueDataSOs);
							if (dialogueDataSOs != null)
							{

								RefreshListView(out ListView dialoguesListView, "dialogues-list", dialogueDataSOs);

								dialoguesListView.onSelectionChange += (dialogueEnumerable) =>
								{


									DialogueDataSO dialogueData = GetDataFromListViewItem<DialogueDataSO>(dialogueEnumerable);
									DisplayAllProperties(dialogueData, "dialogue-info-scroll");
								};
							}
							VisualElement DialogueList = rootVisualElement.Q<VisualElement>("dialogues-list");
							SetAddDialogueButtonsForStep(out VisualElement ButtonsPanel);
							//DialogueList.Q<VisualElement>("buttons-panel").Clear();
							DialogueList.Add(ButtonsPanel);

						};

					}
				};
			}
		};
	}

	private void OnDisable()
	{
		AssetDatabase.SaveAssets();
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

		if (AllDialogueList != null)
			AllDialogue = AllDialogueList.ToArray();
	}

	private void SetAddDialogueButtonsForStep(out VisualElement ButtonsPanel)
	{
		ButtonsPanel = new VisualElement();
		if (_currentSelectedStep != null)
		{
			if (_currentSelectedStep.DialogueBeforeStep == null)
			{
				Button AddDialogueBeforeStepButton = new Button();
				AddDialogueBeforeStepButton.text = "Add Dialogue Before Step";
				AddDialogueBeforeStepButton.clickable.clicked += AddDialogueBeforeStep;
				ButtonsPanel.Add(AddDialogueBeforeStepButton);
			}
			if (_currentSelectedStep.CompleteDialogue == null)
			{
				Button AddCompletionDialogueButton = new Button();
				AddCompletionDialogueButton.text = "Add Completion Dialogue";
				AddCompletionDialogueButton.clickable.clicked += AddCompletionDialogue;
				ButtonsPanel.Add(AddCompletionDialogueButton);
			}
			if (_currentSelectedStep.IncompleteDialogue == null)
			{

				Button AddIncompletionDialogueButton = new Button();
				AddIncompletionDialogueButton.text = "Add Incompletion Dialogue";
				AddIncompletionDialogueButton.clickable.clicked += AddIncompletionDialogue;
				ButtonsPanel.Add(AddIncompletionDialogueButton);
			}
		}
	}

	private void AddDialogueBeforeStep()
	{
		DialogueDataSO asset = ScriptableObject.CreateInstance<DialogueDataSO>();
		int questlineId = 0;
		questlineId = _currentSelectedQuestLine.IdQuestline;
		int questId = 0;
		questId = _currentSeletedQuest.IdQuest;
		int stepId = 0;
		stepId = _currentSeletedQuest.Steps.FindIndex(o => o == _currentSelectedStep) + 1;
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId + "/Step" + stepId + "/SD-S" + stepId + "-Q" + questId + "-QL" + questlineId + ".asset");

		EditorUtility.SetDirty(asset);
		EditorUtility.SetDirty(_currentSeletedQuest);
		AssetDatabase.SaveAssets();
		_currentSelectedStep.DialogueBeforeStep = asset;
		asset.DialogueType = DialogueType.StartDialogue;
		//	asset.CreateLine(); 
		rootVisualElement.Q<VisualElement>("steps-list").Q<ListView>().SetSelection(_idStepSelected);
	}

	private void AddCompletionDialogue()
	{
		DialogueDataSO asset = ScriptableObject.CreateInstance<DialogueDataSO>();
		int questlineId = 0;
		questlineId = _currentSelectedQuestLine.IdQuestline;
		int questId = 0;
		questId = _currentSeletedQuest.IdQuest;
		int stepId = 0;
		stepId = _currentSeletedQuest.Steps.FindIndex(o => o == _currentSelectedStep) + 1;

		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId + "/Step" + stepId + "/CD-S" + stepId + "-Q" + questId + "-QL" + questlineId + ".asset");
		_currentSelectedStep.CompleteDialogue = asset;
		asset.DialogueType = DialogueType.CompletionDialogue;
		//	asset.CreateLine();
		EditorUtility.SetDirty(asset);
		EditorUtility.SetDirty(_currentSeletedQuest);
		AssetDatabase.SaveAssets();

		rootVisualElement.Q<VisualElement>("steps-list").Q<ListView>().SetSelection(_idStepSelected);
	}

	private void AddIncompletionDialogue()
	{
		DialogueDataSO asset = ScriptableObject.CreateInstance<DialogueDataSO>();
		int questlineId = 0;
		questlineId = _currentSelectedQuestLine.IdQuestline;
		int questId = 0;
		questId = _currentSeletedQuest.IdQuest;
		int stepId = 0;
		stepId = _currentSeletedQuest.Steps.FindIndex(o => o == _currentSelectedStep) + 1;
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId + "/Step" + stepId + "/ID-S" + stepId + "-Q" + questId + "-QL" + questlineId + ".asset");

		asset.DialogueType = DialogueType.IncompletionDialogue;
		//asset.CreateLine();
		_currentSelectedStep.IncompleteDialogue = asset;
		EditorUtility.SetDirty(asset);
		EditorUtility.SetDirty(_currentSeletedQuest);
		AssetDatabase.SaveAssets();

		rootVisualElement.Q<VisualElement>("steps-list").Q<ListView>().SetSelection(_idStepSelected);
	}

	private void SetUpQuestPreview(QuestSO quest)
	{
		if (quest == null)
			return;
		if (quest.Steps == null)
			return;

		rootVisualElement.Q<VisualElement>("actor-conversations").Clear();

		foreach (StepSO step in quest.Steps)
			LoadAndInitStepUXML(step);
	}

	private void LoadAndInitStepUXML(StepSO step)
	{
		//Clear actor conversations area
		ScrollView actorConversationsVE = rootVisualElement.Q<ScrollView>("actor-conversations");
		// Import UXML
		var stepVisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Quests/Editor/StepDetail.uxml");
		VisualElement stepVE = stepVisualTree.CloneTree();
		VisualElement dialogueAreaVE = stepVE.Q<VisualElement>("dialogue-area");

		//Title
		if (step != null)
			stepVE.Q<Label>("step-title-label").text = "Step" + step.name[1];

		//IsDone
		Toggle isDoneToggle = stepVE.Q<Toggle>("step-done-toggle");
		isDoneToggle.value = step.IsDone;
		isDoneToggle.SetEnabled(false);

		DialogueDataSO dialogueToPreview = default;
		dialogueToPreview = step.StepToDialogue();
		if (dialogueToPreview != null)
		{
			//setPreview Actor for each step
			if (step.Actor != null)
			{
				Image actorPreview = LoadActorImage(step.Actor.name);
				//Add Image
				VisualElement preview = stepVE.Q<VisualElement>("actor-preview");
				preview.Add(actorPreview);
			}
			VisualElement VE = CreateDialoguePreviewWithBranching(dialogueToPreview);
			dialogueAreaVE.Add(VE);
		}

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

	}

	private void LoadAndInitOptionsDialogueLineUXML(DialogueDataSO completeDialogue, DialogueDataSO incompleteDialogue, VisualElement parent)
	{
		// Import UXML
		var dialogueVisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Quests/Editor/DialogueLine.uxml");
		VisualElement dialogueVE = dialogueVisualTree.CloneTree();

		// Set line
		Label leftLineLabel = dialogueVE.Q<Label>("left-line-label");
		Label rightLineLabel = dialogueVE.Q<Label>("right-line-label");


		//	leftLineLabel.text = completeDialogue.DialogueLines[0].GetLocalizedStringImmediateSafe();
		//	if (incompleteDialogue != null)
		//rightLineLabel.text = incompleteDialogue.DialogueLines[0].GetLocalizedStringImmediateSafe();

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
				if (itemsSource[i] != null)
					(element as Label).text = nameProperty.GetValue(itemsSource[i]) as string;
			}
		};
		listview.itemsSource = itemsSource;
		listview.itemHeight = 16;
		listview.selectionType = UnityEngine.UIElements.SelectionType.Single;
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

	private Image LoadActorImage(string actorName)
	{
		Image actorPreview = new Image();
		Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath($"Assets/Scripts/Quests/Editor/ActorImages/{actorName}.png", typeof(Texture2D));
		actorPreview.image = texture;
		return actorPreview;
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
		if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Quests/Questline" + id))
			AssetDatabase.CreateFolder("Assets/ScriptableObjects/Quests", "Questline" + id);
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Quests/Questline" + id + "/QL" + id + ".asset");
		EditorUtility.SetDirty(asset);
		AssetDatabase.SaveAssets();
		//refresh
		LoadAllQuestsData();
	}
	void RemoveQuestline()
	{
		if (_currentSelectedQuestLine == null)
			return;

		while (_currentSelectedQuestLine.Quests.Count > 0)
		{
			QuestSO quest = _currentSelectedQuestLine.Quests[0];
			RemoveQuest(quest);
			_currentSelectedQuestLine.Quests.RemoveAt(0);
		}
		AssetDatabase.DeleteAsset(_currentSelectedQuestLine.GetPath());
		//refresh List
		LoadAllQuestsData();
	}

	void AddQuest()
	{
		QuestSO asset = CreateInstance<QuestSO>();
		int questlineId = 0;
		questlineId = _currentSelectedQuestLine.IdQuestline;
		int questId = 0;
		questId = _currentSelectedQuestLine.Quests.Count + 1;


		if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId))
			AssetDatabase.CreateFolder("Assets/ScriptableObjects/Quests/Questline" + questlineId, "Quest" + questId);
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId + "/Q" + questId + "-QL" + questlineId + ".asset");
		asset.SetQuestId(questId);
		_currentSelectedQuestLine.Quests.Add(asset);
		EditorUtility.SetDirty(asset);
		EditorUtility.SetDirty(_currentSelectedQuestLine);
		AssetDatabase.SaveAssets();
		//refresh
		rootVisualElement.Q<VisualElement>("questlines-list").Q<ListView>().SetSelection(_idQuestlineSelected);
	}

	void RemoveQuest()
	{
		if (_currentSeletedQuest == null)
			return;
		//When removing a step, remove its references in the parent quest
		if (_currentSelectedQuestLine.Quests.Exists(o => o == _currentSeletedQuest))
			_currentSelectedQuestLine.Quests.Remove(_currentSelectedQuestLine.Quests.Find(o => o == _currentSeletedQuest));
		//when removing a step remove its dialogues
		while (_currentSeletedQuest.Steps.Count > 0)
		{
			StepSO step = _currentSeletedQuest.Steps[0];
			RemoveStep(step);
			_currentSeletedQuest.Steps.RemoveAt(0);
		}

		AssetDatabase.DeleteAsset(_currentSeletedQuest.GetPath());
		_idQuestSelected = -1;
		//refresh List
		rootVisualElement.Q<VisualElement>("questlines-list").Q<ListView>().SetSelection(_idQuestlineSelected);
	}

	void RemoveQuest(QuestSO questToRemove)
	{
		if (questToRemove == null)
			return;
		//when removing a step remove its dialogues
		foreach (StepSO step in questToRemove.Steps)
		{
			//When removing a step, remove its references in the parent quest
			RemoveStep(step);
		}

		AssetDatabase.DeleteAsset(questToRemove.GetPath());
	}

	void AddStep()
	{
		StepSO asset = ScriptableObject.CreateInstance<StepSO>();
		int questlineId = 0;
		questlineId = _currentSelectedQuestLine.IdQuestline;
		int questId = 0;
		questId = _currentSeletedQuest.IdQuest;
		int stepId = 0;
		stepId = _currentSeletedQuest.Steps.Count + 1;
		if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId + "/Step" + stepId))
			AssetDatabase.CreateFolder("Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId, "Step" + stepId);
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Quests/Questline" + questlineId + "/Quest" + questId + "/Step" + stepId + "/S" + stepId + "-Q" + questId + "-QL" + questlineId + ".asset");
		_currentSeletedQuest.Steps.Add(asset);
		EditorUtility.SetDirty(asset);
		EditorUtility.SetDirty(_currentSeletedQuest);
		AssetDatabase.SaveAssets();
		//refresh
		rootVisualElement.Q<VisualElement>("quests-list").Q<ListView>().SetSelection(_idQuestSelected);
	}

	void RemoveStep()
	{
		if (_currentSelectedStep == null)
			return;
		//When removing a step, remove its references in the parent quest
		if (_currentSeletedQuest.Steps.Exists(o => o == _currentSelectedStep))
			_currentSeletedQuest.Steps.Remove(_currentSeletedQuest.Steps.Find(o => o == _currentSelectedStep));
		//when removing a step remove its dialogues
		if (_currentSelectedStep.DialogueBeforeStep != null)
		{
			RemoveDialogue(_currentSelectedStep.DialogueBeforeStep);
		}
		if (_currentSelectedStep.CompleteDialogue != null)
		{
			RemoveDialogue(_currentSelectedStep.CompleteDialogue);
		}
		if (_currentSelectedStep.IncompleteDialogue != null)
		{
			RemoveDialogue(_currentSelectedStep.IncompleteDialogue);
		}
		AssetDatabase.DeleteAsset(_currentSelectedStep.GetPath());
		_idStepSelected = -1;
		//refresh List
		rootVisualElement.Q<VisualElement>("quests-list").Q<ListView>().SetSelection(_idQuestSelected);
	}

	void RemoveStep(StepSO stepToRemove)
	{
		if ((stepToRemove == null))
			return;
		//when removing a step remove its dialogues
		if (stepToRemove.DialogueBeforeStep != null)
		{
			RemoveDialogue(stepToRemove.DialogueBeforeStep);
			stepToRemove.DialogueBeforeStep = null;
		}
		if (stepToRemove.CompleteDialogue != null)
		{
			RemoveDialogue(stepToRemove.CompleteDialogue);
			stepToRemove.CompleteDialogue = null;
		}
		if (stepToRemove.IncompleteDialogue != null)
		{
			RemoveDialogue(stepToRemove.IncompleteDialogue);
			stepToRemove.IncompleteDialogue = null;
		}
		AssetDatabase.DeleteAsset(stepToRemove.GetPath());
	}

	void RemoveDialogue(DialogueDataSO dialogueToRemove)
	{
		//	dialogueToRemove.RemoveLineFromSharedTable();
		AssetDatabase.DeleteAsset(dialogueToRemove.GetPath());
	}

	private VisualElement CreateDialoguePreviewWithBranching(DialogueDataSO dialogueDataSO)
	{
		VisualElement dialoguePreviewVE = new VisualElement();
		dialoguePreviewVE.name = "Dialogue";
		/*foreach (LocalizedString localizedString in dialogueDataSO.DialogueLines)
		{
			Label dialogueLine = new Label();
			dialogueLine.name = dialogueDataSO.DialogueType.ToString();
			dialogueLine.text = localizedString.GetLocalizedStringImmediateSafe();
			dialoguePreviewVE.Add(dialogueLine);
		}
		if (dialogueDataSO.Choices != null)
		{
			VisualElement choicesVE = new VisualElement();
			choicesVE.name = "Choices";
			for (int i = 0; i < dialogueDataSO.Choices.Count; i++)
			{
				VisualElement choiceVE = new VisualElement();
				Choice choice = dialogueDataSO.Choices[i];
				Button dialogueButton = new Button();
				dialogueButton.name = "Button" + _idQuestlineSelected;
				dialogueButton.text = choice.Response.GetLocalizedStringImmediateSafe();
				choiceVE.Add(dialogueButton);
				if (choice.NextDialogue != null)
					choiceVE.Add(CreateDialoguePreviewWithBranching(choice.NextDialogue));

				choiceVE.name = "Choice";
				choicesVE.Add(choiceVE);
			}
			dialoguePreviewVE.Add(choicesVE);
		}*/
		return dialoguePreviewVE;
	}
}