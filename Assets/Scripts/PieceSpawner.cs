using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private Piece[] availablePieces = null;

    [SerializeField] private int spawnCooldownTicks = 3;

    private int currentSpawnCooldown = 0;

    private void Start()
    {
        GridManager.Instance.OnGridTick += Instance_OnGridTick;
    }

    private void Instance_OnGridTick()
    {
        if (currentSpawnCooldown == 0)
        {
            Piece selectedPiecePrefab = availablePieces[Random.Range(0, availablePieces.Length)];
            int spawnHeight = GridManager.height - selectedPiecePrefab.size.y;
            GridManager.Instance.SpawnObject(selectedPiecePrefab, new Vector2Int(0, spawnHeight));
            currentSpawnCooldown = spawnCooldownTicks;
        }
        else
        {
            currentSpawnCooldown -= 1;
        }
    }
}
