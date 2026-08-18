using Microsoft.AspNetCore.Mvc;
using AvatarNavigator.API.Services;

namespace AvatarNavigator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvatarController : ControllerBase
    {
        private readonly IAvatarService _avatarService;
        private readonly IAgentService _agentService;
        private readonly ILogger<AvatarController> _logger;

        public AvatarController(IAvatarService avatarService, IAgentService agentService, ILogger<AvatarController> logger)
        {
            _avatarService = avatarService;
            _agentService = agentService;
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

        [HttpGet("avatars")]
        public async Task<ActionResult<List<AvatarInfo>>> GetAvatarList()
        {
            var avatars = await _agentService.GetAvatarListAsync();
            return Ok(avatars);
        }

        [HttpPost("agent/process-request")]
        public async Task<ActionResult<AgentResponse>> ProcessAgentRequest([FromBody] AgentRequest request)
        {
            if (string.IsNullOrEmpty(request.UserRequest))
                return BadRequest("User request is required.");

            var response = await _agentService.ProcessUserRequestAsync(request.UserRequest, request.SelectedAvatarId);
            return Ok(response);
        }

        [HttpGet("agent/config")]
        public async Task<ActionResult<AgentConfig>> GetAgentConfig()
        {
            var isConfigured = await _agentService.IsConfiguredAsync();
            if (!isConfigured)
                return NotFound(new { message = "Agent is not configured" });

            var config = await _agentService.GetConfigAsync();
            return Ok(config);
        }

        [HttpGet("health")]
        public async Task<IActionResult> HealthCheck()
        {
            var liveConfig = await _avatarService.GetLiveAvatarConfigAsync();
            var agentConfigured = await _agentService.IsConfiguredAsync();
            var avatars = await _agentService.GetAvatarListAsync();

            return Ok(new
            {
                status = "Avatar service is running",
                liveAvatarConfigured = liveConfig.Enabled,
                speechConfigured = liveConfig.SpeechConfigured,
                liveAvatarEndpointConfigured = liveConfig.LiveAvatarConfigured,
                avatarName = liveConfig.AvatarName,
                voiceName = liveConfig.VoiceName,
                message = liveConfig.Message,
                warning = liveConfig.Warning,
                agentConfigured = agentConfigured,
                availableAvatars = avatars.Count
            });
        }
    }

    public class SpeechRequest
    {
        public string Text { get; set; } = string.Empty;
    }

    public class AgentRequest
    {
        public string UserRequest { get; set; } = string.Empty;
        public string? SelectedAvatarId { get; set; }
    }
}
