using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BhvRotateSelf : MonoBehaviour
{
	public const float RotateSpeed = 60f;

	void Update()
	{
		this.transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
	}
}