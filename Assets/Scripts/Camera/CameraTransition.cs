using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;




public class CameraTransition : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [Space(10)]
    [SerializeField] PostProcessVolume maskOnPostProcess;
    [Space(10)]

    [SerializeField] Transform defaultTransform;
    [SerializeField] Transform specialTileTransform;
    [SerializeField] Transform maskOnTransform;

    [Space(10)]
    [SerializeField] float transitionDuration;
    [SerializeField] AnimationCurve cameraTransition;

    float transitionTimer = 0f;
    Vector3 currentVelocity;

    bool isTransitioning;
    Transform targetTransform;

    Vector3 startPosition;
    Quaternion startRotation;

    float targetWeight = 0f;

    // Start is called before the first frame update
    void Start()
    {
        transitionTimer = 0f;
        isTransitioning = false;
        mainCamera.transform.position = defaultTransform.position;
        mainCamera.transform.rotation = defaultTransform.rotation;
    }

    private void OnEnable()
    {
        //EventRepository.OnTileEnter += PlayerEnterTile;
        //EventRepository.OnTileExit += TransitionToDefault;
        EventRepository.OnActionKeyPressed += PlayerMaskOn;

    }

    private void OnDisable()
    {
        //EventRepository.OnTileEnter -= PlayerEnterTile;
        //EventRepository.OnTileExit -= TransitionToDefault;
        EventRepository.OnActionKeyPressed -= PlayerMaskOn;
    }
    // Update is called once per frame
    void Update()
    {
        if (isTransitioning)
        {
            CameraTransitionMovement(targetTransform);
            PostProcessBlending();
        }
    }

    /*
    public void TransitionToDefault(object sender, SpecialTileEventArgs e)
    {
        Debug.Log("Player has left the tile");

        if (StateMachine.GetCurrentState() == State.MaskOn)
            return;

        StartTransition(defaultTransform);

    }
    */

    /*
    public void PlayerEnterTile(object sender, SpecialTileEventArgs e)
    {
        Debug.Log("Player has entered the tile");

        if (StateMachine.GetCurrentState() == State.MaskOn)
            return;

        StartTransition(specialTileTransform);
    }
    */
    public void PlayerMaskOn(bool maskOn)
    {
        //if (StateMachine.GetCurrentMask() == Mask.Lost)
        //    return;

        if (maskOn)
        {
            Debug.Log("Player has put the mask");

            StartTransition(maskOnTransform);
        }
        else
        {
            Debug.Log("Player took the mask off");
            //if (StateMachine.GetCurrentTile() == Tile.Regular)
            StartTransition(defaultTransform);
            //else if (StateMachine.GetCurrentTile() == Tile.Special)
            //StartTransition(specialTileTransform);
        }

        SetTargetWeightForPostProcessing(maskOn);
    }

    /*
    public void PlayerMaskOn(object sender, ActionPressedEventArgs e)
    {
        if (StateMachine.GetCurrentMask() == Mask.Lost)
            return;

        if (e.isMaskOn)
        {
            Debug.Log("Player has put the mask");

            StartTransition(maskOnTransform);
        }
        else
        {
            Debug.Log("Player took the mask off");
            //if (StateMachine.GetCurrentTile() == Tile.Regular)
            StartTransition(defaultTransform);
            //else if (StateMachine.GetCurrentTile() == Tile.Special)
            //StartTransition(specialTileTransform);
        }

        SetTargetWeightForPostProcessing(e.isMaskOn);
    }
    */

    void StartTransition(Transform target)
    {
        targetTransform = target;

        startPosition = mainCamera.transform.position;
        startRotation = mainCamera.transform.rotation;

        transitionTimer = 0f;
        isTransitioning = true;
    }

    void CameraTransitionMovement(Transform targetTransform)
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);
        float curveT = cameraTransition.Evaluate(t);

        mainCamera.transform.position = Vector3.Lerp(startPosition, targetTransform.position, curveT);
        mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetTransform.rotation, curveT);


        if (t >= 1)
        {
            isTransitioning = false;
        }
    }

    public void SetTargetWeightForPostProcessing(bool isOn)
    {
        targetWeight = isOn ? 1f : 0f;
    }

    void PostProcessBlending()
    {
        maskOnPostProcess.weight = Mathf.MoveTowards(maskOnPostProcess.weight, targetWeight, Time.deltaTime * transitionDuration);
      
  
    }
}
