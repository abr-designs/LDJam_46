using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CaveManController : MonoBehaviour
{
    public GameObject torchPrefab;
    public float throwForce;
    
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
    
    private Animator Animator;
    private SpriteRenderer SpriteRenderer;

    private new GameObject gameObject;
    private new Transform transform;
    private new Rigidbody2D rigidbody;

    [SerializeField]
    private Vector2Int moveDirection;
    
    //================================================================================================================//
    
    // Start is called before the first frame update
    private void Start()
    {
        gameObject = cavemanObject;
        transform = gameObject.transform;

        Animator = gameObject.GetComponent<Animator>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        
        torchRenderer = TorchObject.GetComponent<SpriteRenderer>();

        facingRight = true;
        facingCamera = true;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();

        ApplyMotion();

        if (Input.GetKeyDown(KeyCode.Space))
            ThrowTorch();
    }

    private void LateUpdate()
    {
        UpdateAnimations();
    }

    //================================================================================================================//

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            facingCamera = false;
            facingRight = _facingRight;
            moveDirection.y = 1;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            moveDirection.y = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            facingCamera = true;
            facingRight = _facingRight;
            moveDirection.y = -1;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            moveDirection.y = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection.x = -1;
            facingRight = false;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            moveDirection.x = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection.x = 1;
            facingRight = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            moveDirection.x = 0;
        }
    }

    private void ApplyMotion()
    {
        if (moveDirection == Vector2Int.zero)
            return;

        var posDelta = new Vector3(moveDirection.x, moveDirection.y, 0f) * (speed * Time.deltaTime) ;

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

    private void ThrowTorch()
    {
        var direction = new Vector2(facingRight ? 1f : -1f, facingCamera ? -1f : 1f);
        
        var temp = Instantiate(torchPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        temp.AddForce(direction * throwForce);
        temp.AddTorque(Random.Range(0f, 0.3f) *(Random.value > 0.5f ? 1f : -1), ForceMode2D.Force);
    }


}
