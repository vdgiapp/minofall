using System.Collections.Generic;
using UnityEngine;

namespace Minofall
{
    /// <summary>
    /// Một bộ tạo mảnh sử dụng hệ thống túi để đảm bảo phân phối đồng đều các tetromino.
    /// </summary>
    public class PieceGenerator
    {
        /// <summary>
        /// Kich thước cần xem trước của hàng đợi.
        /// </summary>
        public const int PREVIEW_SIZE = 3;

        /// <summary>
        /// Hàng đợi các mảnh tiếp theo.
        /// </summary>
        private readonly Queue<int> _nextQueue = new();

        /// <summary>
        /// Danh sách các tetromino có sẵn trong túi.
        /// </summary>
        private readonly List<int> _pieceBag = new();

        /// <summary>
        /// Khởi tạo bộ tạo mảnh.
        /// </summary>
        public void Initialize()
        {
            FillBag();
            FillQueue();
        }

        /// <summary>
        /// Làm đầy túi với tất cả các tetromino và xáo trộn chúng.
        /// </summary>
        public void FillBag()
        {
            _pieceBag.Clear();
            for (int i = 0; i < Tetrominoes.Length; i++)
            {
                _pieceBag.Add(i);
            }
            // Shuffle
            for (int i = 0; i < _pieceBag.Count; i++)
            {
                int randomIndex = Random.Range(i, _pieceBag.Count);
                (_pieceBag[i], _pieceBag[randomIndex]) = (_pieceBag[randomIndex], _pieceBag[i]);
            }
        }

        /// <summary>
        /// Làm đầy hàng đợi từ túi.
        /// </summary>
        public void FillQueue()
        {
            while (_nextQueue.Count < PREVIEW_SIZE && _pieceBag.Count > 0)
            {
                int p = _pieceBag[0];
                _pieceBag.RemoveAt(0);
                _nextQueue.Enqueue(p);
            }
        }

        /// <summary>
        /// Trả về một mảng của hàng đợi mà không loại bỏ các mảnh.
        /// </summary>
        /// <returns></returns>
        public int[] PeekQueue()
        {
            return _nextQueue.ToArray();
        }

        /// <summary>
        /// Lấy mảnh tiếp theo từ hàng đợi và làm đầy lại hàng đợi từ túi nếu cần.
        /// </summary>
        /// <returns></returns>
        public int GetNextPiece()
        {
            int piece = _nextQueue.Dequeue();
            if (_pieceBag.Count == 0) FillBag();
            while (_nextQueue.Count < PREVIEW_SIZE)
            {
                int p = _pieceBag[0];
                _pieceBag.RemoveAt(0);
                _nextQueue.Enqueue(p);
            }
            return piece;
        }
    }
}