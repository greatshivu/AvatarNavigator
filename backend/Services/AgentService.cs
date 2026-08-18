using System.Text;
using System.Text.Json;

namespace AvatarNavigator.API.Services
{
    /// <summary>
    /// Represents an available avatar from Azure Live Avatar List
    /// </summary>
    public class AvatarInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string AvatarType { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? PreviewUrl { get; set; }
    }

    /// <summary>
    /// Agent response with thinking process and navigation options
    /// </summary>
    public class AgentResponse
    {
        public string UserRequest { get; set; } = string.Empty;
        public string AiThinking { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public List<NavigationOption> NavigationOptions { get; set; } = new();
        public string SelectedAvatarId { get; set; } = "Lisa";
        public string SelectedAvatarName { get; set; } = "Lisa";
        public string? AudioStreamUrl { get; set; }
        public bool RequiresUserConfirmation { get; set; } = false;
    }

    /// <summary>
    /// Navigation option suggested by the agent
    /// </summary>
    public class NavigationOption
    {
        public string Id { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public Dictionary<string, string>? Parameters { get; set; }
    }

    /// <summary>
    /// Agent configuration
    /// </summary>
    public class AgentConfig
    {
        public bool Enabled { get; set; }
        public string? Endpoint { get; set; }
        public string? ApiKey { get; set; }
        public string? ModelDeploymentName { get; set; }
        public string? AvatarListEndpoint { get; set; }
    }

    public interface IAgentService
    {
        Task<List<AvatarInfo>> GetAvatarListAsync();
        Task<AgentResponse> ProcessUserRequestAsync(string userRequest, string? selectedAvatarId = null);
        Task<bool> IsConfiguredAsync();
        Task<AgentConfig> GetConfigAsync();
    }

    public class AgentService : IAgentService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AgentService> _logger;
        private readonly IAvatarService _avatarService;

        public AgentService(IConfiguration configuration, HttpClient httpClient, ILogger<AgentService> logger, IAvatarService avatarService)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _logger = logger;
            _avatarService = avatarService;
        }

        public async Task<bool> IsConfiguredAsync()
        {
            var enabled = _configuration.GetValue<bool>("AzureFoundryAgent:Enabled");
            var endpoint = _configuration["AzureFoundryAgent:Endpoint"];
            var apiKey = _configuration["AzureFoundryAgent:ApiKey"];

            return enabled && !string.IsNullOrWhiteSpace(endpoint) && !string.IsNullOrWhiteSpace(apiKey);
        }

        public async Task<AgentConfig> GetConfigAsync()
        {
            return new AgentConfig
            {
                Enabled = _configuration.GetValue<bool>("AzureFoundryAgent:Enabled"),
                Endpoint = _configuration["AzureFoundryAgent:Endpoint"],
                ApiKey = _configuration["AzureFoundryAgent:ApiKey"],
                ModelDeploymentName = _configuration["AzureFoundryAgent:ModelDeploymentName"],
                AvatarListEndpoint = _configuration["AzureFoundryAgent:AvatarListEndpoint"]
            };
        }

        public async Task<List<AvatarInfo>> GetAvatarListAsync()
        {
            try
            {
                if (!await IsConfiguredAsync())
                {
                    _logger.LogWarning("Azure Foundry Agent is not configured.");
                    return GetDefaultAvatars();
                }

                var config = await GetConfigAsync();
                
                // If AvatarListEndpoint is configured, fetch from Azure
                if (!string.IsNullOrWhiteSpace(config.AvatarListEndpoint))
                {
                    return await FetchAvatarsFromAzureAsync(config.AvatarListEndpoint, config.ApiKey!);
                }

                return GetDefaultAvatars();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching avatar list: {ex.Message}");
                return GetDefaultAvatars();
            }
        }

        private async Task<List<AvatarInfo>> FetchAvatarsFromAzureAsync(string endpoint, string apiKey)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);
                    
