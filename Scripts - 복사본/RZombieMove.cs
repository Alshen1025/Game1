using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RZombieMove : MonoBehaviour
{
    public float distance;
    public LayerMask isLayer;
    public float speed;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;
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
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D forwardRaycast = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        RaycastHit2D backwardRaycast = Physics2D.Raycast(transform.position, -transform.right, distance, isLayer);
        if(forwardRaycast.collider != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, forwardRaycast.collider.transform.position, Time.deltaTime*speed);
            spriteRenderer.flipX = false;
        }else if (backwardRaycast.collider != null){
            transform.position = Vector3.MoveTowards(transform.position, backwardRaycast.collider.transform.position, Time.deltaTime*speed);
            spriteRenderer.flipX = true;
        }
        
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
        Destroy(gameObject,0.5f);
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
