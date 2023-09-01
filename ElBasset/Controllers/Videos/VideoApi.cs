using ElBasset.DataBase;
using ElBasset.Repo.UnitOfWork;
using ElBasset.Service.UploadFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElBasset.Controllers.Videos
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VideoApi : ControllerBase
    {

        private readonly IUnitOfWork _unit;
        public VideoApi(IUnitOfWork unit)
        {
            this._unit = unit;
        }
        [HttpPost]
        public async Task<IActionResult> UploadVideo()
        {
            string name = Request.Form["lName"];
            string depId = Request.Form["depId"];
            string Decription = Request.Form["Vdescription"];
            string video = Request.Form["video"];
            int DepId = Convert.ToInt32(depId);
            IFormFile postedFile1 = Request.Form.Files[0];
            IFormFile imageFile = Request.Form.Files[1];
            var ImageVideoPath = Upload.UploadImage(imageFile);
            var videoLink = Upload.UploadVideo(postedFile1, DepId);
            var CurrentVideo = new Video() { dateTime = DateTime.Now, Name = name,VideoDecription=Decription, VideoLink = videoLink,ImageVideoPath= ImageVideoPath, DepartmentId = DepId };
            await _unit.VideoRepo.Insert(CurrentVideo);
            if (await _unit.CompleteAsync() > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVideo()
        {
            string name = Request.Form["lName"];
            string depId = Request.Form["depId"];
            string Description = Request.Form["Vdescription"];
            string id = Request.Form["id"];
            int DepId = Convert.ToInt32(depId);
            int Id = Convert.ToInt32(id);
            IFormFile postedFile1 = Request.Form.Files[0];
            IFormFile ImageFile1 = Request.Form.Files[1];
            var CurrnetVideo = await _unit.VideoRepo.GetByIdAsync(Id);
            //Delete Old Pdf First
            var DeletOldFileResult = Upload.Deletevideo(CurrnetVideo.VideoLink, CurrnetVideo.DepartmentId);
            if (!DeletOldFileResult)
                return BadRequest();
            var DeleteCurrentImage = Upload.DeleteImage(CurrnetVideo.ImageVideoPath);
            if (!DeleteCurrentImage)
                return BadRequest();
            var VideoLink = Upload.UploadVideo(postedFile1, DepId);
            var ImageLink = Upload.UploadImage(ImageFile1);
            CurrnetVideo.VideoLink = VideoLink;
            CurrnetVideo.Name = name;
            CurrnetVideo.DepartmentId = DepId;
            CurrnetVideo.ImageVideoPath = ImageLink;
            CurrnetVideo.VideoDecription = Description;
            if (await _unit.CompleteAsync() > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVideo()
        {
            string id = Request.Form["id"];
            int Id = Convert.ToInt32(id);
            var CurreVideo = await _unit.VideoRepo.GetByIdAsync(Id);
            var DeletVideoResult = Upload.Deletevideo(CurreVideo.VideoLink, CurreVideo.DepartmentId);
            if (!DeletVideoResult)
                return BadRequest();
            var DeleteImageResult = Upload.DeleteImage(CurreVideo.ImageVideoPath);
            if (!DeleteImageResult)
                return BadRequest();
            _unit.VideoRepo.Delete(CurreVideo.Id);
            if (await _unit.CompleteAsync() > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
