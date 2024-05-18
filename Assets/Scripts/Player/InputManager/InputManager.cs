using System.Collections;
using UnityEngine;

namespace PlatformeGame2D
{
    public static class InputManager
    {
        static readonly string HorizontalInput = "Horizontal";

        static readonly string VerticalInput = "Vertical";
        static readonly string JumpInput = "Jump";
        static readonly string DashInput = "Dash";

        static readonly string GrabInput = "Grab";

        //设置的是静态类所以可以直接使用（类名.函数）
        //并且在动画层面，会使动画没有延迟感
        public static float HorizontalRaw()
        {

            //相比Input.GetAxis,Raw输入更加灵敏，移动时更加流畅
            return Input.GetAxisRaw(HorizontalInput);

        }

        public static float VerticalRaw()
        {
            return Input.GetAxisRaw(VerticalInput);
        }
        public static bool Jump()
        {
            return Input.GetButtonDown(JumpInput);
        }

        //检测冲刺输入
        public static bool Dash()
        {
            return Input.GetButtonDown(DashInput);
        }

        public static bool Grab()
        {


            return Input.GetButtonDown(GrabInput);
        }
    }
}