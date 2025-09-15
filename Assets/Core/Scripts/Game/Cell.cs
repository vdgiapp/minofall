using UnityEngine;

namespace Minofall
{
    /// <summary>
    /// Class tượng trưng cho một ô trong trò chơi.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Chi tiết hiển thị của ô.
        /// </summary>
        public CellDisplay cellDisplay;

        /// <summary>
        /// Màu sắc của ô.
        /// </summary>
        public Color color;

        /// <summary>
        /// Cờ kiểm tra xem ô có bị chiếm hay không.
        /// </summary>
        public bool occupied;

        /// <summary>
        /// Constructor mặc định khởi tạo một ô trống với màu trong suốt.
        /// </summary>
        public Cell()
        {
            Initialize(new(), Color.clear, false);
        }

        /// <summary>
        /// Constructor khởi tạo một ô với các thuộc tính cụ thể.
        /// </summary>
        /// <param name="cellDisplay">Chi tiết hiển thị</param>
        /// <param name="color">Màu sắc</param>
        /// <param name="occupied">Đã bị chiếm hay chưa</param>
        public Cell(CellDisplay cellDisplay, Color color, bool occupied)
        {
            Initialize(cellDisplay, color, occupied);
        }

        /// <summary>
        /// Đặt lại các thuộc tính của ô.
        /// </summary>
        /// <param name="cellDisplay">Chi tiết hiển thị</param>
        /// <param name="color">Màu sắc</param>
        /// <param name="occupied">Đã bị chiếm hay chưa</param>
        public void Initialize(CellDisplay cellDisplay, Color color, bool occupied)
        {
            this.cellDisplay = cellDisplay;
            this.color = color;
            this.occupied = occupied;
        }
    }
}