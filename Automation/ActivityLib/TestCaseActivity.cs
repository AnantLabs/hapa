using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;

namespace ActivityLib
{
    [Designer(typeof(TestCaseActivityDesigner))]
    public class TestCaseActivity : CodeActivity
    {
        // Define an activity input argument of type string
        [RequiredArgument]
        public InArgument<string> TestCaseName { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            
            // Obtain the runtime value of the Text input argument
            string text = context.GetValue(this.TestCaseName);
            Debug.WriteLine("Executing TestCase " + TestCaseName);
        }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (this.TestCaseName != null)
                if (this.TestCaseName.Expression != null)
                    DisplayName = this.TestCaseName.Expression.ToString();
        }
        
    }
}
