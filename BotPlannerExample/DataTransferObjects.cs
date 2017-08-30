using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotPlanner.Dialogs
{
    public class PlannerTask
    {
        public TaskDto[] value { get; set; }
    }

    public class TaskDto
    {
        public string planId { get; set; }
        public string bucketId { get; set; }
        public string title { get; set; }
        public string orderHint { get; set; }
        public string assigneePriority { get; set; }
        public int percentComplete { get; set; }
        public object startDateTime { get; set; }
        public DateTime createdDateTime { get; set; }
        public object dueDateTime { get; set; }
        public bool hasDescription { get; set; }
        public string previewType { get; set; }
        public object completedDateTime { get; set; }
        public object completedBy { get; set; }
        public int referenceCount { get; set; }
        public int checklistItemCount { get; set; }
        public int activeChecklistItemCount { get; set; }
        public object conversationThreadId { get; set; }
        public string id { get; set; }
    }

}