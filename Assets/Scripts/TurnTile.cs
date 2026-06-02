using UnityEngine;
using UnityEngine.EventSystems;

public class TurnTile : ConveyorTile
{
    [SerializeField] private Direction turnDirection;
    private bool inputWindowOpen = false;
    private Direction selectedDirection;
    


    public void OpenInputWindow()
    {
        inputWindowOpen = true;
        selectedDirection = GetDirection();
    }

    public Direction CloseInputWindowAndGetDirection()
    {
        inputWindowOpen = false;

        return selectedDirection;
    }
    public void Submit()
    {

        if (inputWindowOpen)
        {
            selectedDirection = turnDirection;
        }

    }
}
