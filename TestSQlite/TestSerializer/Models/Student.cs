using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSerializer.Models
{
    class Student
    {
        /// <summary>

        /// 自增主键

        /// </summary>

        [AutoIncrement, PrimaryKey,NotNull]

        public int ID { get; set; }



        /// <summary>

        /// 姓名

        /// </summary>
        [NotNull]
        public string NAME { get; set; }

        /// <summary>

        /// 年龄 不为空

        /// </summary>

        [NotNull]

        public int SCHOOLID { get; set; }



        /// <summary>

        /// 地址

        /// </summary>

        public string TITLE { get; set; }

    }
}
