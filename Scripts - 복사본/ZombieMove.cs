using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int HP = 3;
    AudioSource audioSource;
    public AudioClip ZombieDead;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        Invoke("Think",2);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down , 1 , LayerMask.GetMask("Platform"));
        if(rayHit.collider == null)
            Turn();
        
                
    }
    void Think(){
        nextMove = Random.Range(-1,2);

        float nextThinkTime = Random.Range(1f, 2f);
        Invoke("Think",nextThinkTime);

        anim.SetInteger("zWalkSpeed",nextMove);
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == -1;
            
    }
    void Turn(){
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == -1;
        CancelInvoke();
        Invoke("Think",2);
    }
    public void TakeDamage(int damage){
        if(HP != 0){
            HP = HP-damage;
            OnDamaged(transform.position);
        }
        else{
            anim.SetTrigger("Dead");
            killedZombie();
        }
        
    }
    void killedZombie(){
        Destroy(gameObject,.5f);
        audioSource.clip = ZombieDead;
        audioSource.Play();
        
    }
    void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 12;
        spriteRenderer.color = new Color(1,1,1,0.4f);

        Invoke("OffDamaged",0.25f);
    }
    void OffDamaged()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1,1,1,1);
    }
}
