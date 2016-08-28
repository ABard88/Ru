using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardHighlights : MonoBehaviour 
{

	public static BoardHighlights Instance{ set; get;}
	public GameObject highlightPrefab;
	public GameObject fortressPrefab;
	private List<GameObject> highlights;
	private List<GameObject> fortresses;


	private void Start()
	{
		Instance = this;
		highlights = new List<GameObject> ();
		fortresses = new List<GameObject> ();
		HighlightFortress ();

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

	public void HighlightAllowedMoves(bool[,] moves)
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

	public void HighlightFortress()
	{
		GameObject fo = highlights.Find (g => !g.activeSelf);
		for(int i=0; i<10;i++)
		{
			for(int j=0;j<3;j++)
			{
				if((i==0 && j==0) || (i==3 && j==1)|| (i==7 && j==1) || (i==8 && j==0) || (i==0 && j==2)|| (i==8 && j==2))
				{
					fo = Instantiate(fortressPrefab);
					fo.SetActive(true);
					fortresses.Add(fo);
					fo.transform.position=new Vector3(i+0.5f,0,j+0.5f);
				}

			}
		}

	}

	public void HideHighlights()
	{
		foreach(GameObject go in highlights)
			go.SetActive(false);
	}

}
