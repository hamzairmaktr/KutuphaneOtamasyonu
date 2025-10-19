using Microsoft.AspNetCore.Components.Forms;

namespace IKitaplik.BlazorUI.Helpers
{
    public static class FileConverter
    {
        public static async Task<IFormFile> ConvertToIFormFile(IBrowserFile browserFile)
        {
            var stream = browserFile.OpenReadStream();
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, browserFile.Name, browserFile.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = browserFile.ContentType
            };

            return formFile;
        }
    }

}
