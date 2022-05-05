using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerController : MonoBehaviour
{
    public ParticleSystem dust;
    public ParticleSystem dustDieWater;
    private Collider2D coll;
    public static PlayerController instance;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask force;
    [SerializeField] private string scenneName;
    [SerializeField] private GameObject startRaycast;
    [SerializeField] private GameObject Bin;
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
    private Rigidbody2D rb;
    private Rigidbody2D rbBin;
    private Animator anim;
    public float speed;
    public float jumpForce;
    public float sppedRotate;
    public  int starCount;
    bool checkleftRight;
    bool checkJump;
    public bool canMove = true;
    private int hp = 3;
    public bool isTouchStone = false;
    public int nextSceneLoad;
    public Button jumpBtn;
    public float InputHorizontal;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        starCount = 0;
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        rbBin = Bin.GetComponent<Rigidbody2D>();
        
    }
    void Update()
    {
       if(canMove) MoveByKeyboard();
       if(canMove) Move();
       CheckPushBin();
    }
    private void Move(){
        float dirX = CrossPlatformInputManager.GetAxis("Horizontal")* 6;
        InputHorizontal = dirX;
        Vector2 movement = new Vector2(dirX, 0);
        rb.AddForce(movement * speed);
        rb.AddTorque(-dirX*1.5f, 0);
        // jump
        // if(Input.GetKeyDown(KeyCode.Space)  && coll.IsTouchingLayers(ground) ){
        //     rb.AddForce(new Vector2(0, jumpForce));
        // }
          CheckParticle();
    }
    private void MoveByKeyboard(){
        // move left and right
        float moveHorizontal = Input.GetAxis("Horizontal");
        //InputHorizontal = moveHorizontal;
        Vector2 movement = new Vector2(moveHorizontal, 0);
      
        rb.AddForce(movement * speed);
        rb.AddTorque(-moveHorizontal*1.5f, 0);
        // jump
        //float moveVertical = Input.GetAxis ("Vertical");
        if((Input.GetKeyDown(KeyCode.Space) && coll.IsTouchingLayers(ground) )){
            //Vector2 jump = new Vector2(0, moveVertical);
            rb.AddForce(new Vector2(0, jumpForce));
            CreatrDust();
    }
        // particle system
        CheckParticle();
        //CheckJumpingParticle();
        //dust.Play();
    }
    public void Jump(){
        if(coll.IsTouchingLayers(ground)){
            rb.AddForce(new Vector2(0, jumpForce));
            CreatrDust();
        }
    }
     private void OnTriggerEnter2D(Collider2D other)
     {
            if(other.tag == TagConst.COLLECTABLE){
                Destroy(other.gameObject);
                starCount++;
                GameGUIManager.Ins.UpdatePowerBar(starCount, 3);
            }
            if(other.gameObject.tag == TagConst.WAVE){
                anim.SetTrigger("DieByWater");
                dustDieWater.Play();
                rb.gravityScale = 0.1f;
                canMove = false;
                Invoke("RestartScene", 3f);
            }
            if(other.gameObject.tag == "CantMove"){
                canMove = false;
                Invoke("NormalState", 1f);
            }
            if(other.gameObject.tag == TagConst.NEXTLEVEL){
                // Move to next level
                 SceneManager.LoadScene("Level" + nextSceneLoad);
                 // Setting Int for Index
                 if(nextSceneLoad > PlayerPrefs.GetInt("levelAt")){
                     PlayerPrefs.SetInt("levelAt", nextSceneLoad);
                 }

            }
            if(other.gameObject.tag == TagConst.DIE){
                Invoke("RestartScene",.5f);
            }
            
     }
     private void NormalState(){
         canMove = true;
     }
    private void OnCollisionEnter2D(Collision2D other)
     {
        if(rb.velocity.y < -0.1f){
            if(other.gameObject.tag == TagConst.ENEMY){
                anim.SetTrigger("Happy");
                Invoke("NormalStateAnimation", 1.5f);
                Enenmy.instance.AnimDie();
               rb.AddForce(new Vector2(0, jumpForce));
            }
        }
        else{
            if(other.gameObject.tag == TagConst.ENEMY){
                anim.SetTrigger("Hurt");
                Invoke("NormalStateAnimation", 2f);
                 if (other.gameObject.transform.position.x > transform.position.x)
                {
                    // enemy is right
                    rb.velocity = new Vector2(-8f, 12f);
                    
                }
                else
                {
                    // enenmy is left
                     rb.velocity = new Vector2(8f, 12f);
            
                }
                // giam mau
                hp--;
                GameGUIManager.Ins.CheckHp(hp);
                Invoke("CheckDie", 0.5f);
            }
        }
     }

  
     // ------Particle System------
     void CreatrDust(){
         dust.Play();
     }
     void NormalStateAnimation(){
         anim.SetTrigger("Normal");
     }
     void CheckLeftRight(){
         if(sppedRotate > 0){
                checkleftRight = true;
            }
            else if(sppedRotate < 0){
                checkleftRight = false;
         }
     }
     void CheckParticle()
     {
         Invoke("CheckLeftRight", 0.1f);
        if(coll.IsTouchingLayers(ground)){
             if (checkleftRight == true){
             if(sppedRotate < 0){
                    dust.Play();
             }
         }
         else if(checkleftRight == false){
             if(sppedRotate > 0){
                    dust.Play();
             }
         }
        }
     }
    void CheckJumping(){
        if(rb.velocity.y > 0){
            checkJump = true;
        }
        else if(rb.velocity.y <= 0){
            checkJump = false;
        }
    }
    void CheckJumpingParticle(){
        CheckJumping();
        if(checkJump == true){
            if(coll.IsTouchingLayers(ground)){
                dust.Play();
            }
        }
    }
    // ------end Particle System------
    void CheckDie(){
        if(hp <= 0){
            anim.SetTrigger("Die");
            canMove = false;
            rb.gravityScale = -1f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.eulerAngles = new Vector3(0, 0, 0);
    }}
    void DestroyPlayer(){
        Destroy(gameObject);
        RestartScene();
    }
    public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
   private void CheckPushBin(){
       int layerBin = 6;
       int layerMaskBin = 1 << layerBin;
       int layerWall = 8;
       int layerMaskWall = 1 << layerWall;

       RaycastHit2D hitBin = Physics2D.Raycast(startRaycast.transform.position, Vector2.down, 2f, layerMaskBin);
       Debug.DrawLine(startRaycast.transform.position, startRaycast.transform.position + Vector3.down*2f, Color.red);
       RaycastHit2D hitWall = Physics2D.Raycast(startRaycast.transform.position, Vector2.left, 2f, layerMaskWall);
       Debug.DrawLine(startRaycast.transform.position, startRaycast.transform.position + Vector3.left*2f, Color.green);
       if(hitBin.collider != null && hitWall.collider != null)
       {
           if(hitBin.collider.tag == TagConst.BIN && hitWall.collider.tag == TagConst.WALL){
               Debug.Log("Push Bin");
               if(InputHorizontal < 0){
                    Vector2 Movement = new Vector2(InputHorizontal, 0);
                    rbBin.AddForce(-Movement);
                    rb.AddTorque(-InputHorizontal, 0);
               }                
           }
       }
   }
}


