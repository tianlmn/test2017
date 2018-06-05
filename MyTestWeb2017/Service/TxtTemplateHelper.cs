using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyTestWeb2017.Service
{
    public class TxtTemplateHelper: IDisposable
    {
        //对一批源txt数据队列进行处理的操作
        private readonly Action<IList<string>> _processor;

        //一次处理的txt数据行数
        private readonly int _batchSize;

        //文件流
        private StreamReader _sr;

        public TxtTemplateHelper(HttpPostedFileBase file, Action<IList<string>> processor, int batchSize = 1000)
        {
            _processor = processor;
            _batchSize = batchSize;

            if (file != null && file.ContentLength > 0)
            {
                _sr = new StreamReader(file.InputStream);
            }
        }

        public void Run()
        {
            if (_sr != null)
            {
                string line;
                int count = 0;
                var strList = new List<string>();
                while ((line = _sr.ReadLine()) != null)
                {
                    count++;
                    strList.Add(line);

                    if (count == _batchSize)
                    {
                        _processor(strList);
                        count = 0;
                        strList.Clear();
                    }
                }

                if (strList.Count > 0)
                {
                    _processor(strList);
                }
            }
        }

        public void Dispose()
        {
            if (_sr != null)
            {
                _sr.Close();
                _sr = null;
            }
        }
    }
}