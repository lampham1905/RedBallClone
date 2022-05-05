using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrallaxBackGround : MonoBehaviour
{
    private float length, startpos;
    public GameObject Player;
    public float  parallaxEffect;
   private void Start()
  {
    startpos = transform.position.x;
    length = GetComponent<SpriteRenderer>().bounds.size.x;   
  }

  void Update(){
      float temp = (Player.transform.position.x * (1 - parallaxEffect));
      float dist = (Player.transform.position.x * parallaxEffect);
      transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);  
      if(temp > startpos + length) startpos += length;
      else if(temp < startpos - length) startpos -= length;  
  }
}
