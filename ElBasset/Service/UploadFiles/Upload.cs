using ElBasset.DataBase;
using ElBasset.DTO.DTO;

namespace ElBasset.Service.UploadFiles
{
    public static class Upload
    {
        // Videos 
        public static string UploadVideo(IFormFile File, int DepartmentId)
        {
            var VideoPath = Environment.CurrentDirectory + $"/wwwroot/Files/Video/{DepartmentId}";
            string VideoName = Guid.NewGuid() + Path.GetFileName(File.FileName.Replace(" ",""));
            string FinallPath = Path.Combine(VideoPath, VideoName);
            using (var stream = new FileStream(FinallPath, FileMode.Create))
            {
                File.CopyTo(stream);
            }
            return VideoName;
        }
        public static bool Deletevideo(string VideoName, int DepartmentId)
        {
            string FilePath = Environment.CurrentDirectory + $"/wwwroot/Files/Video/{DepartmentId}/{VideoName}";
            // Check if file exists with its full path
            if (System.IO.File.Exists(FilePath))
            {
                // If file found, delete it
                System.IO.File.Delete(FilePath);
                return true;
            }
            return false;
        }

        // Lecture
        public static string UploadLecturePdf(IFormFile File, int DepartmentId)
        {
            var LecturePath = Environment.CurrentDirectory + $"/wwwroot/Files/Lecture/{DepartmentId}";
            string LectureName = Guid.NewGuid() + Path.GetFileName(File.FileName.Replace(" ", ""));
            string FinallPath = Path.Combine(LecturePath, LectureName);
            using (var stream = new FileStream(FinallPath, FileMode.Create))
            {
                File.CopyTo(stream);
            }
            return LectureName;
        }
        public static bool DeleteLecture(string PdfName, int DepartmentId)
        {
            string FilePath = Environment.CurrentDirectory + $"/wwwroot/Files/Lecture/{DepartmentId}/{PdfName}";
            // Check if file exists with its full path
            if (System.IO.File.Exists(FilePath))
            {
                // If file found, delete it
                System.IO.File.Delete(FilePath);
                return true;
            }
            return false;
        }
        // Image
        public static string UploadImage(IFormFile File)
        {

            var ImagePath = Environment.CurrentDirectory + $"/wwwroot/Files/Images";
            string ImageName = Guid.NewGuid() + Path.GetFileName(File.FileName.Replace(" ", ""));
            string FinallPath = Path.Combine(ImagePath, ImageName);
            using (var stream = new FileStream(FinallPath, FileMode.Create))
            {
                File.CopyTo(stream);
            }
            return ImageName;
        }
        public static bool DeleteImage(string PhotoPath)
        {
            string Path = Environment.CurrentDirectory + $"/wwwroot/Files/Images/{PhotoPath}";
            // Check if file exists with its full path
            if (System.IO.File.Exists(Path))
            {
                // If file found, delete it
                System.IO.File.Delete(Path);
                return true;
            }
            return false;
        }
    }

}
