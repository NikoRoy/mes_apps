using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Activity
{
    public class IntakeActivityAdapter<T> : IActivity 
    {
        private IActivityLogger<T> _actlog;

        public IntakeActivityAdapter(IActivityLogger<T> al)
        {
            this._actlog = al;
        }
        public async Task LogActivity(IMessage obj, string action, string response)
        {
            await _actlog.LogActivity((T)obj, action, response);
            //throw new NotImplementedException();
        }

    }
}