                    var response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var avatars = JsonSerializer.Deserialize<List<AvatarInfo>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        return avatars ?? GetDefaultAvatars();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling Azure Avatar List endpoint: {ex.Message}");
            }

            return GetDefaultAvatars();
        }

        private List<AvatarInfo> GetDefaultAvatars()
        {
            return new List<AvatarInfo>
            {
                new AvatarInfo
                {
                    Id = "Lisa",
                    Name = "Lisa",
                    DisplayName = "Lisa (English)",
                    AvatarType = "Neural",
                    PreviewUrl = "https://aka.ms/azure-avatar-lisa"
                },
                new AvatarInfo
                {
                    Id = "Grace",
                    Name = "Grace",
                    DisplayName = "Grace (English)",
                    AvatarType = "Neural",
                    PreviewUrl = "https://aka.ms/azure-avatar-grace"
                },
                new AvatarInfo
                {
                    Id = "Eric",
                    Name = "Eric",
                    DisplayName = "Eric (English)",
                    AvatarType = "Neural",
                    PreviewUrl = "https://aka.ms/azure-avatar-eric"
                }
            };
        }

        public async Task<AgentResponse> ProcessUserRequestAsync(string userRequest, string? selectedAvatarId = null)
        {
            try
            {
                if (!await IsConfiguredAsync())
                {
                    return new AgentResponse
                    {
                        UserRequest = userRequest,
                        AiThinking = "Agent not configured",
                        Response = "Azure Foundry Agent is not configured. Please add the required configuration to appsettings.json",
                        NavigationOptions = GetDefaultNavigationOptions()
                    };
                }

                // Use Azure OpenAI if available for AI thinking, otherwise provide simulated response
                var response = await GenerateAgentResponseAsync(userRequest, selectedAvatarId);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing user request: {ex.Message}");
                return new AgentResponse
                {
                    UserRequest = userRequest,
                    AiThinking = "Error occurred",
                    Response = $"An error occurred while processing your request: {ex.Message}",
                    NavigationOptions = GetDefaultNavigationOptions()
                };
            }
        }

        private async Task<AgentResponse> GenerateAgentResponseAsync(string userRequest, string? selectedAvatarId)
        {
            // Check if Azure OpenAI is configured for AI thinking
            var useOpenAI = _configuration.GetValue<bool>("AzureOpenAI:Enabled");
            
            if (useOpenAI)
            {
                return await GenerateResponseWithOpenAIAsync(userRequest, selectedAvatarId);
            }

            // Fallback: provide intelligent response based on keyword analysis
            return GenerateSimulatedAgentResponse(userRequest, selectedAvatarId);
        }

        private async Task<AgentResponse> GenerateResponseWithOpenAIAsync(string userRequest, string? selectedAvatarId)
        {
            try
            {
                var endpoint = _configuration["AzureOpenAI:Endpoint"];
                var apiKey = _configuration["AzureOpenAI:ApiKey"];
                var deploymentName = _configuration["AzureOpenAI:DeploymentName"];
                var apiVersion = _configuration["AzureOpenAI:ApiVersion"];

                if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(apiKey))
                {
                    _logger.LogWarning("Azure OpenAI not properly configured");
                    return GenerateSimulatedAgentResponse(userRequest, selectedAvatarId);
                }

                var url = $"{endpoint}openai/deployments/{deploymentName}/chat/completions?api-version={apiVersion}";

                var messages = new object[]
                {
                    new { role = "system", content = "You are a helpful AI assistant integrated with an Azure Live Avatar. Provide responses and suggest navigation options. Format your thinking process and navigation options clearly." },
                    new { role = "user", content = userRequest }
                };

                var requestBody = new
                {
                    messages = messages,
                    temperature = 0.7,
                    max_tokens = 1000
                };

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Headers.Add("api-key", apiKey);
                    request.Content = new StringContent(
                        JsonSerializer.Serialize(requestBody),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<JsonElement>(content);

                        var messageContent = result.GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString() ?? "";

                        return ParseAgentResponse(userRequest, messageContent, selectedAvatarId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling Azure OpenAI: {ex.Message}");
            }

            return GenerateSimulatedAgentResponse(userRequest, selectedAvatarId);
        }

        private AgentResponse GenerateSimulatedAgentResponse(string userRequest, string? selectedAvatarId)
        {
            var aiThinking = AnalyzeUserRequest(userRequest);
            var navigationOptions = DetermineNavigationOptions(userRequest);
            var response = GenerateNaturalResponse(userRequest, navigationOptions);

            return new AgentResponse
            {
                UserRequest = userRequest,
                AiThinking = aiThinking,
                Response = response,
                NavigationOptions = navigationOptions,
                SelectedAvatarId = selectedAvatarId ?? "Lisa",
                SelectedAvatarName = selectedAvatarId ?? "Lisa",
                RequiresUserConfirmation = navigationOptions.Count > 1
            };
        }

        private AgentResponse ParseAgentResponse(string userRequest, string messageContent, string? selectedAvatarId)
        {
            var navigationOptions = ExtractNavigationOptions(messageContent);

            return new AgentResponse
            {
                UserRequest = userRequest,
                AiThinking = ExtractThinkingProcess(messageContent),
                Response = messageContent,
                NavigationOptions = navigationOptions,
                SelectedAvatarId = selectedAvatarId ?? "Lisa",
                SelectedAvatarName = selectedAvatarId ?? "Lisa",
                RequiresUserConfirmation = navigationOptions.Count > 1
            };
        }

        private string AnalyzeUserRequest(string userRequest)
        {
            var lowercaseRequest = userRequest.ToLower();

            if (lowercaseRequest.Contains("order") || lowercaseRequest.Contains("purchase"))
                return "User is interested in placing or viewing orders. I should provide order-related options.";

            if (lowercaseRequest.Contains("product") || lowercaseRequest.Contains("item") || lowercaseRequest.Contains("inventory"))
                return "User wants to explore products or inventory. I should show available items.";

            if (lowercaseRequest.Contains("navigation") || lowercaseRequest.Contains("help") || lowercaseRequest.Contains("guide"))
                return "User needs guidance. I should provide helpful navigation options.";

            if (lowercaseRequest.Contains("avatar") || lowercaseRequest.Contains("change"))
                return "User wants to switch avatars or modify appearance settings.";

            return "User request analyzed. Generating appropriate response and navigation options.";
        }

        private List<NavigationOption> DetermineNavigationOptions(string userRequest)
        {
            var options = new List<NavigationOption>();
            var lowercaseRequest = userRequest.ToLower();

            // Default navigation options
            options.Add(new NavigationOption
            {
                Id = "view-items",
                Label = "View Items",
                Description = "Browse available products",
                Action = "navigate",
                Parameters = new Dictionary<string, string> { { "route", "/items" } }
            });

            options.Add(new NavigationOption
            {
                Id = "view-orders",
                Label = "View Orders",
                Description = "Check your orders",
                Action = "navigate",
                Parameters = new Dictionary<string, string> { { "route", "/orders" } }
            });

            options.Add(new NavigationOption
            {
                Id = "change-avatar",
                Label = "Change Avatar",
                Description = "Switch to a different avatar",
                Action = "changeAvatar",
                Parameters = new Dictionary<string, string> { { "showAvatarList", "true" } }
            });

            // Context-based options
            if (lowercaseRequest.Contains("help"))
            {
                options.Add(new NavigationOption
                {
                    Id = "show-guide",
                    Label = "Show Guide",
                    Description = "Display application guide",
                    Action = "showGuide"
                });
            }

            return options;
        }

        private string GenerateNaturalResponse(string userRequest, List<NavigationOption> options)
        {
            var lowercaseRequest = userRequest.ToLower();

            if (lowercaseRequest.Contains("hello") || lowercaseRequest.Contains("hi"))
                return "Hello! I'm your AI assistant powered by Azure Live Avatar. How can I help you today? Would you like to view items, check orders, or change your avatar?";

            if (lowercaseRequest.Contains("order"))
                return "I can help you with your orders. Would you like to view your current orders or place a new one?";

            if (lowercaseRequest.Contains("product") || lowercaseRequest.Contains("item"))
                return "Let me show you our available products. You can browse and learn more about each item.";

            if (lowercaseRequest.Contains("avatar"))
                return "I can help you change to a different avatar. Would you like to see the available options?";

            return $"I understand you're asking about: \"{userRequest}\". Here are some options that might help:";
        }

        private List<NavigationOption> ExtractNavigationOptions(string responseText)
        {
            // Parse navigation options from AI response
            var options = new List<NavigationOption>();

            if (responseText.ToLower().Contains("item") || responseText.ToLower().Contains("product"))
            {
                options.Add(new NavigationOption
                {
                    Id = "view-items",
                    Label = "View Items",
                    Description = "Browse available products",
                    Action = "navigate"
                });
            }

            if (responseText.ToLower().Contains("order"))
            {
                options.Add(new NavigationOption
                {
                    Id = "view-orders",
                    Label = "View Orders",
                    Description = "Check your orders",
                    Action = "navigate"
                });
            }

            return options.Count > 0 ? options : GetDefaultNavigationOptions();
        }

        private string ExtractThinkingProcess(string responseText)
        {
            // Extract thinking process from AI response
            var lines = responseText.Split('\n');
            return string.Join(" ", lines.Take(2));
        }

        private List<NavigationOption> GetDefaultNavigationOptions()
        {
            return new List<NavigationOption>
            {
                new NavigationOption
                {
                    Id = "view-items",
                    Label = "View Items",
                    Description = "Browse available products",
                    Action = "navigate",
                    Parameters = new Dictionary<string, string> { { "route", "/items" } }
                },
                new NavigationOption
                {
                    Id = "view-orders",
                    Label = "View Orders",
                    Description = "Check your orders",
                    Action = "navigate",
                    Parameters = new Dictionary<string, string> { { "route", "/orders" } }
                },
                new NavigationOption
                {
                    Id = "change-avatar",
                    Label = "Change Avatar",
                    Description = "Switch to a different avatar",
                    Action = "changeAvatar",
                    Parameters = new Dictionary<string, string> { { "showAvatarList", "true" } }
                }
            };
        }
    }
}
