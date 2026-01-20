using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float laneChangeDuration = 0.3f;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float gravity = -20.0f;
    [SerializeField] private float slideDuration = 0.8f;
    [SerializeField] private float SlideHeightMultiplier = 0.5f;
    [SerializeField] private float CentreHeightYOffset = 0.5f;

    [Header("Lane Data")]
    [SerializeField] private Transform LeftTransform;
    [SerializeField] private Transform MiddleTransform;
    [SerializeField] private Transform RightTransform;

    [Header("Ground Check")]
    [SerializeField] private bool isPlayerGrounded = false; // For Debug Purpose
    [SerializeField] private Transform GroundCheckTransform;
    [SerializeField] private float GroundCheckRadius = 0.3f;
    [SerializeField] private LayerMask GroundLayerMask;

    [Header("Obstacle Check")]
    [SerializeField] private Transform ObstacleCheckTransform;
    [SerializeField] private float RayDistance;
    [SerializeField] private LayerMask ObstacleLayerMask;

    [Header("Collectible")]
    [SerializeField] private string CoinTag;

    private CharacterController controller;
    private Animator animator;

    private int CurrentLaneIndex = 1; // 0 = Left, 1 = Middle, 2 = Right

    private float VerticalVelocity;
    private float originalControllerHeight;

    private bool isGameOver = false;
    private bool isChangingLane = false;
    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isGameOver = false;

        originalControllerHeight = controller.height;

        transform.position = MiddleTransform.position;
        CurrentLaneIndex = 1;
    }

    void Update()
    {
        Vector3 forwardMove = transform.forward * moveSpeed * Time.deltaTime;

        if (IsGrounded())
        {
            if (VerticalVelocity < 0)
            {
                VerticalVelocity = -2.0f;
                animator.SetBool("Jumping", false);
            }
        }

        VerticalVelocity += gravity * Time.deltaTime;
        Vector3 verticalMove = Vector3.up * VerticalVelocity * Time.deltaTime;
        controller.Move(forwardMove + verticalMove);
    }

    private void FixedUpdate()
    {
        if (IsCollidingWithObstacle() && !isGameOver)
        {
            isGameOver = true;
        }
    }

    private void MoveRight()
    {
        if (CurrentLaneIndex == 2 || isChangingLane)
        {
            Debug.Log("Already in Right Lane");
            return;
        }
        else if (CurrentLaneIndex == 1)
        {
            CurrentLaneIndex++;
        }
        else if (CurrentLaneIndex == 0)
        {
            CurrentLaneIndex = 1;
        }

        StartCoroutine(ChangeLane(GetTargetPosition()));
    }

    private void MoveLeft()
    {
        if (CurrentLaneIndex == 0 || isChangingLane)
        {
            Debug.Log("Already in Left Lane");
            return;
        }
        else if (CurrentLaneIndex == 1)
        {
            CurrentLaneIndex = 0;
        }
        else if (CurrentLaneIndex == 2)
        {
            CurrentLaneIndex = 1;
        }

        StartCoroutine(ChangeLane(GetTargetPosition()));
    }

    private IEnumerator ChangeLane(Vector3 TargetPosition)
    {
        isChangingLane = true;

        float elapsedTime = 0f;
        float startPositionX = transform.position.x;
        float targetPositionX = TargetPosition.x;

        while (elapsedTime < laneChangeDuration)
        {
            float newX = Mathf.Lerp(startPositionX, targetPositionX, (elapsedTime / laneChangeDuration));

            Vector3 moveDelta = new Vector3(newX - transform.position.x, 0, 0);

            controller.Move(moveDelta);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        controller.Move(new Vector3(targetPositionX - transform.position.x, 0, 0));
        isChangingLane = false;
    }

    private Vector3 GetTargetPosition()
    {
        return CurrentLaneIndex switch
        {
            0 => LeftTransform.position,
            1 => MiddleTransform.position,
            2 => RightTransform.position,
            _ => transform.position,
        };
    }

    private void Jump()
    {
        if (!IsGrounded())
        {
            return;
        }

        VerticalVelocity = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        animator.SetBool("Jumping", true);
    }

    private void Slide()
    {
        if (isSliding || !IsGrounded()) return;

        StartCoroutine(PerformSlide());
    }

    private IEnumerator PerformSlide()
    {
        isSliding = true;
        controller.height = originalControllerHeight * SlideHeightMultiplier;
        controller.center = new Vector3(controller.center.x, controller.center.y + CentreHeightYOffset, controller.center.z);

        animator.SetBool("Sliding", true);

        yield return new WaitForSeconds(slideDuration);

        controller.height = originalControllerHeight;
        controller.center = new Vector3(controller.center.x, controller.center.y - CentreHeightYOffset, controller.center.z);

        isSliding = false;
        animator.SetBool("Sliding", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(CoinTag))
        {
            Debug.Log("Collided with " + other.gameObject.name);
        }
    }

    private bool IsCollidingWithObstacle()
    {
        RaycastHit[] RayHit = new RaycastHit[1];
        int hit = Physics.RaycastNonAlloc(ObstacleCheckTransform.position, Vector3.forward, RayHit, RayDistance, ObstacleLayerMask);
        return hit > 0;
    }

    private bool IsGrounded()
    {
        Collider[] hitColliders = new Collider[5];

        int hit = Physics.OverlapSphereNonAlloc(
            GroundCheckTransform.position,
            GroundCheckRadius,
            hitColliders,
            GroundLayerMask
        );

        isPlayerGrounded = hit > 0;
        return isPlayerGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheckTransform.position, GroundCheckRadius);
        Gizmos.DrawRay(ObstacleCheckTransform.position, Vector3.forward * RayDistance);
    }

    #region Input Functions

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 inputValue = context.ReadValue<Vector2>();
            if (inputValue.x < 0)
            {
                MoveLeft();
            }
            else if (inputValue.x > 0)
            {
                MoveRight();
            }
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
    }

    public void OnSlideInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Slide();
        }
    }


    #endregion

}
