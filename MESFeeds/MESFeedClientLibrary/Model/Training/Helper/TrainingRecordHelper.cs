using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Model.Training.DataAccess;

namespace MESFeedClientLibrary.Model.Training.Helper
{
    public enum TrainingRecordType
    {
        TdbCur,
        TdbAct,
        TdbExp
    }
    public class TrainingRecordHelper
    {
        public Func<TrainingRecordCurrency> CreateTRCur { get; set; }
        public Func<TrainingRecordAction> CreateTRAct { get; set; }
        public Func<TrainingRecordExpiration> CreateTREx { get; set; }
        //public Func<TrainingRecordType, IQuery> CreateTR { get; set; }
        public Func<TrainingRecordType, ITrainingRecord> CreateTR { get; set; }

        public TrainingRecordHelper()
        {
            this.CreateTRCur = () => new TrainingRecordCurrency();
            this.CreateTRAct = () => new TrainingRecordAction();
            this.CreateTREx = () => new TrainingRecordExpiration();
            this.CreateTR = x =>
            {

                switch (x)
                {
                    case TrainingRecordType.TdbCur:
                        return new TrainingRecordCurrency();
                    case TrainingRecordType.TdbAct:
                        return new TrainingRecordAction();
                    case TrainingRecordType.TdbExp:
                        return new TrainingRecordExpiration();
                    default:
                        break;
                }
                return null;
            };
        }

    }
}
