# Ambrosio - The Personal Assistant

This bot is a sample Ambrosio to conceive for **************, power by **********
This bot has been created using [Microsoft Bot Framework][1], 

# Prerequisites
- [Visual Studio 2017 15.7][2] or newer installed.
- [.Net Core 2.1][3] or higher installed.  
- [Node Package manager 10.15.3 LTS][12] or higher installed
- If you don't have an Azure subscription, create a [free account][10].
- Install the latest version of the [Azure CLI][11] tool. Version 2.0.52 or higher.
- Azure Bot Service CLI tools (latest versions)
    Shell
        ```bash
        npm install -g ludown luis-apis qnamaker botdispatch msbot chatdown
        ```
- LuisGen
	Shell
		```bash
        dotnet tool install -g luisgen 
        ```
- [Bot Framework Emulator 4.1][6] or newer installed


#Deploy Ambrosio bot
Ambrosio bot require the following Azure services for end-to-end operation:

-Azure Web App
-Azure Cognitive Services - Language Understanding
-Azure Cognitive Services - QnA Maker (includes Azure Search, Azure Web App)


The following steps will help you to deploy these services using the provided deployment scripts into solution:

1 - Retrieve your LUIS Authoring Key.

Review this documentation page for the correct LUIS portal for your deployment region. 
Note that www.luis.ai refers to the US region and an authoring key retrieved from this portal will not work with a europe deployment.
Once signed in click on your name in the top right hand corner.
Choose Settings and make a note of the Authoring Key for later use.

2 - Open up a Command Prompt.
3 - Login into your Azure Account using the Azure CLI and set subscriptions that you have acess to in the Azure Portal
	Shell
		```bash
        az login
		az account set --subscription "YOUR SUBSCRIPTION AZURE GUID"
        ```
4 - Run the msbot clone services command to deploy your services and configure a .bot file in your project. 
NOTE: After deployment is complete, you must make note of the bot file secret that is shown in the Command Prompt window for later use.
Please go first to https://apps.dev.microsoft.com and manually create a new application, making note of the AppId and Password/Secret. 
	Shell
		```bash
        msbot clone services --name "AmbrosioBot" --luisAuthoringKey "LUIS_AUTHORING_KEY" --folder "DeploymentScripts\en" --location "centralus" --% --appSecret "APP_PASSWORD_SECRET" --appId APP_ID
        ```
5 - After deployment is complete, update appsettings.json with your bot file secret.
		```bash
        "botFilePath": "./YOUR_BOT_FILE.bot",
		"botFileSecret": "YOUR_BOT_SECRET",
        ```
		
# Running Locally

## Visual Studio
- Open AmbrosioBot.csproj in Visual Studio.
- Run the project (press `F5` key).

## Testing the bot using Bot Framework Emulator
[Microsoft Bot Framework Emulator][5] is a desktop application that allows bot 
developers to test and debug their bots on localhost or running remotely through a tunnel.
- Install the [Bot Framework emulator][6].

## Connect to bot using Bot Framework Emulator **V4**
- Launch the Bot Framework Emulator.
- File -> Open bot and open [AmbrosioBot.bot](AmbrosioBot.bot).

# Deploy the bot to Azure
See [Deploy your C# bot to Azure][50] for instructions.

The deployment process assumes you have an account on Microsoft Azure and are able to log into the [Microsoft Azure Portal][60].

If you are new to Microsoft Azure, please refer to [Getting started with Azure][70] for guidance on how to get started on Azure.
	Shell
		```bash
        az bot publish -g ambrosio-group -n AmbrosioBot --proj-file-path AmbrosioBot.csproj --version v4
        ```
		
# Further reading
* [Bot Framework Documentation][80]
* [Bot Basics][90]
* [Azure Bot Service Introduction][100]
* [Azure Bot Service Documentation][110]
* [Azure CLI][120]
* [msbot CLI][130]
* [Azure Portal][140]
* [Language Understanding using LUIS][150]

[1]: https://dev.botframework.com
[2]: https://docs.microsoft.com/en-us/visualstudio/releasenotes/vs2017-relnotes
[3]: https://dotnet.microsoft.com/download/dotnet-core/2.1
[5]: https://github.com/microsoft/botframework-emulator
[6]: https://aka.ms/botframeworkemulator

[10]: https://azure.microsoft.com/free/
[11]: https://docs.microsoft.com/cli/azure/install-azure-cli?view=azure-cli-latest
[12]: https://nodejs.org/en/

[50]: https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-deploy-azure?view=azure-bot-service-4.0
[60]: https://portal.azure.com
[70]: https://azure.microsoft.com/get-started/
[80]: https://docs.botframework.com
[90]: https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0
[100]: https://docs.microsoft.com/en-us/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0
[110]: https://docs.microsoft.com/en-us/azure/bot-service/?view=azure-bot-service-4.0
[120]: https://docs.microsoft.com/en-us/cli/azure/?view=azure-cli-latest
[130]: https://github.com/Microsoft/botbuilder-tools/tree/master/packages/MSBot
[140]: https://portal.azure.com
[150]: https://www.luis.ai
