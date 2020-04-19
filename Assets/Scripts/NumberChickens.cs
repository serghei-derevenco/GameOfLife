using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberChickens : MonoBehaviour
{
	Text numberChickens; 

	private void Awake()
	{
		numberChickens = GetComponent<Text>();
		
	}

    private void Update()
	{
		

		GameObject[] chickens = GameObject.FindGameObjectsWithTag ("Chicken");
		int chickenTotal = chickens.Length;

		
		numberChickens.text = "Total chickens: " + chickenTotal.ToString();
	}	
}
