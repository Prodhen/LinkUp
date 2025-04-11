using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace API.Helper
{
    public static class ImageHelper
    {
        private static IWebHostEnvironment _environment;
        public static void Initialize(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public static async Task<string> ProcessPicture(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(formFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var picturePath = $"/uploads/{uniqueFileName}";

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                return picturePath;
            }
            catch (Exception ex)
            {
                return $"Error uploading file: {ex.Message}";
            }
        }

    }
}
