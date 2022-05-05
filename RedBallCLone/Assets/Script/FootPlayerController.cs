using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPlayerController : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y);
    }
     void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == TagConst.ENEMY){
            Enenmy.instance.AnimDie();
        }    
    }
}
