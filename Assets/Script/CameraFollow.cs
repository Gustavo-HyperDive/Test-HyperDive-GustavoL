using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0f, 10f, -10f);
    public float distance = 1f;
    public float smoothSpeed = 5f;

    private float rotationStepAngle = 90f;
    private float currentYaw = 0f; //Rota��o atual em torno do eixo Y.
    private bool isRotating = false;
    private Vector3 currentOffset;

    void Start()
    {
        UpdateOffset();
        transform.position = target.position + currentOffset;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    void LateUpdate()
    {
        if (target == null) return;

        //Rata��o da C�mera
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentYaw -= rotationStepAngle;
            UpdateOffset();
            isRotating = true;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentYaw += rotationStepAngle;
            UpdateOffset();
            isRotating = true;
        }

        Vector3 desiredPosition = target.position + currentOffset;

        if (isRotating) // Rota��o Suave 
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, desiredPosition) < 0.05f)
            {
                transform.position = desiredPosition;
                isRotating = false;
            }
        }
        else
        {
            // Movimento instant�neo se n�o estiver rotacionando
            transform.position = desiredPosition;
        }

        // Mant�m a rota��o sempre voltada para o player
        Quaternion baseRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = baseRotation;
    }

    void UpdateOffset()
    {
        Vector3 scaledOffset = offset.normalized * offset.magnitude * distance;
        currentOffset = Quaternion.Euler(0f, currentYaw, 0f) * scaledOffset;
    }

    //Envia o �ngulo atual da c�mera para ajustar a dire��o do movimento no outro script (ChessPieceMove)
    public float GetCurrentYaw()
    {
        return currentYaw;
    }
}
