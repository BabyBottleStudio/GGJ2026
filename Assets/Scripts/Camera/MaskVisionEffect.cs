using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskVisionEffect : MonoBehaviour
{
    public GameSettings gameSettings;
    Camera mainCamera;
    public LayerMask ghostLayer;

    public Material ghostMaterial;


    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.cullingMask &= ~ghostLayer;

        if (gameSettings == null)
        {
            Debug.LogWarning("Game settings scriptableObject is null!");
            return;
        }

        SetUpGhostMaterial();
    }

    private void SetUpGhostMaterial()
    {
        if (ghostMaterial == null)
        {
            Debug.LogWarning("Ghost material is null");
            return;
        }

        if (!ghostMaterial.HasProperty("_DissolveAmt"))
        {
            Debug.LogWarning("Ghost material does not have _dissolveAmt property");
            return;
        }

        ghostMaterial.SetFloat("_DissolveAmt", 1);
    }

    private void OnEnable()
    {
        EventRepository.OnKeyCollected += SubscribeToEvent;
    }

    private void OnDisable()
    {
        EventRepository.OnActionKeyPressed -= ToggleGhostVision;
    }

    void SubscribeToEvent(object sender, PickupCollectedEventArgs e)
    {
        EventRepository.OnActionKeyPressed += ToggleGhostVision;
        EventRepository.OnKeyCollected -= SubscribeToEvent;
    }


    void ToggleGhostVision(bool maskOn)
    {
        StopAllCoroutines();
        StartCoroutine(GhostDissolveRoutine(maskOn));

        if (maskOn)
        {
            // Kada je maska UKLJUČENA:
            // Uključi "Ghosts" layer u Culling Mask-u kamere (OR operacija)
            // treba da je odmah aktivan
            mainCamera.cullingMask |= ghostLayer;
        }
        else
        {
            // Isključi "Ghosts" layer iz Culling Mask-a kamere (AND NOT operacija)
            // aktivira se kada kamera završi povratak, a za to vreme traje efekat nestajanja
            StartCoroutine(TurnGhostLayerOff(gameSettings.CameraTransitionDuration));
        }
    }

    IEnumerator TurnGhostLayerOff(float duration)
    {
        yield return new WaitForSeconds(duration);
        mainCamera.cullingMask &= ~ghostLayer;
    }


    IEnumerator GhostDissolveRoutine(bool isMaskUsed)
    {
        float timer = 0f;
        float dissolveDuration = gameSettings.CameraTransitionDuration;
        float startValue = ghostMaterial.GetFloat("_DissolveAmt");
        float targetValue = (isMaskUsed ? 0 : 1);

        while (timer < dissolveDuration)
        {
            timer += Time.deltaTime;
            float t = timer / dissolveDuration;

            float dissolveValue = Mathf.SmoothStep(startValue, targetValue, t);
            //dissolveValue = Mathf.SmoothStep(currentValue, targetValue, t);
            //  crna → bela
            ghostMaterial.SetFloat("_DissolveAmt", dissolveValue);

            yield return null;
        }

        ghostMaterial.SetFloat("_DissolveAmt", targetValue);
    }
}
