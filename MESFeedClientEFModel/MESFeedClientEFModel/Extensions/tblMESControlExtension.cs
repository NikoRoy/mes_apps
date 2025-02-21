using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientEFModel.Extensions
{
    public partial class tblMESControl
    {
        public static async Task UpdateLastRunDate(MESFeedClientEntities entity, string type, DateTime date)
        {
            using (entity)
            {
                var ent = entity.tblMESControls.Where(l => l.Interface == type).SingleOrDefault();

                ent.LastRunDate = date;
                await entity.SaveChangesAsync();
            }
        }
    }
}
