using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace MyWeb.Shared.Sessions
{
    public class MySession : IMySession
    {
        private string SessionName = "UserData";
        private readonly IDistributedCache _session;

        public MySession(
            IDistributedCache session
            )
        {
            _session = session; 
            string v = _session.GetString(SessionName);
            if (v != null)
            {
                string[] vv = v.Split(' ');
                UserId = long.Parse(vv[0]);
                UserName = vv[1];
            }
            else
            {
                UserId = null;
                UserName = null;
            } 
        }

        public long? UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Permissions { get; set; }

        public void Init(string userId, string userName)
        { 
            string txt = userId + " " + userName;
            _session.SetString("UserData", txt);
        }
    }

    public interface IMySession
    {
        long? UserId { get; set; }
        string UserName { get; set; }
        List<string> Permissions { get; set; }
        public void Init(string id, string userName);
    }
}
