using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cinemax.Application.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(IFormFile file, string containerName);
        Task DeleteAsync(string blobName, string containerName);
    }
}
