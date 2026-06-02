using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ItemMover : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float fallDepth = 1f;
    [SerializeField] private float fallDuration = 2f;
    [SerializeField] private float turnDecisionTime = 0.7f;
    [SerializeField] private Colour colour;
        public Colour GetColour() => colour;

    private bool isMoving = false;
    private bool hasEnded = false;

    private Vector2Int currentGridPosition;
    private Vector2Int startGridPosition;
    private Vector3 startWorldPosition;
    private Vector3 startScale;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            StartCoroutine(MoveLoop());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetToCurrentLevelStart();
        }
    }

    private IEnumerator MoveLoop()
    {
        isMoving = true;

        while (!hasEnded)
        {
            BlackHole hole = GridManager.Instance.GetHoleAt(currentGridPosition);

            if (hole != null)
            {
                hole.OnItemReached(this);
                yield break;
            }

            Teleporter teleporter = GridManager.Instance.GetTeleporterAt(currentGridPosition);

            if (teleporter != null)
            {
                Vector3 exitWorldPos = teleporter.GetExitPosition();
                exitWorldPos.z += 1; //add 1 to transport it directly to the conveyor belt
                transform.position = exitWorldPos;

                currentGridPosition = GridManager.Instance.WorldToGrid(exitWorldPos);

                yield return new WaitForSeconds(0.2f);
            }

            ConveyorTile currentConveyor = GridManager.Instance.GetConveyorAt(currentGridPosition);

            if (currentConveyor == null)
            {
                Debug.Log("You lost, Try again!");
                LevelManager.Instance.ResetCurrentLevel();
                yield break;
            }

            

            Direction nextDirection = currentConveyor.GetDirection();

            TurnTile turnTile = currentConveyor.GetComponent<TurnTile>();

            if (turnTile != null)
            {
                turnTile.OpenInputWindow();

                yield return new WaitForSeconds(turnDecisionTime);

                nextDirection = turnTile.CloseInputWindowAndGetDirection();
            }
 

            Vector2Int nextGridPosition = currentGridPosition + DirectionToGridOffset(nextDirection);

            Vector3 targetWorldPosition = GridManager.Instance.GridToWorld(nextGridPosition);
            targetWorldPosition.y = 0.5f;

            yield return MoveToPosition(targetWorldPosition);

            currentGridPosition = nextGridPosition;
        }

        isMoving = false;
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPosition;
    }

    public void FallIntoHole(Vector3 holePosition, bool _isCorrectHole)
    {
        if (hasEnded)
        {
            return;
        }

        hasEnded = true;
        StartCoroutine(FallRoutine(holePosition, _isCorrectHole));
    }

    private IEnumerator FallRoutine(Vector3 holePosition, bool _isCorrectHole)
    {
        Vector3 startPosition = transform.position;

        Vector3 endPosition = new Vector3(
            holePosition.x,
            holePosition.y - fallDepth,
            holePosition.z
        );

        Vector3 initialScale = transform.localScale;
        Vector3 finalScale = Vector3.zero;

        float timer = 0f;

        while (timer < fallDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fallDuration;

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            transform.localScale = Vector3.Lerp(initialScale, finalScale, t);

            yield return null;
        }

        if (!_isCorrectHole)
        {
            LevelManager.Instance.ResetCurrentLevel();
        }
        else {
            LevelManager.Instance.CompleteLevel();
        }   
    }

    private Vector2Int DirectionToGridOffset(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector2Int(0, 1);

            case Direction.Down:
                return new Vector2Int(0, -1);

            case Direction.Left:
                return new Vector2Int(-1, 0);

            case Direction.Right:
                return new Vector2Int(1, 0);

            default:
                return Vector2Int.zero;
        }
    }

    public void InitializeLevelStart()
    {
        StopAllCoroutines();

        hasEnded = false;
        isMoving = false;

        startWorldPosition = transform.position;
        startScale = transform.localScale;

        startGridPosition = GridManager.Instance.WorldToGrid(startWorldPosition);
        currentGridPosition = startGridPosition;

        transform.position = startWorldPosition;
        transform.localScale = startScale;
    }

    public void ResetToCurrentLevelStart()
    {
        StopAllCoroutines();

        hasEnded = false;
        isMoving = false;

        transform.position = startWorldPosition;
        transform.localScale = startScale;

        currentGridPosition = startGridPosition;
    }
}
