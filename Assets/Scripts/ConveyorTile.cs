using UnityEngine;

public class ConveyorTile : MonoBehaviour
{

    [SerializeField] private Direction moveDirection;
      public Direction GetDirection() => moveDirection;
      public void SetDirection(Direction direction) => moveDirection = direction;


}