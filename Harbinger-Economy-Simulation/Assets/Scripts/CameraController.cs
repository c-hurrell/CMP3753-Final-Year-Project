using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] private float cameraMovementSpeed;

    public InputActionReference move; 

    private Vector2 _moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(_moveDirection.x * cameraMovementSpeed, _moveDirection.y * cameraMovementSpeed);
    }
    
    public void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = inputValue.ReadValue<Vector2>().y / 100f;

        var cameraHeight = transform.position.z + value;

        var temp = new Vector3(transform.position.x, transform.position.y, cameraHeight);

        transform.position = temp;
        // if (Mathf.Abs(value) > 0.1f)
        // {
        //     zoomHeigh
        // }

    }
}
