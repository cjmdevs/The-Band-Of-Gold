using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{   

    public float speed;

    private bool isChasing;
    public Rigidbody2D rb;
    private Transform player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isChasing == true){
             Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player"){
            if (player == null) {
                player = collision.transform;
            }
            
            isChasing = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision){
         if (collision.gameObject.tag == "Player"){
            rb.velocity = Vector2.zero;
            isChasing = false;
        }
    }
}
