using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour 
{
	public static BoardManager Instance{ set; get;}
	private bool[,] allowedMoves{ set; get;}


	public Pawns[,] Pawns{ set; get;}
	private Pawns selectedPawn;
	private const float TILE_SIZE=1.0f;
	private const float TILE_OFFSET=0.5f;

	private int selectionX = -1;
	private int selectionY = -1;

	public bool isWhiteTurn=true;

	public List<GameObject> pawnPrefab;
	private List<GameObject> activePawn= new List<GameObject>(); 

	private void SpawnPawn(int index, int x, int y)
	{
		GameObject go = Instantiate (pawnPrefab [index], GetTile(x,y), Quaternion.identity) as GameObject;
		go.transform.SetParent (transform);
		Pawns[x,y]=go.GetComponent<Pawns>();
		Pawns[x,y].SetPosition(x,y);
		activePawn.Add (go);
	}
	private Vector3 GetTile(int x, int y)
	{
		Vector3 origin = Vector3.zero;
		origin.x += (TILE_SIZE * x);
		origin.z += (TILE_SIZE * y);
		return origin;
	}

	private void SpawnAllPieces()
	{

		activePawn = new List<GameObject> ();
		Pawns=new Pawns[10,3];
		//Spawn White Side
		for(int i=3; i<7;i++)
		{
			SpawnPawn (0,i,0); 
		}
		//Spawn Black Side
		for(int i=3; i<7;i++)
		{
			SpawnPawn (1,i,2); 
		}
	}
	private void Start()
	{
		SpawnAllPieces ();
	}



	private void Update()
	{
		UpdateSelection ();
		DrawBoard ();

		if (Input.GetMouseButtonDown (0)) 
		{
			if(selectionX>=0 && selectionY>=0)
			{
				if(selectedPawn==null)
				{
					//Select Pawn
					SelectedPawn(selectionX,selectionY);
				}
				else
				{
					//Move Pawn
					MovePawn(selectionX,selectionY);
				}
			}
		}
	}

	private void SelectedPawn(int x, int y)
	{
		if (Pawns [x, y] == null) 
		{
			return;
		}
		if (Pawns [x, y].isWhite != isWhiteTurn) 
		{
			return;
		}
//		allowedMoves = Pawns [x, y].PossibleMove ();
		selectedPawn = Pawns [x, y];
//		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);
	}

	private void MovePawn(int x, int y)
	{
		if (selectedPawn.PossibleMove(x,y)) 
		{
			Pawns[selectedPawn.CurrentX,selectedPawn.CurrentY]=null;
			selectedPawn.transform.position= GetTile(x,y);
			Pawns[x,y]=selectedPawn;
			isWhiteTurn=!isWhiteTurn;
		}
		selectedPawn = null;
	}

	private void UpdateSelection()
	{
		if (!Camera.main)
			return;
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("BoardPlane"))) {
			selectionX = (int)hit.point.x;
			selectionY = (int)hit.point.z;
		} 
		else 
		{
			selectionX = -1;
			selectionY = -1;
		}
	}
	private void DrawBoard()
	{
		Vector3 widthLine = Vector3.right * 10;
		Vector3 heightLine = Vector3.forward * 3;

		for (int i=0; i<=3; i++)
		{
			Vector3 start= Vector3.forward*i;
			Debug.DrawLine(start, start+widthLine);
			for (int j=0; j<=10; j++)
			{
				start= Vector3.right*j;
				Debug.DrawLine(start, start+heightLine);
			}
		}

		//Draw Selection
		if (selectionX >= 0 && selectionY >= 0) 
		{
			Debug.DrawLine(Vector3.forward*selectionY+Vector3.right*selectionX,Vector3.forward*(selectionY+1)+Vector3.right*(selectionX+1)); 
			Debug.DrawLine(Vector3.forward*(selectionY+1)+Vector3.right*selectionX,Vector3.forward*selectionY+Vector3.right*(selectionX+1)); 

		}
		 
	}
}
