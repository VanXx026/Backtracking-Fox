using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Collider2D coll; //人物下面的圆形碰撞体
    public Collider2D boxColl; //人物上面的方形碰撞体

    public LayerMask ground; //地面的Layer
    public Transform groundCheck; //地面检测
    public Transform cellCheck; //头顶检测

    public float moveSpeed = 10f;
    private float facedDir; //当前人物面朝的方向
    public float jumpForce = 10f;
    private bool jumpPressed; //是否按下了跳跃键
    private int jumpCount = 2; //跳跃次数
    public int cherryNum;

    //玩家状态
    private bool isGround; //是否站在地上
    private bool isJump; //是否在跳跃
    private bool isGetHit; //是否受伤
    private bool isCrouch; //是否蹲下
    private bool isDash; //是否冲刺
    private bool isWallFront; //面前是否存在墙
    private bool isWallSlide; //是否在墙上下滑
    private bool isWallJump; //是否在墙上跳跃

    [Space]

    // //音效
    // [Header("AudioSource")]
    // public AudioSource jumpAudio;
    // public AudioSource getHitAudio;
    // public AudioSource getCollectionsAudio;

    // [Space]

    // //Dash参数
    // [Header("DashControl")]
    // public float dashTime; //dash时长
    // public float dashTimeLeft; //dash剩余时间
    // public float lastDashTime = -30f; //上一次dash时间点，初始值设为负确保游戏一开始就可以执行冲刺，因为一开始的游戏时间是0
    // public float dashCoolDown; //dash冷却时间
    // public float dashSpeed; //dash速度

    //墙上下滑参数
    [Header("WallSlideControl")]
    public float wallSlideSpeed;
    public Transform frontCheck;

    [Header("RollBack参数")]
    public float rollBackTime;
    private float timer;
    public Color rollBackColor;
    public Transform rollBacks;
    public List<GameObject> rollBackList;
    private int i = 0;
    private GameObject rollBackPrefab;
    private Vector3 currentVelocity;
    public bool isRollBacking; // 是否在回溯
    public bool isTimeLine; // 是否在记录时间轴
    public bool isStartToRollBack; // 准备回溯
    public int timeLineSkillNum;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<CircleCollider2D>();

        timer = rollBackTime;
        rollBackPrefab = new GameObject();
        isRollBacking = false;
        isTimeLine = true;
    }

    private void Start()
    {
        cherryNum = FindObjectsOfType<Collections>().Length;
        timeLineSkillNum = LevelManager.instance.levelSkillList[LevelManager.instance.levelNow];
        UIManager.instance.skillText.text = string.Format("X {0}", timeLineSkillNum);
    }

    private void Update()
    {
        // FIXME:优化一下这里，到0的时候还是可以按
        // if (timeLineSkillNum > 0)
        // {
        if (Input.GetMouseButtonDown(1) && !isTimeLine && timeLineSkillNum >= 0) // 开启时间轴
        {
            isTimeLine = true;
            Camera.main.GetComponent<PostEffectGrey>().enabled = false;
            AudioManager.instance.PlaySkillClip();
        }
        else if (Input.GetMouseButtonDown(1) && isTimeLine && timeLineSkillNum > 0) // 关闭时间轴
        {
            isTimeLine = false;
            timeLineSkillNum--;
            UIManager.instance.skillText.text = string.Format("X {0}", timeLineSkillNum);
            Camera.main.GetComponent<PostEffectGrey>().enabled = true;
            AudioManager.instance.PlaySkillClip();
        }
        // }

        if (!isRollBacking && isTimeLine)
        {
            GenerateRollBack();
        }

        if (isStartToRollBack && !isRollBacking)
        {
            isRollBacking = true;
            RollBack();
        }

        if (isRollBacking)
        {
            return;
        }

        //这样优化跳跃手感的思想是：
        //在Update中逐帧调用，即时拿到用户的输入，在FixedUpdate中执行通过jumpPressed控制的Jump方法。
        //这样就可以做到既及时相应，又能物理地执行跳跃方法。
        //加上jumpCount计数之后可以解决跳跃过程中按了很多次空格然后落地瞬间还会再跳一次的问题
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }

        //下蹲Crouch
        // if (Input.GetButton("Crouch"))
        // {
        //     boxColl.enabled = false;
        //     isCrouch = true;
        // }
        // else
        // {
        //     if (Physics2D.OverlapCircle(cellCheck.position, 0.1f, ground))
        //     {
        //         boxColl.enabled = false;
        //         isCrouch = true;
        //     }
        //     else
        //     {
        //         boxColl.enabled = true;
        //         isCrouch = false;
        //     }
        // }

        // //冲刺Dash
        // if (Input.GetKeyDown(KeyCode.LeftShift) && facedDir != 0)
        // {
        //     if (Time.time >= lastDashTime + dashCoolDown) //如果游戏运行时间 大于等于 上次冲刺时间 + 冲刺冷却时间
        //     {
        //         //冲刺就绪
        //         ReadyToDash();
        //     }
        // }

        //FIXME:存在贴墙之后跳跃卡墙的问题
        //墙上下滑WallSlide
        // if (isWallFront && !isGround)
        //     isWallSlide = true;
        // else
        //     isWallSlide = false;

        // //墙上跳跃WallJump
        // if (isWallSlide && !isGround)
        //     isWallJump = true;
        // else
        //     isWallJump = false;

        //墙上跳跃WallJump
        if (isWallFront && !isGround)
            isWallJump = true;
        else
            isWallJump = false;
    }

    private void FixedUpdate()
    {
        if (isRollBacking)
        {
            return;
        }

        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground); //判断此时人物是否在地面上
        isWallFront = Physics2D.OverlapCircle(frontCheck.position, 0.1f, ground); //判断此时人物面前是否有墙

        WallSlide(); //墙上下滑
        WallJump(); //墙上跳跃
        // Dash(); //冲刺
        // //规定冲刺时不能跳跃和移动
        // if (isDash)
        //     return;
        GroundMovement(); //地面基本移动
        Jump(); //跳跃


        SwitchAnimation();
    }

    //地面基本移动
    private void GroundMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        facedDir = Input.GetAxisRaw("Horizontal"); //移动方向

        //移动
        if (!isGetHit)
            rb.velocity = new Vector2(moveSpeed * horizontal, rb.velocity.y);

        //从受击反弹中恢复过来
        if (rb.velocity.x == 0 && isGetHit)
        {
            isGetHit = false;
        }

        //移动方向改变
        if (facedDir != 0)
        {
            transform.localScale = new Vector3(facedDir, 1, 1);
        }
    }

    //实现多段跳
    private void Jump()
    {
        //如果是在地面上，初始化一下
        if (isGround)
        {
            jumpCount = 2; //二段跳
            isJump = false;
        }

        if(jumpPressed && isWallJump)
        {
            rb.velocity = Vector2.zero;
            rb.velocity = new Vector2(-transform.localScale.x * 10, jumpForce);
            isJump = true;
            AudioManager.instance.PlayJumpClip();
            jumpCount--; //已经跳了一次
            jumpPressed = false;
        }

        //要从地面上起跳
        else if (jumpPressed && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJump = true;
            // jumpAudio.Play(); //播放跳跃音效
            AudioManager.instance.PlayJumpClip();
            jumpCount--; //已经跳了一次
            jumpPressed = false;
        }
        //在空中继续跳
        else if (jumpPressed && jumpCount > 0 && !isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // jumpAudio.Play(); //播放跳跃音效
            AudioManager.instance.PlayJumpClip();
            jumpCount--;
            jumpPressed = false;
        }
    }

    // //下蹲
    // private void Crouch()
    // {
    //     isCrouch = true;
    //     boxColl.enabled = false;
    // }

    //切换动画
    //FIXME:失足下落的效果不好，要么只能跳一次，要么能够像跳跃一样正常的切换动画
    private void SwitchAnimation()
    {
        anim.SetFloat("Run", Mathf.Abs(rb.velocity.x));

        // anim.SetBool("Hit", isGetHit);

        // anim.SetBool("Crouch", isCrouch);

        if (isGround)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
        }
        else if (!isGround && rb.velocity.y > 0) //上升
        {
            anim.SetBool("Jump", true);
        }
        else if (rb.velocity.y < 0) //下落
        {
            //如果是跳跃中的下落
            if (isJump)
                anim.SetBool("Jump", false);

            anim.SetBool("Fall", true); //失足下落，未主动按跳跃时
        }
    }

    //碰撞触发
    private void OnTriggerEnter2D(Collider2D other)
    {
        //碰到collection
        if (other.CompareTag("Collection") && gameObject.tag == "RollBackPlayer")
        {
            // getCollectionsAudio.Play(); //播放吃到收集品时的音效
            AudioManager.instance.PlayGainClip();
            Destroy(other.gameObject);
        }
        //碰到DeadLine
        if (other.CompareTag("DeadLine"))
        {
            //TODO:游戏结束
            GameManager.instance.GameOver();
            UIManager.instance.OpenGameLosePanel();
        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         //人物在下落时
    //         if (rb.velocity.y < 0)
    //         {
    //             Destroy(other.gameObject); //把怪踩死
    //             rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    //             isJump = true;
    //             jumpCount--; //已经跳了一次
    //         }
    //         //受击
    //         else
    //         {
    //             isGetHit = true;
    //             getHitAudio.Play(); //播放受击动画
    //             rb.AddForce(new Vector2(-transform.localScale.x, 1) * 5, ForceMode2D.Impulse);
    //         }
    //     }
    // }

    // //冲锋就绪
    // private void ReadyToDash()
    // {
    //     isDash = true;

    //     lastDashTime = Time.time; //现在这个时间开始冲刺

    //     dashTimeLeft = dashTime; //冲刺剩余时间初始化
    // }

    //执行冲刺
    // private void Dash()
    // {
    //     if (isDash)
    //     {
    //         if (dashTimeLeft >= 0)
    //         {
    //             //如果在空中，给一个向上的力，使冲刺更实用
    //             if (!isGround)
    //                 rb.velocity = new Vector2(dashSpeed * facedDir, jumpForce / 2);
    //             else
    //                 rb.velocity = new Vector2(dashSpeed * facedDir, rb.velocity.y);

    //             dashTimeLeft -= Time.deltaTime;
    //             ShadowPool.Instance.GetFromPool();
    //         }
    //         else //冲刺结束
    //         {
    //             isDash = false;
    //         }
    //     }
    // }

    //执行墙上下滑逻辑
    private void WallSlide()
    {
        if (isWallSlide)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
        }
    }

    //执行墙上跳跃逻辑
    private void WallJump()
    {
        if (isWallJump)
        {
            jumpCount = 1;
        }
    }

    // 生成时间轴
    private void GenerateRollBack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            float distance = 0;
            if (rollBackList.Count > 0)
            {
                Vector3 startPos = rollBackList[rollBackList.Count - 1].transform.position;
                Vector3 endPos = transform.position;
                distance = (endPos - startPos).magnitude;
                if (distance > 2.0f)
                {
                    for (int i = 1; i < (int)distance; i += 2)
                    {
                        var rollbackSTE = Instantiate(rollBackPrefab, ((i / distance) * (endPos - startPos)) + startPos, Quaternion.identity);
                        rollbackSTE.transform.SetParent(rollBacks);
                        rollBackList.Add(rollbackSTE);
                    }
                }
            }
            var rollback = Instantiate(rollBackPrefab, transform.position, Quaternion.identity);
            rollback.name = $"rollback {i++}";
            rollback.transform.SetParent(rollBacks);
            rollBackList.Add(rollback);

            var sr = rollback.AddComponent<SpriteRenderer>();
            sr.sprite = spriteRenderer.sprite;
            sr.color = rollBackColor;
            sr.sortingLayerName = "Player";
            sr.flipX = transform.localScale.x == 1 ? false : true;
            timer = rollBackTime;
        }
    }

    // 回溯
    private void RollBack()
    {
        StartCoroutine(RollBackIE());
    }
    IEnumerator RollBackIE()
    {
        gameObject.tag = "RollBackPlayer";
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        anim.enabled = false;
        transform.localScale = Vector3.one;

        AudioManager.instance.PlayRollBackClip();

        while (rollBackList.Count != 0)
        {
            GameObject currentRollBack = rollBackList[rollBackList.Count - 1];
            if (currentRollBack.GetComponent<SpriteRenderer>())
            {
                spriteRenderer.sprite = currentRollBack.GetComponent<SpriteRenderer>().sprite;
                spriteRenderer.flipX = currentRollBack.GetComponent<SpriteRenderer>().flipX;
            }

            Vector3 startPos = transform.position;
            float timer = 0;
            while (timer <= 1.0f)
            {
                timer += (2.0f / rollBackTime) * Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, currentRollBack.transform.position, timer);
                yield return null;
            }
            rollBackList.Remove(currentRollBack);
            Destroy(currentRollBack);
        }
        gameObject.tag = "Player";
        isRollBacking = false;
        rb.isKinematic = false;
        anim.enabled = true;
        spriteRenderer.flipX = false;

        cherryNum = FindObjectsOfType<Collections>().Length;
        if (cherryNum == 0)
        {
            GameManager.instance.GameWin();
        }
        else
        {
            GameManager.instance.GameOver();
        }
    }

}
