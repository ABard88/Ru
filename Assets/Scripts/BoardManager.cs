using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour 
{
	public static BoardManager Instance{ set; get;}
	private bool[,] allowedMoves{ set; get;}

	public Camera mainCam;
	public Camera diceCam;

	public Text UITurn;
	public Text UIScoreP1;
	public Text UIScoreP2;
	
	public Pawns[,] Pawns{ set; get;}

	public GameObject[] blacks;
	private Pawns selectedPawn;
	public GameObject dice;
	private const float TILE_SIZE=1.0f;
	private const float TILE_OFFSET=0.5f;

	public static int selectionX = -1;
	public static int selectionY = -1;

	public int scoreP1=0;
	public int scoreP2 = 0;
	public int moveCounter=1;
	public int redTeam=4;
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
		mainCam.enabled = true;
		diceCam.enabled = false;
	}

	public void SwitchCam()
	{
		mainCam.enabled = !mainCam.enabled;
		diceCam.enabled = !diceCam.enabled;
	}
	IEnumerator Delay(float waitTime)
	{
		if (!isWhiteTurn) 
		{
			yield return new WaitForSeconds (waitTime);
			StartCoroutine (Autoplay (2.0f));
			yield break;
		} 
		yield break;
	}

	IEnumerator Autoplay(float waitTime)
	{
//		SwitchCam();
//		Dice.GetComponent<ApplyForceinRandomDirection>().Roll();
		if (!isWhiteTurn)
		{
			blacks = GameObject.FindGameObjectsWithTag ("Black");
			int[] pieceCounterX = new int[4];
			int[] pieceCounterY = new int[4];
			int i = 0;
			// insert foreach in a for loop where i= no. of pawns. check Gwent3PolCamp4
			foreach (GameObject Black in blacks) {
				pieceCounterX [i] = Black.GetComponent<Pieces> ().CurrentX; //This tells current xspot of selected pawn
				pieceCounterY [i] = Black.GetComponent<Pieces> ().CurrentY; //This tells current yspot of selected pawn
				i++;
			}
			// Write down code for SelectedPawn function
			bool canMove = false;
			int countPawn = 0;
			bool canKill = false;
			bool onFort = false;
			while (countPawn< redTeam && canKill==false && onFort==false) 
			{
				allowedMoves = Pawns [pieceCounterX [countPawn], pieceCounterY [countPawn]].PossibleMove ();
				for (int a=0; a<=9; a++)
					for (int b=0; b<3; b++)
						if (allowedMoves [a, b]) {
							canMove = Pieces.canMove; // Find a way to use can move to check and move the pawn
							canKill = Pieces.Kill;
							onFort = Pieces.Fort;
							Debug.Log ("Can Move");
						}
				if (canMove == true) {
					selectedPawn = Pawns [pieceCounterX [countPawn], pieceCounterY [countPawn]];
					BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);
				}
				countPawn++;
			}
			if (canKill == false && onFort == false && countPawn == redTeam)
			{
				int temp = Random.Range (0, redTeam);
				selectedPawn = Pawns [pieceCounterX [temp], pieceCounterY [temp]]; //[0] This makes the 1st pawn move
				allowedMoves = Pawns [pieceCounterX [temp], pieceCounterY [temp]].PossibleMove (); // write better logic to make the furthest one move.
				int tempD = Pieces.xSpotP2;
				int tempE = Pieces.ySpotP2;
				if (tempD == 7 && tempE == 2) { // Condition added so as not to move the pawn on to square 7,2
					selectedPawn = Pawns [pieceCounterX [Random.Range (0, redTeam)], pieceCounterY [Random.Range (0, redTeam)]];
					allowedMoves = Pawns [pieceCounterX [Random.Range (0, redTeam)], pieceCounterY [Random.Range (0, redTeam)]].PossibleMove ();
				}
			
			}
			//Write down code for MovePawn function [Re written]
			int d = Pieces.xSpotP2; // These two numbers have to be equal to the bool value that is 
			int e = Pieces.ySpotP2; // spit out by the Possible Move function in Pieces
			if (allowedMoves [d, e]) {
				Pawns P1 = Pawns [d, e];
				if (P1 != null && P1.isWhite) {
					//Capture Pawn and respawn at an empty space
					activePawn.Remove (P1.gameObject);
					bool replaced = false;
					if (P1.isWhite) {
						for (int c=3; c<7; c++) {
							if (Pawns [c, 0] == null && replaced == false) {
								SpawnPawn (0, c, 0);
								replaced = true;
							}
						}
					}

					Destroy (P1.gameObject);				
				}
			}	
			Pawns [selectedPawn.CurrentX, selectedPawn.CurrentY] = null;
			selectedPawn.transform.position = GetTile (d, e);
			selectedPawn.SetPosition (d, e);
			Pawns [d, e] = selectedPawn;
			moveCounter = moveCounter + 1;
			Debug.Log ("Blacks=" + selectedPawn.CurrentX + "," + selectedPawn.CurrentY);
			if (selectedPawn.CurrentX == 8 && selectedPawn.CurrentY == 2) {
				scoreP2 = scoreP2 + 1;
				Destroy (selectedPawn.gameObject);
				redTeam--;
			}			
			if ((selectedPawn.CurrentX == 3 && selectedPawn.CurrentY == 1) || (selectedPawn.CurrentX == 7 && selectedPawn.CurrentY == 1) || (selectedPawn.CurrentX == 0 && selectedPawn.CurrentY == 2) || (selectedPawn.CurrentX == 8 && selectedPawn.CurrentY == 2)) {
				isWhiteTurn = false;
			} else {
				isWhiteTurn = !isWhiteTurn;
			}
			BoardHighlights.Instance.HideHighlights ();
			selectedPawn = null;
		}
		yield break;
	}
	IEnumerator PlayerMove(float waitTime)
	{
		if (Input.GetMouseButtonDown (0)) // Input for selecting pawn
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
		yield break;
	}
	public void Play()
	{
		if (!isWhiteTurn) 
		{
			StartCoroutine (Delay (3.0f));
		} 
		else 
			StartCoroutine(PlayerMove (3.0f));

	}
	private void Update()
	{
		string whoseTurn;
		//Display Game states.
		if (isWhiteTurn) 
		{
			 whoseTurn = "Whites' Move";
		}
		else 
		{
			 whoseTurn = "Reds' Move";
		}
		UIScoreP1.text = "Whites Score:" + scoreP1.ToString ();
		UIScoreP2.text = "Reds Score:" + scoreP2.ToString ();
		UITurn.text = whoseTurn.ToString ();
		UpdateSelection ();
		DrawBoard ();

		//Switch cams
		if (Input.GetKeyUp(KeyCode.Z)) 
		{
			SwitchCam();
		}
		if (Input.GetKeyUp(KeyCode.Space)) 
		{
			SwitchCam();
		}

		// Check winning conditions.
		if (scoreP1 == 4) 
		{
			whiteWin.transform.localPosition=new Vector3(5,2,0);
		}
		if (scoreP2 == 4) 
		{
			blacWin.transform.localPosition=new Vector3(5,2,0);
		}

		Play ();
		// Calling computers turn
	}

	private void SelectedPawn(int x, int y) // function to select pawn
	{
		if (Pawns [x, y] == null) 
		{
			return;
		}
		zeroCounter = dice.GetComponent<DisplayCurrentDieValue> ().currentValue;
		// If you roll a zero then no movement possible therefore switch sides
		if (zeroCounter == 0) 
		{
			isWhiteTurn=!isWhiteTurn;
		}

		if (Pawns [x, y].isWhite != isWhiteTurn) 
		{
			return;
		}

		bool canMove = false;
		allowedMoves = Pawns [x, y].PossibleMove (); // checks for allowed moves of the selected pawn;
		for (int i=0; i<=9; i++)
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
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves); // highlights where the pawn will move to
	}

	private void MovePawn(int x, int y) // Function to move pawn
	{
		if (allowedMoves[x,y]) 
		{
			Pawns P1= Pawns[x,y];
			if(P1!=null && P1.isWhite!=isWhiteTurn)
			{
				//Capture Pawn and respawn at an empty space
				activePawn.Remove(P1.gameObject);
				bool replaced=false;
				if(P1.isWhite)
				{
					for(int i=3; i<7;i++)
					{
						if(Pawns[i,0]==null && replaced==false)
						{
							SpawnPawn(0,i,0);
							replaced=true;
						}
					}
				}
				else
				{
					for(int i=3; i<7;i++)
					{
						if(Pawns[i,2]==null && replaced==false)
						{
							SpawnPawn(1,i,2);
							replaced=true;
						}
					}
				}
				Destroy(P1.gameObject);

			}
			// moves pawns.
			Pawns[selectedPawn.CurrentX,selectedPawn.CurrentY]=null;
			selectedPawn.transform.position= GetTile(x,y);
			selectedPawn.SetPosition(x,y);
			Pawns[x,y]=selectedPawn;
			moveCounter=moveCounter+1;
			if(selectedPawn.CurrentX==8 && selectedPawn.CurrentY==0) // Updates score when pawn lands on final square
			{
				scoreP1=scoreP1+1;
				Debug.Log("Score="+scoreP1);
				Destroy(selectedPawn.gameObject);
			}


//If landing on fortress dont change turn else change turn
			if((selectedPawn.CurrentX==3 && selectedPawn.CurrentY==1)||(selectedPawn.CurrentX==7 && selectedPawn.CurrentY==1)||(selectedPawn.CurrentX==0 && selectedPawn.CurrentY==0)||(selectedPawn.CurrentX==0 && selectedPawn.CurrentY==2) ||(selectedPawn.CurrentX==8 && selectedPawn.CurrentY==0)||(selectedPawn.CurrentX==8 && selectedPawn.CurrentY==2))
			{
				isWhiteTurn=isWhiteTurn;
			}
			else
			{
				isWhiteTurn=!isWhiteTurn;
			}
		}
		BoardHighlights.Instance.HideHighlights ();
		selectedPawn = null;

	}

	private void UpdateSelection() // Identifies the pawn selected.
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
	private void DrawBoard() // Draws a coordinate grid which helps in identifying locations of pieces.
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
