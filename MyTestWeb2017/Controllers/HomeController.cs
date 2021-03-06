﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyTest2017;
using System.IO.Compression;
using MyTestWeb2017.Business.HpaReviewFeed;
using MyTestWeb2017.framework;
using MyTestWeb2017.Models;
using MyTestWeb2017.Models.HpaReviewFeed;
using MyTestWeb2017.Models.RequestModels;
using MyTestWeb2017.Service;

namespace MyTestWeb2017.Controllers
{
    public class HomeController : Controller
    {
        private ISerializer Serializer = new DotNetXmlSerializer();
        public ActionResult Index(BaseRequestModel request)
        {
            //var testAsync = new AsyncTest();
            //testAsync.Test();
            //HomeService.testhttpcontext(this.HttpContext);

            request.MHString = MvcHtmlString.Create("<strong>strong</strong>");


            ViewData.Model = request;
            return new ViewResult()
            {
                ViewName="Test/Index1",
                ViewData = ViewData
            };

        }

        public ActionResult Test()
        {
            //并行调用partner接口

            var numList = new List<int>();
            var resultConcurrentBag = new ConcurrentBag<int>();
            var threadBag = new ConcurrentBag<int>();
            for (var i = 0; i < 10; i++)
            {
                numList.Add(i);
            }
            Parallel.ForEach(numList,(item, loopState) => Threadwork(item, resultConcurrentBag));
            return View("Test");
        }
         
        public ActionResult TestStrBit()
        {
            char[] s = "123456".ToArray();

            return View("Test");
        }

        public void Threadwork(int item,ConcurrentBag<int> resultConcurrentBag)
        {
            {
                Thread.Sleep(1000);
                resultConcurrentBag.Add(item);
                Thread.Sleep(20000);
            }
        }

        public ActionResult GetFileZip(BaseRequestModel request)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = $"{DateTime.Now}.xml";//客户端保存的文件名

                    var ss = string.Empty;
                    EnglishFeedFile reviewList = null;

                    var uploadType = RequestHelper.GetStringFromParameters("UploadType");
                    switch (uploadType)
                    {
                        case "qcxml":
                            ss = DealQcXml(file);
                            break;
                        case "feedexcel":
                            ss = DealFeedExcel(file);
                            break;
                        case "feedxml":
                            fileName = $"{file.FileName}_{DateTime.Now}.xls";
                            ss = DealFeedXml(file);
                            break;
                        case "reviewfeed":
                            reviewList = DealReviewFeed(Request.Files);
                            break;
                    }

                    Response.ContentType = "application/octet-stream";
                    //通知浏览器下载文件而不是打开
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    //Response.BinaryWrite(Encoding.UTF8.GetBytes(ss));
                    WriteBigFile(reviewList);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("GetFileZip");
        }

        private void WriteBigFile(EnglishFeedFile reviewList)
        {
            byte[] fileBuffer = new byte[1024];//每次读取1024字节大小的数据
            if (reviewList != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    GC.Collect();
                    Thread.Sleep(1000);
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ReviewListingsType));
                    serializer.Serialize(stream, reviewList.GetReviewList());

