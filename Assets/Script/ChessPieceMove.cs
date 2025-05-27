using UnityEngine;

public class ChessPieceMove : MonoBehaviour
{
    [Header("Configura��es de Movimento")]
    public float moveAmount = 1f;
    public float moveSpeed = 5f;

    private bool isMoving = false;
    private bool collided = false;
    private Rigidbody rb;

    private CameraFollow cameraFollow;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void Update()
    {
        if (!isMoving)
        {
            Vector3 inputDir = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W)) inputDir = Vector3.forward;
            else if (Input.GetKeyDown(KeyCode.S)) inputDir = Vector3.back;
            else if (Input.GetKeyDown(KeyCode.A)) inputDir = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.D)) inputDir = Vector3.right;

            if (inputDir != Vector3.zero)
            {
                // Pega o �ngulo atual da c�mera para ajustar a dire��o do movimento
                float camYaw = 0f;
                if (cameraFollow != null)
                    camYaw = cameraFollow.GetCurrentYaw();

                // Rotaciona a dire��o do input pelo �ngulo da c�mera
                Vector3 moveDir = Quaternion.Euler(0, camYaw, 0) * inputDir;

                // Define a pr�xima posi��o em grid
                Vector3 nextPos = transform.position + moveDir.normalized * moveAmount;

                // Verifica se pode se mover (raycast para colis�o)
                if (!Physics.Raycast(transform.position, moveDir, moveAmount * 0.9f))
                {
                    StartCoroutine(MoveToPosition(nextPos));
                }

                
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collided = true;
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 destination)
    {
        isMoving = true;
        collided = false;
        float maxTime = 1.5f; // tempo limite de seguran�a
        float timer = 0f;

        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            if (collided || timer > maxTime)
            {
                Debug.Log("Movimento interrompido por colis�o ou timeout.");
                break;
            }

            Vector3 newPos = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime * 10);
            rb.MovePosition(newPos);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
    }
}
