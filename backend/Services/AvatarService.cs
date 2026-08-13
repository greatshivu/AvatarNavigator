using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace AvatarNavigator.API.Services
{
    public interface IAvatarService
    {
        Task<string> ProcessVoiceCommandAsync(string audioPath);
        Task<string> SynthesizeSpeechAsync(string text);
    }

    public class AvatarService : IAvatarService
    {
        private readonly IConfiguration _configuration;

        public AvatarService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> ProcessVoiceCommandAsync(string audioPath)
        {
            try
            {
                var config = SpeechConfig.FromSubscription(
                    _configuration["AzureAvatar:SubscriptionKey"],
                    _configuration["AzureAvatar:Region"]
                );
                config.SpeechRecognitionLanguage = "en-US";

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
                        return "Error processing audio.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> SynthesizeSpeechAsync(string text)
        {
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
