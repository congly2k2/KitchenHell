using System;

namespace Interfaces
{
    public interface IHasProgress
    {
        public event EventHandler<OnProgressChangeEventArgs> OnProgressChange;
        public class OnProgressChangeEventArgs : EventArgs
        {
            public float ProgressNormalized;
        }
    }
}
