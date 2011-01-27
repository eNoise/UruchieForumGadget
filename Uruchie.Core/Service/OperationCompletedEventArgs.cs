using System;
using System.ComponentModel;

namespace Uruchie.Core.Service
{
    public class OperationCompletedEventArgs<T> : AsyncCompletedEventArgs
    {
        public OperationCompletedEventArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
        }

        public T Result { get; set; }
    }
}