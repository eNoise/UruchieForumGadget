using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Uruchie //for visibility
{
    public static class EnumerbableExtensions
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }
    }
}
