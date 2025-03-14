using System;

namespace LFramework
{
    [Serializable]
    public abstract class LCollectStep
    {
        [Serializable]
        public enum AddType
        {
            Append = 0,
            Join = 1,
            Insert = 2,
        }

        public abstract string DisplayName { get; }

        public abstract void Apply(LCollectItem item);
    }
}
