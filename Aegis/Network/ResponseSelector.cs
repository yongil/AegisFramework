﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.IO;



namespace Aegis.Network
{
    internal class ResponseSelector
    {
        private struct Data
        {
            public PacketPredicate Predicate;
            public IOEventHandler Dispatcher;


            public Data(PacketPredicate predicate, IOEventHandler dispatcher)
            {
                Predicate = predicate;
                Dispatcher = dispatcher;
            }
        }





        private readonly Session _session;
        private readonly List<Data> _listResponseAction;


        public ResponseSelector(Session session)
        {
            _session = session;
            _listResponseAction = new List<Data>();
        }


        public void Add(PacketPredicate predicate, IOEventHandler dispatcher)
        {
            _listResponseAction.Add(new Data(predicate, dispatcher));
        }


        public bool Dispatch(StreamBuffer buffer)
        {
            foreach (var data in _listResponseAction)
            {
                if (data.Predicate(buffer) == true)
                {
                    try
                    {
                        _listResponseAction.Remove(data);
                        data.Dispatcher(new IOEventResult(_session, IOEventType.Read, buffer.Buffer, AegisResult.Ok));
                    }
                    catch (Exception e)
                    {
                        Logger.Err(LogMask.Aegis, e.ToString());
                    }
                    return true;
                }
            }

            return false;
        }
    }
}
