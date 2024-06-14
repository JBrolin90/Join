using System.Data;

namespace JoinTables;


public partial class Join :DataTable
{
    
        public Join(DataSet joinSet) => Init(joinSet);
        public void Fill() => _Fill();
        public void SelectAll() => _SelectAll();
        public void Select(List<DataColumn> columns) => _Select(columns);
}