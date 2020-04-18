using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : Interactable, IFlammable
{
    [SerializeField] 
    private bool startOnFire;
    [SerializeField] 
    private bool startBurned;
    [SerializeField] 
    private bool burnForever;
    
    public bool onFire{get; private set;}
    protected bool burned{get; private set;}
    
    //================================================================================================================//
    
    [SerializeField] protected float burnTime;
    protected float timer;

    [SerializeField] private AnimationCurve burnCurve;

    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    

    [SerializeField] private GameObject firePrefab;
    private Fire fire;

    protected new SpriteRenderer renderer { get; private set; }
    protected new Transform transform { get; private set; }
    
    //================================================================================================================//

    // Start is called before the first frame update
    private void Start()
    {
        
        transform = gameObject.transform;
        renderer = GetComponent<SpriteRenderer>();
        
        if(startOnFire)
            StarFire();
        else if (startBurned)
        {
            TriggerBurned();
            onFire = false;
        }
    }

    private void Update()
    {
        if (burned)
            return;

        if (!onFire)
            return;

        if(!burnForever)
            UpdateFire();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (onFire && CaveMan.isHoldingTorch && !CaveMan.isTorchLit)
                CaveMan.SetTorchLit(true);

            return;
        }

        if (!other.gameObject.CompareTag("Torch"))
            return;

        if (!other.gameObject.GetComponent<Torch>().isLit)
            return;

        if (burned || onFire)
            return;

        StarFire();

    }
    
    //================================================================================================================//

    protected virtual void StarFire()
    {
        fire = Instantiate(firePrefab, transform.position, Quaternion.identity).GetComponent<Fire>();
        //TODO Countdown burn time, change color

        onFire = true;
        RegisterFlammable();
    }

    /// <summary>
    /// Wont get called if burnForever is set to true
    /// </summary>
    protected virtual void UpdateFire()
    {
        
        if (timer >= burnTime)
        {
            TriggerBurned();

            //TODO Change the sprite of the bush to burned one, disable collider
            fire.BurnOut(FireBurnedOut);

            return;
        }

        timer += Time.deltaTime;

        renderer.color = Color.Lerp(startColor, endColor, burnCurve.Evaluate(timer / burnTime));
    }

    protected virtual void TriggerBurned()
    {
        burned = true;
        renderer.sprite = null;

    }

    protected virtual void FireBurnedOut()
    {
        onFire = false;
        UnRegisterFlammable();
    }
    
    //================================================================================================================//
    public bool isOnFire()
    {
        return onFire;
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
