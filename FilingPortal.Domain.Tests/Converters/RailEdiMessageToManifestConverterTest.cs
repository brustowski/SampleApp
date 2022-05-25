using System;
using System.Collections.Generic;
using FilingPortal.Domain.DTOs.Rail.Manifest;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Converters
{
    [TestClass]
    public class RailEdiMessageToManifestConverterTest: TestWithApplicationMapping
    {
        private RailEdiMessage CreateRailEdiMessage()
        {
            return new RailEdiMessage
            {
                Id = 1,
                EmMessageText = "B011001BERBD                                                                    1MBNSF21CATRAIN                  19298     000001        P                      2MBNSF01736298201902                                                            1P3004102519                                                                    1JBNSF                                                                          1B017362982019801010000000001TNK  0000185913LB                                  2B0000000000  BRUDERHEIM                                                        4BSI U7435125-A2-0590                                                           4BWY 454134                                                                     0NSH BRUDERHEIM ENERGY TERMINAL                                                 2N555018 RR 202                                                                 3NBRUDERHEIM         ABT0B0S0                                                   0NCN TESORO CANADA SUPPLY & DISTRIBUTION                                        2N110 9TH AVENUE SW                                                             3NCALGARY            ABT2P0T1                                                   0NC1 ZENITH ENERGY TERMINALS HOLDINGS LL                                        2N5501 NW FRONT AVE                                                             3NPORTLAND           OR97210                                                    0NBT TESORO CANADA SUPPLY & DISTRIBUTION                                        2N110 9TH AVE SW                                                                3NCALGARY            ABT2P0T1                                                   0NCB CHARTER BROKERAGE                  171001BER                               1CUTLX683520    105702                        RR000000000000000000000           0D           000000000000185913LB                                               1D0000000001PETROLEUM CRUDE OIL                                        TNK      2DNO MARKS OR NUMBERS                                                           1VUN1267         PETROLEUM CRUDE OIL           18004249300                      Y  1001BERBD00026                                                               ",
                CreatedUser = "test_user",
                CreatedDate = DateTime.Now,
                CwLastModifiedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                RailBdParseds = new List<RailBdParsed>()
            };
        } 

        [TestMethod]
        public void Map_ConvertsEdiMessageToManifest()
        {
            RailEdiMessage ediMessage = CreateRailEdiMessage();
            Manifest manifest = ediMessage.Map<RailEdiMessage, Manifest>();
            Assert.IsNotNull(manifest.ManifestHeader);
        }
    }
}
