using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool space_down = false;
    public float jump_power = 1;
    private Vector3 movement;
    public bool in_air = false;
    public float jump_multiplier;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision theCollision)
    {
        if (theCollision.gameObject.CompareTag("Stop") && in_air)
        {
            movement = new Vector3(0.0f, 0.0f, 0.0f);
            rb.velocity = movement;
            rb.Sleep();
            in_air = false;
        }
    }

    void OnCollisionExit(Collision theCollision)
    {
        if (theCollision.gameObject.CompareTag("Stop"))
        {
            in_air = true;
        }
    }


    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movement = new Vector3(horizontal, 0f, vertical);

        MovePlayer();

        if (Input.GetKeyDown("space"))
        {
            space_down = true;
        }

        if(space_down){
            jump_power = jump_power + jump_multiplier;
        }

        if(Input.GetKeyDown("right")){
            movement = new Vector3(40.0f, 0.0f, 0.0f);
        }

        if(Input.GetKeyDown("left")){
            movement = new Vector3(-40.0f, 0.0f, 0.0f);
        }

        if(Input.GetKeyUp("right")){
            movement = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if(Input.GetKeyUp("left")){
            movement = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (Input.GetKeyUp("space") && space_down)
        {
            rb.AddForce(Vector3.up * jump_power, ForceMode.Impulse);
            rb.AddForce(movement * speed);
            jump_power = 0;
            space_down = false;
        }
    }

    private void MovePlayer()
    {
        Vector3 MovementVector = transform.TransformDirection(movement) * speed;
        rb.velocity = new Vector3(MovementVector.x, rb.velocity.y, MovementVector.z);
    }


}