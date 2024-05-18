using Unity.Mathematics;
using UnityEngine;

namespace PlatformeGame2D
{

    // 玩家实体类
    public class Player_E_C : MonoBehaviour
    {

        

        [Header("主角")]
        public bool isGrab = false;
        //主角
        public GameObject player;
        // 主角预制体
        public GameObject playerPrefab;

        public Collider2D playerCollider;
        // 速度
        public float speed;
        // 刚体组件
        public Rigidbody2D rigidbody_2D;
        // 是否面朝右侧
        public bool m_facingRight = true;

        // 在地面上的记忆时间
        public float m_groundedRememberTime = 0.25f;

        // 在地面上的记忆
        public float m_groundedRemember = 0f;

        // 冲刺时间
        public float m_dashTime;

        // 是否在空中进行过冲刺
        public bool m_hasDashedInAir = false;

        // 是否在墙上
        public bool m_onWall = false;

        // 是否在右墙上
        public bool m_onRightWall = false;

        // 是否在左墙上
        public bool m_onLeftWall = false;

        // 是否正在抓墙
        public bool m_wallGrabbing = false;

        // 抓墙的粘滞时间
        public readonly float m_wallStickTime = 0.25f;

        // 抓墙的粘滞时间计数
        public float m_wallStick = 0f;

        // 是否正在墙壁跳跃
        public bool m_wallJumping = false;

        // 冲刺冷却时间
        public float m_dashCooldown;

        // 角色在墙壁的哪一侧
        public int m_onWallSide;

        // 玩家侧面
        public int m_playerSide;


        [Header("动画")]
        // 用于动画处理和其他用途的公共变量
        [HideInInspector] public Animator m_anim;

        //储存起来id方便快速读取，也可以用string来直接使用
        public readonly int anim_MoveId = Animator.StringToHash("Move");
        public readonly int anim_JumpStateId = Animator.StringToHash("JumpState");
        public readonly int anim_IsJumpingId = Animator.StringToHash("IsJumping");
        public readonly int anim_WallGrabbingId = Animator.StringToHash("WallGrabbing");
        public readonly int anim_IsDashingId = Animator.StringToHash("IsDashing");
        
        public readonly int anim_IsQuit = Animator.StringToHash("IsQuit");
        [HideInInspector] public bool isGrounded; // 是否在地面上
        [HideInInspector] public float moveInput; // 角色的水平输入
        [HideInInspector] public bool canMove = true; // 是否可以移动
        [HideInInspector] public bool isDashing = false; // 是否正在冲刺
        [HideInInspector] public bool actuallyWallGrabbing = false; // 是否实际抓墙
        [HideInInspector] public bool isCurrentlyPlayable = false; // 是否当前可玩

        // 跳跃相关配置
        [Header("跳跃")] // Unity 编辑器中显示的标题
        public float jumpForce; // 跳跃力量
        [SerializeField] public float fallMultiplier; // 掉落时的重力乘数
        [HideInInspector] public Transform groundCheck; // 检测角色是否在地面上的 Transform
        [SerializeField] public float groundCheckRadius; // 地面检测的半径
        [SerializeField] public LayerMask whatIsGround; // 哪些层被认为是地面

        [SerializeField] public LayerMask whatIswall;
        [SerializeField] public int extraJumpCount = 1; // 额外的跳跃次数
        [SerializeField] public GameObject jumpEffect; // 跳跃时的特效

        // 冲刺相关配置
        [Header("冲刺")]
        [SerializeField] public float dashSpeed; // 冲刺速度
        [SerializeField] public float startDashTime = 0.1f; // 冲刺开始的时间
        [SerializeField] public float dashCooldown; // 冲刺冷却时间
        [SerializeField] public GameObject dashEffect; // 冲刺时的特效

        // 抓墙和跳跃相关的配置
        public Vector2 grabRightOffset; // 抓墙检测右边界的偏移量
        public Vector2 grabLeftOffset; // 抓墙检测左边界的偏移量
        public float grabCheckRadius = 0.24f; // 抓墙检测的半径
        public float slideSpeed = 1.5f; // 滑行速度
        public Vector2 wallJumpForce = new Vector2(10.5f, 18f); // 墙壁跳跃的力量
        public Vector2 wallClimbForce = new Vector2(4f, 14f); // 墙壁攀爬的力量




        public Transform player_Birth_Display;

        [Header("摄像机")]
        public Camera main_camera;
        public float smoothSpeed;
        public Vector3 offset;
        [Header("Camera bounds")]
        public Vector3 minCamerabounds;
        public Vector3 maxCamerabounds;

 
        //UI临时获取
        public GameObject UI_win;

        public void SetupAndGetComponent()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            UI_win = GameObject.FindGameObjectWithTag("UI_win");
            playerPrefab = Resources.Load<GameObject>(@"Prefabs/Players/Player");
            player_Birth_Display = GameObject.FindGameObjectWithTag("Player_Birth_Display").transform;

            //  cameraFollow.target = player.gameObject.transform;

            m_dashTime = startDashTime;
            m_dashCooldown = dashCooldown;
            //角色在墙壁的哪一侧
            m_onWallSide = 0;
            //玩家侧面
            m_playerSide = 1;


            //==== 初始化基础设置 ====

            //==== 跳跃 ====
            speed = 3f;
            jumpForce = 13.5f;
            fallMultiplier = 1.2f;

            groundCheckRadius = 0.2f;
            whatIsGround = LayerMask.GetMask("Ground");
            whatIswall = LayerMask.GetMask("Wall");
            extraJumpCount = 1;

            //==== 冲刺 ====
            dashSpeed = 12.5f;
            startDashTime = 0.15f;
            dashCooldown = 0.5f;
            grabRightOffset = new Vector2(0.06f, 0f);
            grabLeftOffset = new Vector2(-0.06f, 0f);
            grabCheckRadius = 0.2f;//0.24f
            slideSpeed = 2.5f;
            wallJumpForce = new Vector2(5.5f, 13f);
            wallClimbForce = new Vector2(4f, 14f);





            rigidbody_2D = player.GetComponent<Rigidbody2D>();
            
            m_anim = player.GetComponentInChildren<Animator>();
            //之前一直在这段代码报错，上一段代码出现问题就会导致后面的代码就运行不了
            groundCheck = player.transform.Find("GroundCheck").transform;

            playerCollider = player.GetComponent<Collider2D>();

            //==== 摄像机 ====
            main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            smoothSpeed = 0.08f;
            offset = new Vector3(0f, 1.5f, -10f);
            minCamerabounds = new Vector3(-0.14f, -10f, -10f);
            maxCamerabounds = new Vector3(14f, 10f, 10f);

            //==== UI ====
            UI_win.SetActive(false);
        }







    }
}