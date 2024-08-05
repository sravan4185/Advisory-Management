namespace AdvisorManagement.Cache
{
    public class MRUCache<K, V> where K : notnull
    {
        private readonly int _capacity;
        private readonly LinkedList<(K key, V value)> _cacheList;
        private readonly Dictionary<K, LinkedListNode<(K key, V value)>> _cacheMap;

        public MRUCache(int capacity = 5)
        {
            _capacity = capacity;
            _cacheList = new LinkedList<(K key, V value)>();
            _cacheMap = new Dictionary<K, LinkedListNode<(K key, V value)>>();
        }

        public V Get(K key)
        {
            if (_cacheMap.TryGetValue(key, out var node))
            {
                _cacheList.Remove(node);
                _cacheList.AddFirst(node);
                return node.Value.value;
            }
            throw new KeyNotFoundException();
        }

        public void Put(K key, V value)
        {
            if (_cacheMap.TryGetValue(key, out var node))
            {
                _cacheList.Remove(node);
            }
            else if (_cacheList.Count >= _capacity)
            {
                RemoveLast();
            }
            AddFirst(key, value);
        }

        public void Delete(K key)
        {
            if (_cacheMap.TryGetValue(key, out var node))
            {
                _cacheList.Remove(node);
                _cacheMap.Remove(key);
            }
        }

        public bool Contains(K key)
        {
            return _cacheMap.ContainsKey(key);
        }

        private void AddFirst(K key, V value)
        {
            var newNode = new LinkedListNode<(K key, V value)>((key, value));
            _cacheList.AddFirst(newNode);
            _cacheMap[key] = newNode;
        }

        private void RemoveLast()
        {
            var lastNode = _cacheList.Last;
            if (lastNode != null)
            {
                _cacheMap.Remove(lastNode.Value.key);
                _cacheList.RemoveLast();
            }
        }
    }
}
