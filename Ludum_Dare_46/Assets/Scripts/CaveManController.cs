using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Use this guide: https://www.raywenderlich.com/23-introduction-to-the-new-unity-2d-tilemap-system

public class CaveManController : MonoBehaviour
{
    public GameObject torchPrefab;
    public float throwForce;

    public bool isHoldingTorch;
    public Torch canPickupTorch;
    public bool isTorchLit { get; private set; }

    private Vector2 throwDirection;

    public bool inWater;

    
    //================================================================================================================//
    
    [SerializeField]
    private float speed;

    private bool moving;
    
    //Facing forward means towards the camera
    #region Facing Direction
    private bool facingCamera
    {
        get;
        set;
        //TODO Need to set the animator value here
    }

    private bool facingRight
    {
        get => _facingRight;
        set
        {
            _facingRight = value;
            if(facingCamera)
                SpriteRenderer.flipX = !facingRight;
            else
                SpriteRenderer.flipX = facingRight;
        }
    }
    private bool _facingRight;
    #endregion //Facing Direction
    
    [SerializeField]
    private GameObject cavemanObject;

    [SerializeField] 
    private GameObject TorchObject;

    private SpriteRenderer torchRenderer;
    [SerializeField]
    private Sprite litTorchSprite;
    [SerializeField]
    private Sprite outTorchSprite;
    
    private Animator Animator;
    private SpriteRenderer SpriteRenderer;

    private new GameObject gameObject;
    private new Transform transform;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    //[SerializeField]
    public Vector2Int moveDirection => _moveDirection;
    public Vector2Int _moveDirection;


    //================================================================================================================//
    
    // Start is called before the first frame update
    
    private void Start()
    {
        gameObject = cavemanObject;
        transform = gameObject.transform;

        Animator = gameObject.GetComponent<Animator>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();

        
        torchRenderer = TorchObject.GetComponent<SpriteRenderer>();

        facingRight = true;
        facingCamera = true;
        isHoldingTorch = true;
        canPickupTorch = null;
        isTorchLit = true;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();

        ApplyMotion();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isHoldingTorch)
                ThrowTorch();
            else if (canPickupTorch)
                PickupTorch();
        }
            
    }

    private void LateUpdate()
    {
        UpdateAnimations();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name, other.gameObject);
    }

    //================================================================================================================//

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            facingCamera = false;
            facingRight = _facingRight;
            _moveDirection.y = 1;
            throwDirection = Vector2.up;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            _moveDirection.y = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            facingCamera = true;
            facingRight = _facingRight;
            _moveDirection.y = -1;
            throwDirection = Vector2.down;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            _moveDirection.y = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _moveDirection.x = -1;
            facingRight = false;
            throwDirection = Vector2.left;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            _moveDirection.x = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            _moveDirection.x = 1;
            facingRight = true;
            throwDirection = Vector2.right;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            _moveDirection.x = 0;
        }
    }

    private void ApplyMotion()
    {
        if (moveDirection == Vector2Int.zero)
            return;

        var targetSpeed = inWater ? speed / 2f : speed;

        var posDelta = new Vector3(moveDirection.x, moveDirection.y, 0f) * (targetSpeed * Time.deltaTime) ;

        transform.position += posDelta;
    }

    private void UpdateAnimations()
    {
        /*//This needs to update 
        if(facingCamera)
            SpriteRenderer.flipX = !facingRight;
        else
            SpriteRenderer.flipX = facingRight;*/
        
        Animator.SetBool("Moving", (moveDirection != Vector2Int.zero));
        Animator.SetBool("FaceCamera", facingCamera);
    }
    
    //================================================================================================================//

    public void SetInWater(bool inWater)
    {
        this.inWater = inWater;

        if (isHoldingTorch && isTorchLit)
            SetTorchLit(false);
        
        Animator.SetBool("InWater", inWater);
    }
    
    //================================================================================================================//

    private void SetIsHoldingTorch(bool isHoldingTorch)
    {
        this.isHoldingTorch = isHoldingTorch;
        
        TorchObject.SetActive(isHoldingTorch);
    }

    private void ThrowTorch()
    {
        //isHoldingTorch = false;
        SetIsHoldingTorch(false);
        var direction = throwDirection;
        
        var temp = Instantiate(torchPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        temp.GetComponent<Torch>().Init(collider, isTorchLit);
        
        //TODO Move the force add to the torch object
        temp.AddForce(direction * throwForce);
        temp.AddTorque(Random.Range(0f, 0.3f) *(Random.value > 0.5f ? 1f : -1), ForceMode2D.Force);
    }

    private void PickupTorch()
    {
        //isHoldingTorch = true;
        SetIsHoldingTorch(true);
        SetTorchLit(canPickupTorch.isLit);
        
        Destroy(canPickupTorch.gameObject);
        canPickupTorch = null;
    }

    public void SetTorchLit(bool onFire)
    {
        isTorchLit = onFire;

        torchRenderer.sprite = isTorchLit ? litTorchSprite : outTorchSprite;
    }

    //================================================================================================================//

    #if UNITY_EDITOR

    [ContextMenu("Snuff Out")]
    public void BurnOutTorch()
    {
        SetTorchLit(false);
    }
    
    #endif
}
