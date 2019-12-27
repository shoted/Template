using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lexun.Common;
using Lexun.Template.Data.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceStack.ServiceClient.Web;

namespace Lexun.Template.Web.Utils
{
    public abstract class TemplatePage : BaseWmlPage
    {
        protected WJson WJson = new WJson();

        protected int UserId
        {
            get
            {
                if (!UCommon.IsDevelopment) return User.UserId;
                int userid = CRequest.GetInt("userid");
                return userid == 0 ? 33316 : userid;
            }
        }

        private void ProcessAllowOrigin()
        {
            if (UCommon.IsDevelopment)
            {
                Response.AddHeader("Access-Control-Allow-Origin", "*");
                Response.AddHeader("Access-Control-Allow-Methods", "*");
                Response.AddHeader("Access-Control-Max-Age", "2000");
                return;
            }

            Response.AddHeader("Content-Type", "application/json;charset=utf-8");
            List<string> allowDomain = new List<string>() { "http://chat.lexun.com", "http://act.lexun.com" };
            Response.AddHeader("Access-Control-Allow-Credentials", "true");
            if (Request.UrlReferrer == null) return;
            string url = "http://" + Request.UrlReferrer.Host;
            if (allowDomain.Contains(url))
                Response.AddHeader("Access-Control-Allow-Origin", url);
        }

        /// <summary>
        /// httpGet请求
        /// </summary>
        public abstract void Get();
        /// <summary>
        /// httpPost请求
        /// </summary>
        public abstract void Post();

        protected void InitData()
        {
            if (Request.HttpMethod.Equals(HttpMethod.Get))
            {
                Get();
            }
            else if (Request.HttpMethod.Equals(HttpMethod.Post))
            {
                Post();
            }
        }


        protected virtual void OutPut()
        {
            Response.ClearContent();
            // 设置为驼峰命名
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            Response.Write(JsonConvert.SerializeObject(WJson, serializerSettings));
            Response.End();
        }


        protected override void PageShow()
        {
            ProcessAllowOrigin();
            if (NeedLogin() && UserId <= 0)
            {
                WJson.SetValue(false, HttpStatusCode.Unauthorized, "请登录");
                OutPut();
                return;
            }
            UCommon.SaftExcute(InitData);
            OutPut();
        }

        protected virtual bool NeedLogin()
        {
            return true;
        }

        protected bool IsBlackSelf()
        {
            int result = 0;
            try
            {
                var jsonData = CHtmlDown.HttpPost("http://g.lexun.com/gamepage/gamehandle.aspx?op=gamedef&_r="
                    + CTools.GetRandom(10000000, 100000000) + "&", wml.CdLxt, Encoding.UTF8);
                result = CTools.ToInt(CRegex.Matchs(jsonData, "\"result\":(?<result>\\d*)")[0].Groups["result"].Value);
                if (result > 0)
                {
                    WJson.Code = HttpStatusCode.Ambiguous;
                    WJson.AddDataItem("url", "http://g.lexun.com/gamepage/blackself.aspx");
                }
            }
            catch (Exception ex)
            {
                CLog.WriteLog(ex.Message + ex.StackTrace);
                if (UCommon.IsDevelopment) throw;
            }
            return result > 0;
        }
    }
}