using OracleMESFeeds.DataAccess;
using OracleMESFeeds.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OracleMESFeeds.Helper
{
    public enum OracleRecordType
    {
        WorkOrder,
        Item, 
        Inventory
    }
    public class OracleRecordHelper
    {
        public Func<IEnumerable<WorkOrder>> WorkOrder = () => WorkOrderDownloadQuery.GetWorkOrders();
        public Func<IEnumerable<Item>> Item = () => ItemDownloadQuery.GetItemTransactions();
        public Func<IEnumerable<InventoryDownload>> Inventory = () => InventoryDownloadQuery.GetOpenInventoryTransactions();

        
    }    
}
