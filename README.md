# JurassicDemoApp
.Net Framework Demo Application

Демо для демонстрации C# skills на базе https://github.com/paulbartrum/jurassic

+ Добавлен логгер на базе Serilog с использованием настроек логгирования из настроек приложения
+ Логгер интегрирован через SerilogConsoleOutput - кастомизированный вариант StandardConsoleOutput
+ Для исполнения CreateServiceW используется класс ServiceTools (отсюда http://stackoverflow.com/questions/358700/how-to-install-a-windows-service-programmatically-in-c )
+ исполнение делается кастомизированной функцией CreateService
+ функция подключается через лямбда-выражение
+ чтобы не заставлять пользователя ловить ошибки вида "Недостаточно прав", в приложение добавлен манифест с запросом прав Администратора. Это же работает и при отладке - студия просит перезапуск, если запущена не под админом
