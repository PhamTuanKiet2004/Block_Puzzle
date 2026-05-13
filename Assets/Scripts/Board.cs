using UnityEngine;

public class Board : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public const int size = 8;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;
    private readonly Cell[,] cells = new Cell[size, size]; 
    void Start()
    {
        for (var r = 0; r < size; ++r)
        {
            for (var c = 0; c < size; ++c)
            {
                cells[r, c] = Instantiate(cellPrefab, cellsTransform);
                cells[r, c].transform.position = new(c + 0.5f, r + 0.5f, 0.0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
