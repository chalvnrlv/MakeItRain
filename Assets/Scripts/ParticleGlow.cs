using UnityEngine;
using System.Collections;

/// <summary>
/// Attach to the particle prefab.
/// When the particle physically contacts another collider (particle-particle
/// or particle-body), it flashes a glow via material emission.
///
/// Requirements:
///  - The particle's material must support emission (Standard / URP Lit / etc.)
///  - For visible bloom, enable Post Processing with a Bloom override in your camera.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class ParticleGlow : MonoBehaviour
{
    [Header("Glow Color")]
    [SerializeField] private Color glowColor = new Color(0.4f, 0.8f, 1f);

    [Header("Intensity")]
    [Tooltip("Peak HDR emission intensity (>1 triggers bloom)")]
    [SerializeField] private float maxIntensity = 4f;

    [Header("Timing")]
    [SerializeField] private float riseTime  = 0.08f;   // how fast it lights up
    [SerializeField] private float holdTime  = 0.05f;   // hold at peak
    [SerializeField] private float fadeTime  = 0.35f;   // how fast it fades

    private Material mat;
    private Coroutine glowRoutine;

    // ─── Lifecycle ────────────────────────────────────────────────────────────

    void Awake()
    {
        // renderer.material creates a per-instance copy so particles glow independently
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }

    void OnEnable()
    {
        // Reset emission every time the particle is pulled from the pool
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

    // ─── Collision hooks ─────────────────────────────────────────────────────

    // Particle ↔ Particle  (Rigidbody2D + Collider2D, non-trigger)
    void OnCollisionEnter2D(Collision2D _) => TriggerGlow();

    // Particle ↔ Body collider  (if PolygonCollider2D is set as trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore the DestroyZone — the particle is about to be disabled anyway
        if (other.CompareTag("DestroyZone")) return;
        TriggerGlow();
    }

    // ─── Glow logic ───────────────────────────────────────────────────────────

    public void TriggerGlow()
    {
        if (glowRoutine != null) StopCoroutine(glowRoutine);
        glowRoutine = StartCoroutine(GlowCoroutine());
    }

    private IEnumerator GlowCoroutine()
    {
        // Rise
        yield return LerpEmission(0f, maxIntensity, riseTime);

        // Hold at peak
        yield return new WaitForSeconds(holdTime);

        // Fade out
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
