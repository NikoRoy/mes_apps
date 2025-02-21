using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBModelLibrary;

namespace CloneClientTests
{
    public class CloneManagerTest : CloneManager
    {
        public CloneManagerTest(EmployeeTrainingEntities ct) : base(ct)
        {
        }
        public CloneManagerTest(string tstcn) : base(tstcn)
        {
        }

        public void TestHistoryClone()
        {
            //reset max identity to one less
            var mi = tblTrainingHistory.MaxIdentity;
            tblTrainingHistoryTest.UpdateIdentity(mi - 1);

            this.ProcessHistory( this.Context);
            this.Context.SaveChanges();
        }
        public IEnumerable<tblTrainingHistory> TestHistoryGet(int i)
        {
            tblTrainingHistoryTest.UpdateIdentity(i);
            return tblTrainingHistoryTest.GetItemsToClone(this.Context);
        }
    }
}
