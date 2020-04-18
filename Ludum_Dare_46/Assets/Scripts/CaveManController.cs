using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveManController : MonoBehaviour
{
    
    [SerializeField]
    private float speed;

    private bool moving;
    
    //Facing forward means towards the camera
    #region Facing Direction
    private bool facingCamera
    {
        get => _facingCamera;
        set => _facingCamera = value;
        //TODO Need to set the animator value here
    }
    private bool _facingCamera;

    private bool facingRight
    {
        get => _facingRight;
        set
        {
            _facingRight = value;

        }
    }
    private bool _facingRight;
    #endregion //Facing Direction
    
    [SerializeField]
    private GameObject cavemanObject;

    private Animator Animator;
    private SpriteRenderer SpriteRenderer;

    private new GameObject gameObject;
    private new Transform transform;

    [SerializeField]
    private Vector2Int moveDirection;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        gameObject = cavemanObject;
        transform = gameObject.transform;

        Animator = gameObject.GetComponent<Animator>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        facingRight = true;
        facingCamera = true;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();

        UpdateAnimations();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            facingCamera = false;
            moveDirection.y = 1;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            moveDirection.y = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            facingCamera = true;
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

        /*moving = false;
        if (Input.GetKey(KeyCode.W))
        {
            facingCamera = false;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            facingCamera = true;
            moving = true;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            facingRight = false;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            facingRight = true;
            moving = true;
        }*/
    }

    private void UpdateAnimations()
    {
        if(facingCamera)
            SpriteRenderer.flipX = !facingRight;
        else
            SpriteRenderer.flipX = facingRight;
        
        Animator.SetBool("Moving", (moveDirection != Vector2Int.zero));
        Animator.SetBool("FaceCamera", facingCamera);
    }
    
    


}
