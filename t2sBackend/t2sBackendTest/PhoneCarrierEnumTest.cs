using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sDbLibrary;

namespace t2sBackendTest
{
    [TestClass]
    public class PhoneCarrierEnumTest
    {
        [TestCategory("PhoneCarrierEnum")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void NegativeIntegerCastToPhoneCarrierThrowsException()
        {
            PhoneCarrier phoneCarrier = (PhoneCarrier)(-1);
        }

        [TestCategory("PhoneCarrierEnum")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void VeryLargeIntegerCastToPhoneCarrierThrowsException()
        {
            PhoneCarrier phoneCarrier = (PhoneCarrier)(1000000);
        }

        [TestCategory("PhoneCarrierEnum")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void PhoneCarrierEnumValueOfZeroIsNotDefinedThrowsException()
        {
            PhoneCarrier phoneCarrier = (PhoneCarrier)(0);
        }

        [TestCategory("PhoneCarrierEnum")]
        [TestMethod]
        public void SmallNumberCastToPhoneCarrierReturnsCorrectPhoneCarrierEnumValue()
        {
            PhoneCarrier phoneCarrier = (PhoneCarrier)(1);
            Assert.AreEqual("verizon", phoneCarrier.GetName());
            Assert.AreEqual("vtext.com", phoneCarrier.GetEmail());
            Assert.AreEqual(1, (int)phoneCarrier);
        }

        [TestCategory("PhoneCarrierEnum")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void EmptyStringCastToPhoneCarrierThrowsException()
        {
            PhoneCarrier phoneCarrier = (PhoneCarrier)("");
        }

        [TestCategory("PhoneCarrierEnum")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void InvalidCarrierNameStringThrowsException()
        {
            PhoneCarrier phoneCarrier = (PhoneCarrier)("VERISON");
        }

        [TestCategory("PhoneCarrierEnum")]
        [TestMethod]
        public void ValidCarrierNameStringReturnsCorrectPhoneCarrierEnumValue()
        {
            PhoneCarrier phoneCarrier = (PhoneCarrier)("verizon");
            Assert.AreEqual("verizon", phoneCarrier.GetName());
            Assert.AreEqual("vtext.com", phoneCarrier.GetEmail());
            Assert.AreEqual(1, (int)phoneCarrier);
        }

        [TestCategory("PhoneCarrierEnum")]
        [TestMethod]
        public void ToUpperOfPhoneCarrierNameStillReturnsCorrectPhoneCarrierEnum()
        {
            PhoneCarrier phoneCarrier = (PhoneCarrier)("VeRiZoN");
            Assert.AreEqual("verizon", phoneCarrier.GetName());
            Assert.AreEqual("vtext.com", phoneCarrier.GetEmail());
            Assert.AreEqual(1, (int)phoneCarrier);
        }
    }
}
