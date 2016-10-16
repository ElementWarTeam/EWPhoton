using UnityEngine;
using System.Collections;

public class ScollUV : MonoBehaviour
{

	float parralax = 5f;

	void Update ()
	{
		MeshRenderer mr = GetComponent<MeshRenderer> ();

		Material mat = mr.material;

		Vector2 offset = mat.mainTextureOffset;
		Vector2 position = this.transform.position;

		offset.x = position.x / transform.localScale.x * parralax;
		offset.y = position.y / transform.localScale.y * parralax;

		mat.mainTextureOffset = this.transform.position;
	}
}
