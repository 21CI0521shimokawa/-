using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlingBuckGround : MonoBehaviour
{
	[SerializeField]
	private float Speed;
	[SerializeField]
	private float LimitPosition;

	void Update()
	{
		transform.Translate(Speed*Time.deltaTime, 0, 0);
		if (transform.position.x < LimitPosition)
		{
			transform.position = new Vector3(44f, 0.19f, 0);
		}
	}
}
