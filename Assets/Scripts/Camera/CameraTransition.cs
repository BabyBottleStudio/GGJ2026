using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;




public class CameraTransition : MonoBehaviour
{
    public GameSettings gameSettings;
    [SerializeField] Camera mainCamera;
    [Space(10)]
    [SerializeField] PostProcessVolume startingPostProcess;
    [SerializeField] PostProcessVolume maskOnPostProcess;
    [Space(10)]

    [SerializeField] Transform startTransform;
    [SerializeField] Transform defaultTransform;
    [SerializeField] Transform specialTileTransform;
    [SerializeField] Transform maskOnTransform;

    //AnimationCurve cameraTransition;
    //[Space(10)]
    //[SerializeField] float transitionDuration;
    //[SerializeField] AnimationCurve cameraTransition;



    float transitionTimer = 0f;
    Vector3 currentVelocity;

    bool isTransitioning;
    Transform targetTransform;

    Vector3 startPosition;
    Quaternion startRotation;

    float targetWeight = 0f;

    //Func<float, bool> isTargetWeightReached;

    // Start is called before the first frame update
    void Start()
    {
        transitionTimer = 0f;
        isTransitioning = false;
        mainCamera.transform.position = defaultTransform.position;
        mainCamera.transform.rotation = defaultTransform.rotation;
        mainCamera.fieldOfView = gameSettings.IntroFOV;

        //isTargetWeightReached = IsTargetWeightReached();

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
    //void Update()
    //{
    //    //if (isTransitioning)
    //    //{
    //    //    StopAllCoroutines();
    //    //    StartCoroutine(CameraTransition_Movement());
    //    //    //CameraTransitionMovement();
    //    //    PostProcessBlending();
    //    //}
    //}
    public void TransitionToDefault()
    {
        StartTransition(defaultTransform);
        SetTargetWeightForPostProcessing(false);

        StopAllCoroutines();
        StartCoroutine(CameraTransitionMovement(gameSettings.CameraIntroToGamePosTransition, gameSettings.CameraIntroToGameRotationTransition, gameSettings.CameraIntroTransitionDuration));
        StartCoroutine(PostProcessBlending(startingPostProcess, gameSettings.PostProcesWeightTransition, gameSettings.PostProcessWeightTransitionDuration));
        StartCoroutine(CameraFOVBlending(gameSettings.CameraIntroFOVTransition, gameSettings.CameraFOVTransitionDuration));
        //CameraTransitionMovement();
        //startingPostProcess.gameObject.SetActive(false);
    }
    public void StartingPostProcessActive(bool isActive)
    {
        startingPostProcess.gameObject.SetActive(isActive);
        if (isActive)
        {
            startingPostProcess.weight = 1f;
        }
    }

    public void PlayerMaskOn(bool maskOn)
    {
        if (maskOn)
        {
            //Debug.Log("Player has put the mask");
            StartTransition(maskOnTransform);
        }
        else
        {
            //Debug.Log("Player took the mask off");
            StartTransition(defaultTransform);

        }

        SetTargetWeightForPostProcessing(maskOn);

        StopAllCoroutines();
        StartCoroutine(CameraTransitionMovement(gameSettings.CameraTransition, gameSettings.CameraTransitionDuration));
        StartCoroutine(PostProcessBlending(maskOnPostProcess, gameSettings.CameraTransition, 3f)); //gameSettings.CameraTransitionDuration));
    }

    void StartTransition(Transform target)
    {
        targetTransform = target;

        startPosition = mainCamera.transform.position;
        startRotation = mainCamera.transform.rotation;

        transitionTimer = 0f;
        isTransitioning = true;
    }
    /*
    void CameraTransitionMovement()//Transform targetTransform)
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / gameSettings.CameraTransitionDuration);
        float curveT = gameSettings.CameraTransition.Evaluate(t);

        mainCamera.transform.position = Vector3.Lerp(startPosition, targetTransform.position, curveT);
        mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetTransform.rotation, curveT);


        if (t >= 1)
        {
            isTransitioning = false;
        }
    }*/

    public void SetCameraToStartPosition()
    {
        mainCamera.transform.position = startTransform.position;
        mainCamera.transform.rotation = startTransform.rotation;
        mainCamera.fieldOfView = gameSettings.IntroFOV;
    }

    public void SetTargetWeightForPostProcessing(bool isOn)
    {
        targetWeight = isOn ? 1f : 0f;
        //isTargetWeightReached = IsTargetWeightReached();
    }

    /*
    bool IsTargetWeightReached(float postProcessWeight)
    {
        if (targetWeight == 1f)
        {
            return postProcessWeight < targetWeight;
        }
        else if (targetWeight == 0)
        {
            return postProcessWeight >= targetWeight;
        }

        return false;
    }
    */

    /*
    Func<float, bool> IsTargetWeightReached()
    {
        switch (targetWeight)
        {
            case 0f:
                return (currentWeight) => currentWeight >= targetWeight;
            case 1f:
                return (currentWeight) => currentWeight < targetWeight;
            default:
                throw new ArgumentException(targetWeight.ToString());
        }
    }
    */
    /*
    void PostProcessBlending()
    {
        var amt = Mathf.MoveTowards(maskOnPostProcess.weight, targetWeight, Time.deltaTime * gameSettings.CameraTransitionDuration);
        maskOnPostProcess.weight = amt;
    } 
    */



    IEnumerator PostProcessBlending(PostProcessVolume postProcess, AnimationCurve transitionCurve, float duration)
    {
        float transitionTimer = 0f;
        //float amt = postProcess.weight == 1f ? 1f : 0f;
        float startWeight = postProcess.weight;

        while (transitionTimer < duration) // (treba da se upotrebi delegat koji radi pravilnu promenu)
        {
            transitionTimer += Time.deltaTime;

            float t = Mathf.Clamp01(transitionTimer / duration);
            float curveT = transitionCurve.Evaluate(t);

            //var amt = Mathf.MoveTowards(postProcess.weight, targetWeight, curveT); //Time.deltaTime * gameSettings.CameraTransitionDuration);
           var amt = Mathf.Lerp(startWeight, targetWeight, curveT); //Time.deltaTime * gameSettings.CameraTransitionDuration);
            postProcess.weight = amt;
            yield return null;
        }

        postProcess.weight = targetWeight;
    }

    IEnumerator CameraTransitionMovement(AnimationCurve transitionCurve, float duration)
    {
        while (transitionTimer < duration)
        {
            transitionTimer += Time.deltaTime;
            //float t = Mathf.Clamp01(transitionTimer / gameSettings.CameraTransitionDuration);
            float t = Mathf.Clamp01(transitionTimer / duration);
            float curveT = transitionCurve.Evaluate(t);

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetTransform.position, curveT);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetTransform.rotation, curveT);

            yield return null;
        }

        isTransitioning = false;
    }


    IEnumerator CameraTransitionMovement(AnimationCurve posTransitionCurve, AnimationCurve rotationTransitionCurve, float duration)
    {
        while (transitionTimer < duration)
        {
            transitionTimer += Time.deltaTime;
            //float t = Mathf.Clamp01(transitionTimer / gameSettings.CameraTransitionDuration);
            float t = Mathf.Clamp01(transitionTimer / duration);
            float posCurveT = posTransitionCurve.Evaluate(t);
            float rotationCurveT = rotationTransitionCurve.Evaluate(t);

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetTransform.position, posCurveT);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetTransform.rotation, rotationCurveT);

            yield return null;
        }

        isTransitioning = false;
    }


    IEnumerator CameraFOVBlending(AnimationCurve transitionCurve, float duration)
    {
        float transitionTimer = 0f;

        float startFOV = gameSettings.IntroFOV;

        while(transitionTimer < duration)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / duration);
            float curveT = transitionCurve.Evaluate(t);

            mainCamera.fieldOfView = Mathf.Lerp(startFOV, gameSettings.GameFOV, curveT);

            yield return null;
        }

        mainCamera.fieldOfView = gameSettings.GameFOV;
    }

}
