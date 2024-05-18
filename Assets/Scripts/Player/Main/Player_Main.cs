using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformeGame2D
{
    public class Player_Main : MonoBehaviour
    {
        public Player_E_C player_Main_Ctx;


        Character_Factory character_Factory;

        CameraFollow cameraFollow;

        void Start()

        {
            //生成的是一个全新的实例化的对象，而不是引用，这会导致使用的对象和Player_Main的对象不同


            player_Main_Ctx = GetComponent<Player_E_C>();

            character_Factory = new Character_Factory();

            cameraFollow = new CameraFollow();

            //直接找子物体效率高
            player_Main_Ctx = transform.GetComponent<Player_E_C>();

            player_Main_Ctx.SetupAndGetComponent();

        }


        float restDT = 0;

        void Update()
        {

            float deltaTime = Time.deltaTime;

            float fixedDeltaTime = Time.fixedDeltaTime;
            restDT += deltaTime;
            if (restDT >= fixedDeltaTime)
            {
                while (restDT >= fixedDeltaTime)
                {
                    restDT -= fixedDeltaTime;
                    UpdateConduct();
                }
            }
            else
            {
                UpdateConduct();
                restDT = 0;
            }



        }

        void FixedUpdate()
        {

            float deltaTime = Time.deltaTime;

            float fixedDeltaTime = Time.fixedDeltaTime;
            restDT += deltaTime;
            if (restDT >= fixedDeltaTime)
            {
                while (restDT >= fixedDeltaTime)
                {
                    restDT -= fixedDeltaTime;
                    fixedUpdateConduct();
                }
            }
            else
            {
                fixedUpdateConduct();
                restDT = 0;
            }

        }

        void UpdateConduct()
        {
            if (Time.timeScale != 0)
            {
                if (player_Main_Ctx.player.gameObject != null && player_Main_Ctx.UI_win.activeSelf == false)
                {
                    player_Main_Ctx.isCurrentlyPlayable = true;
                }
                //处理水平输入
                player_Main_Ctx.moveInput = InputManager.HorizontalRaw();

                // 该方法（UPdate）会立刻停止，不是返回上一个代码语句，而是跳出方法，重新执行该方法，直到为True为止
                if (!player_Main_Ctx.isCurrentlyPlayable) return;

                Player_Domain.HandleDashInput(player_Main_Ctx);
                Player_Domain.HandleJumpInput(player_Main_Ctx);
                Player_Domain.IsGrabing(player_Main_Ctx);
                //动画处理
                Player_Domain.player_Animation(player_Main_Ctx);



                //把来回移动迁移到每个敌人身上，因为不想统计了很麻烦
                //Enemy_Domain.EnemyMove(player_Main_Ctx);



                character_Factory.DestoryPlayer(player_Main_Ctx);
                character_Factory.InstancePlayer(player_Main_Ctx);

                Player_Domain.GameOver(player_Main_Ctx);
                if (player_Main_Ctx.UI_win.activeSelf == true)
                {

                    Destroy(this.gameObject);

                }
            }

        }

        void fixedUpdateConduct()
        {
            if (player_Main_Ctx.isCurrentlyPlayable)
            {
                //摄像机跟随
                cameraFollow.CameraFollowUpdate(player_Main_Ctx);
                Player_Domain.checkGrounded(player_Main_Ctx);
                Player_Domain.CalculateSides(player_Main_Ctx);
                Player_Domain.CheckWallGrab(player_Main_Ctx);
                Player_Domain.WalkJumpingAndJumping(player_Main_Ctx);
                Player_Domain.MoveToFlip(player_Main_Ctx);
                Player_Domain.Dashing(player_Main_Ctx);
            }
        }

    }
}
