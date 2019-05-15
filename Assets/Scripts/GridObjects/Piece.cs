using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : GridObjectBase
{
    public int health = 3;
    public int stunLeft = 0;

    public void Damage(int damage, int shotPush, int stun)
    {

        stunLeft = stun;
        health -= damage;

        GridManager.Instance.MoveObjectYMax(this, new Vector2Int(Position.x, Position.y + shotPush));


        if (health <= 0)
        {
            GridManager.Instance.DestroyPiece(this);
        }
    }
        
}
