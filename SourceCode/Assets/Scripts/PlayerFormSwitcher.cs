using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PlayerForm
{
    Solid,
    Ghost
}

public class PlayerFormSwitcher : MonoBehaviour
{
    [Header("Transformation Settings")]
    public float minSwitchTime = 2f;
    public float maxSwitchTime = 5f;
    public float warningTime = 0.4f;

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Color solidColor = Color.white;
    public Color ghostColor = new Color(0.4f, 0.8f, 1f, 0.4f);

    public TilemapCollider2D TileMapColider;
    public PlayerForm CurrentForm { get; private set; }
        = PlayerForm.Solid;

    public bool IsGhost => CurrentForm == PlayerForm.Ghost;
    public bool IsSolid => CurrentForm == PlayerForm.Solid;

    public event Action<PlayerForm> OnFormChanged;

    private float switchTimer;
    private bool isWarning;
    private float warningTimer;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        CurrentForm = PlayerForm.Solid;
        ResetTimer();
        UpdateVisuals();
    }

    private void Update()
    {
        switchTimer -= Time.deltaTime;

        if (switchTimer <= warningTime && !isWarning)
        {
            isWarning = true;
            warningTimer = 0f;
        }

        if (isWarning)
        {
            warningTimer += Time.deltaTime;

            // Blink shortly before transforming.
            if (spriteRenderer != null)
                spriteRenderer.enabled =
                    Mathf.FloorToInt(warningTimer * 12f) % 2 == 0;
        }

        if (switchTimer <= 0f)
            SwitchForm();
    }

    public void SwitchForm()
    {
        CurrentForm = IsSolid
            ? PlayerForm.Ghost
            : PlayerForm.Solid;

        isWarning = false;
        ResetTimer();
        UpdateVisuals();

        OnFormChanged?.Invoke(CurrentForm);
    }

    private void ResetTimer()
    {
        switchTimer = UnityEngine.Random.Range(
            minSwitchTime, maxSwitchTime
        );
    }

    private void UpdateVisuals()
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.enabled = true;
        spriteRenderer.color = IsGhost ? ghostColor : solidColor;
        TileMapColider.enabled = !IsGhost;
    }
}
