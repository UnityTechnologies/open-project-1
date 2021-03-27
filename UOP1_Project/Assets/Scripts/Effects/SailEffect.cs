using UnityEngine;

/// <summary>
/// Affects the strength of the sail effect
/// </summary>
class SailEffect : MonoBehaviour
{
	[SerializeField, Range(0,1)] private float strength;
	[SerializeField] private MeshRenderer sailRenderer;

	[SerializeField, MinMax] private Vector2 windSpeed, windDensity, windStrength, sailBulgeStrength;

	private readonly int propIdWindSpeed = Shader.PropertyToID("_Wind_Speed"),
						propIdWindDensity = Shader.PropertyToID("_Wind_Density"),
						propIdWindStrength = Shader.PropertyToID("_Wind_Strength"),
						propIdSailBulgeStrength = Shader.PropertyToID("_SailBulgeStrength");

	/// <summary>
	/// Overal strength modifier of the sail effect
	/// </summary>
	public float Strength
	{
		get => strength;
		set
		{
			strength = value;
			applySettingsToMatPropBlock();
		}
	}

	void Awake() => applySettingsToMatPropBlock();
	void OnValidate() => applySettingsToMatPropBlock();
	void OnDestroy() //make sure to clear the propblock again!
	{
		if(sailRenderer == null)
			sailRenderer.SetPropertyBlock(null);
	}
	//Automatically set the min and max to the material setting, so we dont overwrite anything
	void Reset()
	{
		if (sailRenderer == null)
			sailRenderer = GetComponent<MeshRenderer>();

		if (sailRenderer == null || sailRenderer.sharedMaterial == null)
			return;
		var mat = sailRenderer.sharedMaterial;

		windSpeed = new Vector2(mat.GetFloat(propIdWindSpeed), mat.GetFloat(propIdWindSpeed) * 2f);
		windDensity = new Vector2(mat.GetFloat(propIdWindDensity), mat.GetFloat(propIdWindDensity) * 1.5f);
		windStrength = new Vector2(mat.GetFloat(propIdWindStrength), mat.GetFloat(propIdWindStrength) * 1.5f);
		sailBulgeStrength = new Vector2(mat.GetFloat(propIdSailBulgeStrength), mat.GetFloat(propIdSailBulgeStrength) * 1.5f);

	}

	//At the moment only intended for editor, and default-settings
	[ContextMenu("Apply current settings to material")]
	private void applySettingsToMaterial()
	{
		var mat = sailRenderer.sharedMaterial;
		mat.SetFloat(propIdWindSpeed, Mathf.Lerp(windSpeed.x, windSpeed.y, strength));
		mat.SetFloat(propIdWindDensity, Mathf.Lerp(windDensity.x, windDensity.y, strength));
		mat.SetFloat(propIdWindStrength, Mathf.Lerp(windStrength.x, windStrength.y, strength));
		mat.SetFloat(propIdSailBulgeStrength, Mathf.Lerp(sailBulgeStrength.x, sailBulgeStrength.y, strength));

	}
	
	private void applySettingsToMatPropBlock()
	{
		MaterialPropertyBlock matPropBlock = new MaterialPropertyBlock();
		matPropBlock.SetFloat(propIdWindSpeed, Mathf.Lerp(windSpeed.x, windSpeed.y, strength));
		matPropBlock.SetFloat(propIdWindDensity, Mathf.Lerp(windDensity.x, windDensity.y, strength));
		matPropBlock.SetFloat(propIdWindStrength, Mathf.Lerp(windStrength.x, windStrength.y, strength));
		matPropBlock.SetFloat(propIdSailBulgeStrength, Mathf.Lerp(sailBulgeStrength.x, sailBulgeStrength.y, strength));

		sailRenderer.SetPropertyBlock(matPropBlock);
	}

}

/// <summary>
/// Shows this Vector2 property as a min/max slider, where X is min and Y is max. Use Mathf.lerp(x,y, t) to get a value in between them!
/// </summary>
class MinMaxAttribute : PropertyAttribute
{
	public float MinLimit = 0, MaxLimit = 5;
}
#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(MinMaxAttribute))]
class MinMaxAttributeDrawer : UnityEditor.PropertyDrawer
{
	public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
	{
		if (property.propertyType != UnityEditor.SerializedPropertyType.Vector2)
		{
			Debug.LogError("Currently the MinMax attribute only works on a Vector2 property!");
			return;
		}
		var attr = attribute as MinMaxAttribute;

		float x = property.vector2Value.x,
			y = property.vector2Value.y;



		UnityEditor.EditorGUI.MinMaxSlider(position, label, ref x, ref y, attr.MinLimit, attr.MaxLimit);
		if (GUI.changed)
			property.vector2Value = new Vector2(x, y);
	}
}

#endif
