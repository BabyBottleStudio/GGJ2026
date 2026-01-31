using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private Vector2 _input;

    CharacterController _characterController;
    [SerializeField] float _movcementSpeed;

    private Vector3 _direction;

    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;

    private const float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3f;
    float _velocity;


    // Start is called before the first frame update
    void Start()
    {
        _characterController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyRotation();

        ApplyMovement();
        ApllyGravity();
    }

    private void ApplyMovement()
    {
        _characterController.Move(_direction * _movcementSpeed * Time.deltaTime);
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0)
            return;
        ////direction = new Vector3 ()
        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }


    private void ApllyGravity()
    {
        if (_characterController.isGrounded && _velocity < 0f)
        {
            _velocity = -1f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
        _direction.y = _velocity;
    }
    public void Movement(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();

        _direction = new Vector3(_input.x, 0f, _input.y);
    }
}
