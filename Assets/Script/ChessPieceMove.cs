using UnityEngine;

public class ChessPieceMove : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveAmount = 1f;
    public float moveSpeed = 5f;

    private bool isMoving = false;
    private bool collided = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isMoving)
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W)) direction = Vector3.forward;
            else if (Input.GetKeyDown(KeyCode.S)) direction = Vector3.back;
            else if (Input.GetKeyDown(KeyCode.A)) direction = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.D)) direction = Vector3.right;

            if (direction != Vector3.zero)
            {
                Vector3 nextPos = transform.position + direction * moveAmount;

                // Raycast simples para evitar iniciar movimento se já tem algo no caminho
                if (!Physics.Raycast(transform.position, direction, moveAmount * 0.9f))
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
        float maxTime = 1.5f; // tempo limite de segurança
        float timer = 0f;

        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            if (collided || timer > maxTime)
            {
                Debug.Log("Movimento interrompido por colisão ou timeout.");
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
