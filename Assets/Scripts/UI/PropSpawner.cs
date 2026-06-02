using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject streetLampPrefab;
    [SerializeField] private GameObject treePrefab;

    [Header("Map Tile Size")]
    [SerializeField] private float tileWidth = 40.96f;
    [SerializeField] private float tileHeight = 23.04f;

    [Header("Counts Per Tile")]
    [SerializeField] private int houseCount = 3;
    [SerializeField] private int streetLampCount = 6;
    [SerializeField] private int treeCount = 10;

    [Header("Rules")]
    [SerializeField] private int maxAttemptsPerObject = 30;
    [SerializeField] private float houseMinDistance = 6f;
    [SerializeField] private float streetLampMinDistance = 2f;
    [SerializeField] private float treeMinDistance = 1f;

    [Header("Car Rule")]
    [SerializeField] private float carSpawnChance = 0.4f;
    [SerializeField] private float carMinDistanceFromHouse = 2f;
    [SerializeField] private float carMaxDistanceFromHouse = 5f;

    [Header("Overlap Radius")]
    [SerializeField] private float houseRadius = 1.2f;
    [SerializeField] private float carRadius = 0.8f;
    [SerializeField] private float streetLampRadius = 0.5f;
    [SerializeField] private float treeRadius = 0.7f;

    [Header("Cross Type Min Distance")]
    [SerializeField] private float houseOtherMinDistance = 3f;
    [SerializeField] private float carOtherMinDistance = 2f;
    [SerializeField] private float streetLampOtherMinDistance = 1.5f;
    [SerializeField] private float treeOtherMinDistance = 2f;

    private readonly List<PlacedProp> placedProps = new List<PlacedProp>();

    private struct PlacedProp
    {
        public Vector2 position;
        public float radius;
        public PropType type;

        public PlacedProp(Vector2 position, float radius, PropType type)
        {
            this.position = position;
            this.radius = radius;
            this.type = type;
        }
    }

    private enum PropType
    {
        House,
        Car,
        StreetLamp,
        Tree
    }

    public void SpawnPropsOnTile(Vector3 tileCenter, Transform tilePropsParent)
    {
        ClearChildren(tilePropsParent);
        placedProps.Clear();

        SpawnHouses(tileCenter, tilePropsParent);
        SpawnStreetLamps(tileCenter, tilePropsParent);
        SpawnTrees(tileCenter, tilePropsParent);
    }

    private void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private void SpawnHouses(Vector3 tileCenter, Transform parent)
    {
        for (int i = 0; i < houseCount; i++)
        {
            if (TryFindPosition(tileCenter, houseRadius, PropType.House, out Vector2 pos))
            {
                SpawnProp(housePrefab, pos, houseRadius, PropType.House, parent);

                if (Random.value <= carSpawnChance)
                {
                    TrySpawnCarNearHouse(pos, parent);
                }
            }
        }
    }

    private void TrySpawnCarNearHouse(Vector2 housePos, Transform parent)
    {
        for (int attempt = 0; attempt < maxAttemptsPerObject; attempt++)
        {
            Vector2 pos = GetRandomPointAround(housePos, carMinDistanceFromHouse, carMaxDistanceFromHouse);

            if (IsOverlappingAny(pos, carRadius, PropType.Car))
                continue;

            SpawnProp(carPrefab, pos, carRadius, PropType.Car, parent);
            return;
        }
    }

    private void SpawnStreetLamps(Vector3 tileCenter, Transform parent)
    {
        for (int i = 0; i < streetLampCount; i++)
        {
            if (TryFindPosition(tileCenter, streetLampRadius, PropType.StreetLamp, out Vector2 pos))
            {
                SpawnProp(streetLampPrefab, pos, streetLampRadius, PropType.StreetLamp, parent);
            }
        }
    }

    private void SpawnTrees(Vector3 tileCenter, Transform parent)
    {
        for (int i = 0; i < treeCount; i++)
        {
            if (TryFindPosition(tileCenter, treeRadius, PropType.Tree, out Vector2 pos))
            {
                SpawnProp(treePrefab, pos, treeRadius, PropType.Tree, parent);
            }
        }
    }

    private bool TryFindPosition(Vector3 tileCenter, float radius, PropType type, out Vector2 result)
    {
        result = Vector2.zero;

        for (int attempt = 0; attempt < maxAttemptsPerObject; attempt++)
        {
            Vector2 pos = GetRandomPointInTile(tileCenter);

            if (IsOverlappingAny(pos, radius, type))
                continue;

            if (type == PropType.House && IsTooCloseToSameType(pos, PropType.House, houseMinDistance))
                continue;

            if (type == PropType.StreetLamp && IsTooCloseToSameType(pos, PropType.StreetLamp, streetLampMinDistance))
                continue;

            if (type == PropType.Tree && IsTooCloseToSameType(pos, PropType.Tree, treeMinDistance))
                continue;

            result = pos;
            return true;
        }

        return false;
    }

    private bool IsOverlappingAny(Vector2 pos, float radius, PropType type)
    {
        for (int i = 0; i < placedProps.Count; i++)
        {
            PlacedProp prop = placedProps[i];

            float minDistance = radius + prop.radius;
            minDistance += GetExtraSpacing(type, prop.type);

            if (Vector2.Distance(pos, prop.position) < minDistance)
            {
                return true;
            }
        }

        return false;
    }

    private float GetExtraSpacing(PropType a, PropType b)
    {
        if (a == b) return 0f;

        if (a == PropType.House || b == PropType.House)
            return houseOtherMinDistance;

        if (a == PropType.Car || b == PropType.Car)
            return carOtherMinDistance;

        if (a == PropType.Tree || b == PropType.Tree)
            return treeOtherMinDistance;

        if (a == PropType.StreetLamp || b == PropType.StreetLamp)
            return streetLampOtherMinDistance;

        return 0f;
    }

    private bool IsTooCloseToSameType(Vector2 pos, PropType type, float minDistance)
    {
        for (int i = 0; i < placedProps.Count; i++)
        {
            PlacedProp prop = placedProps[i];

            if (prop.type != type) continue;

            if (Vector2.Distance(pos, prop.position) < minDistance)
                return true;
        }

        return false;
    }

    private void SpawnProp(GameObject prefab, Vector2 pos, float radius, PropType type, Transform parent)
    {
        if (prefab == null) return;

        Instantiate(prefab, pos, Quaternion.identity, parent);
        placedProps.Add(new PlacedProp(pos, radius, type));
    }

    private Vector2 GetRandomPointInTile(Vector3 tileCenter)
    {
        float x = Random.Range(tileCenter.x - tileWidth / 2f, tileCenter.x + tileWidth / 2f);
        float y = Random.Range(tileCenter.y - tileHeight / 2f, tileCenter.y + tileHeight / 2f);

        return new Vector2(x, y);
    }

    private Vector2 GetRandomPointAround(Vector2 center, float minRadius, float maxRadius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(minRadius, maxRadius);

        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return center + dir * distance;
    }
}