                    long totalLength = stream.Length;
                    stream.Seek(0,SeekOrigin.Begin);
                    while (totalLength > 0 && Response.IsClientConnected)//持续传输文件
                    {
                        int length = stream.Read(fileBuffer, 0, fileBuffer.Length);//每次读取1024个字节长度的内容
                        Response.OutputStream.Write(fileBuffer, 0, length);//写入到响应的输出流
                        totalLength = totalLength - length;
                    }
                }
            }

        }

        private string DealFeedXml(HttpPostedFileBase file)
        {
            var sb = new StringBuilder();
            using (var read = new StreamReader(file.InputStream))
            {
                var ss = read.ReadToEnd();
                var entity = Serializer.Deserialize<ListingsType>(ss) as ListingsType;

                var objList = entity?.ListModel;
                sb.Append("Ctriphotelid\thotelname\taddress\tprovincename\tcityname\taddress_postcode\tcountrycode\tlatitude\tlongitude\tphone\tCategory\t");
                sb.Append("\n");

                foreach (var item in objList)
                {
                    sb.Append($"{item.Ctriphotelid}\t{item.HotelName?.Value}\t{item?.Addresscn.Component.FirstOrDefault(c=>c.Name=="addr1")?.Value}\t{item?.Addresscn.Component.FirstOrDefault(c => c.Name == "province")?.Value}\t{item?.Addresscn.Component.FirstOrDefault(c => c.Name == "city")?.Value}\t{item?.Addresscn.Component.FirstOrDefault(c => c.Name == "postal_code")?.Value}\t{item.Country}\t{item.Lat}\t{item.Lon}\t{item.Phone.Value}\t{item.Category}\t");
                    sb.Append("\n");
                }

                return sb.ToString();
            }

        }

        private string DealQcXml(HttpPostedFileBase file)
        {
            var sb = new StringBuilder(); 
            using (var read = new StreamReader(file.InputStream))
            {
                var ss = read.ReadToEnd();
                var entity = Serializer.Deserialize<QueryControl>(ss) as QueryControl;
                
                foreach (var o in entity.Itiner.Override)
                {
                    foreach (var p in o.PropertyList)
                    {
                        sb.Append(p + "\n");
                    }
                }
            }

            
            return sb.ToString();
        }

        public string DealFeedExcel(HttpPostedFileBase file)
        {
            //以字符流的形式下载文件
            var zip = new ChineseFeedHelper();
            var csvHelper = new ExcelTemplateHelper<HotelFeedModel>(file, zip.FeedXmlProcess);

            csvHelper.Run();

            var ss = Serializer.Serialize(zip.TargetListing);
            return ss;
        }

        [HttpPost]
        public EnglishFeedFile DealReviewFeed(HttpFileCollectionBase files)
        {
            //byte[] result = null;
            string host = Request.Params["Host"];
            string lang = Request.Params["Lang"];

            var englishFile = new EnglishFeedFile();
            if (files != null && files.Count > 0)
            {
                
                for (var i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    if (file != null && file.FileName.Contains("english"))
                    {
                        using (ExcelTemplateHelper<HotelFeedModel> exHelper = new ExcelTemplateHelper<HotelFeedModel>(file, englishFile.FeedXmlProcess))
                        {
                            exHelper.Run();
                        }

                        file.InputStream.Dispose();
                    }

                }

                englishFile.GetReviewList().ListModel =
                    englishFile.GetReviewList().ListModel.OrderBy(l => l.Ctriphotelid).ToList();
                englishFile.SortHotelList = englishFile.GetReviewList().ListModel.Select(l => l.Ctriphotelid).ToArray();

                for (var i = 0;i<files.Count;i++)
                {
                    HttpPostedFileBase file = files[i];

                    if (file != null && file.FileName.Contains("pic"))
                    {
                        var imageFile = new ImageDeco(englishFile, host, lang);
                        using (var imageDeco = new ExcelTemplateHelper<ImageModel>(file, imageFile.FeedXmlProcess))
                        {
                            imageDeco.Run();
                        }
                    }

                    if (file != null && file.FileName.Contains("review"))
                    {
                        var reviewFile = new ReviewDeco(englishFile, host, lang);
                        using (var reviewDeco = new ExcelTemplateHelper<ReviewModel>(file, reviewFile.FeedXmlProcess))
                        {
                            reviewDeco.Run();
                        }
                    }

                    if (file != null && file.FileName.Contains("name"))
                    {
                        var nameFile = new NameDeco(englishFile, host, lang);
                        using (var nameDeco = new ExcelTemplateHelper<NameModel>(file, nameFile.FeedXmlProcess))
                        {
                            nameDeco.Run();
                        }
                    }

                    if (file != null && file.FileName.Contains("DESC"))
                    {
                        var descFile = new DescTxtDeco(englishFile, host, lang);
                        using (var descDeco = new TxtTemplateHelper(file, descFile.FeedXmlProcess))
                        {
                            descDeco.Run();
                        }
                    }

                    file?.InputStream.Dispose();
                }

                englishFile.GetReviewList().ListModel = englishFile.GetReviewList().ListModel.Where(l => l.HasName || l.HasReview).ToList();
                //result = Serializer.SerializeToBytes(englishFile.GetReviewList());
                //result = XmlHelper.XmlSerialize(englishFile.GetReviewList(), Encoding.UTF8);

            }

            return englishFile;
        }



        public class ChineseFeedHelper
        {
            public ListingsType TargetListing = new ListingsType();
            public void FeedXmlProcess(IList<HotelFeedModel> dataList)
            {
                foreach (var d in dataList)
                {
                    var t = new ListType()
                    {
                        Ctriphotelid = Convert.ToInt32(d.Ctriphotelid),
                        HotelName = new HotelNameType(){Value= d.hotelname },
                        Addresscn = new AddressType()
                        {
                            Component = new List<Component>()
                            {
                                new Component(){Name = "addr1", Value=d.address},
                                new Component(){Name = "province", Value=d.provincename},
                                new Component(){Name = "city", Value=d.cityname},
                                new Component(){Name = "postal_code",Value=d.address_postcode},
                            }
                        },
                        Country = d.countrycode,
                        Lat = Convert.ToDouble(d.latitude),
                        Lon = Convert.ToDouble(d.longitude),
                        Category = "Hotel",
                        Phone = new PhoneType() { Value=d.phone}
                    };
                    TargetListing.ListModel.Add(t);
                }
            }
        }

        

        public ActionResult Redict(int id)
        {
            //var request = new BaseRequestModel(){CityId=id};
            //return View("Test/Index1", request);
            return RedirectToAction("Index","Home",new {city= id});
        }
    }
}