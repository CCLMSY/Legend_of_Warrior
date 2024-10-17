using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    
    public PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Character character;

    public Vector2 inputDirection;

    [Header("基本参数")]
    public float speed;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    public float jumpForce;
    public float wallJumpForce;
    public float hurtForce;
    public float slideDistance;
    public float slideSpeed;

    public int slidePowerCost;

    private Vector2 oriOffset;
    private Vector2 oriSize;

    [Header("材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        character = GetComponent<Character>();

        inputControl = new PlayerInputControl();

        oriOffset = coll.offset;
        oriSize = coll.size;

        // 跳跃
        inputControl.GamePlay.Jump.started += Jump;

        #region 行走
        runSpeed = speed;
        inputControl.GamePlay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isGrounded) speed = walkSpeed;
        };
        inputControl.GamePlay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGrounded) speed = runSpeed;
        };
        #endregion

        // 攻击
        inputControl.GamePlay.Attack.started += PlayerAttack;

        //滑铲
        inputControl.GamePlay.Slide.started += PlayerSlide;
        
        inputControl.Enable();
    }

    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += OnLoadRequestEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadRequestEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }
    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();

        CheckState();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isAttack && !isSlide) Move();
    }

    private void OnLoadDataEvent()
    {
        isDead = false;
        inputControl.GamePlay.Enable();
    }

    // 场景加载时禁用输入
    private void OnLoadRequestEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.GamePlay.Disable();
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputControl.GamePlay.Enable();
    }
    private void Move()
    {
        if (!isCrouch && !wallJump)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        

        // 人物翻转
        int faceDirection = inputDirection.x > 0 ? 1 : inputDirection.x < 0 ? -1 : (int)transform.localScale.x;
        transform.localScale = new Vector3(faceDirection, 1, 1);

        // 蹲下
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGrounded;
        if (isCrouch)
        {
            // 修改碰撞体大小
            coll.offset = new Vector2(coll.offset.x, 0.8f);
            coll.size = new Vector2(coll.size.x, 1.6f);
        }
        else
        {
            // 恢复碰撞体大小
            coll.offset = oriOffset;
            coll.size = oriSize;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGrounded){
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            
            // 打断滑铲协程
            isSlide = false;
            StopAllCoroutines();
        }
        if (physicsCheck.onWall){
            rb.AddForce(new Vector2(-inputDirection.x, 2.7f)*wallJumpForce,ForceMode2D.Impulse);
            wallJump = true;
        }
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if(physicsCheck.onWall) return;
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    private void PlayerSlide(InputAction.CallbackContext context)
    {
        if(!isSlide && physicsCheck.isGrounded && !(physicsCheck.touchLeftWall || physicsCheck.touchRightWall) && character.curPower>=slidePowerCost){
            isSlide = true;
            var targetPos = new Vector3(transform.position.x + transform.localScale.x * slideDistance, transform.position.y);
            
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            StartCoroutine(TriggerSlide(targetPos));

            character.OnSlide(slidePowerCost);
        }
    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do{
            yield return null;
            if (!physicsCheck.isGrounded)
                break;

            //滑铲过程中撞墙
            if ((physicsCheck.touchLeftWall || physicsCheck.touchRightWall) && Mathf.Abs(transform.localScale.x) > 0f)
            {
                isSlide = false;
                break;
            }

            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
        }while(Mathf.Abs(transform.position.x - target.x) > 0.1f);

        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();
    }
    #endregion
    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGrounded ? normal : wall;

        if(physicsCheck.onWall) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);

        if(wallJump && rb.velocity.y < 0f) wallJump = false;
    }
}
