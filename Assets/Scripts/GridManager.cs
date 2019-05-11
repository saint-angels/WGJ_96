using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : SingletonComponent<GridManager>
{
    public const int width = 10;
    public const int height = 10;

    private GridObjectBase[,] grid = new GridObjectBase[width, height];

    public bool SpawnObject(GridObjectBase newObjectPrefab, Vector2Int position)
    {
        bool canSpawn = IsFree(position, newObjectPrefab.size);

        if (canSpawn)
        {
            GridObjectBase newPiece = Instantiate(newObjectPrefab, transform);
            newPiece.UpdatePosition(position);

            ForRectDo(position, newPiece.size, (x,y) =>
            {
                grid[x, y] = newPiece;
            });
        }

        return canSpawn;
    }

    public bool IsFree(Vector2Int position, Vector2Int size)
    {
        bool isFree = true;

        ForRectDo(position, size, (x, y) =>
        {
            if (grid[x, y] != null)
            {
                isFree = false;
            }
        });
        return isFree;
    }

    private void ForRectDo(Vector2Int position, Vector2Int size, Action<int,int> action)
    {
        for (int i = position.x; i < position.x + size.x; i++)
        {
            for (int j = position.y; j < position.y + size.y; j++)
            {
                action(i, j);
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
