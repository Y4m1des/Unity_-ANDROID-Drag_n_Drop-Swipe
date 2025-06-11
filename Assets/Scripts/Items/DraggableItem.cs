using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    private InputHandler inputHandler;
    private Camera mainCamera;

    private bool isDrag;

    private void Awake()
    {
        inputHandler = InputHandler.Instance;
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        isDrag = true;
    }

    private void OnMouseUp()
    {
        isDrag = false; 
    }

    //if game object can be dragged then finds touch position and moves to it
    private void Update()
    {
        if(isDrag)
        {
            Vector2 touchPosition = mainCamera.ScreenToWorldPoint(inputHandler.PrimaryPosition()) - transform.position;
            transform.Translate(touchPosition);
        }
    }
}
