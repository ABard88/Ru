using UnityEngine;
using System.Collections;

public class Pieces : Pawns 

{
	public GameObject Dice;
	public int step;
	public static int decreementDieValue, xSpot,ySpot, xSpotP2, ySpotP2;
	bool retreat=false;

	public override bool[,] PossibleMove()
	{
		bool[,] r = new bool[10, 3];
		Pawns P1;
		Dice=GameObject.FindGameObjectWithTag("Dice");
		bool rollOn=true;
		bool turn;

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

			xSpot=CurrentX;
			ySpot=CurrentY;
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
			P1=BoardManager.Instance.Pawns[xSpot,ySpot];
			if(P1==null)
				r[xSpot,ySpot]=true;
			else
			{
				bool v1,v2;
				v1 = ((P1.isWhite==false) && (P1.CurrentX!=3) && (P1.CurrentY==1));
				v2 = ((P1.isWhite==false) && (P1.CurrentX!=7) && (P1.CurrentY==1));
				if( v1 && v2 )
				{
					r[xSpot,ySpot]=true;
				}
			}

		}
		else 
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
				r[xSpotP2,ySpotP2]=true;
			}
			else
			{
				bool v1,v2;
				v1 = ((P1.isWhite) && (P1.CurrentX!=3) && (P1.CurrentY==1));
				v2 = ((P1.isWhite) && (P1.CurrentX!=7) && (P1.CurrentY==1));
				if( v1 && v2 )
				{
					r[xSpotP2,ySpotP2]=true;
				}
			}

		}

		return r;
	}

}
