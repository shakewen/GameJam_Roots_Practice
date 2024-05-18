using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_GameOver : MonoBehaviour
{
    private Button gameOverButton;

    
    
    void Start()
    {
        gameOverButton = GetComponent<Button>();
        //gameOverButton.onClick.AddListener(Gameover);
        
    }
    
    public void Gameover()
    {
        Debug.Log("退出程序成功");
        Application.Quit();
        
        Resources.UnloadUnusedAssets();
    }

    
}