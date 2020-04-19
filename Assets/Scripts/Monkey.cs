using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : Unit
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
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
