using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDBModelLibrary
{
    public partial class ElectronicSignatureExtension : ElectronicSignature
    {
        public static bool ExecuteElectronicSignature(Guid esig, EmployeeTrainingEntities context)
        {
            try
            {
                System.Data.Entity.Core.Objects.ObjectParameter output = new System.Data.Entity.Core.Objects.ObjectParameter("Result", 0);
                context.pExecuteElectronicSignature(esig, "Test Cloning", output);
                if (Convert.ToInt32(output.Value) != 1)
                {
                    throw new Exception("Unable to execute your electronic signature. Please try again.");
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static ElectronicSignature CreateElectronicSignature(EmployeeTrainingEntities context)
        {
            try
            {
                var esign = new ElectronicSignature()
                {
                    ElectronicSignatureId = Guid.NewGuid(),
                    UserName = "Training Action Cloner",
                    EmployeeNumber = "999999",
                    FirstName = "Clone",
                    LastName = "Client",
                    PasswordVerifiedDateTime = DateTime.Now

                };
                context.ElectronicSignatures.Add(esign);
                //context.SaveChanges();
                return esign;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
