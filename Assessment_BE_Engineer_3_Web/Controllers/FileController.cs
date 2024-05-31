using Assessment_BE_Engineer_3_Utility;
using Assessment_BE_Engineer_3_Web.Models;
using Assessment_BE_Engineer_3_Web.Models.DTO;
using Assessment_BE_Engineer_3_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Assessment_BE_Engineer_3_Web.Controllers
{
	public class FileController : Controller
	{
		private readonly IFileService _fileService;
		private string fileUrl;
		public FileController(IFileService fileService, IConfiguration configuration)
		{
			_fileService = fileService;
			fileUrl = configuration.GetValue<string>("ServiceUrls:FileAPI");
		}
		[Authorize]
		public async Task<IActionResult> Index()
		{
			List<FileDTO> list = new();
			var token = HttpContext.Session.GetString(SD.SessionToken);
			var response = await _fileService.GetAllAsync<APIResponse>(token);
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<FileDTO>>(Convert.ToString(response.Result));
			}
			return View(list);
		}

		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(FileCreateDTO fileCreateDTO)
		{
			var token = HttpContext.Session.GetString(SD.SessionToken);
			var response = await _fileService.CreateAsync<APIResponse>(fileCreateDTO, token);
			if (response != null && response.IsSuccess)
			{
				return RedirectToAction(nameof(Index));
			}
			return View(fileCreateDTO);
		}

		[HttpGet]
		public async Task<IActionResult> Download(int id)
		{
			try
			{
				var token = HttpContext.Session.GetString(SD.SessionToken);
				var response = await _fileService.DownloadAsync<APIResponse>(id, token);
                if (response != null && response.IsSuccess)
                {
					var result = JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(Convert.ToString(response.Result));
                    return Json(new { success = true, filename=result.Keys.First(),bytes = result.Values.First() });
            }


                else
            {
                // Handle API error response
					throw new Exception("API request failed");
				}
			}
			catch (Exception ex)
			{
				// Handle any exceptions that may occur during the request
				throw new Exception("Error downloading file: " + ex.Message);
			}

		}
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var token = HttpContext.Session.GetString(SD.SessionToken);
			var response = await _fileService.DeleteAsync<APIResponse>(id, token);
			if (response != null && response.IsSuccess)
			{
				return Json(new { success = true });
			}
			else
			{
				// Handle API error response
				return Json(new { success = false });
			}
		}

	}
}
