using System.Collections;
using UnityEngine;

public class TurnButton : MonoBehaviour
{
    [SerializeField] TurnTile[] targetTiles;
    private bool isPressing = false;
    private float pressDepth = 0.2f;
    private Vector3 startPosition;
    private Vector3 pressedPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
        pressedPosition = startPosition + (Vector3.down * pressDepth);
    }

    private void OnMouseDown()
    {
        if (!isPressing)
        {
            StartCoroutine(ButtonPress());

            foreach (TurnTile tile in targetTiles)
            {
                if (tile != null)
                {
                    tile.Submit();
                }
            }
        }
    }

    public IEnumerator ButtonPress()
    {
        isPressing = true;

        while (Vector3.Distance(transform.localPosition, pressedPosition) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, pressedPosition, 5.0f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        while (Vector3.Distance(transform.localPosition, startPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, 2.0f * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = startPosition; 

        isPressing = false;


    }

}
