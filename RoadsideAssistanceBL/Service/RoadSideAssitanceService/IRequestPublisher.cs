using RoadsideAssistanceBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideAssistanceBL.Service
{
    public interface IRequestPublisher
    {
        void NotifyAssistants(Action<Assistant, string> callback);
        Task<Assistant> GetConfirmedAssistant();
    }
}
