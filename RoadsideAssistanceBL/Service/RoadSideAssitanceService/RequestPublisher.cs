using RoadsideAssistanceBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace RoadsideAssistanceBL.Service
{
    public class RequestPublisher : IRequestPublisher
    {
        private List<Assistant> assitants;
        private ServiceRequest serviceRequest;

        public RequestPublisher(List<Assistant> list, ServiceRequest request)
        {
            assitants = list;
            serviceRequest = request;
        }

        public void NotifyAssistants(Action<Assistant, string> callback)
        {
            foreach (var assitant in assitants)
            {
                assitant.Notify(serviceRequest, callback);
            }
        }

        public async Task<Assistant> GetConfirmedAssistant()
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;
            var task = Task<Assistant>.Factory.StartNew(() =>
            {
                Assistant reponse = null;
                Action<Assistant, string> Callback = delegate (Assistant assistant, string decision)
                {
                    if (decision == "accepted" && reponse == null)
                    {
                        Log.Information($"Assitant {assistant.Name} is assigned for work");
                        lock (assistant)
                        {
                            assistant.ConfirmAssignment();
                        }
                        reponse = assistant;

                    }
                };
                NotifyAssistants(Callback);
                while (reponse == null)
                {
                }
                return reponse;
            }, cancellationToken);

            int timeout = 10 * 1000; //wait for 10 seconds to timeout
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                return await task;
            }
            {
                tokenSource.Cancel();
                return null;
            }
        }
    }
}
