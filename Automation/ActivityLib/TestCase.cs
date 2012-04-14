using System.Activities;
using System.Xml.Linq;


namespace ActivityLib
{
    public sealed class TestCase : ActionSet
    {
        // Define an activity input argument of type string
        //public InArgument<string> Text { get; set; }

        public string Client { get; set; }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            //call base.CacheMetadata to add the Activities and Variables to this activity's metadata
            //base.CacheMetadata(metadata);
            //add the private implementation variable: currentIndex
            metadata.AddImplementationVariable(CurrentIndex);
            if (Name != null)
                DisplayName = "Test Case: " + Name;
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