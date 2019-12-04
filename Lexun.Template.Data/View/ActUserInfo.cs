using Lexun.Template.Data.Utils;

namespace Lexun.Template.Data.View
{
    public class ActUserInfo
    {
        public int UserId { get; set; }
        public string HeadImg { get; set; }
        public int Sex { get; set; }
        public string Nick { get; set; }

        public ActUserInfo(int userid)
        {
            UserId = userid;
            HeadImg = UCommon.GetUserHeadImg(userid);
            Sex = UCommon.GetUserSex(userid);
            Nick = UCommon.GetUserNick(userid);
        }
    }
}
