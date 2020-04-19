using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Interactable, IFlammable
{
    
    [SerializeField]
    private Collider2D rigidbodyCollider;

    private new Rigidbody2D rigidbody;

    private new SpriteRenderer renderer;

    [SerializeField]
    private Sprite litSprite;
    [SerializeField]
    private Sprite outSprite;

    public bool isLit;
    public LayerMask mask;
    
    //================================================================================================================//

    private void Start()
    {
        if(!renderer)
            renderer = GetComponent<SpriteRenderer>();
        
        if (!rigidbody)
            rigidbody = GetComponent<Rigidbody2D>();
        
        SetIsLit(isLit);
    }

    private void LateUpdate()
    {
        if (!isLit)
            return;

        if (!rigidbody.IsSleeping())
            return;

        //var colliders = new Collider2D[4];
        if(rigidbody.IsTouchingLayers(mask.value))
            SetIsLit(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        
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

    private void OnDestroy()
    {
        UnRegisterFlammable();
    }

    //================================================================================================================//

    
    private void SetIsLit(bool isLit)
    {
        this.isLit = isLit;
        
        if (isLit)
            LevelManager.RegisterFire(this);
        
        renderer.sprite = isLit ? litSprite : outSprite;
        
    }
    
    //================================================================================================================//

    public void Init(Collider2D ignoreCollider, bool isLit)
    {
        Physics2D.IgnoreCollision(ignoreCollider, rigidbodyCollider);

        if (!renderer)
            renderer = GetComponent<SpriteRenderer>();

        if (!rigidbody)
            rigidbody = GetComponent<Rigidbody2D>();


        SetIsLit(isLit);
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
        if (!LevelManager)
            return;
        LevelManager.UnRegisterFire(this);
    }
}
