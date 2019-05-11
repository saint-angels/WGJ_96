using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : SingletonComponent<GridManager>
{
    public event Action OnGridTick = () => { };

    public const int width = 10;
    public const int height = 10;
    public float tickDuration = 1f;

    private GridObjectBase[,] grid = new GridObjectBase[width, height];
    private List<Piece> activePieces = new List<Piece>();

    public GridObjectBase SpawnObject(GridObjectBase newObjectPrefab, Vector2Int position)
    {
        bool canSpawn = IsFree(position, newObjectPrefab.size);

        if (canSpawn)
        {
            GridObjectBase newObject = Instantiate(newObjectPrefab, transform);
            newObject.UpdatePosition(position);
            if (newObject is Piece)
            {
                activePieces.Add((Piece)newObject);
            }

            ForRectDo(position, newObject.size, (x,y) =>
            {
                grid[x, y] = newObject;
            });
            return newObject;
        }
        else
        {
            return null;
        }
    }



    public bool IsFree(Vector2Int position, Vector2Int size)
    {
        bool isFree = true;

        bool outOfBounds = position.x + size.x > width
            || position.x < 0
            || position.y + size.y > height
            || position.y < 0;
        if (outOfBounds)
        {
            return false;
        }

        ForRectDo(position, size, (x, y) =>
        {
            if (grid[x, y] != null)
            {
                isFree = false;
            }
        });
        return isFree;
    }

    public void MoveObject(GridObjectBase movedObject, Vector2Int newPosition)
    {
        //Remove piece from prev location
        ForRectDo(movedObject.Position, movedObject.size, (x, y) =>
        {
            if (grid[x, y] == movedObject)
            {
                grid[x, y] = null;
            }
        });

        //Move piece to new position
        movedObject.UpdatePosition(newPosition);
        ForRectDo(movedObject.Position, movedObject.size, (x, y) =>
        {
            grid[x, y] = movedObject;
        });
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

    private IEnumerator GridTickRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickDuration);
            UpdateGravity();
            OnGridTick();
        }
    }

    private void UpdateGravity()
    {
        foreach (var piece in activePieces)
        {
            bool pieceFalling = true;
            Vector2Int pieceFeetRect = new Vector2Int(piece.size.x, 1);
            ForRectDo(piece.Position, pieceFeetRect, (x, y) =>
            {
                bool pieceHasPartAtPoint = grid[x, y] == piece;
                bool pieceOnTheFloor = pieceHasPartAtPoint && y == 0;
                bool pieceOnOther = pieceHasPartAtPoint && y > 0 && grid[x, y - 1] != null;
                if (pieceOnTheFloor || pieceOnOther)
                {
                    pieceFalling = false;
                }
            });

            if (pieceFalling)
            {
                MoveObject(piece, piece.Position - Vector2Int.up);
            }
        }
    }

    void Start()
    {
        StartCoroutine(GridTickRoutine());
    }

    void Update()
    {
        
    }
}
