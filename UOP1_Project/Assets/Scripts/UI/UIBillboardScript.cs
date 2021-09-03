using UnityEngine;

public class UIBillboardScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		Vector3 a = Camera.main.transform.position - transform.position;

		a.x = a.z = 0.0f;

		transform.LookAt(Camera.main.transform.position - a);

		transform.rotation = (Camera.main.transform.rotation);
	}
}
