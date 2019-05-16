using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : GridObjectBase
{
    public int health = 3;
    public int stunLeft = 0;

    public void Init(Color color)
    {
        foreach (var rendw in GetComponentsInChildren<SpriteRenderer>())
        {
            rendw.color = color;
        }
        
    }

    public void Damage(int damage, int shotPushHeigh, int stun)
    {
        stunLeft = stun;
        health -= damage;

        int pushbackHeight = Mathf.Max(Position.y, shotPushHeigh);
        GridManager.Instance.MoveObjectYMax(this, new Vector2Int(Position.x, pushbackHeight));


        if (health <= 0)
        {
            GridManager.Instance.DestroyPiece(this);
        }
    }
        
}
