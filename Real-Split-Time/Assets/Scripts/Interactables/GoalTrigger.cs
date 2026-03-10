using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GoalTrigger : MonoBehaviour
{
    private bool triggered;

    void OnEnable()
    {
        TimeManager.OnReset += ResetGoal;
    }

    void OnDisable()
    {
        TimeManager.OnReset -= ResetGoal;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            TimeManager.Instance.OnLevelComplete();
        }
    }

    void ResetGoal()
    {
        triggered = false;
    }
}
