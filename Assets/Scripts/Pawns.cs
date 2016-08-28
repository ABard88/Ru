using UnityEngine;
using System.Collections;

public class Pawns : MonoBehaviour 
{
	public int CurrentX{ set; get;}
	public int CurrentY{ set; get;}
	public bool isWhite;

	public void SetPosition(int x, int y)
	{
		CurrentX = x;
		CurrentY = y;
	}

	public virtual 	bool[,] PossibleMove()
	{
		return new bool[10,3];
	}



}
