using AutoMapper;
using ElBasset.DataBase;
using ElBasset.Repo.UnitOfWork;
using ElBasset.Service.UploadFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElBasset.Controllers.Videos
{
    [Authorize(Roles = "الادمن")]
    public class VideoController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _Mapper;
        public VideoController(IUnitOfWork _unit, IMapper Map)
        {
            this._unit = _unit;
            this._Mapper = Map;
        }
        public async Task<IActionResult> AllVideo()
        {
            await Data();
            var AllLecture = new List<Video>();
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
            var AllLecture = (await _unit.VideoRepo.GetAllAsync()).Where(s => s.DepartmentId == DepartmentId).ToList();
            return View("AllVideo", AllLecture);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            if (Id == 0)
                return View("~/Views/Shared/Error.cshtml");
            var CurrentLecture = await _unit.VideoRepo.GetByIdAsync(Id);
            if (CurrentLecture ==null)
                return View("Views/Shared/Error.cshtml");
            await Data();
            return View(CurrentLecture);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(int Id)
        {
            var CurrentLecture = await _unit.VideoRepo.GetByIdAsync(Id);
            if (CurrentLecture == null)
            {
                return NotFound();
            }
            var DeleteLectureResult = Upload.DeleteLecture(CurrentLecture.VideoLink, CurrentLecture.DepartmentId);
            if (!DeleteLectureResult)
                return View("~/View/Shared/Error.cshtml");
            _unit.VideoRepo.Delete(CurrentLecture);
            if (await _unit.CompleteAsync() > 0)
            {
                await Data();
                return RedirectToAction("AllLecture");
            }
            return View("~/View/Shared/Error.cshtml");
        }
        private async Task Data()
        {
            var AllDepartments = await _unit.DepartmentRepo.GetAllAsync();
            @ViewBag.AllDepartment = AllDepartments;
        }
    }
}
