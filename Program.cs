/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * DateTime		:  2008-10-22 12:34:52
 * Description	:  Program 的摘要说明
 *
 * ***********************************************/


using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using Kingthy.Test.Socks5.Server.Core;
using System.Net;
using System.Net.Http;

namespace Kingthy.Test.Socks5.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //TCPSocket5Client client = new TCPSocket5Client("127.0.0.1", 7001, "test", "test");
            //client.Connect();
            //for (int i = 0; i < 10000; i++)
            //{
            //    HttpClientHandler httpClientHandler = new HttpClientHandler();
            //    httpClientHandler.Proxy = new WebProxy("127.0.0.1", 7001);
            //    HttpClient httpClient = new HttpClient(httpClientHandler);
            //    var hrm = httpClient.GetAsync("http://www.baidu.com").Result;

            //    Console.WriteLine($"{DateTime.Now.ToString()} {hrm.StatusCode}");
            //    Thread.Sleep(50);
            //}



            TCPSocket5Client client = new TCPSocket5Client("58.218.213.78", 10859, "201712071957334815", "45051552");
            client.Connect();
            TCPSocks5Server server = new TCPSocks5Server(7001, client);
            server.LogWatcher = Console.Out;
            server.Start();



            Console.WriteLine("如果要停止服务,请按回车键!!");
            Console.ReadLine();
            //server.Stop();
        }
    }
}
