using System;
using System.Reflection;

namespace Uruchie.Core.Service
{
    public class VersionCheckEventArgs : OperationCompletedEventArgs<Version>
    {
        public VersionCheckEventArgs(Exception error, bool cancelled, object userState) 
            : base(error, cancelled, userState)
        {
            IsCurrentVersionActual = Assembly.GetAssembly(typeof (UruchieForumService)).GetName().Version >= Result;
        }

        public bool IsCurrentVersionActual { get; private set; }
    }
}
