using System.Collections;
using System.Collections.Generic;
using PlatformeGame2D;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{




    // CameraFollowUpdate 方法用于更新摄像机跟随玩家角色的位置
    public void CameraFollowUpdate(Player_E_C player_E_C)
    {
        if (player_E_C.player == null)
        {

            // 如果玩家角色为空，则通过标签查找玩家对象


            player_E_C.player = GameObject.FindGameObjectWithTag("Player").gameObject;


        }
        else
        {
            // 计算摄像机跟随的期望位置
            Vector3 desiredPosition = player_E_C.player.transform.localPosition + player_E_C.offset;
            var localPosition = player_E_C.main_camera.transform.localPosition;
            // 使用线性插值平滑摄像机移动
            Vector3 smoothedPosition = Vector3.Lerp(localPosition, desiredPosition, player_E_C.smoothSpeed);
            localPosition = smoothedPosition;

            // // 将摄像机位置限制在最小和最大边界之间
            // localPosition = new Vector3(
            //     Mathf.Clamp(localPosition.x, player_E_C.minCamerabounds.x, player_E_C.maxCamerabounds.x),
            //     Mathf.Clamp(localPosition.y, player_E_C.minCamerabounds.y, player_E_C.maxCamerabounds.y),
            //     Mathf.Clamp(localPosition.z, player_E_C.minCamerabounds.z, player_E_C.maxCamerabounds.z)
            //     );
            player_E_C.main_camera.transform.localPosition = localPosition;
        }
    }

    // public void SetTarget(Transform targetToSet)
    // {
    //     target = targetToSet;
    // }
}
