using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(
            IPlatformRepo repo,
            IMapper mapper,
            ICommandDataClient commandDataClient)
        {
            _commandDataClient = commandDataClient;
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platformModels = _repo.GetAllPlatforms();

            var platformReadDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platformModels);

            return Ok(platformReadDtos);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformModel = _repo.GetPlatformById(id);

            if (platformModel is null)
            {
                return NotFound();
            }

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            return Ok(platformReadDto);
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(
            PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(platformModel);
            _repo.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (System.Exception)
            {
                throw;
            }

            return CreatedAtRoute(
                nameof(GetPlatformById),
                new { Id = platformReadDto.Id },
                platformReadDto);
        }
    }
}