using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    private PlayerInput _input;
    private CharacterController controller;
    public bool isGravityActive = true;
    public float verticalVelocity;
    private float gravity = -9.8f;
    private PlayerStateMachine stateMachine;
    public bool isAllowedToClimb = true;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    public void ProhibitClimb(float duration)
    {
        StartCoroutine(ProhibitClimbCoroutine(duration));
    }

    IEnumerator ProhibitClimbCoroutine(float duration)
    {
        isAllowedToClimb = false;
        Debug.Log($"Prohibit {isAllowedToClimb}");
        yield return new WaitForSeconds(duration);
        isAllowedToClimb = true;
        Debug.Log($"Release prohibit {isAllowedToClimb}");
    }

    private void Update()
    {
        UseGravity();
    }

    private void UseGravity()
    {
        if (isGravityActive)
        {
            verticalVelocity += gravity * Time.deltaTime;
            controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        }
    }
}
