using Lexun.Template.Web.Utils;

namespace Lexun.Template.Web
{
    public partial class Test : TemplatePage
    {
        protected override bool CheckLogin()
        {
            return false;
        }

        public override void Get()
        {

        }

        public override void Post()
        {

        }
    }


}