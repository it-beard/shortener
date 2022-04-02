# Описание
Web-сервис по сокращению ссылок от АйТиБороды.

# Настройка и запуск
1. Подготовьте SQL-подобную базу данных
   - Установите [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) или любую другую SQL-native базу данных (Azure SQL Edge под ARM и т.п.)
   - Создайте базу с именем `pds` (или любым другим на ваш вкус)
   - Обновите строку подключения к БД в поле `ConnectionStrings:DefaultConnection` файла `shortener/Shortener/Itbeard.Shortener/appsettings.json `
   - Миграция базы до актуального состояния произайдет автоматически при первом запуске проекта.
3. Настройте аутентификацию через Auth0
4. Соберите и запустите проект `Itbeard.Shortener`
