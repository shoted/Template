using System.Collections.Generic;
using System.Net;

namespace Lexun.Template.Web.Utils
{
    public class WJson
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 状态值 0:请求成功
        /// </summary>
        public HttpStatusCode Code { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public Dictionary<string, object> Data = null;

        /// <summary>
        /// 添加或修改data指定键值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        public void AddDataItem(string key, object val)
        {
            if (Data != null)
            {
                if (Data.ContainsKey(key))
                    Data[key] = val;
                else
                    Data.Add(key, val);
            }
            else
            {
                Data = new Dictionary<string, object> { { key, val } };
            }
        }
        /// <summary>
        /// 将指定键的值移除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>移除结果</returns>
        public bool Remove(string key)
        {
            return Data.ContainsKey(key) && Data.Remove(key);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void SetValue(bool isSuccess, HttpStatusCode code, string message = "")
        {
            IsSuccess = isSuccess;
            Code = code;
            Message = message;
        }
    }
}