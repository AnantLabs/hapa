using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using BaseLib;
using BaseLib.TestDataServiceReference;

namespace ActivityLib
{

    public class LogActivity : CodeActivity<string>
    {
        // Define an activity input argument of type string

        public InArgument<string> Content { get; set; }
        public InArgument<string> Type { get; set; }
        public InArgument<string> Name { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override string Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            string content = Content.Get(context);
            string type = Type.Get(context);
            string name = Name.Get(context);
            TestDataWebServiceSoapClient client =  TestDataServiceContext.getInstance().getClient();
            string id = client.InsertData(type, name, content);
            TestDataServiceContext.getInstance().returnClient(client);
            client.Close();
            return id;
        }
    }
}
