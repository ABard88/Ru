using UnityEngine;
using System.Collections;

public class ApplyForceinRandomDirection : MonoBehaviour 
{
	public ForceMode forceMode;
	public float forceAmount=35.0f;
	public float torque=50.0f;
	public string buttonName = "Fire1";
	public Rigidbody rb; // rigid body comment
 
	
	// Update is called once per frame
	void Update () 
	{
	  if (Input.GetButtonDown (buttonName)) 
		{
			rb.GetComponent<Rigidbody>();
			rb.AddForce(Random.onUnitSphere*forceAmount,forceMode);
			rb.AddTorque(Random.onUnitSphere*torque,forceMode);
		}
	}
}
