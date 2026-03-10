using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class PressurePlate : MonoBehaviour
{
    public Door connectedDoor;
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private int objectsOnPlate;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        TimeManager.OnReset += ResetPlate;
    }

    void OnDisable()
    {
        TimeManager.OnReset -= ResetPlate;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            objectsOnPlate++;
            if (objectsOnPlate == 1)
            {
                Activate();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            objectsOnPlate--;
            if (objectsOnPlate <= 0)
            {
                objectsOnPlate = 0;
                Deactivate();
            }
        }
    }

    void Activate()
    {
        sr.sprite = activeSprite;
        if (connectedDoor != null)
            connectedDoor.SetOpen(true);
    }

    void Deactivate()
    {
        sr.sprite = inactiveSprite;
        if (connectedDoor != null)
            connectedDoor.SetOpen(false);
    }

    void ResetPlate()
    {
        objectsOnPlate = 0;
        Deactivate();
    }
}
