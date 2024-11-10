using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using quiz.Logger;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace quiz.Ui;

/// <summary>
/// Класс содержит функции для отправки сообщений по электронной почте.
/// </summary>
public class SendEmailLogic : IEmailSender
{
    /// <summary>
    /// Объект с данными настроек приложения.
    /// </summary>
    private IConfiguration _configuration { get; }

    /// <summary>
    /// Объект сервиса для управления сообщениями лога.
    /// </summary>
    private readonly ILogDbDirect _logger;

    public SendEmailLogic(IConfiguration configuration, ILogDbDirect logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Добавление в контейнер зависимостей сервиса для управления отправкой писем по электронной почте. Он должен существовать в единственном экземпляре (singleton).
        string serverName = _configuration["SmtpServerName"];
        int portNumber = int.Parse(_configuration["SmtpServerPortNumber"]);
        string from = _configuration["SmtpServerFrom"];
        string smtpServerUserName = _configuration["SmtpServerUserName"];
        string smtpServerPassword = _configuration["SmtpServerPassword"];

        return CreateMessageAsync(
            serverName,
            portNumber,
            email,
            from,
            subject,
            htmlMessage,
            true,
            smtpServerUserName,
            smtpServerPassword,
            string.Empty,
            true,
            true
            );
    }

    /// <summary>
    /// Создание сообщения электронной почты.
    /// </summary>
    private async Task CreateMessageAsync(string iServerName, int iPortNumber, string iToAddress, string iFromAddress, string iSubjectText, string iBodyText, bool iAuthRequired, string iUserName, string iPassword, string iAttachFileName, bool enableSsl, bool isBodyHtml = false)
    {
        try
        {
            //Создание экземпляра почтового сообщения.
            MailMessage msgMessageInstance = new MailMessage();
            //Настройка параметров сообщения.
            msgMessageInstance.From = new MailAddress(iFromAddress, iFromAddress);
            msgMessageInstance.To.Add(new MailAddress(iToAddress, iToAddress));
            msgMessageInstance.Subject = iSubjectText;
            msgMessageInstance.Body = iBodyText;
            msgMessageInstance.IsBodyHtml = isBodyHtml;

            //Требуется прикрепить файл к письму?
            if ((iAttachFileName.Length > 0) && (File.Exists(iAttachFileName)))
            {
                //[Требуется прикрепить файл к письму].
                //Прикрепление файла к письму.
                msgMessageInstance.Attachments.Add(new Attachment(iAttachFileName));
            }

            //Настройка параметров для определения SMTP-сервера.
            SmtpClient cliMailClient = new SmtpClient
                                       {
                                           Host = iServerName,
                                           Port = iPortNumber,
                                           DeliveryMethod = SmtpDeliveryMethod.Network,
                                           UseDefaultCredentials = false,
                                           EnableSsl = enableSsl
                                       };

            //Подписка на событие, возникающее при отправке сообщения.
            cliMailClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            //Авторизация требуется?
            if (iAuthRequired)
            {
                //[Авторизация требуется].
                //Настройка авторизации.
                cliMailClient.Credentials = new NetworkCredential(iUserName, iPassword);
            }

            //Попытка отправки сообщения.
            cliMailClient.SendAsync(msgMessageInstance, null);
        }
        catch (Exception ex)
        {
            await _logger.WriteLineAsync($"MailLogic: Ошибка при отправке письма электронной почты на адрес '{iToAddress}': {ex}.");
        }
    }
    
    /// <summary>
    /// Событие-результат отправки сообщения.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        //*** Код этой процедуры в режиме release не должен отображать сообщения.
    }
}