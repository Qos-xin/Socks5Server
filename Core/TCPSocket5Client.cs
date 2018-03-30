using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace Kingthy.Test.Socks5.Server.Core
{
    public class TCPSocket5Client
    {

        public TcpClient tcpClient = new TcpClient();
        public string RemoteHost { get; set; }
        public ushort RemotePort { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }

        public bool IsStarting { get; set; }
        public TCPSocket5Client(string RemoteHost, ushort RemotePort, string UserName, string PassWord)
        {
            this.RemoteHost = RemoteHost;
            this.RemotePort = RemotePort;
            this.UserName = UserName;
            this.PassWord = PassWord;
        }
        public void ContectRemote()
        {

        }



        public void Connect()
        {
            byte[] receiveBuffer = new byte[tcpClient.Client.ReceiveBufferSize];
            tcpClient.Connect(RemoteHost, RemotePort);
            if (tcpClient.Connected)
            {
                if (DoShakeHands())
                {
                    if (RequireValidate)
                    {
                        if (ValidateIdentity())
                        {
                            IsStarting = true;
                            return;
                        }
                    }
                }
            }
            IsStarting = false;
        }

        /// <summary>
        /// 握手
        /// </summary>
        /// <returns></returns>
        private bool DoShakeHands()
        {
            SocketUtils.Send(tcpClient.Client, new byte[] { 0x05, 0x01, 0x02 });
            if (SocketUtils.Receive(tcpClient.Client, 2, out byte[] buffer))
            {
                if (buffer.Count() == 2 && buffer[0] == 0x05 && buffer[1] == 0x02)
                {
                    Console.WriteLine("握手成功!");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <returns></returns>
        private bool ValidateIdentity()
        {

            var stream = new MemoryStream();
            stream.WriteByte(0x01);
            var UByte = Encoding.ASCII.GetBytes(UserName);
            stream.WriteByte((byte)UByte.Count());
            stream.Write(UByte, 0, UByte.Count());
            var UPass = Encoding.ASCII.GetBytes(PassWord);
            stream.WriteByte((byte)UPass.Count());
            stream.Write(UPass, 0, UPass.Count());
            SocketUtils.Send(tcpClient.Client, stream.ToArray());
            if (SocketUtils.Receive(tcpClient.Client, 2, out byte[] buffer))
            {
                if (buffer.Count() == 2 && buffer[0] == 0x01 && buffer[1] == 0x00)
                {
                    Console.WriteLine("用户认证通过");
                    return true;
                }
            }
            return false;
        }


        private void OnReceiveRemote(IAsyncResult state)
        {

            Socket socket = state.AsyncState as Socket;
            if (socket.Connected)
            {
                if (SocketUtils.Receive(socket, 2, out byte[] receiveBuffer))
                {

                    if (receiveBuffer[0] == 0x01 && receiveBuffer[1] == 0x00)
                    {

                    }
                    socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, OnReceiveRemote, socket);
                }
                else
                {
                    //远程关闭了连接
                }
            }


        }

        /// <summary>
        /// 是否需要验证身份
        /// </summary>
        private bool RequireValidate
        {
            get
            {
                return !string.IsNullOrEmpty(this.UserName) || !string.IsNullOrEmpty(this.PassWord);
            }
        }
    }

}