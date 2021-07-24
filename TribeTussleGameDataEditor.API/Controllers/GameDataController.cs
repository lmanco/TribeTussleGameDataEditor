using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Security.Claims;
using TribeTussleGameDataEditor.API.Controllers.Util;
using TribeTussleGameDataEditor.API.DAL.Models;
using TribeTussleGameDataEditor.API.DAL.Repositories;

namespace TribeTussleGameDataEditor.API.Controllers
{
    [ApiVersion("1"), Route("game-data")]
    [ApiController]
    [Produces("application/json")]
    public class GameDataController : ControllerBase
    {
        private const string CreationErrorTitle = "Invalid New Game Data";
        private const string NameExistsErrorDetail = "Game data with the same name already exists.";
        private const string UpdateErrorTitle = "Invalid Game Data Update";
        private const string NotFoundTitle = "Not Found";
        private const string NotFoundDetail = "The requested game data was not found.";
        private const string NotFoundForUpdateDetail = "The requested game data to update was not found.";
        private const string NotFoundForDeleteDetail = "The requested game data to delete was not found.";

        private IGameDataRepository _gameDataRepository;
        private readonly IMapper _mapper;
        private readonly IResponseObjectFactory _responseObjectFactory;
        private long CurrentUserId
        {
            get
            {
                return long.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            }
        }

        public GameDataController(IGameDataRepository gameDataRepository, IMapper mapper, IResponseObjectFactory responseObjectFactory)
        {
            _gameDataRepository = gameDataRepository;
            _mapper = mapper;
            _responseObjectFactory = responseObjectFactory;
        }

        // GET: api/v1/game-data
        [HttpGet]
        public ActionResult<IResponseObject> GetGameDataNamesList()
        {
            return _responseObjectFactory.CreateResponseObject(_gameDataRepository.ListNamesByUserId(CurrentUserId));
        }

        // GET: api/v1/game-data/name
        [HttpGet("{name}")]
        public ActionResult<IResponseObject> GetGameData(string name)
        {
            GameData gameData = _gameDataRepository.GetByUserIdAndName(CurrentUserId, name);
            if (gameData == null)
                return GameDataNotFound(NotFoundDetail);
            return _responseObjectFactory.CreateResponseObject(gameData);
        }

        // POST: api/v1/game-data
        [HttpPost]
        public ActionResult<IResponseObject> PostGameData(GameDataRequestDTO gameDataDTO)
        {
            if (_gameDataRepository.GameDataFileExists(CurrentUserId, gameDataDTO.Name))
            {
                ErrorResponseObject errorResponse = _responseObjectFactory
                    .CreateErrorResponseObject(HttpStatusCode.Conflict, CreationErrorTitle, NameExistsErrorDetail);
                return Conflict(errorResponse);
            }
            GameData gameData = _mapper.Map<GameData>(gameDataDTO);
            _gameDataRepository.Create(CurrentUserId, gameData);
            return CreatedAtAction("GetGameData", new { name = gameDataDTO.Name }, gameData);
        }

        // PUT: api/v1/game-data/name
        [HttpPut("{name}")]
        public ActionResult<IResponseObject> PutGameData(string name, GameDataRequestDTO gameDataDTO)
        {
            if (gameDataDTO.Name != name && _gameDataRepository.GameDataFileExists(CurrentUserId, gameDataDTO.Name))
            {
                ErrorResponseObject errorResponse = _responseObjectFactory
                    .CreateErrorResponseObject(HttpStatusCode.Conflict, UpdateErrorTitle, NameExistsErrorDetail);
                return Conflict(errorResponse);
            }
            if (!_gameDataRepository.GameDataFileExists(CurrentUserId, name))
                return GameDataNotFound(NotFoundForUpdateDetail);
            GameData gameData = _mapper.Map<GameData>(gameDataDTO);
            _gameDataRepository.Update(CurrentUserId, gameData, name);
            return NoContent();
        }

        // DELETE: api/v1/game-data/name
        [HttpDelete("{name}")]
        public ActionResult<IResponseObject> DeleteGameData(string name)
        {
            if (!_gameDataRepository.GameDataFileExists(CurrentUserId, name))
                return GameDataNotFound(NotFoundForDeleteDetail);
            _gameDataRepository.DeleteByUserIdAndName(CurrentUserId, name);
            return NoContent();
        }

        private ActionResult<IResponseObject> GameDataNotFound(string errorDetail)
        {
            return _responseObjectFactory
                .CreateErrorResponseObject(HttpStatusCode.NotFound, NotFoundTitle, errorDetail);
        }
    }
}
