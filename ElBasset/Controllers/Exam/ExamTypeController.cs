using AutoMapper;
using ElBasset.DataBase;
using ElBasset.DTO.DTO;
using ElBasset.Repo.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElBasset.Controllers.Exam
{
    [Authorize(Roles = "الادمن")]
    public class ExamTypeController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public ExamTypeController(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        public async Task<IActionResult> AllExamType()
        {
            var AllExamType = await _unit.ExamTypeRepo.GetAllAsync();
            var Data = _mapper.Map<IEnumerable<ExamType>, IEnumerable<ExamTypeDTO>>(AllExamType);
            return View(Data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExamTypeDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var Data = _mapper.Map<ExamType>(model);
            await _unit.ExamTypeRepo.Insert(Data);
            if (await _unit.CompleteAsync() > 0)
                return RedirectToAction(nameof(AllExamType));
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id is null)
                return BadRequest();
            var CurrentExamType = await _unit.ExamTypeRepo.GetByIdAsync(Id);
            if (CurrentExamType is null)
                return NotFound();
            var Data = _mapper.Map<ExamTypeDTO>(CurrentExamType);
            return View(Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ExamTypeDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var Data = _mapper.Map<ExamType>(model);
            _unit.ExamTypeRepo.Update(Data);
            if (await _unit.CompleteAsync() > 0)
                return RedirectToAction(nameof(AllExamType));
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id is null)
                return BadRequest();
            var CurrentExamType = await _unit.ExamTypeRepo.GetByIdAsync(Id);
            if (CurrentExamType is null)
                return NotFound();
            return View(CurrentExamType);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var CurrentExamType = await _unit.ExamTypeRepo.GetByIdAsync(Id);
            if (CurrentExamType is null)
                return NotFound();
            _unit.ExamTypeRepo.Delete(Id);
            if (await _unit.CompleteAsync() > 0)
                return RedirectToAction(nameof(AllExamType));
            return View(CurrentExamType);
        }
    }
}
