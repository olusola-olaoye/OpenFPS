/*
Written by Olusola Olaoye
Copyright © 2020
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(FirstPersonController))]
public partial class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    public static PlayerInput Instance
    {
        get
        {
            return instance;
        }
    }

    // are we controlling player or computer system
    public enum ControlState
    {
        Player,
        SystemOS,
        MissileSystem,
        Chopper
    }
    public ControlState control_state
    {
        get;
        set;
    }

    float horizontal;
    float vertical;
    float rotation_up_down;
    float rotation_left_right;

    private EventSystem event_system
    {
        get
        {
            return FindObjectOfType<EventSystem>();
        }
    }
    public Vector3 move_vector
    {
        get;
        private set;
    }
    public Vector3 look_vector
    {
        get;
        private set;
    }
    private FirstPersonController fps;


    // player actions
    public bool jump{get; private set;}
    public bool bend{get;private set;}
    public bool shoot{ get; private set;}
    public bool reload { get;private set;}
    public bool interact { get; private set; }
    public bool disinteract { get; private set;}
    public bool run{get;private set;}
    public bool escape{ get;private set;}

    private const float
         max_look = 50, min_look = -40;

    [SerializeField] [Range(1, 6)]
    private float scroll_sensitivity = 1;

    void Start ()
    {
        if(instance == null){
            instance = this;
        }
        fps = gameObject.GetComponent<FirstPersonController>();  
	}
	void Update ()
    {
        pcControl();
    }

    void pcControl()
    {
        jump = Input.GetButton("Jump");
        bend = Input.GetKey(KeyCode.C);
        shoot = Input.GetAxis("Fire1") > 0;
        reload = Input.GetAxis("Fire2") > 0;
        interact = Input.GetKeyDown(KeyCode.E);
        disinteract = Input.GetKeyDown(KeyCode.T);
        run = Input.GetKeyDown(KeyCode.V);
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        escape = Input.GetKeyDown(KeyCode.Escape);

        pcTurn();
        move_vector = new Vector3(horizontal, 0, vertical);
        fps.movePlayer(move_vector, Quaternion.Euler(look_vector), jump, bend);
    }

    private void pcTurn()
    {
        rotation_up_down += -(Input.GetAxis("Mouse Y") * scroll_sensitivity);
        rotation_left_right += (Input.GetAxis("Mouse X") * scroll_sensitivity);
        rotation_up_down = Mathf.Clamp(rotation_up_down, min_look, max_look);
        look_vector = new Vector3(rotation_up_down, rotation_left_right, 0);
    }
}
