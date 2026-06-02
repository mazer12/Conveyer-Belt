using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject[] levels;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private ItemMover item;

    private int currentLevelIndex = 0;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }
    private void Start()
    {
        LoadLevel(0);
    }

    public void CompleteLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
        {
            Debug.Log("Game Complete!");
            return;
        }

        LoadLevel(currentLevelIndex);
    }

    public void ResetCurrentLevel()
    {
        item.ResetToCurrentLevelStart();
    }

    private void LoadLevel(int index)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(i == index);
        }


        gridManager.SetActiveLevel(levels[index].transform);

        item = levels[index].GetComponentInChildren<ItemMover>();
        item.InitializeLevelStart();

        Debug.Log("Loaded Level " + (index + 1));
    }
}