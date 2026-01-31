using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private Vector2 _input;

    CharacterController _characterController;
    [SerializeField] float speed;

    private Vector3 _direction;

    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;


    // Start is called before the first frame update
    void Start()
    {
        _characterController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.sqrMagnitude == 0)
            return;
        ////direction = new Vector3 ()
        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();

        _direction = new Vector3(_input.x, 0f, _input.y);
    }
}
