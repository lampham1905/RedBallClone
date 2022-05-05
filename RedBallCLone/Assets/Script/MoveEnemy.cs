using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //MoveLeft();
        for(float i = 0; i <= 90; i+= Time.deltaTime){
                 transform.Rotate(0, 0, i);
             }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private  void Roll(){
       
             for(float i = 0; i <= 90; i+= Time.deltaTime*Time.deltaTime){
                 transform.Rotate(0, 0, i);
             }
         
    }
    IEnumerator MoveLeft(){
         yield return new WaitForSeconds(0.1f);
        Roll();
    }
}
