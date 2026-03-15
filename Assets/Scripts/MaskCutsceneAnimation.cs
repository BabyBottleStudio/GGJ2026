using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskCutsceneAnimation : MonoBehaviour
{
    public RectTransform target;
    public RectTransform start;
    //public float speed = 8f;
    public float duration;
    public AnimationCurve positionLerpCurve;
    public AnimationCurve scaleLerpCurve;
    float timer;
    RectTransform rect;
    //Animator animator;
    Vector3 startPosition;
    Vector3 startScale;


    void Start()
    {
        rect = GetComponent<RectTransform>();
        //animator = GetComponent<Animator>();
        //IconAnimation();
    }

    void Update()
    {

    }


    public void IconAnimation()
    {
        // animator.enabled = false;
        Debug.Log("Animation should start here");
        StartCoroutine(IconAnimationRoutine());
    }

    IEnumerator IconAnimationRoutine()
    {

        timer = 0f;

        startScale = Vector3.one; // rect.localScale;
        rect.position = start.position;
        startPosition = start.position;
        //Debug.Log($"StartScale = {startScale}");

        // while (Vector3.Distance(rect.position, target.position) > 5f)

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            float positionT = positionLerpCurve.Evaluate(t);
            float scaleT = scaleLerpCurve.Evaluate(t);


            rect.position = Vector3.Lerp(startPosition, target.position, positionT);
            rect.localScale = Vector3.Lerp(startScale, Vector3.zero, scaleT);

            yield return null;
        }

        //gameObject.SetActive(false);

    }
}
