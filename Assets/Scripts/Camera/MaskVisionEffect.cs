using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskVisionEffect : MonoBehaviour
{
    public GameSettings gameSettings;
    Camera mainCamera;
    public LayerMask ghostLayer;

    public Material ghostMaterial;
    float maxDissolveValue = 2.5f;

    float timer;

    // Start is called before the first frame update
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

        ghostMaterial.SetFloat("_DissolveAmt", maxDissolveValue);
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
        if (maskOn)
        {
            // Kada je maska UKLJUČENA:
            // Uključi "Ghosts" layer u Culling Mask-u kamere (OR operacija)
            // treba da je odmah aktivan
            StopAllCoroutines();
            StartCoroutine(GhostDissolveRoutine(StateMachine.GetMaskState()));
            mainCamera.cullingMask |= ghostLayer;
        }
        else
        {
            // Kada je maska ISKLJUČENA:
            // Isključi "Ghosts" layer iz Culling Mask-a kamere (AND NOT operacija)
            // aktivira se kada kamera završi povratak, a za to vreme traje efekat nestajanja

            StopAllCoroutines();
            StartCoroutine(GhostDissolveRoutine(StateMachine.GetMaskState()));
            StartCoroutine(TurnGhostLayerOff(gameSettings.CameraTransitionDuration));
        }
    }

    IEnumerator TurnGhostLayerOff(float duration)
    {
        yield return new WaitForSeconds(duration);
        mainCamera.cullingMask &= ~ghostLayer;

    }


    IEnumerator GhostDissolveRoutine(MaskUse isMaskUsed)
    {
        float timer = 0f;
        float dissolveDuration = gameSettings.CameraTransitionDuration;
        float startValue = ghostMaterial.GetFloat("_DissolveAmt");
        float targetValue = (isMaskUsed == MaskUse.MaskOn ? 0 : maxDissolveValue);

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
