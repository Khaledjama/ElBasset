using AutoMapper;
using ElBasset.DataBase;
using ElBasset.DataBase.DataBase;
using ElBasset.DTO.DTO;
using ElBasset.Repo.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ElBasset.Controllers.Exam
{
    [Authorize(Roles = "الطالب")]
    public class AllExamsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public AllExamsController(AppDbContext Db, UserManager<ApplicationUser> UserManager, IMapper Mapper)
        {
            this._db = Db;
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
                var AllExam = _db.Exams.Include(s => s.ExamType).Include(s => s.Department).Where(s=>s.DepartmentId==CurrentUser.DepartmentId).ToList();
                var Data = _mapper.Map<IEnumerable<DataBase.Exam>, IEnumerable<ExamDTO>>(AllExam);
                return View(Data);
            }
            catch
            {

                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
