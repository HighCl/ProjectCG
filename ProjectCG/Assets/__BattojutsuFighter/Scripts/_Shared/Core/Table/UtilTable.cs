using System;
using System.Collections.Generic;
using System.Linq;
using MinbGamesLib;

public static class UtilTable
{
    public static void SetDataTable(Dictionary<int, List<TRFoundation>> dataTable)
    {
        dataTable.Add((int)TableType.Example, GetTableList<TRExample>());
    }

    public static int GetIndex<T>() where T : class
    {
        if (typeof(T) == typeof(TRExample)) return (int)TableType.Example;

        throw new InvalidOperationException($"Unhandled type: {typeof(T).Name}");
    }

    public static List<TRFoundation> GetTableList<T>() where T : class
    {
        if (typeof(T) == typeof(TRExample))
            return BGExample.MetaDefault.Select(entity => new TRExample((BGExample)entity) as TRFoundation).ToList();

        throw new InvalidOperationException($"Unsupported type {typeof(T).Name}");
    }
}