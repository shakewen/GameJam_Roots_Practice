using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Air : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(Wall());
    }

    
    void Update()
    {
        
    }

    IEnumerator Wall()
    {
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);
    }
    
}
