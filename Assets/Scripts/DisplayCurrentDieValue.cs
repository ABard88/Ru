using UnityEngine;
using System.Collections;

public class DisplayCurrentDieValue : MonoBehaviour 
{
	public LayerMask dieValueColliderLayer=-1;
	public int currentValue=1;
	public int rollVal=2;
	public bool rollComplete=false;
	Rigidbody rb;
	
	void Update () 
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.up, out hit, Mathf.Infinity,dieValueColliderLayer))
		{
			if(rollComplete==true)
			{
				currentValue=hit.collider.GetComponent<DieValue>().value;
				rollVal=currentValue;
			}
		}
		rb = GetComponent<ApplyForceinRandomDirection> ().rb;
		if (rb.IsSleeping ()) {
			rollComplete = true;
		} else
			rollComplete = false;

	}

	void OnGUI()
	{
		GUILayout.Label (currentValue.ToString());
	}
}
