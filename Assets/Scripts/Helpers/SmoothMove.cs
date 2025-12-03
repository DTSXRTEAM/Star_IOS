using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMove : MonoBehaviour
{
	[SerializeField] private float smoothTime = 0.3f;
	[SerializeField] private float deadZone = 0.1f; // distance offset before smooth moving
	[SerializeField] private float maxSmoothDist = 0.3f; // if distance is highter, just move

	public Vector3 targetPosition = Vector3.zero;
	private Vector3 velocity = Vector3.zero;

	public void Start()
	{
		Debug.Log("Start Smooth move" + targetPosition.ToString("F3"));
	}

	public void SetTarget(Vector3 position, Quaternion rotation)
	{
		Debug.Log("Current AR-origin pos: " + transform.position.ToString("F3"));

		Debug.Log("New target pos: " + position.ToString("F3"));
		transform.rotation = rotation;

		float dist = Vector3.Distance(position, transform.position);
		Debug.Log("Distance: " + dist.ToString("F3"));

		if (dist < deadZone)
		{
			Debug.Log("Within deadzone");
			return;
		}
		else if (dist > maxSmoothDist)
		{
			transform.position = position;
			Debug.Log("Over max, Repositioning");
		}
		targetPosition = position;
	}

	void Update()
	{
		if (targetPosition == Vector3.zero)
			return;

		// Smoothly move the camera towards that target position
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
	}
}
