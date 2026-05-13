using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField] private Block[] blocks;

    private void Start()
    {
        // Chiều rộng tổng cộng của Board là Board.size (8)
        float boardWidth = Board.size;

        // Chia Board thành các phần bằng nhau cho mỗi block
        var blockSpace = boardWidth / blocks.Length;

        // Tính toán cellSize dựa trên mong muốn hiển thị 
        var cellSize = boardWidth / (Block.Size * blocks.Length + blocks.Length + 1);

        for (var i = 0; i < blocks.Length; i++)
        {
            // X: Căn giữa mỗi block trong phần không gian của nó
            float xPos = blockSpace * (i + 0.5f);

            // Y: Đưa xuống dưới Board. Board kết thúc ở Y=0, 
            // nên ta đưa xuống khoảng -1.5 hoặc -2.0 
            float yPos = -1.5f;

            blocks[i].transform.localPosition = new Vector3(xPos, yPos, 0.0f);

            // Căn chỉnh kích thước tỉ lệ với cell của Board
            blocks[i].transform.localScale = new Vector3(cellSize, cellSize, 1f);

            blocks[i].Initialize();
        }
        Generate();
    }
    private void Generate()
    {
        for (var i = 0; i < blocks.Length; ++i)
        {
            blocks[i].Show(0);
        }
    }
}