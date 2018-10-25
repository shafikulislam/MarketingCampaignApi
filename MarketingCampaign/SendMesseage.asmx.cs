using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace MarketingCampaign
{
    /// <summary>
    /// Summary description for SendMesseage
    /// </summary>
    [WebService(Namespace = "http://localhost:55261/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // use/not use AJAX, uncomment/comment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SendMesseage : System.Web.Services.WebService
    {

        string fullLog = "";
        [WebMethod]
        public string SendMultiple(List<SingleMesseage> messeages)
        {
            string log = "Got A Request! at " + DateTime.Now.ToString("O") + Environment.NewLine;

            messeages.ForEach(i => log += i.MsgType + (SendMsg(i) ? " Send " : " Send Failed ") + " To "+i.MsgTo+" at "+DateTime.Now.ToString("O")+ Environment.NewLine);

            WriteToLog(log);
            return log;
        }

        [WebMethod]
        public string SendSingle(SingleMesseage messeage)
        {
            string log = "Got A Request! at " + DateTime.Now.ToString("O") + Environment.NewLine;
            log += messeage.MsgType + (SendMsg(messeage) ? " Send " : " Send Failed ") + " To " + messeage.MsgTo + " at " +
                   DateTime.Now.ToString("O") + Environment.NewLine;

            WriteToLog(log);
            return log;
        }


        public static bool SendMsg(SingleMesseage singleMesseage)
        {
            bool msdSendOrNot=true;
            //////All the works
            /////if msg Type is Email Then Send Mail
            /////if msg Type is SMS Then Send SMS
            return msdSendOrNot;
        }

        void WriteToLog(string text)
        {
            try
            {
                string path = @"C:\Anik\Log.txt";
                string createText = "";
                if (!File.Exists(path))
                {
                    createText += "Log Start" + Environment.NewLine;
                    createText += DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + Environment.NewLine;
                    createText += text;
                    text = createText;
                    File.Create(path).Dispose();
                }
                using (FileStream fs = new FileStream(path, FileMode.Append))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(text);
                    fs.Write(info, 0, info.Length);

                    // writing data in bytes already
                    byte[] data = new byte[] { 0x0 };
                    fs.Write(data, 0, data.Length);
                }

            }
            catch (Exception)
            {

            }
            try
            {

                MCampDataContext db = new MCampDataContext();
                User logUser = (User)Session["loguser"];
                Log aLog = new Log();
                aLog.UserId = logUser.Id;
                aLog.LTime = DateTime.Now;
                aLog.Messeage = text;
                db.Logs.InsertOnSubmit(aLog);
                db.SubmitChanges();

            }
            catch (Exception)
            {
                
            }
        }

    }

    public class SingleMesseage
    {
        [Required]
        public string MsgType { get; set; }
        [Required]
        public string MsgTo { get; set; }
        [Required]
        public string Messeage { get; set; }
    }
}
