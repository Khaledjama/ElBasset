using AutoMapper;
using ElBasset.DataBase;
using ElBasset.DTO.DTO;
using ElBasset.Repo.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Composition;
using System.Security.Claims;

namespace ElBasset.Controllers.Videos
{
    [Authorize(Roles = "الطالب")]
    public class ListVideoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListVideoController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> UserManager, IMapper Mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = Mapper;
            this._userManager = UserManager;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var CurrentUser = _userManager.Users.FirstOrDefault(s => s.Id == userId);
                var AllVideoList = (await _unitOfWork.VideoRepo.GetAllAsync()).Where(s => s.DepartmentId == CurrentUser.DepartmentId).ToList();
                var Data = _mapper.Map<List<Video>, List<VideosDTO>>(AllVideoList);
                return View(Data);
            }
            catch (Exception)
            {

                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Filter(int Id)
        {

            var CurrentVideo = await _unitOfWork.VideoRepo.GetByIdAsync(Id);
            var Data = _mapper.Map<VideosDTO>(CurrentVideo);
            return View(Data);
        }
    }
}
