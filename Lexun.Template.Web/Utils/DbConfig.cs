using Lexun.Common;

namespace Lexun.Template.Web.Utils
{
    public class DbConfig
    {
        public static CDatabase GetDB()
        {
            return new CDatabase("ActivityCenter_Act");
        }

        public static CDatabase GetLxMsgDB()
        {
            return new CDatabase(CConn.LxFriends_Msg, true);
        }

       

        public static void CloseDb(CDatabase db)
        {
            if (db != null)
                db.close();
        }
    }
}