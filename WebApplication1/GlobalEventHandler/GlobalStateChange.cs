using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.GlobalEventHandler
{
    public enum TaskEventType
    {
        Deletion,
        ImportantStateChange,
        NameChange,
        ExecutionStateChange
    }

    public enum ListEventType
    {
        NameChange,
        TaskLeftNumberChange
    }

    //public class SidebarEventArgs
    //{

    //}

    public class TaskEventArgs
    {
        public int TaskId { get; }
        public bool IsStep { get; }
        public TaskEventType Type { get; }

        
        public TaskEventArgs(int TaskId, bool IsStep, TaskEventType Type)
        {
            this.TaskId = TaskId;
            this.IsStep = IsStep;
            this.Type = Type;
        }
    }

    public class ListEventArgs
    {
        public int ListId { get; }
        public ListEventType Type { get; }

        public ListEventArgs(int ListId, ListEventType Type)
        {
            this.ListId = ListId;
            this.Type = Type;
        }

    }



    public class GlobalStateChange
    {
        public void InvokeStateChange()
        {
            this.StateHasChanged?.Invoke(this, new EventArgs());
        }
        public void InvokeRefreshNewListUrl()
        {
            this.RefreshNewListUrl?.Invoke(this, new EventArgs());
        }
        public void InvokeChangeRightSidebarVisibility(int clickedTaskId)
        {
            ChangeRightSidebarVisibility?.Invoke(this, clickedTaskId);

        }
        public void InvokeChangeLeftSidebarVisibility()
        {
            ChangeLeftSidebarVisibility?.Invoke(this, false);
        }
        //public void InvokeRefreshMainTable()
        //{
        //    //RefreshMainTable?.Invoke(this, false);
        //}
        public void InvokeSomethingChangedTask(object sender, int TaskId, bool IsStep, TaskEventType Type)
        {
            TaskSomethingChanged?.Invoke(sender, new TaskEventArgs(TaskId, IsStep, Type));
        }


        public void InvokeListChangedEvent(object sender, int ListId, ListEventType Type)
        {
            ListSomethingChanged?.Invoke(sender, new ListEventArgs(ListId, Type));
        }

        public event EventHandler StateHasChanged;
        public event EventHandler RefreshNewListUrl;
        public event EventHandler<int> ChangeRightSidebarVisibility;
        public event EventHandler<bool> ChangeLeftSidebarVisibility;
        //public event EventHandler<bool> RefreshMainTable;
        public event EventHandler<TaskEventArgs> TaskSomethingChanged;
        public event EventHandler<ListEventArgs> ListSomethingChanged;
    }
}
