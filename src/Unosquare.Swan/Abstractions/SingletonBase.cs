﻿namespace Unosquare.Swan.Abstractions
{
    using System;

    /// <summary>
    /// Represents a singleton pattern abstract class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonBase<T> : IDisposable
        where T : class
    {
        private bool _isDisposing; // To detect redundant calls

        /// <summary>
        /// The static, singleton instance reference.
        /// </summary>
        protected static readonly Lazy<T> LazyInstance = new Lazy<T>(
            valueFactory: () => Activator.CreateInstance(typeof(T), true) as T, 
            isThreadSafe: true);

        /// <summary>
        /// Disposes the internal singleton instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// Call the GC.SuppressFinalize if you override this method and use
        /// a non-default class finalizer (destructor)
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (_isDisposing) return;

            // free managed resources
            if (LazyInstance != null)
            {
                try
                {
                    var disposableInstance = LazyInstance.Value as IDisposable;
                    disposableInstance?.Dispose();
                }
                catch
                {
                    // swallow
                }
            }

            _isDisposing = true;
        }

        /// <summary>
        /// Gets the instance that this singleton represents.
        /// If the instance is null, it is constructed and assigned when this member is accessed.
        /// </summary>
        public static T Instance => LazyInstance.Value;
    }
}