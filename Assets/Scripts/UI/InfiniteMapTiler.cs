using System.Collections.Generic;
using UnityEngine;

public class InfiniteMapTiler : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject mapTilePrefab;
    [SerializeField] private Transform mapTilesParent;
    [SerializeField] private PropSpawner propSpawner;

    [SerializeField] private float tileWidth = 40.96f;
    [SerializeField] private float tileHeight = 23.04f;

    [SerializeField] private int gridRadius = 2;

    private class TileSlot
    {
        public GameObject tileObject;
        public Transform propsParent;
        public Vector2Int coord;
    }

    private readonly List<TileSlot> tiles = new List<TileSlot>();
    private Vector2Int currentCenterTile;

    private void Start()
    {
        currentCenterTile = GetTileCoord(player.position);
        GenerateInitialTiles();
    }

    private void Update()
    {
        Vector2Int playerTile = GetTileCoord(player.position);

        if (playerTile != currentCenterTile)
        {
            currentCenterTile = playerTile;
            RepositionOnlyOutOfRangeTiles();
        }
    }

    private void GenerateInitialTiles()
    {
        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                Vector2Int coord = currentCenterTile + new Vector2Int(x, y);
                CreateTile(coord);
            }
        }
    }

    private void CreateTile(Vector2Int coord)
    {
        Vector3 pos = TileCoordToWorldPosition(coord);

        GameObject tile = Instantiate(mapTilePrefab, pos, Quaternion.identity, mapTilesParent);

        GameObject propsRoot = new GameObject("Props");
        propsRoot.transform.SetParent(tile.transform);
        propsRoot.transform.localPosition = Vector3.zero;

        TileSlot slot = new TileSlot
        {
            tileObject = tile,
            propsParent = propsRoot.transform,
            coord = coord
        };

        tiles.Add(slot);

        if (propSpawner != null)
        {
            propSpawner.SpawnPropsOnTile(pos, slot.propsParent);
        }
    }

    private void RepositionOnlyOutOfRangeTiles()
    {
        HashSet<Vector2Int> desiredCoords = new HashSet<Vector2Int>();

        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                desiredCoords.Add(currentCenterTile + new Vector2Int(x, y));
            }
        }

        HashSet<Vector2Int> occupiedCoords = new HashSet<Vector2Int>();

        foreach (TileSlot slot in tiles)
        {
            if (desiredCoords.Contains(slot.coord))
            {
                occupiedCoords.Add(slot.coord);
            }
        }

        Queue<Vector2Int> missingCoords = new Queue<Vector2Int>();

        foreach (Vector2Int coord in desiredCoords)
        {
            if (!occupiedCoords.Contains(coord))
            {
                missingCoords.Enqueue(coord);
            }
        }

        foreach (TileSlot slot in tiles)
        {
            if (desiredCoords.Contains(slot.coord))
                continue;

            if (missingCoords.Count == 0)
                return;

            Vector2Int newCoord = missingCoords.Dequeue();
            Vector3 newPos = TileCoordToWorldPosition(newCoord);

            slot.coord = newCoord;
            slot.tileObject.transform.position = newPos;

            if (propSpawner != null)
            {
                propSpawner.SpawnPropsOnTile(newPos, slot.propsParent);
            }
        }
    }

    private Vector2Int GetTileCoord(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / tileWidth);
        int y = Mathf.FloorToInt(worldPos.y / tileHeight);

        return new Vector2Int(x, y);
    }

    private Vector3 TileCoordToWorldPosition(Vector2Int coord)
    {
        return new Vector3(coord.x * tileWidth, coord.y * tileHeight, 0f);
    }
}