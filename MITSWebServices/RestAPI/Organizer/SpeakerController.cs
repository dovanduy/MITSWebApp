using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;
using Newtonsoft.Json.Linq;

namespace MITSWebServices.Controllers
{

    [Route("api/[controller]")]
    public class SpeakerController : ControllerBase
    {
        private readonly IOrganizerRepo _evRepo;
        private readonly ILogger<SpeakerController> _logger;

        public SpeakerController(IOrganizerRepo evRepo, ILogger<SpeakerController> logger)
        {
            _evRepo = evRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var speakers = await _evRepo.GetSpeakers();

            if (!speakers.Any())
            {
                return NotFound("No Speakers were found");
            }
            else
            {
                return Ok(speakers);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var speaker = await _evRepo.GetSpeakers(id);

            if (speaker == null)
            {
                return NotFound("Speaker not Found");
            }
            else
            {
                return Ok(speaker);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Speaker speaker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<Speaker> { Status = false, ModelState = ModelState });
            }

            try
            {
                var newSpeaker = await _evRepo.InsertSpeaker(speaker);
                if (newSpeaker == null)
                {
                    return BadRequest(new ApiResponse<Speaker> { Status = false });
                }

                return CreatedAtAction("speaker", new { id = speaker.Id },
                        new ApiResponse<Speaker> { Status = true, Model = speaker });
            }

            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(new ApiResponse<Speaker> { Status = false });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Speaker speakerWithUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<Speaker> { Status = false, ModelState = ModelState });
            }

            try
            {
                var status = await _evRepo.UpdateSpeaker(id, speakerWithUpdate);
                if (!status)
                {
                    return BadRequest(new ApiResponse<Speaker> { Status = false });
                }
                return Ok(new ApiResponse<Speaker> { Status = true, Model = speakerWithUpdate });
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(new ApiResponse<Speaker> { Status = false });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var status = await _evRepo.DeleteSpeaker(id);
                if (!status)
                {
                    return BadRequest(new ApiResponse<Speaker> { Status = false });
                }
                return Ok(new ApiResponse<Speaker> { Status = true });
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message);
                return BadRequest(new ApiResponse<Speaker> { Status = false });
            }
        }
    }
}