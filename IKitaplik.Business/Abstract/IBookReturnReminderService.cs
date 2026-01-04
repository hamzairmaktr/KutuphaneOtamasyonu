using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IBookReturnReminderService
    {
        Task SendReturnRemindersAsync();
    }
}
