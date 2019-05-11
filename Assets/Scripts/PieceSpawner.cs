using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private Piece[] availablePieces = null;

    [SerializeField] private int spawnDelay = 2;

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Piece selectedPiecePrefab = availablePieces[Random.Range(0, availablePieces.Length)];
            int spawnHeight = GridManager.height - selectedPiecePrefab.size.y;
            GridManager.Instance.SpawnObject(selectedPiecePrefab, new Vector2Int(0, spawnHeight));
            yield return new WaitForSeconds(spawnDelay);

            
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
}
