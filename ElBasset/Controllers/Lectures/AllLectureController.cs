using AutoMapper;
using ElBasset.DataBase;
using ElBasset.DTO.DTO;
using ElBasset.Repo.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElBasset.Controllers.Lectures
{
    [Authorize(Roles = "الطالب")]
    public class AllLectureController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public AllLectureController(IUnitOfWork UnitOfWork, UserManager<ApplicationUser> UserManager, IMapper Mapper)
        {
            this._unitOfWork = UnitOfWork;
            this._userManager = UserManager;
            this._mapper = Mapper;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                // Get Current Users 
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var CurrentUser = _userManager.Users.FirstOrDefault(s => s.Id == userId);
                var AllLecture = (await _unitOfWork.LecturetRepo.GetAllAsync()).Where(s => s.DepartmentId == CurrentUser.DepartmentId).ToList();
                var Data = _mapper.Map<List<Lecture>, List<LectureDTO>>(AllLecture);
                return View(Data);
            }
            catch 
            {

                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public async Task<IActionResult> DownloadLecture(int Id)
        {
            try
            {
                var FileName = (await _unitOfWork.LecturetRepo.GetByIdAsync(Id)).PdfLink;
                int DepartmentId = (await _unitOfWork.LecturetRepo.GetByIdAsync(Id)).DepartmentId;
                var LecturePath = Environment.CurrentDirectory + $"/wwwroot/Files/Lecture/{DepartmentId}/{FileName}";
                string outputFilePath = Path.Combine(LecturePath);

                if (!System.IO.File.Exists(outputFilePath))
                {
                    return NotFound();
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

    }
}
