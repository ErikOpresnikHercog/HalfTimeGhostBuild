using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput controls;

    

    Vector2 move;

    public float moveSpeed = 5f;

    public Rigidbody2D Rb;

    public Animator animator;

    public DialogueManager dialogueManager;


    Vector2 movement;
    private void Awake()
    {
        controls = new PlayerInput();
        dialogueManager = FindObjectOfType<DialogueManager>();
        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Confirm.performed += ctx => dialogueManager.canDialogue = true;
        controls.Gameplay.Confirm.canceled += ctx => dialogueManager.canDialogue = false;
        controls.Gameplay.Confirm.performed += _ => dialogueManager.ShowBubble();
        

    }

    void Move()
    {
        print("Yes");
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = move.x;
        movement.y = move.y;
        gameObject.GetComponentInChildren<PlayerVacuum>().x = movement.x;
        gameObject.GetComponentInChildren<PlayerVacuum>().y = movement.y;
        animator.SetFloat("horizontal", movement.x);
        animator.SetFloat("vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }
    void FixedUpdate()
    {
        if(dialogueManager.isDialogue == false)
        {
            Rb.MovePosition(Rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
