using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

namespace PlatformeGame2D
{
    public static class Player_Domain
    {


        public static void checkGrounded(Player_E_C player_E_C)
        {

            //是否在地面上
            player_E_C.isGrounded = Physics2D.OverlapCircle(player_E_C.groundCheck.position, player_E_C.groundCheckRadius, player_E_C.whatIsGround);
        }

        public static void CheckWallGrab(Player_E_C player_E_C)
        {
            var position = player_E_C.player.transform.position;
            // 检测角色是否在墙上
            player_E_C.m_onWall = Physics2D.OverlapCircle((Vector2)position + player_E_C.grabRightOffset, player_E_C.grabCheckRadius, player_E_C.whatIswall)
                        || Physics2D.OverlapCircle((Vector2)position + player_E_C.grabLeftOffset, player_E_C.grabCheckRadius, player_E_C.whatIswall);
            player_E_C.m_onRightWall = Physics2D.OverlapCircle((Vector2)position + player_E_C.grabRightOffset, player_E_C.grabCheckRadius, player_E_C.whatIswall);
            player_E_C.m_onLeftWall = Physics2D.OverlapCircle((Vector2)position + player_E_C.grabLeftOffset, player_E_C.grabCheckRadius, player_E_C.whatIswall);
        }

        public static void WalkJumpingAndJumping(Player_E_C player_E_C)
        {
            // // 如果在抓墙并且墙壁跳跃，则重置墙壁跳跃标志
            // if ((player_E_C.m_wallGrabbing || player_E_C.isGrounded) && player_E_C.m_wallJumping)
            // {
            //     player_E_C.m_wallJumping = false;
            // }

            // 如果当前可玩
            if (player_E_C.isCurrentlyPlayable)
            {
                // 如果能够正常移动且没挂在墙壁上
                if (player_E_C.canMove && !player_E_C.m_wallGrabbing)
                    player_E_C.rigidbody_2D.velocity = new Vector2(player_E_C.moveInput * player_E_C.speed, player_E_C.rigidbody_2D.velocity.y);
                //如果不能正常移动，设置跳跃参数
                else if (!player_E_C.canMove)
                    player_E_C.rigidbody_2D.velocity = new Vector2(0f, player_E_C.rigidbody_2D.velocity.y);


                // 当跳跃结束下降时
                if (player_E_C.rigidbody_2D.velocity.y < 0f)
                {
                    float maxFallSpeed = -11.5f; // 设置最大下落速度,下降的值太大会导致物理判定失效
                    //float gravityScale = 6f; // 设置重力系数
                    player_E_C.rigidbody_2D.velocity = new Vector2(player_E_C.rigidbody_2D.velocity.x, Mathf.Max(player_E_C.rigidbody_2D.velocity.y, maxFallSpeed)); // 限制下落速度
                    //player_E_C.rigidbody_2D.gravityScale = gravityScale; // 设置重力系数
                }



            }


        }

        public static void CalculateSides(Player_E_C player_E_C)
        {
            if (player_E_C.m_onRightWall)
                player_E_C.m_onWallSide = 1;
            else if (player_E_C.m_onLeftWall)
                player_E_C.m_onWallSide = -1;
            else
                player_E_C.m_onWallSide = 0;

            if (player_E_C.m_facingRight)
                player_E_C.m_playerSide = 1;
            else
                player_E_C.m_playerSide = -1;
        }
        //无需在main调用
        public static void Flip(Player_E_C player_E_C)
        {
            player_E_C.m_facingRight = !player_E_C.m_facingRight;
            Vector3 scale = player_E_C.player.transform.localScale;
            scale.x *= -1;
            player_E_C.player.transform.localScale = scale;
        }

        public static void MoveToFlip(Player_E_C player_E_C)
        {
            if (player_E_C.isCurrentlyPlayable)
            {
                if (player_E_C.canMove && !player_E_C.m_wallGrabbing)
                {
                    // 翻转角色
                    if ((!player_E_C.m_facingRight && player_E_C.moveInput > 0f) ||
                        (player_E_C.m_facingRight && player_E_C.moveInput < 0f))
                    {
                        Flip(player_E_C);
                    }
                }

            }

        }

