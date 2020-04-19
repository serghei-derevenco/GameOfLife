using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberCrocodiles : MonoBehaviour
{
	Text numberCrocodiles; 

	private void Awake()
	{
		numberCrocodiles = GetComponent<Text>();
		
	}

    private void Update()
	{
		

		GameObject[] crocodiles = GameObject.FindGameObjectsWithTag ("Crocodile");
		int crocodileTotal = crocodiles.Length;

		
		numberCrocodiles.text = "Total crocodiles: " + crocodileTotal.ToString();
	}	
}
