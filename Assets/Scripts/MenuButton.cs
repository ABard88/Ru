using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour 
{
	
	public void Ru(string Ru)
	{
		Application.LoadLevel (Ru);
	}
	
	public void Domination (string Domination)
	{
		Application.LoadLevel (Domination);
	}
	public void DaethMatch (string DeathMatch)
	{
		Application.LoadLevel (DeathMatch);
	}
	
	public void Settings (string Settings)
	{
		Application.LoadLevel (Settings);
	}
	public void StartM (string StartM)
	{
		Application.LoadLevel (StartM);
	}
		
	public void Exit (string Exit)
	{
		Application.Quit ();
	}
	
	
}
