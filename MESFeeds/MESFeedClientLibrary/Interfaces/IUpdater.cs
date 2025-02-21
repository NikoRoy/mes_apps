using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Interfaces
{
    public interface IUpdater<T>
    {
        Task UpdateAsync(IEnumerable<T> obj, DateTime date = default(DateTime));      
    }
}
