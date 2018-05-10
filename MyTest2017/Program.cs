using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace MyTest2017
{
    class Program
    {
        static void Main(string[] args)
        {
            //var tt = Test();
            //Console.WriteLine("Test Start!");
            //Console.WriteLine(tt.Result);
            //Console.WriteLine("Test End!");
            //Console.ReadLine();


            //for (int i = 0; i < 5; i++)
            //{
            //    SubThread();
            //}
            //Console.WriteLine("Main thread exits.");
            //Console.ReadLine();

            //newattrtest();

            //testdic();

            //testoverride_new();

            //testSerialize();

            //test_delegate_event();

            //TestEnumToInt();

            //testLazy();

            //testSubClass(new LittleStudent());


        }

        static void testLazy()
        {
            Lazy<Student> stu = new Lazy<Student>();
            if (!stu.IsValueCreated)
                Console.WriteLine("Student isn't init!");
            Console.WriteLine(stu.Value.Name);
            stu.Value.Name = "Tom";
            stu.Value.Age = 21;
            Console.WriteLine(stu.Value.Name);
            Console.Read();
        }

        static void testSubClass(Student s)
        {
            s.Hello();
        }

        public class Student
        {
            public Student()
            {
                this.Name = "DefaultName";
                this.Age = 0;
                Console.WriteLine("Student is init...");
            }

            public void Hello()
            {
                Console.WriteLine("我要打农药");
            }

            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class LittleStudent:Student
        {
            public LittleStudent()
            {
                Console.WriteLine("LittleStudent is init...");
            }

            public string NongyaoLevel { get; set; } = "wangzhe";
        }

        static Task SubThread()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Sleep for 2 seconds.");
                Thread.Sleep(2000);
            });
        }

        static async Task<int> Test()
        {
            var aa = Test1();
            await aa;
            Console.WriteLine("Test1 End!");
            
            return  aa.Result;
        }


        static Task<int> Test1()
        {
            Thread.Sleep(1000);
            Console.WriteLine("create task in test1");

            return Task.Run(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("Test1");
                return 99;
            });
        }

        static void TestEnumToInt()
        {
            Console.Write((int) EnumToInt.Three);
        }

        enum EnumToInt
        {
            One = 1,
            Three = 3
        }

        static void newattrtest()
        {

            SS[] test = {null};
            SS a = null;
            
            Console.Write(test?[0]?.Name);
        }

        class SS
        {
            public string Name { get; set; }
        }

        static void test_delegate_event()
        {
            Console.WriteLine("场景开始了.");
            //生成小王

            小王 w = new 小王();
            //生成小账
            小张 z = new 小张();

            //指定监视
            z.PlayGame += new PlayGameHandler(w.扣钱);

            //开始玩游戏
            z.玩游戏();

            Console.WriteLine("场景结束");
            Console.ReadLine();
        }


        static void testdic()
        {
            //IDictionary<object, object> dic = new Dictionary<object, object> {{"xx", "yy"}};
            //HashSet<object> hashset = new HashSet<object> {};
            //hashset.Add("xx");
            //Console.WriteLine(dic["xxx"]);

            //如果指定键未找到，则 Get 操作引发 System.Collections.Generic.KeyNotFoundException，而Set 操作创建一个带指定键的新元素。
            var dic = new Dictionary<string, string> { { "xx", "yy" } };
            dic["x"] = "y";
            Console.WriteLine(dic["x"]);
        }

        static void testoverride_new()
        {
            Car car1 = new Car();
            car1.DescribeCar();
            System.Console.WriteLine("----------");


            Car car2 = new ConvertibleCar();
            car2.DescribeCar();
            System.Console.WriteLine("----------");


            Car car3 = new Minivan();
            car3.DescribeCar();
            System.Console.WriteLine("----------");
            System.Console.ReadKey();
        }

        // Define the base class
        class Car
        {
            public virtual void DescribeCar()
            {
                System.Console.WriteLine("Four wheels and an engine.");
            }
        }
        // Define the derived classes
        class ConvertibleCar : Car
        {
            public new void DescribeCar()
            {
                System.Console.WriteLine("A roof that opens up.");
            }
        }


        class Minivan : Car
        {
            public override void DescribeCar()
            {
                System.Console.WriteLine("Carries seven people.");
            }
        }

        static void testSerialize()
        {
            var obj = new SerializeObject() { Name = "xx", Age = 10 };
            var xx = new TestSerialize<SerializeObject>();
            var yy = xx.Serialize(obj);
            Console.WriteLine(yy);
        }

    }

    public abstract class Animal
    {
    }

    public class Dog : Animal
    {
    }

    internal class C
    {
        public int GetC()
        {
            return 3;
        }
    }


    public interface IMyList<out T>

    {

        T GetElement();

        //void ChangeT(T t);
    }

    

    public class TestSerialize<T>
    {
        public string Serialize(T t)
        {
            string result = "";
            SerializeObject objcopy = null;
            var xmlformat = new XmlSerializer(typeof(SerializeObject));
            var binformat = new BinaryFormatter();

            //xml-stream,序列化流
            using (var st = new MemoryStream())
            {
                xmlformat.Serialize(st, t);
                st.Position = 0;
                objcopy = xmlformat.Deserialize(st) as SerializeObject;

                st.Position = 0;
                byte[] temp = new byte[1024 * 1024 * 4];
                st.Read(temp, 0, temp.Length);
                result = Encoding.Default.GetString(temp);
            }


            //xml-textwriter-textreader，序列化字符串
            using (var sw = new StringWriter())
            {
                xmlformat.Serialize(sw, t);
                result = sw.ToString();


                using (var sr = new StringReader(result))
                {
                    objcopy = xmlformat.Deserialize(sr) as SerializeObject;
                }


            }

            //json+simplejson
            result = SimpleJsonHelper.JsonSerializer(t);
            objcopy = SimpleJsonHelper.JsonDeserialize<SerializeObject>(result);
            return result;

        }
    }

    [Serializable]
    //[DataContract]
    public class SerializeObject
    {
        //[DataMember]
        public string Name { get; set; }

        //[DataMember]
        public int Age { get; set; }
    }


    public class JsonHelper
     {
         /// <summary>
         /// JSON序列化
         /// </summary>
         public static string JsonSerializer<T>(T t)
         {
             DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
             MemoryStream ms = new MemoryStream();
             ser.WriteObject(ms, t);
             string jsonString = Encoding.Default.GetString(ms.ToArray());
             ms.Close();

             return jsonString;
         }
  
         /// <summary>
         /// JSON反序列化
         /// </summary>
         public static T JsonDeserialize<T>(string jsonString)
         {
             DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
             MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(jsonString));
             T obj = (T)ser.ReadObject(ms);
             return obj;
         }
     }

    public class SimpleJsonHelper
    {
        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            var result = new JavaScriptSerializer().Serialize(t);
            return result;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            T result = new JavaScriptSerializer().Deserialize<T>(jsonString);
            return result;
        }
    }


    //http://topic.csdn.net/t/20040903/17/3338252.html
    //定义委托处理程序
    public delegate void PlayGameHandler(object sender, System.EventArgs e);


    //   负责扣钱的人   
    public class 小王
    {
        public 小王()
        {
            Console.WriteLine("生成小王");
        }

        public void 扣钱(object sender, EventArgs e)
        {
            Console.WriteLine("小王：好小子，上班时间胆敢玩游戏");
            Console.WriteLine("小王：看看你小子有多少钱");
            小张 f = (小张)sender;
            Console.WriteLine("小张的钱：" + f.钱.ToString());
            Console.WriteLine("开始扣钱");
            System.Threading.Thread.Sleep(500);
            f.钱 = f.钱 - 500;
            Console.WriteLine("扣完了.现在小张还剩下：" + f.钱.ToString());
        }
    }

    //如果玩游戏，则引发事件
    public class 小张
    {
        //先定义一个事件，这个事件表示“小张”在玩游戏。
        public event PlayGameHandler PlayGame;
        //保存小张钱的变量   
        private int m_Money;

        public 小张()
        {
            Console.WriteLine("生成小张.");
            m_Money = 1000;   //构造函数，初始化小张的钱。
        }

        public int 钱   //此属性可以操作小张的钱。   
        {
            get
            {
                return m_Money;
            }
            set
            {
                m_Money = value;
            }
        }

        public void 玩游戏()
        {
            Console.WriteLine("小张开始玩游戏了..");
            Console.WriteLine("小张:CS好玩，哈哈哈！   我玩..");
            System.Threading.Thread.Sleep(500);
            System.EventArgs e = new EventArgs();
            OnPlayGame(e);
        }

        protected virtual void OnPlayGame(EventArgs e)
        {
            if (PlayGame != null)
            {
                PlayGame(this, e);
            }
        }
    }

}
