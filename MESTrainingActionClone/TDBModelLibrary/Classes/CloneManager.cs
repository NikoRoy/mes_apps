using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDBModelLibrary
{
    public class CloneManager 
    {
        public string Connection;
        protected EmployeeTrainingEntities Context;

        public CloneManager(EmployeeTrainingEntities ct)
        {
            Context = ct;
        }
        public CloneManager(string destinationCn)
        {
            Connection = destinationCn;
            Context = new EmployeeTrainingEntities(destinationCn);
        }

        public void CloneActions()
        {
            Context = new EmployeeTrainingEntities(Connection);

            try
            {
                ProcessCORT(Context);
                ProcessCurrentRequirements(Context);
                ProcessHistory(Context);
                ProcessJobTracking(Context);

                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Context.Dispose();
            }

        }
        protected internal void ProcessHistory( EmployeeTrainingEntities destEntity)
        {
            try
            {
                //get latest prd(source) records 
                var records = tblTrainingHistory.GetItemsToClone();
                // clone prd record to tst entity
                foreach (var prdrecord in records)
                {
                    // clone without identity
                    var tstclone = prdrecord.Clone();
                    //var esign = ElectronicSignatureExtension.CreateElectronicSignature(destEntity);
                    //tstclone.ActiveTrainingElecronicSignatureId = esign.ElectronicSignatureId;
                    //tstclone.ElectronicSignatureId = esign.ElectronicSignatureId;

                    //ElectronicSignatureExtension.ExecuteElectronicSignature(esign.ElectronicSignatureId, destEntity);
                    destEntity.tblTrainingHistories.Add(tstclone);
                    //update identity for next time
                    tblTrainingHistory.UpdateIdentityState(records.Select(i => i.TrainingHistoryID).Max());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected internal void ProcessCORT( EmployeeTrainingEntities destEntity)
        {
            try
            {
                //get latest prd(source) records 
                var records = ChangeOrderRequiredTraining.GetItemsToClone();
                // clone prd record to tst entity
                foreach (var prdrecord in records)
                {
                    // clone without identity
                    var tstclone = prdrecord.Clone();
                    destEntity.ChangeOrderRequiredTrainings.Add(tstclone);
                    //update identity for next time
                    ChangeOrderRequiredTraining.UpdateIdentityState(records.Select(i => i.ChangeOrderRequiredTrainingId).Max());
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected internal void ProcessCurrentRequirements(  EmployeeTrainingEntities destEntity)
        {
            try
            {
                //get latest prd(source) records 
                var records = tblCurrentTrainingRequirement.GetItemsToClone();
                // clone prd record to tst entity
                foreach (var prdrecord in records)
                {
                    //already existing is tst
                    var tstr = destEntity.tblCurrentTrainingRequirements.Where(r => r.CurrentTrainingDocID == prdrecord.CurrentTrainingDocID)
                                                                  .Where(r => r.Employee_number == prdrecord.Employee_number)
                                                                  .SingleOrDefault();
                    
                    if (tstr != null)
                    {
                        //tstr.AssignedByElectronicSignatureId = prdrecord.AssignedByElectronicSignatureId;
                        tstr.CurrentTrainingType = prdrecord.CurrentTrainingType;
                        tstr.DeletedBy = prdrecord.DeletedBy;
                        //tstr.DeletedByElectronicSignatureId = prdrecord.DeletedByElectronicSignatureId;
                        tstr.Employee_Number_UpdatedBy = prdrecord.Employee_Number_UpdatedBy;
                        tstr.Employee_Number_UpdatedOn = prdrecord.Employee_Number_UpdatedOn;
                        tstr.EnteredBy = prdrecord.EnteredBy;
                        tstr.EnteredDate = prdrecord.EnteredDate;
                        tstr.Previous_Employee_Number = prdrecord.Previous_Employee_Number;
                    }
                    else
                    {
                        // clone without identity
                        var tstclone = prdrecord.Clone();

                        //var esign = ElectronicSignatureExtension.CreateElectronicSignature(destEntity);
                        //tstclone.AssignedByElectronicSignatureId = esign.ElectronicSignatureId;

                        //ElectronicSignatureExtension.ExecuteElectronicSignature(esign.ElectronicSignatureId, destEntity);
                        destEntity.tblCurrentTrainingRequirements.Add(tstclone);
                        //update identity for next time
                        tblCurrentTrainingRequirement.UpdateIdentityState(records.Select(i => i.CurrentTrainingID).Max());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected internal void ProcessJobTracking( EmployeeTrainingEntities destEntity)
        {
            try
            {
                //get latest prd(source) records 
                var records = xxatr_jobprocedure_tracking.GetItemsToClone();
                // clone prd record to tst entity
                foreach (var prdrecord in records)
                {
                    // clone without identity
                    var tstclone = prdrecord.Clone();
                    destEntity.xxatr_jobprocedure_tracking.Add(tstclone);
                    //update identity for next time
                    xxatr_jobprocedure_tracking.UpdateIdentityState(records.Select(i => i.trackID).Max());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
