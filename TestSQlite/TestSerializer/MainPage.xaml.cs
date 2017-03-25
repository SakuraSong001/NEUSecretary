using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using Windows.Storage;
using System.IO;
using SQLite.Net.Attributes;
using System.Linq;
using Windows.Foundation.Metadata;
using Windows.UI.Popups;
using System.Text;
//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace TestSerializer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 


    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private static Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Windows.Storage.StorageFile sampleFile = await localFolder.GetFileAsync("20143714_class.db");
                string contents = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                textblock.Text= contents;

            }
            catch (Exception)
            {
                textblock.Text= localFolder.Path;
            }
      
        }

        private static String DB_NAME = "20143714_class.db";
        private static String TABLE_NAME = "STUDENT";
        private static String SQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (Key TEXT,Value TEXT);";
        private static String SQL_QUERY_VALUE = "SELECT Value FROM " + TABLE_NAME + " WHERE Key = (?);";
        private static String SQL_INSERT = "INSERT INTO " + TABLE_NAME + " VALUES(?,?);";
        private static String SQL_UPDATE = "UPDATE " + TABLE_NAME + " SET Value = ? WHERE Key = ?";
        private static String SQL_DELETE = "DELETE FROM " + TABLE_NAME + " WHERE Key = ?";
        private String key;
        private String value;

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String fdlocal = ApplicationData.Current.LocalCacheFolder.Path;
            String filename = "20143714_class.db";
            String dbfullpath = Path.Combine(fdlocal, filename);
            //ISQLitePlatform platform = new SQLitePlatformWinRT();
            // SQLiteConnection conn = new SQLiteConnection(platform, dbfullpath);

            /*
            var db = new SQLiteConnection(new SQLitePlatformWinRT(), dbfullpath);

            db.CreateTable<Student>();
            Student item = new Student { id = 1, name = "Leeeeo", schoolid = 123456, title = "test" };
            db.Insert(item);
            */


            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbfullpath);
            StringBuilder sb = new StringBuilder();
            var list = conn.Table<Student>();
            foreach (var item in list)
            {
                sb.AppendLine($"{item.id} {item.name} {item.schoolid} {item.title}");
            }
            await new MessageDialog(sb.ToString()+dbfullpath).ShowAsync();
        
            /*
            conn.DeleteAll<Student>();
                Student[] stus =
                {
                    new Student {id=1,name="Leeeeo",schoolid=123456,title="test" } };
                int n = conn.InsertAll(stus);
                textblock.Text=($"已插入(n)条数据");
           

           */
           /*
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbfullpath);
            TableQuery<Student> t = conn.Table<Student>();
                var q = from s in t.AsParallel()
                        orderby s.schoolid
                        select s;
                Lv.ItemsSource = q;
          */
        
        }

        [Table("STUDENT")]
        public class Student
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
    }
}
