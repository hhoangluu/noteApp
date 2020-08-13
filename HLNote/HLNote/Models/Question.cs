using SQLite;

namespace HLNote
{
     public class Question
    {
        [PrimaryKey, AutoIncrement]
        public int QuestionId { get; set; }
        public string QuestionStr { get; set; }
    }
}
