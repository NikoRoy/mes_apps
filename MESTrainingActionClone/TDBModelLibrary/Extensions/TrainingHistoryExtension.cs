using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDBModelLibrary
{
    public partial class tblTrainingHistory
    {
        public static int MaxIdentity { get; protected internal set; }
        static tblTrainingHistory()
        {
            MaxIdentity = GetInitMaxIdentity();
        }

        private static int GetInitMaxIdentity()
        {
            using(var context = new EmployeeTrainingEntities())
            {
                return context.tblTrainingHistories.Select(i => i.TrainingHistoryID).Max();
            }
        }

        public tblTrainingHistory Clone()
        {
            return new tblTrainingHistory()
            {
                ActiveTrainingApproval = this.ActiveTrainingApproval,
                ActiveTrainingApprovedOn = this.ActiveTrainingApprovedOn,
                ActiveTrainingApprovedBy = this.ActiveTrainingApprovedBy,
                //ActiveTrainingElecronicSignatureId = this.ActiveTrainingElecronicSignatureId,
                //ElectronicSignatureId = this.ElectronicSignatureId,
                Employee_Number = this.Employee_Number,
                Employee_Number_UpdatedBy = this.Employee_Number_UpdatedBy,
                Employee_Number_UpdatedOn = this.Employee_Number_UpdatedOn,
                EnteredBy = this.EnteredBy,
                EnteredDate = this.EnteredDate,
                Previous_Employee_Number = this.Previous_Employee_Number,
                TrainDate = this.TrainDate,
                TrainDeptName = this.TrainDeptName,
                TrainDeptNum = this.TrainDeptNum,
                TrainDocID = this.TrainDocID,
                TrainDocRev = this.TrainDocRev,
                //TrainingHistoryID = this.TrainingHistoryID,
                TrainingLevel = this.TrainingLevel,
                TrainTitle = this.TrainTitle,
                TrainValidatedByFirstName = this.TrainValidatedByFirstName,
                TrainValidatedByLastName = this.TrainValidatedByLastName,
                TrainValidatedByNumber = this.TrainValidatedByNumber
            };

        }

        public static IEnumerable<tblTrainingHistory> GetItemsToClone()
        {
            using (var context = new EmployeeTrainingEntities())
            {
                return context.tblTrainingHistories.Where(i => i.TrainingHistoryID > MaxIdentity).ToList();
            }
        }
        public static void UpdateIdentityState(int i)
        {
            MaxIdentity = Math.Max(MaxIdentity, i);
        }
      
    }
}
