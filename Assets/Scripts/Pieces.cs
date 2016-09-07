using UnityEngine;
using System.Collections;

public class Pieces : Pawns 

{
	public GameObject Dice; //Uses values from the dice.
	public int step; // used as a countdown from the dice value to finish playing appropriate no of steps
	public static int decreementDieValue, xSpot,ySpot, xSpotP2, ySpotP2; // ints for identifying final move coordinates
	bool retreat=false; // used when you've rolled -1 
	public static bool Kill=false, Fort =false, canMove=false; // activated when computer can kill or land on fortress helps in computer decision

	public override bool[,] PossibleMove()
	{
		// initialize variables
		bool[,] r = new bool[10, 3];
		Pawns P1;
		Dice=GameObject.FindGameObjectWithTag("Dice");
		bool rollOn=true;
		bool turn;
		Fort = false;
		Kill = false;
		canMove = false;
		//White Moves
		if (isWhite)
		{
			if(rollOn)
			{
				step = Dice.GetComponent<DisplayCurrentDieValue> ().currentValue; 
				rollOn=false;
			}
			turn=BoardManager.isWhiteTurn;
			decreementDieValue=step;
			if(decreementDieValue==0){BoardManager.isWhiteTurn=false;}
			if(decreementDieValue<0)
			{
				decreementDieValue=1;
				retreat=true;
			}
			// gets current position of the pawn
			xSpot=CurrentX;
			ySpot=CurrentY;
			// shadow moves on the grid according to the path laid out
			while(decreementDieValue!=0)
			{
				while((xSpot>0) && (ySpot<1) && (decreementDieValue>0))
				{
					xSpot=xSpot-1;
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						xSpot=xSpot+2;
						retreat=false;
					}
				}
				while((xSpot==0) && (ySpot==0) && (decreementDieValue>0))
				{
					ySpot=ySpot+1;
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						xSpot=xSpot+1;
						ySpot=ySpot-1;
						retreat=false;
					}
				}
				while((xSpot==0) && (ySpot==1) && (decreementDieValue>0))
				{
					xSpot=xSpot+1;
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						ySpot=ySpot-1;
						xSpot=xSpot-1;
						retreat=false;
					}
				}
				while((xSpot<9) && (ySpot>0) && (decreementDieValue>0))
				{
					xSpot=xSpot+1;
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						xSpot=xSpot-2;
						retreat=false;
					}
				}

				while((xSpot==9) && (ySpot==1) && (decreementDieValue>0))
				{
					ySpot=ySpot-1;
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						xSpot=xSpot-1;
						retreat=false;
					}
				}
				while((xSpot==9) && (ySpot==0) && (decreementDieValue>0))
				{
					xSpot=xSpot-1;	
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						ySpot=ySpot+1;
						retreat=false;
					}
				}
			}

			Debug.Log(xSpot+","+ySpot);
			P1=BoardManager.Instance.Pawns[xSpot,ySpot]; // Selects the final resting squre in the shadow move

			if(P1==null) // checks if the square is empty
			{
				r[xSpot,ySpot]=true;
				if(xSpot==7 && ySpot==0)
				{
					r[xSpot,ySpot]=false;
				}
			}
			else // if square is not empty
			{
				bool v1,v2;
				v1 = ((P1.isWhite==false) && (P1.CurrentX!=3) && (P1.CurrentY==1)); 
				v2 = ((P1.isWhite==false) && (P1.CurrentX!=7) && (P1.CurrentY==1));
				if( v1 && v2 ) // if square is occupied by a red piece not on the fortress
				{
					r[xSpot,ySpot]=true;
				}
			}

		}
		else // mirror image of white, canKill and onFort bools are activated when true so BoardManager Autoplay() can make decisions.
		{
//			Dice.GetComponent<ApplyForceinRandomDirection>().Roll(); // Get Black to Roll
			step = Dice.GetComponent<DisplayCurrentDieValue> ().rollVal;// Get the dievalue
			decreementDieValue=step;
			if(decreementDieValue<0)
			{
				decreementDieValue=1;
				retreat=true;
			}
			xSpotP2=CurrentX; ySpotP2=CurrentY;

			while(decreementDieValue!=0)
			{
				while((xSpotP2>0) && (ySpotP2>1) && (decreementDieValue>0))
				{
					xSpotP2=xSpotP2-1;
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						xSpotP2=xSpotP2+2;
						retreat=false;
					}
					if(xSpotP2==9)
					{
						xSpotP2=xSpotP2+1;
					}
				}
				while((xSpotP2==0) && (ySpotP2==2) && (decreementDieValue>0))
				{
					ySpotP2=ySpotP2-1;
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						xSpotP2=xSpotP2+1;
						ySpotP2=ySpotP2+1;
						retreat=false;
					}
				}

				while((xSpotP2<9) && (ySpotP2==1) && (decreementDieValue>0))
				{
					xSpotP2=xSpotP2+1;
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						xSpotP2=xSpotP2-2;
						if(xSpot==0)
						{
							ySpot=ySpot+1;
						}
						retreat=false;
					}
				}
				
				while((xSpotP2==9) && (ySpotP2==1) && (decreementDieValue>0))
				{
					ySpotP2=ySpotP2+1;
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						xSpotP2=xSpotP2-1;
						retreat=false;
					}
				}
				while((xSpotP2==9) && (ySpotP2==2) && (decreementDieValue>0))
				{
					xSpotP2=xSpotP2-1;	
					decreementDieValue=decreementDieValue-1;
					if(retreat==true)
					{
						ySpotP2=ySpotP2-1;
						retreat=false;
					}
				}
				
			}
			//Move use if else case or use a different variable P1 is not null then it will not return true.
			P1=BoardManager.Instance.Pawns[xSpotP2,ySpotP2];
			if( P1==null)
			{
				canMove=true;
				r[xSpotP2,ySpotP2]=true;
				if(xSpotP2==7 && ySpotP2==2)
				{
					canMove=false;
					r[xSpotP2,ySpotP2]=false;
				}
				if((xSpotP2==3 && ySpotP2==1)||(xSpotP2==7 && ySpotP2==1)||(xSpotP2==0 && ySpotP2==2)||(xSpotP2==8 && ySpotP2==2))
				{
					Fort=true;
					canMove=true;
				}
			}
			// Introduce a condition where if y=2 and x<8 don't move.
			else
			{
				bool v1,v2;
				v1 = ((P1.isWhite) && (P1.CurrentX!=3) && (P1.CurrentY==1));
				v2 = ((P1.isWhite) && (P1.CurrentX!=7) && (P1.CurrentY==1));
				if(v1 && v2)
				{
					canMove=true;
					Kill=true;
					r[xSpotP2,ySpotP2]=true;
				}
			}

		}
		return r;
	}

}
