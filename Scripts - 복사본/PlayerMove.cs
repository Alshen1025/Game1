using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameControl gameControl;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector2 boxSize;
    CapsuleCollider2D capsuleCollider;


    public AudioClip Jump;
    public AudioClip Item;
    public AudioClip Attack;
    public AudioClip GameOver;
    public AudioClip ZombieHit;
    AudioSource audioSource;
    

    // Start is called before the first frame update
    void Awake()    
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gameControl = FindObjectOfType<GameControl>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        //점프
        if(Input.GetButtonDown("Jump") && anim.GetBool("isJump") != true){
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
            audioSource.clip = Jump;
            audioSource.Play();
        }
        //정지 시 속도
        if (Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f,rigid.velocity.y);
        }

        //방향전환
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        //걷기 애니메이션
        if(Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalk", false);
        else
            anim.SetBool("isWalk", true);

        //공격
        if(curTime <= 0)
        {
            if(Input.GetKey(KeyCode.Z)){
                anim.SetTrigger("atk");
                audioSource.clip = Attack;
                audioSource.Play();
                curTime = coolTime;
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position,boxSize,0);
                foreach(Collider2D collider in collider2Ds)
                {
                    Debug.Log(collider.tag);
                    if(collider.tag == "Zombie"){
                        audioSource.clip = ZombieHit;
                        audioSource.Play();
                        collider.GetComponent<ZombieMove>().TakeDamage(1);
                    }
                    else if(collider.tag =="RZombie"){
                        audioSource.clip = ZombieHit;
                        audioSource.Play();
                        collider.GetComponent<RZombieMove>().TakeDamage(1);
                    }
                    else if(collider.tag =="AZombie"){
                        audioSource.clip = ZombieHit;
                        audioSource.Play();
                        collider.GetComponent<AZombieMove>().TakeDamage(1);
                    }
                    else if(collider.tag =="BZombie"){
                        audioSource.clip = ZombieHit;
                        audioSource.Play();
                        collider.GetComponent<BZombie>().TakeDamage(1);
                    }
                }
            
            }
    
        
        }
        else{
            curTime -= Time.deltaTime;
        }
        
        
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(pos.position,boxSize);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");//이동 속도
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed)//최대 속력
            rigid.velocity = new Vector2(maxSpeed,rigid.velocity.y);
        else if(rigid.velocity.x < maxSpeed*(-1))
            rigid.velocity = new Vector2(maxSpeed*(-1),rigid.velocity.y);
        
        //착지
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));
            RaycastHit2D rayHitPlatform = Physics2D.Raycast(rigid.position, Vector3.down , 1 , LayerMask.GetMask("Platform"));
            RaycastHit2D rayHitObject = Physics2D.Raycast(rigid.position, Vector3.down , 1 , LayerMask.GetMask("Object"));
            if(rayHitPlatform.collider != null||rayHitObject.collider != null){
                if(rayHitPlatform.distance <  0.7f || rayHitObject.distance <  0.7f)
                    anim.SetBool("isJump",false);
            }
            
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Zombie"){
            if(gameObject.layer != 11){
                Vector2 targetPos = collision.transform.position;
                OnDamaged(targetPos);
            }
        }else if(collision.gameObject.tag == "RZombie"){
            if(gameObject.layer != 11){
                Vector2 targetPos = collision.transform.position;
                OnDamaged(targetPos);
            }

        }else if(collision.gameObject.tag == "BZombie"){
            if(gameObject.layer != 11){
                Vector2 targetPos = collision.transform.position;
                OnDamaged(targetPos);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Finish")
        {
            gameControl.NextStage();
        }
        else if(collision.gameObject.tag == "item")
        {
            if (gameControl.playerLife < 5){
                audioSource.clip = Item;
                audioSource.Play();
                gameControl.LifeUp();
                collision.gameObject.SetActive(false);
            }
        }
    }
    
    public void OnDamaged(Vector2 targetPos)
    {
        gameControl.LifeDown();
        gameObject.layer = 11;
        spriteRenderer.color = new Color(1,1,1,0.4f);
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc,1)*10 ,ForceMode2D.Impulse);

        Invoke("OffDamaged" , 3);
    }
    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1,1,1,1);
    }

    
    public void Die(){
        audioSource.clip = GameOver;
        audioSource.Play();
        Time.timeScale =0;
    }
    public void VelocityZero(){
        rigid.velocity = Vector2.zero;
    }
}
