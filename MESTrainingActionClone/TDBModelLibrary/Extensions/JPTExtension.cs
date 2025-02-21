using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDBModelLibrary
{
    public partial class xxatr_jobprocedure_tracking
    {
        public static int MaxIdentity { get; protected internal set; }
        static xxatr_jobprocedure_tracking()
        {
            MaxIdentity = GetInitMaxIdentity();
        }

        private static int GetInitMaxIdentity()
        {
            using(var context = new EmployeeTrainingEntities())
            {
                return context.xxatr_jobprocedure_tracking.Select(i => i.trackID).Max();
            }
        }

        public xxatr_jobprocedure_tracking Clone()
        {
            return new xxatr_jobprocedure_tracking()
            {
                bundle = this.bundle,
                createDate = this.createDate,
                doc_ID = this.doc_ID,
                employee_number = this.employee_number,
                isTraining = this.isTraining,
                job_number = this.job_number,
                job_type = this.job_type,
                logoutDate = this.logoutDate,
                recordedBy = this.recordedBy,
                scanID = this.scanID,
                status = this.status,
                //trackID = this.trackID,
                trainerEmployeeId = this.trainerEmployeeId
            };
        }

        public static IEnumerable<xxatr_jobprocedure_tracking> GetItemsToClone()
        {
            using (var context = new EmployeeTrainingEntities())
            {
                return context.xxatr_jobprocedure_tracking.Where(i => i.trackID > MaxIdentity).ToList();
            }
        }
        public static void UpdateIdentityState(int i)
        {
            MaxIdentity = Math.Max(MaxIdentity, i);
        }
    }
}
