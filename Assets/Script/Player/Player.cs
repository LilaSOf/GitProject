using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;

    private float InputX;
    private float InputY;
    private Vector2 movementInput;

    private Rigidbody2D rb;
    private bool IsMoving;
    private Animator[] animators;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }
    private void PlayerInput()
    {
        InputX = Input.GetAxisRaw("Horizontal");
        InputY = Input.GetAxisRaw("Vertical");
        if(InputX != 0 && InputY != 0)
        {
            InputX = InputX * 0.6f;
            InputY = InputY * 0.6f;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            InputX = InputX * 0.5f;
            InputY = InputY * 0.5f;
        }
        movementInput = new Vector2(InputX, InputY);
        IsMoving = movementInput != Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        SwitchAnimation();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * Speed * Time.deltaTime);
    }
    private void SwitchAnimation()
    {
        foreach (var animator in animators)
        {
            animator.SetBool("isMoving", IsMoving);
            if(IsMoving )
            {
                animator.SetFloat("InputX", InputX);
                animator.SetFloat("InputY", InputY);
            }
        }
    }

}
