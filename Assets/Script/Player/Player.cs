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
    private bool InputDisable = false;

    private float MouseX;
    private float MouseY;

    private bool useTool;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }
    private void OnEnable()
    {
        EventHandler.BeforeFade += OnBeforFade;
        EventHandler.AfterFade += OnAfterFade;
        EventHandler.MouseClickEvent += OnMouseClickEvenet;
        EventHandler.GameStateControllerEvent += OnGameStateControllerEvent;
    }
    private void OnDisable()
    {
        EventHandler.BeforeFade -= OnBeforFade;
        EventHandler.AfterFade -= OnAfterFade;
        EventHandler.MouseClickEvent -= OnMouseClickEvenet;
        EventHandler.GameStateControllerEvent -= OnGameStateControllerEvent;
    }

    private void OnGameStateControllerEvent(GameState gameState)
    {
       switch(gameState)
        {
            case GameState.Start:
                InputDisable = false;
                break;
            case GameState.Pasue:
                InputDisable = true;
                break;
        }
    }

    private void OnAfterFade(string SceneName)
    {
        InputDisable = false;
    }

    private void OnBeforFade(string SceneName)
    {
        InputDisable = true;
    }
    private void OnMouseClickEvenet(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        //执行动画，完毕之后执行逻辑内容
        if (itemDetails.itemType != ItemType.Furniture&& itemDetails.itemType != ItemType.Seed && itemDetails.itemType != ItemType.Commodity )
        {
            MouseX = mouseWorldPos.x - transform.position.x;
            MouseY = mouseWorldPos.y - transform.position.y;
            if (Mathf.Abs(MouseX) > Mathf.Abs(MouseY))
                MouseY = 0;
            else
                MouseX = 0;
           StartCoroutine(UseToolRoutine(mouseWorldPos, itemDetails));
        }
        else
        {
            EventHandler.CallExcuteActionAfterAnimation(mouseWorldPos, itemDetails);
        }
    }
    private IEnumerator UseToolRoutine(Vector3 mouseWorldPos, ItemDetails itemDetails)
    {
        useTool = true;
        InputDisable = true;
        yield return null;
        foreach (var anim in animators)
        {
            anim.SetTrigger("useTool");
            anim.SetFloat("mouseX", MouseX);
            anim.SetFloat("mouseY", MouseY);
        }
        yield return new WaitForSeconds(0.5f);
        InputDisable = false;
        EventHandler.CallExcuteActionAfterAnimation(mouseWorldPos, itemDetails);
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
        if(!InputDisable)
          PlayerInput();
        else
            IsMoving =false;
        SwitchAnimation();
    }
    private void FixedUpdate()
    {
        if(!InputDisable)
        {
            Movement();
        }
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
