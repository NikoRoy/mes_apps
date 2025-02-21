using MESFeedClientEFModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.BusinessLayer
{
    public class EntityFactory
    {
        public static MESFeedClientEntities GenerateContext()
        {
            return new MESFeedClientEntities();
        }
    }
}
