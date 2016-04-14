/************************************************************************
 * Copyright (C) 2007 Jamaa Technologies
 *
 * This file is part of Jamaa SMPP Library.
 *
 * Jamaa SMPP Library is free software. You can redistribute it and/or modify
 * it under the terms of the Microsoft Reciprocal License (Ms-RL)
 *
 * You should have received a copy of the Microsoft Reciprocal License
 * along with Jamaa SMPP Library; See License.txt for more details.
 *
 * Author: Benedict J. Tesha
 * benedict.tesha@jamaatech.com, www.jamaatech.com
 *
 ************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using JamaaTech.Smpp.Net.Lib;
using JamaaTech.Smpp.Net.Lib.Util;

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public sealed class CancelSm : SmOperationPDU
    {
        #region Variables
        private SmppAddress vDestinationAddress;
        #endregion

        #region Constuctors
        public CancelSm()
            : base(new PDUHeader(CommandType.CancelSm))
        {
            vDestinationAddress = new SmppAddress();
        }

        internal CancelSm(PDUHeader header)
            : base(header)
        {
            vDestinationAddress = new SmppAddress();
        }
        #endregion

        #region Properties
        public override SmppEntityType AllowedSource
        {
            get { return SmppEntityType.ESME; }
        }

        public override SmppSessionState AllowedSession
        {
            get { return SmppSessionState.Transmitter; }
        }

        public SmppAddress DestionationAddress
        {
            get { return vDestinationAddress; }
        }
        #endregion

        #region Methods
        public override ResponsePDU CreateDefaultResponce()
        {
            PDUHeader header = new PDUHeader(CommandType.CancelSmResp, vHeader.SequenceNumber);
            return new CancelSmResp(header);
        }

        protected override byte[] GetBodyData()
        {
            ByteBuffer buffer = new ByteBuffer(64);
            buffer.Append(EncodeCString(vMessageID));
            buffer.Append(vSourceAddress.GetBytes());
            buffer.Append(vDestinationAddress.GetBytes());
            return buffer.ToBytes();
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            vMessageID = DecodeCString(buffer);
            vSourceAddress = SmppAddress.Parse(buffer);
            vDestinationAddress = SmppAddress.Parse(buffer);
            //If this pdu has no option parameters
            //If there is still something in the buffer, we then have more than required bytes
            if (buffer.Length > 0) { throw new TooManyBytesException(); }
        }
        #endregion
    }
}
