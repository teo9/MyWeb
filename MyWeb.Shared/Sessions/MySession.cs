using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace MyWeb.Shared.Sessions
{
    public class MySession : IMySession
    {
        private string SessionName = "UserData";
        private string PermissionName = "Permissions";
        private readonly IDistributedCache _session;

        public MySession(
            IDistributedCache session
            )
        {
            _session = session;
            LoadValues();
        }

        public long? UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Permissions { get; set; }

        public void Init(string userId, string userName, List<string> permissions)
        {
            string txt = userId + " " + userName;
            _session.SetString(SessionName, txt);
            string perms = permissions.Aggregate((x, y) => x + ((char)0x1) + y);
            _session.SetString(PermissionName, perms);
            LoadValues();
        }

        private void LoadValues()
        {
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

            string v1 = _session.GetString(PermissionName);
            if (v1 != null)
            {
                string[] vv = v1.Split((char)0x1);
                Permissions = vv.ToList();
            }
            else
            {
                Permissions = new List<string>();
            }
        }
    }

    public interface IMySession
    {
        long? UserId { get; set; }
        string UserName { get; set; }
        List<string> Permissions { get; set; }
        public void Init(string id, string userName, List<string> permissions);
    }
}
