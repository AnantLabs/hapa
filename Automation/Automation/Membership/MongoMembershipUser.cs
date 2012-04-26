using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Automation.Membership
{
    public class MongoMembershipUser 
    {
        public string _id { get; set; }
        public string ParentId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastActivityDate { get; set; }

        public string MobilePin { get; set; }
        public string Email { get; set; }
        public string LoweredEmail { get; set; }


        public string Password { get; set; }
        public string EncodedPassword { get; set; }
        public int PasswordFormat { get; set; }
        public string PasswordFormatString { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }

        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }


        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastLockoutDate { get; set; }

        public int FailedPasswordAttemptCount { get; set; }
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        public string Comment { get; set; }


        internal bool ChangePassword(string p1, string p2)
        {
            if (!Password.Equals(p1))
                return false;
            Password = p2;
            MongoDB.DB.GetInstance().Update<MongoMembershipUser>(this);
            return true;
        }
    }
}