/*
Written by Olusola Olaoye
Copyright © 2020
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FirstPersonController : MonoBehaviour
{
    public enum RunState
    {
        walk,
        run,
        crouch,
        Idle
    }
    public RunState run_state
    {
        get;
        set;
    }
    [SerializeField] [Range(0,30)]
    private float movement_speed;

    [SerializeField] [Range(0,10)]
    private float turn_Speed;

    public Camera camera_;

    private const float run_multiplier = 1.5f;
    private float run_speed;
    private bool is_running;
    private const float crouch_multiplier = 0.4f;
    private const float tired_multiplier = 0.5f;

    private float crouch_speed;
    private CharacterController cc;

    public CharacterController character_Controller
    {
        get
        {
            return cc;
        }
    }
    private float vertical_velocity;

    [SerializeField] private AudioSource walk_audio;
    [SerializeField] private AudioClip walk_clip1;

    // advust chracter controller height via these values
    public readonly float
        bend_height = 4.5f, stand_height = 10f, jump_height = 1.2f, gravity_scale = 0.28f;

    private bool airborne;

    void Start ()
    {
        cc = gameObject.GetComponent<CharacterController>();
        run_speed = movement_speed * run_multiplier;
        crouch_speed = movement_speed * crouch_multiplier;
        airborne = true;
	}
	
    public void movePlayer(Vector3 direction, Quaternion look, bool jump, bool bend)
    {
        // if you pressed run button and are moving and you have the stamina to run
        is_running = FindObjectOfType<PlayerInput>().run && direction.magnitude > 0 && gameObject.GetComponent<PlayerStamina>().current_stamina > 5;

        // essentially, movement vector is assigned according to whether the player is running, bending, or is tired or just walking
        Vector3 move_vector = new Vector3(direction.x *
            (bend ? crouch_speed : (is_running ?
            run_speed :
            gameObject.GetComponent<PlayerStamina>().current_stamina <= 0 ?
            movement_speed * Time.deltaTime * tired_multiplier :
            movement_speed))
            * Time.deltaTime, 0, direction.z *
            (bend ? crouch_speed :
            (is_running ? run_speed : movement_speed))
            * Time.deltaTime);

        transform.rotation = Quaternion.Euler(look.eulerAngles.x, look.eulerAngles.y, 0);
        move_vector = transform.rotation * move_vector;

        if (!cc.isGrounded){
            vertical_velocity += Physics.gravity[1] * Time.deltaTime * gravity_scale;
        }

        if (is_running)
        {
            run_state = RunState.run;
        }

        else if (bend && move_vector.magnitude > 0) run_state = RunState.crouch;
        else if (move_vector.magnitude > 0) run_state = RunState.walk;
        else if (move_vector.magnitude == 0) run_state = RunState.Idle;


        if (bend) { cc.height = bend_height; }
        else { cc.height = stand_height; }

        if (cc.isGrounded && airborne && !walk_audio.isPlaying){
            walk_audio.Play();
            airborne = false;
        }

        if (jump && cc.isGrounded)
        {
            vertical_velocity = jump_height;
            airborne = true;
        }

        Vector3 character_move = new Vector3(move_vector.x, vertical_velocity, move_vector.z);
        cc.Move(character_move);
        // adjust audio pitch which also affects audio tempo
        // this is based on whether player is running walking or bending
        if (cc.isGrounded && !walk_audio.isPlaying && move_vector.magnitude > 0.1f)
        {
            walk_audio.clip = walk_clip1;
            walk_audio.volume = Random.Range(0.19f, 0.39f);
            walk_audio.pitch = is_running ? Random.Range(1.5f, 1.75f) :
                bend ? Random.Range(0.96f, 1.3f) : Random.Range(1.3f, 1.5f);
            walk_audio.Play();
        }
    }   
    
    public void setJump(float v)
    {
        vertical_velocity = v;
    }
}