using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APIDisk
{
    public class Class1
    {
        public DriveService Authorization()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive,
                                 DriveService.Scope.DriveFile};
            var clientId = "400980761835-lhopbbn43unvri70ski7a1dhe8g81u4h.apps.googleusercontent.com";
            var clientSecret = "K1ZSaosSRoPYYGNmqJuJXaZX";
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
                scopes, Environment.UserName, CancellationToken.None, new FileDataStore("Daimto.GoogleDrive.Auth.Store")).Result;

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Quickstart",
            });
            return service;

        }

        public Dictionary<string, string> GetFolders(DriveService _service, Dictionary<string, string> folders)
        {
            FilesResource.ListRequest listRequest = _service.Files.List();
            var files = listRequest.Execute();
            foreach (File file in files.Items)
            {
                if (file.MimeType == "application/vnd.google-apps.folder")
                {
                    folders.Add(file.Id, file.Title);
                }
            }
            return folders;

        }
        public File UploadFile(DriveService _service, ExportData _uploadFile)
        {
            File body = new File();
            body.Title = _uploadFile.FileName;
            body.Description = "File uploaded by Diamto Drive Sample";
            body.MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            body.Parents = new List<ParentReference>() { new ParentReference() { Id = _uploadFile.Folder } };

            System.IO.MemoryStream stream = new System.IO.MemoryStream(_uploadFile.Data);
            try
            {
                FilesResource.InsertMediaUpload request = _service.Files.Insert(body, stream, body.MimeType);
                request.Upload();
                return request.ResponseBody;
            }
            catch (Exception e)
            {
               
                return null;
            }
        }

        public DriveService Authorization()

        var services = new DriveService(новый BaseClientService.Initializer() {
                ApiKey = "[API ключ]", // с https://console.developers.google.com (открытый доступ API)
                ApplicationName = «Пример API - диска»
            });
    }
}
