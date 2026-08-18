# Azure Foundry Agent & Avatar Integration Guide

## Overview
This document explains how to configure and use the new Azure Foundry Agent integration with Azure Live Avatar List and AI thinking capabilities.

## Features Added

### 1. **Azure Foundry Agent Service**
- Integrates with Azure Foundry Agent for intelligent request processing
- Supports multiple avatars from Azure Live Avatar List
- Provides AI thinking process visualization
- Generates context-aware navigation options

### 2. **Multiple Avatar Support**
- Fetch avatar list from Azure Live Avatar service
- Switch between different avatars dynamically
- Each avatar can have different voice characteristics
- Default avatars: Lisa, Grace, Eric

### 3. **AI Thinking Ability**
- Analyzes user requests using AI/reasoning
- Provides visible thinking process
- Suggests appropriate navigation options
- Integrates with Azure OpenAI for enhanced responses

### 4. **Intelligent Navigation**
- Suggests actions based on user requests
- Provides confirmation when multiple options available
- Executes navigation actions (routing, avatar changes, etc.)

## Configuration Setup

### Step 1: Update `appsettings.json`

Add the following sections to your `appsettings.json`:

```json
{
  "AzureAvatar": {
    "SubscriptionKey": "your-speech-key-here",
    "Region": "eastus",
    "UseLiveAvatar": false,
    "AvatarName": "Lisa",
    "VoiceName": "en-US-JennyNeural",
    "LiveAvatarEndpoint": "https://your-region.tts.speech.microsoft.com/cognitiveservices/v1",
    "LiveAvatarVideoUrl": "https://your-live-avatar-stream-url"
  },
  "AzureFoundryAgent": {
    "Enabled": false,
    "Endpoint": "https://your-foundry-endpoint.cognitiveservices.azure.com/",
    "ApiKey": "your-foundry-api-key-here",
    "ModelDeploymentName": "your-agent-deployment-name",
    "AvatarListEndpoint": "https://your-avatar-list-endpoint.azurewebsites.net/api/avatars"
  },
  "AzureOpenAI": {
    "Enabled": false,
    "Endpoint": "https://your-openai-resource.openai.azure.com/",
    "ApiKey": "your-openai-api-key-here",
    "DeploymentName": "your-gpt-deployment-name",
    "ApiVersion": "2024-02-15-preview"
  }
}
```

### Step 2: Get Your Azure Credentials

