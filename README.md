[![License](https://img.shields.io/github/license/it-beard/shortener)](https://github.com/it-beard/shortener/blob/main/LICENSE)
[![Stars](https://img.shields.io/github/stars/it-beard/shortener)](https://github.com/it-beard/shortener/stargazers)
[![Issues](https://img.shields.io/github/issues/it-beard/shortener)](https://github.com/it-beard/shortener/issues)

[![Deploy apps to Production](https://github.com/it-beard/shortener/actions/workflows/deployment-prod-action.yml/badge.svg?branch=main)](https://github.com/it-beard/shortener/actions/workflows/deployment-prod-action.yml)

# Description
**URL Shortener** is an open-source web service for shortening URLs and based on .NET 6 and Blazor Server techologies.

URL shortener consist of two parts:
- User area
- Admin area

**User area** is a simple page that working as redirection mechanism. 
- User type shortened link in browser
- Goes to the redirector's page (file `Redirector.razor`)
- `Redirector.razor` redirect user to target URL

**Admin area** has functionality described bellow:
- Authentication flow using Auth0 
- Reducing links into short link wit 7-simbolic auto-generated or predefined alias
- Editing target URL in created short link
- Support multilanguage interface (English, Russian, Belarusian)

Also Shortener collect data about rederections for each link: count of redirections, agent info etc.

## Interface of application

<img src="https://github.com/it-beard/shortener/blob/develop/.github/readme-images/0.png" title="Login interface" width="600" />

<img src="https://github.com/it-beard/shortener/blob/develop/.github/readme-images/1.png" title="List of all URLs" width="600" />

<img src="https://github.com/it-beard/shortener/blob/develop/.github/readme-images/2.png" title="Reduce URL interface" width="600" />

<img src="https://github.com/it-beard/shortener/blob/develop/.github/readme-images/3.png" title="Edit URL interface" width="600" />

# Settigs-up and Run local instance
1. Prepare a SQL-like database.
   - Setup an [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or any other SQL-like database (for example `Azure SQL Edge` good one for ARM processers and so on)
   - Create the database with `pds` (or any other name on your choise).
   - Update the database connection string in the`ConnectionStrings:DefaultConnection` field of `shortener/Shortener/Itbeard.Shortener/appsettings.json` file.
   - Migration of the database to the current state will occur automatically the first time you run the project.
2. Set up authentication via Auth0.
   - Register an account on Auth0 service.
   - Create application `Application` with type `Single Page Application`
   - Update shortener settings in `shortener/Shortener/Itbeard.Shortener/appsettings.json` file:
     - `Auth0:Domain` = value of `Domain` field from Auth0 application
     - `Auth0:ClientId` = value of `Client ID` field from Auth0 application
     - `Auth0:ClientSecret` = value of `Client Secret` field from Auth0 application
   - Update settings of Auth0 application:
     - In field `Allowed Callback URLs` set `https://localhost:5001//admin/callback`.
     - In field `Allowed Logout URLs` set `https://localhost:5001`.
   - In Auth0, delete the database on the `Authentication - Database` tab or remove your `Application` link to this database, so that you only authenticate through Google. You can control the login methods on the `Connections` tab of your application in Auth0.
   - Add a `Rule` in which you explicitly specify which emails have access to authorization in the application:
     - Go to `Auth Pipeline - Rules` menu.
     - Add rule in accordance with `Whitelist` template and update rule by emails that are allowed to access the authorization.
     - Or create a rule with the following code (if the `Whitelist` template is suddenly not founded in Auth0):
```
function userWhitelist(user, context, callback) {

  // Access should only be granted to verified users.
  if (!user.email || !user.email_verified) {
    return callback(new UnauthorizedError('Access denied.'));
  }

  const whitelist = [ 'user1@example.com', 'user2@example.com' ]; //authorized users
  const userHasAccess = whitelist.some(
    function (email) {
      return email === user.email;
    });

  if (!userHasAccess) {
    return callback(new UnauthorizedError('Access denied.'));
  }

  callback(null, user, context);
}
```
3. Add URL of your Shortener instanse in to field `AppUrl` in `shortener/Shortener/Itbeard.Shortener/appsettings.json` file.
4. Build and run project `Itbeard.Shortener`.
5. Open https://localhost:5001/admin in your browser - this URL is the entry point to the admin interface of the Shortener.


## Useful links
- Instructions of how to setting up Auth0 in Blazor: https://auth0.com/blog/securing-blazor-webassembly-apps/
