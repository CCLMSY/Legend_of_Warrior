using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    private PlayerController playerController;
    private Rigidbody2D rb;
    [Header("检测参数")]
    public bool autoBottom; // 以bottom为基准点的对象，自动设置监测点
    public bool isPlayer;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRadius;
    public LayerMask groundLayer;
    [Header("状态")]
    public bool isGrounded;
    public bool touchLeftWall;
    public bool touchRightWall;
    public bool onWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        if (autoBottom)
        {
            leftOffset = new Vector2(coll.offset.x - coll.size.x / 2, coll.size.y / 2);
            rightOffset = new Vector2(coll.offset.x + coll.size.x / 2, coll.size.y / 2);
            bottomOffset = new Vector2(coll.offset.x, 0);
            checkRadius = 0.1f;
        }

        if (isPlayer){
            playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
        }
    }
    private void Update()
    {
        Check();
    }
    private void Check()
    {
        // 检测是否在地面上
        if(onWall)
            isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRadius, groundLayer);
        else 
            isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), checkRadius, groundLayer);


        // 检测是否碰到墙壁
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x * transform.localScale.x, leftOffset.y), checkRadius, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x * transform.localScale.x, rightOffset.y), checkRadius, groundLayer);
        // 检测是否在墙壁上
        if(isPlayer) onWall = (touchLeftWall  || touchRightWall) && Mathf.Abs(playerController.inputDirection.x) > 0.1f && rb.velocity.y < 0f;
    }

    private void OnDrawGizmosSelected() // 在Scene视图中绘制检测范围
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x * transform.localScale.x, leftOffset.y), checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x * transform.localScale.x, rightOffset.y), checkRadius);
    }
}
