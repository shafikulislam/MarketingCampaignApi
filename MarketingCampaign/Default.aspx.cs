using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Css.Extensions;

namespace MarketingCampaign
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User logUser = (User)Session["loguser"];
            if (!IsPostBack)
            {
                if (logUser != null) LoadAllCampaignsByUser(logUser.Id);
            }
        }

        private void LoadAllCampaignsByUser(long id)
        {
            MCampDataContext db = new MCampDataContext();
            var campaigns = (from c in db.Campaigns
                where c.UserId == id
                select c).ToList();

            campaignListTableTbody.InnerHtml = MakeCampaignTableHtml(campaigns);
        }

        private string MakeCampaignTableHtml(List<Campaign> campaigns)
        {
            string html = "";
            int i = 1;
            foreach (Campaign c in campaigns)
            {
                List<long> s = new List<long>();
                c.CampaignCustomers.ForEach(j => s.Add(j.CustomerId));
                html += "<tr>";
                html += "<td>"+i+"</td>";
                html += "<td>"+c.Title+"</td>";
                html += "<td>"+c.Type+"</td>";
                html += "<td>"+c.CustomerCount+"</td>";
                html += "<td class='hidden customers'>"+string.Join(",",s)+"</td>";
                html += "<td><button type='button' class='btn btn-sm btn-primary' onClick='sendMesseage(\""+c.Id+"\");'>Send</button></td>";
                html += "</tr>";
                i++;
            }
            return html;
        }

        User aUser = new User();
        MCampDataContext db  = new MCampDataContext();


        [WebMethod]
        public static string SendMsgForCampaign(long campaignId, string messeage)
        {
            MCampDataContext db = new MCampDataContext();
            var cus = (from u in db.CampaignCustomers where u.CampaignId == campaignId select u.Customer).ToList();

            List<localhost.SingleMesseage> messeages = new List<localhost.SingleMesseage>();
            cus.ForEach(i=>
            {
                var f = i.CampaignCustomers.FirstOrDefault();
                if (f != null)
                    messeages.Add(new localhost.SingleMesseage()
                    {
                        MsgType = f.Campaign.Type,
                        MsgTo = f.Campaign.Type=="SMS"?i.Mobile:i.Email,
                        Messeage = f.Campaign.Messeage
                    });
            });
            localhost.SendMesseage x = new localhost.SendMesseage();
            return x.SendMultiple(messeages.ToArray());
        }

        [WebMethod]
        public static string SendMsgForCampaign(long customerId)
        {
            MCampDataContext db = new MCampDataContext();
            var customer = (from u in db.Customers where u.Id == customerId select u).FirstOrDefault();

            localhost.SendMesseage x = new localhost.SendMesseage();
            var res = x.SendSingle(new localhost.SingleMesseage()
            {
                
            });
            return res;
        }
    }
}