using System;
using MinbGamesLib;
using UnityEngine;

[Serializable]
public class TRExample: TRFoundation
{
    [field: SerializeField] public override int TableId { get; protected set; }
    [field: SerializeField] public override string Name { get; protected set; }

    public TRExample(BGExample example): base()
    {
        TableId = example.table_id;
        Name = example.name;
    }
}