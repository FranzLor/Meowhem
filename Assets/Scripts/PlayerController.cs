using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6.0f;


    private Rigidbody2D rigidBody;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Animator playerAnimator;
    private Vector2 lastMoveDirection = Vector2.down;
    private float moveMagnitude;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();
        playerAnimator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
        UpdateAnimationParameters();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;

        if (movement.magnitude > 0.1f)
        {
            lastMoveDirection = movement;
        }
    }

    private void UpdateAnimationParameters()
    {
        playerAnimator.SetFloat("moveX", movement.x);
        playerAnimator.SetFloat("moveY", movement.y);

        playerAnimator.SetFloat("lastMoveX", lastMoveDirection.x);
        playerAnimator.SetFloat("lastMoveY", lastMoveDirection.y);

        moveMagnitude = movement.magnitude;
        playerAnimator.SetFloat("moveMag", moveMagnitude);
    }

    private void Move()
    {
        rigidBody.MovePosition(rigidBody.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

}
