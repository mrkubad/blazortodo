using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApplication1.Data
{
    public class WeatherForecastService
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }
        public WeatherForecastService()
        {

        }
        public Task<bool> CreateListinDB(string ListName)
        {
            bool result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("INSERT INTO `lists` (`ListName`) VALUES (\"{0}\")", ListName);
                result = db.InsertQuery() > -1;
            }
            return Task.FromResult(result);
        }
        public Task<List<int>> GetListsIdsFromDb()
        {
            List<int> result = null;

            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = "SELECT `Id` FROM `lists`";
                result = db.SelectQueryForSingleColumn<int>();
            }
            return Task.FromResult(result);
        }

        public Task<bool> CheckIfListNameExist(string ListName)
        {
            bool result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT COUNT(`Id`) > 0 AS 'IsTableContain' FROM `lists` WHERE `ListName` = '{0}'", ListName);
                result = db.SelectQueryForSingleValue<bool>();
            }
            return Task.FromResult(result);
        }

        public Task<string> GetListNameFromNumber(int listNumber)
        {
            string result = null;
            using(DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `ListName` FROM `lists` WHERE `id`={0}", listNumber);
                result = db.SelectQueryForSingleValue<string>();
            }
            return Task.FromResult(result);
        }
        public Task<bool> SetListNameFromNumber(int listNumber, string listName)
        {
            bool result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("UPDATE `lists` SET `ListName`=\"{0}\" WHERE `Id`={1}", listName, listNumber);
                result = db.InsertQuery() > -1;
            }
            return Task.FromResult(result);
        }
        public Task<List<int>> GetTasksIdsListContains(int ListNumber)
        {
            List<int> result = new List<int>();
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `Id` FROM `tasks` WHERE `ListId`={0}", ListNumber);
                //result.AddRange(db.SelectQuery<string>().Select(row => new Tuple<int, string>(int.Parse(row["Id"]), row["TaskName"])));
                result.AddRange(db.SelectQueryForSingleColumn<int>());
            }
            return Task.FromResult(result);
        }

        public Task<int> DeleteList(int ListNumber)
        {
            int result = -1;

            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("DELETE FROM `lists` WHERE `lists`.`Id` = {0};", ListNumber);
                result = db.DeleteQuery();
            }

            return Task.FromResult(result);
        }
        public Task<int> AddNewTask(int ListNumber, string TaskName)
        {
            int result = -1;

            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("INSERT INTO `tasks`(`TaskName`, `ListId`) VALUES (\"{0}\",{1});", TaskName, ListNumber);
                if (db.InsertQuery() > 0)
                {
                    db.Query = "SELECT LAST_INSERT_ID();";
                    result = db.SelectQueryForSingleValue<int>();
                }

            }
            return Task.FromResult(result);
        }
        public Task<bool> UpdateTaskState(int TaskId, bool TaskState)
        {
            int queryResult = -1;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = TaskState
                           ? string.Format("UPDATE `tasks` SET `Done`= {0}, `CompletionDate` = CURRENT_TIMESTAMP WHERE `Id` = {1}", TaskState, TaskId)
                           : string.Format("UPDATE `tasks` SET `Done`= {0}, `CompletionDate` = NULL WHERE `Id` = {1}", TaskState, TaskId);
                queryResult = db.UpdateQuery();
            }
            return Task.FromResult(queryResult > -1 ? true : false);
        }
        public Task<bool> GetTaskState(int TaskId)
        {
            bool result = false;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `Done` FROM `tasks` WHERE `Id` = {0}", TaskId);
                result = db.SelectQueryForSingleValue<bool>();
            }
            return Task.FromResult(result);
        }
        public Task UpdateTaskImportance(int TaskId, bool IsImportant)
        {
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("UPDATE `tasks` SET `IsImportant`= {0} WHERE `Id` = {1}", IsImportant, TaskId);
                db.UpdateQuery();
            }
            return Task.CompletedTask;
        }
        public Task<bool> GetTaskInportance(int TaskId)
        {
            bool result = false;

            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `IsImportant` FROM `tasks` WHERE `Id` = {0}", TaskId);
                result = db.SelectQueryForSingleValue<bool>();
            }
            return Task.FromResult(result);
        }
        public Task<int> GetNumberOfUncompletedTasksFromList(int ListNumber)
        {
            int result = 0;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT COUNT(`Done`) FROM `tasks` WHERE `ListId` = {0} AND `Done` = 0", ListNumber);
                result = db.SelectQueryForSingleValue<int>();
            }
            return Task.FromResult(result);
        }
        public Task DeleteTask(int TaskId)
        {
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("DELETE FROM `tasks` WHERE `Id` = {0};", TaskId);
                db.DeleteQuery();
            }
            return Task.CompletedTask;
        }

        public Task<DateTime> GetTaskCreationDate(int TaskId)
        {
            DateTime result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `CreationDate` FROM `tasks` WHERE `Id` = {0}", TaskId);
                result = db.SelectQueryForSingleValue<DateTime>();
            }
            return Task.FromResult(result);
        }

        public Task<DateTime> GetTaskComplitionDate(int TaskId)
        {
            DateTime result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `CompletionDate` FROM `tasks` WHERE `Id` = {0}", TaskId);
                result = db.SelectQueryForSingleValue<DateTime>();
            }
            return Task.FromResult(result);
        }

        public Task<string> GetTaskNameFromId(int TaskId)
        {
            string result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `TaskName` FROM `tasks` WHERE `Id` = {0}", TaskId);
                result = db.SelectQueryForSingleValue<string>();
            }
            return Task.FromResult(result);
        }


        public Task<List<int>> GetStepsIds(int TaskId)
        {
            List<int> result = new List<int>();
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `Id` FROM `steps` WHERE `TaskId` = {0}", TaskId);
                result.AddRange(db.SelectQueryForSingleColumn<int>());
            }
            return Task.FromResult(result);
        }

        public Task<string> GetStepName(int StepId)
        {
            string result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `StepName` FROM `steps` WHERE `Id` = {0}", StepId);
                result = db.SelectQueryForSingleValue<string>();
            }
            return Task.FromResult(result);
        }

        public Task<bool> GetStepExecutionState(int StepId)
        {
            bool result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("SELECT `IsDone` FROM `steps` WHERE `Id` = {0}", StepId);
                result = db.SelectQueryForSingleValue<bool>();
            }
            return Task.FromResult(result);
        }

        public Task<int> DeleteStep(int StepId)
        {
            int result = -1;

            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("DELETE FROM `steps` WHERE `steps`.`Id` = {0};", StepId);
                result = db.DeleteQuery();
            }
            return Task.FromResult(result);
        }

       public Task<bool> UpdateStepExecutionState(int StepId, bool StepState)
        {
            int queryResult = -1;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = StepState
                           ? string.Format("UPDATE `tasks` SET `Done`= 1, `CompletionDate` = CURRENT_TIMESTAMP WHERE `Id` = {0}", StepId)
                           : string.Format("UPDATE `tasks` SET `Done`= 0, `CompletionDate` = NULL WHERE `Id` = {0}", StepId);
                queryResult = db.UpdateQuery();
            }
            return Task.FromResult(queryResult > -1 ? true : false);
        }

        public Task<bool> UpdateTaskName(int TaskId, string NewTaskName)
        {
            bool result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("UPDATE `tasks` SET `TaskName`=\"{0}\" WHERE `Id`={1}", NewTaskName, TaskId);
                result = db.InsertQuery() > -1;
            }
            return Task.FromResult(result);
        }
        public Task<bool> UpdateStepName(int StepId, string NewStepName)
        {
            bool result;
            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("UPDATE `steps` SET `StepName`=\"{0}\" WHERE `Id`={1}", NewStepName, StepId);
                result = db.InsertQuery() > -1;
            }
            return Task.FromResult(result);
        }
        public Task<int> AddNewStep(int TaskNumber, string StepName)
        {
            int result = -1;

            using (DataBaseConnection db = new DataBaseConnection())
            {
                db.Query = string.Format("INSERT INTO `steps`(`StepName`, `TaskId`) VALUES (\"{0}\",{1});", StepName, TaskNumber);
                if (db.InsertQuery() > 0)
                {
                    db.Query = "SELECT LAST_INSERT_ID();";
                    result = db.SelectQueryForSingleValue<int>();
                }

            }
            return Task.FromResult(result);
        }


    }
}
