using System.Text;

namespace VLTP.Models
{
    public class LoginInfo
    {
        public bool LoginPermitted {get;}
        public string Message {get;}

        public LoginInfo(bool permission, string msg)
        {
            this.LoginPermitted = permission;
            this.Message = msg;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("LoginInfo = { ");
            sb.Append("LoginPermitted: ");
            sb.Append(this.LoginPermitted);
            sb.Append(", Message: ");
            sb.Append(this.Message);
            sb.Append(" }");

            return sb.ToString();
        }
    }
}