using System.Collections.Generic;
using UnityEngine;

namespace Minofall
{
    public class PieceGenerator
    {
        public const int PREVIEW_SIZE = 3;

        private Queue<int> _nextQueue = new();
        private List<int> _pieceBag = new();

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

        public void FillQueue()
        {
            while (_nextQueue.Count < PREVIEW_SIZE && _pieceBag.Count > 0)
            {
                int p = _pieceBag[0];
                _pieceBag.RemoveAt(0);
                _nextQueue.Enqueue(p);
            }
        }

        public int[] PeekQueue()
        {
            return _nextQueue.ToArray();
        }

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