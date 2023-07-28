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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        movementInput = new Vector2(InputX, InputY);
    }
    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * Speed * Time.deltaTime);
    }
}
