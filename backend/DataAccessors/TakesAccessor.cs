using System.Threading.Tasks;
using System.Text;
using System;
using SyaApi.Plugins;
using SyaApi.Entities;

namespace SyaApi.DataAccessors
{
    public static class TakesAccessor
    {

        ///<summery>
        /// 创建takes,由ApplyController.ProManageApp调用
        /// work_time, absent_num, absent_time 自动设为0
        /// dumei 08.24
        ///</summery>
        public static async Task<int> Create(TakesEntity takes)
        {
            var query = "INSERT INTO takes(student_id,work_id,work_time,absent_num,absent_time) VALUES(@student_id,@work_id,@work_time,@absent_num,@absent_time)";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@student_id",takes.student_id);
            command.Parameters.AddWithValue("@work_id",takes.work_id);
            command.Parameters.AddWithValue("@work_time",0);
            command.Parameters.AddWithValue("@absent_num",0);
            command.Parameters.AddWithValue("@absent_time",0);

            await command.ExecuteNonQueryAsync();
            return (int)command.LastInsertedId;
        }

        ///<summery>
        /// 查询student工作总时长和请假总次数
        /// dumei 08.24
        ///</summery>
        public static async Task<SumWorkAndAbsent> GetSumOfWorkAndAbsent(int stu_id)
        {
            var query = @"SELECT sum(work_time) AS sum_work_time, 
            CAST(sum(absent_num) AS SIGNED) AS sum_absent_num 
            FROM takes WHERE student_id=@id";

            using var connection = DatabaseConnector.Connect();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", stu_id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                SumWorkAndAbsent temp = new SumWorkAndAbsent()
                {
                    sum_work_time = reader["sum_work_time"] is System.DBNull ? 0 : reader.GetDouble("sum_work_time"),
                    sum_absent_num = reader["sum_absent_num"] is System.DBNull ? 0 : Convert.ToInt32(reader.GetInt64("sum_absent_num"))
                    
                };
                return temp;
            }
            return null; // the student have no takes
        }


    }
}