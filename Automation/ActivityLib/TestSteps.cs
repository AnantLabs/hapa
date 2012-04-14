using System.Activities;
using System.Xml.Linq;


namespace ActivityLib
{
    public class TestSteps : ActionSet
    {
        // Define an activity input argument of type string
        //public InArgument<string> Text { get; set; }

        //[Browsable(false)]
        //public InArgument<XElement> BookMark { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {
            base.Execute(context);
            //stop the workflow here, wait for the client return. And TestInstance will take care of everything else.
            //context.CreateBookmark(Id.Get<string>(context));
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            //call base.CacheMetadata to add the Activities and Variables to this activity's metadata
            //base.CacheMetadata(metadata);
            //add the private implementation variable: currentIndex
            metadata.AddImplementationVariable(CurrentIndex);
            if (Name != null)
                DisplayName = "Test Steps: " + Name;
        }

        //public override XElement SetData(ActivityContext context, DataContext currentDataContext)
        //{
        //    //throw new NotImplementedException();
        //    return null;
        //}
    }
}