using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebNovel.ViewModels;
using IoFile = System.IO.File;

namespace WebNovel.Controllers.Api
{
    [Authorize]
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Contributor")]
        public async Task<ActionResult> News_Upload_Save(string metaData)
        {
            var files = Request.Form.Files;
            if (metaData == null)
            {
                return await Save(files);
            }
            var fileBlob = Upload_Save(files, metaData, "news_cover");
            return Json(fileBlob);

        }
        [Authorize(Roles = "Admin,Contributor")]
        public ActionResult News_Upload_Remove(string[] fileNames)
        {
            Upload_Remove(fileNames, "news_cover");
            return Content("");
        }

        [Authorize(Roles = "Admin,Contributor")]
        public async Task<ActionResult> Story_Upload_Save(string metaData)
        {
            var files = Request.Form.Files;
            if (metaData == null)
            {
                return await Save(files);
            }
            var fileBlob = Upload_Save(files, metaData, "story_cover");
            return Json(fileBlob);

        }
        [Authorize(Roles = "Admin,Contributor")]
        public ActionResult Story_Upload_Remove(string[] fileNames)
        {
            Upload_Remove(fileNames, "story_cover");
            return Content("");
        }
        public async Task<ActionResult> Upload_Avatar([FromForm] IFormFile CroppedImage, [FromForm] string CurrentName)
        {
            if (CroppedImage == null || CroppedImage.Length == 0)
            {
                return BadRequest(new { Success = false, Message = "Invalid image data." });
            }
            ResponseViewModel<string> result = new ResponseViewModel<string>();
            try
            {
                var fileName = $"{Guid.NewGuid()}.jpg";
                var path = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "EmpImgs", fileName);
                if (!Directory.Exists(Directory.GetParent(path).FullName))
                {
                    Directory.CreateDirectory(Directory.GetParent(path).FullName);
                }
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await CroppedImage.CopyToAsync(stream);
                }
                // Giải mã chuỗi base64
                if (!string.IsNullOrEmpty(CurrentName))
                {
                    var physicalPath = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "EmpImgs", CurrentName);

                    if (IoFile.Exists(physicalPath))
                    {
                        IoFile.Delete(physicalPath);
                    }
                }
                result.Success = true;
                result.Data = new List<string> { fileName };
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return Json(result);
            }
        }
        public async Task<ActionResult> Chunk_Upload_CitizenIDImg_Save(string metaData)
        {
            var files = Request.Form.Files;
            if (metaData == null)
            {
                return await Save(files);
            }
            var fileBlob = Upload_Save(files, metaData, "CCCD");
            return Json(fileBlob);

        }
        public ActionResult Chunk_Upload_CitizenIDImg_Remove(string[] fileNames)
        {
            Upload_Remove(fileNames, "CCCD");
            return Content("");
        }



        private async Task<ActionResult> Save(IEnumerable<IFormFile> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                    var physicalPath = Path.Combine(_env.ContentRootPath, "Uploads", fileName);

                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            return Content("");
        }

        private FileResultViewModel Upload_Save(IFormFileCollection files, string metaData, string folderName)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(metaData));

            JsonSerializer serializer = new JsonSerializer();
            ChunkMetaDataViewModel chunkData;
            using (StreamReader streamReader = new StreamReader(ms))
            {
                chunkData = (ChunkMetaDataViewModel)serializer.Deserialize(streamReader, typeof(ChunkMetaDataViewModel));
            }

            string path = String.Empty;
            var extensition = Path.GetExtension(chunkData.FileName);
            var name = Path.GetFileNameWithoutExtension(chunkData.FileName);
            var newFileName = $"{name}__{DateTime.Now.ToString("yyyyMMdd__HHmmssfff")}{extensition}";

            if (files != null)
            {
                foreach (var file in files)
                {
                    path = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", folderName, newFileName);
                    AppendToFile(path, file);
                }
            }

            FileResultViewModel fileBlob = new FileResultViewModel();
            fileBlob.uploaded = chunkData.TotalChunks - 1 <= chunkData.ChunkIndex;
            fileBlob.fileUid = chunkData.UploadUid;
            fileBlob.fileName = newFileName;

            return fileBlob;
        }

        private void AppendToFile(string fullPath, IFormFile content)
        {
            try
            {
                var dir = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                using (FileStream stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    content.CopyTo(stream);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        private string Upload_Remove(string[] fileNames, string folderName)
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", folderName, fileName);

                    if (IoFile.Exists(physicalPath))
                    {
                        IoFile.Delete(physicalPath);
                    }
                }
            }
            return "";
        }

    }
}
