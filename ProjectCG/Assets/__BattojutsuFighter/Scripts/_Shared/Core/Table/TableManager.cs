using MinbGamesLib;

public class TableManager : BaseTableManager<TableManager>
{
    protected override void SetDataTable() => UtilTable.SetDataTable(dataTable);
    protected override int GetIndex<T>() => UtilTable.GetIndex<T>();

    protected override void OnInit()
    {
    }
}
