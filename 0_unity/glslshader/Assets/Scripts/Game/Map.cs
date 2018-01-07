using UnityEngine;
using System.Collections;

namespace Game
{
    [ExecuteInEditMode]
    public class Map
    {
        private static Bounds _bounds;
        public Bounds Bounds
        {
            get
            {
                return _bounds;
            }
            set
            {
                _bounds = value;
            }
        }

        public void Awake()
        {
            _bounds = new Bounds();
        }
    }
}