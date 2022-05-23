using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlingBuckGround : MonoBehaviour
{
	[SerializeField]
	private float Speed;
	[SerializeField]
	private float LimitPosition;
	[SerializeField]
	private Vector2 RestartPosition;

	void Update()
	{
		transform.Translate(Speed*Time.deltaTime, 0, 0);
		if (transform.position.x> LimitPosition)
		{
			transform.position = RestartPosition;
		}
	}
}
