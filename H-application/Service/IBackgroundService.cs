using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Service
{
    public interface IBackgroundService
    {

        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
