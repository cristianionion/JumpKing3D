using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jump_multiplier;
    public float min_power;
    public float max_power;

    private bool isWalking = false;
    private bool Squat = false;
    private bool space_down = false;
    public float jump_power = 0;
    private Vector3 last_velocity;
    private Vector3 movement;
    private bool in_air = false;
    private Rigidbody rb;
    public float power;

    public TextMeshProUGUI timer;
    private float startTime = 0f;
    private float currentTime = 0f;
    Animator m_Animator;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator> ();
    }
    //stop the player when they land on a platform
    void OnCollisionEnter(Collision theCollision)
    {
        if (theCollision.gameObject.CompareTag("Stop") && in_air)
        {
            //movement = new Vector3(0.0f, 0.0f, 0.0f);
            //rb.velocity = movement;
            //rb.Sleep();
            in_air = false;
            m_Animator.SetBool("inAir", in_air);
        }

        if (theCollision.gameObject.CompareTag("Bounce") && in_air)
        {
            Vector3 reflect = Vector3.Reflect(last_velocity, Vector3.right);
            rb.velocity = reflect;
        }
    }
    //Set the player to in air when they jump
    void OnCollisionExit(Collision theCollision)
    {
        if (theCollision.gameObject.CompareTag("Stop"))
        {
            in_air = true;
            m_Animator.SetBool("inAir", in_air);
        }
    }

    float Power(float power){
        return (1 - power) * min_power + power * max_power;
    }

    public float turnSpeed = 20.0f;


    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

 
    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);


        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

    }

    void OnAnimatorMove()
    {
        rb.MovePosition(rb.position + m_Movement * m_Animator.deltaPosition.magnitude * 2);
        rb.MoveRotation(m_Rotation);
    }

    void Update()
    {
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        //movement = new Vector3(horizontal, 0f, vertical);

        //MovePlayer();


        // Displays game time for player
        if (startTime <= 0){
            currentTime += Time.deltaTime;
            int minutes = (int)currentTime / 60;
            int seconds = (int)currentTime %60; 
            if (seconds < 10){
                timer.text = "Time Elapsed: " +minutes.ToString()+":0"+seconds.ToString();
            }
            else{
                timer.text = "Time Elapsed: "+ minutes.ToString()+":"+ seconds.ToString();
            }

            
        }

        last_velocity = rb.velocity;
        //See if player is trying to jump
        if (Input.GetKeyDown("space"))
        {
            space_down = true;
        }
        //while player is holding space add power
        if(space_down){
            jump_power = jump_power + jump_multiplier;
            Squat = true;
            isWalking = false;
            m_Animator.SetBool("Squat", Squat);
            m_Animator.SetBool("IsWalking", isWalking);
        }
        //if player presses the right key then jump right
        if(Input.GetKeyDown("right")){
            movement = new Vector3(350.0f, 0.0f, 0.0f);
        }
        //if player presses the left key then jump left
        if(Input.GetKeyDown("left")){
            movement = new Vector3(-350.0f, 0.0f, 0.0f);
        }
        //if player releases the right key then dont jump that way anymore
        if(Input.GetKeyUp("right")){
            movement = new Vector3(0.0f, 0.0f, 0.0f);
        }
        //if player releases the left key then dont jump that way anymore
        if(Input.GetKeyUp("left")){
            movement = new Vector3(0.0f, 0.0f, 0.0f);
        }
        //when the player releases the space bar then jump up and to whatever direction the wanted
        if (Input.GetKeyUp("space") && space_down && !in_air)
        {
            if(jump_power > 1){
                jump_power = 1;
            }

            power = 0;
            power = Power(jump_power);
            rb.AddForce(Vector3.up * Power(jump_power), ForceMode.Impulse);
            rb.AddForce(movement * speed);
            jump_power = 0;
            space_down = false;
            Squat = false;
            m_Animator.SetBool("Squat", Squat);
            
        }

        // player movement when on the ground
        // if (Input.GetKey("left") && !space_down && !in_air){
        //     movement = new Vector3(-6.0f,0.0f,0.0f);
        //     //rb.velocity =  movement;
        //     rb.AddForce(movement*speed);
        // }
        // if (Input.GetKey("right") && !space_down && !in_air){
        //     movement = new Vector3(6.0f,0.0f,0.0f);
        //     //rb.velocity = movement;
        //     rb.AddForce(movement*speed);
        // }
        // // stops ball from rolling on the ground if there is no left or right input
        // if (Input.GetKeyUp("left") && !in_air && !space_down){
        //     rb.velocity = new Vector2 (-2,0);
        // }
        // if (Input.GetKeyUp("right") && !in_air && !space_down ){
        //     rb.velocity = new Vector2 (2,0);
        // }
        // if player falls through ground floor, reset position
        if (transform.position.y <0.3f){
            //transform.position = new Vector3 (0.0f,0.5f,0.0f);
        }
        
    }

    /*private void MovePlayer()
    {
        Vector3 MovementVector = transform.TransformDirection(movement) * speed;
        rb.velocity = new Vector3(MovementVector.x, rb.velocity.y, MovementVector.z);
    }*/


}