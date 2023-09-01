using AutoMapper;
using ElBasset.DataBase;
using ElBasset.DataBase.DataBase;
using ElBasset.DTO.DTO;
using ElBasset.Repo.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElBasset.Controllers.Exam
{
    [Authorize(Roles = "الادمن")]
    public class ExamsController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        public ExamsController(IUnitOfWork unit, IMapper mapper, AppDbContext db)
        {
            _unit = unit;
            _mapper = mapper;
            _db = db;
        }
        public async Task<IActionResult> AllExam()
        {
            try
            {
                var AllExam = _db.Exams.Include(s => s.ExamType).Include(s => s.Department).ToList();
                ViewBag.AllDepartment = await _unit.DepartmentRepo.GetAllAsync();
                var Data = _mapper.Map<IEnumerable<DataBase.Exam>, IEnumerable<ExamDTO>>(AllExam);
                return View(Data);
            }
            catch 
            {
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Filter(int DepartmentId)
        {

            ViewBag.AllDepartment = await _unit.DepartmentRepo.GetAllAsync();
            var AllExams = (await _unit.ExamRepo.GetAllAsync()).Where(s => s.DepartmentId == DepartmentId).ToList();
            var Data = _mapper.Map<IEnumerable<DataBase.Exam>, IEnumerable<ExamDTO>>(AllExams);
            return View("AllExam", Data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllDepartment = await _unit.DepartmentRepo.GetAllAsync();
            ViewBag.AllExamType = await _unit.ExamTypeRepo.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExamDTO model)
        {
            if (ModelState.IsValid)
            {
                var ResultDate = DateTime.Compare(model.StartDateTimeExam, model.EndDateTimeExam);
                if (ResultDate < 0)
                    model.Statues = true;
                else
                    model.Statues = false;
                var Data = _mapper.Map<DataBase.Exam>(model);
                await _unit.ExamRepo.Insert(Data);
                if (await _unit.CompleteAsync() > 0)
                    return RedirectToAction(nameof(AllExam));
            }
            ViewBag.AllDepartment = await _unit.DepartmentRepo.GetAllAsync();
            ViewBag.AllExamType = await _unit.ExamTypeRepo.GetAllAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {

            if (Id is null)
                return BadRequest();
            var CurrentExam = await _unit.ExamRepo.GetByIdAsync(Id);
            if (CurrentExam is null)
                return NotFound();
            var Data = _mapper.Map<ExamDTO>(CurrentExam);
            ViewBag.AllDepartment = await _unit.DepartmentRepo.GetAllAsync();
            ViewBag.AllExamType = await _unit.ExamTypeRepo.GetAllAsync();
            return View(Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ExamDTO model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == null)
                    return NotFound();
                var CurrentExam = await _unit.ExamRepo.GetByIdAsync(model.Id);
                if (CurrentExam == null)
                    return NotFound();
                var ResultDate = DateTime.Compare(model.StartDateTimeExam, model.EndDateTimeExam);
                if (ResultDate < 0)
                    model.Statues = true;
                else
                    model.Statues = false;
                var Data = _mapper.Map(model, CurrentExam);
                 _unit.ExamRepo.Update(Data);
                if (await _unit.CompleteAsync() > 0)
                    return RedirectToAction(nameof(AllExam));
            }
            ViewBag.AllDepartment = await _unit.DepartmentRepo.GetAllAsync();
            ViewBag.AllExamType = await _unit.ExamTypeRepo.GetAllAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id is null)
                return BadRequest();
            var CurrentExam = await _unit.ExamRepo.GetByIdAsync(Id);
            if (CurrentExam is null)
                return NotFound();
            var Data = _mapper.Map<ExamDTO>(CurrentExam);
            return View(Data);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var CurrentExam = await _unit.ExamRepo.GetByIdAsync(Id);
            if (CurrentExam is null)
                return NotFound();
            _unit.ExamRepo.Delete(Id);
            if (await _unit.CompleteAsync() > 0)
                return RedirectToAction(nameof(AllExam));
            return View(CurrentExam);
        }
        
    }
}
