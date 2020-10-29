using ApkCenterAdminApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Src
{
    public class Errors
    {
        private const string _FileName = Program.DataDirectory + "Errors.json";

        public Dictionary<string, List<ErrorModel>> AllErrors { get; private set; }

        private readonly FileHelper _fileHelper;

        public Errors()
        {
            _fileHelper = new FileHelper();
            AllErrors = _fileHelper.UpdateStorage(_FileName, new Dictionary< string, List<ErrorModel> >());
        }


        public (int, string) DeleteError(ErrorModel errorModel)
        {
            string type = errorModel.GetErrorType();
            if (!AllErrors.ContainsKey(type) || AllErrors[type].Count == 0)
            {
                return (404, "Missing keys for Deleting Error");
            }

            List<ErrorModel> errors = AllErrors[type];
            for (int index = 0; index < errors.Count; index++)
            {
                if(errors[index].Error == errorModel.Error)
                {
                    AllErrors[type].RemoveAt(index);
                    _fileHelper.StoreJson(_FileName, AllErrors);

                    return (200, "Successfull delete Error");
                }
            }

            return (404, "Can't found any error like That");
        }

        public (int, string) AddError(string _, ErrorModel model)
        {
            string type = model.GetErrorType();
            if (type is null) return (404, "Error type is null");
            if (!AllErrors.ContainsKey(type))
            {
                return (404, "ErrorType do not exists");
            }

            var (code, message) = (404, "Error Catched");
            for (int i = 0; i < AllErrors[type].Count; i++)
            {
                ErrorModel errorModel = AllErrors[type][i];
                if (errorModel.Error == model.Error)
                {
                    AddRequestedTimes(i, errorModel, type);
                    (code, message) = (200, "add error successfully");
                    break;
                }
            }

            //add new one 
            if (code != 200)
            {
                ErrorModel newModel = new ErrorModel
                {
                    Error = model.Error,
                };
                AllErrors[type].Add(newModel);
                (code, message) = (200, "Created new Error");
            }

            if(code == 200)
            {
                Task.Run(() => AllErrors = _fileHelper.UpdateStorage(_FileName, AllErrors));
            }

            return (code, message);
        }

        private void AddRequestedTimes(int index, ErrorModel errorModel, string type)
        {
            errorModel.RequestedTimes += 1;
            AllErrors[type].RemoveAt(index);

            // sort set 
            bool called = false;
            if (AllErrors[type].Count != 0)
            {
                int length = Math.Min(index, (AllErrors[type].Count - 1));
                for (int i = length; i > -1; i--)
                {
                    if (AllErrors[type][i].RequestedTimes > errorModel.RequestedTimes)
                    {
                        called = true;
                        AllErrors[type].Insert(i + 1, errorModel);
                        break;
                    }
                }
            }

            if (!called)
            {
                AllErrors[type].Insert(0, errorModel);
            }
        }

    }
}
