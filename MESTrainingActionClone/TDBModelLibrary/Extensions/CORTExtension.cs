using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDBModelLibrary
{
    public partial class ChangeOrderRequiredTraining
    {
        public static int MaxIdentity { get; protected internal set; }

        static ChangeOrderRequiredTraining()
        {
            MaxIdentity = GetInitMaxIdentity();
        }
        public ChangeOrderRequiredTraining Clone()
        {
            var newCORT = new ChangeOrderRequiredTraining();
            newCORT.Active = this.Active;
            newCORT.ChangeOrderNumber = this.ChangeOrderNumber;
            //newCORT.ChangeOrderRequiredTrainingId = this.ChangeOrderRequiredTrainingId;
            newCORT.Committed = this.Committed;
            newCORT.CreatedAtDateTime = this.CreatedAtDateTime;
            newCORT.CreatedByUser = this.CreatedByUser;
            newCORT.DocumentId = this.DocumentId;
            newCORT.DueDateModifier = this.DueDateModifier;
            newCORT.EmployeeName = this.EmployeeName;
            newCORT.EmployeeNumber = this.EmployeeNumber;
            newCORT.InactivatedAtDateTime = this.InactivatedAtDateTime;
            newCORT.InactivatedByUser = this.InactivatedByUser;
            newCORT.InactivatedReasonCode = this.InactivatedReasonCode;
            newCORT.InactivatedReasonOther = this.InactivatedReasonOther;
            newCORT.ModifiedAtDateTime = this.ModifiedAtDateTime;
            newCORT.ModifiedByUser = this.ModifiedByUser;
            newCORT.ModifiedReasonCode = this.ModifiedReasonCode;
            newCORT.ModifiedReasonOther = this.ModifiedReasonOther;
            newCORT.RequiredTrainingLevel = this.RequiredTrainingLevel;
            newCORT.Revision = this.Revision;

            return newCORT;
        }

        public static IEnumerable<ChangeOrderRequiredTraining> GetItemsToClone()
        {
            using(var context = new EmployeeTrainingEntities())
            {
                return context.ChangeOrderRequiredTrainings.Where(i => i.ChangeOrderRequiredTrainingId > MaxIdentity).ToList();
            }
        }

        public static void UpdateIdentityState(int i)
        {
            MaxIdentity = Math.Max(MaxIdentity, i);
        }

        private static int GetInitMaxIdentity()
        {
            using (var context = new EmployeeTrainingEntities())
            {
                return context.ChangeOrderRequiredTrainings.Select(i => i.ChangeOrderRequiredTrainingId).Max();
            }
        }
    }
}
