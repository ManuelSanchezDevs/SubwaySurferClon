using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Middle = 0, Left = -2, Right = 2 }

public class PlayerController : MonoBehaviour
{
    private Transform myTransform;  
    private Animator _animator;
    public Animator Animator { get => _animator; set => _animator = value; }
    private CharacterController _myCharacterController;
    public CharacterController MyCharacterController { get => _myCharacterController; set => _myCharacterController = value; }
    private PlayerCollision playerCollision;

    private Side position; 
    private Vector3 motionVector;
    [Header("Player Controller")]
    [SerializeField] private float _fordwardSpeed;
    public float FordwardSpeed { get => _fordwardSpeed; set => _fordwardSpeed = value; }

    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float jumpPower;
    private float newXPosition;
    private float yPosition;
    private float xPosition;
    private float rollTimer;

    private int _IdDogdgeLeft = Animator.StringToHash("DodgeLeft");
    public int IdDogdgeLeft { get => _IdDogdgeLeft; set => _IdDogdgeLeft = value; }
    private int _IdDodgeRight = Animator.StringToHash("DodgeRight");
    public int IdDodgeRight { get => _IdDodgeRight; set => _IdDodgeRight = value; }
    private int _IdJump = Animator.StringToHash("Jump");
    public int IdJump { get => _IdJump; set => _IdJump = value; }
    private int _IdFall = Animator.StringToHash("Fall");
    public int IdFall { get => _IdFall; set => _IdFall = value; }
    private int _IdLanding = Animator.StringToHash("Landing");
    public int IdLanding { get => _IdLanding; set => _IdLanding = value; }
    private int _IdRoll = Animator.StringToHash("Roll");
    public int IdRoll { get => _IdRoll; set => _IdRoll = value; }
    private int _IdStumbleLow = Animator.StringToHash("StumbleLow");
    public  int IdStumbleLow { get => _IdStumbleLow; set => _IdStumbleLow = value; }
    private int _IdStumbleCornerRight = Animator.StringToHash("StumbleCornerRight");
    public int IdStumbleCornerRight { get => _IdStumbleCornerRight; set => _IdStumbleCornerRight = value; }
    private int _IdStumbleCornerLeft = Animator.StringToHash("StumbleCornerLeft");
    public int IdStumbleCornerLeft { get => _IdStumbleCornerLeft; set => _IdStumbleCornerLeft = value; }
    private int _IdStumbleFall = Animator.StringToHash("StumbleFall");
    public int IdStumbleFall { get => _IdStumbleFall; set => _IdStumbleFall = value; }
    private int _IdStumbleOffLeft = Animator.StringToHash("StumbleOffLeft");
    public int IdStumbleOffLeft { get => _IdStumbleOffLeft; set => _IdStumbleOffLeft = value; }
    private int _IdStumbleOffRight = Animator.StringToHash("StumbleOffRight");
    public int IdStumbleOffRight { get => _IdStumbleOffRight; set => _IdStumbleOffRight = value; }
    private int _IdStumbleSideLeft = Animator.StringToHash("StumbleSideLeft");
    public int IdStumbleSideLeft { get => _IdStumbleSideLeft; set => _IdStumbleSideLeft = value; }
    private int _IdStumbleSideRight = Animator.StringToHash("StumbleSideRight");
    public int IdStumbleSideRight { get => _IdStumbleSideRight; set => _IdStumbleSideRight = value; }
    private int _IdDeathBounce = Animator.StringToHash("DeathBounce");
    public int IdDeathBounce { get => _IdDeathBounce; set => _IdDeathBounce = value; }
    private int _IdDeathLower = Animator.StringToHash("DeathLower");
    public  int IdDeathLower { get => _IdDeathLower; set => _IdDeathLower = value; }
    private int _IdDeathMovingTrain = Animator.StringToHash("DeathMovingTrain");
    public int IdDeathMovingTrain { get => _IdDeathMovingTrain; set => _IdDeathMovingTrain = value; }
    private int _IdDeathUpper = Animator.StringToHash("DeathUpper");
    public int IdDeathUpper { get => _IdDeathUpper; set => _IdDeathUpper = value; }

    private bool swipeLeft, swipeRight, swipeUp, swipeDown;

    [Header("Player States")]
    [SerializeField] private bool isJumping;

