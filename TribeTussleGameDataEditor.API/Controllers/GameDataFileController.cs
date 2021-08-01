using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using TribeTussleGameDataEditor.API.Controllers.Util;
using TribeTussleGameDataEditor.API.DAL.Repositories;
using System.Web;

namespace TribeTussleGameDataEditor.API.Controllers
{
    [ApiVersion("1"), Route("game-data-file")]
    [ApiController]
    [Produces("application/octet-stream")]
    public class GameDataFileController : ControllerBase
    {
        private IGameDataRepository _gameDataRepository;
        private long CurrentUserId
        {
            get
            {
                return long.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            }
        }

        public GameDataFileController(IGameDataRepository gameDataRepository)
        {
            _gameDataRepository = gameDataRepository;
        }

        // GET: api/v1/game-data-file/name
        [HttpGet("{name}")]
        public ActionResult GetGameDataFile(string name)
        {
            if (!_gameDataRepository.GameDataFileExists(CurrentUserId, name))
                return NotFound();
            byte[] fileData = _gameDataRepository.GetGameFileContents(CurrentUserId, name, out string fileName);
            string contentType = "application/octet-stream";
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true,
            };
            Response.Headers.Append("Content-Disposition", cd.ToString());
            return File(fileData, contentType);
        }
    }
}
