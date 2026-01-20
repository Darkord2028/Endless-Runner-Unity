using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float laneChangeDuration = 0.3f;

    [SerializeField] private Transform LeftTransform;
    [SerializeField] private Transform MiddleTransform;
    [SerializeField] private Transform RightTransform;

    private int CurrentLaneIndex = 1; // 0 = Left, 1 = Middle, 2 = Right
    private bool isChangingLane = false;

    void Start()
    {
        transform.position = MiddleTransform.position;
        CurrentLaneIndex = 1;
    }

    void Update()
    {
        
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

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < laneChangeDuration)
        {
            transform.position = Vector3.Lerp(startPosition, TargetPosition, (elapsedTime / laneChangeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = TargetPosition;
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

    #endregion

}
