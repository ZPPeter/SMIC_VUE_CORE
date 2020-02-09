using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SMIC.Web.Host.Hubs
{
    /// <summary>
    /// 消息处理
    /// </summary>
    public class MessageDealWidth
    {
        public static async Task DealWidth(string message,MyChatHub chatHub)
        {
            await Task.Run(() => {
                try
                {
                    MessageData data = JsonConvert.DeserializeObject<MessageData>(message);
                    if (data != null)
                    {
                        ConnectionUser connectionUser = null;
                        MessageData sendMsg = null;
                        switch (data.MessageType)
                        {
                            case MessageType.Line:
                                connectionUser = ConnectionManager.ConnectionUsers.Where(m => m.ConnectionId == chatHub.Context.ConnectionId).FirstOrDefault();
                                //处理连接消息
                                if (connectionUser == null)
                                {
                                    connectionUser = new ConnectionUser();
                                    connectionUser.ConnectionId = chatHub.Context.ConnectionId;
                                    connectionUser.UserId = data.SendUserId;
                                    ConnectionManager.ConnectionUsers.Add(connectionUser);
                                }
                                //处理发送回执消息
                                sendMsg = new MessageData();
                                sendMsg.MessageBody = data.SendUserId;
                                sendMsg.MessageType = MessageType.LineReceipt;
                                sendMsg.SendUserId = "0";
                                //发给自己
                                //chatHub.Clients.Client(chatHub.Context.ConnectionId).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(sendMsg));
                                foreach (ConnectionUser user in ConnectionManager.ConnectionUsers)
                                {
                                    chatHub.Clients.Client(user.ConnectionId).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(sendMsg));
                                    //chatHub.Clients.All.SendAsync
                                }
                                break;
                            case MessageType.Text:
                                //处理普通文字消息
                                ChatCore.SendMessage(chatHub, data);
                                break;
                            case MessageType.LineReceipt:
                                //处理连接回执消息
                                ChatCore.SendMessage(chatHub, data);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}
