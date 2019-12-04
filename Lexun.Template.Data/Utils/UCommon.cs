using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lexun.Common;
using Lexun.Stone;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lexun.Template.Data.Utils
{
    public class UCommon
    {
        public static string HostName = "http://act.lexun.com/celebration/index";
        public static List<int> AdminList = new List<int> { 47932589, 50083, 47897418 };

        public static int Sid
        {
            get { return 1; }
        }

        public static bool IsDevelopment
        {
            get { return CTools.GetStrFromConfig("AspNetEnvironment") == "Development"; }
        }

        public static bool IsStaging
        {
            get { return CTools.GetStrFromConfig("AspNetEnvironment") == "Staging"; }
        }

        public static bool IsProduction
        {
            get { return CTools.GetStrFromConfig("AspNetEnvironment") == "Production"; }
        }

        /// <summary>
        /// 分享内线
        /// </summary>
        /// <param name="shareid"></param>
        /// <returns></returns>
        public static string GetShareMsg(int shareid)
        {
            var roomidArr = new int[] { 90, 95, 99, 100, 101, 110 };
            int roomid = roomidArr[CTools.GetRandom(0, roomidArr.Length)];
            string code = UDes.Encode("shareid=" + shareid);
            List<string> baseWord = new List<string>()
            {
                string.Format("我在虫虫农场发现个好东西，(url={0}{1}&code={2})赶快过来你就知道了~(/url)",HostName,roomid,code),
                string.Format("一个人在这玩好寂寞，过来陪我一块玩嘛，(url={0}{1}&code={2})我在这等你！(/url)",HostName,roomid,code),
            };
            return baseWord[CTools.GetRandom(0, baseWord.Count)];
        }

        /// <summary>
        /// 空间动态内线
        /// </summary>
        /// <returns></returns>
        public static string GetZoonActionStr()
        {
            var roomidArr = new int[] { 90, 95, 99, 100, 101, 110 };
            int roomid = roomidArr[CTools.GetRandom(0, roomidArr.Length)];

            var baseWord = new List<string>()
            {
                "在" + "(url=" + HostName + roomid + ")" + "虫虫农场" + "(/url)，玩投资种菜领取了收益",
            };
            return baseWord[CTools.GetRandom(0, baseWord.Count)];
        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetUserHeadImg(int userid)
        {
            if (IsDevelopment)
                return "http://pp.lexun.com/headimg/128X128/head_" + ((userid - 1) % 130 + 1) + ".jpg";

            var headimg = CUser.UserHeadImg(userid);
            if (headimg != null)
            {
                return BlogString.CCache.GetSourceUrl(headimg.Facemiddle);
            }
            return "http://fbbs.lexun.com/images/face/default.png";
        }

        /// <summary>
        /// 获取用户乐币
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static long GetUserStone(int userid)
        {
            return IsDevelopment ? 100000000000L : CStone.GetUserStone(userid);
        }

        /// <summary>
        /// 获取用户昵称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetUserNick(int userid)
        {
            return IsDevelopment ? "乐讯_" + userid % 1000 : CUser.GetUserNick(userid);
        }

        /// <summary>
        /// 获取用户性别
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static int GetUserSex(int userid)
        {
            return IsDevelopment ? 1 : CUser.GetUserSex(userid);
        }

        public static List<string> GetMyFriend(int userid)
        {
            if (IsDevelopment) return new List<string>()
            {
                "47923572","33316",
            };
            return RedisPv.Instance.GetMyFriend(userid) ?? new List<string>();
        }

        public static List<string> GetFriOl2(int userid)
        {
            if (IsDevelopment) return new List<string>()
            {
                "47923572","33316",
            };
            return RedisPv.Instance.GetFriOl2(userid) ?? new List<string>();
        }

        /// <summary>
        /// 获取用户生日
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetUserBirthday(int userid)
        {
            return IsDevelopment ? "1990-12-10" : CUser.GetUserBirthday(userid);
        }

        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string GetSourceUrl(string img)
        {
            return IsDevelopment ? img : BlogString.CCache.GetSourceUrl(img);
        }

        /// <summary>
        /// 加乐币
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="stone"></param>
        /// <param name="sid"></param>
        /// <param name="remark"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool AddStone(int userid, long stone, int sid, string remark, out string outmsg)
        {
            if (IsDevelopment)
            {
                outmsg = "成功";
                return true;
            }
            return CStone.AddStone(userid, stone, sid, SysFlagEnum.IsSystem, 0, 0, remark, out outmsg);
        }

        /// <summary>
        /// 扣乐币
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="stone"></param>
        /// <param name="sid"></param>
        /// <param name="remark"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool ReduceStone(int userid, long stone, int sid, string remark, out string outmsg)
        {
            if (IsDevelopment)
            {
                outmsg = "成功";
                return true;
            }
            return CStone.ReduceStone(userid, stone, sid, SysFlagEnum.IsSystem, 0, remark, out outmsg);
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pwd"></param>
        /// <param name="outmsg"></param>
        /// <returns></returns>
        public static bool CheckLoginPwd(int userid, string pwd, out string outmsg)
        {
            if (IsDevelopment)
            {
                outmsg = "成功";
                return true;
            }
            return CUser.CheckLoginPwd(userid, pwd, out outmsg);
        }

        /// <summary>
        /// list分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listall"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static List<T> ListPager<T>(List<T> listall, int page, int pagesize, ref int total)
        {
            try
            {
                total = 0;
                List<T> list = new List<T>();

                if (listall == null || listall.Count <= 0) return list;

                if (page <= 0 || pagesize <= 0) return list;

                total = listall.Count;

                list = listall.Skip((page - 1) * pagesize).Take(pagesize).ToList();

                return list;
            }
            catch (Exception ex)
            {
                CLog.WriteLog(ex.Message + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 时间转换成长整型
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ConvertToLongTime(string date)
        {
            return CTools.convertToTimeMillis(CTools.ToDateTime(date));
        }

        /// <summary>
        ///  计算距离现在时间长度
        /// </summary>
        /// <param name="longtime"></param>
        /// <returns></returns>
        public static string TimeDiff(long longtime)
        {
            var datetime = CTools.GetUNIXTime(longtime / 1000.0);
            DateTime datetime2 = DateTime.Now;
            int days = datetime2.Subtract(datetime).Days;
            int hours = datetime2.Subtract(datetime).Hours;
            int minutes = datetime2.Subtract(datetime).Minutes;
            int seconds = datetime2.Subtract(datetime).Seconds;
            if (days > 0)
            {
                return days + "天前";
            }
            if (hours > 0)
            {
                return hours + "小时前";
            }
            if (minutes > 0)
            {
                return minutes + "分钟前";
            }
            if (seconds > 0)
            {
                return seconds + "秒前";
            }
            return "1秒前";
        }

        /// <summary>
        /// 安全执行
        /// </summary>
        /// <param name="c"></param>
        /// <param name="callBack"></param>
        public static void SaftExcute(Action c, Action callBack = null)
        {
            try
            {
                if (c != null)
                    c.Invoke();
            }
            catch (Exception ex)
            {
                CLog.WriteLog(ex.Message + ex.StackTrace);
                if (callBack != null)
                    callBack.Invoke();
                if (IsDevelopment) throw;
            }
        }

        /// <summary>
        /// 线程执行
        /// </summary>
        /// <param name="c"></param>
        public static void TaskExcute(Action c)
        {
            if (c == null) return;
            if (IsDevelopment) return;
            SaftExcute(() =>
            {
                Task.Factory.StartNew(c);
            });
        }

        /// <summary>
        /// 获得json
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetObjectByKey<T>(JToken jsonData, string key)
        {
            if (jsonData == null || !jsonData.HasValues) return default(T);
            try
            {
                JToken jToken = jsonData.Value<JToken>(key);
                return ToObject<T>(jToken.ToString());
            }
            catch (Exception ex)
            {
                CLog.WriteLog(ex.Message + ex.StackTrace);
                return default(T);
            }
        }


        /// <summary>
        /// 获得jsonData对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T ToObject<T>(string input)
        {
            if (string.IsNullOrEmpty(input)) return default(T);
            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)input;
                }
                if (typeof(T) == typeof(bool))
                {
                    return (T)(object)Convert.ToBoolean(input);
                }
                if (typeof(T) == typeof(DateTime))
                {
                    return (T)(object)Convert.ToDateTime(input);
                }
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception ex)
            {
                CLog.WriteLog(ex.Message + ex.StackTrace);
                return default(T);
            }
        }

    }
}