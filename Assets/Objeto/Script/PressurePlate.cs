using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private string[] activatingTags = new[] { "Player" };

    [Header("Eventos")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private int overlappingCount = 0;
    private bool isPressed = false;

    private bool TagMatches(GameObject go)
    {
        foreach (var t in activatingTags)
            if (go.CompareTag(t))
                return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!TagMatches(other.gameObject)) return;
        overlappingCount++;
        if (!isPressed)
        {
            isPressed = true;
            onPressed?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!TagMatches(other.gameObject)) return;
        overlappingCount = Mathf.Max(0, overlappingCount - 1);
        if (overlappingCount == 0 && isPressed)
        {
            isPressed = false;
            onReleased?.Invoke();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = isPressed ? Color.green : Color.red;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
