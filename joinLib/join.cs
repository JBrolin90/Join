using System.Data;

namespace JoinTables;
// q: What is this class?



public partial class EditableJoin : DataTable
{

        public EditableJoin(DataSet joinSet) => Init(joinSet);
        public void SelectAll() => _SelectAll();
        public void Select(List<DataColumn> columns) => _Select(columns);
        public void InnerJoin() => _InnerJoin();
        public void InnerJoin(DataColumn primaryKey, DataColumn foreignKey) => _InnerJoin(primaryKey, foreignKey);
        public void InnerJoin(List<DataColumn> primaryKeys, List<DataColumn> foreignKeys) => _InnerJoin(primaryKeys, foreignKeys);
}