using ElBasset.DataBase;
using Microsoft.AspNetCore.Hosting;

namespace ElBasset.Service.DeleteFiles
{
    public static class Delete
    {
        public static bool  DeleteLectureFile(string fileName, int DepartmentId)
        {
            var ExistingFile = Environment.CurrentDirectory + $"/wwwroot/Files/Lecture/{DepartmentId}/{fileName}";
            if (ExistingFile.Count() > 0)
            {
                System.IO.File.Delete(ExistingFile);
                return true;
            }
            return false;

        }
    }
}
