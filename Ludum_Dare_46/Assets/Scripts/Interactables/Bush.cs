using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Flammable
{
    [SerializeField] 
    private GameObject deadBushPrefab;

    private BurnedBush _burnedBush;
    
    

    /*protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if (!other.gameObject.CompareTag("Player")) 
            return;
        
        if (!burned || onFire) 
            return;
        
        GetComponent<Collider2D>().enabled = false;
        //TODO Play animation
        _burnedBush.BreakBush(() =>
        {
            Destroy(gameObject);
        });
    }*/

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) 
            return;
        
        if (!burned || onFire) 
            return;
        
        GetComponent<Collider2D>().enabled = false;
        //TODO Play animation
        _burnedBush.BreakBush(() =>
        {
            Destroy(gameObject);
        });
    }

    protected override void TriggerBurned()
    {
        base.TriggerBurned();

        if (_burnedBush) 
            return;
        
        _burnedBush = Instantiate(deadBushPrefab, transform.position, Quaternion.identity)
            .GetComponent<BurnedBush>();
        _burnedBush.transform.parent = transform;
        
        
    }

    protected override void TriggerIsPassable()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }
}
