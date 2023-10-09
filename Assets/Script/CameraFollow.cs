using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	public float smoothing = 5f;
	public Vector3 offset;

	void FixedUpdate ()
	{
		Vector3 desiredPosition = target.position + offset;
		transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothing*Time.deltaTime);
		
	}

}