        public static void Dashing(Player_E_C player_E_C)
        {
            if (player_E_C.isCurrentlyPlayable)
            {
                // 处理冲刺逻辑
                if (player_E_C.isDashing)
                {
                    // 如果冲刺时间小于等于0，则结束冲刺
                    if (player_E_C.m_dashTime <= 0f)
                    {
                        player_E_C.isDashing = false;
                        player_E_C.m_dashCooldown = player_E_C.dashCooldown;
                        player_E_C.m_dashTime = player_E_C.startDashTime;
                        player_E_C.rigidbody_2D.velocity = Vector2.zero;
                    }
                    else
                    {
                        // 减少冲刺时间，并根据角色面向方向设置速度
                        player_E_C.m_dashTime -= Time.deltaTime;
                        if (player_E_C.m_facingRight)
                            player_E_C.rigidbody_2D.velocity = Vector2.right * player_E_C.dashSpeed;
                        else
                            player_E_C.rigidbody_2D.velocity = Vector2.left * player_E_C.dashSpeed;
                    }
                }
            }
        }

        public static void IsGrabing(Player_E_C player_E_C)
        {

            if (player_E_C.isCurrentlyPlayable)
            {
                // 处理墙壁抓取
                if (InputManager.Grab() && player_E_C.m_onWall && player_E_C.m_playerSide == player_E_C.m_onWallSide)
                {
                    player_E_C.isGrab = !player_E_C.isGrab;



                }
                else if (!player_E_C.m_onWall)
                {
                    player_E_C.isGrab = false;
                }

                if (player_E_C.isGrab && player_E_C.m_onWall && player_E_C.m_playerSide == player_E_C.m_onWallSide)
                {

                    // 开始抓墙
                    player_E_C.actuallyWallGrabbing = true;
                    player_E_C.m_wallGrabbing = true;
                    // player_E_C.rigidbody2D.velocity = new Vector2(player_E_C.moveInput * player_E_C.speed, -player_E_C.slideSpeed);

                    player_E_C.m_wallStick = player_E_C.m_wallStickTime;
                    if (player_E_C.m_wallGrabbing)
                    {
                        // 设置向上或向下的速度以模拟爬墙
                        player_E_C.rigidbody_2D.velocity = new Vector2(0, InputManager.VerticalRaw() * player_E_C.slideSpeed);
                    }
                }
                else
                {

                    // 结束抓墙
                    player_E_C.m_wallStick -= Time.deltaTime;
                    player_E_C.actuallyWallGrabbing = false;
                    //恢复正常的移动逻辑
                    player_E_C.rigidbody_2D.velocity = new Vector2(player_E_C.moveInput * player_E_C.speed, player_E_C.rigidbody_2D.velocity.y);

                    if (player_E_C.m_wallStick <= 0f)
                        player_E_C.m_wallGrabbing = false;
                }
            }


        }

        //Update内执行
        public static void HandleDashInput(Player_E_C player_E_C)
        {
            // 处理冲刺输入
            if (!player_E_C.isDashing && !player_E_C.m_hasDashedInAir && player_E_C.m_dashCooldown <= 0f)
            {
                // 检测冲刺输入
                if (InputManager.Dash())
                {
                    player_E_C.isDashing = true;
                    // // 播放冲刺特效
                    // PoolManager.instance.ReuseObject(dashEffect, transform.position, Quaternion.identity);
                    // 如果角色在空中，则标记为在空中冲刺
                    if (!player_E_C.isGrounded)
                    {
                        player_E_C.m_hasDashedInAir = true;
                    }
                    // 冲刺逻辑在 FixedUpdate 中处理
                }
            }
            player_E_C.m_dashCooldown -= Time.deltaTime;

            // 如果角色在空中冲刺过一次但现在在地面上
            if (player_E_C.m_hasDashedInAir && player_E_C.isGrounded)
            {
                player_E_C.m_hasDashedInAir = false;
            }
        }

