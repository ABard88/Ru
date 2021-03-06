﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardHighlights : MonoBehaviour 
{

	public static BoardHighlights Instance{ set; get;}
	public GameObject highlightPrefab;
	public GameObject fortressPrefab;
	public GameObject highlightkhadan;
	public GameObject compIndicator;
	public GameObject playerIndicator;
	private List<GameObject> highlights;
	private List<GameObject> fortresses;
	private List<GameObject> khadan;
	private GameObject ct, pt;

	private void Start()
	{
		Instance = this;
		highlights = new List<GameObject> ();
		fortresses = new List<GameObject> ();
		khadan = new List<GameObject> ();
		HighlightFortress ();
		Khadan ();
		ct=Instantiate (compIndicator);
		pt=Instantiate (playerIndicator);
	}

	private GameObject GetHighlightObject() 
	{
		GameObject go = highlights.Find (g => !g.activeSelf);
		if (go == null) 
		{
			go=Instantiate(highlightPrefab);
			highlights.Add(go);
		}
		return go;
	}

	public void HighlightAllowedMoves(bool[,] moves) // Highlights for possible moves.
	{
		for (int i=0; i<10; i++) 
		{
			for (int j=0; j<3; j++) 
			{
				if(moves[i,j])
				{
					GameObject go=GetHighlightObject();
					go.SetActive(true);
					go.transform.position=new Vector3(i+0.5f,0,j+0.5f);
				}
			}
		}
	}

	public void HighlightFortress() // This highlights the fortresses and is always on as it's called at the start.
	{
		GameObject fo = highlights.Find (g => !g.activeSelf);
		for(int i=0; i<10;i++)
		{
			for(int j=0;j<3;j++)
			{
				if((i==0 && j==0) || (i==3 && j==1)|| (i==7 && j==1) || (i==8 && j==0) || (i==0 && j==2)|| (i==8 && j==2)) //Fortress coordinates
				{
					fo = Instantiate(fortressPrefab);
					fo.SetActive(true);
					fortresses.Add(fo);
					fo.transform.position=new Vector3(i+0.5f,0,j+0.5f);
				}

			}
		}

	}

	public void Khadan()// Black space for inaccessible square also called at the start so always on.
	{
		GameObject ko = highlights.Find (g => !g.activeSelf);
		for(int i=0; i<10;i++)
		{
			for(int j=0;j<3;j++)
			{
				if((i==7 && j==0) || (i==7 && j==2))
				{
					ko = Instantiate(highlightkhadan);
					ko.SetActive(true);
					khadan.Add(ko);
					ko.transform.position=new Vector3(i+0.5f,0,j+0.5f);
				}
				
			}
		}
	}

	public void HideHighlights()
	{
		foreach(GameObject go in highlights)
			go.SetActive(false);
	}

	public void Update()
	{
		bool who = BoardManager.isWhiteTurn;
		if (!who) 
		{
			ct.transform.position = new Vector3 (2, 1, 3);
			pt.transform.position = new Vector3 (20, 1, 3);
		} 
		else 
		{
			pt.transform.position = new Vector3 (2, 1, 3);
			ct.transform.position = new Vector3 (20, 1, 3);
		}

	}

}
