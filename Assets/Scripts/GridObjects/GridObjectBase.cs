using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectBase : MonoBehaviour
{
    public Vector2Int Position
    {
        get
        {
            return new Vector2Int((int)transform.position.x, (int)transform.position.y);
        }
    }

    public Vector2Int size = Vector2Int.one;

    public void UpdatePosition(Vector2Int newPosition)
    {
        transform.position = new Vector3(newPosition.x, newPosition.y, 0);   
    }
}
