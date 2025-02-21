using System;
using System.Threading.Tasks;
using MESFeedClientEFModel.Extensions;
using MESFeedClientLibrary.Activity;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.BusinessLayer
{
    public enum InterfaceTypes
    {
        BlueMountain,
        TrainingRecord,
        TrainingDocument,
        TrainingGroup
    }
    public static class BusinessExtensions
    {
        public static async Task LogActivity(this MessageAdapter message)
        {
            var header = new ServiceBusActivity();
            var body = new ServiceBusActivityDetail();

            var headertask = header.LogActivity(message, "", "");
            var bodytask = body.LogActivity(message, message._received.MessageId, "");

            await headertask;
            await bodytask;
            
        }

        #region updates
        public static async Task UpdateControlTime(DateTime date, InterfaceTypes type)
        {
            await tblMESControl.UpdateLastRunDate(EntityFactory.GenerateContext(), type.ToString(), date);
        }

        public static void UpdateQueueTable()
        {
            using (var entity = EntityFactory.GenerateContext())
            {
                entity.Database.CommandTimeout = 120;
                entity.spUpdateMESQueue();
            }
        }
        public static void UpdateQueueTableNewUser()
        {
            using (var entity = EntityFactory.GenerateContext())
            {
                entity.Database.CommandTimeout = 120;
                entity.spQueueNewUsers();
            }
        }
        public static void SyncMESQueue(string emp, string doc, bool? sync)
        {
            using (var entity = EntityFactory.GenerateContext())
            {
                entity.Database.CommandTimeout = 120;
                entity.spSyncMESQueue(emp, doc, sync);
            }
        }
        public static void BackFeedTrainingActivity()
        {
            using (var entity = EntityFactory.GenerateContext())
            {
                entity.Database.CommandTimeout = 120;
                var i = entity.spTrainingActionBackFeed();
                if (i < 0)
                    throw new StoredProcedureException("spTrainingActionBackFeed Exception");
            }
        }
        public static void GetBlueMountainRecords()
        {

        }
        
        #endregion
        #region gets
        //public static IEnumerable<IMessage> GetTrainingRecords(IList<SqlParameter> pl)
        //{
        //    using (var entity = EntityFactory.GenerateContext())
        //    {
        //        entity.Database.CommandTimeout = 120;
        //        return entity.spCheckLastActions(3).GetEnumerator();
        //    }
        //}
        //public static IEnumerable<IMessage> GetTrainingBundles(IList<SqlParameter> pl)
        //{
        //    using (var entity = EntityFactory.GenerateContext())
        //    {
        //        entity.Database.CommandTimeout = 120;
        //        return entity.spBundleDailyChangesGet().GetEnumerator();
        //    }
        //}
        //public static IEnumerable<IMessage> GetTrainingDocuments(IList<SqlParameter> pl)
        //{
        //    using (var entity = EntityFactory.GenerateContext())
        //    {
        //        entity.Database.CommandTimeout = 120;
        //        return entity.spDocDailyChangesGet().GetEnumerator();
        //    }
        //}
        #endregion
    }
}
