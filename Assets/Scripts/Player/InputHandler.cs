using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class InputHandler : Singleton<InputHandler>
{
    private InputActions inputActions;
    private Camera mainCamera;

    #region EVENTS
    //this is events that gets params [Vector2 - position, float - time] and return void
    //calls when player touch screene
    public Action<Vector2, float> OnStartTouch;
    //calls when player take finger off the screene
    public Action<Vector2, float> OnEndTouch;
    #endregion

    public override void Awake()
    {
        base.Awake();

        inputActions = new InputActions();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new InputActions();
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        //subscribe methods to the events when:
        //[START TOUCH SCREENE]
        inputActions.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        //[TAKE FINGER OFF THE SCREENE]
        inputActions.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

    }

    //calls when [START TOUCH SCREENE]
    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        OnStartTouch?.Invoke(mainCamera.ScreenToWorldPoint(inputActions.Touch.PrimaryPosition.ReadValue<Vector2>()),
            (float)context.startTime);
    }

    //calls when [TAKE FINGER OFF THE SCREENE]
    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        OnEndTouch?.Invoke(mainCamera.ScreenToWorldPoint(inputActions.Touch.PrimaryPosition.ReadValue<Vector2>()),
            (float)context.time);
    }

    //this method will returns position of the finger
    public Vector2 PrimaryPosition()
    {
        return inputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
    }
}
