namespace Common
{
    public static class Const
    {
        #region ClientStatus enum

        public enum ClientStatus
        {
            Registered = 0,
            Controlled = 1
        }

        #endregion

        #region InstanceStatus enum

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

        #endregion

        #region ResultReaction enum

        public enum ResultReaction
        {
            Continue = 0,
            Terminate = 64,
            RecordError = 1,
            RecordWarning = 2,
            StopCurrentTest = 4
        }

        #endregion

        #region ResultType enum

        public enum ResultType
        {
            Success = 0,
            Failed = 1,
            Warning = 2,
            Stopped = 4,
            Unevaluated = 8,
            Fatal = 16
        }

        #endregion

        public const string AttributeName = "Name";
        public const string AttributeIcon = "Icon";
        public const string AttributeId = "Id";
        public const string AttributeParentId = "ParentId";
        public const string AttributeDescription = "Description";

        public const int PauseAfterRegisterFailure = 17000; // in ms
    }
}