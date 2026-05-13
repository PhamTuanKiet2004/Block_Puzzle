using UnityEngine;

public class Block : MonoBehaviour
{
    public const int Size = 5;
    [SerializeField] private Cell cellPrefab;
    private readonly Cell[,] cells = new Cell[Size, Size];
    //Hàm hiển thị các block
    public void Initialize()
    {
        for (var r=0;r<Size; ++r)
        {
            for (var c = 0; c < Size; ++c)
            {
                cells[r, c] = Instantiate(cellPrefab, transform); 
            }
        }
    }
    public void Show(int polyominoIndex)
    {
        Hide();
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoColumns = polyomino.GetLength(1);
        var center = new Vector2 (polyominoColumns * 0.5f, polyominoRows * 0.5f);
        for(var r = 0;r<polyominoRows; ++r)
        {
            for(var c = 0;c<polyominoColumns; ++c)
            {
                if (polyomino[r, c ] > 0)
                {
                    cells[r, c].transform.localPosition = new(c-center.x+0.5f, r-center.y+0.5f, 0.0f);
                    cells[r, c].Normal();
                }
            }
        }
    }
    private void Hide()
    {
        for (var r = 0; r < Size; ++r)
        {
            for (var c = 0; c < Size; ++c)
            {
                cells[r, c].Hide();
            }
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }
    private void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag");
    }
    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
    }
}
