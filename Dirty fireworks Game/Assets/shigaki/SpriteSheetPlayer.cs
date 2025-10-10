using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class SpriteSheetPlayer : MonoBehaviour
{
    [Header("Sprite Frames")]
    [SerializeField] private SpriteRenderer targetRenderer;
    [SerializeField] private Sprite[] frames;

    [Header("Playback")]
    [SerializeField, Min(1f)] private float fps = 15f;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool playOnEnable = true;
    [SerializeField] private int startFrame = 0;
    [SerializeField] private bool useUnscaledTime = false;

    [Header("Events")]
    public UnityEvent onFinished;   // 一度再生の完了、または非ループの完走時

    private int current;
    private float timer;
    private bool isPlaying;

    public float FPS { get => fps; set => fps = Mathf.Max(1f, value); }
    public bool Loop { get => loop; set => loop = value; }
    public bool IsPlaying => isPlaying;

    void Reset()
    {
        targetRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (playOnEnable) Play(startFrame);
    }

    void Update()
    {
        if (!isPlaying || frames == null || frames.Length == 0 || targetRenderer == null) return;

        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        timer += dt;

        float frameTime = 1f / fps;
        while (timer >= frameTime)
        {
            timer -= frameTime;
            StepFrame();
        }
    }

    private void StepFrame()
    {
        current++;

        if (current >= frames.Length)
        {
            if (loop)
            {
                current = 0;
            }
            else
            {
                current = frames.Length - 1;
                Apply();
                isPlaying = false;
                onFinished?.Invoke();
                return;
            }
        }
        Apply();
    }

    private void Apply()
    {
        if (targetRenderer != null && frames != null && frames.Length > 0)
        {
            targetRenderer.sprite = frames[current];
        }
    }

    // --- Public API ---
    public void Play(int start = 0)
    {
        if (frames == null || frames.Length == 0 || targetRenderer == null) return;
        current = Mathf.Clamp(start, 0, frames.Length - 1);
        timer = 0f;
        isPlaying = true;
        Apply();
    }

    public void PlayOnce(int start = 0)
    {
        loop = false;
        Play(start);
    }

    public void Stop()
    {
        isPlaying = false;
    }

    public void SetFrames(Sprite[] newFrames, bool autoPlay = true, bool resetIndex = true)
    {
        frames = newFrames;
        if (frames == null || frames.Length == 0) { Stop(); return; }
        if (resetIndex) current = 0;
        Apply();
        if (autoPlay) Play(current);
    }

    public void SetRenderer(SpriteRenderer renderer)
    {
        targetRenderer = renderer;
        Apply();
    }
}
