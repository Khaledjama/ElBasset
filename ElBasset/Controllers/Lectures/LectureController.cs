using AutoMapper;
using ElBasset.DataBase;
using ElBasset.DTO.DTO;
using ElBasset.Repo.UnitOfWork;
using ElBasset.Service.UploadFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElBasset.Controllers.Lectures
{
    [Authorize(Roles = "الادمن")]
    public class LectureController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _Mapper;
        public LectureController(IUnitOfWork _unit, IMapper Map)
        {
            this._unit = _unit;
            this._Mapper = Map;
        }
        public async Task<IActionResult> AllLecture()
        {
            await Data();
            var AllLecture = new List<Lecture>();
            return View(AllLecture);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await Data();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Filter(int DepartmentId)
        {
            
            await Data();
            var AllLecture = (await _unit.LecturetRepo.GetAllAsync()).Where(s => s.DepartmentId == DepartmentId).ToList();
            return View("AllLecture", AllLecture);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            if (Id == 0)
                return View("~/Views/Shared/Error.cshtml");
            var CurrentLecture =await _unit.LecturetRepo.GetByIdAsync(Id);
            if(CurrentLecture==null)
                return View("~/Views/Shared/Error.cshtml");
            await Data();
            return View(CurrentLecture);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(int Id)
        {
            var CurrentLecture =await _unit.LecturetRepo.GetByIdAsync(Id);
            if (CurrentLecture==null)
            {
                return NotFound();
            }
            var DeleteLectureResult = Upload.DeleteLecture(CurrentLecture.PdfLink, CurrentLecture.DepartmentId);
            if (!DeleteLectureResult)
                return View("~/View/Shared/Error.cshtml");
            _unit.LecturetRepo.Delete(CurrentLecture);
            if (await _unit.CompleteAsync() > 0)
            {
                await Data();
                return RedirectToAction("AllLecture");
            }
            return View("~/View/Shared/Error.cshtml");
        }
        public async Task<IActionResult> DownloadPdf(int Id)
        {
            try
            {
                var FileName = (await _unit.LecturetRepo.GetByIdAsync(Id)).PdfLink;
                int DepartmentId = (await _unit.LecturetRepo.GetByIdAsync(Id)).DepartmentId;
                var LecturePath = Environment.CurrentDirectory + $"/wwwroot/Files/Lecture/{DepartmentId}/{FileName}";
                string outputFilePath = Path.Combine(LecturePath);

                if (!System.IO.File.Exists(outputFilePath))
                {
                    ViewBag.Message = "هناك خطأ";
                }

                var fileInfo = new System.IO.FileInfo(outputFilePath);
                Response.ContentType = "application/pdf";
                Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
                Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

                // Send the file to the client
                return File(System.IO.File.ReadAllBytes(outputFilePath), "application/pdf", fileInfo.Name);
            }
            catch 
            {

                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private async Task Data()
        {
            var AllDepartments = await _unit.DepartmentRepo.GetAllAsync();
            @ViewBag.AllDepartment = AllDepartments;
        }
    }
}
