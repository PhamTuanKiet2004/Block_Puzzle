using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;
using TMPro;

public class Board : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public const int size = 8;

    private const string BestScoreKey = "BestScore";

    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;

    [Space(8.0f)]
    [SerializeField] private TMP_Text Score_Text;
    [SerializeField] private TMP_Text bestScore_Text;


    private readonly Cell[,] cells = new Cell[size, size];
    private readonly int[,] data = new int[size, size]; //0 Empty, 1 Hover, 2 Normal
    private readonly List<Vector2Int> hoverPoints = new();

    private readonly List<int> highlightPolyominoColumns = new();
    private readonly List<int> highlightPolyominoRows = new();
    private readonly List<int> fullLineColumns = new();
    private readonly List<int> fullLineRows = new();

    private Vector2Int previousHoverPoint;
    private readonly List<Vector2Int> previousHoverPoints = new();

    private int score;
    private int bestScore;


    void Start()
    {
        for (var r = 0; r < size; ++r)
        {
            for (var c = 0; c < size; ++c)
            {
                cells[r, c] = Instantiate(cellPrefab, cellsTransform);
                cells[r, c].transform.position = new(c + 0.5f, r + 0.5f, 0.0f);
                cells[r, c].Hide();
            }
        }
        score = 0;
        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        Score_Text.text = score.ToString();
        bestScore_Text.text = bestScore.ToString();
    }
    public void Hover(Vector2Int point, int polyominoIndex)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0); 
        var polyominoColumns = polyomino.GetLength(1);
        Unhover();
        Unhighlight();

        highlightPolyominoColumns.Clear();
        highlightPolyominoRows.Clear();

        HoverPoints(point, polyominoRows, polyominoColumns, polyomino);

        if (hoverPoints.Count > 0)
        {
            previousHoverPoint = point; 
            previousHoverPoints.Clear();
            previousHoverPoints.AddRange(hoverPoints);

            Hover();
            Highlight(point, polyominoRows, polyominoColumns);

        }
        else if(previousHoverPoints.Count > 0 && Math.Abs(point.x -previousHoverPoint.x) < 2 && Math.Abs(point.y - previousHoverPoint.y) < 2)
        {
            point = previousHoverPoint;
            hoverPoints.Clear();
            hoverPoints.AddRange(previousHoverPoints);

            Hover();
            Highlight(point, polyominoRows, polyominoColumns);
        }
        else
        {
            previousHoverPoints.Clear();
        }
    }

    private void HoverPoints(Vector2Int point, int polyominoRows, int polyominoColumns, int[,] polyomino)
    {
        for(var r = 0; r<polyominoRows; ++r)
        {
            for (var c = 0; c<polyominoColumns;  ++c)
            {
                if (polyomino[r, c] > 0)
                {
                    var hoverPoint = point + new Vector2Int(c, r);
                    if(IsValidPoint(hoverPoint) == false)
                    {
                        hoverPoints.Clear();
                        return;
                    }
                    hoverPoints.Add(hoverPoint);
                }
            }
        }
    }
    private bool IsValidPoint(Vector2Int point)
    {
        if (point.x < 0 || size <= point.x) return false;
        if (point.y < 0 || size <= point.y) return false;
        if (data[point.y, point. x] > 0) return false;
        return true;
    }
    private void Hover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 1;
            cells[hoverPoint.y, hoverPoint.x].Hover();
        }
    }
    private void Unhover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 0;
            cells[hoverPoint.y, hoverPoint.x].Hide();
        }
        hoverPoints.Clear();
    }
    public bool Place(Vector2Int point, int polyominoIndex)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoColumns = polyomino.GetLength(1);
        Unhover();
        HoverPoints(point, polyominoRows, polyominoColumns, polyomino);
        if (hoverPoints.Count > 0)
        {
            Place(point, polyominoColumns, polyominoRows);

            previousHoverPoints.Clear();
            return true;
        }
        else if (previousHoverPoints.Count > 0 && Math.Abs(point.x - previousHoverPoint.x) < 2 && Math.Abs(point.y - previousHoverPoint.y) < 2)
        {
            point = previousHoverPoint;
            hoverPoints.Clear();
            hoverPoints.AddRange(previousHoverPoints);
            Place(point, polyominoColumns, polyominoRows);
            previousHoverPoints.Clear();
            return true;
        }
        previousHoverPoints.Clear();
        return false;
    }
    private void Place(Vector2Int point, int polyominoColumns, int polyominoRows)
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 2;
            cells[hoverPoint.y, hoverPoint.x].Normal();
        }

        ClearFullLines(point, polyominoColumns, polyominoRows);
        hoverPoints.Clear();
    }

    private void ClearFullLines(Vector2Int point, int polyominoColumns, int polyominoRows)
    {
        FullLineColumns(point.x, point.x + polyominoColumns);
        FullLineRows(point.y, point.y + polyominoRows);

        AddScore(fullLineColumns.Count * size + fullLineRows.Count* size);

        ClearFullLineColumns();
        ClearFullLineRows();
    }
    private void FullLineColumns(int fromColumn, int toColumnExclusive)
    {
        fullLineColumns.Clear();
        for( var c = fromColumn; c<toColumnExclusive; ++c)
        {
            if (c < 0 || c >= size) continue;
            var isFullLine = true;
            for (var r=0;r<size; ++r)
            {
                if (data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if(isFullLine == true)
            {
                fullLineColumns.Add(c);
            }
        }
    }
    private void FullLineRows(int fromRow, int toRowExclusive)
    {
        fullLineRows.Clear();
        for (var r = fromRow; r < toRowExclusive; ++r)
        {
            if (r < 0 || r >= size) continue;
            var isFullLine = true;
            for (var c = 0; c < size; ++c)
            {
                if (data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine == true)
            {
                fullLineRows.Add(r);
            }
        }
    }
    private void ClearFullLineColumns()
    {
        foreach (var c in fullLineColumns)
        {
            for(var r=0;r<size; ++r)
            {
                data[r, c] = 0;
                cells[r, c].Hide();
            }
        }
    }
    private void ClearFullLineRows()
    {
        foreach (var r in fullLineRows)
        {
            for (var c = 0; c < size; ++c)
            {
                data[r, c] = 0;
                cells[r, c].Hide();
            }
        }
    }

    private void Highlight(Vector2Int point, int polyominoRows, int polyominoColumns)
    {
        PredictFullLineColumns(point.x, point.x + polyominoColumns);
        PredictFullLineRows(point.y, point.y + polyominoRows);
        HighlightFullLineColumns();
        HighlightFullLineRows();

        foreach (var fullLineColumn in fullLineColumns)
        {
            highlightPolyominoColumns.Add(fullLineColumn - point.x);
        }
        foreach (var fullLineRow in fullLineRows)
        {
            highlightPolyominoRows.Add(fullLineRow - point.y);
        }
    }

    private void Unhighlight()
    {
        UnHighlightFullLineColumns();
        UnHighlightFullLineRows();
    }
    private void PredictFullLineColumns(int fromColumn, int toColumnExclusive)
    {
        fullLineColumns.Clear();
        for (var c = fromColumn; c < toColumnExclusive; ++c)
        {
            if (c < 0 || c >= size) continue;
            var isFullLine = true;
            for (var r = 0; r < size; ++r)
            {
                if (data[r, c] != 1 && data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine == true)
            {
                fullLineColumns.Add(c);
            }
        }
    }
    private void PredictFullLineRows(int fromRow, int toRowExclusive)
    {
        fullLineRows.Clear();
        for (var r = fromRow; r < toRowExclusive; ++r)
        {
            if (r < 0 || r >= size) continue;
            var isFullLine = true;
            for (var c = 0; c < size; ++c)
            {
                if (data[r, c] != 1 && data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine == true)
            {
                fullLineRows.Add(r);
            }
        }
    }

    private void HighlightFullLineColumns()
    {
        foreach (var c in fullLineColumns)
        {
            for (var r = 0; r < size; ++r)
            {
                if (data[r, c] == 2 || data[r, c] == 1)
                {
                    cells[r, c].Highlight();
                }
            }
        }
    }
    private void HighlightFullLineRows()
    {
        foreach (var r in fullLineRows)
        {
            for (var c = 0; c < size; ++c)
            {
                if (data[r, c] == 2 || data[r, c] == 1)
                {
                    cells[r, c].Highlight();
                }
            }
        }
    }
    private void UnHighlightFullLineColumns()
    {
        foreach (var c in fullLineColumns)
        {
            for (var r = 0; r < size; ++r)
            {
                if (data[r, c] == 2)
                {
                    cells[r, c].Normal();
                }
            }
        }
    }
    private void UnHighlightFullLineRows()
    {
        foreach (var r in fullLineRows)
        {
            for (var c = 0; c < size; ++c)
            {
                if (data[r, c] == 2)
                {
                    cells[r, c].Normal();
                }
            }
        }
    }

    public bool CheckPlace(int polyominoIndex)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoColumns = polyomino.GetLength(1);

        for (var r = 0; r < size - polyominoRows; ++r)
        {
            for (var c = 0; c < size - polyominoColumns; ++c)
            {
                if (CheckPlace(c, r, polyominoColumns, polyominoRows, polyomino) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool CheckPlace(int column, int row, int polyominoColumns, int polyominoRows, int[,] polyomino)
    {
        for (var r = 0; r < polyominoRows; ++r)
        {
            for (var c = 0; c < polyominoColumns; ++c)
            {
                if (polyomino[r, c] > 0 && data[row + r, column + c] == 2)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void AddScore (int amount)
    {
        score += amount;
        if(score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
        }

        Score_Text.text = score.ToString();
        bestScore_Text.text = bestScore.ToString(); 
    }

    public List<int> HighlightPolyominoColumns => highlightPolyominoColumns;
    public List<int> HighlightPolyominoRows => highlightPolyominoRows;
}
