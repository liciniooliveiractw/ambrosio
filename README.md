# Ambrosio - The Personal Assistant

# Prerequisites
- [Visual Studio 2017 15.7][2] or newer installed.
- [.Net Core 2.1][3] or higher installed.  
- [Node Package manager 10.15.3 LTS][12] or higher installed
- If you don't have an Azure subscription, create a [free account][10].
- Install the latest version of the [Azure CLI][11] tool. Version 2.0.52 or higher.
- Azure Bot Service CLI tools (latest versions)
```bash
npm install -g ludown luis-apis qnamaker botdispatch msbot chatdown
```
- LuisGen
```bash
dotnet tool install -g luisgen 
```
    
- [Bot Framework Emulator 4.1][6] or newer installed

# Deploy Ambrosio bot
Ambrosio bot require the following Azure services for end-to-end operation:

- Azure Web App
- Azure Cognitive Services - Language Understanding
- Azure Cognitive Services - QnA Maker (includes Azure Search, Azure Web App)

1 - Retrieve your LUIS Authoring Key.

Review this documentation page for the correct LUIS portal for your deployment region. 
Note that www.luis.ai refers to the US region and an authoring key retrieved from this portal will not work with a europe deployment.
Once signed in click on your name in the top right hand corner.
Choose Settings and make a note of the Authoring Key for later use.

2 - Open up a Command Prompt

3 - Login into your Azure Account using the Azure CLI and set subscriptions that you have acess to in the Azure Portal
```bash
az login
az account set --subscription "YOUR SUBSCRIPTION AZURE GUID"
```
4 - Run the msbot clone services command to deploy your services and configure a .bot file in your project. 

NOTE: After deployment is complete, you must make note of the bot file secret that is shown in the Command Prompt window for later use.
Please go first to https://apps.dev.microsoft.com and manually create a new application, 
making note of the AppId and Password/Secret.
```bash
msbot clone services --name 'AmbrosioBot-[nickname]' --luisAuthoringKey 'LUIS_AUTHORING_KEY' --folder 'DeploymentScripts\en' --location 'westeurope' --% --appSecret 'APP_PASSWORD_SECRET' --appId APP_ID
```

5 - After deployment is complete, update appsettings.json with your bot file secret
```bash
'botFilePath': './YOUR_BOT_FILE.bot',
'botFileSecret': 'YOUR_BOT_SECRET,
```
And update too "YOUR_BOT_FILE.bot", where service name is 'development', the appId and appPassword must be empty. 
```bash
{
"type": "endpoint",
"appId": "",
"appPassword": "",
"endpoint": "http://localhost:3978/api/messages",
"id": "1",
"name": "development"
}
```
Finally, restart the emulator and rebuild the solution.
# Running Locally

## Visual Studio
- Open AmbrosioBot.csproj in Visual Studio.
- Run the project (press `F5` key).

## Testing the bot using Bot Framework Emulator
[Microsoft Bot Framework Emulator][5] is a desktop application that allows bot 
developers to test and debug their bots on localhost or running remotely through a tunnel.

## Connect to bot using Bot Framework Emulator **V4**
- Launch the Bot Framework Emulator.
- File -> Open bot and open [AmbrosioBot.bot](AmbrosioBot.bot).

# Deploy the bot to Azure

The deployment process assumes you have an account on Microsoft Azure and are able to log into the [Microsoft Azure Portal][60].
```bash
az bot publish -g ambrosio-group -n AmbrosioBot --proj-file-path AmbrosioBot.csproj --version v4
```

[1]: https://dev.botframework.com
[2]: https://docs.microsoft.com/en-us/visualstudio/releasenotes/vs2017-relnotes
[3]: https://dotnet.microsoft.com/download/dotnet-core/2.1
[5]: https://github.com/microsoft/botframework-emulator
[6]: https://aka.ms/botframeworkemulator
[10]: https://azure.microsoft.com/free/
[11]: https://docs.microsoft.com/cli/azure/install-azure-cli?view=azure-cli-latest
[12]: https://nodejs.org/en/
[60]: https://portal.azure.com
