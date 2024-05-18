
using System.Collections.Generic;
using PlatformeGame2D;
using Unity.Mathematics;
using UnityEngine;

public class Character_Factory : MonoBehaviour
{



    public void InstancePlayer(Player_E_C player_E_C)
    {

        if (player_E_C.player == null)
        {
            //当player被摧毁的时候，他的引用设置为null
            player_E_C.rigidbody_2D = null;
            player_E_C.m_anim = null;
            player_E_C.groundCheck = null;
            player_E_C.playerCollider = null;

            // GameObject playerPrefab = player_E_C.playerPrefab;
            // Transform player_Birth_Point = player_E_C.player_Birth_Display;
            //根据prefab生成的player重新获得实例，这个时候原来的引用是悬挂的，所以需要重新获取
            GameObject newPlayer = Instantiate(player_E_C.playerPrefab, player_E_C.player_Birth_Display.position, quaternion.identity);

            player_E_C.player = newPlayer;
            player_E_C.rigidbody_2D = player_E_C.player.GetComponent<Rigidbody2D>();
            player_E_C.m_anim = player_E_C.player.GetComponentInChildren<Animator>();
            player_E_C.groundCheck = player_E_C.player.transform.Find("GroundCheck");
            player_E_C.playerCollider = player_E_C.player.GetComponent<BoxCollider2D>();
            //代码中更新主角的朝向为右边，因为如果是倒着死的，就为false；

            //所以当使用函数的时候变量没有得到更新,会出现转向相反的问题
            player_E_C.m_facingRight = true;
            //重新设置玩家的侧面朝向
            player_E_C.m_playerSide = 1;
            //重新设置玩家在墙的哪边
            player_E_C.m_onWallSide = 0;




        }


    }

    public void DestoryPlayer(Player_E_C player_E_C)
    {
        if (player_E_C.playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || player_E_C.player.transform.position.y <= -20f)
        {



            //不设置为null，就会造成悬挂指针，导致后续复活的主角无法正常获取引用的组件
            //销毁并不意味着消失了，他的引用都还存在，所以我们需要销毁他的引用
            //如果不将引用设置为null，你的代码可能会尝试与一个不存在的对象进行交互,或不能进入语句进行操作

            if (player_E_C.player != null)
            {
                Destroy(player_E_C.player);


                player_E_C.player = null;
            }






        }
    }

}