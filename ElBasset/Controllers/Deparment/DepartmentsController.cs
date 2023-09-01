using AutoMapper;
using ElBasset.DataBase;
using ElBasset.DTO.DTO;
using ElBasset.Repo.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElBasset.Controllers.Deparment
{
    [Authorize(Roles = "الادمن")]
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _Unit;
        private readonly IMapper _mapper;
        public DepartmentsController(IUnitOfWork unit, IMapper map)
        {
            _Unit = unit;
            _mapper = map;
        }
        public async Task<IActionResult> Index()
        {
            var AllDep = await _Unit.DepartmentRepo.GetAllAsync();
            var Data = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentDTO>>(AllDep);
            return View(Data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var Data = _mapper.Map<Department>(model);
            await _Unit.DepartmentRepo.Insert(Data);
            if (await _Unit.CompleteAsync() > 0)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var CurrentDepartment = await _Unit.DepartmentRepo.GetByIdAsync(Id);
            var Data = _mapper.Map<DepartmentDTO>(CurrentDepartment);
            return View(Data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var Data = _mapper.Map<Department>(model);
            _Unit.DepartmentRepo.Update(Data);
            if (await _Unit.CompleteAsync() > 0)
                return RedirectToAction("Index");
            return View(Data);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var CurrentDepartment = await _Unit.DepartmentRepo.GetByIdAsync(Id);
            var Data = _mapper.Map<DepartmentDTO>(CurrentDepartment);
            return View(Data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(int Id)
        {
            var CurrentDepartment = _Unit.DepartmentRepo.GetByIdAsync(Id);
            if (CurrentDepartment == null)
                return View("~/Views/Shared/Error.cshtml");
            _Unit.DepartmentRepo.Delete(Id);
            if (await _Unit.CompleteAsync() > 0)
                return RedirectToAction("Index");
            return View("~/Views/Shared/Error.cshtml");
        }
    }

}
