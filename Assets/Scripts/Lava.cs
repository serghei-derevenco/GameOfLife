using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public virtual void ReceiveDamage()
	{
		Die();
	}   

	protected virtual void Die()
	{
		Destroy(gameObject);
	}

    // private void OnTriggerEnter2D(Collider2D collider)
    // {
    // 	Chicken chik = collider.GetComponent<Chicken>();
    // 	Monkey monk = collider.GetComponent<Monkey>();
    // 	Crocodile croco = collider.GetComponent<Crocodile>();

    // 	if (chik)
    // 	{
    // 		Destroy(gameObject);
    		
    // 	}

    // 	else if (monk)
    // 	{
    // 		Destroy(gameObject);
    		
    // 	}

    // 	else if (croco)
    // 	{
    // 		Destroy(gameObject);
    		
    // 	}
    // }

}
