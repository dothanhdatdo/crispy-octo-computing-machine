using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net.NetworkInformation;
namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            const double interval60Minutes = 120* 1000; // milliseconds to one hour

            Timer checkForTime = new Timer(interval60Minutes);
            checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            checkForTime.Enabled = true;
        }
        void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            Ping ping = new Ping();

            PingReply reply = ping.Send("192.168.35.21");

            if (reply.Status == IPStatus.Success)
            {
                //SendEmail("ok");
                Utilities.WriteLogError("Status: success!!");

            }
            else
            {
                SendEmail("Nok");

            }
        }
        static void SendEmail(string mailBodyhtml)
        {
            //string mailBodyhtml =
            //    "<p>some text here</p>";

            var msg = new MailMessage("dothanhdatdo@gmail.com", "dothanhdatdo1@gmail.com", "Hello", mailBodyhtml);
            //msg.To.Add("to2@gmail.com");
            msg.IsBodyHtml = true;
            var smtpClient = new SmtpClient("smtp.gmail.com", 587); //**if your from email address is "from@hotmail.com" then host should be "smtp.hotmail.com"**
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential("dothanhdatdo@gmail.com", "");
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
            Console.WriteLine("Email Sent Successfully");
        }   
        private void timer_Tick(object sender, ElapsedEventArgs args)
        {
            Utilities.WriteLogError("Timer has ticked for doing something in 30s!!!");

        }
        protected override void OnStop()
        {
            timer.Enabled = true;
            Utilities.WriteLogError("1st WindowsService has been stop");

        }
        //------------
        public class Utilities
        {
            public static void WriteLogError(Exception ex)
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                    sw.WriteLine(DateTime.Now.ToString("g") + ": " + ex.Source + ";" + ex.Message);
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception)
                {

                }
            }
            public static void WriteLogError(string message)
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                    sw.WriteLine(DateTime.Now.ToString("g") + ": " + message);
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception)
                {

                    //
                }
            }
        }
     
    }
}
