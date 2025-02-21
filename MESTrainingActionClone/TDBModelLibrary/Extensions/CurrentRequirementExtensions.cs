using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDBModelLibrary
{
    public partial class tblCurrentTrainingRequirement
    {


        public static int MaxIdentity { get; protected internal set; }

        static tblCurrentTrainingRequirement()
        {
            MaxIdentity = GetInitMaxIdentity();
        }

        private static int GetInitMaxIdentity()
        {
            using(var context = new EmployeeTrainingEntities())
            {
                return context.tblCurrentTrainingRequirements.Select(i => i.CurrentTrainingID).Max();
            }
        }

        public tblCurrentTrainingRequirement Clone()
        {
            return new tblCurrentTrainingRequirement()
            {
                //AssignedByElectronicSignatureId = this.AssignedByElectronicSignatureId,
                CurrentTrainingDocID = this.CurrentTrainingDocID,
                //CurrentTrainingID = this.CurrentTrainingID,
                CurrentTrainingType = this.CurrentTrainingType,
                DeletedBy = this.DeletedBy,
                //DeletedByElectronicSignatureId = this.DeletedByElectronicSignatureId,
                Employee_number = this.Employee_number,
                Employee_Number_UpdatedBy = this.Employee_Number_UpdatedBy,
                Employee_Number_UpdatedOn = this.Employee_Number_UpdatedOn,
                EnteredBy = this.EnteredBy,
                EnteredDate = this.EnteredDate,
                Previous_Employee_Number = this.Previous_Employee_Number
            };
        }

        public static IEnumerable<tblCurrentTrainingRequirement> GetItemsToClone()
        {
            using (var context = new EmployeeTrainingEntities())
            {
                return context.tblCurrentTrainingRequirements.Where(i => i.CurrentTrainingID > MaxIdentity).ToList();
            }
        }
        public static void UpdateIdentityState(int i)
        {
            MaxIdentity = Math.Max(MaxIdentity, i);
        }
    }
}
