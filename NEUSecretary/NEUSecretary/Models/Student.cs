using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEUSecretary.Models
{
    [Table("STUDENT")]
    class Student
    {
        [Column("ID")]
        [NotNull, PrimaryKey]
        public int id { get; set; }

        [Column("NAME")]
        [NotNull]
        public string name { get; set; }

        [Column("SCHOOLID")]
        [NotNull]
        public int schoolid { get; set; }

        [Column("TITLE")]
        public string title { get; set; }
    }

    [Table("CLASS")]
    class StudentClass
    {
        [Column("ID")]
        [NotNull, PrimaryKey]
        public int id { get; set; }

        [Column("WEEKDAY")]
        [NotNull]
        public int weekday { get; set; }

        [Column("TIME")]
        [NotNull]
        public int time { get; set; }

        [Column("CLASSINFO")]
        [NotNull]
        public string classinfo { get; set; }

    }
}
