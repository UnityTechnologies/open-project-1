using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.Localization;

public class QuestEditorWindow : EditorWindow
{
	private Image actorPreview;
	private QuestSO currentSeletedQuest;

    [MenuItem("ChopChop/QuestEditorWindow")]
    public static void ShowWindow()
    {
        QuestEditorWindow wnd = GetWindow<QuestEditorWindow>();
        wnd.titleContent = new GUIContent("QuestEditorWindow");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Quests/Editor/QuestEditorWindow.uxml");
        root.Add(visualTree.CloneTree());

        //Add Image
        VisualElement preview = root.Q<VisualElement>("actor-preview");
        actorPreview = new Image();
        preview.Add(actorPreview);

        //Import USS
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Quests/Editor/QuestEditorWindow.uss");
        root.styleSheets.Add(styleSheet);

        //Register button event
        Button refreshQuestPreviewBtn = root.Q<Button>("refresh-preview-btn");
        refreshQuestPreviewBtn.RegisterCallback<ClickEvent>((evt) => SetUpQuestPreview(currentSeletedQuest));

        LoadAllQuestsData();
    }

    private void LoadAllQuestsData()
    {
	    //Load all questlines
	    FindAllSOByType(out QuestlineSO[] questLineSOs);
	    RefreshListView(out ListView allQuestlinesListView, "questlines-list", questLineSOs);

	    allQuestlinesListView.onSelectionChanged += (questlineEnumerable) =>
	    {
		    QuestlineSO questLine = GetDataFromListViewItem<QuestlineSO>(questlineEnumerable);
		    RefreshListView(out ListView allQuestsListView, "quests-list", questLine.Quests.ToArray());

		    allQuestsListView.onSelectionChanged += (questEnumerable) =>
		    {
			    currentSeletedQuest = GetDataFromListViewItem<QuestSO>(questEnumerable);
			    RefreshListView(out ListView allStepsListView, "steps-list",currentSeletedQuest.Steps.ToArray() );

			    SetUpQuestPreview(currentSeletedQuest);

			    allStepsListView.onSelectionChanged += (stepEnumerable) =>
			    {
				    StepSO step = GetDataFromListViewItem<StepSO>(stepEnumerable);
				    DisplayAllProperties(step, "step-info-scroll");

				    //Find all DialogueDataSOs in the same folder of the StepSO
				    FindAllSOsInTargetFolder(step, out DialogueDataSO[] dialogueDataSOs);
				    RefreshListView(out ListView dialoguesListView, "dialogues-list", dialogueDataSOs);

				    dialoguesListView.onSelectionChanged += (dialogueEnumerable) =>
				    {
					    DialogueDataSO dialogueData = GetDataFromListViewItem<DialogueDataSO>(dialogueEnumerable);
					    DisplayAllProperties(dialogueData, "dialogue-info-scroll");
				    };
			    };
		    };
	    };
    }

    private T GetDataFromListViewItem<T>(List<object> enumberable) where T : ScriptableObject
    {
	    T data = default;
	    foreach (var item in enumberable)
	    {
		    data = item as T;
	    }
	    return data;
    }

    private void SetUpQuestPreview(QuestSO quest)
    {
	    if (quest == null)
		    return;

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
		LoadAndInitStartDialogueLineUXML(step.DialogueBeforeStep, dialogueAreaVE);

		//CD ID if any
		if (step.CompleteDialogue != null)
			LoadAndInitOptionsDialogueLineUXML(step.CompleteDialogue, step.IncompleteDialogue, dialogueAreaVE);

		//Type (Check Item etc)
		if (step.Type == stepType.dialogue)
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
		    if (startDialogue.Choices.Count==0)
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
	    var guids = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] {Path.GetDirectoryName(AssetDatabase.GetAssetPath(target))});

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
	    ScrollView scrollView= new ScrollView();

	    SerializedObject dataObject = new SerializedObject(data);
	    SerializedProperty dataProperty = dataObject.GetIterator();
	    dataProperty.Next(true);

	    while (dataProperty.NextVisible(false))
	    {
		    PropertyField prop=new PropertyField(dataProperty);
		    prop.SetEnabled(dataProperty.name != "m_Script");
		    prop.Bind(dataObject);
		    scrollView.Add(prop);
	    }
	    parentVE.Add(scrollView);
    }
}
