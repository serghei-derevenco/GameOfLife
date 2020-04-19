using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberMonkeys : MonoBehaviour
{
    Text numberMonkeys; 

	private void Awake()
	{
		numberMonkeys = GetComponent<Text>();
		
	}

    private void Update()
	{
		

		GameObject[] monkeys = GameObject.FindGameObjectsWithTag ("Monkey");
		int monkeyTotal = monkeys.Length;

		
		numberMonkeys.text = "Total monkeys: " + monkeyTotal.ToString();
	}	
}
