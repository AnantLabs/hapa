using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class Const
    {
        public enum ResultType
        {
            Success = 0,
            Failed = 1,
            Warning = 2,
            Stopped = 4,
            Unevaluated = 8,
            Fatal = 16
        }
        public enum ResultReaction
        {
            Continue = 0,
            Terminate = 64,
            RecordError = 1,
            RecordWarning = 2,
            StopCurrentTest = 4
        }
        public enum InstanceStatus
        {
            Ready = 0,
            Running = 1,
            Pause = 2,
            Stopped = 4,
            Terminate = 8,
            Finished = 16,
            SavingResult = 32
        }

        public enum ClientStatus
        {
            Registered = 0,
            Controlled = 1
        }

        public const string AttributeName = "Name";
        public const string AttributeIcon = "Icon";
        public const string AttributeId = "_id";
        public const string AttributeParentId = "ParentId";
        public const string AttributeDescription = "Description";
        
    }
}
