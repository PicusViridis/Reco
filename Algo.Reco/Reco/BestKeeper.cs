using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Reco
{
    public class BestKeeper<T> : IReadOnlyCollection<T>
    {
        readonly Comparer<T> _comparer;
        readonly T[] _items;
        int _count;

        public BestKeeper( int capacity, Comparison<T> comparator )
        {
            _items = new T[capacity];
            _comparer = Comparer<T>.Create( comparator );
        }

        public int Capacity => _items.Length;

        public int Count => _count;

        public bool AddCandidate( T newOne )
        {
            int idx = Array.BinarySearch( _items, 0, _count, newOne, _comparer );
            if( idx < 0 ) idx = ~idx;
            if( idx == _items.Length ) return false;
            if( _count < _items.Length ) ++_count;
            int remainder = _count - idx - 1;
            if( remainder > 0 ) Array.Copy( _items, idx, _items, idx + 1, remainder );
            _items[idx] = newOne;
            return true;
        }

        public IEnumerator<T> GetEnumerator() => _items.Take( _count ).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
