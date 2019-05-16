using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : GridObjectBase
{
    public int health = 3;
    public int stunLeft = 0;
    public Transform visualsContainer;

    public List<Piece> linkedPieces = new List<Piece>();

    private Color colorNormal;
    private Color colorLinked;

    public void LinkPiece(Piece newPiece)
    {
        linkedPieces.Add(newPiece);
        SetColor(colorLinked);
    }

    public void ClearLinkedPieces()
    {
        linkedPieces.Clear();
        SetColor(colorNormal);
    }

    public void Init(Color colorNormal, Color colorLinked)
    {
        this.colorNormal = colorNormal;
        this.colorLinked = colorLinked;
    }

    public void SetColor(Color color)
    {
        foreach (var rendw in GetComponentsInChildren<SpriteRenderer>())
        {
            rendw.color = color;
        }
    }

    public void SmashedByOtherPiece()
    {
        if (stunLeft > 1)
        {
            stunLeft = 1;
        }
        visualsContainer.DOKill();
        visualsContainer.DOShakePosition(.1f, Random.insideUnitSphere);
    }

    public void Damage(int damage, int shotPushHeigh, int stun)
    {
        Shake();
        stunLeft = stun;

        if (linkedPieces.Count > 0)
        {
            linkedPieces.Add(this);
            GameController.Instance.AddScore(10 * linkedPieces.Count * linkedPieces.Count);
            GridManager.Instance.DestroyPieces(linkedPieces);
            return;
        }

        health -= damage;

        int pushbackHeight = Mathf.Max(Position.y, shotPushHeigh);
        GridManager.Instance.MoveObjectYMax(this, new Vector2Int(Position.x, pushbackHeight));


        if (health <= 0)
        {
            GameController.Instance.AddScore(10);
            GridManager.Instance.DestroyPiece(this);
        }
    }

    private void Shake()
    {
        visualsContainer.DOKill();
        visualsContainer.DOShakePosition(.1f, Random.insideUnitSphere * 2f);
    }
        
}
