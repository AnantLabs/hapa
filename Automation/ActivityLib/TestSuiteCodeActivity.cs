using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace ActivityLib
{
    [Designer(typeof(TestSuiteActivityDesigner))]
    public class TestSuiteCodeActivity : NativeActivity
    {
        //public Activity Body { get; set; }
        public String name { get; set; }
        public Collection<Activity> children { get; set; }
        public Collection<Variable> variables { get; set; }
        Variable<int> currentIndex;
        CompletionCallback onChildComplete;

        public TestSuiteCodeActivity():base()
        {
            this.children = new Collection<Activity>();
            this.variables = new Collection<Variable>();
            this.currentIndex = new Variable<int>();
        }
        public Collection<Activity> Activities
        {
            get
            {
                return this.children;
            }
        }

        public Collection<Variable> Variables
        {
            get
            {
                return this.variables;
            }
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            //call base.CacheMetadata to add the Activities and Variables to this activity's metadata
            //base.CacheMetadata(metadata);
            //add the private implementation variable: currentIndex 
            metadata.AddImplementationVariable(this.currentIndex);
            if (this.name != null)                
                    DisplayName ="Test Suite: "+ this.name;
            
        }

        protected override void Execute(NativeActivityContext context)
        {
            InternalExecute(context, null);
        }

        void InternalExecute(NativeActivityContext context, ActivityInstance instance)
        {
            //grab the index of the current Activity
            int currentActivityIndex = this.currentIndex.Get(context);
            if (currentActivityIndex == Activities.Count)
            {
                //if the currentActivityIndex is equal to the count of MySequence's Activities
                //MySequence is complete
                return;
            }

            if (this.onChildComplete == null)
            {
                //on completion of the current child, have the runtime call back on this method
                this.onChildComplete = new CompletionCallback(InternalExecute);
            }

            //grab the next Activity in MySequence.Activities and schedule it
            Activity nextChild = Activities[currentActivityIndex];
            context.ScheduleActivity(nextChild, this.onChildComplete);

            //increment the currentIndex
            this.currentIndex.Set(context, ++currentActivityIndex);
        }

    }
}
