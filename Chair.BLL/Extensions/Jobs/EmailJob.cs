using System.Net;
using System.Net.Mail;
using Quartz;

namespace Chair.BLL.Extensions.Jobs;

public class EmailJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var mergedJobDataMap = new JobDataMap((IDictionary<string, object>)context.JobDetail.JobDataMap);
        foreach (var key in context.Trigger.JobDataMap.Keys)
        {
            mergedJobDataMap[key] = context.Trigger.JobDataMap[key];
        }

        var email = mergedJobDataMap.GetString("email");
        var subject = mergedJobDataMap.GetString("subject");
        var message = mergedJobDataMap.GetString("message");

        await SendEmailNotificationAsync(email, subject, message);
    }

    public async Task SendEmailNotificationAsync(string email, string subject, string message)
    {
        using var client = new SmtpClient("smtp", 587)
        {
            Credentials = new NetworkCredential("mail", "password"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage("mail", email, subject, message)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(mailMessage);
    }
}

