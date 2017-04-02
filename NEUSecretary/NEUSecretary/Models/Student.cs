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

    [Table("SCORE")]
    class Scoreinfo
    {
        [Column("ID")]
        [NotNull, PrimaryKey]
        public int id { get; set; }

        [Column("CLASSPRO")]
        [NotNull]
        public string classPro { get; set; }

        [Column("CLASSNO")]
        [NotNull]
        public string classNo { get; set; }

        [Column("CLASSNAME")]
        [NotNull]
        public string className { get; set; }

        [Column("CLASSTYPE")]
        [NotNull]
        public string classType { get; set; }

        [Column("CLASSCOST")]
        [NotNull]
        public string classCost { get; set; }

        [Column("CLASSPAY")]
        [NotNull]
        public string classPay { get; set; }

        [Column("SCORETYPE")]
        [NotNull]
        public string scoreType { get; set; }

        [Column("SCORE")]
        [NotNull]
        public string score { get; set; }
    }

    [Table("GPA")]
    class GPA
    {
        [Column("ID")]
        [NotNull, PrimaryKey]
        public int id { get; set; }

        [Column("GPA")]
        [NotNull]
        public string gpa { get; set; }
    }

    [Table("ROOM")]
    class ROOM
    {
        [Column("ID")]
        [NotNull, PrimaryKey]
        public int id { get; set; }

        [Column("CLASSROOM")]
        [NotNull]
        public string classroom { get; set; }

        [Column("ROOMINFO")]
        [NotNull]
        public string roominfo { get; set; }
    }
}
