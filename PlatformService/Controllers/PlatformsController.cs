using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        public PlatformsController(IPlatformRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platformItems = _repo.GetAllPlatforms();

            var results = _mapper.Map<IEnumerable<PlatformReadDto>>(platformItems);

            return Ok(results);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repo.GetPlatformById(id);

            if (platformItem is null)
            {
                return NotFound();
            }

            var result = _mapper.Map<PlatformReadDto>(platformItem);

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto createDto)
        {
            var itemToCreate = _mapper.Map<Platform>(createDto);
            _repo.CreatePlatform(itemToCreate);
            _repo.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(itemToCreate);

            return CreatedAtRoute(
                nameof(GetPlatformById),
                new { Id = platformReadDto.Id },
                platformReadDto);
        }
    }
}