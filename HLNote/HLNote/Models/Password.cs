using SQLite;
using SQLiteNetExtensions.Attributes;

namespace HLNote
{
    public class Password
    {
        [PrimaryKey, AutoIncrement]
        public int PasswordId { get; set; }
        public string PasswordStr { get; set; }
        public string Answer { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public Question Question { get; set; }
        [ForeignKey(typeof(Question))]
        public int QuestionId { get; set; }
    }
}
