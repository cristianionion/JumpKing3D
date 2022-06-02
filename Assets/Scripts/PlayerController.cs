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
    public AudioSource grass_walking;
    public AudioSource grass_landing;
    public float power;
    public float jump_power = 0;

    private bool isWalking = false;
    private bool Squat = false;
    private bool space_down = false;
    private bool isGrass = true;
    private Vector3 last_velocity;
    private Vector3 movement;
    private bool in_air = false;
    private Rigidbody rb;
    private bool playing;

    public TextMeshProUGUI timer;
    private float startTime = 0f;
    private float currentTime = 0f;
    Animator m_Animator;

    public AudioSource solid_landing;
    public AudioSource solid_walking;
    private bool canAudioPlay = false;
    

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
            isGrass = false;
            in_air = false;
            if (canAudioPlay == true){
                solid_landing.time = 0.2f;
                solid_landing.Play ();
            }
            m_Animator.SetBool("inAir", in_air);
        }

        if (theCollision.gameObject.CompareTag("Grass") && in_air)
        {
            isGrass = true;
            in_air = false;
            if (canAudioPlay == true){
                grass_landing.time = 0.2f;
                grass_landing.Play ();
            }
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
            canAudioPlay = true;
            m_Animator.SetBool("inAir", in_air);
        }

        if (theCollision.gameObject.CompareTag("Grass"))
        {
            in_air = true;
            canAudioPlay = true;
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

        if(isWalking && !in_air){
            if(isGrass){
                if(!playing){
                    grass_walking.Play ();
                    playing = true;
                }
            }else{
                if(!playing){
                    solid_walking.Play ();
                    playing = true;
                }
            }
        }

        if(!isWalking){
            if(isGrass){
                grass_walking.Stop ();
                playing = false;
            }else{
                solid_walking.Stop ();
                playing = false;
            }
        }


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
            grass_walking.Stop ();
            solid_walking.Stop ();
            playing = false;
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

        
        if (transform.position.y <0.3f){
            //transform.position = new Vector3 (0.0f,0.5f,0.0f);
        }
        
    }



}