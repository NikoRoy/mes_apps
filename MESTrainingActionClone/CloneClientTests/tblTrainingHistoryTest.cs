using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBModelLibrary;

namespace CloneClientTests
{
    public class tblTrainingHistoryTest : tblTrainingHistory
    {
        public static void UpdateIdentity(int i)
        {
            MaxIdentity = i;
        }
        public static IEnumerable<tblTrainingHistory> GetItemsToClone(EmployeeTrainingEntities context)
        {
            return context.tblTrainingHistories.Where(i => i.TrainingHistoryID > MaxIdentity).ToList();
        }
    }
}
