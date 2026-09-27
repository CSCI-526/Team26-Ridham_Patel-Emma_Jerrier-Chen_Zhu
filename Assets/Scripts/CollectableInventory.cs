using UnityEngine;

public enum CollectableType { Freeze, Reveal }

[DisallowMultipleComponent]
public class CollectableInventory : MonoBehaviour
{
    [Min(0.1f)] public float freezeDuration = 5f;
    [Min(0.1f)] public float revealDuration = 5f;

    public int FreezeCount { get; private set; }
    public int RevealCount { get; private set; }
    public float FreezeRemaining => Mathf.Max(0f, freezeUntil - Time.time);
    public float RevealRemaining => Mathf.Max(0f, revealUntil - Time.time);
    public bool IsFreezeActive => FreezeRemaining > 0f;
    public bool IsRevealActive => RevealRemaining > 0f;

    private float freezeUntil;
    private float revealUntil;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) TryUseFreeze();
        if (Input.GetKeyDown(KeyCode.L)) TryUseReveal();
    }

    public void Collect(CollectableType type)
    {
        if (type == CollectableType.Freeze) FreezeCount++;
        else RevealCount++;
    }

    public bool TryUseFreeze()
    {
        if (FreezeCount == 0 || IsFreezeActive) return false;

        FreezeCount--;
        freezeUntil = Time.time + Mathf.Max(0.1f, freezeDuration);
        return true;
    }

    public bool TryUseReveal()
    {
        if (RevealCount == 0 || IsRevealActive) return false;

        RevealCount--;
        revealUntil = Time.time + Mathf.Max(0.1f, revealDuration);
        return true;
    }
}
