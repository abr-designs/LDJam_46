using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Interactable, IFlammable
{
    
    [SerializeField]
    private Collider2D rigidbodyCollider;

    private new SpriteRenderer renderer;

    [SerializeField]
    private Sprite litSprite;
    [SerializeField]
    private Sprite outSprite;

    public bool isLit { get; private set; }
    
    //================================================================================================================//

    private void Start()
    {
        if(!renderer)
            renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        CaveMan.canPickupTorch = this;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        CaveMan.canPickupTorch = null;
    }
    
    //================================================================================================================//

    public void Init(Collider2D ignoreCollider, bool isLit)
    {
        Physics2D.IgnoreCollision(ignoreCollider, rigidbodyCollider);

        if (!renderer)
            renderer = GetComponent<SpriteRenderer>();
        
        
        this.isLit = isLit;
        renderer.sprite = isLit ? litSprite : outSprite;
    }
    
    public bool isOnFire()
    {
        return isLit;
    }

    public void RegisterFlammable()
    {
       LevelManager.RegisterFire(this);
    }

    public void UnRegisterFlammable()
    {
        LevelManager.UnRegisterFire(this);
    }
}
