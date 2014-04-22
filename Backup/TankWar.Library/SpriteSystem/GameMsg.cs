using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;

namespace TankWar.Library.SpriteSystem
{
    public class GameMsg
    {
        private static MessageQueue mq = null;

        private static string path = @".\private$\坦克大战";

        static GameMsg()
        {
            

            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(path);
            }

            mq = new MessageQueue(path);
            mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
        }
        
        public static void SendMsg(string msg)
        {
            mq.Send(msg, "GameOver");
        }

        public static string GetGameOverMessage()
        {
            string msgstr = null;

            Message[] msgs = mq.GetAllMessages();

            foreach (Message msg in msgs)
            {
                if (msg.Label.Equals("GameOver"))
                {  
                    msgstr = msg.Body.ToString();
                    break;
                }
            }

            mq.Purge();

            return msgstr;
        }
    }
}
