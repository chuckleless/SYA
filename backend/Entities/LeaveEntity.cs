using System.Collections.Generic;

namespace SyaApi.Entities
{
    public class LeaveEntity
    {
        public int leave_id{get; set;}

        public int student_id{get; set;}

        public int work_id{get; set;}
        
        public string student_name{get; set;}

        public string work_name{get; set;}

        public string content{get; set;}

        //public string leave_time{get; set;} //请假的开始时间

        public string proof{get; set;}

        public int status{get; set;}

        public System.DateTime request_time{get; set;} //申请时间

        public double leave_duration{get; set;} //请假的持续时长

        public string leave_day{get; set;}

        public string leave_start{get; set;}

        public string leave_end{get; set;}
    }
    
    public class LeaveItemEntity
    {
        public int total{get; set;}
        public List<LeaveEntity> leaveItem{get;set;}

    }
}