using azurefilestoragedpm.Repositories;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace azurefilestoragedpm.Controllers
{
    public class HomeController : Controller
    {
        RepositoryFileAzureStorage repository;

        public HomeController()
        {
            repository = new RepositoryFileAzureStorage();
        }

        // GET: Index
        [HttpGet]
        public ActionResult Index(string accion, string fichero)
        {
            if (accion == "eliminar")
            {
                repository.EliminarFichero(fichero);
            }

            List<string> ficheros = repository.GetListadoFicheros();
            ViewBag.Ficheros = ficheros;


            return View();
        }

        // POST: Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string accion, HttpPostedFileBase ficheroSubir)
        {
            List<string> ficheros = repository.GetListadoFicheros();
            ViewBag.Ficheros = ficheros;

            if (accion == "muestra")
            {
                string clave = CloudConfigurationManager.GetSetting("cadenaConexion");
                CloudStorageAccount cuenta = CloudStorageAccount.Parse(clave);
                CloudFileClient cliente = cuenta.CreateCloudFileClient();
                CloudFileShare recurso = cliente.GetShareReference("direjemplo");
                CloudFileDirectory directorio = recurso.GetRootDirectoryReference();
                CloudFile fichero = directorio.GetFileReference("ficheromuestra.txt");
                string texto = await fichero.DownloadTextAsync();

                ViewBag.Texto = texto;
            }
            else if (accion == "subir")
            {
                repository.CrearFichero(ficheroSubir);
            }

            return View();
        }
    }
}