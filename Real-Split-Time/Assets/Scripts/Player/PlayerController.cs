using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 16f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.05f;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private SpriteRenderer sr;

    private float moveInput;
    private bool isGrounded;
    public bool canControl = true;
    public bool isRecording;

    private List<RecordedFrame> currentRecording = new List<RecordedFrame>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // Escape always works
        if (kb.escapeKey.wasPressedThisFrame)
        {
            TimeManager.Instance.FullReset();
            return;
        }

        if (!canControl)
        {
            if (kb.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("Space pressed. canControl=false. LevelInitializer.Instance=" + (LevelInitializer.Instance != null));
                if (LevelInitializer.Instance != null)
                    LevelInitializer.Instance.NextLevel();
            }
            return;
        }

        // Horizontal input
        moveInput = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) moveInput -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveInput += 1f;

        // Jump
        if ((kb.spaceKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Split Time
        if (kb.tabKey.wasPressedThisFrame)
        {
            TimeManager.Instance.SplitTime();
        }

        // Retry
        if (kb.rKey.wasPressedThisFrame)
        {
            TimeManager.Instance.ResetCurrentAttempt();
        }

        // Flip sprite
        if (moveInput > 0.01f) sr.flipX = false;
        else if (moveInput < -0.01f) sr.flipX = true;
    }

    void FixedUpdate()
    {
        CheckGround();

        if (canControl)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        if (isRecording)
        {
            currentRecording.Add(new RecordedFrame(
                transform.position,
                sr.flipX,
                isGrounded,
                moveInput
            ));
        }
    }

    void CheckGround()
    {
        Vector2 boxCenter = (Vector2)col.bounds.center + Vector2.down * (col.bounds.extents.y + groundCheckDistance * 0.5f);
        Vector2 boxSize = new Vector2(col.bounds.size.x * 0.9f, groundCheckDistance);
        isGrounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer) != null;
    }

    public void StartRecording()
    {
        currentRecording.Clear();
        isRecording = true;
    }

    public List<RecordedFrame> StopRecording()
    {
        isRecording = false;
        return new List<RecordedFrame>(currentRecording);
    }

    public void ResetToSpawn(Vector3 spawnPos)
    {
        transform.position = spawnPos;
        rb.linearVelocity = Vector2.zero;
    }

    public void SetControl(bool enabled)
    {
        canControl = enabled;
        if (!enabled)
        {
            rb.linearVelocity = Vector2.zero;
            moveInput = 0f;
        }
    }

    public void Kill()
    {
        TimeManager.Instance.ResetCurrentAttempt();
    }
}
