using UnityEngine;

public class BlackHole : MonoBehaviour
{

    [SerializeField] private Colour colour;
    private bool isCorrectHole = false;
    
    public void OnItemReached(ItemMover item)
    {
        if (item.GetColour() == colour)
        {
            isCorrectHole = true;
        }
        item.FallIntoHole(transform.position, isCorrectHole);      
    }
}