        public static void HandleJumpInput(Player_E_C player_E_C)
        {

            // 处理跳跃输入
            if (InputManager.Jump() && (player_E_C.isGrounded || player_E_C.m_groundedRemember > 0f)) // 正常单次跳跃
            {
                // 设置垂直速度为跳跃力量
                player_E_C.rigidbody_2D.velocity = new Vector2(player_E_C.rigidbody_2D.velocity.x, player_E_C.jumpForce);
                // // 播放跳跃特效
                // PoolManager.instance.ReuseObject(jumpEffect, groundCheck.position, Quaternion.identity);
            }
            // else if (InputManager.Jump() && player_E_C.m_wallGrabbing && player_E_C.moveInput != player_E_C.m_onWallSide) // 在墙壁时跳跃
            // {
            //     // 结束抓墙
            //     player_E_C.m_wallGrabbing = false;
            //     player_E_C.m_wallJumping = true;
            //     Debug.Log("Wall jumped");
            //     // 根据角色面向方向进行翻转
            //     if (player_E_C.m_playerSide == player_E_C.m_onWallSide)
            //         Flip(player_E_C);
            //     // 添加墙壁跳跃的力量
            //     player_E_C.rigidbody2D.AddForce(new Vector2(-player_E_C.m_onWallSide * player_E_C.wallJumpForce.x, player_E_C.wallJumpForce.y), ForceMode2D.Impulse);
            // }
            // else if (InputManager.Jump() && player_E_C.m_wallGrabbing && player_E_C.moveInput != 0 && (player_E_C.moveInput == player_E_C.m_onWallSide)) // 跳跃到墙壁上
            // {
            //     // 结束抓墙
            //     player_E_C.m_wallGrabbing = false;
            //     player_E_C.m_wallJumping = true;
            //     Debug.Log("Wall climbed");
            //     // 根据角色面向方向进行翻转
            //     if (player_E_C.m_playerSide == player_E_C.m_onWallSide)
            //         Flip(player_E_C);
            //     // 添加墙壁攀爬的力量
            //     player_E_C.rigidbody2D.AddForce(new Vector2(-player_E_C.m_onWallSide * player_E_C.wallClimbForce.x, player_E_C.wallClimbForce.y), ForceMode2D.Impulse);
            // }
        }
        public static void player_Animation(Player_E_C player_E_C)
        {
            if (player_E_C.isCurrentlyPlayable)
            {
                player_E_C.m_anim.SetBool(player_E_C.anim_IsQuit, false);
                
                player_E_C.m_anim.SetFloat(player_E_C.anim_MoveId, MathF.Abs(player_E_C.rigidbody_2D.velocity.x));

                //攀爬
                if (player_E_C.actuallyWallGrabbing)
                {
                    player_E_C.m_anim.SetBool(player_E_C.anim_WallGrabbingId, true);
                }
                else
                {
                    player_E_C.m_anim.SetBool(player_E_C.anim_WallGrabbingId, false);
                }

                if (!player_E_C.isGrounded && !player_E_C.actuallyWallGrabbing)
                {
                    player_E_C.m_anim.SetBool(player_E_C.anim_IsJumpingId, true);
                }
                else
                {
                    player_E_C.m_anim.SetBool(player_E_C.anim_IsJumpingId, false);
                }


                float verticalVelocity = player_E_C.rigidbody_2D.velocity.y;
                player_E_C.m_anim.SetFloat(player_E_C.anim_JumpStateId, verticalVelocity);

                player_E_C.m_anim.SetBool(player_E_C.anim_IsDashingId, player_E_C.isDashing);
            }
            
            

        }




        //处理主角在碰到敌人后的逻辑
        public static void GameOver(Player_E_C player_E_C)
        {
            if (player_E_C.playerCollider.IsTouchingLayers(LayerMask.GetMask("WinPlace")))
            {
                
                if (player_E_C.player != null)
                {
                    
                    
                    player_E_C.UI_win.SetActive(true);
                    player_E_C.isCurrentlyPlayable = false;
                    GameObject winplace = GameObject.FindWithTag("Finish");
                    winplace.SetActive(false);
                    player_E_C.m_anim.SetBool(player_E_C.anim_IsQuit, true);
                    
                }
            }
        }

        
    }

}