using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crocodile : Unit
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
    	Banana banana = collider.GetComponent<Banana>();

    	if (banana)
    	{
    		Destroy(gameObject);
    	}

    	Lava lava = collider.GetComponent<Lava>();

    	if (lava)
    	{
    		Destroy(gameObject);
    	}

    	Fertile fertile = collider.GetComponent<Fertile>();

    	if (fertile)
    	{
    		int temp = Random.Range(1, 5);
    		if (temp % 4 == 1)
    			Instantiate(gameObject);
    	}
    }
}
