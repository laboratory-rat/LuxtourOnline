using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuxtourOnline.Utilites;
using System.Threading.Tasks;
using NLog;

namespace LuxtourOnline.Tasks
{
    public class EmailTestJob : IJob
    {
        Logger _logger = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {

            Task.Factory.StartNew(() =>
            {
                _logger.Info("Starting mail sycle");

                try
                {
                    MailMaster.SendMail("info@luxtour.online", "testing mail actions", "Subject", "body message");
                    MailMaster.SendMail("oleg.timofeev20@gmail.com", "testing mail actions", "Subject", "body message");
                }
                catch (Exception ex)
                {
                    _logger.Error("Can't send email! " + ex);
                }
             });
        }
    }
}