using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SwipeDetection : Singleton<SwipeDetection>
{
    #region PARAMS
    [field: SerializeField]
    private float minDistance = .2f;
    [field: SerializeField]
    private float maxTime = 1f;
    [field: SerializeField, Range(0f,1f)]
    private float threshold = .9f;
    #endregion

    #region EVENTS
    //this is event that get param [Vector2 - direction] and return void
    public Action<Vector2> OnSwipeDetection;
    #endregion
    
    private InputHandler inputHandler;

    private Vector2 startPosition;
    private float startTime;
    
    private Vector2 endPosition;
    private float endTime;

    public override void Awake()
    {
        base.Awake();

        inputHandler = InputHandler.Instance;
    }

    private void OnEnable()
    {
        //subscribe methods to the events
        inputHandler.OnStartTouch += SwipeStart;
        inputHandler.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        //unsubscribe methods from the events
        inputHandler.OnStartTouch -= SwipeStart;
        inputHandler.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;

        DetectSwipe();
    }

    //checks min distance and max detection time between the initial touch and touch before taking finger off the screen
    private void DetectSwipe()
    {
        if(Vector3.Distance(startPosition, endPosition) >= minDistance &&
            endTime - startTime <= maxTime)
        {
            var direction = (endPosition - startPosition).normalized;
            SwipeDirection(direction);
        }
    }

    //checks scalar product of vectors
    //if it's approached to 0, selects direction [RIGHT - LEFT]
    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.right, direction) > threshold)
        {
            OnSwipeDetection?.Invoke(Vector2.right);
        }

        if (Vector2.Dot(Vector2.left, direction) > threshold)
        {
            OnSwipeDetection?.Invoke(Vector2.left);
        }
    }
}
