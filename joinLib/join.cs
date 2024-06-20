using System.Data;

namespace JoinTables;
// q: What is this class?



public partial class EditableJoin : DataTable
{

        public EditableJoin(DataSet joinSet) => Init(joinSet);
        public void SelectAll() => _SelectAll();
        public void Select(List<DataColumn> columns) => _Select(columns);
        public void InnerJoin() => _InnerJoin();
        public void InnerJoin(DataColumn id1, DataColumn id2) => _InnerJoin(id1, id2);
        
}