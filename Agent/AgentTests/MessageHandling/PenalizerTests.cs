using Microsoft.VisualStudio.TestTools.UnitTesting;
using Agent.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;
using CommunicationLibrary.Model;
using CommunicationLibrary;
using CommunicationLibrary.Response;
using CommunicationLibrary.Request;
using CommunicationLibrary.Error;
using System.Threading;

namespace Agent.MessageHandling.Tests
{
    [TestClass()]
    public class PenalizerTests
    {
        readonly Penalties _samplePenalties = new Penalties
        {
            CheckForSham = "30",
            DestroyPiece = "30",
            Discovery = "30",
            InformationExchange = "30",
            Move = "60",
            PutPiece = "100"
        };

        [TestMethod()]
        public void TestCheckPieceResponseReceiveFreeAfter()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<CheckHoldedPieceResponse>(
                new CheckHoldedPieceResponse());
            int waitTime = Int32.Parse(_samplePenalties.CheckForSham);

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestCheckPieceResponseReceiveBlockedDuring()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<CheckHoldedPieceResponse>(
                new CheckHoldedPieceResponse());

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Assert.IsTrue(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestDestroyPieceResponseReceiveFreeAfter()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<DestroyPieceResponse>(
                new DestroyPieceResponse());
            int waitTime = Int32.Parse(_samplePenalties.DestroyPiece);

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestDestroyPieceResponseReceiveBlockedDuring()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<DestroyPieceResponse>(
                new DestroyPieceResponse());

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Assert.IsTrue(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestDiscoveryResponseReceiveFreeAfter()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<DiscoveryResponse>(
                new DiscoveryResponse());
            int waitTime = Int32.Parse(_samplePenalties.Discovery);

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestDiscoveryResponseReceiveBlockedDuring()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<DiscoveryResponse>(
                new DiscoveryResponse());

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Assert.IsTrue(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestMoveResponseReceiveFreeAfter()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<MoveResponse>(
                new MoveResponse());
            int waitTime = Int32.Parse(_samplePenalties.Move);

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestMoveResponseReceiveBlockedDuring()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<MoveResponse>(
                new MoveResponse());

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Assert.IsTrue(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestPutPieceResponseReceiveFreeAfter()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<PutPieceResponse>(
                new PutPieceResponse());
            int waitTime = Int32.Parse(_samplePenalties.PutPiece);

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestPutPieceResponseReceiveBlockedDuring()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<PutPieceResponse>(
                new PutPieceResponse());
            //checking after half of wait time only in one test because:
            //if penalty to wait is to slow then during the time it takes to execute
            //instructions before assert penalty could be over
            
            //longer penalty only in one test otherwise all tests would take too long
            int waitTime = Int32.Parse(_samplePenalties.PutPiece);

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Thread.Sleep(waitTime / 2);
            Assert.IsTrue(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestPenaltyNotWaitedReceiveFreeAfter()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<PenaltyNotWaitedError>(
                new PenaltyNotWaitedError { WaitUntill = DateTime.Now.AddMilliseconds(30) });
            int waitTime = (int)(receivedMessage.MessagePayload.WaitUntill - DateTime.Now)
                .TotalMilliseconds + 10;//precision (from documentation)

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestPenaltyNotWaitedReceiveBlockedBefore()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var receivedMessage = new Message<PenaltyNotWaitedError>(
                new PenaltyNotWaitedError { WaitUntill = DateTime.Now.AddMilliseconds(100) });
            int waitTime = (int)(receivedMessage.MessagePayload.WaitUntill - DateTime.Now)
                .TotalMilliseconds;

            //when
            penalizer.PenalizeOnReceive(receivedMessage);

            //then
            Thread.Sleep(waitTime/2);
            Assert.IsTrue(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestTwoMessageReceiveFreeAfterBigger()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var penaltyNotWaitedMessage = new Message<PenaltyNotWaitedError>(
                new PenaltyNotWaitedError { WaitUntill = DateTime.Now.AddMilliseconds(30) });
            int penaltyNotWaitedWaitTime = (int)(penaltyNotWaitedMessage.MessagePayload.WaitUntill - DateTime.Now)
                .TotalMilliseconds;
            var moveMessage = new Message<MoveResponse>(
                new MoveResponse());
            int moveWaitTime = Int32.Parse(_samplePenalties.Move);
            int waitTime = Math.Max(moveWaitTime, penaltyNotWaitedWaitTime);


            //when
            penalizer.PenalizeOnReceive(penaltyNotWaitedMessage);
            penalizer.PenalizeOnReceive(moveMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestTwoMessageReceiveStillBlockedAfterSmaller()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var penaltyNotWaitedMessage = new Message<PenaltyNotWaitedError>(
                new PenaltyNotWaitedError { WaitUntill = DateTime.Now.AddMilliseconds(30) });
            int penaltyNotWaitedWaitTime = (int)(penaltyNotWaitedMessage.MessagePayload.WaitUntill - DateTime.Now)
                .TotalMilliseconds;
            var moveMessage = new Message<MoveResponse>(
                new MoveResponse());
            int moveWaitTime = Int32.Parse(_samplePenalties.Move);
            int waitTime = Math.Min(moveWaitTime, penaltyNotWaitedWaitTime);


            //when
            penalizer.PenalizeOnReceive(penaltyNotWaitedMessage);
            penalizer.PenalizeOnReceive(moveMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsTrue(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestExchangeRequestSentFreeAfter()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var sentMessage = new Message<ExchangeInformationRequest>(
                new ExchangeInformationRequest { });
            int waitTime = Int32.Parse(_samplePenalties.InformationExchange);

            //when
            penalizer.PenalizeOnSend(sentMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestExchangeRequestSentBlockedBefore()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var sentMessage = new Message<ExchangeInformationRequest>(
                new ExchangeInformationRequest { });

            //when
            penalizer.PenalizeOnSend(sentMessage);

            //then
            Assert.IsTrue(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestExchangeResponseSentFreeAfter()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var sentMessage = new Message<ExchangeInformationResponse>(
                new ExchangeInformationResponse { });
            int waitTime = Int32.Parse(_samplePenalties.InformationExchange);

            //when
            penalizer.PenalizeOnSend(sentMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestExchangeResponseSentBlockedBefore()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var sentMessage = new Message<ExchangeInformationResponse>(
                new ExchangeInformationResponse { });

            //when
            penalizer.PenalizeOnSend(sentMessage);

            //then
            Assert.IsTrue(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestReceiveAndSendFreeAfterBigger()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var exchangeRequest = new Message<ExchangeInformationRequest>(
                new ExchangeInformationRequest());
            int exchangeWaitTime = Int32.Parse(_samplePenalties.InformationExchange);
            var moveMessage = new Message<MoveResponse>(
                new MoveResponse());
            int moveWaitTime = Int32.Parse(_samplePenalties.Move);
            int waitTime = Math.Max(moveWaitTime, exchangeWaitTime);


            //when
            penalizer.PenalizeOnReceive(exchangeRequest);
            penalizer.PenalizeOnReceive(moveMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsFalse(penalizer.UnderPenalty);
        }

        [TestMethod()]
        public void TestReceiveAndSendStillBlockedAfterSmaller()
        {
            //given
            var penalizer = new Penalizer(_samplePenalties);
            var exchangeRequest = new Message<ExchangeInformationRequest>(
                new ExchangeInformationRequest());
            int exchangeWaitTime = Int32.Parse(_samplePenalties.InformationExchange);
            var moveMessage = new Message<MoveResponse>(
                new MoveResponse());
            int moveWaitTime = Int32.Parse(_samplePenalties.Move);
            int waitTime = Math.Min(moveWaitTime, exchangeWaitTime);


            //when
            penalizer.PenalizeOnReceive(exchangeRequest);
            penalizer.PenalizeOnReceive(moveMessage);

            //then
            Thread.Sleep(waitTime);
            Assert.IsTrue(penalizer.UnderPenalty);
        }
    }
}