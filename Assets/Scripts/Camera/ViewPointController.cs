using UnityEngine;

public class ViewPointController : Singleton<ViewPointController>
{
    #region PARAMS
    [field: SerializeField]
    private float minDistance = -12f;
    [field: SerializeField]
    private float maxDistance = 11f;
    [field: SerializeField]
    private float stepDistance = 3f;
    #endregion

    private GameObject viewPoint;
    private SwipeDetection swipeDetection;

    private float currPosition;

    public override void Awake()
    {
        base.Awake();

        viewPoint = this.gameObject;
        swipeDetection = SwipeDetection.Instance;
        currPosition = viewPoint.transform.position.x;
    }

    private void OnEnable()
    {
        swipeDetection.OnSwipeDetection += MoveViewPoint;
    }

    private void OnDisable()
    {
        swipeDetection.OnSwipeDetection -= MoveViewPoint;
    }

    //move view point of virtual cinemachine to certain position
    private void MoveViewPoint(Vector2 direction)
    {
        currPosition = Mathf.Clamp(currPosition + direction.x * stepDistance, minDistance, maxDistance);
        viewPoint.transform.position = new Vector2(currPosition, viewPoint.transform.position.y);
    }
}
