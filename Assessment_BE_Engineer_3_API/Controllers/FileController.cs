using Assessment_BE_Engineer_3_API.Models;
using Assessment_BE_Engineer_3_API.Models.DTO;
using Assessment_BE_Engineer_3_API.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Assessment_BE_Engineer_3_API.Controllers
{
    [ApiController]
    [Route("api/v1/files")]
    public class FileController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        protected APIResponse _response;
        IMapper _mapper;
		private readonly IWebHostEnvironment _hostEnvironment;

		public FileController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _response = new();
            _mapper = mapper;
			_hostEnvironment = hostEnvironment;
		}
        [HttpGet]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllAsync()
        {
            var Files = (await _unitOfWork.FileRepository.GetAllAsync());
            _response.Result = _mapper.Map<List<FileDTO>>(Files);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateAsync([FromForm] FileCreateDTO fileCreateDTO)
        {

            //get size and extension of file
            if (ModelState.IsValid)
            {
                FileModel fileToCreate = _mapper.Map<FileModel>(fileCreateDTO);

                string cleanedFileName = _unitOfWork.FileRepository.RemoveSpecialCharactersAndSpaces(fileToCreate.Name.ToLower());
                string fileExtension = Path.GetExtension(fileToCreate.FormFile.FileName);
                fileToCreate.Name = cleanedFileName+ Guid.NewGuid().ToString() + fileExtension;

                long fileSizeBytes = fileToCreate.FormFile.Length;
                fileToCreate.Size = _unitOfWork.FileRepository.FormatFileSize(fileSizeBytes);

                //string rootPath = _hostEnvironment.WebRootPath;
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string uploadsPath = Path.Combine(baseDirectory, "uploads");
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(uploadsPath);

                //var uploads = @"./uploads";
                using (var fileStreams = new FileStream(Path.Combine(uploadsPath, fileToCreate.Name), FileMode.Create))
                {
                    fileToCreate.FormFile.CopyTo(fileStreams);
                }
                fileToCreate.Location = uploadsPath+"/" + fileToCreate.Name;

                _unitOfWork.FileRepository.AddAsync(fileToCreate);
                _unitOfWork.Save();
                _response.Result = _mapper.Map<FileDTO>(fileToCreate);
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                _response.ErrorMessages = errors;

                return BadRequest(_response);
            }
        }
        [HttpGet("{id}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DownloadAsync(int id)
        {
            var file = _unitOfWork.FileRepository.GetFirstOrDefault(f => f.Id == id);
			if (file == null)
			{
				return NotFound("File not found");
			}
			// Path to the file you want to make available for download
			string filePath = file.Location; // Replace with the actual file path
            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File location not found");
            }
            // Read the file into a byte array
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            // Get the file name from the file path
            string fileName = Path.GetFileName(filePath);
            // Set response headers for content disposition (attachment) to trigger download
            Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
            // Return the file data as a file content result
            File(fileBytes, "application/octet-stream"); // Set content type to generic binary data
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = new Dictionary<string, byte[]>() { { file.Name, fileBytes } };
            return Ok(_response);
        }

		[HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<APIResponse> Delete(int id)
		{
			var file = _unitOfWork.FileRepository.GetFirstOrDefault(f => f.Id == id);
            if(file == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
			// Path to the file you want to make available for download
            _unitOfWork.FileRepository.Remove(file);
            _unitOfWork.Save();
			string filePath = file.Location;
			if (System.IO.File.Exists(filePath))
			{
                System.IO.File.Delete(filePath);
			}
			_response.IsSuccess = true;
			_response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
		}

	}
}
