using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MyTest2017
{
    public class AsyncTest
    {
        public Task<TOut> Async<TOut, TIn>(Func<TIn, TOut> async, TIn state = default(TIn))
        {
            var currentThread = Thread.CurrentThread;
            var currentHttpContext = System.Web.HttpContext.Current;
            var currentState = new AsyncState<TIn>(currentThread, currentHttpContext, state);
            return Task<TOut>.Factory.StartNew
            (
                o =>
                {
                    var s = (AsyncState<TIn>)o;
                    if (s?.Thread != null && s.HttpContext != null)
                    {
                        var culture = s.Thread.CurrentCulture;
                        if (culture.Name == "th-TH" || culture.Name == "th")
                        {
                            culture.DateTimeFormat = new DateTimeFormatInfo
                            {
                                Calendar = CultureInfo.InvariantCulture.Calendar
                            };
                        }
                        Thread.CurrentThread.CurrentCulture = culture;
                        Thread.CurrentThread.CurrentUICulture = culture;
                        System.Web.HttpContext.Current = s.HttpContext;
                        return async(s.State);
                    }
                    return default(TOut);
                },
                currentState
            );
        }

        public int GetCityId(int hotelid)
        {
            Thread.Sleep(2000);
            return hotelid + 10;
        }

        public bool IsDomestic(int hotel)
        {
            Thread.Sleep(5000);
            return hotel % 2 == 0;
        }

        public void Test()
        {
            int hotelId = 1;
            var taskList = new List<Task>();
            var getCityIdTask = Async(GetCityId, hotelId);
            var isDomesticTask = Async(IsDomestic, hotelId);
            taskList.Add(getCityIdTask);
            taskList.Add(isDomesticTask);

            int cityId = getCityIdTask?.Result ?? 1;
            bool isDomestic = isDomesticTask?.Result ?? true;

        }

        public class AsyncState<T>
        {
            public AsyncState(Thread thread, HttpContext httpContext, T state)
            {
                Thread = thread;
                HttpContext = httpContext;
                State = state;
            }

            public Thread Thread { get; }

            public HttpContext HttpContext { get; }

            public T State { get; }
        }
    }

    
}
