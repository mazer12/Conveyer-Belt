using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    private readonly Dictionary<Vector2Int, ConveyorTile> conveyors = new();
    private readonly Dictionary<Vector2Int, BlackHole> holes = new();
    private Dictionary<Vector2Int, Teleporter> teleporters = new();

    [SerializeField] private Transform levelRoot;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        NewLevelSetup();
    }
    private void NewLevelSetup()
    {
        conveyors.Clear();
        holes.Clear();
        teleporters.Clear();

        ConveyorTile[] conveyorTiles = levelRoot.GetComponentsInChildren<ConveyorTile>(true);
        BlackHole[] blackHoles = levelRoot.GetComponentsInChildren<BlackHole>(true);
        Teleporter[] teleporterHoles = levelRoot.GetComponentsInChildren<Teleporter>();

        foreach (Teleporter teleporter in teleporterHoles)
        {
            Vector2Int gridPosition = WorldToGrid(teleporter.transform.position);

            if (!teleporters.ContainsKey(gridPosition))
            {
                teleporters.Add(gridPosition, teleporter);
            }
        }


        foreach (ConveyorTile conveyor in conveyorTiles)
        {
            Vector2Int gridPosition = WorldToGrid(conveyor.transform.position);

            if (!conveyors.ContainsKey(gridPosition))
            {
                conveyors.Add(gridPosition, conveyor);
            }

        }

        foreach (BlackHole hole in blackHoles)
        {
            Vector2Int gridPosition = WorldToGrid(hole.transform.position);

            if (!holes.ContainsKey(gridPosition))
            {
                holes.Add(gridPosition, hole);
            }

        }
    }

    public void SetActiveLevel(Transform newActiveLevelRoot)
    {
        levelRoot = newActiveLevelRoot;
        NewLevelSetup();
    }
    public ConveyorTile GetConveyorAt(Vector2Int _position)
    {
        conveyors.TryGetValue(_position, out ConveyorTile _tile);
        return _tile;
    }

    public BlackHole GetHoleAt(Vector2Int _position)
    {
        holes.TryGetValue(_position, out BlackHole _hole);
        return _hole;
    }

    public Teleporter GetTeleporterAt(Vector2Int gridPosition)
    {
        teleporters.TryGetValue(gridPosition, out Teleporter teleporter);
        return teleporter;
    }

    public Vector3 GridToWorld(Vector2Int _gridPosition)
    {
        return new Vector3(_gridPosition.x, 0f, _gridPosition.y);
    }

    public Vector2Int WorldToGrid(Vector3 _position)
    {
        return new Vector2Int((int)_position.x, (int)_position.z);
    }
    
}
