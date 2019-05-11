using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerGun gunPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GridManager.Instance.SpawnObject(gunPrefab, Vector2Int.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) Move(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) Move(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Move(Vector2.right);
    }

    private void Move(Vector2 direction)
    {

    }
}
