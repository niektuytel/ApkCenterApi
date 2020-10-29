
using ApkCenterAdminApi.Src;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Models
{

    public class Requests
    {
        private const string _FileName = Program.DataDirectory + "RequestedApps.json";

        public List<RequestModel> AllRequests { get; private set; }

        private readonly FileHelper _fileHelper;

        public Requests()
        {
            _fileHelper = new FileHelper();
            AllRequests = _fileHelper.UpdateStorage(_FileName, new List<RequestModel>());
        }


        public (int, string) AddRequest(string _, string appTitle)
        {
            (int code, string message) = (404, "error");

            for( int i = 0; i < AllRequests.Count; i++)
            {
                if(AllRequests[i].Title == appTitle)
                {
                    AddRequestedTimes(i, AllRequests[i]);
                    (code, message) = (200, "Successfully add request");
                    break;
                }
            }

            // add new one
            if(code == 404)
            {
                RequestModel newModel = new RequestModel
                {
                    Title = appTitle,
                    RequestedTimes = 1
                };
                AllRequests.Add(newModel);
                (code, message) = (200, "Succeeded added new request");
            }

            Task.Run( () => AllRequests = _fileHelper.UpdateStorage(_FileName, AllRequests));

            return (code, message);
        }

        public (int, string) DeleteRequest(string title)
        {
            for (int index = 0; index < AllRequests.Count; index++)
            {
                if(AllRequests[index].Title == title)
                {
                    AllRequests.RemoveAt(index);
                    _fileHelper.StoreJson(_FileName, AllRequests);
                    
                    return (200, "Successfully deleted");
                }
            }

            return (404, "Title can't been found");
        }

        private void AddRequestedTimes(int index, RequestModel model)
        {
            model.RequestedTimes++;
            AllRequests.RemoveAt(index);

            // sort set 
            bool called = false;
            if(AllRequests.Count != 0)
            {
                int length = Math.Min(index, (AllRequests.Count - 1));
                for (int i = length; i > -1; i--)
                {
                    if(AllRequests[i].RequestedTimes > model.RequestedTimes)
                    {
                        called = true;
                        AllRequests.Insert(i+1, model);
                        break;
                    }
                }
            }

            if(!called)
            {
                AllRequests.Insert(0, model);
            }

        }
    }
}
