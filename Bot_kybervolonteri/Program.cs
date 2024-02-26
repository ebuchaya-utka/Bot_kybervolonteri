using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using System.IO;
//using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net;
using OfficeOpenXml;
using System.Text.RegularExpressions;


namespace TelegramBotExperiments
{
    
    class Program
    {
        static public int ab = 0;
        static ITelegramBotClient bot = new TelegramBotClient("6595172380:AAGKgeFBD67lOpErz0goIoO5Ilm7nLtIb-Q");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)

        {

            // Некоторые действия
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            //if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            //{
            //    var message = update.Message;
            //    if (message.Text.ToLower() == "/start")
            //    {
            //        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
            //        return;
            //    }
            //    await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
            //}

            try
            {
                
                
                // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            // Эта переменная будет содержать в себе все связанное с сообщениями
                            var message = update.Message;

                            // From - это от кого пришло сообщение (или любой другой Update)
                            var user = message.From;

                            // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                            // Chat - содержит всю информацию о чате
                            var chat = message.Chat;

                            //скачивает фото в виде документа

                            if (message.Document != null) 
                            {
                                await botClient.SendTextMessageAsync(message.Chat.Id,"отправь файлом");
                                var fileId = update.Message.Document.FileId;
                                var fileInfo = await botClient.GetFileAsync(fileId);
                                var filePath = fileInfo.FilePath;

                                string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{message.Document.FileName}";
                                await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                                await botClient.DownloadFileAsync(filePath, fileStream);
                                fileStream.Close();



                                return;
                            }

                            

                            // Теперь ваш текст содержит ссылку, которая откроет почтовый клиент Mail при нажатии.


                            // Добавляем проверку на тип Message
                            switch (message.Type)
                            {
                                // Тут понятно, текстовый тип
                                case MessageType.Text:
                                    {
                                        // тут обрабатываем команду /start, остальные аналогично


                                        //Console.WriteLine("Введите URL адрес в формате https://....:");
                                        //string userInput = Console.ReadLine();

                                        Regex regex = new Regex(@"^https:\/\/.*$");
                                        //if (regex.IsMatch(userInput))
                                        //{
                                        //    await botClient.SendTextMessageAsync(
                                        //    chat.Id,
                                        //    $"Спасибо! URL-адрес успешно принят");
                                        //}
                                        //else
                                        //{
                                        //    await botClient.SendTextMessageAsync(
                                        //    chat.Id,
                                        //    $"Неверный формат URL адреса. Пожалуйста, введите URL в формате https://....");
                                        //}


                                        if (regex.IsMatch(message.Text))
                                        {
                                            await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            $"Спасибо! URL-адрес успешно принят");

                                        }


                                        if (message.Text == "/start")
                                        {

                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Привет!\n" +
                                                "На связи Киберволонтеры Хабаровского края\n");

                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Выбери подходящую команду:\n" +
                                                "/info - Чтобы познакомиться с организацией\n" +
                                                "/help - Чтобы помочь нам с поиском противоправного контента\n" +
                                                "/reply - Чтобы ...\n");
                                            return;
                                        }

                                        if (message.Text == "/info")
                                        {
                                            //await botClient.SendTextMessageAsync(
                                            //  chat.Id,
                                            //  "Благодарим за проявленный интерес к нашей организации!\n");


                                            // Тут создаем нашу клавиатуру
                                            var infoKeyboard = new InlineKeyboardMarkup(
                                                new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                                                {
                                        // Каждый новый массив - это дополнительные строки,
                                        // а каждая дополнительная кнопка в массиве - это добавление ряда




                                                    new InlineKeyboardButton[] // тут создаем массив кнопок
                                                    {
                                                        InlineKeyboardButton.WithUrl("Мы в VK", "https://vk.com/kibervolonterykhv"),
                                                        InlineKeyboardButton.WithUrl("Мы в Telegram", "https://t.me/kibervolonterykhv"),
                                                    },
                                                    new InlineKeyboardButton[]
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("Написать на почту", "Написать на почту"),
                                                        //InlineKeyboardButton.WithCallbackData("Тут еще копка", "button2"), // после нажатия всплывает сообщение
                                                        InlineKeyboardButton.WithCallbackData("Перейти на наш сайт", "button3"), //после нажатия выводится окно с сообщением
                                                    },
                                                });

                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Благодарим за проявленный интерес к нашей организации!",
                                                replyMarkup: infoKeyboard); // Все клавиатуры передаются в параметр replyMarkup