    [SerializeField] private bool _isRolling;
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }

    [SerializeField] private bool  _isGrounded;
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }

    [SerializeField]private bool _isCollisionZ = false;
    public bool IsCollisionZ { get => _isCollisionZ; set => _isCollisionZ = value; }

    [SerializeField]private bool _isGameOver = false;
    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }

    private void Start()
    {
        position = Side.Middle; 
        myTransform = GetComponent<Transform>(); 
        _animator = GetComponent<Animator>(); 
        _animator.enabled = false;
        _myCharacterController = GetComponent<CharacterController>(); 
        playerCollision = GetComponent<PlayerCollision>();
        yPosition = -7;
    }

    private void Update()
    {
        SetPlayerPosition();
        MovePlayer();
        Jump();
        Roll();
        _isGrounded = _myCharacterController.isGrounded;
    }

    public void GetSwipe() 
    {
        if (!_isGameOver)
        {
            swipeLeft = Input.GetKeyDown(KeyCode.A);
            swipeRight = Input.GetKeyDown(KeyCode.D);
            swipeUp = Input.GetKeyDown(KeyCode.Space);
            swipeDown = Input.GetKeyDown(KeyCode.S);
        }
        
    }

    private void SetPlayerPosition()
    {
        if (swipeLeft && !_isRolling) 
        {
            if (position == Side.Middle)
            {
                UpdatePlayerXPosition(Side.Left); 
                SetPlayerAnimator(_IdDogdgeLeft,false);
            }
            else if (position == Side.Right) 
            {
                UpdatePlayerXPosition(Side.Middle); 
                SetPlayerAnimator(_IdDogdgeLeft,false); 
            }
        }
        else if (swipeRight && !_isRolling) 
        {
            if (position == Side.Middle) 
            {
                UpdatePlayerXPosition(Side.Right); 
                SetPlayerAnimator(_IdDodgeRight,false); 
            }
            else if(position == Side.Left) 
            { 
                UpdatePlayerXPosition(Side.Middle); 
                SetPlayerAnimator(_IdDodgeRight,false);
            }
        }
    }

    public void UpdatePlayerXPosition(Side plPosition) 
    {                                                             
        newXPosition = (int)plPosition; 
        position = plPosition;    
    }

    public void SetPlayerAnimator(int id, bool isCrossFade, float fadeTime = 0.1f) 
    {
        _animator.SetLayerWeight(0, 1);
        if (isCrossFade)
        {
            _animator.CrossFadeInFixedTime(id, fadeTime);
        }
        else
        {
            _animator.Play(id);
        }
        ResetCollision();
    }

    public void SetPlayerAnimatorWithLayer(int id)
    {
        _animator.SetLayerWeight(1, 1);
        _animator.Play(id);
        ResetCollision();
    }

    private void ResetCollision()
    {
        Debug.Log(playerCollision.collisionX.ToString() + " "
                + playerCollision.collisionY.ToString() + " " +
                  playerCollision.collisionZ.ToString());

        playerCollision.collisionX = CollisionX.None;
        playerCollision.collisionY = CollisionY.None;
        playerCollision.collisionZ = CollisionZ.None;
    }

    private void MovePlayer()
    {
        xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed);
        motionVector = new Vector3(xPosition - myTransform.position.x,yPosition * Time.deltaTime,FordwardSpeed * Time.deltaTime);
        _myCharacterController.Move(motionVector);
    }

    private void Jump()
    {
        if (_myCharacterController.isGrounded)
        {
            isJumping = false;
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                SetPlayerAnimator(_IdLanding, false);

            if (swipeUp && !_isRolling)
            {
                isJumping = true;
                yPosition = jumpPower;
                SetPlayerAnimator(_IdJump,true);
            }
        }
        else
        {
            yPosition -= jumpPower * 2 * Time.deltaTime;
            if (_animator.velocity.x <= 0 && !IsCollisionZ)
                SetPlayerAnimator(_IdFall, false);
        }
    }

    private void Roll()
    {
        rollTimer -= Time.deltaTime;
        if (rollTimer <= 0)
        {
            _isRolling = false;
            rollTimer = 0;
            _myCharacterController.center = new Vector3(0,.45f,0);
            _myCharacterController.height = .9f;
        }
        if (swipeDown && !isJumping)
        {
            _isRolling = true;
            rollTimer = .5f;
            SetPlayerAnimator(_IdRoll,true);
            _myCharacterController.center = new Vector3(0, .2f, 0);
            _myCharacterController.height = .4f;
        }
    }
}
