using UnityEngine;

//đây là lớp static khai báo các mảng 2 chiều định nghĩa các hình dạng của khối
public static class Polyominos
{

    private static readonly int[][,] polyominos = new int[][,]
    {
        new int[,] { { 1 } }, // Dấu chấm

        // ================= 2 Ô =================
        new int[,] { { 1, 1 } },       // Ngang 2
        new int[,] { { 1 }, { 1 } },   // Dọc 2

        // ================= 3 Ô =================
        new int[,] { { 1, 1, 1 } },             // Ngang 3
        new int[,] { { 1 }, { 1 }, { 1 } },     // Dọc 3
        
        // L nhỏ (4 hướng)
        new int[,] { { 1, 1 }, { 1, 0 } },
        new int[,] { { 1, 1 }, { 0, 1 } },
        new int[,] { { 1, 0 }, { 1, 1 } },
        new int[,] { { 0, 1 }, { 1, 1 } },

        // ================= 4 Ô =================
        new int[,] { { 1, 1 }, { 1, 1 } },      // Vuông 2x2
        new int[,] { { 1, 1, 1, 1 } },          // Ngang 4
        new int[,] { { 1 }, { 1 }, { 1 }, { 1 } }, // Dọc 4
        
        // Chữ T nhỏ (4 hướng)
        new int[,] { { 1, 1, 1 }, { 0, 1, 0 } },
        new int[,] { { 0, 1, 0 }, { 1, 1, 1 } },
        new int[,] { { 1, 0 }, { 1, 1 }, { 1, 0 } },
        new int[,] { { 0, 1 }, { 1, 1 }, { 0, 1 } },

        // Hình Z và S (4 ô)
        new int[,] { { 1, 1, 0 }, { 0, 1, 1 } },
        new int[,] { { 0, 1, 1 }, { 1, 1, 0 } },

        // ================= 5 Ô =================
        new int[,] { { 1, 1, 1, 1, 1 } },       // Ngang 5
        new int[,] { { 1 }, { 1 }, { 1 }, { 1 }, { 1 } }, // Dọc 5
        
        // L lớn 3x3 (4 hướng)
        new int[,] { { 1, 1, 1 }, { 1, 0, 0 }, { 1, 0, 0 } },
        new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 } },
        new int[,] { { 1, 0, 0 }, { 1, 0, 0 }, { 1, 1, 1 } },
        new int[,] { { 0, 0, 1 }, { 0, 0, 1 }, { 1, 1, 1 } },

        // Dấu cộng (+)
        new int[,] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } },

        // ================= BONUS ĐẶC BIỆT =================
        // Vuông 3x3 (Khối bự 9 ô - "Trùm cuối" cực kỳ phổ biến trong game 1010!)
        new int[,]
        {
            { 1, 1, 1 },
            { 1, 1, 1 },
            { 1, 1, 1 }
        },
        new int[,]
        {
            { 1, 1, 0 },
            { 0, 0, 0 },
            { 1, 1, 1 }
        },

        // 2. Hình chữ X (Khối chéo góc, khoảng trống ở các cạnh)
        new int[,]
        {
            { 1, 0, 1 },
            { 0, 1, 0 },
            { 1, 0, 1 }
        },

        // 4. Hình chữ U (Khá khó để lách vào khe)
        new int[,]
        {
            { 1, 0, 1 },
            { 1, 0, 1 },
            { 1, 1, 1 }
        },

        // 5. Hình 2 dấu chấm cách xa nhau (Khoảng cách 3 ô)
        // Rất dễ gây ảo giác cho người chơi khi cố gắng thả xuống
        new int[,]
        {
            { 1, 0, 0, 0, 1 }
        },

        // 6. Bậc thang vô hình (Chỉ có các ô ở đường chéo)
        new int[,]
        {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 }
        },
        new int[,]
        {
            { 1, 1 },
            { 1, 1 },
            { 1, 1 }
        },
        new int[,]
        {
            { 1, 1, 1 },
            { 1, 1, 1 }
        }
    };
    static Polyominos()
    {
        foreach (var polyomino in polyominos)
        {
            ReverseRows(polyomino);
        }
    }
    public static int[,] Get(int index) => polyominos[index];
    public static int Length => polyominos.Length;
    private static void ReverseRows(int[,] polyomino)
    {
        var polyominoRows = polyomino.GetLength(0);
        var polyominoColumns = polyomino.GetLength(1);
        for (var r = 0;r <polyominoRows/2; ++r)
        {
            var topRow = r;
            var bottomRow = polyominoRows - 1 - r;
            for(var c = 0; c < polyominoColumns; ++c)
            {
                var tmp = polyomino[topRow, c];
                polyomino[topRow, c] = polyomino[bottomRow, c];
                polyomino[bottomRow, c] = tmp;
            }
        }
    }
}
