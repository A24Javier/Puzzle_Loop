using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Header("Eventos")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private int overlappingCount = 0;
    private bool isPressed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Phantom"))
        {
            return;
        }
        
        overlappingCount++;
        if (!isPressed)
        {
            isPressed = true;
            onPressed?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Phantom"))
        {
            return;
        }
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
