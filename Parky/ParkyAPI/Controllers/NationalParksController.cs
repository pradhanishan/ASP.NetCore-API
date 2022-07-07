using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Models.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _np;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository np, IMapper mapper)
        {
            _np = np;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _np.GetNationalParks();
            var nationalParksdto = new List<NationalParkDto>();

            foreach (var nationalPark in nationalParks)
            {
                nationalParksdto.Add(_mapper.Map<NationalParkDto>(nationalPark));
            }

            return Ok(nationalParksdto);
        }
        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId"> The id of the national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _np.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound();
            var nationalParksdto = _mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalParksdto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(ModelState);

            if (_np.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError(string.Empty, "National Park already exists");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_np.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(400, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { version = HttpContext.GetRequestedApiVersion().ToString(), nationalParkId = nationalParkObj.Id }, nationalParkObj);

        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]

        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id) return BadRequest(ModelState);

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_np.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong when updating the record {nationalParkObj.Name}");
                return StatusCode(400, ModelState);
            }
            return NoContent();

        }

        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_np.NationalParkExists(nationalParkId)) return NotFound();

            var nationalParkObj = _np.GetNationalPark(nationalParkId);
            if (!_np.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError(string.Empty, $"Something went wrong when deleting the record {nationalParkObj.Name}");
                return StatusCode(400, ModelState);
            }
            return NoContent();

        }

    }
}
