using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile
{
    public static Dictionary<int, Color> NumColors { get; } = new()
    {
        { 0, new Color(0, 0, 0, 0) }, // 0 is transparent
        { 1, new Color(0, 0, 250, 255) },
        { 2, new Color(0, 125, 0, 255) },
        { 3, new Color(240, 20, 20, 255) },
        { 4, new Color(0, 0, 125, 255) },
        { 5, new Color(120, 0, 0, 255) },
        { 6, new Color(0, 130, 130, 255) },
        { 7, new Color(132, 0, 132, 255) },
        { 8, new Color(123, 123, 123, 255) },
    };

    public static GameObject SquarePrefab { get; } = Resources.Load<GameObject>("Prefabs/Square");
    public static Sprite BlankSprite { get; } = Resources.Load<Sprite>("Textures/BlankSquare");
    public static Sprite FlagSprite { get; } = Resources.Load<Sprite>("Textures/FlagSquare");
    public static Sprite MineSprite { get; } = Resources.Load<Sprite>("Textures/MineSquare");
    public static Sprite MineClickedSprite { get; } = Resources.Load<Sprite>("Textures/MineClickedSquare");
    public static Sprite MineCrossedSprite { get; } = Resources.Load<Sprite>("Textures/MineCrossedSquare");
    public static Sprite UnknownSprite { get; } = Resources.Load<Sprite>("Textures/UnknownSquare");
    public const int SpriteSize = 32;

    public Vector2Int Position { get; }
    public bool IsMine { get; private set; }
    public bool IsFlagged { get; private set; }
    public bool IsRevealed { get; private set; }
    private GameObject gameObject;

    /// <summary>
    /// Create a new tile at the given coordinates, with a gameObject.
    /// The created tile has no flag and no mine, and is not revealed.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Tile(int x, int y)
    {
        Position = new Vector2Int(x, y);
        IsMine = false;
        IsFlagged = false;
        IsRevealed = false;
        gameObject = Object.Instantiate(SquarePrefab, new Vector2(x, y), Quaternion.identity);
        gameObject.name = "Tile at (" + x + ", " + y + ")";
    }

    /// <summary>
    /// Place a bomb in the tile.
    /// </summary>
    public void SetBomb() => IsMine = true;

    /// <summary>
    /// Toggle flag placement on the tile. Change the tile's skin.
    /// </summary>
    /// <returns>True if a flag has been placed, false if a flag has been removed</returns>
    public bool ToggleFlag()
    {
        if (IsRevealed) throw new System.Exception("Can't call ToggleFlagged on a revealed tile");
        IsFlagged = !IsFlagged;
        if (IsFlagged) SetSprite(FlagSprite);
        else SetSprite(UnknownSprite);
        return IsFlagged;
    }

    /// <summary>
    /// Reveal the tile. Change the tile's skin.
    /// </summary>
    /// <returns>True if a mine has been revealed, false otherwise</returns>
    public bool Reveal()
    {
        if (IsFlagged) throw new System.Exception("Can't call Reveal on a flagged tile");
        IsRevealed = true;
        if (IsMine) SetSprite(MineClickedSprite);
        else SetSprite(BlankSprite);
        return IsMine;
    }

    /// <summary>
    /// Change the sprite of the tile.
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSprite(Sprite sprite) => gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

    public void SetNumber(int num)
    {
        gameObject.GetComponentInChildren<TextMeshPro>().text = num.ToString();
        gameObject.GetComponentInChildren<TextMeshPro>().color = Tile.NumColors[num];
    }

    public void Destroy()
    {
        Object.Destroy(gameObject);
    }
}
