﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public static class DistinctExtensions
{
    public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
    {
        return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
    }

    public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector, IEqualityComparer<V> comparer)
    {
        return source.Distinct(new CommonEqualityComparer<T, V>(keySelector, comparer));
    }
}
