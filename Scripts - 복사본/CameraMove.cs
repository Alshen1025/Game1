using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPos = player.transform.position;
        transform.position = new Vector3(PlayerPos.x, PlayerPos.y, transform.position.z);
        
    }
}
