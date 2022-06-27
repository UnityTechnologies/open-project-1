using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTypeDetector : MonoBehaviour
{
	[SerializeField] private GroundTypeListSO _groundTypeList = default;


	[Space]
	[Header("Debugger")]
	[SerializeField] private bool _debugMode = false;
	[SerializeField] private string _groundTypeTitle = default;

	Color vertexColorNearest;
	GroundTypeSO _groundType_Debugger = default;
	Mesh _mesh;
	MeshCollider _meshCollider;
	Vector3 p0;
	Vector3 p1;
	Vector3 p2;
	Transform hitTransform;
	GroundTypeSO result;



	void Start()
    {
        
    }


    void FixedUpdate()
    {

#if UNITY_EDITOR
		DebugGroundType();
#endif

	}

	private void DebugGroundType()
	{
		_groundType_Debugger = GetGroundType();
		if (_groundType_Debugger != null)
		{
			_groundTypeTitle = _groundType_Debugger.Title;
		}
		else
		{
			_groundTypeTitle = "Not Defined";
		}
	}

	public GroundTypeSO GetGroundType()
	{
		if (_groundTypeList == null)
		{
			Debug.LogError("Please Define Ground Types in GroundTypeList ScriptableObject and refrence it to this script component");
			return null;
		}

		result = null;
		_meshCollider = null;
		_mesh = null;
		RaycastHit hit;

		void FindNearestVertexColor()
		{
			p0 = _mesh.vertices[_mesh.triangles[hit.triangleIndex * 3 + 0]];
			p1 = _mesh.vertices[_mesh.triangles[hit.triangleIndex * 3 + 1]];
			p2 = _mesh.vertices[_mesh.triangles[hit.triangleIndex * 3 + 2]];

			hitTransform = hit.collider.transform;

			p0 = hitTransform.TransformPoint(p0);
			p1 = hitTransform.TransformPoint(p1);
			p2 = hitTransform.TransformPoint(p2);

			float distance_0 = Vector3.Distance(p0, transform.position);
			float distance_1 = Vector3.Distance(p1, transform.position);
			float distance_2 = Vector3.Distance(p2, transform.position);

			if (distance_0 <= distance_1 &&
				distance_0 <= distance_2)
			{
				vertexColorNearest = _mesh.colors[_mesh.triangles[hit.triangleIndex * 3 + 0]];
			}
			else if (distance_1 <= distance_0 &&
					distance_1 <= distance_2)
			{
				vertexColorNearest = _mesh.colors[_mesh.triangles[hit.triangleIndex * 3 + 1]];
			}
			else
			{
				vertexColorNearest = _mesh.colors[_mesh.triangles[hit.triangleIndex * 3 + 2]];
			}

			if (_debugMode)
			{
				Debug.Log("Nearest Vertex Color: " + vertexColorNearest.ToString());
			}
		}

		void FindGroundTypeBasedOn_VertexColor()
		{
			float r_Diffrence;
			float g_Diffrence;
			float b_Diffrence;
			float totalDiffrence;

			float minDiffrence = float.MaxValue;
			for (int i = 0; i < _groundTypeList.groundTypes.Length; i++)
			{
				if (_groundTypeList.groundTypes[i].hasGroundTag)
				{
					r_Diffrence = Mathf.Abs(vertexColorNearest.r - _groundTypeList.groundTypes[i].vertexColorRGB.x);
					g_Diffrence = Mathf.Abs(vertexColorNearest.g - _groundTypeList.groundTypes[i].vertexColorRGB.y);
					b_Diffrence = Mathf.Abs(vertexColorNearest.b - _groundTypeList.groundTypes[i].vertexColorRGB.z);
					totalDiffrence = r_Diffrence + g_Diffrence + b_Diffrence;

					if (totalDiffrence <= minDiffrence)
					{
						minDiffrence = totalDiffrence;
						result = _groundTypeList.groundTypes[i];
					}
				}
			}
		}

		void FindGroundTypeBasedOn_GameObjectTag()
		{
			for (int i = 0; i < _groundTypeList.groundTypes.Length; i++)
			{
				if (_groundTypeList.groundTypes[i].hasGroundTag == false &&
					hit.transform.CompareTag(_groundTypeList.groundTypes[i].gameObjectTag))
				{
					result = _groundTypeList.groundTypes[i];
					break;
				}
			}
		}

		
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 50f))
		{
			if (hit.transform.CompareTag("Ground"))
			{
				_meshCollider = hit.collider as MeshCollider;

				if (_meshCollider != null && _meshCollider.sharedMesh != null)
				{
					_mesh = _meshCollider.sharedMesh;

					if (_mesh.colors.Length > 0)
					{
						FindNearestVertexColor();
						FindGroundTypeBasedOn_VertexColor();
					}
				}
			}
			else
			{
				FindGroundTypeBasedOn_GameObjectTag();
			}

		}

		if (result == null)
		{
			Debug.LogError("This type of ground is not defined in GroundTypeList ScriptableObject");
		}

		return result;
	}



}
