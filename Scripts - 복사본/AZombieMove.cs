using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AZombieMove : MonoBehaviour
{
    public float distance;
    public LayerMask isLayer;
    public float speed;
    public float atkDistance;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;
    public int HP = 3;
    public GameObject bullet1;
    public Transform pos;
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
    public float cooltime;
    private float currenttime;
    void FixedUpdate()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance,isLayer);
        Debug.DrawRay(transform.position, transform.right * -1 * distance, Color.red);
        if(raycast.collider != null)
        {            
            if(Vector2.Distance(transform.position, raycast.collider.transform.position)< atkDistance)
            {
                if(currenttime <= 0)
                {
                    GameObject bulletcopy = Instantiate(bullet1, pos.position, transform.rotation);
                    currenttime = cooltime;
                }
                
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, raycast.collider.transform.position, Time.deltaTime*speed);
            }
            currenttime -= Time.deltaTime;
            

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
