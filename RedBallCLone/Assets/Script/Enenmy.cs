using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enenmy : MonoBehaviour
{
     [SerializeField] private float left;
    [SerializeField] private float right;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float speedRotation;
    [SerializeField] private Collider2D coll;
    bool isTouchBin = false;
    bool isTouchWall = false;
    bool isTouchStone =false;
  
    private Collider2D coli;
    private Rigidbody2D rb;
    private Animator anim;
    private bool _isMoving = false;
    public Vector3 anchor;
    
    private bool facingLeft = true;

    // Raycast
    [SerializeField] private GameObject startRaycast;
    [SerializeField] private GameObject Bin;
    [SerializeField] private GameObject Stone;
  



    public static Enenmy instance;
     private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {     
        CheckDestroy();
        if(_isMoving){
            return;
        }
        Move();
        if( isTouchBin && facingLeft){
            facingLeft = false; 
            left = Bin.transform.position.x + 1f;
        }
        if(isTouchStone && facingLeft){
            StartCoroutine(Roll((Vector3.right)));
            facingLeft = false;
            left = Stone.transform.position.x + 1f;
        }
         
    }
    
    private void NormalState(){
        isTouchBin = false;
        isTouchWall = false;
        isTouchStone = false;
    }
    bool moveLeft;
    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > left ) 
            {
               StartCoroutine(Roll((Vector3.left)));
            }
          
           
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < right )
            {
                  StartCoroutine(Roll((Vector3.right)));
                  
            }
            else
            {
                facingLeft = true;
            }
        }
    }
     IEnumerator Roll(Vector3 direction)
    {
            _isMoving = true;
            float remainingAngle = 90;
            Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down /2;
            Vector3 rotationAxis =   Vector3.Cross(Vector3.up, direction);

            while (remainingAngle > 0){
                float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
                transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
                remainingAngle -= rotationAngle;
                yield return null;
            }
            _isMoving = false;
    }
    
         
       
    private void CheckDestroy(){
        int layerBin = 6;
        int layerMaskBin = 1 << layerBin;
        int layerWall = 8;
        int layerMaskWall = 1 << layerWall;

        //RaycastHit2D[] hitWall = Physics2D.RaycastAll(startRaycast.transform.position,  Vector2.right,10f);
        RaycastHit2D hitWall = Physics2D.Raycast(startRaycast.transform.position,  Vector2.right,.5f, layerMaskWall);
       Debug.DrawLine(startRaycast.transform.position,startRaycast.transform.position+ Vector3.right*10f, Color.red);
        RaycastHit2D hitBin = Physics2D.Raycast(startRaycast.transform.position, Vector2.left, .5f, layerMaskBin);
        Debug.DrawLine(startRaycast.transform.position,startRaycast.transform.position+ Vector3.left*10f, Color.green);
        RaycastHit2D hitStone = Physics2D.Raycast(startRaycast.transform.position, Vector2.right, .5f, layerMaskBin);

        RaycastHit2D hitUp = Physics2D.Raycast(startRaycast.transform.position, Vector2.up, .5f, layerMaskBin);
        Debug.DrawLine(startRaycast.transform.position,startRaycast.transform.position+ Vector3.up*10f, Color.blue);
        RaycastHit2D hitDown = Physics2D.Raycast(startRaycast.transform.position, Vector2.down, .5f, layerMaskBin);
        Debug.DrawLine(startRaycast.transform.position,startRaycast.transform.position+ Vector3.down*10f, Color.yellow);
        
        // Check Enemy bi ket giua Bin vaf Wall
        if(hitBin.collider != null && hitWall.collider != null){
             if(hitBin.collider.tag == TagConst.BIN && hitWall.collider.tag == TagConst.WALL){
                AnimDie();
            }
        }

        // Check Enemy bi Stone de chet
        if(hitUp.collider != null && hitDown.collider != null){
            if(hitUp.collider.tag == TagConst.STONE && hitDown.collider.tag == TagConst.GROUND){
                AnimDie();
            }
        }

        if(hitBin.collider != null){
            if(hitBin.collider.tag == TagConst.BIN){
                isTouchBin = true;
                Invoke("NormalState", .1f);
                Debug.Log("Touch Bin");
            }
            else if(hitBin.collider.tag == TagConst.STONE){
                isTouchStone = true;
                Invoke("NormalState", .1f);
                Debug.Log("Touch Stone");
            }
        }
        if(hitWall.collider != null){
              if(hitWall.collider.tag == TagConst.WALL){
            isTouchWall = true;
            Invoke("NormalState", .1f);
            Debug.Log("Touch Wall");
        }
        }
        if(hitStone.collider != null){
            if(hitStone.collider.tag == TagConst.STONE){
                AnimDie();
            }
        }
        
    }
    public void AnimDie(){
        anim.SetTrigger("Die");
    }
    public void DestroyEnemy(){
        Destroy(this.gameObject);
    }
    
    

   
       
    
}
