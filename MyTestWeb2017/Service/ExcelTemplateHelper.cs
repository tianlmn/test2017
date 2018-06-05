using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MyTestWeb2017.Service
{
    /// <summary>
    /// excel处理帮助类
    /// </summary>
    /// <typeparam name="T">excel的源数据类型</typeparam>
    public class ExcelTemplateHelper<T>:IDisposable
        where T : new()
    {
        //excel文件类
        private IWorkbook _workbook = null;

        //处理的sheet开始项
        private readonly int _sheetstart;

        //处理的sheet数量
        private readonly int _sheetcount;
        
        //处理的sheet顺序
        private readonly bool _isbysequence;

        //对一批源excel数据队列进行处理的操作
        private readonly Action<IList<T>> _processor;

        //一次处理的excel数据行数
        private readonly int _batchSize;

        /// <summary>
        /// 构造excel处理类
        /// </summary>
        /// <param name="file">posted文件类</param>
        /// <param name="processor">处理器，处理一行excel数据</param>
        /// <param name="sheetcount">要处理的sheet的表格数量，默认1</param>
        /// <param name="isbysequence">要处理的sheet表格的顺序，默认逆序</param>
        /// <param name="sheetstart">要处理的sheet的表格开始，默认从第0张开始处理</param>
        /// <param name="batchSize">一次处理的excel行数，默认一次500行</param>
        public ExcelTemplateHelper(HttpPostedFileBase file, Action<IList<T>> processor, int sheetcount = 1, bool isbysequence = true, int sheetstart = 0, int batchSize = 500)
        {
            _processor = processor;
            _sheetstart = sheetstart;
            _batchSize = batchSize;
            _sheetcount = sheetcount;
            _isbysequence = isbysequence;
            string fileName = file.FileName;

            var temp = new byte[file.ContentLength];
            file.InputStream.Read(temp, 0, temp.Length);
            file.InputStream.Seek(0, SeekOrigin.Begin);
            using (var sr = new MemoryStream(temp))
            {
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                {
                    _workbook = new XSSFWorkbook(sr);
                }
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                {
                    _workbook = new HSSFWorkbook(sr);
                }
            }
        }


        /// <summary>
        /// Excel处理主方法
        /// </summary>
        public void Run()
        {
            ISheet sheet = null;
            int line = 0;
            var type = typeof(T);
            
            //做多处理20列数据
            var tableHead = new string[20];

            //循环处理每个sheet
            for (int k = _sheetstart; k <_sheetstart+ _sheetcount; k++)
            {
                IList<T> sourceList = new List<T>();
                
                //正序表示从sheet0开始处理，逆序表示从sheetn-1开始处理
                int index = (_isbysequence ? k : (_sheetstart+_sheetcount - 1 - k));
                if (_workbook != null)
                {
                    sheet = _workbook.GetSheetAt(index);
                }

                //处理sheet
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        var cell = firstRow.GetCell(i).ToString();
                        if (!string.IsNullOrWhiteSpace(cell))
                        {
                            string cellValue = cell;
                            if (cellValue != null)
                            {
                                tableHead[i] = cellValue;
                            }
                        }
                    }
                    const int startRow = 1;



                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        //excel源数据类型
                        var o = new T();
                        try
                        {
                            //对excel的一行进行处理，如果遇到异常数据，记录clog，然后跳过该行数据
                            for (int j = 0; j < cellCount; ++j)
                            {
                                if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                {
                                    var propertyName = tableHead[j];
                                    var property = type.GetProperty(propertyName);

                                    //转化数据
                                    if (property != null)
                                    {
                                        var str = row.GetCell(j).ToString();
                                        var t = property.PropertyType;
                                        var v = System.ComponentModel.TypeDescriptor.GetConverter(t).ConvertFrom(str);
                                        property.SetValue(o, v, null);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        
                        line++;
                        sourceList.Add(o);

                        //处理一批数据
                        if (_batchSize>0 && line == _batchSize)
                        {
                            line = 0;
                            _processor(sourceList);
                            sourceList.Clear();
                        }
                    }

                    //处理最后一批数据
                    if (sourceList.Any())
                    {
                        _processor(sourceList);
                    }
                }
            }
            
        }

        public void Dispose()
        {
            _workbook?.Close();
            _workbook = null;
            GC.Collect();
            Thread.Sleep(500);
        }
    }
}
