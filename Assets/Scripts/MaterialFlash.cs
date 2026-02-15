using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFlash : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Materials")]
    [SerializeField] Material[] materials;

    List<Material> filteredMaterials;

    [Header("EffectSettings")]

    [SerializeField] AnimationCurve effectTransition;
    [SerializeField] float flashDuration = 0.2f;
    [SerializeField] float intensity = 1.5f;



    void Start()
    {
        filteredMaterials = new List<Material>();
        FilterMaterials();
    }

    // Update is called once per frame

    private void OnEnable()
    {
        EventRepository.OnMaskPickupAnimFinish += FlashEffect; // invokuje se preko animacije
    }

    private void OnDisable()
    {
        EventRepository.OnMaskPickupAnimFinish += FlashEffect;
        
    }

    public void FlashEffect()
    {
        if (filteredMaterials.Count == 0)
            return;

        //StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        float timer = 0f;

        foreach (var mat in filteredMaterials)
            mat.EnableKeyword("_EMISSION");

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;

            float t = timer / flashDuration;

            //  vrednost iz krive (0–1)
            float curveValue = effectTransition.Evaluate(t);

            //  crna → bela
            Color emissionColor = Color.Lerp(Color.black, Color.white, curveValue);

            foreach (var mat in filteredMaterials)
            {
                mat.SetColor("_EmissionColor", emissionColor * intensity);
            }

            yield return null;
        }

        // 👉 vrati na crno (pošto ne čuvamo original)
        foreach (var mat in filteredMaterials)
        {
            mat.SetColor("_EmissionColor", Color.black);
        }
    }


    void FilterMaterials()
    {
        foreach(var material in materials)
        {
            if (material == null)
                continue;

            if (material.HasProperty("_EmissionColor"))
            {
                filteredMaterials.Add(material);
            }
            else
            {
                Debug.Log($"{material.name} has no _EmissionColor");
            }
        }

        if (filteredMaterials.Count == 0)
        {
            Debug.LogWarning("No emissive materials assigned.");
        }
    }

}
