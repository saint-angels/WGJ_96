using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : SingletonComponent<GridManager>
{
    public event Action OnPlayerDeath = () => { };
    public event Action OnGridTick = () => { };

    [SerializeField] private GameObject gridBlockBackgroundPrefab;

    public const int width = 7;
    public const int height = 10;
    public float tickDuration = 1f;

    private GridObjectBase[,] grid = new GridObjectBase[width, height];
    private List<Piece> activePieces = new List<Piece>();


    public void Init()
    {
        for (int i = activePieces.Count - 1; i >= 0; i--)
        {
            DestroyPiece(activePieces[i]);
        }

        StartCoroutine(GridTickRoutine());
    }

    public GridObjectBase GetFirstObjectInLine(Vector2Int startOrigin, Vector2Int rect)
    {
        GridObjectBase foundObject = null;

        ForGridRectDo(startOrigin, rect, (x, y) =>
        {
            if (grid[x,y] != null && foundObject == null)
            {
                foundObject = grid[x, y];
            }
        });

        return foundObject;
    }


    public GridObjectBase SpawnObject(GridObjectBase newObjectPrefab, Vector2Int position, Color colorNormal, Color colorLinked)
    {
        bool canSpawn = IsFree(position, newObjectPrefab.size);

        if (canSpawn)
        {
            GridObjectBase newObject = Instantiate(newObjectPrefab, transform);
            newObject.UpdatePosition(position);
            if (newObject is Piece)
            {
                (newObject as Piece).Init(colorNormal, colorLinked);
                activePieces.Add((Piece)newObject);
            }

            ForGridRectDo(position, newObject.size, (x,y) =>
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

        ForGridRectDo(position, size, (x, y) =>
        {
            if (grid[x, y] != null)
            {
                isFree = false;
            }
        });
        return isFree;
    }

    public void MoveObjectYMax(GridObjectBase movedObject, Vector2Int maxPosition)
    {
        int objectX = movedObject.Position.x;

        for (int y = movedObject.Position.y + 1; y <= maxPosition.y; y++)
        {
            Vector2Int newPosition = new Vector2Int(objectX, y);
            bool freeSpot = IsFree(newPosition, movedObject.size);
            if (freeSpot)
            {
                MoveObject(movedObject, newPosition);
            }
            else
            {
                break;
            }
        }
    }

    public void MoveObject(GridObjectBase movedObject, Vector2Int newPosition)
    {
        //Remove piece from prev location
        ForGridRectDo(movedObject.Position, movedObject.size, (x, y) =>
        {
            if (grid[x, y] == movedObject)
            {
                grid[x, y] = null;
            }
        });

        //Move piece to new position
        movedObject.UpdatePosition(newPosition);
        ForGridRectDo(movedObject.Position, movedObject.size, (x, y) =>
        {
            grid[x, y] = movedObject;
        });
    }

    public void DestroyPieces(List<Piece> pieces)
    {
        for (int i = pieces.Count - 1; i >= 0; i--)
        {
            DestroyPiece(pieces[i]);
        }
    }

    public void DestroyPiece(Piece piece)
    {
        ForGridObjectRectDo(piece, (x, y) =>
        {
            if (grid[x, y] == piece)
            {
                grid[x, y] = null;
            }
            
        });
        activePieces.Remove(piece);

        Destroy(piece.gameObject);
    }

    private void ForGridObjectRectDo(GridObjectBase gridObject, Action<int, int> action)
    {
        ForGridRectDo(gridObject.Position, gridObject.size, action);
    }

    private void ForGridRectDo(Vector2Int position, Vector2Int size, Action<int,int> action)
    {
        for (int i = position.x; i < position.x + size.x; i++)
        {
            for (int j = position.y; j < position.y + size.y; j++)
            {
                if (i >= 0 && j >= 0 && i < width && j < height)
                {
                    action(i, j);
                }
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
        List<Piece> piecesToDestroy = new List<Piece>();

        foreach (var piece in activePieces)
        {
            piece.ClearLinkedPieces();

            if (piece.stunLeft > 0)
            {
                piece.stunLeft--;
                continue;
            }

            bool pieceFalling = true;
            bool pieceOnTheFloor = false;
            bool playerSmashed = false;
            Vector2Int pieceFeetRect = new Vector2Int(piece.size.x, 1);
            ForGridRectDo(piece.Position, pieceFeetRect, (x, y) =>
            {
                bool pieceHasPartAtPoint = grid[x, y] == piece;
                pieceOnTheFloor = pieceHasPartAtPoint && y == 0;
                bool pieceOnOther = pieceHasPartAtPoint && y > 0 && grid[x, y - 1] != null;
                if (pieceOnTheFloor == false && grid[x, y - 1] is PlayerGun)
                {
                    playerSmashed = true;
                }
                else if (pieceOnTheFloor || pieceOnOther)
                {
                    pieceFalling = false;
                }

                if (pieceOnOther)
                {
                    var bottomPiece = grid[x, y - 1] as Piece;
                    if (bottomPiece != null)
                    {
                        bottomPiece.SmashedByOtherPiece();
                    }
                }
                
            });

            if (playerSmashed)
            {
                OnPlayerDeath();
                return;
            }
            else if (pieceFalling)
            {
                MoveObject(piece, piece.Position - Vector2Int.up);
            }
            else if (pieceOnTheFloor)
            {
                piecesToDestroy.Add(piece);
            }
        }

        foreach (var piece in activePieces)
            ForGridRectDo(piece.Position - Vector2Int.one, piece.size + Vector2Int.one * 2, (x, y) =>
            {
                Piece anotherPiece = grid[x, y] as Piece;
                if (anotherPiece != null && anotherPiece != piece && piece.linkedPieces.Contains(anotherPiece) == false)
                {
                    piece.LinkPiece(anotherPiece);
                }

            });
        { }

        //Destroy pieces on the floor
        for (int i = piecesToDestroy.Count - 1; i >= 0; i--)
        {
            DestroyPiece(piecesToDestroy[i]);
        }
    }

    void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var newGridBlock = Instantiate(gridBlockBackgroundPrefab, transform);
                newGridBlock.transform.position = new Vector3(x, y, 0);
            }
        }
    }

    void Update()
    {
        
    }
}
