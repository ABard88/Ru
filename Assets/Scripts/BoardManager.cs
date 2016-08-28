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

	public static int selectionX = -1;
	public static int selectionY = -1;

	public int scoreP1=0;
	public int scoreP2 = 0;
	public int moveCounter=1;
	public static int[] fortOccupied = new int[6];

	public static bool isWhiteTurn=true;

	public List<GameObject> pawnPrefab;
	private List<GameObject> activePawn= new List<GameObject>(); 
	public GameObject whiteWin;
	public GameObject blacWin;
	public GameObject Dice;
	public int zeroCounter;

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
		Instance = this;
		SpawnAllPieces ();
	}
		
	private void Update()
	{
		UpdateSelection ();
		DrawBoard ();
		zeroCounter = Dice.GetComponent<DisplayCurrentDieValue> ().currentValue;
		if (scoreP1 == 4) 
		{
			whiteWin.transform.localPosition=new Vector3(5,2,0);
		}
		if (scoreP2 == 4) 
		{
			blacWin.transform.localPosition=new Vector3(5,2,0);
		}


		if (Input.GetMouseButtonDown (0)) 
		{
			if(selectionX>=0 && selectionY>=0)
			{
				if(selectedPawn==null)
				{
					//Select Pawn by clicking on it.
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
/*		if (zeroCounter == 0) // If a Zero is rolled switch turns.
		{
			isWhiteTurn= !isWhiteTurn;
		}*/
		if (Pawns [x, y].isWhite != isWhiteTurn) 
		{
			return;
		}

		bool canMove = false;
		allowedMoves = Pawns [x, y].PossibleMove ();
		for (int i=0; i<9; i++)
			for (int j=0; j<3; j++)
				if (allowedMoves [i, j]) 
				{
					canMove = true; 
					Debug.Log("Can Move");
				}
					
		if (!canMove) 
		{
			Debug.Log("Can't Move");
			return;
		}
		selectedPawn = Pawns [x, y];
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);

	}

	private void MovePawn(int x, int y)
	{
		if (allowedMoves[x,y]) 
		{
			Pawns P1= Pawns[x,y];

			if(P1!=null && P1.isWhite!=isWhiteTurn)
			{
				//Capture Pawn
				activePawn.Remove(P1.gameObject);
				if(P1.isWhite)
				{
					SpawnPawn(0,7,0);
				}
				else
				{
					SpawnPawn(1,7,2);
				}
				Destroy(P1.gameObject);

			}
			
			Pawns[selectedPawn.CurrentX,selectedPawn.CurrentY]=null;
			selectedPawn.transform.position= GetTile(x,y);
			selectedPawn.SetPosition(x,y);
			Pawns[x,y]=selectedPawn;
			moveCounter=moveCounter+1;
			if(selectedPawn.CurrentX==8 && selectedPawn.CurrentY==0)
			{
				scoreP1=scoreP1+1;
				Debug.Log("Score="+scoreP1);
				Destroy(selectedPawn.gameObject);
			}
			if(selectedPawn.CurrentX==8 && selectedPawn.CurrentY==2)
			{
				scoreP2=scoreP2+1;
				Debug.Log("Score="+scoreP2);
				Destroy(selectedPawn.gameObject);
			}

//If landing on fortress dont change turn else change turn
			if((selectedPawn.CurrentX==3 && selectedPawn.CurrentY==1)||(selectedPawn.CurrentX==7 && selectedPawn.CurrentY==1)||(selectedPawn.CurrentX==0 && selectedPawn.CurrentY==0)||(selectedPawn.CurrentX==0 && selectedPawn.CurrentY==2) ||(selectedPawn.CurrentX==8 && selectedPawn.CurrentY==0)||(selectedPawn.CurrentX==8 && selectedPawn.CurrentY==2))
			{
				isWhiteTurn=isWhiteTurn;
			}
			else
				isWhiteTurn=!isWhiteTurn;
		}
		BoardHighlights.Instance.HideHighlights ();
		selectedPawn = null;

	}

	private void UpdateSelection()
	{
		if (!Camera.main)
			return;
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("BoardPlane"))) 
		{
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
