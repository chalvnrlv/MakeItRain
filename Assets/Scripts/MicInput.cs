using UnityEngine;

public class MicInput : MonoBehaviour
{
    [Header("Microphone Settings")]
    [SerializeField] private int sampleWindow = 64;

    [Header("Sensitivity")]
    [Tooltip("Multiply raw RMS by this value to reach a usable 0-1 range")]
    [SerializeField] private float sensitivity = 150f;

    [Tooltip("How quickly the loudness value smooths out (higher = snappier)")]
    [SerializeField] private float smoothSpeed = 15f;

    public static float Loudness { get; private set; }

    private AudioClip micClip;
    private string micDevice;

    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("[MicInput] No microphone found. Mic reactivity disabled.");
            enabled = false;
            return;
        }

        micDevice = Microphone.devices[0];
        micClip = Microphone.Start(micDevice, true, 1, AudioSettings.outputSampleRate);
        Debug.Log($"[MicInput] Using microphone: {micDevice}");
    }

    void Update()
    {
        float raw = GetRMSLoudness() * sensitivity;
        float target = Mathf.Clamp01(raw);
        Loudness = Mathf.Lerp(Loudness, target, Time.deltaTime * smoothSpeed);
    }

    private float GetRMSLoudness()
    {
        int micPosition = Microphone.GetPosition(micDevice) - sampleWindow;
        if (micPosition < 0) return 0f;

        float[] samples = new float[sampleWindow];
        micClip.GetData(samples, micPosition);

        float sum = 0f;
        foreach (float s in samples)
            sum += s * s;

        return Mathf.Sqrt(sum / sampleWindow);
    }
}
