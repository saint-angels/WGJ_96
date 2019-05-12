using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : GridObjectBase
{
    public Vector2Int fireArea = new Vector2Int(1, 3);

    public void Shoot()
    {
        Vector2Int shootOrigin = new Vector2Int(Position.x, Position.y + size.y);
        Piece foundPiece = GridManager.Instance.GetFirstObjectInLine(shootOrigin, fireArea) as Piece;
        if (foundPiece != null)
        {
            foundPiece.Damage(1);
            print($"damaging piece {foundPiece.gameObject.name}");
        }
    }
}
