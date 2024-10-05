using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BlazorPaliculas.Server.Helpers
{
    public class AlmacenadorArchivos : IAlmacenadorArchivos
    {
        private readonly string connectionString;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AlmacenadorArchivos(IConfiguration configuration, IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            connectionString = configuration.GetConnectionString("AzureStorage")!;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        //Para Manejo de Imagenes en AZURE Containers

        public async Task RemoveFileAsync(string path, string containerName)
        {
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(path);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> SaveFileAsync(byte[] content, string extention, string containerName)
        {
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);

            var fileName = $"{Guid.NewGuid()}{extention}";
            var blob = client.GetBlobClient(fileName);

            using (var ms = new MemoryStream(content))
            {
                await blob.UploadAsync(ms);
            }
            //Es para obtener la url completa junto con el archivo
            return blob.Uri.ToString();

            //Solo para guardar en DB el Nombre del Archivo
            //return fileName;
        }


        //Para Manejo de Imagenes en Disco

        public async Task<string> UploadImage(IFormFile imageFile, string ruta, string guid)
        {
            var file = guid;
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                ruta,
                file);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"{file}";
        }

        public async Task<string> UploadImage(byte[] imageFile, string extention, string NameFolder)
        {
            var file = $"{Guid.NewGuid()}{extention}";
            var folder = Path.Combine(_env.WebRootPath, NameFolder); //ejemplo wwwroot\\Images\\ImgUser
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string RouteSave = Path.Combine(folder, file);
            await File.WriteAllBytesAsync(RouteSave, imageFile);

            //Este es para tomar la URL Completo con el Archivo
            var urlActual = $"{_httpContextAccessor!.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var rutaParaBd = Path.Combine(urlActual, NameFolder, file).Replace("\\", "/");

            //Este es para guardar la ruta con nombre del archivo y guardarlo
            return rutaParaBd;

            //este es para guardar solo el nombre del archivo
            //return $"{file}";
        }

        //Para Eliminar el archivo donde solo tenemos guardado el nombre del archivo
        public bool DeleteImage(string ruta, string guid)
        {
            string path;
            path = Path.Combine(
                Directory.GetCurrentDirectory(),
                ruta,
                guid);

            File.Delete(path);

            return true;
        }

        public Task EliminarArchivo(string ruta, string nombreContenedor)
        {
            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(_env.WebRootPath, nombreContenedor, nombreArchivo);
            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }
            return Task.CompletedTask;
        }

    }
}
