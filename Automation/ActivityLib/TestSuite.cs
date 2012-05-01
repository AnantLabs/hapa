using System.Activities;

namespace ActivityLib
{
    /// <summary>
    /// Only for management use, it can contain testcase and testsuite; that make recursive-able
    /// And it contains data!
    /// </summary>
    public sealed class TestSuite : ActionSet
    {
        // Define an activity input argument of type string

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        //protected void Execute(CodeActivityContext context)
        //{
        //    // Obtain the runtime value of the Text input argument
        //    string text = context.GetValue(name);
        //}
        //Idea only allow testcase and testsuite children, low priority
        public string Client { get; set; }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            //call base.CacheMetadata to add the Activities and Variables to this activity's metadata
            //base.CacheMetadata(metadata);
            //add the private implementation variable: currentIndex
            metadata.AddImplementationVariable(CurrentIndex);
            if (Name != null)
                DisplayName = "Test Suite: " + Name;
        }

        //public override XElement SetData(ActivityContext context, DataContext currentDataContext)
        //{
        //    //bool overWrite = Overwrite;
        //    CurrentDataContext.LoadData("", currentDataContext, Overwrite);
        //    //TODO how???
        //    return CurrentDataContext.Clone();
        //}
    }
}