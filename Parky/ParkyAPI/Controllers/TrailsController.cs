using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Models.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private ITrailRepository _np;
        private ITrailRepository _tr;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository np, IMapper mapper, ITrailRepository tr)
        {
            _np = np;
            _mapper = mapper;
            _tr = tr;
        }

        /// <summary>
        /// Get list of trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var Trails = _tr.GetTrails();
            var Trailsdto = new List<TrailDto>();

            foreach (var Trail in Trails)
            {
                Trailsdto.Add(_mapper.Map<TrailDto>(Trail));
            }

            return Ok(Trailsdto);
        }
        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="TrailId"> The id of the trail</param>
        /// <returns></returns>
        [HttpGet("{TrailId:int}", Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int TrailId)
        {
            var Trail = _tr.GetTrail(TrailId);
            if (Trail == null) return NotFound();
            var Trailsdto = _mapper.Map<TrailDto>(Trail);
            return Ok(Trailsdto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailCreateDto TrailDto)
        {
            if (TrailDto == null) return BadRequest(ModelState);

            if (_tr.TrailExists(TrailDto.Name))
            {
                ModelState.AddModelError(string.Empty, "Trail already exists");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var TrailObj = _mapper.Map<Trail>(TrailDto);
            if (!_tr.CreateTrail(TrailObj))
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong when saving the record {TrailObj.Name}");
                return StatusCode(400, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { TrailId = TrailObj.Id }, TrailObj);

        }

        [HttpPatch("{TrailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]

        public IActionResult UpdateTrail(int TrailId, [FromBody] TrailUpdateDto TrailDto)
        {
            if (TrailDto == null || TrailId != TrailDto.Id) return BadRequest(ModelState);

            var TrailObj = _mapper.Map<Trail>(TrailDto);
            if (!_tr.UpdateTrail(TrailObj))
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong when updating the record {TrailObj.Name}");
                return StatusCode(400, ModelState);
            }
            return NoContent();

        }

        [HttpDelete("{TrailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int TrailId)
        {
            if (!_tr.TrailExists(TrailId)) return NotFound();

            var TrailObj = _tr.GetTrail(TrailId);
            if (!_tr.DeleteTrail(TrailObj))
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong when deleting the record {TrailObj.Name}");
                return StatusCode(400, ModelState);
            }
            return NoContent();

        }

        [HttpGet("GetTrailInNationalPark/{NationalParkId:int}", Name = "GetTrailsInNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailsInNationalPark(int nationalParkId)
        {
            var objList = _tr.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }

    }
}
