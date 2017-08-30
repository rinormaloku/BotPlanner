# BotPlanner

This project showcases Azure Active Directory Authentication using AuthBot which enables access to Office365 APIs from Bot Applications.

Please update [these lines](https://github.com/rinormaloku/BotPlanner/blob/master/BotPlannerExample/Web.config#L9-L18) with your app credentials. 

```
<add key="BotId" value="{YourBotId}" />
<add key="MicrosoftAppId" value="{YourMicrosoftAppId}" />
<add key="MicrosoftAppPassword" value="{YourMicrosoftAppPassword}" />    
```
You get the above values by registering your bot in [Dev.Bot Framework](https://dev.botframework.com)

```
<add key="ActiveDirectory.Tenant" value="{yourtenant.onmicrosoft.com}" />
<add key="ActiveDirectory.ClientId" value="{YourClientId}" />
<add key="ActiveDirectory.ClientSecret" value="{YourClientSecret}" />
<add key="ActiveDirectory.RedirectUrl"        
       value="https://yourbot.azurewebsites.net/api/OAuthCallback" />
```
To get the above values you need to register your app in your Azure Active Directory, Generate a Key and grant your application with appropriate permissions.

These credentails are loaded into the bot on application start.  [Global.asax.cs](https://github.com/rinormaloku/BotPlanner/blob/master/BotPlannerExample/Global.asax.cs#L17-L22) 

AuthBot uses those credentials to get an access token with the required permissions.

### Graph API
The Graph API call to retrieve user tasks is done in the [GetMyTasks method](https://github.com/rinormaloku/BotPlanner/blob/master/BotPlannerExample/Dialogs/RootDialog.cs#L61-L67).
