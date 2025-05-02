using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.0f;

    private PlayerController player;


    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();

        if (player == null)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;
    }
}
