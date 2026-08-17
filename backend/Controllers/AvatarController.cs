using Microsoft.AspNetCore.Mvc;
using AvatarNavigator.API.Services;

namespace AvatarNavigator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvatarController : ControllerBase
    {
        private readonly IAvatarService _avatarService;
        private readonly ILogger<AvatarController> _logger;

        public AvatarController(IAvatarService avatarService, ILogger<AvatarController> logger)
        {
            _avatarService = avatarService;
            _logger = logger;
        }

        [HttpGet("live-config")]
        public async Task<IActionResult> GetLiveConfig()
        {
            var config = await _avatarService.GetLiveAvatarConfigAsync();
            return Ok(config);
        }

        [HttpPost("voice-command")]
        public async Task<ActionResult<string>> ProcessVoiceCommand([FromForm] IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
                return BadRequest("No audio file provided.");

            var tempPath = Path.Combine(Path.GetTempPath(), audioFile.FileName);
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }

            try
            {
                var result = await _avatarService.ProcessVoiceCommandAsync(tempPath);
                return Ok(new { command = result });
            }
            finally
            {
                if (System.IO.File.Exists(tempPath))
                    System.IO.File.Delete(tempPath);
            }
        }

        [HttpPost("synthesize")]
        public async Task<ActionResult<string>> SynthesizeSpeech([FromBody] SpeechRequest request)
        {
            if (string.IsNullOrEmpty(request.Text))
                return BadRequest("Text is required.");

            var result = await _avatarService.SynthesizeSpeechAsync(request.Text);
            return Ok(new { result });
        }

        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            var liveConfig = await _avatarService.GetLiveAvatarConfigAsync();
            return Ok(new
            {
                status = "Avatar service is running",
                liveAvatarConfigured = liveConfig.Enabled,
                avatarName = liveConfig.AvatarName,
                voiceName = liveConfig.VoiceName,
                message = liveConfig.Message
            });
        }
    }

    public class SpeechRequest
    {
        public string Text { get; set; } = string.Empty;
    }
}
