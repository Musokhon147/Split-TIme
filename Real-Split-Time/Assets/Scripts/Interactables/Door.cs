using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Door : MonoBehaviour
{
    public Sprite openSprite;
    public Sprite closedSprite;

    private BoxCollider2D col;
    private SpriteRenderer sr;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        TimeManager.OnReset += Close;
    }

    void OnDisable()
    {
        TimeManager.OnReset -= Close;
    }

    public void SetOpen(bool open)
    {
        if (open)
        {
            col.enabled = false;
            sr.sprite = openSprite;
            Color c = sr.color;
            c.a = 0.3f;
            sr.color = c;
        }
        else
        {
            col.enabled = true;
            sr.sprite = closedSprite;
            Color c = sr.color;
            c.a = 1.0f;
            sr.color = c;
        }
    }

    void Close()
    {
        SetOpen(false);
    }
}
