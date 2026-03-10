using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class CloneController : MonoBehaviour
{
    private List<RecordedFrame> frames;
    private int currentFrame;
    private bool isPlaying;
    private bool finished;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init(List<RecordedFrame> recording, Sprite cloneSprite)
    {
        frames = recording;
        currentFrame = 0;
        isPlaying = true;
        finished = false;

        sr.sprite = cloneSprite;
        sr.color = new Color(0.4f, 1.0f, 0.7f, 0.75f);
        sr.sortingOrder = 5;

        rb.bodyType = RigidbodyType2D.Kinematic;

        if (frames.Count > 0)
        {
            transform.position = frames[0].position;
        }
    }

    void FixedUpdate()
    {
        if (!isPlaying || frames == null) return;

        if (currentFrame < frames.Count)
        {
            rb.MovePosition(frames[currentFrame].position);
            sr.flipX = frames[currentFrame].flipX;
            currentFrame++;
        }
        else
        {
            if (!finished)
            {
                finished = true;
                // Stay at last position
            }
        }
    }

    public void ResetPlayback()
    {
        currentFrame = 0;
        finished = false;
        isPlaying = true;
    }
}
