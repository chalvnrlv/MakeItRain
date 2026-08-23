using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class ParticleGlow : MonoBehaviour
{
    [Header("Color Randomization")]
    [Tooltip("Min/Max hue range (0-1). Full range = all colors.")]
    [SerializeField] private float hueMin = 0f;
    [SerializeField] private float hueMax = 1f;
    [Tooltip("Saturation — keep high for vivid colors")]
    [SerializeField] private float satMin = 0.7f;
    [SerializeField] private float satMax = 1f;
    [Tooltip("Brightness — keep high so bloom triggers")]
    [SerializeField] private float valMin = 0.9f;
    [SerializeField] private float valMax = 1f;

    private Color glowColor;

    [Header("Intensity")]
    [Tooltip("Peak HDR emission intensity (>1 triggers bloom)")]
    [SerializeField] private float maxIntensity = 4f;

    [Header("Timing")]
    [SerializeField] private float riseTime  = 0.08f;  
    [SerializeField] private float holdTime  = 0.05f;  
    [SerializeField] private float fadeTime  = 0.35f;  

    private Material mat;
    private Coroutine glowRoutine;

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }

    void OnEnable()
    {
        glowColor = Random.ColorHSV(hueMin, hueMax, satMin, satMax, valMin, valMax);

        if (mat.HasProperty("_BaseColor"))
            mat.SetColor("_BaseColor", glowColor);
        else
            mat.SetColor("_Color", glowColor);

        SetEmission(0f);
    }

    void OnDisable()
    {
        if (glowRoutine != null)
        {
            StopCoroutine(glowRoutine);
            glowRoutine = null;
        }
        SetEmission(0f);
    }


    void OnCollisionEnter2D(Collision2D _) => TriggerGlow();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DestroyZone")) return;
        TriggerGlow();
    }


    public void TriggerGlow()
    {
        if (!gameObject.activeInHierarchy) return;

        if (glowRoutine != null) StopCoroutine(glowRoutine);
        glowRoutine = StartCoroutine(GlowCoroutine());
    }

    private IEnumerator GlowCoroutine()
    {
        yield return LerpEmission(0f, maxIntensity, riseTime);

        yield return new WaitForSeconds(holdTime);

        yield return LerpEmission(maxIntensity, 0f, fadeTime);

        glowRoutine = null;
    }

    private IEnumerator LerpEmission(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetEmission(Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }
        SetEmission(to);
    }

    private void SetEmission(float intensity)
    {
        mat.SetColor("_EmissionColor", glowColor * intensity);
    }
}
