/*
Written by Olusola Olaoye
Copyright © 2020
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
public class PlayerStamina : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio_source;
    public float max_stamina = 100f;
    public float current_stamina
    {
        get;
        set;
    }
    public FirstPersonController fps
    {
        get
        {
            return gameObject.GetComponent<FirstPersonController>();
        }
    }
    void Start()
    {
        current_stamina = max_stamina;
    }

    void Update()
    {
        switch(fps.run_state)
        {
            case FirstPersonController.RunState.crouch:
                current_stamina += 1 * Time.deltaTime;
                break;

            case FirstPersonController.RunState.run:
                current_stamina -= 16 * Time.deltaTime;
                break;

            case FirstPersonController.RunState.walk:
                current_stamina += 0.009f * Time.deltaTime;
                break;

            case FirstPersonController.RunState.Idle:
                current_stamina += 3 * Time.deltaTime;
                break;
        }
        if(current_stamina < 20 )
        {
            if(!audio_source.isPlaying)
                audio_source.Play();
        }
        else
        {
            audio_source.Pause();
        }
        current_stamina = Mathf.Clamp(current_stamina, 0, max_stamina);
    }
}
