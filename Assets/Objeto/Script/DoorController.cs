using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Posicion")]
    [SerializeField] private Vector3 closedPosition;
    [SerializeField] private Vector3 openPosition;

    [Header("Configuración")]
    [SerializeField] private float speed = 5f;

    private bool shouldBeOpen = false;

    void Reset()
    {

        closedPosition = transform.localPosition;
        openPosition = closedPosition + Vector3.up * 2f;
    }

    void Update()
    {
        Vector3 target = shouldBeOpen ? openPosition : closedPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
    }

    public void Open()
    {
        shouldBeOpen = true;
    }

    public void Close()
    {
        shouldBeOpen = false;
    }
}
