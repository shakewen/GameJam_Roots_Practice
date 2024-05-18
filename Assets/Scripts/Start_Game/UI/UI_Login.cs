using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Login : MonoBehaviour
{
   private Button button;
    private void Start()
    {
        button = GetComponent<Button>(); // 获取UI_Login对象上的Button组件
        //button.onClick.AddListener(OnButtonClick); // 添加点击事件监听，点击时调用OnButtonClick函数
    }

    public void OnButtonClick()
    {
        // 在此处添加跳转到相应场景的代码
        SceneManager.LoadScene("Gaming");
    }
}