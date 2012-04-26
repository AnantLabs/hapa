using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Common;
using MongoDB;

namespace Automation.Membership
{
    public class MongoMembershipProvider : MembershipProvider
    {
        public override string ApplicationName
        {
            get
            {
                return "MongoMembershipProvider";
            }
            set
            {
                ;
            }
            
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public static MongoMembershipUser CreateUser1(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            string id = Guid.NewGuid().ToString();
            DateTime dt = DateTime.Now;

            MongoMembershipUser newUser = new MongoMembershipUser
            {
                //ApplicationId = _AppId,
                UserName = username,
                LoweredUserName = username.ToLowerInvariant(),
                //Password = EncodePassword(password, (int)PasswordFormat, passwordKey),
                Password = password,
                //EncodedPassword = EncodePassword(password, (int)PasswordFormat, passwordKey),
                //PasswordSalt = passwordKey,
                PasswordAnswer = passwordAnswer, //EncodePassword(passwordAnswer, (int)PasswordFormat, passwordKey),
                PasswordQuestion = passwordQuestion,
                //PasswordFormat = (int)PasswordFormat,
                //PasswordFormatString = PasswordFormat.ToString(),
                Email = email,
                LoweredEmail = email.ToLowerInvariant(),

                IsApproved = isApproved,
                IsLockedOut = false,
                IsAnonymous = false,

                LastActivityDate = dt,
                LastLoginDate = dt,
                LastLockoutDate = dt,
                LastPasswordChangedDate = dt,

                FailedPasswordAnswerAttemptCount = 0,
                FailedPasswordAttemptCount = 0,
                FailedPasswordAnswerAttemptWindowStart = dt,
                FailedPasswordAttemptWindowStart = dt,

                CreateDate = dt,
                Id = id,
                ParentId = "0000000000000"

            };
            
            DB.GetInstance().Add<MongoMembershipUser>(newUser);
            status = MembershipCreateStatus.Success;
            return newUser;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.ProviderError;
            return null;
            //return (MembershipUser)MongoMembershipProvider.CreateUser1(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return true; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public static MongoMembershipUser GetUser1(string username, bool userIsOnline)
        {
            return DB.GetInstance().Find<MongoMembershipUser>("UserName", username);
            //throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 6; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 1; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 10; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Clear; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return "(?=.{6,})(?=(.*\\d){1,})"; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return true; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public static bool ValidateUser1(string username, string password)
        {
            MongoMembershipUser currentUser = MongoMembershipProvider.GetUser1(username, false);
            if (currentUser == null)
                return false;
            if (currentUser.Password.Equals(password))
                return true;
            return false;
        }

        public override bool ValidateUser(string username, string password)
        {
            return MongoMembershipProvider.ValidateUser1(username, password);
        }
    }
}