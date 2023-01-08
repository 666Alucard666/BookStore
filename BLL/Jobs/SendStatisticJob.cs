using System.Net.Mail;
using BLL.Abstractions.ServiceInterfaces;
using DAL;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace BLL.Jobs;

public class SendStatisticJob : IJob
{
    private readonly GigienaStoreDbContext _context;
    private readonly IPersonService _personService;

    public SendStatisticJob(GigienaStoreDbContext context, IPersonService personService)
    {
        _context = context;
        _personService = personService;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var adminMails = await _context.Workers.AsNoTracking().Include(x => x.WorkerNavigation)
            .Where(x => x.Specialty.Equals("Administrator")).Select(x => x.WorkerNavigation.Email).ToListAsync();
        var result = await _personService.GetMonthStatistic();
        if (result)
        {
            foreach (var adminMail in adminMails)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("dmytro.vakhnenko@nure.ua");
                mail.To.Add(adminMail);
                mail.Subject = "Month statistic for your shops";
                mail.Body = "Thanks for being with us";

                var attachment = new Attachment($@"{Environment.CurrentDirectory}\MonthStatistic.pdf");
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("dmytro.vakhnenko@nure.ua", "zjXoS9jz");
                SmtpServer.EnableSsl = true;

                await SmtpServer.SendMailAsync(mail);
            }
        }

    }
}