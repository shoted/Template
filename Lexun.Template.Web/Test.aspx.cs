using System;
using System.Net;
using Lexun.Common;
using Lexun.Template.Web.Utils;
using ServiceStack.ServiceClient.Web;

namespace Lexun.Template.Web
{
    public partial class Test : TemplatePage
    {
        protected override bool NeedLogin()
        {
            if (Request.HttpMethod.Equals(HttpMethod.Get))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取祝福墙
        /// </summary>
        public override void Get()
        {
            WJson.SetValue(true, HttpStatusCode.OK, "获取成功");
        }

        /// <summary>
        /// 发送祝福
        /// </summary>
        public override void Post()
        {
        }
    }
}