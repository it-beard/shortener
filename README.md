[![License](https://img.shields.io/github/license/it-beard/shortener)](https://github.com/it-beard/shortener/blob/main/LICENSE)
[![Stars](https://img.shields.io/github/stars/it-beard/shortener)](https://github.com/it-beard/shortener/stargazers)
[![Issues](https://img.shields.io/github/issues/it-beard/shortener)](https://github.com/it-beard/shortener/issues)

[![Deploy apps to Production](https://github.com/it-beard/shortener/actions/workflows/deployment-prod-action.yml/badge.svg?branch=main)](https://github.com/it-beard/shortener/actions/workflows/deployment-prod-action.yml)

# Описание
Web-сервис по сокращению ссылок от АйТиБороды. Создан на .NET 6 и Blazor Server.

<img src="https://github.com/it-beard/shortener/blob/develop/.github/readme-images/1.png" title="Список всех ссылок" width="600" />

<img src="https://github.com/it-beard/shortener/blob/develop/.github/readme-images/2.png" title="Интерфейс сокращения ссылки" width="600" />

# Настройка и запуск
1. Подготовьте SQL-подобную базу данных.
   - Установите [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) или любую другую SQL-native базу данных (Azure SQL Edge под ARM и т.п.)
   - Создайте базу с именем `pds` (или любым другим на ваш вкус).
   - Обновите строку подключения к БД в поле `ConnectionStrings:DefaultConnection` файла `shortener/Shortener/Itbeard.Shortener/appsettings.json `
   - Миграция базы до актуального состояния произайдет автоматически при первом запуске проекта.
2. Настройте аутентификацию через Auth0.
   - Зарегистрируйтесь в Auth0
   - Создайте приложение `Application` типа `Single Page Application`
   - Обновите настройки сокращателя в файле `shortener/Shortener/Itbeard.Shortener/appsettings.json`:
     - `Auth0:Domain` = `Domain` из приложения в Auth0
     - `Auth0:ClientId` = `Client ID` из приложения в Auth0
     - `Auth0:ClientSecret` = `Client Secret` из приложения в Auth0
   - Обновите настройки приложения Auth0:
     - В поле `Allowed Callback URLs` внесите `https://localhost:5001//admin/callback`.
     - В поле `Allowed Logout URLs` внесите `https://localhost:5001`.
   - В Auth0 удалите базу данных на вкладке `Authentication - Database` или уберите связь вашего `Application` с этой базой, что бы аутентификацию только через Google. Управлять способами логина можно на вкладке `Connections` вашего приложения в Auth0
   - Добавьте `Rule` в котором явно пропишите, какие емейлы имеют доступ к авторизации в приложении
     - Пройдите в меню `Auth Pipeline - Rules`
     - Добавьте правило (rule) по шаблону `Whitelist` и внесите в него емейлы, которым разрешен доступ к авторизации.
     - Либо создайте правило со следующим кодом (если шаблона `Whitelist` вдруг нет в Auth0):
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
3. В файле `shortener/Shortener/Itbeard.Shortener/appsettings.json` внесите адрес вашего сокращателя в поле `AppUrl`
4. Соберите и запустите проект `Itbeard.Shortener`.
5. Откройте в браузере ссылку https://localhost:5001/admin - это точка входа в администраторский интерфейс сокращателя.


## Полезные ссылки
Инструкция по настроке Auth0 в Blazor: https://auth0.com/blog/securing-blazor-webassembly-apps/
