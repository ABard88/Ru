using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour 
{
	public GameObject dice;
	public int roll;
	public GameObject pawn;
	public bool move=true;

	
	// Update is called once per frame
	void Update () 
	{

		roll = dice.GetComponent<DisplayCurrentDieValue>().currentValue;
		Vector3 pos = pawn.transform.position;
		if (roll == 2 && move)
		{
			pawn.transform.localPosition=new Vector3(pos.x,pos.y,pos.z-4.0f);
			move=false;
		}
	}
}
