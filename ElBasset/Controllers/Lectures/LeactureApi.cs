using ElBasset.DataBase;
using ElBasset.Repo.UnitOfWork;
using ElBasset.Service.DeleteFiles;
using ElBasset.Service.UploadFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElBasset.Controllers.Lectures
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LectureApi : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public LectureApi(IUnitOfWork unit)
        {
            this._unit = unit;
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLecture()
        {
            string name = Request.Form["lName"];
            string depId = Request.Form["depId"];
            string id = Request.Form["id"];
            int DepId = Convert.ToInt32(depId);
            int Id = Convert.ToInt32(id);
            IFormFile postedFile1 = Request.Form.Files[0];
            var CurrentLecture = await _unit.LecturetRepo.GetByIdAsync(Id);
            // Delete Old Pdf First
            var DeletOldFileResult = Upload.DeleteLecture(CurrentLecture.PdfLink, CurrentLecture.DepartmentId);
            if (!DeletOldFileResult)
                return BadRequest();
            var PdfLink = Upload.UploadLecturePdf(postedFile1, DepId);
            CurrentLecture.PdfLink = PdfLink;
            CurrentLecture.Name = name;
            CurrentLecture.DepartmentId = DepId;
            if (await _unit.CompleteAsync()>0) 
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> UploadLecture()
        {
            string name = Request.Form["lName"];
            string depId = Request.Form["depId"];
            int DepId = Convert.ToInt32(depId);
            IFormFile postedFile1 = Request.Form.Files[0];
            var PdfLink = Upload.UploadLecturePdf(postedFile1, DepId);
            var CurrentLecture = new Lecture() { dateTime = DateTime.Now, Name = name, DepartmentId = DepId, PdfLink = PdfLink };
            await _unit.LecturetRepo.Insert(CurrentLecture);
            if (await _unit.CompleteAsync()>0) 
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLecture()
        {
            string id = Request.Form["id"];
            int Id = Convert.ToInt32(id);
            var CurreLecture = await _unit.LecturetRepo.GetByIdAsync(Id);
            var DeleteLectureResult = Upload.DeleteLecture(CurreLecture.PdfLink, CurreLecture.DepartmentId);
            if (!DeleteLectureResult)
                return BadRequest();
             _unit.LecturetRepo.Delete(CurreLecture.Id);
            if (await _unit.CompleteAsync()>0) 
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> GetAllLectureByDepId()
        {
            string id = Request.Form["id"];
            int Id = Convert.ToInt32(id);
            var AllData = (await _unit.LecturetRepo.FindAsync(s => s.DepartmentId == Id)).ToList();
            return Ok(AllData);
        }
    }
}
