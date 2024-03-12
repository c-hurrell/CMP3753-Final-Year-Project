using System.Collections;
using System.Collections.Generic;
using HarbingerScripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField] private float cameraMovementSpeed;

    [SerializeField] private GameObject economyManager;
    private float _gameSpeed = 1;

    public InputActionReference move;

    private double _uniqueIncreasePress;
    private double _uniqueDecreasePress;

    private Vector2 _moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(_moveDirection.x * cameraMovementSpeed,0, _moveDirection.y * cameraMovementSpeed);
    }
    
    public void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = inputValue.ReadValue<Vector2>().y / 100f;

        var cameraHeight = transform.position.y + value;

        if (cameraHeight < 10)
            cameraHeight = 10;

        var temp = new Vector3(transform.position.x, cameraHeight, transform.position.z);
        
        transform.position = temp;
        // if (Mathf.Abs(value) > 0.1f)
        // {
        //     zoomHeight
        // }

    }

    public void IncreaseGameSpeed()
    {
        if (_gameSpeed >= 8) {
            _gameSpeed = 8;
        }
        else {
            _gameSpeed *= 2;
        }

        economyManager.GetComponent<EconomyManager>().SetSpeed(_gameSpeed);
    }

    public void DecreaseGameSpeed()
    {
        if (_gameSpeed <= 0.25) {
            _gameSpeed = 0.25f;
        }
        else {
            _gameSpeed /= 2;
        }

        economyManager.GetComponent<EconomyManager>().SetSpeed(_gameSpeed);

    }
}
