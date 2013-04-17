using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace t2sDbLibrary
{
    //public enum PhoneCarrier
    //{
    //    [Description("vtext.com")]
    //    Verizon,
    //    [Description("messaging.sprint.com")]
    //    Sprint,
    //    [Description("email.uscc.net")]
    //    USCellular,
    //    [Description("txt.att.com")]
    //    ATT,
    //    [Description("myboostmobile.com")]
    //    BoostMobile,
    //    [Description("comcastpcs.textmsg.com")]
    //    Comcast,
    //    [Description("qwestmp.com")]
    //    Qwest,
    //    [Description("tmomail.net")]
    //    TMobile,
    //    [Description("sms.wcc.net")]
    //    WestCentralWireless,
    //    [Description("pagemart.net")]
    //    WebLinkWireless,
    //    [Description("airmessage.net")]
    //    VoiceStream,
    //    [Description("voicestream.net")]
    //    MyVerizonWireless,
    //    [Description("myvzw.com")]
    //    VerizonPCS,
    //    [Description("myairmail.com")]
    //    VerisonPagers,
    //    [Description("stpaging.com")]
    //    STPaging,
    //    [Description("email.skytel.com")]
    //    SkytelPagers,
    //    [Description("paging.acswireless.com")]
    //    SBCAmeritechPaging,
    //    [Description("ram-page.com")]
    //    RAMPage,
    //    [Description("pager.qualcomm.com")]
    //    Qualcomm,
    //    [Description("page.propage.net")]
    //    ProPage,
    //    [Description("email.uscc.net")]
    //    Primeco,
    //    [Description("mobilecell1se.com")]
    //    PriceCommunications,
    //    [Description("voicestream.net")]
    //    Powertel,
    //    [Description("pcsone.net")]
    //    PCSOne,
    //    [Description("page1nw.com")]
    //    PageOneNorthwest,
    //    [Description("pagegate.pagenet.ca")]
    //    PageNetCanada,
    //    [Description("pmcl.net")]
    //    PageMartCanada,
    //    [Description("pagemart.net")]
    //    PageMart,
    //    [Description("pacbellpcs.net")]
    //    PacificBell,
    //    [Description("omnipointpcs.com")]
    //    Omnipoint,
    //    [Description("page.nextel.com")]
    //    Nextel,
    //    [Description("isp.com")]
    //    Motient,
    //    [Description("beepone.net")]
    //    MorrisWireless,
    //    [Description("page.mobilcom.net")]
    //    MobilecomPA,
    //    [Description("page.metrocall.com")]
    //    Metrocall,
    //    [Description("pagemci.com")]
    //    MCI,
    //    [Description("inlandlink.com")]
    //    IndianaPagingCo,
    //    [Description("page.infopagesystems.com")]
    //    InfopageSystems,
    //    [Description("messagealert.com")]
    //    GTE,
    //    [Description("gte.pagegate.net")]
    //    GTE1,
    //    [Description("airmessage.net")]
    //    GTE2,
    //    [Description("epage.porta-phone.com")]
    //    GrayLinkPortaPhone,
    //    [Description("webpager.us")]
    //    GCSPaging,
    //    [Description("sendabeep.net")]
    //    GalaxyCorporation,
    //    [Description("page.hit.net")]
    //    DigiPagePageKansas,
    //    [Description("corrwireless.net")]
    //    CorrWirelessCommunications,
    //    [Description("cookmail.com")]
    //    CookPaging,
    //    [Description("pageme.comspeco.net")]
    //    CommunicationSpecialists,
    //    [Description("cingularme.com")]
    //    Cingular,
    //    [Description("cvcpaging.com")]
    //    CentralVermontCommunications,
    //    [Description("mycellone.com")]
    //    CellularOneWest,
    //    [Description("sbcemail.com")]
    //    CellularOne,
    //    [Description("cell1.textmsg.com")]
    //    CellularOne1,
    //    [Description("cellularone.textmsg.com")]
    //    CellularOne2,
    //    [Description("cellularone.txtmsg.com")]
    //    CellularOne3,
    //    [Description("swmsg.com")]
    //    CellularOneSouthWest,
    //    [Description("phone.cellone.net")]
    //    CellularOneEastCoast,
    //    [Description("blsdcs.net")]
    //    BellSouthMobility,
    //    [Description("bellsouthtips.com")]
    //    BellSouthBlackberry,
    //    [Description("wireless.bellsouth.com")]
    //    BellSouth,
    //    [Description("message.bam.com")]
    //    BellAtlantic,
    //    [Description("beepwear.net")]
    //    Beepwear,
    //    [Description("dpcs.mobile.att.net")]
    //    ATTPocketnetPCS,
    //    [Description("mobile.att.net")]
    //    ATTPCS,
    //    [Description("mmode.com")]
    //    ATTFree2Go,
    //    [Description("archwireless.net")]
    //    ArchPagersPageNet,
    //    [Description("clearpath.acswireless.com")]
    //    AmeritechClearpath,
    //    [Description("page.americanmessaging.net")]
    //    AmericanMessaging,
    //    [Description("paging.acswireless.com")]
    //    AmeritechPaging,
    //    [Description("alphanow.net")]
    //    AlphaNow,
    //    [Description("myairmail.com")]
    //    AirtouchPagers,
    //    [Description("advantagepaging.com")]
    //    AdvantageCommunications,
    //    [Description("cellularonewest.com")]
    //    WesternWireless,
    //    [Description("sms.wcc.net")]
    //    VMobile,
    //    [Description("vmobile.ca")]
    //    VirginMobileCanada,
    //    [Description("vmobl.com")]
    //    VirginMobile,
    //    [Description("uswestdatamail.com")]
    //    USWest,
    //    [Description("utext.com")]
    //    Unicel,
    //    [Description("tms.suncom.com")]
    //    Triton,
    //    [Description("msg.telus.com")]
    //    Telus,
    //    [Description("mobile.surewest.com")]
    //    SurewestCommunicaitons,
    //    [Description("tms.suncom.com")]
    //    Sumcom,
    //    [Description("txt.bell.ca")]
    //    SoloMobile,
    //    [Description("satellink.net")]
    //    Satellink,
    //    [Description("text.mtsmobility.com")]
    //    MTS,
    //    [Description("clearlydigital.com")]
    //    MidwestWireless,
    //    [Description("ivctext.com")]
    //    IllinoisValleyCellular,
    //    [Description("csouth1.com")]
    //    CellularSouth,
    //    [Description("bellmobility.ca")]
    //    BellCanada,
    //    [Description("txt.bellmobility.ca")]
    //    BellCanada2,
    //    [Description("message.alltel.com")]
    //    Alltel,
    //    [Description("paging.acswireless.com")]
    //    ACSWireless,
    //    [Description("sms.3rivers.net")]
    //    ThreeRiverWireless,
    //    [Description("blueskyfrog.com")]
    //    BlueSkyFrog
    //}

    /// <summary>
    /// Class used to mimic a type-safe enum
    /// Reference: http://stackoverflow.com/a/424414
    /// </summary>
    public sealed class PhoneCarrier
    {
        private readonly int value;
        private readonly string name;
        private readonly string email;

        private static readonly Dictionary<string, PhoneCarrier> nameInstance = new Dictionary<string, PhoneCarrier>();
        private static readonly Dictionary<int, PhoneCarrier> valueInstance = new Dictionary<int, PhoneCarrier>();

        public static readonly PhoneCarrier Verizon = new PhoneCarrier(1, "Verizon", "vtext.com");
        public static readonly PhoneCarrier Sprint = new PhoneCarrier(2, "Sprint", "messaging.sprint.com");
        public static readonly PhoneCarrier USCellular = new PhoneCarrier(3, "US Cellular", "email.uscc.net");
        public static readonly PhoneCarrier ATT = new PhoneCarrier(4, "AT&T", "txt.att.com");
        public static readonly PhoneCarrier BoostMobile = new PhoneCarrier(5, "Boost Mobile", "myboostmobile.com");
        public static readonly PhoneCarrier Comcast = new PhoneCarrier(6, "Comcast", "comcastpcs.textmsg.com");
        public static readonly PhoneCarrier Qwest = new PhoneCarrier(7, "Qwest", "qwestmp.com");
        public static readonly PhoneCarrier TMobile = new PhoneCarrier(8, "T-Mobile", "tmomail.net");
        public static readonly PhoneCarrier WestCentralWireless = new PhoneCarrier(9, "West Central Wireless", "sms.wcc.net");
        public static readonly PhoneCarrier WebLinkWireless = new PhoneCarrier(10, "WebLink Wireless", "pagemart.net");
        public static readonly PhoneCarrier VoiceStream = new PhoneCarrier(11, "Voice Stream", "airmessage.net");
        public static readonly PhoneCarrier MyVerizonWireless = new PhoneCarrier(12, "My Verizon Wireless", "voicestream.net");
        public static readonly PhoneCarrier VerizonPCS = new PhoneCarrier(13, "Verizon PCS", "myvzw.com");
        public static readonly PhoneCarrier VerizonPagers = new PhoneCarrier(14, "Verizon Pagers", "myairmail.com");
        public static readonly PhoneCarrier VerizonPix = new PhoneCarrier(15, "Verizon Pix", "vzwpix.com");
        public static readonly PhoneCarrier STPaging = new PhoneCarrier(16, "STPaging", "stpaging.com");
        public static readonly PhoneCarrier SkytelPagers = new PhoneCarrier(17, "Skytel Pagers", "email.skytel.com");
        public static readonly PhoneCarrier SBCAmeritechPaging = new PhoneCarrier(18, "SBC Ameritech Paging", "paging.acswireless.com");
        public static readonly PhoneCarrier RAMPage = new PhoneCarrier(19, "RAM Page", "ram-page.com");
        public static readonly PhoneCarrier Qualcomm = new PhoneCarrier(20, "Qualcomm", "pager.qualcomm.com");
        public static readonly PhoneCarrier ProPage = new PhoneCarrier(21, "ProPage", "page.propage.net");
        public static readonly PhoneCarrier Primeco = new PhoneCarrier(22, "Primeco", "email.uscc.net");
        public static readonly PhoneCarrier PriceCommunications = new PhoneCarrier(23, "Price Communications", "mobilecell1se.com");
        public static readonly PhoneCarrier Powertel = new PhoneCarrier(24, "Powertel", "voicestream.net");
        public static readonly PhoneCarrier PCSOne = new PhoneCarrier(25, "PCS One", "pcsone.net");
        public static readonly PhoneCarrier PageOneNorthwest = new PhoneCarrier(26, "Page One Northwest", "page1nw.com");
        public static readonly PhoneCarrier PageNetCanada = new PhoneCarrier(27, "PageNet Canada", "pagegate.pagenet.ca");
        public static readonly PhoneCarrier PageMartCanada = new PhoneCarrier(28, "PageMart Cananda", "pmcl.net");
        public static readonly PhoneCarrier PageMart = new PhoneCarrier(29, "PageMart", "pagemart.net");
        public static readonly PhoneCarrier PacificBell = new PhoneCarrier(30, "Pacific Bell", "pacbellpcs.net");
        public static readonly PhoneCarrier Omnipoint = new PhoneCarrier(31, "Omnipoint", "omnipointpcs.com");
        public static readonly PhoneCarrier Nextel = new PhoneCarrier(32, "Nextel", "page.nextel.com");
        public static readonly PhoneCarrier Motient = new PhoneCarrier(33, "Motient", "isp.com");
        public static readonly PhoneCarrier MorrisWireless = new PhoneCarrier(34, "Morris Wireless", "beepone.net");
        public static readonly PhoneCarrier MobilecomPA = new PhoneCarrier(35, "Mobilecom PA", "page.mobilcom.net");
        public static readonly PhoneCarrier Metrocall = new PhoneCarrier(36, "Metrocall", "page.metrocall.com");
        public static readonly PhoneCarrier MCI = new PhoneCarrier(37, "MCI", "pagemci.com");
        public static readonly PhoneCarrier IndianaPagingCo = new PhoneCarrier(38, "Indiana Paging Co", "inlandlink.com");
        public static readonly PhoneCarrier InfopageSystems = new PhoneCarrier(39, "Infopage Systems", "page.infopagesystems.com");
        public static readonly PhoneCarrier GTE = new PhoneCarrier(40, "GTE", "messagealert.com");
        public static readonly PhoneCarrier GTE1 = new PhoneCarrier(41, "GTE1", "gte.pagegate.net");
        public static readonly PhoneCarrier GTE2 = new PhoneCarrier(42, "GTE2", "airmessage.net");
        public static readonly PhoneCarrier GrayLinkPortaPhone = new PhoneCarrier(43, "ABCDEFG", "epage.porta-phone.com");
        public static readonly PhoneCarrier GCSPaging = new PhoneCarrier(44, "GCS Paging", "webpager.us");
        public static readonly PhoneCarrier GalaxyCorporation = new PhoneCarrier(45, "Galaxy Corporation", "sendabeep.net");
        public static readonly PhoneCarrier DigiPagePageKansas = new PhoneCarrier(46, "DigiPage Page Kansas", "page.hit.net");
        public static readonly PhoneCarrier CorrWirelessCommunications = new PhoneCarrier(47, "Corr Wireless Communications", "corrwireless.net");
        public static readonly PhoneCarrier CookPaging = new PhoneCarrier(48, "Cook Paging", "cookmail.com");
        public static readonly PhoneCarrier CommunicationSpecialists = new PhoneCarrier(49, "Communication Specialists", "pageme.comspeco.net");
        public static readonly PhoneCarrier Cingular = new PhoneCarrier(50, "Cingular", "cingularme.com");
        public static readonly PhoneCarrier CentralVermontCommunications = new PhoneCarrier(51, "Central Vermont Communications", "cvcpaging.com");
        public static readonly PhoneCarrier CellularOneWest = new PhoneCarrier(52, "Cellular One West", "mycellone.com");
        public static readonly PhoneCarrier CellularOne = new PhoneCarrier(53, "Cellular One", "sbcemail.com");
        public static readonly PhoneCarrier CellularOne1 = new PhoneCarrier(54, "Cellular One 1", "cell1.textmsg.com");
        public static readonly PhoneCarrier CellularOne2 = new PhoneCarrier(55, "Cellular One 2", "cellularone.textmsg.com");
        public static readonly PhoneCarrier CellularOne3 = new PhoneCarrier(56, "Cellular One 3", "cellularone.txtmsg.com");
        public static readonly PhoneCarrier CellularOneSouthWest = new PhoneCarrier(57, "Cellular One South West", "swmsg.com");
        public static readonly PhoneCarrier CellularOneEastCoast = new PhoneCarrier(58, "Cellular One East Coast", "phone.cellone.net");
        public static readonly PhoneCarrier BellSouthMobility = new PhoneCarrier(59, "Bell South Mobility", "blsdcs.net");
        public static readonly PhoneCarrier BellSouthBlackberry = new PhoneCarrier(60, "Bell South Blackberry", "bellsouthtips.com");
        public static readonly PhoneCarrier BellSouth = new PhoneCarrier(61, "Bell South", "wireless.bellsouth.com");
        public static readonly PhoneCarrier BellAtlantic = new PhoneCarrier(62, "Bell Atlantic", "message.bam.com");
        public static readonly PhoneCarrier Beepwear = new PhoneCarrier(63, "Beepwear", "beepwear.net");
        public static readonly PhoneCarrier ATTPocketnetPCS = new PhoneCarrier(64, "AT&T Pocketnet PCS", "dpcs.mobile.att.net");
        public static readonly PhoneCarrier ATTPCS = new PhoneCarrier(65, "AT&T PCS", "mobile.att.net");
        public static readonly PhoneCarrier ATTFree2Go = new PhoneCarrier(66, "AT&T Free2Go", "mmode.com");
        public static readonly PhoneCarrier ArchPagersPageNet = new PhoneCarrier(67, "Arch Pagers PageNet", "archwireless.net");
        public static readonly PhoneCarrier AmeritechClearpath = new PhoneCarrier(68, "Ameritech Clearpath", "clearpath.acswireless.com");
        public static readonly PhoneCarrier AmericanMessaging = new PhoneCarrier(69, "American Messaging", "page.americanmessaging.net");
        public static readonly PhoneCarrier AmeritechPaging = new PhoneCarrier(70, "Ameritech Paging", "paging.acswireless.com");
        public static readonly PhoneCarrier AlphaNow = new PhoneCarrier(71, "Alpha Now", "alphanow.net");
        public static readonly PhoneCarrier AirtouchPagers = new PhoneCarrier(72, "Airtouch Pagers", "myairmail.com");
        public static readonly PhoneCarrier AdvantageCommunications = new PhoneCarrier(73, "Advantage Communications", "advantagepaging.com");
        public static readonly PhoneCarrier WesternWireless = new PhoneCarrier(74, "Western Wireless", "cellularonewest.com");
        public static readonly PhoneCarrier VMobile = new PhoneCarrier(75, "VMobile", "sms.wcc.net");
        public static readonly PhoneCarrier VirginMobileCanada = new PhoneCarrier(76, "Virgin Mobile Canada", "vmobile.ca");
        public static readonly PhoneCarrier VirginMobile = new PhoneCarrier(77, "Virgin Mobile", "vmobl.com");
        public static readonly PhoneCarrier USWest = new PhoneCarrier(78, "US West", "uswestdatamail.com");
        public static readonly PhoneCarrier Unicel = new PhoneCarrier(79, "Unicel", "utext.com");
        public static readonly PhoneCarrier Triton = new PhoneCarrier(80, "Triton", "tms.suncom.com");
        public static readonly PhoneCarrier Telus = new PhoneCarrier(81, "Telus", "msg.telus.com");
        public static readonly PhoneCarrier SurewestCommunicaitons = new PhoneCarrier(82, "Surewest Communications", "mobile.surewest.com");
        public static readonly PhoneCarrier Sumcom = new PhoneCarrier(83, "Sumcom", "tms.suncom.com");
        public static readonly PhoneCarrier SoloMobile = new PhoneCarrier(84, "SoloMobile", "txt.bell.ca");
        public static readonly PhoneCarrier Satellink = new PhoneCarrier(85, "Satellink", "satellink.net");
        public static readonly PhoneCarrier MTS = new PhoneCarrier(86, "MTS", "text.mtsmobility.com");
        public static readonly PhoneCarrier MidwestWireless = new PhoneCarrier(87, "Midwest Wireless", "clearlydigital.com");
        public static readonly PhoneCarrier IllinoisValleyCellular = new PhoneCarrier(88, "Illinois Valley Cellular", "ivctext.com");
        public static readonly PhoneCarrier CellularSouth = new PhoneCarrier(89, "Cellular South", "csouth1.com");
        public static readonly PhoneCarrier BellCanada = new PhoneCarrier(90, "Bell Canada", "bellmobility.ca");
        public static readonly PhoneCarrier BellCanada2 = new PhoneCarrier(91, "Bell Canada 2", "txt.bellmobility.ca");
        public static readonly PhoneCarrier Alltel = new PhoneCarrier(92, "Alltel", "message.alltel.com");
        public static readonly PhoneCarrier ACSWireless = new PhoneCarrier(93, "ACS Wireless", "paging.acswireless.com");
        public static readonly PhoneCarrier ThreeRiverWireless = new PhoneCarrier(94, "Three River Wireless", "sms.3rivers.net");
        public static readonly PhoneCarrier BlueSkyFrog = new PhoneCarrier(95, "Blue Sky Frog", "blueskyfrog.com");

        private PhoneCarrier(int value, string name, string email)
        {
            this.value = value;
            this.name = name;
            this.email = email;

            nameInstance[name] = this;
            valueInstance[value] = this;
        }

        public string GetName()
        {
            return name;
        }

        public string GetEmail()
        {
            return email;
        }

        /// <summary>
        /// Converts a string to a corresponding PhoneCarrier
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static explicit operator PhoneCarrier(string name)
        {
            PhoneCarrier result;
            if (nameInstance.TryGetValue(name, out result))
                return result;
            else
                throw new InvalidCastException();
        }

        /// <summary>
        /// Converts an integer to a corresponding PhoneCarrier
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator PhoneCarrier(int value)
        {
            PhoneCarrier result;
            if (valueInstance.TryGetValue(value, out result))
                return result;
            else
                throw new InvalidCastException();
        }

        /// <summary>
        /// Converts a PhoneCarrier to its integer representation
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static explicit operator int(PhoneCarrier p)
        {
            return p.value;
        }
    }
}
