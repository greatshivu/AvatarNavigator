using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace AvatarNavigator.API.Services
{
    public class LiveAvatarSettings
    {
        public bool Enabled { get; set; }
        public bool SpeechConfigured { get; set; }
        public bool LiveAvatarConfigured { get; set; }
        public string AvatarName { get; set; } = "Lisa";
        public string VoiceName { get; set; } = "en-US-JennyNeural";
        public string Region { get; set; } = "eastus";
        public string? LiveAvatarEndpoint { get; set; }
        public string? LiveAvatarVideoUrl { get; set; }
        public string? Message { get; set; }
        public string? Warning { get; set; }
    }

    public interface IAvatarService
    {
        Task<string> ProcessVoiceCommandAsync(string audioPath);
        Task<string> SynthesizeSpeechAsync(string text);
        Task<LiveAvatarSettings> GetLiveAvatarConfigAsync();
    }

    public class AvatarService : IAvatarService
    {
        private readonly IConfiguration _configuration;

        public AvatarService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<LiveAvatarSettings> GetLiveAvatarConfigAsync()
        {
            var speechConfigured = !string.IsNullOrWhiteSpace(_configuration["AzureAvatar:SubscriptionKey"]) &&
                                   !string.IsNullOrWhiteSpace(_configuration["AzureAvatar:Region"]);

            var endpointConfigured = !string.IsNullOrWhiteSpace(_configuration["AzureAvatar:LiveAvatarEndpoint"]);
            var videoConfigured = !string.IsNullOrWhiteSpace(_configuration["AzureAvatar:LiveAvatarVideoUrl"]);
            var useLiveAvatar = _configuration.GetValue<bool>("AzureAvatar:UseLiveAvatar");

            var enabled = useLiveAvatar || endpointConfigured || videoConfigured;

            var warningParts = new List<string>();
            if (!speechConfigured)
            {
                warningParts.Add("Speech key/region are missing.");
            }
            if (!endpointConfigured)
            {
                warningParts.Add("LiveAvatarEndpoint is missing.");
            }
            if (!videoConfigured)
            {
                warningParts.Add("LiveAvatarVideoUrl is missing.");
            }

            var settings = new LiveAvatarSettings
            {
                Enabled = enabled,
                SpeechConfigured = speechConfigured,
                LiveAvatarConfigured = endpointConfigured || videoConfigured || useLiveAvatar,
                AvatarName = _configuration["AzureAvatar:AvatarName"] ?? "Lisa",
                VoiceName = _configuration["AzureAvatar:VoiceName"] ?? "en-US-JennyNeural",
                Region = _configuration["AzureAvatar:Region"] ?? "eastus",
                LiveAvatarEndpoint = _configuration["AzureAvatar:LiveAvatarEndpoint"],
                LiveAvatarVideoUrl = _configuration["AzureAvatar:LiveAvatarVideoUrl"],
                Message = enabled
                    ? "Live Azure Avatar is configured for Lisa."
                    : "Azure Speech Live Avatar is not configured yet. Add the Live Avatar endpoint and stream URL in appsettings.json.",
                Warning = warningParts.Count > 0 ? string.Join(" ", warningParts) : null
            };

            return Task.FromResult(settings);
        }

        private bool HasSpeechCredentials() =>
            !string.IsNullOrWhiteSpace(_configuration["AzureAvatar:SubscriptionKey"]) &&
            !string.IsNullOrWhiteSpace(_configuration["AzureAvatar:Region"]);

        public async Task<string> ProcessVoiceCommandAsync(string audioPath)
        {
            if (!HasSpeechCredentials())
            {
                return "Azure Speech credentials are not configured. Add the Azure Speech subscription key and region before using live voice recognition.";
            }

            if (!System.IO.File.Exists(audioPath))
            {
                return "Audio file was not created correctly.";
            }

            try
            {
                var config = SpeechConfig.FromSubscription(
                    _configuration["AzureAvatar:SubscriptionKey"],
                    _configuration["AzureAvatar:Region"]
                );
                config.SpeechRecognitionLanguage = "en-US";

                // Use a real WAV file. The browser must send PCM WAV data; otherwise Azure Speech may reject the header.
                using (var audioConfig = AudioConfig.FromWavFileInput(audioPath))
                using (var recognizer = new SpeechRecognizer(config, audioConfig))
                {
                    var result = await recognizer.RecognizeOnceAsync();

                    if (result.Reason == ResultReason.RecognizedSpeech)
                    {
                        return result.Text;
                    }
                    else if (result.Reason == ResultReason.NoMatch)
                    {
                        return "Speech not recognized.";
                    }
                    else
                    {
                        return $"Speech recognition failed: {result.Reason}. Check the audio format and Azure Speech configuration.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}. This usually means invalid WAV audio or incorrect Azure Speech credentials.";
            }
        }

        public async Task<string> SynthesizeSpeechAsync(string text)
        {
            if (!HasSpeechCredentials())
            {
                return "Azure Speech credentials are not configured. Add the Azure Speech subscription key and region before using voice synthesis.";
            }

            try
            {
                var config = SpeechConfig.FromSubscription(
                    _configuration["AzureAvatar:SubscriptionKey"],
                    _configuration["AzureAvatar:Region"]
                );
                config.SpeechSynthesisLanguage = "en-US";

                using (var audioConfig = AudioConfig.FromDefaultSpeakerOutput())
                using (var synthesizer = new SpeechSynthesizer(config, audioConfig))
                {
                    var result = await synthesizer.SpeakTextAsync(text);

                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        return "Speech synthesized successfully.";
                    }
                    else
                    {
                        return "Error synthesizing speech.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
