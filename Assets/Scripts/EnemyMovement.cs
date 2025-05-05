using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private float spawnDelay = 1f;

    [Header("Portal Settings")]
    [SerializeField] private Vector2 portalOffset = new Vector2(-0.35f, 0f);
    [SerializeField] private float portalRotation = 90f;
    [SerializeField] private Vector2 portalScale = Vector2.one;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private Animator enemyAnimator;

    [Header("Attack")]
    [SerializeField] private float attackRange = 0.8f;

    private PlayerController player;
    private bool isActive = false;
    private GameObject portalInstance;
    private Animator portalAnimator;
    private Vector2 movementDirection;
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");

    private void Awake()
    {
        
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        if (player == null)
        {
            Destroy(gameObject);
        }

        spriteRenderer.enabled = false;

        StartCoroutine(SpawnSequence());
    }

    private IEnumerator SpawnSequence()
    {
        Vector2 spawnPosition = (Vector2)transform.position + portalOffset;
        Quaternion rotation = Quaternion.Euler(0, 0, portalRotation);

        portalInstance = Instantiate(portalPrefab, spawnPosition, rotation);
        portalInstance.transform.localScale = new Vector3(
            portalScale.x,
            portalScale.y,
            1f);

        portalAnimator = portalInstance.GetComponent<Animator>();
        portalAnimator.Play("Open");
        yield return new WaitForSeconds(spawnDelay);
        spriteRenderer.enabled = true;
        portalAnimator.Play("Idle");
        yield return new WaitForSeconds(0.5f);
        portalAnimator.Play("Close");
        yield return new WaitForSeconds(0.5f);
        Destroy(portalInstance);
        isActive = true;
    }

    private void Update()
    {
        if (!isActive) return;
        UpdateMovement();
        Attack();
    }

    private void UpdateMovement()
    {
        movementDirection = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + movementDirection * (moveSpeed * Time.deltaTime);
        transform.position = targetPosition;

        if (enemyAnimator != null)
        {
            enemyAnimator.SetFloat(MoveX, movementDirection.x);
            enemyAnimator.SetFloat(MoveY, movementDirection.y);
        }
    }


    private void FollowTarget()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;
    }

    private void Attack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            Death();
        }
    }

    private void Death()
    {
        //particles here

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
