using UnityEngine;
using System.Collections;

public class FlightControl : MonoBehaviour {

	public Rigidbody _rigidBody;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var yaw = Input.GetAxis ("Horizontal");
		var pitch = Input.GetAxis ("Vertical");
		_rigidBody.AddRelativeTorque (new Vector3 (pitch*3, yaw*3, 0), ForceMode.Force);
	}
}
