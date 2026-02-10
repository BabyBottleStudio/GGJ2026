using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerInputHandler playerInputHandler;
    private Vector2 _input;
    
    public Animator playerAnimation;

    CharacterController _characterController;
    [SerializeField] float _movementSpeed;

    private Vector3 _direction;

    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;

    private const float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3f;
    float _velocity;

    public ParticleSystem dustParticles;

    float ControllsOffTimer;
    public float ControllsOffDuration;
    //bool playerControllActive;
    Vector3 ControllsOffPosition;

    private void OnEnable()
    {
        EventRepository.OnKeyCollected += SuspendPlayerInput;
    }

    private void OnDisable()
    {
        EventRepository.OnKeyCollected -= SuspendPlayerInput;
    }

    // Start is called before the first frame update
    void Start()
    {
        //playerControllActive = true;
        StateMachine.SetPlayerInputState(PlayerControlls.On);
        ControllsOffTimer = 0f;
        _characterController = this.GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        _input = playerInputHandler.MoveInput;
        _direction = playerInputHandler.Movement;

        //return; 
        if (StateMachine.GetPlayerInputState() == PlayerControlls.Off)
        {
            ControllsOffTimer += Time.deltaTime;
           

            if (ControllsOffTimer >= ControllsOffDuration)
            {
                Debug.Log($"{ControllsOffTimer}");
                ControllsOffTimer = 0f;
                EventRepository.InvokeOnCutsceneEnd();

                //StartCoroutine(Wait(10f));

                StateMachine.SetPlayerInputState(PlayerControlls.On);
                playerInput.ActivateInput();
                //playerControllActive = true;
            }
            return;
        }



        ApplyRotation();

        ApplyMovement();
        ApllyGravity();
        ApplyAnimationMultiplier();
    }

    //IEnumerator Wait(float timeToWait)
    //{
    //    yield return new WaitForSeconds(timeToWait);
    //}

    private void ApplyMovement()
    {
        //if (StateMachine.GetPlayerInputState() == PlayerControlls.Off)
        //    return;

        _characterController.Move(_direction * _movementSpeed * Time.deltaTime);
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

    private void ApplyAnimationMultiplier()
    {
        // 1. Definišemo željenu brzinu (podrazumevano 1f, puna brzina)
        float targetMultiplier = 1f;

        // 2. Proveravamo da li se karakter trenutno kreće
        // Koristimo _input.sqrMagnitude (kvadrat magnitude) jer je performantnije od magnitude
        // i proveravamo da li je veće od nule (tj. da li ima inputa)
        if (_input.sqrMagnitude == 0)
        {
            // Kada je idle (nema inputa), postavljamo usporenu vrednost (0.2f)
            targetMultiplier = 0.2f;

            if (dustParticles.isPlaying)
                dustParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            if (!dustParticles.isPlaying)
                dustParticles.Play();
            
        }

        // 3. Šaljemo izračunatu brzinu u Animator Controller
        // Moras imati Float parametar u Animatoru koji se zove TACNO "AnimSpeedMultiplier"
        if (playerAnimation != null)
        {
            playerAnimation.SetFloat("AnimSpeedMultiplier", targetMultiplier);
        }
    }

    void SuspendPlayerInput(object sender, PickupCollectedEventArgs e)
    {
        var senderGameObject = sender as GameObject;

        if (senderGameObject == null)
        {
            Debug.LogWarning("Casting unsucsesfull!");
            return;
        }

        _input = Vector2.zero;
        _direction = Vector3.zero;
        _velocity = 0f;

        ControllsOffPosition = senderGameObject.transform.position;

        this.transform.position = ControllsOffPosition + new Vector3(0, 0.08f, 0);
        this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        if (dustParticles.isPlaying)
            dustParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        if (playerAnimation != null)
        {
            playerAnimation.SetFloat("AnimSpeedMultiplier", 0f);
        }


        StateMachine.SetPlayerInputState(PlayerControlls.Off);
        playerInput.DeactivateInput();
        //playerControllActive = false;
    }
}