                                            return;
                                        }

                                        if (message.Text == "/help")
                                        {
                                            //await botClient.SendTextMessageAsync(
                                            //  chat.Id,
                                            //  "Благодарим за проявленный интерес к нашей организации!\n");


                                            // Тут создаем нашу клавиатуру
                                            var infoKeyboard = new InlineKeyboardMarkup(
                                                new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                                                {
                                        // Каждый новый массив - это дополнительные строки,
                                        // а каждая дополнительная кнопка в массиве - это добавление ряда




                                                    new InlineKeyboardButton[] // тут создаем массив кнопок
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("Отправить фото", "photo"),
                                                        InlineKeyboardButton.WithCallbackData("Направить ссылку", "web"),
                                                    },
                                                    new InlineKeyboardButton[]
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("Пост социальной сети", "post"),
                                                        //InlineKeyboardButton.WithCallbackData("Тут еще копка", "button2"), // после нажатия всплывает сообщение
                                                        InlineKeyboardButton.WithCallbackData("Иное", "other"), //после нажатия выводится окно с сообщением
                                                    },
                                                });

                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Благодарим за Ваше содействие и помощь в деятельности нашей организации!\n" +
                                                " - Если, гуляя по улице, ты случайно заметил информацию, которая противоречит действующему " +
                                                "законодательству РФ, то смело нажимай команду 'Отправить фото!'\n" +
                                                " - Или, листая ленту в любимой Соцсети, наткнулся на контент, который даже при всем своем желании" +
                                                "не считаешь законным, - жми 'Пост социальной сети'\n" +
                                                " - Гулял на просторах Интернета, но увидел противоправный контент?" +
                                                "Направляй ссылку на него, нажав на кнопку 'Направить ссылку'\n" +
                                                " - Есть иной источник распространения незаконной информации? " +
                                                "Жми 'Иное'\n\n" +
                                                "Перед тем, как отправить найденный источник, обязательно прочти Инструкцию! " +
                                                "В ней подробно прописано, что относится к противоправному контенту, какие социальные " +
                                                "сети мы рассматриваем, и другую важную информацию.",
                                                replyMarkup: infoKeyboard); // Все клавиатуры передаются в параметр replyMarkup


                                           
                                            return;
                                        }

                                        if (message.Text == "/reply")
                                        {
                                           

                                            // Тут все аналогично Inline клавиатуре, только меняются классы
                                            // НО! Тут потребуется дополнительно указать один параметр, чтобы
                                            // клавиатура выглядела нормально, а не как абы что

                                            var replyKeyboard = new ReplyKeyboardMarkup(
                                                new List<KeyboardButton[]>()
                                                {
                                                    new KeyboardButton[]
                                                    {
                                                        new KeyboardButton("Привет!"),
                                            
                                                        new KeyboardButton("Пока!"),
                                                    },
                                                    new KeyboardButton[]
                                                    {
                                                        new KeyboardButton("отправь фотку с запрещённым контентом!")

                                           
                                                    },
                                                    new KeyboardButton[]
                                                    {
                                                        new KeyboardButton("Напиши моему соседу!") 
                                                    }
                                                })
                                            {
                                                // автоматическое изменение размера клавиатуры, если не стоит true,
                                                // тогда клавиатура растягивается чуть ли не до луны,
                                                // проверить можете сами
                                                ResizeKeyboard = true,
                                            };

                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Это reply клавиатура!",
                                                replyMarkup: replyKeyboard); // опять передаем клавиатуру в параметр replyMarkup

                                            return;
                                        }



                                        // фото скачивает на рабочий стол
                                        //надо поменять путь к скачанным файлам

                                        if (message.Type == MessageType.Photo)
                                        {
                                            var fileId = message.Photo[^1].FileId;
                                            var file = await botClient.GetFileAsync(fileId);

                                            var pathToSave = $@"C:\Users\user\Desktop\Бот сука бот\картинки\{fileId}.jpg";
                                            //ab++;// Укажите путь сохранения

                                            using (var client = new WebClient())
                                            {
                                                await client.DownloadFileTaskAsync(file.FilePath, $"{pathToSave}\\{fileId}.jpg");
                                            }

                                            await botClient.SendTextMessageAsync(message.Chat.Id, "спасибо, фото сохранено");
                                        }
                                        else if (message.Type == MessageType.Text && message.Text == "Отправь фотку с запрещённым контентом!")
                                        {
                                            await botClient.SendTextMessageAsync(message.Chat.Id, "Пожалуйста, отправьте картинку");
                                        }



                                        // здесь  сохраняется текст в файл excel

                                          




                                        

                                        return;
                                    }

                                case MessageType.Photo:
                                    {
                                        //if (message.Type == MessageType.Photo)
                                        //{
                                        //    var fileId = message.Photo[^1].FileId;
                                        //    var file = await botClient.GetFileAsync(fileId);

                                        //    var pathToSave = $@"C:\Users\user\Desktop\Бот сука бот\картинки\file{ab}.jpg"; // Укажите путь сохранения

                                        //    using (var client = new WebClient())
                                        //    {
                                        //        await client.DownloadFileTaskAsync(file.FilePath, $"{pathToSave}\\{fileId}.jpg");
                                        //    }

                                        //    await botClient.SendTextMessageAsync(message.Chat.Id, "Cпасибо! Фото успешно сохранено.");
                                        //}
                                        //else if (message.Type == MessageType.Text && message.Text == "Отправь фото с запрещённым контентом!")
                                        //{
                                        //    await botClient.SendTextMessageAsync(message.Chat.Id, "Пожалуйста, отправь картинку файлом");
                                        //}
                                        //return;

                                        var fileId = message.Photo.Last().FileId; // Получаем FileId последней (самой большой по размеру) фотографии
                                        var file = await botClient.GetFileAsync(fileId); // Получаем информацию о файле
                                        var filePath = file.FilePath; // Получаем путь к файлу
                                        string destinationFilePath = $@"C:\Users\user\Desktop\Бот сука бот\картинки\{fileId}.jpg";
                                        //     ab++;// Укажите путь к папке на ПК админа
                                        using var stream = System.IO.File.OpenWrite(destinationFilePath);
                                        await botClient.DownloadFileAsync(filePath, stream); // Сохраняем файл
                                        await botClient.SendTextMessageAsync(message.Chat.Id, "Фотография сохранена"); // Отправляем пользователю сообщение об успешном сохранении
                                        return;
                                    }

                                // Добавил default , чтобы показать вам разницу типов Message
                                default:
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Попробуй еще раз :)");
                                        return;
                                    }
                                
                            }
                            

                            return;
                        }

                    case UpdateType.CallbackQuery:
                        {
                            // Переменная, которая будет содержать в себе всю информацию о кнопке, которую нажали
                            var callbackQuery = update.CallbackQuery;

                            // Аналогично и с Message мы можем получить информацию о чате, о пользователе и т.д.
                            var user = callbackQuery.From;

                            // Выводим на экран нажатие кнопки
                            Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

                            // Вот тут нужно уже быть немножко внимательным и не путаться!
                            // Мы пишем не callbackQuery.Chat , а callbackQuery.Message.Chat , так как
                            // кнопка привязана к сообщению, то мы берем информацию от сообщения.
                            var chat = callbackQuery.Message.Chat;

                            // Добавляем блок switch для проверки кнопок
                            switch (callbackQuery.Data)
                            {
                                // Data - это придуманный нами id кнопки, мы его указывали в параметре
                                // callbackData при создании кнопок. У меня это button1, button2 и button3

                                case "button1":
                                    {
                                        // В этом типе клавиатуры обязательно нужно использовать следующий метод
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            $"Вы нажали на {callbackQuery.Data}");
                                        return;
                                    }

                                case "Написать на почту":
                                    {
                                        string email = "kibervolonterykhv@mail.ru"; // Замените на нужный адрес электронной почты
                                        string mailtoLink = $"mailto:{email}";

                                        // Создаем ссылку с текстом "Отправить почту"
                                        string linkText = "Отправить почту";
                                        string mailtoHtmlLink = $"<a href=\"{mailtoLink}\">{linkText}</a>";

                                        // Вставляем mailtoHtmlLink в ваш текст
                                        string yourTextWithMailtoLink = $"Нажмите {email}, чтобы написать нам на почту";

                                        // В этом типе клавиатуры обязательно нужно использовать следующий метод
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                                        // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            yourTextWithMailtoLink);
                                        return;
                                    }

                                case "web":     // после нажатия всплывает сообщение
                                    {
                                        // А здесь мы добавляем наш сообственный текст, который заменит слово "загрузка", когда мы нажмем на кнопку
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Просто отправь ссылку текстовым сообщением");
                                        

                                        //await botClient.SendTextMessageAsync(
                                        //    chat.Id,
                                        //    $"Спасибо!");
                                        return;
                                    }

                                case "photo":     // после нажатия всплывает сообщение
                                    {
                                        // А здесь мы добавляем наш сообственный текст, который заменит слово "загрузка", когда мы нажмем на кнопку
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Отправь, пожалуйста, фото файлом!\n" +
                                            "После того, как отправишь, напиши краткое описание, что ты посчитал противоправным)", true);

                                        

                                        //var anonKeyboard = new InlineKeyboardMarkup(
                                        //        new List<InlineKeyboardButton[]>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                                        //        { 
                                        //new InlineKeyboardButton[] // тут создаем массив кнопок
                                        //{
                                        //    InlineKeyboardButton.WithUrl("Анонимно", "anonphoto"),
                                        //    InlineKeyboardButton.WithUrl("Не анонимно", "neanonphoto"),
                                        //},
                                       
                                        //        });

                                        //await botClient.SendTextMessageAsync(
                                        //    chat.Id,
                                        //    $"Ты предпочитаешь быть нашим анонимным помощником, или хочешь, " +
                                        //    $"чтобы мы знали кибергероя?",
                                        //    replyMarkup: anonKeyboard); // Все клавиатуры передаются в параметр replyMarkup


                                        return;
                                    }

                               

                                case "button3":   //после нажатия выводится окно с сообщением
                                    {
                                        // А тут мы добавили еще showAlert, чтобы отобразить пользователю полноценное окно
                                        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Большое спасибо!", showAlert: true);

                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            $"Вы нажали на {callbackQuery.Data}");
                                        return;
                                    }
                            }

                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };

            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken

            );
            Console.ReadLine();
        }
    }
}