using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float distance;
    public LayerMask isLayer;
    public float speed;
    public PlayerMove player;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet",2);
    }
    void Awake()
    {
        player = FindObjectOfType<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {   
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance,isLayer);
        if(raycast.collider != null)
        {
            if(raycast.collider.tag == "Player")
            {
                Vector2 targetPos = raycast.collider.transform.position;
                player.OnDamaged(targetPos);
            }
            DestroyBullet();
        }
        transform.Translate(transform.right * -1f * speed * Time.deltaTime);
        
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
