using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace azurefilestoragedpm.Repositories
{
    public class RepositoryAzureApi
    {
        public Uri GenerarTokenFile()
        {
            string clave = CloudConfigurationManager.GetSetting("cadenaazurefilestorage");
            CloudStorageAccount cuenta = CloudStorageAccount.Parse(clave);
            CloudFileClient cliente = cuenta.CreateCloudFileClient();
            CloudFileShare recursoCompartido = cliente.GetShareReference("filestorage");

            SharedAccessFilePolicy permisos = new SharedAccessFilePolicy();
            permisos.SharedAccessExpiryTime = DateTime.Now.AddMinutes(5);
            permisos.Permissions = SharedAccessFilePermissions.Create 
                | SharedAccessFilePermissions.Delete 
                | SharedAccessFilePermissions.List 
                | SharedAccessFilePermissions.Read 
                | SharedAccessFilePermissions.Write;

            string strToken = recursoCompartido.GetSharedAccessSignature(permisos);
            string aux = recursoCompartido.Uri + strToken;
            Uri token = new Uri(aux);

            return token;
        }
    }
}