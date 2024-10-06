using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    [SerializeField] [Range(0.1f, 2.0f)] private float sampleDuration = 1.0f;

    [SerializeField] private DisplayMode displayMode = DisplayMode.FPS;

    public enum DisplayMode
    {
        FPS,
        MS
    }

    private int frames;

    private float duration;
    private float bestDuration = float.MaxValue;
    private float worstDuration;

    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;

        frames += 1;

        duration += frameDuration;

        if (frameDuration < bestDuration)
        {
            bestDuration = frameDuration;
        }

        if (frameDuration > worstDuration)
        {
            worstDuration = frameDuration;
        }

        if (duration >= sampleDuration)
        {
            if (displayMode == DisplayMode.FPS)
            {
                display.SetText("FPS\n{0:0}\n{1:0}\n{2:0}", 1.0f / bestDuration, frames / duration, 1.0f / worstDuration);
            }
            else if (displayMode == DisplayMode.MS)
            {
                display.SetText("MS\n{0:1}\n{1:1}\n{2:1}", 1000.0f *  bestDuration, 1000.0f * duration / frames, 1000.0f * worstDuration);
            }

            frames = 0;

            duration = 0.0f;
            bestDuration = float.MaxValue;
            worstDuration = 0.0f;
        }
    }
}