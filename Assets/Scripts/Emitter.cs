using UnityEngine;

public class Emitter : MonoBehaviour
{
    [SerializeField] private GameObject SpawnPrefab;
    [SerializeField] private int MaxSpawn = 30;
    [SerializeField] private Vector2 SizeRange = new Vector2(0.1f, 0.5f);

    [Header("Spawn Rate")]
    [Tooltip("Spawn interval when the room is silent (seconds)")]
    [SerializeField] private float MaxSpawnRate = 1.5f;
    [Tooltip("Spawn interval when the mic is at full loudness (seconds)")]
    [SerializeField] private float MinSpawnRate = 0.05f;

    [Header("Mic Reactivity")]
    [SerializeField] private bool useMicInput = true;
    [Tooltip("How much loudness scales up particle size (0 = no effect)")]
    [SerializeField] private float sizeBoost = 1.5f;

    private GameObject[] pool;
    private float currentSpawnRate;

    void Start()
    {
        currentSpawnRate = MaxSpawnRate;
        initPool();
        spawn();
    }

    private void initPool()
    {
        pool = new GameObject[MaxSpawn];
        for (int i = 0; i < MaxSpawn; i++)
        {
            var particle = Instantiate(SpawnPrefab);
            particle.SetActive(false);
            pool[i] = particle;
        }
    }

    private void spawn()
    {
        // Mic reactivity: remap loudness → spawn rate and size multiplier
        float loudness = useMicInput ? MicInput.Loudness : 0f;
        currentSpawnRate = Mathf.Lerp(MaxSpawnRate, MinSpawnRate, loudness);
        float scaleMult = 1f + loudness * sizeBoost;

        foreach (var particle in pool)
        {
            if (!particle.activeSelf)
            {
                particle.transform.position = transform.TransformPoint(Random.insideUnitSphere * 0.5f);
                float baseSize = Random.Range(SizeRange.x, SizeRange.y);
                particle.transform.localScale = Vector3.one * baseSize * scaleMult;
                particle.SetActive(true);
                break;
            }
        }

        Invoke("spawn", currentSpawnRate);
    }
}

