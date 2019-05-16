using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerGun gunPrefab = null;

    private PlayerGun gun;
    
    void Start()
    {
        gun = GridManager.Instance.SpawnObject(gunPrefab, Vector2Int.zero, Color.white) as PlayerGun;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.UpArrow)) Move(Vector2Int.up);
        //else if (Input.GetKeyDown(KeyCode.DownArrow)) Move(Vector2Int.down);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Move(Vector2Int.right);
        else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) gun.Shoot();
    }

    private void Move(Vector2Int direction)
    {
        if (GridManager.Instance.IsFree(gun.Position + direction, gun.size))
        {
            GridManager.Instance.MoveObject(gun, gun.Position + direction);
        }
    }
}
