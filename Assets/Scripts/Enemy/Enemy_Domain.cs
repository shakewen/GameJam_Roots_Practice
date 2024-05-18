using System.Runtime.InteropServices;
using PlatformeGame2D;
using UnityEngine;

public class Enemy_Domain: MonoBehaviour
{
    [Header("敌人")]
    

    

    //public Collider2D enemyCollider;

    public float enemySpeed;

    public Transform enemyMovePoint;

    public Vector2 enemyMovePosition;


    void Start()
    {
        //不要设置静态敌人的tag！
        
        // enemyStatic = GameObject.FindGameObjectWithTag("EnemyStatic").gameObject;
        //enemyCollider = enemy.GetComponent<Collider2D>();
        enemyMovePoint = transform.Find("EnemyMovePoint").transform;
        enemyMovePoint.parent = null;

        enemyMovePosition = transform.position;
        enemySpeed = 1.1f;
    }

    void FixedUpdate()
    {
        EnemyMove();
        OnQuit();
    }

    public void EnemyMove()
    {

        
            //     player_E_C.enemy.transform.position = Vector2.Lerp(player_E_C.enemy.transform.position, player_E_C.enemyMovePoint.position,
            // Time.fixedDeltaTime * player_E_C.enemySpeed);
            transform.position = Vector2.MoveTowards(transform.position, enemyMovePoint.position,
            Time.fixedDeltaTime * enemySpeed);


            if (Vector2.Distance(transform.position, enemyMovePoint.position) <= 0.001f)
            {
                var position = enemyMovePosition;
                enemyMovePoint.position = position;
                enemyMovePosition = transform.position;

            }
        

    }

    public void OnQuit()
    {
        GameObject Manager = GameObject.Find("GameManager");
        if(Manager== null)
        {
            Destroy(this);
        }
        
    }


}