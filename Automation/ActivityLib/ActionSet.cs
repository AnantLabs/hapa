using System;
using System.Activities;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Action = ActivityLib.Activities.Action;

namespace ActivityLib
{
    [Designer(typeof (ActionSetDesigner))]
    public abstract class ActionSet : Action
    {
        protected Variable<int> CurrentIndex;
        protected CompletionCallback OnChildComplete;

        protected ActionSet()
        {
            Activities = new Collection<Activity>();
            Variables = new Collection<Variable>();
            CurrentIndex = new Variable<int>();
        }

        public String Name { get; set; }

        public Collection<Activity> Activities { get; set; }

        public Collection<Variable> Variables { get; set; }

        // for normal Property, it will be a checkbox; for InArgument, it will be a textbox.

        [DisplayName(@"Can Overwrite?")]
        public bool Overwrite { get; set; }

        /// <summary>
        /// It is a link to data, on release version, it should be readonly(only allow drop data into it)
        /// for debug reason, we allow read-write.
        /// </summary>
        [Editor(typeof (TreeNodePicker), typeof (DialogPropertyValueEditor))]
        public String Data { get; set; }

        //public InArgument<string> Data { get; set; }

        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        protected override void Execute(NativeActivityContext context)
        {
            InternalExecute(context, null);
            string bookmarkId = Guid.NewGuid().ToString();
            //TODO set bookmarkId to Xelement, set command to computer here
            context.CreateBookmark(bookmarkId, SetResult);
        }

        private void SetResult(NativeActivityContext context, Bookmark bookmark, Object obj)
        {
            //TODO set result here
        }

        private void InternalExecute(NativeActivityContext context, ActivityInstance instance)
        {
            //grab the index of the current Activity
            int currentActivityIndex = CurrentIndex.Get(context);
            if (currentActivityIndex == Activities.Count)
            {
                //if the currentActivityIndex is equal to the count of MySequence's Activities
                //MySequence is complete

                //generate the xml for actionset, send it to communication manager, observe the result
                //when get the result, send it to observers
                return;
            }

            if (OnChildComplete == null)
            {
                //on completion of the current child, have the runtime call back on this method
                OnChildComplete = InternalExecute;
            }

            //grab the next Activity in MySequence.Activities and schedule it
            Activity nextChild = Activities[currentActivityIndex];
            if (nextChild is Action)
            {
                //TODO get all children element here
                //XElement nextChildElement = ((Action)nextChild).SetData(context, CurrentDataContext);
                ;
            }
            context.ScheduleActivity(nextChild, OnChildComplete);

            //increment the currentIndex
            CurrentIndex.Set(context, ++currentActivityIndex);
        }
    }
}