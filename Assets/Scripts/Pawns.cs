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
		return new bool[10,3]; // returns true or false for each square in the 10x3 grid. If true then you can move.
	}



}
