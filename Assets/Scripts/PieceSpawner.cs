using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private Color pieceColorsNormal;
    [SerializeField] private Color pieceColorsLinked;
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
            int randomX = Random.Range(0, GridManager.width - selectedPiecePrefab.size.x + 1);
            GridManager.Instance.SpawnObject(selectedPiecePrefab, new Vector2Int(randomX, spawnHeight), pieceColorsNormal, pieceColorsLinked);
            currentSpawnCooldown = spawnCooldownTicks;
        }
        else
        {
            currentSpawnCooldown -= 1;
        }
    }
}
