using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationTeleporterButton : MonoBehaviour
{
    [SerializeField] private LocationSO _location;
	[SerializeField] private PathSO _path;
	[SerializeField] private TextMeshProUGUI label;

	private EditorTeleporter _teleporter;

	private void Awake()
	{
		_teleporter = GetComponentInParent<EditorTeleporter>();
	}

	private void Start()
	{
		label.text = _location.name + " via " + _path.name;
	}

	//Called by the UI button's UnityEvent
	public void IssueTeleport()
	{
		_teleporter.Teleport(_location, _path);
	}
}
