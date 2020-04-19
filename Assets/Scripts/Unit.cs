using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public virtual void ReceiveDamage()
	{
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce(transform.up * 5.0F, ForceMode2D.Impulse);
		Die();
	}   

	protected virtual void Die()
	{
		Destroy(gameObject);
	} 

	new private Rigidbody2D rigidbody;
	private SpriteRenderer sprite;

    private void Awake()
    {
    	rigidbody = GetComponent<Rigidbody2D>();
    	sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
    	Lava lava = collider.GetComponent<Lava>();

    	if (lava)
    	{
    		Destroy(gameObject);
    	}
    }
}