#### For Azure Speech Services (Existing)
1. Go to [Azure Portal](https://portal.azure.com)
2. Create or select a "Speech" resource
3. Copy the subscription key and region from "Keys and Endpoint"

#### For Azure Foundry Agent
1. Create an "Azure Foundry" resource in Azure Portal
2. Copy the endpoint and API key
3. Note the Model Deployment Name
4. (Optional) Configure the Avatar List Endpoint to fetch avatars from Azure

#### For Azure Live Avatar Service
1. In the Speech resource, enable "Live Avatar" feature
2. Get the Live Avatar endpoint from "Keys and Endpoint"
3. Configure the video stream URL (or use Azure's streaming service)

#### For Azure OpenAI (Optional - for Enhanced AI Thinking)
1. Create an "Azure OpenAI" resource
2. Deploy a GPT model (e.g., gpt-4 or gpt-35-turbo)
3. Copy the endpoint, API key, and deployment name

### Step 3: Enable Features

Enable each feature by setting its `Enabled` flag to `true`:

```json
{
  "AzureFoundryAgent": {
    "Enabled": true,
    "...": "..."
  },
  "AzureOpenAI": {
    "Enabled": true,
    "...": "..."
  }
}
```

## API Endpoints

### Get Avatar List
**Endpoint:** `GET /api/avatar/avatars`

**Response:**
```json
[
  {
    "id": "Lisa",
    "name": "Lisa",
    "displayName": "Lisa (English)",
    "avatarType": "Neural",
    "previewUrl": "https://..."
  },
  ...
]
```

### Process User Request with Agent
**Endpoint:** `POST /api/avatar/agent/process-request`

**Request Body:**
```json
{
  "userRequest": "What products do you have?",
  "selectedAvatarId": "Lisa"
}
```

**Response:**
```json
{
  "userRequest": "What products do you have?",
  "aiThinking": "User wants to explore products. I should show available items.",
  "response": "I can help you browse our available products...",
  "navigationOptions": [
    {
      "id": "view-items",
      "label": "View Items",
      "description": "Browse available products",
      "action": "navigate",
      "parameters": {
        "route": "/items"
      }
    }
  ],
  "selectedAvatarId": "Lisa",
  "selectedAvatarName": "Lisa",
  "requiresUserConfirmation": false
}
```

### Get Agent Configuration
**Endpoint:** `GET /api/avatar/agent/config`

**Response:**
```json
{
  "enabled": true,
  "endpoint": "https://...",
  "modelDeploymentName": "...",
  "avatarListEndpoint": "..."
}
```

### Health Check (Enhanced)
**Endpoint:** `GET /api/avatar/health`

Now includes agent configuration status and available avatars count.

## Usage Flow

### Basic User Interaction Flow

```
1. User sends request (text/voice)
   ↓
2. Backend receives request at /api/avatar/agent/process-request
   ↓
3. AgentService analyzes request (AI thinking)
   ↓
4. Optional: Calls Azure OpenAI for enhanced response
   ↓
5. Determines appropriate avatar (if specified)
   ↓
6. Generates response with navigation options
   ↓
7. Frontend displays avatar, response, and navigation choices
   ↓
8. User selects action/avatar
   ↓
9. Frontend navigates or changes avatar
```

## Frontend Integration

### 1. Add Avatar Selection Component
```typescript
// Get available avatars
async getAvatars() {
  const response = await fetch('/api/avatar/avatars');
  this.avatars = await response.json();
}
```

### 2. Send User Request to Agent
```typescript
async sendUserRequest(request: string) {
  const response = await fetch('/api/avatar/agent/process-request', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      userRequest: request,
      selectedAvatarId: this.selectedAvatarId
    })
  });
  
  const agentResponse = await response.json();
  this.displayResponse(agentResponse);
}
```

### 3. Display Navigation Options
```typescript
displayResponse(agentResponse: AgentResponse) {
  // Show avatar
  this.currentAvatar = agentResponse.selectedAvatarName;
  
  // Show thinking process
  console.log('AI Thinking:', agentResponse.aiThinking);
  
  // Display response
  this.synthesizeSpeech(agentResponse.response);
  
  // Show navigation options
  this.navigationOptions = agentResponse.navigationOptions;
}
```

## Testing

### 1. Test Avatar List Endpoint
```bash
curl http://localhost:5000/api/avatar/avatars
```

### 2. Test Agent Processing
```bash
curl -X POST http://localhost:5000/api/avatar/agent/process-request \
  -H "Content-Type: application/json" \
  -d '{
    "userRequest": "Show me your products",
    "selectedAvatarId": "Lisa"
  }'
```

### 3. Test Health Check
```bash
curl http://localhost:5000/api/avatar/health
```

## Troubleshooting

### Agent Not Processing Requests
- Check if Azure Foundry Agent is enabled in `appsettings.json`
- Verify API key and endpoint are correct
- Check application logs for specific error messages

### Avatar List Empty
- If custom Azure Avatar List endpoint is configured, verify it's accessible
- Check the API key for Azure Avatar List service
- Fall back to default avatars if custom endpoint fails

### AI Thinking Not Showing
- If Azure OpenAI is not configured, simulated responses are used
- Enable Azure OpenAI and provide valid credentials for enhanced AI
- Check logs for API call errors

### Navigation Options Not Generated
- Verify agent is processing requests (check AI thinking)
- Check if keyword matching is identifying request type correctly
- Review navigation option mapping in `AgentService.DetermineNavigationOptions()`

## Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Frontend (Angular)                    │
│  - Avatar Selection, User Input, Response Display        │
└────────────────────┬────────────────────────────────────┘
                     │ HTTP API Calls
┌────────────────────▼────────────────────────────────────┐
│             AvatarController                             │
│  - Routes requests to appropriate services               │
│  - Manages endpoints for avatar & agent operations       │
└───────┬──────────────────────┬──────────────────────────┘
        │                      │
        │                      ▼
        │            ┌─────────────────────┐
        │            │   AgentService      │
        │            │ - Process requests  │
        │            │ - Get avatars       │
        │            │ - AI thinking       │
        │            │ - Nav options       │
        │            └────────┬───────┬────┘
        │                     │       │
        │                     ▼       ▼
        ▼            ┌──────────────────────────────┐
    ┌────────────┐  │  Azure Services              │
    │Avatar      │  │ - OpenAI (Enhanced AI)      │
    │Service     │  │ - Foundry Agent             │
    │- Speech    │  │ - Live Avatar List          │
    │- Synthesis │  │ - Speech Services           │
    └────────────┘  └──────────────────────────────┘
```

## Next Steps

1. Configure Azure services (Foundry Agent, OpenAI, Live Avatar)
2. Update `appsettings.json` with your credentials
3. Test endpoints using provided curl commands
4. Implement frontend components for avatar selection
5. Implement frontend request submission to agent
6. Display navigation options and handle user actions
7. Deploy to production with proper secret management

## Security Notes

- **Never commit credentials** to version control
- Use Azure Key Vault for managing secrets in production
- Restrict API access using API keys and managed identities
- Implement rate limiting on agent endpoints
- Validate and sanitize user input before processing
- Use HTTPS for all API communications

## References

- [Azure Speech Services Documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/)
- [Azure OpenAI Documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/)
- [Azure Live Avatar Preview](https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/avatar)
