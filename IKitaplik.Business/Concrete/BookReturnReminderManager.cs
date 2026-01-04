using Core.Utilities.Security.Email;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using IKitaplik.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class BookReturnReminderManager : IBookReturnReminderService
    {
        private readonly IEmailService _emailService;
        private readonly Context _context;
        public BookReturnReminderManager(Context context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task SendReturnRemindersAsync()
        {
            DateTime tomorrowDate = DateTime.Now.AddDays(1).Date;
            var loansDueTomorrow = await _context.Deposits
                .IgnoreQueryFilters()
                .Where(p=>!p.IsDelivered && p.DeliveryDate.Date == tomorrowDate)
                .Include(p => p.Student)
                .Include(p=> p.Book)
                .ToListAsync();
            foreach (var loan in loansDueTomorrow)
            {
                if (loan.Student != null && !string.IsNullOrEmpty(loan.Student?.EMail) && loan.Book != null)
                {
                    string subject = "Kitap İade Hatırlatıcısı";
                    string body = $"Merhaba {loan.Student.Name},<br/><br/>" +
                                  $"Ödünç aldığınız '{loan.Book.Name}' kitabının iade tarihi yarın ({loan.DeliveryDate:dd.MM.yyyy}). " +
                                  $"Lütfen kitabı zamanında iade etmeyi unutmayınız.<br/><br/>" +
                                  "Teşekkürler,<br/>IKitaplik Ekibi";
                    try
                    {
                        await _emailService.SendAsync(loan.Student.EMail, subject, body, true);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Hatırlatıcı e-posta gönderilirken hata oluştu: " + ex.Message);
                    }
                }
            }
        }
    }
}
